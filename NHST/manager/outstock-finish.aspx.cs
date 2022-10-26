using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using MB.Extensions;
using System.Text;
namespace NHST.manager
{
    public partial class outstock_finish : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                else
                {
                    string username_current = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(username_current);
                    if (ac.RoleID != 0 && ac.RoleID != 5)
                        Response.Redirect("/trang-chu");
                }
                LoadData();
            }
        }

        public void LoadData()
        {
            DateTime currentDate = DateTime.Now;
            string username_current = Session["userLoginSystem"].ToString();
            if (Request.QueryString["id"] != null)
            {
                int id = Request.QueryString["id"].ToInt(0);
                if (id > 0)
                {
                    ViewState["id"] = id;
                    var os = OutStockSessionController.GetByID(id);
                    if (os != null)
                    {
                        bool isShowButton = true;
                        double totalPriceMustPay = 0;
                        double TongCanNang = 0;
                        List<OrderPackage> ops = new List<OrderPackage>();
                        #region Đơn hàng mua hộ
                        var listmainorder = OutStockSessionPackageController.GetByOutStockSessionIDGroupByMainOrderID(id);
                        if (listmainorder.Count > 0)
                        {
                            foreach (var m in listmainorder)
                            {
                                var mainorder = MainOrderController.GetAllByID(Convert.ToInt32(m));
                                if (mainorder != null)
                                {
                                    int mID = mainorder.ID;
                                    double totalPay = 0;
                                    OrderPackage op = new OrderPackage();
                                    op.OrderID = Convert.ToInt32(m);
                                    op.OrderType = 1;
                                    List<SmallpackageGet> sms = new List<SmallpackageGet>();
                                    var packsmain = OutStockSessionPackageController.GetAllByOutStockSessionIDAndMainOrderID(id, Convert.ToInt32(m));
                                    if (packsmain.Count > 0)
                                    {
                                        foreach (var p in packsmain)
                                        {
                                            var sm = SmallPackageController.GetByID(Convert.ToInt32(p.SmallPackageID));
                                            if (sm != null)
                                            {
                                                SmallpackageGet pg = new SmallpackageGet();
                                                if (sm.Status == 4)
                                                {
                                                    isShowButton = true;
                                                }
                                                double weight = 0;
                                                double weightCN = Convert.ToDouble(sm.Weight);
                                                double weightKT = 0;
                                                double dai = 0;
                                                double rong = 0;
                                                double cao = 0;
                                                if (sm.Length != null)
                                                    dai = Convert.ToDouble(sm.Length);
                                                if (sm.Width != null)
                                                    rong = Convert.ToDouble(sm.Width);
                                                if (sm.Height != null)
                                                    cao = Convert.ToDouble(sm.Height);

                                                if (dai > 0 && rong > 0 && cao > 0)
                                                    weightKT = dai * rong * cao / 6000;
                                                if (weightKT > 0)
                                                {
                                                    if (weightKT > weightCN)
                                                    {
                                                        weight = weightKT;
                                                    }
                                                    else
                                                    {
                                                        weight = weightCN;
                                                    }
                                                }
                                                else
                                                {
                                                    weight = weightCN;
                                                }
                                                weight = Math.Round(weight, 1);

                                                string packagecode = sm.OrderTransactionCode;
                                                int Status = Convert.ToInt32(sm.Status);
                                                double payInWarehouse = 0;
                                                double UnitPrice = 0;
                                                if (Convert.ToDouble(sm.TotalPrice) > 0)
                                                {
                                                    UnitPrice = Convert.ToDouble(sm.TotalPrice);
                                                }
                                                pg.UnitPrice = UnitPrice;
                                                pg.ID = sm.ID;
                                                pg.weight = sm.Weight ?? 0;
                                                pg.packagecode = packagecode;
                                                pg.Status = Status;
                                                var feeweightinware = InWarehousePriceController.GetAll();
                                                double payperday = 0;
                                                double maxday = 0;
                                                foreach (var item in feeweightinware)
                                                {
                                                    if (item.WeightFrom < weight && weight <= item.WeightTo)
                                                    {
                                                        maxday = Convert.ToDouble(item.MaxDay);
                                                        payperday = Convert.ToDouble(item.PricePay);
                                                        break;
                                                    }
                                                }
                                                double totalDays = 0;
                                                if (sm.DateInLasteWareHouse != null)
                                                {
                                                    DateTime diw = Convert.ToDateTime(sm.DateInLasteWareHouse);
                                                    TimeSpan ts = currentDate.Subtract(diw);
                                                    if (ts.TotalDays > 0)
                                                        totalDays = Math.Floor(ts.TotalDays);
                                                }
                                                double dayin = totalDays - maxday;
                                                if (dayin > 0)
                                                {
                                                    payInWarehouse = dayin * payperday * weight;
                                                }
                                                pg.DateInWare = totalDays;
                                                totalPay += Math.Round(payInWarehouse, 0);
                                                pg.payInWarehouse = payInWarehouse;

                                                //Thêm cân qui đổi
                                                double canQuiDoi = 0;
                                                if (sm.Width != null && sm.Height != null && sm.Length != null)
                                                    canQuiDoi = (sm.Width.Value * sm.Height.Value * sm.Length.Value) / 6000;
                                                pg.ExchangeWeight = Math.Round(canQuiDoi, 2);
                                                if (sm.Weight != null)
                                                {
                                                    if (canQuiDoi > sm.Weight.Value)
                                                    {
                                                        pg.PayWeight += canQuiDoi;
                                                    }
                                                    else
                                                    {
                                                        pg.PayWeight += sm.Weight.Value;
                                                    }
                                                }
                                                pg.PayWeight = Math.Round(pg.PayWeight, 2);
                                                pg.CODTQVND = sm.CODTQVND;
                                                sms.Add(pg);
                                                SmallPackageController.UpdateWarehouseFeeDateOutWarehouse(sm.ID, payInWarehouse, currentDate);
                                                OutStockSessionPackageController.update(p.ID, currentDate, totalDays, payInWarehouse);
                                            }

                                        }
                                    }
                                    totalPay = Math.Round(totalPay, 0);
                                    op.totalPrice = totalPay;
                                    op.smallpackages = sms;
                                    double mustpay = 0;
                                    double TotalPriceVND = 0;
                                    bool isPay = false;
                                    MainOrderController.UpdateFeeWarehouse(mID, totalPay);
                                    var ma = MainOrderController.GetAllByID(mID);
                                    if (ma != null)
                                    {
                                        TotalPriceVND = Math.Round(Convert.ToDouble(ma.TotalPriceVND), 0);
                                        double deposited = Math.Round(Convert.ToDouble(ma.Deposit), 0);
                                        double totalmustpay = Math.Round(TotalPriceVND + totalPay, 0);
                                        double totalleftpay = Math.Round(totalmustpay - deposited, 0);
                                        if (totalmustpay <= deposited)
                                        {
                                            isPay = true;
                                        }
                                        else
                                        {
                                            double totalleft = Math.Round(totalmustpay - deposited, 0);
                                            if (totalleft > 100)
                                            {
                                                MainOrderController.UpdateStatus(mID, Convert.ToInt32(ma.UID), 7);
                                                mustpay = totalleftpay;
                                            }
                                            else
                                            {
                                                isPay = true;
                                            }
                                        }
                                    }
                                    if (isShowButton == true)
                                    {
                                        if (isPay == false)
                                        {
                                            isShowButton = false;
                                        }
                                    }
                                    op.totalMustPay = mustpay;
                                    op.ToTalPriceVND = TotalPriceVND;
                                    op.isPay = isPay;
                                    ops.Add(op);
                                }
                            }
                        }
                        #endregion
                        #region Đơn hàng VC hộ
                        var listtransportation = OutStockSessionPackageController.GetByOutStockSessionIDGroupByTransportationID(id);
                        if (listtransportation.Count > 0)
                        {
                            foreach (var t in listtransportation)
                            {
                                int tID = Convert.ToInt32(t);
                                var tran = TransportationOrderController.GetByID(tID);
                                if (tran != null)
                                {
                                    double totalPay = 0;
                                    OrderPackage op = new OrderPackage();
                                    op.OrderID = tID;
                                    op.OrderType = 2;
                                    List<SmallpackageGet> sms = new List<SmallpackageGet>();
                                    var packsmain = OutStockSessionPackageController.GetAllByOutStockSessionIDAndTransporationID(id, tID);
                                    if (packsmain.Count > 0)
                                    {
                                        foreach (var p in packsmain)
                                        {
                                            var sm = SmallPackageController.GetByID(Convert.ToInt32(p.SmallPackageID));
                                            if (sm != null)
                                            {
                                                SmallpackageGet pg = new SmallpackageGet();
                                                if (sm.Status != 3)
                                                {
                                                    isShowButton = false;
                                                }
                                                double weight = 0;
                                                double weightCN = Convert.ToDouble(sm.Weight);
                                                double weightKT = 0;
                                                double dai = 0;
                                                double rong = 0;
                                                double cao = 0;
                                                if (sm.Length != null)
                                                    dai = Convert.ToDouble(sm.Length);
                                                if (sm.Width != null)
                                                    rong = Convert.ToDouble(sm.Width);
                                                if (sm.Height != null)
                                                    cao = Convert.ToDouble(sm.Height);

                                                if (dai > 0 && rong > 0 && cao > 0)
                                                    weightKT = dai * rong * cao / 6000;
                                                if (weightKT > 0)
                                                {
                                                    if (weightKT > weightCN)
                                                    {
                                                        weight = weightKT;
                                                    }
                                                    else
                                                    {
                                                        weight = weightCN;
                                                    }
                                                }
                                                else
                                                {
                                                    weight = weightCN;
                                                }
                                                weight = Math.Round(weight, 1);
                                                string packagecode = sm.OrderTransactionCode;
                                                int Status = Convert.ToInt32(sm.Status);
                                                double payInWarehouse = 0;
                                                double UnitPrice = 0;
                                                if (Convert.ToDouble(sm.TotalPrice) > 0)
                                                {
                                                    UnitPrice = Convert.ToDouble(sm.TotalPrice);
                                                }
                                                pg.ID = sm.ID;
                                                pg.weight = sm.Weight ?? 0;
                                                pg.UnitPrice = UnitPrice;
                                                pg.packagecode = packagecode;
                                                pg.Status = Status;
                                                var feeweightinware = InWarehousePriceController.GetAll();
                                                double payperday = 0;
                                                double maxday = 0;
                                                foreach (var item in feeweightinware)
                                                {
                                                    if (item.WeightFrom < weight && weight <= item.WeightTo)
                                                    {
                                                        maxday = Convert.ToDouble(item.MaxDay);
                                                        payperday = Convert.ToDouble(item.PricePay);
                                                        break;
                                                    }
                                                }
                                                double totalDays = 0;
                                                if (sm.DateInLasteWareHouse != null)
                                                {
                                                    DateTime diw = Convert.ToDateTime(sm.DateInLasteWareHouse);
                                                    TimeSpan ts = currentDate.Subtract(diw);
                                                    if (ts.TotalDays > 0)
                                                        totalDays = Math.Floor(ts.TotalDays);
                                                }
                                                double dayin = totalDays - maxday;
                                                if (dayin > 0)
                                                {
                                                    payInWarehouse = dayin * payperday * weight;
                                                }
                                                totalPay += Math.Round(payInWarehouse, 0);
                                                pg.DateInWare = totalDays;
                                                pg.payInWarehouse = payInWarehouse;

                                                //Thêm cân qui đổi
                                                double canQuiDoi = 0;
                                                if (sm.Width != null && sm.Height != null && sm.Length != null)
                                                    canQuiDoi = (sm.Width.Value * sm.Height.Value * sm.Length.Value) / 6000;
                                                pg.ExchangeWeight = Math.Round(canQuiDoi, 2);
                                                if (sm.Weight != null)
                                                {
                                                    if (canQuiDoi > sm.Weight.Value)
                                                    {
                                                        pg.PayWeight += canQuiDoi;
                                                    }
                                                    else
                                                    {
                                                        pg.PayWeight += sm.Weight.Value;
                                                    }
                                                }

                                                sms.Add(pg);
                                                SmallPackageController.UpdateWarehouseFeeDateOutWarehouse(sm.ID, payInWarehouse, currentDate);
                                                OutStockSessionPackageController.update(p.ID, currentDate, totalDays, payInWarehouse);
                                            }
                                        }
                                    }
                                    totalPay = Math.Round(totalPay, 0);
                                    op.totalPrice = totalPay;
                                    op.smallpackages = sms;
                                    double mustpay = 0;
                                    double TotalPriceVND = 0;
                                    bool isPay = false;
                                    TransportationOrderController.UpdateWarehouseFee(tID, totalPay);
                                    var tr = TransportationOrderController.GetByID(tID);
                                    if (tr != null)
                                    {
                                        TotalPriceVND = Math.Round(Convert.ToDouble(tr.TotalPrice), 0);
                                        double deposited = Math.Round(Convert.ToDouble(tr.Deposited), 0);
                                        double totalmustpay = Math.Round(TotalPriceVND + totalPay, 0);
                                        double totalleftpay = Math.Round(totalmustpay - deposited, 0);
                                        if (totalmustpay <= deposited)
                                        {
                                            isPay = true;
                                        }
                                        else
                                        {
                                            double totalleft = Math.Round(totalmustpay - deposited, 0);
                                            if (totalleft > 100)
                                            {
                                                TransportationOrderController.UpdateStatus(tID, 5, currentDate, username_current);
                                                mustpay = Math.Round(totalleftpay, 0);
                                            }
                                            else
                                            {
                                                isPay = true;
                                            }

                                        }
                                    }
                                    if (isShowButton == true)
                                    {
                                        if (isPay == false)
                                        {
                                            isShowButton = false;
                                        }
                                    }
                                    op.totalMustPay = Math.Round(mustpay, 0);
                                    op.ToTalPriceVND = TotalPriceVND;
                                    op.isPay = isPay;
                                    ops.Add(op);
                                }
                            }
                        }
                        #endregion
                        #region Render Data
                        txtFullname.Text = os.FullName;
                        txtPhone.Text = os.Phone;
                        if (isShowButton == true)
                        {
                            pnButton.Visible = true;
                        }
                        else
                        {
                            //pnButton.Visible = true;
                            pnrefresh.Visible = true;
                        }
                        StringBuilder html = new StringBuilder();
                        StringBuilder htmlPrint = new StringBuilder();
                        if (ops.Count > 0)
                        {
                            foreach (var o in ops)
                            {
                                int orderType = o.OrderType;
                                bool isPay = o.isPay;
                                string feeShip = "";
                                string codTQ = "";
                                int colspan = 8;
                                string status = "<span class=\"blue-text font-weight-600\">Đã thanh toán</span>";
                                if (o.isPay == false)
                                {
                                    status = "<span class=\"blue-text font-weight-600\">Chưa thanh toán</span>";
                                }

                                #region New
                                html.Append("<div class=\"responsive-tb package-item\">");
                                if (orderType == 1)
                                {
                                    var mainOrder = MainOrderController.GetByID(o.OrderID);

                                    if (isPay == true)
                                        html.Append("<span class=\"owner\">Đơn hàng mua hộ: #" + o.OrderID + "</span>");
                                    else
                                        html.Append("<span class=\"owner\">Đơn hàng mua hộ: #" + o.OrderID + "</span>");
                                    feeShip = $"<td><span>{mainOrder.IsFastDeliveryPrice}</span></td>";
                                }
                                else
                                {
                                    var transOrder = TransportationOrderController.GetByID(o.OrderID);

                                    if (isPay == true)
                                        html.Append("<span class=\"owner\">Đơn hàng vận chuyển hộ: #" + o.OrderID + "</span>");
                                    else
                                        html.Append("<span class=\"owner\">Đơn hàng vận chuyển hộ: #" + o.OrderID + "</span>");
                                    feeShip = $"<td><span>{string.Format("{0:N0}", transOrder.FeeShipHome)}</span></td>";
                                    codTQ = $"<td><span>{transOrder.TotalCODTQVND}</span></td>";
                                    colspan = 9;
                                }

                                html.Append("<table class=\"table bordered \">");
                                html.Append("<thead>");
                                html.Append("<tr class=\"teal darken-4\">");
                                html.Append("<th>Mã kiện</th>");
                                html.Append("<th>Cân nặng (kg)</th>");
                                html.Append("<th>Cân nặng <br> quy đổi (kg)</th>");
                                html.Append("<th>Cân nặng <br> tính tiền (kg)</th>");
                                html.Append("<th>Đơn giá</th>");
                                html.Append("<th>Ship tận nhà</th>");
                                if (!string.IsNullOrEmpty(codTQ))
                                    html.Append("<th>COD Trung Quốc</th>");
                                html.Append("<th>Ngày lưu kho (Ngày)</th>");
                                html.Append("<th>Trạng thái</th>");
                                html.Append("<th>Tiền cân nặng</th>");
                                html.Append("</tr>");
                                html.Append("</thead>");
                                html.Append("<tbody>");

                                var listpackages = o.smallpackages;
                                foreach (var p in listpackages)
                                {
                                    double donGia = 0;
                                    if (p.PayWeight != 0)
                                        donGia = Math.Round(p.UnitPrice / p.PayWeight, 0);
                                    html.Append("<tr>");
                                    html.Append("<td><span>" + p.packagecode + "</span></td>");
                                    html.Append("<td><span>" + p.weight + "</span></td>");
                                    html.Append("<td><span>" + p.ExchangeWeight + "</span></td>");
                                    html.Append("<td><span>" + p.PayWeight + "</span></td>");
                                    html.Append("<td><span>" + string.Format("{0:N0}", donGia) + "</span></td>");
                                    html.Append(feeShip);
                                    if (!string.IsNullOrEmpty(codTQ))
                                        html.Append(codTQ);
                                    html.Append("<td><span>" + p.DateInWare + "</span></td>");
                                    html.Append("<td>" + PJUtils.IntToStringStatusSmallPackageNew(p.Status) + "</td>");
                                    html.Append("<td>" + string.Format("{0:N0}", p.UnitPrice) + " VNĐ</td>");
                                    html.Append("</tr>");
                                    TongCanNang += Convert.ToDouble(p.PayWeight);
                                }

                                html.Append("</tbody>");
                                html.Append("<tbody>");
                                html.Append("<tr>");
                                html.Append($"<td colspan=\"{colspan}\"><span class=\"black-text font-weight-500\">Tổng tiền lưu kho</span></td>");
                                html.Append("<td><span class=\"black-text font-weight-600\">" + string.Format("{0:N0}", o.totalPrice) + " VNĐ</span></td>");
                                html.Append("</tr>");
                                html.Append("<tr>");
                                html.Append($"<td colspan=\"{colspan}\"><span class=\"black-text font-weight-500\">Trạng thái</span></td>");
                                html.Append("<td>" + status + "</td>");
                                html.Append("</tr>");
                                html.Append("<tr>");
                                html.Append($"<td colspan=\"{colspan}\"><span class=\"black-text font-weight-500\">Tiền cần thanh toán</span></td>");
                                html.Append("<td><span class=\"red-text font-weight-700\">" + string.Format("{0:N0}", o.totalMustPay) + " VNĐ</span></td>");
                                html.Append("</tr>");
                                html.Append("<tr>");
                                html.Append($"<td colspan=\"{colspan}\"><span class=\"black-text font-weight-500\">Tổng tiền đơn hàng</span></td>");
                                html.Append("<td><span class=\"blue-text font-weight-700\">" + string.Format("{0:N0}", o.ToTalPriceVND) + " VNĐ</span></td>");
                                html.Append("</tr>");
                                html.Append("</tbody>");
                                html.Append("</table>");
                                html.Append("</div>");
                                #endregion

                                totalPriceMustPay += o.totalMustPay;

                                htmlPrint.Append("<article class=\"pane-primary\" style=\"color:#000\">");
                                if (orderType == 1)
                                {
                                    htmlPrint.Append("   <div class=\"heading\"><h3 class=\"lb\" style=\"color:#000\">Đơn hàng mua hộ: <span style=\"text-align:right\">#" + o.OrderID + "</span></h3></div>");
                                }
                                else
                                {
                                    htmlPrint.Append("   <div class=\"heading\"><h3 class=\"lb\" style=\"color:#000\">Đơn hàng VC hộ: <span style=\"text-align:right\">#" + o.OrderID + "</span></h3></div>");
                                }

                                htmlPrint.Append("   <article class=\"pane-primary\">");
                                htmlPrint.Append("       <table class=\"rgMasterTable normal-table full-width\" style=\"text-align:center\">");
                                htmlPrint.Append("           <tr>");
                                htmlPrint.Append("               <th style=\"color:#000\">Mã kiện</th>");
                                htmlPrint.Append("               <th style=\"color:#000\">Cân nặng (kg)</th>");
                                htmlPrint.Append("               <th style=\"color:#000\">Cân nặng <br> quy đổi (kg)</th>");
                                htmlPrint.Append("               <th style=\"color:#000\">Cân nặng <br> tính tiền (kg)</th>");
                                htmlPrint.Append("               <th style=\"color:#000\">Đơn giá</th>");
                                htmlPrint.Append("<th style=\"color:#000\">Ship tận nhà</th>");
                                if (!string.IsNullOrEmpty(codTQ))
                                    htmlPrint.Append("<th style=\"color:#000\">COD Trung Quốc</th>");
                                htmlPrint.Append("               <th style=\"color:#000\">Ngày lưu kho (ngày)</th>");
                                htmlPrint.Append("               <th style=\"color:#000\">Tiền cân nặng</th>");
                                htmlPrint.Append("           </tr>");

                                foreach (var p in listpackages)
                                {
                                    double donGia = 0;
                                    if (p.PayWeight != 0)
                                        donGia = Math.Round(p.UnitPrice / p.PayWeight, 0);
                                    htmlPrint.Append("           <tr>");
                                    htmlPrint.Append("               <td>" + p.packagecode + "</td>");
                                    htmlPrint.Append("               <td>" + p.weight + "</td>");
                                    htmlPrint.Append("               <td>" + p.ExchangeWeight + "</td>");
                                    htmlPrint.Append("               <td>" + p.PayWeight + "</td>");
                                    htmlPrint.Append("               <td>" + string.Format("{0:N0}", donGia) + "</td>");
                                    htmlPrint.Append(feeShip);
                                    if (!string.IsNullOrEmpty(codTQ))
                                        htmlPrint.Append(codTQ);
                                    htmlPrint.Append("               <td>" + p.DateInWare + "</td>");
                                    htmlPrint.Append("               <td>" + string.Format("{0:N0}", p.UnitPrice) + " vnđ</td>");
                                    htmlPrint.Append("           </tr>");
                                }

                                htmlPrint.Append("           <tr style=\"font-size: 15px; text-transform: uppercase\">");
                                htmlPrint.Append($"               <td colspan=\"{colspan - 1}\" class=\"text-align-right\">Tổng tiền cần thanh toán</td>");
                                htmlPrint.Append("               <td>" + string.Format("{0:N0}", o.totalPrice) + " vnđ</td>");
                                htmlPrint.Append("           </tr>");

                                htmlPrint.Append("<tr style=\"font-size: 15px; text-transform: uppercase\">");
                                htmlPrint.Append($"<td colspan=\"{colspan - 1}\"><span class=\"text-align-right\">Tổng tiền đơn hàng</span></td>");
                                htmlPrint.Append("<td><span class=\"blue-text font-weight-700\">" + string.Format("{0:N0}", o.ToTalPriceVND) + " VNĐ</span></td>");
                                htmlPrint.Append("</tr>");
                                htmlPrint.Append("       </table>");
                                htmlPrint.Append("   </article>");
                                htmlPrint.Append("</article>");

                            }
                            ltrList.Text = html.ToString();
                            ViewState["content"] = htmlPrint.ToString();
                        }
                        #endregion
                        if (totalPriceMustPay > 0)
                        {
                            OutStockSessionController.updateTotalPay(id, totalPriceMustPay);
                        }
                        ltrTotalNotPay.Text = string.Format("{0:N0}", totalPriceMustPay);
                        ltrTotalWeight.Text = TongCanNang.ToString();
                    }
                }
            }
        }


        [WebMethod]
        public static string Delete(string PackageID)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var user = AccountController.GetByUsername(username);
                if (user != null)
                {
                    int userRole = Convert.ToInt32(user.RoleID);

                    if (userRole == 0 || userRole == 2 || userRole == 5)
                    {
                        var check = SmallPackageController.GetByID(Convert.ToInt32(PackageID));
                        if (check != null)
                        {
                            SmallPackageController.Delete(check.ID);
                            return "ok";
                        }
                        else return "null";
                    }
                    else return "null";
                }
                else return "null";
            }
            else return "null";
        }

        protected void btnHidden_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username_current = Session["userLoginSystem"].ToString();
            int id = ViewState["id"].ToString().ToInt(0);
            if (id > 0)
            {
                int UID = 0;
                var outs = OutStockSessionController.GetByID(id);
                if (outs != null)
                    UID = Convert.ToInt32(outs.UID);

                var sessionpack = OutStockSessionPackageController.GetAllByOutStockSessionID(id);
                if (sessionpack.Count > 0)
                {
                    List<Main> mo = new List<Main>();
                    foreach (var item in sessionpack)
                    {
                        if (item.MainOrderID > 0)
                        {
                            bool check = mo.Any(x => x.MainOrderID == Convert.ToInt32(item.MainOrderID));
                            if (check != true)
                            {
                                Main m = new Main();
                                m.MainOrderID = Convert.ToInt32(item.MainOrderID);
                                mo.Add(m);
                            }
                        }
                    }
                }
            }
        }

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username_current = Session["userLoginSystem"].ToString();
            int id = ViewState["id"].ToString().ToInt(0);
            if (id > 0)
            {
                int UID = 0;
                var outs = OutStockSessionController.GetByID(id);
                if (outs != null)
                    UID = Convert.ToInt32(outs.UID);

                OutStockSessionController.update(id, txtFullname.Text, txtPhone.Text, 2, currentDate, username_current);
                var sessionpack = OutStockSessionPackageController.GetAllByOutStockSessionID(id);
                if (sessionpack.Count > 0)
                {
                    List<Main> mo = new List<Main>();
                    List<Trans> to = new List<Trans>();
                    foreach (var item in sessionpack)
                    {
                        SmallPackageController.UpdateStatus(Convert.ToInt32(item.SmallPackageID), 4, currentDate, username_current);
                        SmallPackageController.UpdateDateOutWarehouse(Convert.ToInt32(item.SmallPackageID), username_current, currentDate);

                        if (item.MainOrderID > 0)
                        {
                            bool check = mo.Any(x => x.MainOrderID == Convert.ToInt32(item.MainOrderID));
                            if (check != true)
                            {
                                Main m = new Main();
                                m.MainOrderID = Convert.ToInt32(item.MainOrderID);
                                mo.Add(m);
                            }
                        }
                        else
                        {
                            bool check = to.Any(x => x.TransportationOrderID == Convert.ToInt32(item.TransportationID));
                            if (check != true)
                            {
                                Trans t = new Trans();
                                t.TransportationOrderID = Convert.ToInt32(item.TransportationID);
                                to.Add(t);
                            }
                        }
                    }

                    double PercentOrder = 0;
                    double PercentTran = 0;
                    var user = AccountController.GetByID(Convert.ToInt32(UID));
                    var confi = ConfigurationController.GetByTop1();
                    PercentOrder = Convert.ToDouble(confi.XuMuaHo);
                    PercentTran = Convert.ToDouble(confi.XuVanChuyen);

                    if (mo.Count > 0)
                    {
                        foreach (var item in mo)
                        {
                            var m = MainOrderController.GetAllByID(item.MainOrderID);
                            if (m != null)
                            {
                                MainOrderController.UpdateStatus(item.MainOrderID, Convert.ToInt32(m.UID), 10);
                                if (m.CompleteDate == null)
                                {
                                    MainOrderController.UpdateCompleteDate(m.ID, currentDate);
                                }
                                double TotalPrice = 0;
                                double FeeBuyPro = 0;
                                if (!string.IsNullOrEmpty(m.FeeBuyPro))
                                {
                                    FeeBuyPro = Convert.ToDouble(m.FeeBuyPro);
                                }
                                TotalPrice = Math.Round(FeeBuyPro * PercentOrder / 100, 0);

                                if (!string.IsNullOrEmpty(user.GioiThieuID))
                                {
                                    double XuCurrent = 0;
                                    double XuWallet = 0;
                                    var c = AccountController.GetByMaGioiThieu(user.GioiThieuID);
                                    if (c != null)
                                    {
                                        if (c.MaGioiThieu == user.GioiThieuID)
                                        {
                                            if (outs.IsOutStockOrder != true)
                                            {
                                                HistoryIntroduceController.Insert(c.ID, c.Username, TotalPrice.ToString(), "Bạn nhận được xu từ đơn mua hộ mã là " + m.ID + " của tài khoản " + user.Username + " là: " + string.Format("{0:N0}", TotalPrice) + " xu", TotalPrice.ToString(), 1, currentDate, username_current);

                                                if (!string.IsNullOrEmpty(c.XuTichLuy))
                                                {
                                                    XuCurrent = Convert.ToDouble(c.XuTichLuy);
                                                }
                                                XuWallet = XuCurrent + TotalPrice;
                                                AccountController.UpdateXu(c.ID, XuWallet.ToString(), currentDate, username_current);
                                                OutStockSessionController.UpdateCheckXuatKhoOrder(outs.ID, true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (to.Count > 0)
                    {
                        foreach (var item in to)
                        {
                            var ts = TransportationOrderController.GetByID(item.TransportationOrderID);
                            if (ts != null)
                            {
                                TransportationOrderController.UpdateStatus(ts.ID, 7, DateTime.Now, username_current);

                                double TotalPrice = 0;
                                double FeeWeight = 0;
                                if (Convert.ToDouble(ts.FeeWeight) > 0)
                                {
                                    FeeWeight = Convert.ToDouble(ts.FeeWeight);
                                }
                                TotalPrice = Math.Round(FeeWeight * PercentTran / 100, 0);

                                if (!string.IsNullOrEmpty(user.GioiThieuID))
                                {
                                    double XuCurrent = 0;
                                    double XuWallet = 0;
                                    var c = AccountController.GetByMaGioiThieu(user.GioiThieuID);
                                    if (c != null)
                                    {
                                        if (c.MaGioiThieu == user.GioiThieuID)
                                        {
                                            if (outs.IsOutStockOrder != true)
                                            {
                                                HistoryIntroduceController.Insert(c.ID, c.Username, TotalPrice.ToString(), "Bạn nhận được xu từ đơn vận chuyển hộ mã là " + ts.ID + " của tài khoản " + user.Username + " là: " + string.Format("{0:N0}", TotalPrice) + " xu", TotalPrice.ToString(), 1, currentDate, username_current);

                                                if (!string.IsNullOrEmpty(c.XuTichLuy))
                                                {
                                                    XuCurrent = Convert.ToDouble(c.XuTichLuy);
                                                }
                                                XuWallet = XuCurrent + TotalPrice;
                                                AccountController.UpdateXu(c.ID, XuWallet.ToString(), currentDate, username_current);
                                                OutStockSessionController.UpdateCheckXuatKhoTrans(outs.ID, true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string content = ViewState["content"].ToString();
                var html = "";
                html += "<div class=\"print-bill\">";
                html += "   <div class=\"top\">";
                html += "       <div class=\"left\">";
                html += "          <span class=\"company-info\">NHẬP HÀNG TIẾT KIỆM</span>";
                html += "          <span class=\"company-info\">Hà Nội</span>";
                html += "       </div>";
                html += "       <div class=\"right\">";
                html += "           <span class=\"bill-num\">Mẫu số 01 - TT</span>";
                html += "           <span class=\"bill-promulgate-date\">(Ban hành theo Thông tư số 133/2016/TT-BTC ngày 26/8/2016 của Bộ Tài chính)</span>";
                html += "       </div>";
                html += "   </div>";
                html += "   <div class=\"bill-title\">";
                html += "       <h1>PHIẾU XUẤT KHO</h1>";
                html += "       <span class=\"bill-date\">" + string.Format("{0:dd/MM/yyyy HH:mm}", currentDate) + " </span>";
                html += "   </div>";
                html += "   <div class=\"bill-content\">";
                html += "       <div class=\"bill-row\">";
                html += "           <label class=\"row-name\">Họ và tên người đến nhận: </label>";
                html += "           <label class=\"row-info\">" + txtFullname.Text + "</label>";
                html += "       </div>";
                html += "       <div class=\"bill-row\">";
                html += "           <label class=\"row-name\">Số ĐT người đến nhận: </label>";
                html += "           <label class=\"row-info\">" + txtPhone.Text + "</label>";
                html += "       </div>";
                html += "       <div class=\"bill-row\" style=\"border:none\">";
                html += "           <label class=\"row-name\">Danh sách kiện: </label>";
                html += "           <label class=\"row-info\"></label>";
                html += "       </div>";
                html += "       <div class=\"bill-row\" style=\"border:none\">";
                html += content;
                html += "       </div>";
                html += "   </div>";
                html += "   <div class=\"bill-footer\">";
                html += "       <div class=\"bill-row-two\">";
                html += "           <strong>Người xuất hàng</strong>";
                html += "           <span class=\"note\">(Ký, họ tên)</span>";
                html += "       </div>";
                html += "       <div class=\"bill-row-two\">";
                html += "           <strong>Người nhận hàng</strong>";
                html += "           <span class=\"note\">(Ký, họ tên)</span>";
                html += "           <span class=\"note\" style=\"margin-top:100px;\">" + txtFullname.Text + "</span>";
                html += "       </div>";
                html += "   </div>";
                html += "</div>";

                StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script language='javascript'>");
                sb.Append(@"VoucherPrint('" + html + "')");
                sb.Append(@"</script>");

                ///hàm để đăng ký javascript và thực thi đoạn script trên
                if (!ClientScript.IsStartupScriptRegistered("JSScript"))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "JSScript", sb.ToString());

                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                id = int.Parse(Request.QueryString["id"]);
            }

            if (id > 0)
            {
                var outStock = OutStockSessionController.GetByID(id);
                var outStockPackages = OutStockSessionPackageController.GetAllByOutStockSessionID(id);
                StringBuilder StrExport = new StringBuilder();
                StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
                StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
                StrExport.Append("<DIV  style='font-size:12px;'>");
                StrExport.Append("<table border=\"1\">");
                StrExport.Append("  <tr>");
                StrExport.Append("      <th><strong>ID</strong></th>");
                StrExport.Append("      <th><strong>Nhân viên</strong></th>");
                StrExport.Append("      <th><strong>Họ tên khách</strong></th>");
                StrExport.Append("      <th><strong>Điện thoại</strong></th>");
                StrExport.Append("      <th><strong>Mã kiện</strong></th>");
                StrExport.Append("      <th><strong>Cân tính tiền (kg)</strong></th>");
                StrExport.Append("      <th><strong>Ngày lưu kho</strong></th>");
                StrExport.Append("      <th><strong>Tiền Ship tận nhà</strong></th>");
                StrExport.Append("      <th><strong>Tiền cân nặng</strong></th>");
                StrExport.Append("      <th><strong>Tiền đã trả</strong></th>");
                StrExport.Append("      <th><strong>Tiền cần thanh toán</strong></th>");
                StrExport.Append("      <th><strong>Tổng tiền đơn hàng</strong></th>");
                StrExport.Append("      <th><strong>Trạng thái</strong></th>");
                StrExport.Append("      <th><strong>Ngày tạo</strong></th>");
                StrExport.Append("  </tr>");
                foreach (var item in outStockPackages)
                {
                    var sm = SmallPackageController.GetByID(item.SmallPackageID ?? 0);
                    double canQuiDoi = 0;
                    if (sm.Width != null && sm.Height != null && sm.Length != null)
                        canQuiDoi = (sm.Width.Value * sm.Height.Value * sm.Length.Value) / 6000;
                    double canTinhTien = 0;
                    if (sm.Weight != null)
                    {
                        if (canQuiDoi > sm.Weight.Value)
                        {
                            canTinhTien += canQuiDoi;
                        }
                        else
                        {
                            canTinhTien += sm.Weight.Value;
                        }
                    }
                    var mainOrder = MainOrderController.GetByID(sm.MainOrderID ?? 0);
                    var transOrder = TransportationOrderController.GetByID(sm.TransportationOrderID ?? 0);
                    var tongTienDonHang = mainOrder != null ? Convert.ToDouble(mainOrder.TotalPriceVND) : transOrder.TotalPrice;
                    var shipTanNha = mainOrder != null ? Convert.ToDouble(mainOrder.IsFastDeliveryPrice) : transOrder.FeeShipHome;
                    StrExport.Append("  <tr>");
                    StrExport.Append("      <td>" + item.ID + "</td>");
                    StrExport.Append("      <td>" + outStock.Username + "</td>");
                    StrExport.Append("      <td>" + outStock.FullName + "</td>");
                    StrExport.Append("      <td>" + outStock.Phone + "</td>");
                    StrExport.Append("      <td>" + item.OrderTransactionCode + "</td>");
                    StrExport.Append("      <td>" + Convert.ToDouble(canTinhTien) + "</td>");
                    StrExport.Append("      <td>" + item.DayInWarehouse + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(shipTanNha)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(sm.TotalPrice)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(tongTienDonHang) - outStock.TotalPay) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(outStock.TotalPay)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(tongTienDonHang)) + "</td>");
                    StrExport.Append("      <td>" + PJUtils.IntToStringStatusSmallPackageNew(Convert.ToInt32(sm.Status)) + "</td>");
                    StrExport.Append("      <td>" + item.CreatedDate + "</td>");
                    StrExport.Append("  </tr>");
                }
                StrExport.Append("</table>");
                StrExport.Append("</div></body></html>");
                string strFile = "ExcelReportOutStockList.xls";
                string strcontentType = "application/vnd.ms-excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.Write(StrExport.ToString());
                Response.Flush();
                //Response.Close();
                Response.End();
            }
        }

        public class OrderPackage
        {
            public int OrderID { get; set; }
            public int OrderType { get; set; }
            public List<SmallpackageGet> smallpackages { get; set; }
            public double totalPrice { get; set; }
            public bool isPay { get; set; }
            public double totalMustPay { get; set; }
            public double UnitPrice { get; set; }
            public double ToTalPriceVND { get; set; }
        }
        public class SmallpackageGet
        {
            public int ID { get; set; }
            public string packagecode { get; set; }
            public double UnitPrice { get; set; }
            public double weight { get; set; }
            public double DateInWare { get; set; }
            public int Status { get; set; }
            public double payInWarehouse { get; set; }
            public double ExchangeWeight { get; set; }
            public double PayWeight { get; set; }
            public string CODTQVND { get; set; }
            public string TransportFee { get; set; }
        }
        public class Main
        {
            public int MainOrderID { get; set; }
        }
        public class Trans
        {
            public int TransportationOrderID { get; set; }
        }
    }
}