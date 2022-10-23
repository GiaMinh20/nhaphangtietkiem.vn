using NHST.Models;
using NHST.Controllers;
using System;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Net;
using Supremes;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NHST.Bussiness;
using MB.Extensions;
using Telerik.Web.UI;
using Microsoft.AspNet.SignalR;
using NHST.Hubs;
using System.Web.Script.Serialization;
namespace NHST.manager
{
    public partial class transportationdetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session["userLoginSystem"] = "xuemei912";
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                else
                {
                    string username_current = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(username_current);
                    int RoleID = Convert.ToInt32(ac.RoleID);
                    if (ac.RoleID == 1)
                        Response.Redirect("/trang-chu");
                    else
                    {
                        if (RoleID == 8)
                        {
                            Response.Redirect("/admin/home.aspx");
                        }
                    }
                }
                LoadDDL();
                loaddata();
            }
        }
        public void LoadDDL()
        {

            var warehousefrom = WarehouseFromController.GetAllWithIsHidden(false);
            if (warehousefrom.Count > 0)
            {
                foreach (var item in warehousefrom)
                {
                    ListItem a = new ListItem(item.WareHouseName, item.ID.ToString());
                    ddlOrderWareHouseFrom.Items.Add(a);
                }
                ddlOrderWareHouseFrom.DataBind();
            }


            var warehouseto = WarehouseController.GetAllWithIsHidden(false);
            if (warehouseto.Count > 0)
            {
                foreach (var item in warehouseto)
                {
                    ListItem a = new ListItem(item.WareHouseName, item.ID.ToString());
                    ddlOrderWareHouseTo.Items.Add(a);
                }
                ddlOrderWareHouseTo.DataBind();
            }

            var shippingtype = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shippingtype.Count > 0)
            {
                foreach (var item in shippingtype)
                {
                    ListItem a = new ListItem(item.ShippingTypeName, item.ID.ToString());
                    ddlOrderShippingType.Items.Add(a);
                }

                ddlOrderShippingType.DataBind();
            }
        }

        public void loaddata()
        {
            var config = ConfigurationController.GetByTop1();
            double currency = 0;
            if (config != null)
            {
                currency = Convert.ToDouble(config.Currency);
            }
            var id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                ViewState["ID"] = id;
                var t = TransportationOrderController.GetByID(id);
                if (t != null)
                {
                    int tID = t.ID;
                    #region lấy dữ liệu
                    int status = Convert.ToInt32(t.Status);
                    int warehouseFrom = Convert.ToInt32(t.WarehouseFromID);
                    int warehouseTo = Convert.ToInt32(t.WarehouseID);
                    int shippingType = Convert.ToInt32(t.ShippingTypeID);
                    double currency_addOrder = Convert.ToDouble(t.Currency);

                    double FeeWeight = 0;
                    if (t.FeeWeight != null)
                    {
                        FeeWeight = Math.Round(Convert.ToDouble(t.FeeWeight), 0);
                    }

                    double UnitPrice = 0;
                    if (t.FeeWeight != null)
                    {
                        UnitPrice = Math.Round(Convert.ToDouble(t.UnitPrice), 0);
                    }

                    double FeeHome = 0;
                    if (t.FeeShipHome != null)
                    {
                        FeeHome = Math.Round(Convert.ToDouble(t.FeeShipHome), 0);
                    }

                    double checkProductFee = 0;
                    if (t.CheckProductFee != null)
                    {
                        checkProductFee = Math.Round(Convert.ToDouble(t.CheckProductFee), 0);
                    }

                    double packagedFee = 0;
                    if (t.PackagedFee != null)
                    {
                        packagedFee = Math.Round(Convert.ToDouble(t.PackagedFee), 0);
                    }

                    double insurranceFee = 0;
                    if (t.InsurranceFee != null)
                    {
                        insurranceFee = Math.Round(Convert.ToDouble(t.InsurranceFee), 0);
                    }

                    double totalCodeTQCYN = 0;
                    if (t.TotalCODTQCYN != null)
                    {
                        totalCodeTQCYN = Math.Round(Convert.ToDouble(t.TotalCODTQCYN), 0);
                    }

                    double totalCODTQVND = 0;
                    if (t.TotalCODTQVND != null)
                    {
                        totalCODTQVND = Math.Round(Convert.ToDouble(t.TotalCODTQVND), 0);
                    }

                    double totalPriceVND = Math.Round(Convert.ToDouble(t.TotalPrice), 0);
                    double totalPriceCYN = 0;
                    if (totalPriceVND > 0)
                        totalPriceCYN = Math.Round(totalPriceVND / currency_addOrder, 2);
                    ltrOrderID.Text = id.ToString();
                    double deposited = Math.Round(Convert.ToDouble(t.Deposited), 0);
                    //double totalPackage = 0;
                    double totalWeight = 0;

                    StringBuilder htmlPackages = new StringBuilder();
                    var smallpackages = SmallPackageController.GetByTransportationOrderID(tID);
                    if (smallpackages.Count > 0)
                    {
                        //totalPackage = smallpackages.Count;
                        foreach (var p in smallpackages)
                        {
                            double weight = 0;
                            double weightCN = Math.Round(Convert.ToDouble(p.Weight), 1);
                            double volume = Math.Round(Convert.ToDouble(p.Volume), 2);
                            double weightKT = 0;
                            double dai = 0;
                            double rong = 0;
                            double cao = 0;
                            if (p.Length != null)
                                dai = Convert.ToDouble(p.Length);
                            if (p.Width != null)
                                rong = Convert.ToDouble(p.Width);
                            if (p.Height != null)
                                cao = Convert.ToDouble(p.Height);

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
                            htmlPackages.Append("<tr class=\"package-item\" data-code=\"" + p.OrderTransactionCode + "\" data-weight=\"" + weight + "\" data-package-id=\"" + p.ID + "\">");
                            htmlPackages.Append("   <td>" + p.ID + "</td>");
                            htmlPackages.Append("   <td>" + p.OrderTransactionCode + "</td>");
                            htmlPackages.Append("   <td>" + p.ProductType + "</td>");
                            htmlPackages.Append("   <td>" + p.ProductQuantity + "</td>");
                            htmlPackages.Append("   <td>" + weight + " kg</td>");
                            //htmlPackages.Append("   <td>" + volume + " m3</td>");

                            bool isCheckProduct = false;
                            bool isPackaged = false;
                            bool isInsurrance = false;

                            if (p.IsCheckProduct != null)
                            {
                                isCheckProduct = Convert.ToBoolean(p.IsCheckProduct);
                            }
                            if (p.IsPackaged != null)
                            {
                                isPackaged = Convert.ToBoolean(p.IsPackaged);
                            }
                            if (p.IsInsurrance != null)
                            {
                                isInsurrance = Convert.ToBoolean(p.IsInsurrance);
                            }
                            if (isCheckProduct == true)
                            {
                                htmlPackages.Append("   <td><label><input type = \"checkbox\" checked disabled /><span ></span ></label ></td>");
                            }
                            else
                            {
                                htmlPackages.Append("   <td><label><input type = \"checkbox\" disabled /><span ></span ></label ></td>");
                            }
                            if (isPackaged == true)
                            {
                                htmlPackages.Append("   <td><label><input type = \"checkbox\" checked disabled /><span ></span ></label ></td>");
                            }
                            else
                            {
                                htmlPackages.Append("   <td><label><input type = \"checkbox\" disabled /><span ></span ></label ></td>");
                            }
                            if (isInsurrance == true)
                            {
                                htmlPackages.Append("   <td><label><input type = \"checkbox\" checked disabled /><span ></span ></label ></td>");
                            }
                            else
                            {
                                htmlPackages.Append("   <td><label><input type = \"checkbox\" disabled /><span ></span ></label ></td>");
                            }

                            htmlPackages.Append("   <td>¥" + p.CODTQCYN + " - " + string.Format("{0:N0}", Convert.ToDouble(p.CODTQVND)) + " VNĐ</td>");
                            htmlPackages.Append("   <td>" + p.UserNote + "</td>");

                            if (status == 1)
                                htmlPackages.Append("   <td><span class=\"badge red darken-2 white-text border-radius-2\">Chưa duyệt</span></td>");
                            else
                                htmlPackages.Append("   <td>" + PJUtils.IntToStringStatusSmallPackageNew(Convert.ToInt32(p.Status)) + "</td>");

                            htmlPackages.Append("</tr>");
                            totalWeight += weight;
                        }
                    }
                    else
                    {
                        var transportationDetail = TransportationOrderDetailController.GetByTransportationOrderID(tID);
                        if (transportationDetail.Count > 0)
                        {
                            //totalPackage = transportationDetail.Count;
                            foreach (var p in transportationDetail)
                            {
                                double weight = Math.Round(Convert.ToDouble(p.Weight), 1);
                                htmlPackages.Append("<tr class=\"package-item\" data-code=\"" + p.TransportationOrderCode + "\" data-weight=\"" + weight + "\" data-package-id=\"" + p.ID + "\">");
                                htmlPackages.Append("   <td>" + p.ID + "</td>");
                                htmlPackages.Append("   <td><div class=\"product-infobox\">" + p.TransportationOrderCode + "</div></td>");
                                htmlPackages.Append("   <td>" + p.ProductType + "</td>");
                                htmlPackages.Append("   <td>" + p.ProductQuantity + "</td>");
                                htmlPackages.Append("   <td>" + p.Weight + " kg</td>");
                                //htmlPackages.Append("   <td>" + p.Volume + " m3</td>");

                                bool isCheckProduct = false;
                                bool isPackaged = false;
                                bool isInsurrance = false;

                                if (p.IsCheckProduct != null)
                                {
                                    isCheckProduct = Convert.ToBoolean(p.IsCheckProduct);
                                }
                                if (p.IsPackaged != null)
                                {
                                    isPackaged = Convert.ToBoolean(p.IsPackaged);
                                }
                                if (p.IsInsurrance != null)
                                {
                                    isInsurrance = Convert.ToBoolean(p.IsInsurrance);
                                }
                                if (isCheckProduct == true)
                                {
                                    htmlPackages.Append("   <td><label><input type = \"checkbox\" checked disabled /><span ></span ></label ></td>");
                                }
                                else
                                {
                                    htmlPackages.Append("   <td><label><input type = \"checkbox\" disabled /><span ></span ></label ></td>");
                                }
                                if (isPackaged == true)
                                {
                                    htmlPackages.Append("   <td><label><input type = \"checkbox\" checked disabled /><span ></span ></label ></td>");
                                }
                                else
                                {
                                    htmlPackages.Append("   <td><label><input type = \"checkbox\" disabled /><span ></span ></label ></td>");
                                }
                                if (isInsurrance == true)
                                {
                                    htmlPackages.Append("   <td><label><input type = \"checkbox\" checked disabled /><span ></span ></label ></td>");
                                }
                                else
                                {
                                    htmlPackages.Append("   <td><label><input type = \"checkbox\" disabled /><span ></span ></label ></td>");
                                }
                                htmlPackages.Append("   <td>¥" + p.CODTQCYN + " - " + string.Format("{0:N0}", Convert.ToDouble(p.CODTQVND)) + " VNĐ</td>");
                                htmlPackages.Append("   <td>" + p.UserNote + "</td>");
                                htmlPackages.Append("   <td><span class=\"badge red darken-2 white-text border-radius-2\">Chưa duyệt</span></td>");
                                htmlPackages.Append("</tr>");
                                totalWeight += weight;
                            }
                        }
                    }
                    ltrPackages.Text = htmlPackages.ToString();
                    #endregion

                    #region gán dữ liệu
                    hdfCurrency.Value = currency_addOrder.ToString();
                    hdfStatus.Value = status.ToString();
                    txtTotalPriceVND.Text = string.Format("{0:N0}", Math.Round(totalPriceVND, 0));
                    OrderTotalPrice.Text = string.Format("{0:N0} VNĐ", Math.Round(totalPriceVND, 0));
                    txtTotalPriceCNY.Text = Math.Round(totalPriceCYN, 2).ToString();
                    OrderPaidPrice.Text = string.Format("{0:N0} VNĐ", Math.Round(deposited, 0));
                    OrderRemainingPrice.Text = string.Format("{0:N0} VNĐ", Math.Round(totalPriceVND - deposited, 0));
                    ddlOrderWareHouseFrom.SelectedValue = warehouseFrom.ToString();
                    ddlOrderWareHouseTo.SelectedValue = warehouseTo.ToString();
                    ddlOrderShippingType.SelectedValue = shippingType.ToString();
                    ddlOrderStatus.SelectedValue = status.ToString();
                    //txtMainOrderCode.Text = MainOrderCode;                   
                    txtTotalWeight.Text = Math.Round(totalWeight, 2).ToString();
                    txtFeeWeight.Text = string.Format("{0:N0}", Math.Round(FeeWeight, 0));
                    txtUnitPrice.Text = UnitPrice.ToString();
                    txtFeeCheckPackage.Text = string.Format("{0:N0}", Math.Round(checkProductFee, 0));
                    txtFeePack.Text = string.Format("{0:N0}", Math.Round(packagedFee, 0));
                    txtFeeInsurrance.Text = string.Format("{0:N0}", Math.Round(insurranceFee, 0));
                    txtTotalCODCNY.Text = totalCodeTQCYN.ToString();
                    txtTotalCODVN.Text = string.Format("{0:N0}", Math.Round(totalCODTQVND, 0));
                    lblNote.Text = t.Description;
                    txtStaffNote.Text = t.StaffNote;
                    txtFeeHome.Text = string.Format("{0:N0}", Math.Round(FeeHome, 0));
                    #endregion
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (ac != null)
            {
                if (ac.RoleID == 0 || ac.RoleID == 2 || ac.RoleID == 6 || ac.RoleID == 4 || ac.RoleID == 5)
                {
                    var id = Convert.ToInt32(ViewState["ID"]);
                    if (id > 0)
                    {
                        var t = TransportationOrderController.GetByID(id);
                        if (t != null)
                        {
                            int tID = t.ID;
                            string Username = t.Username;
                            int UID = Convert.ToInt32(t.UID);
                            int warehouseFrom = ddlOrderWareHouseFrom.SelectedValue.ToInt();
                            int warehouseTo = ddlOrderWareHouseTo.SelectedValue.ToInt();
                            int shippingType = ddlOrderShippingType.SelectedValue.ToInt();
                            int status = ddlOrderStatus.SelectedValue.ToInt();

                            var smallpacs = SmallPackageController.GetByTransportationOrderID(tID);
                            if (status == 0)
                            {
                                //var smallpacs = SmallPackageController.GetByTransportationOrderID(tID);
                                if (smallpacs.Count > 0)
                                {
                                    foreach (var item in smallpacs)
                                    {
                                        SmallPackageController.Delete(item.ID);
                                    }
                                }
                                double deposited = Math.Round(Convert.ToDouble(t.Deposited), 0);
                                if (deposited > 0)
                                {
                                    var user_deposited = AccountController.GetByID(Convert.ToInt32(t.UID));
                                    if (user_deposited != null)
                                    {
                                        double wallet = Math.Round(Convert.ToDouble(user_deposited), 0);
                                        double walletleft = Math.Round(wallet + deposited, 0);
                                        AccountController.updateWallet(UID, walletleft, currentDate, username_current);
                                        HistoryPayWalletController.InsertTransportation(UID, username_current, 0, deposited,
                                        username_current + " nhận lại tiền của đơn hàng vận chuyển hộ: " + t.ID + ".",
                                        walletleft, 2, 11, currentDate, username_current, t.ID);
                                    }
                                }
                            }
                            else if (status == 1)
                            {
                                if (smallpacs.Count > 0)
                                {
                                    foreach (var item in smallpacs)
                                    {
                                        SmallPackageController.Delete(item.ID);
                                    }
                                }
                            }
                            else if (status == 2)
                            {
                                var setNoti = SendNotiEmailController.GetByID(15);
                                if (setNoti != null)
                                {
                                    if (setNoti.IsSentNotiUser == true)
                                    {
                                        NotificationsController.Inser(UID,
                                                          t.Username, t.ID,
                                                          "Đơn hàng vận chuyển hộ " + t.ID + " đã được duyệt.",
                                                          10, currentDate, ac.Username, true);
                                    }

                                    if (setNoti.IsSendEmailUser == true)
                                    {
                                        var acg = AccountController.GetByID(UID);
                                        if (acg != null)
                                        {
                                            try
                                            {
                                                PJUtils.SendMailGmail("pandaorder.com@gmail.com", "xxx", acg.Email,
                                                    "Thông báo tại Panda Order.", "Đơn hàng vận chuyển hộ " + t.ID + " đã được duyệt.", "");
                                            }
                                            catch { }
                                        }

                                    }
                                }
                            }
                            else if (status == 4)
                            {
                                if (smallpacs.Count > 0)
                                {
                                    foreach (var item in smallpacs)
                                    {
                                        SmallPackageController.UpdateStatus(item.ID, 2, currentDate, username_current);
                                    }
                                }
                            }
                            else if (status == 5)
                            {
                                if (smallpacs.Count > 0)
                                {
                                    foreach (var item in smallpacs)
                                    {
                                        SmallPackageController.UpdateStatus(item.ID, 3, currentDate, username_current);
                                    }
                                }
                            }
                            else if (status == 7)
                            {
                                if (smallpacs.Count > 0)
                                {
                                    foreach (var item in smallpacs)
                                    {
                                        SmallPackageController.UpdateStatus(item.ID, 4, currentDate, username_current);
                                    }
                                }
                            }

                            var smallpackages1 = SmallPackageController.GetByTransportationOrderID(tID);
                            if (status > 1)
                            {
                                if (smallpackages1.Count == 0)
                                {
                                    var transportationDetail = TransportationOrderDetailController.GetByTransportationOrderID(tID);
                                    if (transportationDetail.Count > 0)
                                    {
                                        foreach (var p in transportationDetail)
                                        {
                                            SmallPackageController.InsertWithTransportationIDNew(t.ID, 0, p.TransportationOrderCode, p.ProductType,
                                            0, Convert.ToDouble(p.Weight), 0, Convert.ToBoolean(p.IsCheckProduct), Convert.ToBoolean(p.IsPackaged), Convert.ToBoolean(p.IsInsurrance),
                                            p.CODTQCYN.ToString(), p.CODTQVND.ToString(), p.UserNote, "", p.ProductQuantity.ToString(), 1, currentDate, username_current, UID, Username);
                                        }
                                    }
                                }
                            }

                            double totalprice = 0;
                            double currency = Convert.ToDouble(t.Currency);
                            double totalWeight = Convert.ToDouble(t.TotalWeight);
                            //double FeeWeight = Convert.ToDouble(txtFeeWeight.Text);
                            double UnitPrice = Convert.ToDouble(txtUnitPrice.Text);
                            double FeeWeight = Math.Round(Convert.ToDouble(UnitPrice * totalWeight), 2);

                            double FeeHome = Convert.ToDouble(txtFeeHome.Text);
                            double checkProductPrice = Convert.ToDouble(txtFeeCheckPackage.Text);
                            double packagePrice = Convert.ToDouble(txtFeePack.Text);
                            double insurrancePrice = Convert.ToDouble(txtFeeInsurrance.Text);
                            double codtqCYN = Convert.ToDouble(txtTotalCODCNY.Text);
                            //double codtqVND = Convert.ToDouble(txtTotalCODVN.Text);
                            double codtqVND = Math.Round(Convert.ToDouble(codtqCYN * currency), 2);

                            totalprice = FeeWeight + checkProductPrice + packagePrice + insurrancePrice + codtqVND + FeeHome;
                            totalprice = Math.Round(totalprice, 0);

                            string StaffNote = txtStaffNote.Text;

                            TransportationOrderController.UpdateNew(tID, UID, t.Username, warehouseFrom, warehouseTo, shippingType, status, totalWeight, currency,
                            checkProductPrice, packagePrice, codtqCYN, codtqVND, insurrancePrice, totalprice, t.Description, currentDate, username_current, FeeWeight, StaffNote, FeeHome, UnitPrice);

                            var trasOrderDetail = TransportationOrderDetailController.GetAllByTransportationOrderID(tID);
                            var smallPKToUpdate = SmallPackageController.GetByOrderTransactionCode(trasOrderDetail.TransportationOrderCode);
                            SmallPackageController.UpdateTotalPrice(smallPKToUpdate.ID, FeeWeight);
                            PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công", "s", true, Page);
                        }
                    }
                }
            }
        }

        #region webservice
        [WebMethod]
        public static string getPrice(double weight, int warehouseFrom, int warehouseTo, int shippingType)
        {
            var t = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(warehouseFrom,
                    warehouseTo, shippingType, true);
            double price = 0;
            double totalprice = 0;
            if (t.Count > 0)
            {
                foreach (var w in t)
                {
                    if (w.WeightFrom < weight && weight <= w.WeightTo)
                    {
                        price = Math.Round(Convert.ToDouble(w.Price), 0);
                    }
                }
            }
            totalprice = Math.Round(price * weight, 0);
            return totalprice.ToString();
        }
        #endregion
    }
}