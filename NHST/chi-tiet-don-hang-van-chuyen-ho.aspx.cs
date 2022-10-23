using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using static NHST.Controllers.SmallPackageController;
using static Telerik.Web.UI.OrgChartStyles;

namespace NHST
{
    public partial class chi_tiet_don_hang_van_chuyen_ho1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                LoadData();
            }
        }

        public void LoadData()
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                int UID = obj_user.ID;
                var id = RouteData.Values["id"].ToString().ToInt(0);
                if (id > 0)
                {
                    ViewState["ID"] = id;
                    var t = TransportationOrderController.GetByIDAndUID(id, UID);
                    var smallPackages = SmallPackageController.GetByTransportationOrderID(t.ID);
                    if (t != null)
                    {
                        ltrMainOrderID.Text += "Chi tiết đơn hàng vận chuyển hộ #" + t.ID + "";

                        double totalPrice = Math.Round(Convert.ToDouble(t.TotalPrice), 0);
                        double FeeWeight = Math.Round(Convert.ToDouble(t.FeeWeight), 0);
                        double deposited = Math.Round(Convert.ToDouble(t.Deposited), 0);

                        //Các loại cân nặng
                        double totalWeight = Math.Round(Convert.ToDouble(t.TotalWeight), 0);
                        double canQuiDoi = 0;
                        double canTinhTien = 0;
                        double canThucTe = 0;
                        foreach (var item in smallPackages)
                        {
                            int packageType = 0;
                            if (item.Width != null && item.Height != null && item.Length != null)
                                canQuiDoi = (item.Width.Value * item.Height.Value * item.Length.Value) / 6000;
                            if (item.Weight != null)
                            {
                                canThucTe = item.Weight.Value;
                                if (canQuiDoi > item.Weight.Value)
                                {
                                    canTinhTien += canQuiDoi;
                                }
                                else
                                {
                                    canTinhTien += item.Weight.Value;
                                }
                            }
                        }
                        double totalmustpayleft = totalPrice - deposited;
                        double totalPay = Math.Round(totalPrice, 0);

                        string createdDate = string.Format("{0:dd/MM/yyyy}", t.CreatedDate);
                        int stt = Convert.ToInt32(t.Status);
                        string status = PJUtils.generateTransportationStatusNew2(stt);

                        string khoTQ = "";
                        string khoDich = "";
                        string shippingTypeName = "";
                        double totalPackage = 0;

                        int tID = t.ID;
                        var wareTQ = WarehouseFromController.GetByID(Convert.ToInt32(t.WarehouseFromID));
                        if (wareTQ != null)
                            khoTQ = wareTQ.WareHouseName;
                        var wareDich = WarehouseController.GetByID(Convert.ToInt32(t.WarehouseID));
                        if (wareDich != null)
                            khoDich = wareDich.WareHouseName;
                        var shippingType = ShippingTypeToWareHouseController.GetByID(Convert.ToInt32(t.ShippingTypeID));
                        if (shippingType != null)
                            shippingTypeName = shippingType.ShippingTypeName;

                        StringBuilder htmlPackages = new StringBuilder();
                        var tD = TransportationOrderDetailController.GetAllByTransportationOrderID(tID);
                        if (tD != null)
                        {
                            bool isCheckProduct = false;
                            bool isPackaged = false;
                            bool isInsurrance = false;
                            if (tD.IsCheckProduct != null)
                            {
                                isCheckProduct = Convert.ToBoolean(tD.IsCheckProduct);
                            }
                            if (tD.IsPackaged != null)
                            {
                                isPackaged = Convert.ToBoolean(tD.IsPackaged);
                            }
                            if (tD.IsInsurrance != null)
                            {
                                isInsurrance = Convert.ToBoolean(tD.IsInsurrance);
                            }
                            htmlPackages.Append("<tr>");
                            htmlPackages.Append("<td>" + tD.TransportationOrderCode + "</td>");
                            htmlPackages.Append("<td>" + tD.ProductType + "</td>");
                            htmlPackages.Append("<td class=\"no-wrap\">" + tD.ProductQuantity + "</td>");
                            htmlPackages.Append("<td>" + tD.StaffNoteCheck + "</td>");
                            htmlPackages.Append("<td class=\"no-wrap\">" + totalWeight + "</td>");
                            htmlPackages.Append("<td class=\"center-checkbox\">");
                            htmlPackages.Append("<label>");
                            if (isCheckProduct == true)
                            {
                                htmlPackages.Append("<input type=\"checkbox\" checked disabled />");
                            }
                            else
                            {
                                htmlPackages.Append("<input type=\"checkbox\" disabled />");
                            }
                            htmlPackages.Append("<span></span>");
                            htmlPackages.Append("</label>");
                            htmlPackages.Append("</td>");
                            htmlPackages.Append("<td class=\"center-checkbox\">");
                            htmlPackages.Append("<label>");
                            if (isPackaged == true)
                            {
                                htmlPackages.Append("<input type=\"checkbox\" checked disabled />");
                            }
                            else
                            {
                                htmlPackages.Append("<input type=\"checkbox\" disabled />");
                            }
                            htmlPackages.Append("<span></span>");
                            htmlPackages.Append("</label>");
                            htmlPackages.Append("</td>");
                            htmlPackages.Append("<td class=\"center-checkbox\">");
                            htmlPackages.Append("<label>");
                            if (isInsurrance == true)
                            {
                                htmlPackages.Append("<input type=\"checkbox\" checked disabled />");
                            }
                            else
                            {
                                htmlPackages.Append("<input type=\"checkbox\" disabled />");
                            }
                            htmlPackages.Append("<span></span>");
                            htmlPackages.Append("</label>");
                            htmlPackages.Append("</td>");
                            htmlPackages.Append("<td class=\"no-wrap\">¥" + tD.CODTQCYN + " - " + string.Format("{0:N0}", Convert.ToDouble(tD.CODTQVND)) + " VNĐ</td>");
                            htmlPackages.Append("<td class=\"tb-date\">" + tD.UserNote + "</td>");
                            htmlPackages.Append("<td class=\"no-wrap\">" + PJUtils.TransportationOrderDetailStatusNew(Convert.ToInt32(t.Status)) + "</td>");
                            htmlPackages.Append("</tr>");
                        }
                        ltrListPackage.Text = htmlPackages.ToString();

                        if (stt > 4)
                        {
                            if (totalmustpayleft > 0)
                            {
                                ltrBtn.Text += "  <a href=\"javascript:;\" onclick=\"PaymentOrder('" + t.ID + "')\" class=\"btn\">Thanh toán</a>";
                            }
                        }
                        if (stt == 1)
                        {
                            ltrBtn.Text += " <a href=\"javascript:;\" onclick=\"cancelOrder()\" class=\"btn\">Hủy đơn hàng</a>";
                            btnHuy.Visible = true;
                            btnHuy.Attributes.Add("style", "display:none");
                        }

                        #region Lấy thông tin
                        StringBuilder html = new StringBuilder();

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Ngày tạo đơn hàng:</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\" col s12 m6\">");
                        html.Append("<p class=\"black-text bold\">" + createdDate + "</p>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Trạng thái:</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"col s12 m6\">" + status + "</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Kho TQ</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"col s12 m6\"><p class=\"black-text bold\">" + khoTQ + "</p></div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Kho đích</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"col s12 m6\"><p class=\"black-text bold\">" + khoDich + "</p></div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Phương thức vận chuyển</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"col s12 m6\"><p class=\"black-text bold\">" + shippingTypeName + "</p></div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Tổng số kiện</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"col s12 m6\"><p class=\"black-text bold\">" + totalPackage + "</p></div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Tổng cân nặng thực tế</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"col s12 m6\"><p class=\"black-text bold\">" + canThucTe + " Kg</p></div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Tổng cân nặng quy đổi</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"col s12 m6\"><p class=\"black-text bold\">" + canQuiDoi + " Kg</p></div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Tổng cân nặng tính tiền</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"col s12 m6\"><p class=\"black-text bold\">" + canTinhTien + " Kg</p></div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Phí giao hàng</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"input-field col s12 m6\">");
                        html.Append("<input placeholder=\"0\" type=\"text\" value=\"" + string.Format("{0:N0}", t.FeeShipHome) + "\" disabled>");
                        html.Append("<label>Việt Nam đồng (VNĐ)</label>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Phí kiểm hàng</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"input-field col s12 m6\">");
                        html.Append("<input placeholder=\"0\" type=\"text\" value=\"" + string.Format("{0:N0}", t.CheckProductFee) + "\" disabled>");
                        html.Append("<label>Việt Nam đồng (VNĐ)</label>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Phí đóng gỗ</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"input-field col s12 m6\">");
                        html.Append("<input placeholder=\"0\" type=\"text\" value=\"" + string.Format("{0:N0}", t.PackagedFee) + "\" disabled>");
                        html.Append("<label>Việt Nam đồng (VNĐ)</label>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Phí bảo hiểm</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"input-field col s12 m6\">");
                        html.Append("<input placeholder=\"0\" type=\"text\" value=\"" + string.Format("{0:N0}", t.InsurranceFee) + "\" disabled>");
                        html.Append("<label>Việt Nam đồng (VNĐ)</label>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Tổng COD Trung Quốc</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"input-field col s12 m6\">");
                        html.Append("<input placeholder=\"0\" type=\"text\" value=\"" + string.Format("{0:N0}", t.TotalCODTQVND) + "\" disabled>");
                        html.Append("<label>Việt Nam đồng (VNĐ)</label>");
                        html.Append("</div>");
                        html.Append("<div class=\"input-field col s12 m6\">");
                        html.Append("<input placeholder=\"0\" type=\"text\" value=\"" + t.TotalCODTQCYN + "\" disabled>");
                        html.Append("<label>Tệ (¥)</label>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        //html.Append("<div class=\"order-row\">");
                        //html.Append("<div class=\"left-fixed\"><p class=\"txt\">Tiền lưu kho</p></div>");
                        //html.Append("<div class=\"right-content\">");
                        //html.Append("<div class=\"row\">");
                        //html.Append("<div class=\"input-field col s12 m6\">");
                        //html.Append("<input placeholder=\"0\" type=\"text\" value=\"" + string.Format("{0:N0}", warehouseFee) + "\" disabled class=\"\">");
                        //html.Append("<label>Việt Nam đồng (VNĐ)</label>");
                        //html.Append("</div>");
                        //html.Append("</div>");
                        //html.Append("</div>");
                        //html.Append("</div>");

                        if (totalPrice > 0)
                        {
                            html.Append("<div class=\"order-row\">");
                            html.Append("<div class=\"left-fixed\"><p class=\"txt\">Tiền vận chuyển</p></div>");
                            html.Append("<div class=\"right-content\">");
                            html.Append("<div class=\"row\">");
                            html.Append("<div class=\"input-field col s12 m6\">");
                            html.Append("<input placeholder=\"0\" type=\"text\" value=\"" + string.Format("{0:N0}", FeeWeight) + "\" disabled class=\"\">");
                            html.Append("<label>Việt Nam đồng (VNĐ)</label>");
                            html.Append("</div>");
                            html.Append("</div>");
                            html.Append("</div>");
                            html.Append("</div>");

                            html.Append("<div class=\"order-row\">");
                            html.Append("<div class=\"left-fixed\"><p class=\"txt\">Tổng tiền</p></div>");
                            html.Append("<div class=\"right-content\">");
                            html.Append("<div class=\"row\">");
                            html.Append("<div class=\"input-field col s12 m6\">");
                            html.Append("<input placeholder=\"0\" type=\"text\" value=\"" + string.Format("{0:N0}", totalPay) + "\" disabled class=\"bold\">");
                            html.Append("<label>Việt Nam đồng (VNĐ)</label>");
                            html.Append("</div>");
                            html.Append("</div>");
                            html.Append("</div>");
                            html.Append("</div>");
                            if (totalmustpayleft >= deposited)
                            {
                                html.Append("<div class=\"order-row\">");
                                html.Append("<div class=\"left-fixed\"><p class=\"txt\">Đã thanh toán</p></div>");
                                html.Append("<div class=\"right-content\">");
                                html.Append("<div class=\"row\">");
                                html.Append("<div class=\"input-field col s12 m6\">");
                                html.Append("<input placeholder=\"0\" type=\"text\" value=\"" + string.Format("{0:N0}", deposited) + "\" disabled class=\"bold\">");
                                html.Append("<label>Việt Nam đồng (VNĐ)</label>");
                                html.Append("</div>");
                                html.Append("</div>");
                                html.Append("</div>");
                                html.Append("</div>");
                                double leftMoney = totalPay - deposited;
                                html.Append("<div class=\"order-row\">");
                                html.Append("<div class=\"left-fixed\"><p class=\"txt\">Còn lại</p></div>");
                                html.Append("<div class=\"right-content\">");
                                html.Append("<div class=\"row\">");
                                html.Append("<div class=\"input-field col s12 m6\">");
                                html.Append("<input placeholder=\"0\" type=\"text\" value=\"" + string.Format("{0:N0}", leftMoney) + "\" disabled class=\"red-text bold\">");
                                html.Append("<label>Việt Nam đồng (VNĐ)</label>");
                                html.Append("</div>");
                                html.Append("</div>");
                                html.Append("</div>");
                                html.Append("</div>");
                            }
                        }

                        html.Append("<div class=\"order-row\">");
                        html.Append("<div class=\"left-fixed\"><p class=\"txt\">Ghi chú:</p></div>");
                        html.Append("<div class=\"right-content\">");
                        html.Append("<div class=\"row\">");
                        html.Append("<div class=\"input-field col s12 m12\"><textarea id=\"textarea2\" class=\"materialize-textarea\">" + t.Description + "</textarea></div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");

                        ltrInfor.Text = html.ToString();
                        #endregion

                        //#region load Map
                        //List<warehouses> lwh = new List<warehouses>();
                        //var smallkhoTQ = WarehouseFromController.GetByID(t.WarehouseFromID.Value);
                        //if (smallkhoTQ != null)
                        //{
                        //    warehouses wh = new warehouses();
                        //    wh.name = smallkhoTQ.WareHouseName;
                        //    wh.lat = smallkhoTQ.Latitude;
                        //    wh.lng = smallkhoTQ.Longitude;

                        //    ltrTQ.Text = "<div class=\"from\"><span class=\"lb position\" data-lat=\"" + smallkhoTQ.Latitude + "\" data-lng=\"" + smallkhoTQ.Longitude + "\" id=\"js-map-from\">" + smallkhoTQ.WareHouseName + "</span></div>";

                        //    var lsmall = SmallPackageController.GetByTransportationOrderID(t.ID);
                        //    if (lsmall.Count > 0)
                        //    {
                        //        var inTQ = lsmall.Where(x => Convert.ToInt32(x.CurrentPlaceID) == smallkhoTQ.ID && x.Status == 2).ToList();
                        //        if (inTQ.Count > 0)
                        //        {
                        //            List<package> lpc = new List<package>();
                        //            foreach (var item in inTQ)
                        //            {
                        //                package pk = new package();
                        //                pk.code = item.OrderTransactionCode;
                        //                pk.status = "Đang vận chuyển";
                        //                pk.classColor = "being-transport";
                        //                lpc.Add(pk);
                        //            }
                        //            wh.package = lpc;
                        //        }
                        //    }
                        //    lwh.Add(wh);
                        //}

                        //var smallkhoVN = WarehouseController.GetByID(Convert.ToInt32(t.WarehouseID.Value));
                        //if (smallkhoVN != null)
                        //{
                        //    warehouses wh = new warehouses();
                        //    wh.name = smallkhoVN.WareHouseName;
                        //    wh.lat = smallkhoVN.Latitude;
                        //    wh.lng = smallkhoVN.Longitude;

                        //    ltrVN.Text = "<div class=\"to\"><span class=\"lb position\" data-lat=\"" + smallkhoVN.Latitude + "\" data-lng=\"" + smallkhoVN.Longitude + "\" id=\"js-map-to\">" + smallkhoVN.WareHouseName + "</span></div>";

                        //    var lsmall = SmallPackageController.GetByTransportationOrderID(t.ID);
                        //    if (lsmall.Count > 0)
                        //    {
                        //        var inVN = lsmall.Where(x => Convert.ToInt32(x.CurrentPlaceID) == smallkhoVN.ID && x.Status == 3).ToList();
                        //        if (inVN.Count > 0)
                        //        {
                        //            List<package> lpc = new List<package>();
                        //            foreach (var item in inVN)
                        //            {
                        //                package pk = new package();
                        //                pk.code = item.OrderTransactionCode;
                        //                pk.status = "Đã về kho đích";
                        //                pk.classColor = "transported";
                        //                lpc.Add(pk);
                        //            }
                        //            wh.package = lpc;
                        //        }
                        //    }
                        //    lwh.Add(wh);
                        //}

                        //JavaScriptSerializer serializer = new JavaScriptSerializer();
                        //hdfLoadMap.Value = serializer.Serialize(lwh);

                        //#endregion
                    }
                }
            }
        }

        public class warehouses
        {
            public string name { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public List<package> package { get; set; }
        }

        public class package
        {
            public string code { get; set; }
            public string status { get; set; }
            public string classColor { get; set; }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int OID = hdfOrderID.Value.ToInt();
                if (OID > 0)
                {
                    var t = TransportationOrderController.GetByIDAndUID(OID, Convert.ToInt32(obj_user.ID));

                    if (t != null)
                    {
                        double wallet = Convert.ToDouble(obj_user.Wallet);
                        double TotalPriceVND = Convert.ToDouble(t.TotalPrice);
                        double Deposited = 0;
                        double LeftMoney = 0;

                        if (t.Deposited != null)
                            Deposited = Convert.ToDouble(t.Deposited);

                        LeftMoney = Math.Round(TotalPriceVND - Deposited, 0);

                        if (LeftMoney > 0)
                        {
                            if (LeftMoney <= wallet)
                            {
                                double walletLeft = Math.Round(wallet - LeftMoney, 0);
                                int a = TransactionController.PayVanChuyenHo(t.ID, TotalPriceVND, 6, currentDate, username_current, Convert.ToInt32(obj_user.ID), walletLeft, 0, LeftMoney, username_current + " đã thanh toán đơn hàng vận chuyển hộ: " + t.ID + ".", 1, 8);
                                if (a == 1)
                                {
                                    var setNoti = SendNotiEmailController.GetByID(14);
                                    if (setNoti != null)
                                    {
                                        if (setNoti.IsSentNotiAdmin == true)
                                        {
                                            var admins = AccountController.GetAllByRoleID(0);
                                            if (admins.Count > 0)
                                            {
                                                foreach (var admin in admins)
                                                {
                                                    NotificationsController.Inser(admin.ID, admin.Username, t.ID, "Đơn hàng vận chuyển hộ " + t.ID + " đã được thanh toán.", 10, currentDate, username_current, false);
                                                    string strPathAndQuery = Request.Url.PathAndQuery;
                                                    string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                    string datalink = "" + strUrl + "manager/chi-tet-don-hang-van-chuyen-ho/" + t.ID;
                                                    PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng vận chuyển hộ " + t.ID + " đã được thanh toán.", datalink);

                                                }
                                            }
                                        }

                                        //if (setNoti.IsSentEmailAdmin == true)
                                        //{
                                        //    var admins = AccountController.GetAllByRoleID(0);
                                        //    if (admins.Count > 0)
                                        //    {
                                        //        foreach (var admin in admins)
                                        //        {
                                        //            try
                                        //            {
                                        //                PJUtils.SendMailGmail("monamedia", "uvnajxzzurygsduf", admin.Email,
                                        //                    "Thông báo tại THUẬN ĐẠT EXPRESS.", "Đơn hàng vận chuyển hộ " + t.ID + " đã được thanh toán.", "");
                                        //            }
                                        //            catch { }
                                        //        }
                                        //    }
                                        //}
                                    }

                                    PJUtils.ShowMessageBoxSwAlert("Thanh toán đơn thành công", "s", true, Page);
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý, vui lòng thử lại sau", "e", true, Page);
                                }
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Số tiền trong tài khoản của bạn không đủ để thanh toán đơn hàng này.", "e", true, Page);
                            }
                        }
                    }
                }
            }
        }

        protected void btnHuy_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                int UID = obj_user.ID;
                var id = ViewState["ID"].ToString().ToInt(0);
                if (id > 0)
                {
                    var t = TransportationOrderController.GetByIDAndUID(id, UID);
                    if (t != null)
                    {
                        TransportationOrderController.UpdateStatus(t.ID, 0, DateTime.UtcNow.AddHours(7), username_current);
                        PJUtils.ShowMessageBoxSwAlert("Hủy đơn hàng thành công", "s", true, Page);
                    }
                }
            }
        }
    }
}