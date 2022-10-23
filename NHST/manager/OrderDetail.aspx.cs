using NHST.Models;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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
    public partial class OrderDetail : System.Web.UI.Page
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
                    int RoleID = Convert.ToInt32(ac.RoleID);
                    if (ac.RoleID == 1)
                        Response.Redirect("/trang-chu");
                    else
                    {
                        if (RoleID == 4)
                        {
                            Response.Redirect("/manager/home.aspx");
                        }
                    }
                }
                checkOrderStaff();
                LoadDDL();
                loaddata();
            }
        }

        public void checkOrderStaff()
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                int RoleID = obj_user.RoleID.ToString().ToInt();
                int UID = obj_user.ID;
                var id = Convert.ToInt32(Request.QueryString["id"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {
                        int status_order = Convert.ToInt32(o.Status);
                        if (RoleID == 0 || RoleID == 2)
                        {

                        }
                        else if (RoleID == 3)
                        {
                            if (status_order >= 2)
                            {
                                //Role đặt hàng
                                if (o.DathangID == UID)
                                {
                                }

                                else
                                {
                                    Response.Redirect("/manager/OrderList.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }

                        }
                        else if (RoleID == 4)
                        {
                            if (status_order >= 5 && status_order < 7)
                            {
                                //Role kho TQ
                                if (o.KhoTQID == UID || o.KhoTQID == 0)
                                {

                                }
                                else
                                {
                                    Response.Redirect("/manager/OrderList.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }

                        }
                        else if (RoleID == 6)
                        {
                            if (status_order != 1)
                            {
                                //Role sale
                                if (o.SalerID == UID)
                                {

                                }
                                else
                                {
                                    Response.Redirect("/manager/OrderList.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }
                        }
                        else if (RoleID == 7)
                        {
                            if (status_order >= 2)
                            {

                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }
                        }
                        else if (RoleID == 8)
                        {
                            if (status_order >= 9 && status_order < 10)
                            {

                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("/manager/OrderList.aspx");
                }
            }
        }
        public void LoadDDL()
        {
            ddlSaler.Items.Clear();
            ddlSaler.Items.Insert(0, "Chọn Saler");

            ddlDatHang.Items.Clear();
            ddlDatHang.Items.Insert(0, "Chọn nhân viên đặt hàng");

            ddlKhoTQ.Items.Clear();
            ddlKhoTQ.Items.Insert(0, "Chọn nhân viên kho TQ");

            ddlKhoVN.Items.Clear();
            ddlKhoVN.Items.Insert(0, "Chọn nhân viên Việt Nam");

            var salers = AccountController.GetAllByRoleID(6);
            if (salers.Count > 0)
            {
                ddlSaler.DataSource = salers;
                ddlSaler.DataBind();
            }

            var dathangs = AccountController.GetAllByRoleID(3);
            if (dathangs.Count > 0)
            {
                ddlDatHang.DataSource = dathangs;
                ddlDatHang.DataBind();
            }

            var khotqs = AccountController.GetAllByRoleID(4);
            if (khotqs.Count > 0)
            {
                ddlKhoTQ.DataSource = khotqs;
                ddlKhoTQ.DataBind();
            }

            var khovns = AccountController.GetAllByRoleID(5);
            if (khovns.Count > 0)
            {
                ddlKhoVN.DataSource = khovns;
                ddlKhoVN.DataBind();
            }
            var warehousefrom = WarehouseFromController.GetAllWithIsHidden(false);
            if (warehousefrom.Count > 0)
            {
                ddlWarehouseFrom.DataSource = warehousefrom;
                ddlWarehouseFrom.DataBind();
            }


            var warehouse = WarehouseController.GetAllWithIsHidden(false);
            if (warehouse.Count > 0)
            {
                ddlReceivePlace.DataSource = warehouse;
                ddlReceivePlace.DataBind();
            }

            var shippingtype = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shippingtype.Count > 0)
            {
                ddlShippingType.DataSource = shippingtype;
                ddlShippingType.DataBind();
            }
        }
        public void loaddata()
        {

            var config = ConfigurationController.GetByTop1();
            double currency = 0;
            double currency1 = 0;
            if (config != null)
            {
                double currencyconfig = 0;
                if (!string.IsNullOrEmpty(config.Currency))
                    currencyconfig = Convert.ToDouble(config.Currency);

                currency = Math.Round(currencyconfig, 0);
                currency1 = Math.Round(currencyconfig, 0);
            }

            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);

            int uid = obj_user.ID;

            var id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                var o = MainOrderController.GetAllByID(id);
                if (o != null)
                {
                    hdfOrderID.Value = o.ID.ToString();
                    if (o.OrderType == 3)
                    {
                        pnbaogia.Visible = true;
                        hdfBaoGiaVisible.Value = "1";
                    }
                    else
                    {
                        pnbaogia.Visible = false;
                        hdfBaoGiaVisible.Value = "0";
                    }
                    chkIsCheckPrice.Value = Convert.ToBoolean(o.IsCheckNotiPrice).ToString();
                    chkIsDoneSmallPackage.Value = Convert.ToBoolean(o.IsDoneSmallPackage).ToString();

                    ViewState["ID"] = id;
                    //ltrPrint.Text += "<a class=\"btn btn border-btn\" target=\"_blank\" href='/manager/PrintStamp.aspx?id=" + id + "'>In Tem</a>";
                    double currentcyynn = 0;
                    if (!string.IsNullOrEmpty(o.CurrentCNYVN))
                        currentcyynn = Math.Round(Convert.ToDouble(o.CurrentCNYVN), 0);
                    currency = currentcyynn;
                    currency1 = currency;
                    hdfcurrent.Value = Math.Round(currency, 0).ToString();
                    ViewState["MOID"] = id;

                    #region Lịch sử thanh toán
                    StringBuilder htmlPaid = new StringBuilder();
                    var PayorderHistory = PayOrderHistoryController.GetAllByMainOrderID(o.ID);
                    if (PayorderHistory.Count > 0)
                    {

                        foreach (var item in PayorderHistory)
                        {
                            htmlPaid.Append("<tr>");
                            htmlPaid.Append("    <td>" + item.CreatedDate + "</td>");
                            htmlPaid.Append("    <td>" + PJUtils.ShowStatusPayHistoryNew(item.Status.ToString().ToInt(0)) + "</td>");
                            if (item.Type.ToString() == "1")
                            {
                                htmlPaid.Append("    <td>Trực tiếp</td>");
                            }
                            else
                            {
                                htmlPaid.Append("    <td>Ví điện tử</td>");
                            }
                            htmlPaid.Append("    <td>" + string.Format("{0:N0}", item.Amount.Value) + " VNÐ</td>");
                            htmlPaid.Append("</tr>");
                        }
                        //rptPayment.DataSource = PayorderHistory;
                        //rptPayment.DataBind();
                    }
                    else
                    {

                        htmlPaid.Append("<tr class=\"noti\"><td class=\"red-text\" colspan=\"4\">Không có lịch sử thanh toán nào</td></tr>");
                        //ltrpa.Text = "<tr>Chưa có lịch sử thanh toán nào.</tr>";
                    }
                    ltrpa.Text = htmlPaid.ToString();
                    #endregion                    

                    if (obj_user != null)
                    {
                        hdfID.Value = obj_user.ID.ToString();
                        #region CheckRole
                        int RoleID = Convert.ToInt32(obj_user.RoleID);

                        if (RoleID == 7)
                        {
                            ddlStatus.Items.Add(new ListItem("Hủy đơn hàng", "1"));
                            ddlStatus.Items.Add(new ListItem("Mới tạo", "0"));
                            ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đã mua hàng", "4"));
                            ddlStatus.Items.Add(new ListItem("Admin đã thanh toán", "8"));
                            ddlStatus.Items.Add(new ListItem("Shop phát hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Hàng về kho TQ", "6"));
                            ddlStatus.Items.Add(new ListItem("Đang vận chuyển Quốc tế", "3"));
                            ddlStatus.Items.Add(new ListItem("Hàng về kho VN", "7"));
                            ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                            ddlStatus.Enabled = false;
                            pCNShipFeeNDT.Visible = false;
                            ////pBuyNDT.Visible = false;
                            pWeightNDT.Visible = false;
                            pCheckNDT.Visible = false;
                            pPackedNDT.Visible = false;
                            pDeposit.Enabled = false;
                            pCNShipFee.Enabled = false;
                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;
                            pCheck.Enabled = false;
                            pWeight.Enabled = false;
                            pPacked.Enabled = false;
                            pShipHome.Enabled = true;
                            ltr_OrderFee_UserInfo.Visible = false;
                            ltr_AddressReceive.Visible = false;
                            //btnUpdate.Visible = true;
                            //btnThanhtoan.Visible = true;
                            ////pShipHomeNDT.Visible = true;
                            ////pnadminmanager.Visible = true;
                            ddlWarehouseFrom.Enabled = true;
                            ddlReceivePlace.Enabled = true;
                            ddlShippingType.Enabled = true;
                            pAmountDeposit.Enabled = false;
                        }
                        else if (RoleID == 3)
                        {
                            ddlStatus.Items.Add(new ListItem("Hủy đơn hàng", "1"));
                            ddlStatus.Items.Add(new ListItem("Mới tạo", "0"));
                            ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đã mua hàng", "4"));
                            ddlStatus.Items.Add(new ListItem("Admin đã thanh toán", "8"));
                            ddlStatus.Items.Add(new ListItem("Shop phát hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Hàng về kho TQ", "6"));
                            ddlStatus.Items.Add(new ListItem("Đang vận chuyển Quốc tế", "3"));
                            ddlStatus.Items.Add(new ListItem("Hàng về kho VN", "7"));
                            ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));

                            pCNShipFeeNDT.Enabled = true;
                            pCNShipFee.Enabled = true;
                            pWeightNDT.Visible = false;
                            pCheckNDT.Visible = false;
                            pPackedNDT.Visible = false;
                            pDeposit.Enabled = false;
                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;
                            pCheck.Enabled = false;
                            pWeight.Enabled = false;
                            pPacked.Enabled = false;
                            pShipHome.Enabled = false;
                            ltr_OrderFee_UserInfo.Visible = true;
                            ltr_AddressReceive.Visible = true;
                            ddlShippingType.Enabled = true;
                            ddlWarehouseFrom.Enabled = true;
                            ltrBtnUpdate.Text = "<a href=\"javascript:;\" class=\"btn mt-2\" onclick=\"UpdateOrder()\">CẬP NHẬT</a>";
                        }
                        else if (RoleID == 4)
                        {
                            if (o.Status < 4)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Mới tạo", "0"));
                                ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                                ddlStatus.Items.Add(new ListItem("Đã mua hàng", "4"));
                                ddlStatus.Items.Add(new ListItem("Admin đã thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Shop phát hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Hàng về kho TQ", "6"));
                                ddlStatus.Items.Add(new ListItem("Đang vận chuyển Quốc tế", "3"));
                                ddlStatus.Items.Add(new ListItem("Hàng về kho VN", "7"));
                                ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                //btnUpdate.Visible = false;
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;
                                pWeightNDT.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            else if (o.Status >= 4 && o.Status < 6)
                            {
                                ddlStatus.Enabled = true;
                                pPackedNDT.Enabled = true;
                                pPacked.Enabled = true;
                                pWeightNDT.Enabled = true;
                                pWeight.Enabled = true;

                                ddlStatus.Items.Add(new ListItem("Đã mua hàng", "4"));
                                ddlStatus.Items.Add(new ListItem("Admin đã thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Shop đã phát hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Hàng về kho TQ", "6"));
                            }
                            else if (o.Status >= 6)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Mới tạo", "0"));
                                ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                                ddlStatus.Items.Add(new ListItem("Đã mua hàng", "4"));
                                ddlStatus.Items.Add(new ListItem("Admin đã thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Shop đã phát hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Hàng về kho TQ", "6"));
                                ddlStatus.Items.Add(new ListItem("Đang vận chuyển Quốc tế", "3"));
                                ddlStatus.Items.Add(new ListItem("Hàng về kho VN", "7"));
                                //ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                //btnUpdate.Visible = false;
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;
                                pWeightNDT.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            ////pDepositNDT.Visible = false;                            

                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = false;

                            pCheck.Enabled = false;
                            pCheckNDT.Enabled = false;

                            pDeposit.Enabled = false;
                            pBuyNDT.Enabled = false;

                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;

                            rTotalPriceRealCYN.Visible = false;
                            rTotalPriceReal.Visible = false;
                            pHHCYN.Visible = false;
                            pHHVND.Visible = false;

                            pShipHome.Enabled = false;
                            ltr_OrderFee_UserInfo.Visible = false;
                            ltr_AddressReceive.Visible = false;
                        }
                        else if (RoleID == 5)
                        {
                            if (o.Status < 4)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Mới tạo", "0"));
                                ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                                ddlStatus.Items.Add(new ListItem("Đã mua hàng", "4"));
                                ddlStatus.Items.Add(new ListItem("Admin đã thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Shop phát hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Hàng về kho TQ", "6"));
                                ddlStatus.Items.Add(new ListItem("Đang vận chuyển Quốc tế", "3"));
                                ddlStatus.Items.Add(new ListItem("Hàng về kho VN", "7"));
                                ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                //btnUpdate.Visible = false;
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;
                                pWeightNDT.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            else if (o.Status >= 4)
                            {
                                ddlStatus.Enabled = true;
                                ddlStatus.Items.Add(new ListItem("Đã mua hàng", "4"));
                                ddlStatus.Items.Add(new ListItem("Admin đã thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Shop phát hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Hàng về kho TQ", "6"));
                                ddlStatus.Items.Add(new ListItem("Đang vận chuyển Quốc tế", "3"));
                                ddlStatus.Items.Add(new ListItem("Hàng về kho VN", "7"));
                                ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                                ////ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                                ////ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;

                                pWeightNDT.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            ////if (o.Status >= 7)
                            ////{
                            ////    ddlStatus.Enabled = false;
                            ////    ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ////    ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                            ////    ddlStatus.Items.Add(new ListItem("Đang về Việt Nam", "6"));
                            ////    ddlStatus.Items.Add(new ListItem("Đã nhận hàng tại VN", "7"));
                            ////    ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                            ////    ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ////    btnUpdate.Visible = false;
                            ////    pPackedNDT.Enabled = false;
                            ////    pPacked.Enabled = false;
                            ////    pWeightNDT.Enabled = false;
                            ////    pWeight.Enabled = false;
                            ////}

                            ////pDepositNDT.Visible = false;

                            pCNShipFeeNDT.Enabled = false;

                            rTotalPriceRealCYN.Visible = false;
                            rTotalPriceReal.Visible = false;
                            pHHCYN.Visible = false;
                            pHHVND.Visible = false;

                            pCheckNDT.Enabled = false;
                            pCheck.Enabled = false;

                            pDeposit.Enabled = false;

                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = false;

                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;

                            pShipHome.Enabled = false;

                            ltr_OrderFee_UserInfo.Visible = false;
                            ltr_AddressReceive.Visible = false;
                        }
                        else if (RoleID == 0)
                        {
                            pnadminmanager.Visible = true;
                            ddlStatus.Items.Add(new ListItem("Hủy đơn hàng", "1"));
                            ddlStatus.Items.Add(new ListItem("Mới tạo", "0"));
                            ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đã mua hàng", "4"));
                            ddlStatus.Items.Add(new ListItem("Admin đã thanh toán", "8"));
                            ddlStatus.Items.Add(new ListItem("Shop phát hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Hàng về kho TQ", "6"));
                            ddlStatus.Items.Add(new ListItem("Đang vận chuyển Quốc tế", "3"));
                            ddlStatus.Items.Add(new ListItem("Hàng về kho VN", "7"));
                            ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));

                            //btnThanhtoan.Visible = true;
                            pAmountDeposit.Enabled = true;
                            pDeposit.Enabled = true;
                            chkCheck.Value += "true";
                            chkPackage.Value += "true";
                            chkShiphome.Value += "true";
                            ddlWarehouseFrom.Enabled = true;
                            ddlReceivePlace.Enabled = true;
                            ddlShippingType.Enabled = true;

                            double TotalPrice = Convert.ToDouble(o.TotalPriceVND);
                            double Deposit = Convert.ToDouble(o.Deposit);
                            double MustPay = Math.Round(TotalPrice - Deposit, 0);

                            ltrBtnUpdate.Text = "<a href=\"javascript:;\" class=\"btn mt-2\" onclick=\"UpdateOrder()\">CẬP NHẬT</a>";
                            if (o.Status > 6 && o.Status != 8)
                            {
                                if (MustPay > 0)
                                    ltrBtnPayment.Text = "<a href =\"javascript:;\" class=\"btn mt-2\" onclick=\"payallorder('" + o.ID + "',$(this))\" style=\"background-color: green; margin-left: 10px;\">THANH TOÁN</a>";
                            }
                        }
                        else if (RoleID == 2)
                        {
                            pnadminmanager.Visible = true;
                            ddlStatus.Items.Add(new ListItem("Hủy đơn hàng", "1"));
                            ddlStatus.Items.Add(new ListItem("Mới tạo", "0"));
                            ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đã mua hàng", "4"));
                            ddlStatus.Items.Add(new ListItem("Admin đã thanh toán", "8"));
                            ddlStatus.Items.Add(new ListItem("Shop phát hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Hàng về kho TQ", "6"));
                            ddlStatus.Items.Add(new ListItem("Đang vận chuyển Quốc tế", "3"));
                            ddlStatus.Items.Add(new ListItem("Hàng về kho VN", "7"));
                            ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));

                            ddlStatus.Enabled = true;
                            pDeposit.Enabled = false;
                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = true;
                            pBuy.Enabled = true;
                            pWeightNDT.Enabled = false;
                            pWeight.Enabled = false;
                            pCheckNDT.Enabled = false;
                            pCheck.Enabled = false;
                            pPackedNDT.Enabled = true;
                            pPacked.Enabled = true;
                            pShipHome.Enabled = true;
                            ddlShippingType.Enabled = true;

                            double TotalPrice = Convert.ToDouble(o.TotalPriceVND);
                            double Deposit = Convert.ToDouble(o.Deposit);
                            double MustPay = Math.Round(TotalPrice - Deposit, 0);

                            ltrBtnUpdate.Text = "<a href=\"javascript:;\" class=\"btn mt-2\" onclick=\"UpdateOrder()\">CẬP NHẬT</a>";
                            if (o.Status > 6)
                            {
                                if (MustPay > 0)
                                    ltrBtnPayment.Text = "<a href =\"javascript:;\" class=\"btn mt-2\" onclick=\"payallorder('" + o.ID + "',$(this))\" style=\"background-color: green; margin-left: 10px;\">THANH TOÁN</a>";
                            }
                        }
                        else if (RoleID == 6)
                        {
                            ddlStatus.Items.Add(new ListItem("Hủy đơn hàng", "1"));
                            ddlStatus.Items.Add(new ListItem("Mới tạo", "0"));
                            ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đã mua hàng", "4"));
                            ddlStatus.Items.Add(new ListItem("Admin đã thanh toán", "8"));
                            ddlStatus.Items.Add(new ListItem("Shop phát hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Hàng về kho TQ", "6"));
                            ddlStatus.Items.Add(new ListItem("Đang vận chuyển Quốc tế", "3"));
                            ddlStatus.Items.Add(new ListItem("Hàng về kho VN", "7"));
                            ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));

                            ddlStatus.Enabled = false;
                            pDeposit.Enabled = false;
                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = false;
                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;
                            pWeightNDT.Enabled = false;
                            pWeight.Enabled = false;
                            pCheckNDT.Enabled = false;
                            pCheck.Enabled = false;
                            pPackedNDT.Enabled = false;
                            pPacked.Enabled = false;
                            pShipHome.Enabled = false;

                            rTotalPriceRealCYN.Visible = false;
                            rTotalPriceReal.Visible = false;
                            pHHCYN.Visible = false;
                            pHHVND.Visible = false;
                        }
                        int countOc = 1;
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode2) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight2))
                        {
                            hdfoc2.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc2.Value = "0";
                        }
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode3) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight3))
                        {
                            hdfoc3.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc3.Value = "0";
                        }
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode4) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight4))
                        {
                            hdfoc4.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc4.Value = "0";
                        }
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode5) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight5))
                        {
                            hdfoc5.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc5.Value = "0";
                        }
                        hdforderamount.Value = countOc.ToString();
                        #endregion
                        #region Lấy thông tin nhân viên
                        ddlSaler.SelectedValue = o.SalerID.ToString();
                        ddlDatHang.SelectedValue = o.DathangID.ToString();
                        ddlKhoTQ.SelectedValue = o.KhoTQID.ToString();
                        ddlKhoVN.SelectedValue = o.KhoVNID.ToString();
                        string NameSale = "";
                        int SaleID = Convert.ToInt32(o.SalerID);
                        if (SaleID > 0)
                        {
                            var sale = AccountController.GetByID(SaleID);
                            if (sale != null)
                            {
                                NameSale = sale.Username;
                            }
                        }
                        ltrSaleName.Text = "<strong>" + NameSale + "</strong>";
                        #endregion
                        #region Lấy thông tin người đặt
                        string Username = "";
                        var usercreate = AccountController.GetByID(Convert.ToInt32(o.UID));
                        double ckFeeBuyPro = 0;
                        double ckFeeWeight = 0;
                        if (usercreate != null)
                        {
                            ckFeeBuyPro = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeBuyPro.ToString());
                            ckFeeWeight = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeWeight.ToString());

                            lblCKFeebuypro.Text = ckFeeBuyPro.ToString();
                            lblCKFeeWeight.Text = ckFeeWeight.ToString();

                            hdfFeeBuyProDiscount.Value = ckFeeBuyPro.ToString();
                            hdfFeeWeightDiscount.Value = ckFeeWeight.ToString();

                            Username = usercreate.Username;
                        }
                        else
                        {
                            lblCKFeebuypro.Text = "0";
                            lblCKFeeWeight.Text = "0";
                            hdfFeeBuyProDiscount.Value = "0";
                            hdfFeeWeightDiscount.Value = "0";
                        }

                        ltr_OrderID.Text = "<strong>" + o.ID + "</strong>";
                        ltrUsername.Text = "<strong>" + Username + "</strong>";

                        StringBuilder customerInfo = new StringBuilder();
                        var ui = AccountInfoController.GetByUserID(Convert.ToInt32(o.UID));
                        if (ui != null)
                        {
                            string phone = ui.MobilePhonePrefix + ui.MobilePhone;
                            customerInfo.Append("<table class=\"table\">");
                            customerInfo.Append("    <tbody>");
                            customerInfo.Append("        <tr>");
                            customerInfo.Append("            <td>Username</td>");
                            customerInfo.Append("            <td>" + AccountController.GetByID(Convert.ToInt32(o.UID)).Username + "</td>");
                            customerInfo.Append("        </tr>");
                            customerInfo.Append("        <tr>");
                            customerInfo.Append("            <td>Địa chỉ</td>");
                            customerInfo.Append("            <td>" + ui.Address + "</td>");
                            customerInfo.Append("        </tr>");
                            customerInfo.Append("        <tr>");
                            customerInfo.Append("            <td>Email</td>");
                            customerInfo.Append("            <td><a href=\"" + ui.Email + "\">" + ui.Email + "</a></td>");
                            customerInfo.Append("        </tr>");
                            customerInfo.Append("        <tr>");
                            customerInfo.Append("            <td>Số ĐT</td>");
                            customerInfo.Append("            <td><a href=\"tel:+" + phone + "\">" + phone + "</a></td>");
                            customerInfo.Append("        </tr>");
                            customerInfo.Append("        <tr>");
                            customerInfo.Append("            <td>Ghi chú</td>");
                            customerInfo.Append("            <td>" + o.Note + "</td>");
                            customerInfo.Append("        </tr>");
                            customerInfo.Append("    </tbody>");
                            customerInfo.Append("</table>");
                        }
                        ltr_OrderFee_UserInfo.Text = customerInfo.ToString();
                        var kd = AccountController.GetByID(Convert.ToInt32(o.SalerID));
                        var dathang = AccountController.GetByID(Convert.ToInt32(o.DathangID));
                        var khotq = AccountController.GetByID(Convert.ToInt32(o.KhoTQID));
                        var khovn = AccountController.GetByID(Convert.ToInt32(o.KhoVNID));
                        if (kd != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên kinh doanh:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + kd.Username + "</strong></dd>";
                        }
                        if (dathang != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên đặt hàng:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + dathang.Username + "</strong></dd>";
                        }
                        if (khotq != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên kho TQ:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + khotq.Username + "</strong></dd>";
                        }
                        if (khovn != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên Việt Nam:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + khovn.Username + "</strong></dd>";
                        }
                        #endregion
                        #region Lấy thông tin đơn hàng
                        txtMainOrderCode.Text = o.MainOrderCode;

                        //NEW HDK
                        var listMainOrderCode = MainOrderCodeController.GetAllByMainOrderID(o.ID);
                        ListItem ddlitem = new ListItem("Chọn mã đơn hàng", "0");
                        ddlMainOrderCode.Items.Add(ddlitem);
                        if (listMainOrderCode != null)
                        {
                            if (listMainOrderCode.Count > 0)
                            {
                                StringBuilder html = new StringBuilder();
                                foreach (var item in listMainOrderCode)
                                {
                                    ListItem listitem = new ListItem(item.MainOrderCode, item.ID.ToString());
                                    ddlMainOrderCode.Items.Add(listitem);
                                    html.Append("<div class=\"row order-wrap\">");
                                    html.Append("    <div class=\"input-field col s10 m11 MainOrderInPut\">");
                                    html.Append("        <input type=\"text\" class=\"MainOrderCode\"  data-orderCodeID=\"" + item.ID + "\"  onkeypress=\"myFunction($(this))\" value=\"" + item.MainOrderCode + "\">");
                                    html.Append("       <span class=\"helper-text hide\" style=\"position:absolute;\">");
                                    html.Append("       <label style=\"color:green\">Đã cập nhật</label>");
                                    html.Append("       </span>");
                                    html.Append("    </div>");
                                    if (RoleID == 0)
                                    {
                                        html.Append("    <a href=\"javascript:;\" onclick=\"deleteMVD($(this))\" style=\"line-height:80px;position:absolute\" class=\"remove-order tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class=\"material-icons\">remove_circle</i></a>");
                                    }
                                    html.Append("</div>");
                                }
                                lrtMainOrderCode.Text = html.ToString();
                            }
                        }

                        chkCheck.Value = o.IsCheckProduct.ToString().ToBool().ToString();
                        chkPackage.Value = o.IsPacked.ToString().ToBool().ToString();
                        chkShiphome.Value = o.IsFastDelivery.ToString().ToBool().ToString();
                        hdfIsInsurrance.Value = Convert.ToBoolean(o.IsInsurrance).ToString();
                        //chkIsFast.Checked = o.IsFast.ToString().ToBool();
                        double feeeinwarehouse = 0;
                        if (o.FeeInWareHouse != null)
                            feeeinwarehouse = Convert.ToDouble(o.FeeInWareHouse);
                        rFeeWarehouse.Text = Math.Round(feeeinwarehouse, 0).ToString();

                        if (o.IsGiaohang != null)
                        {
                            chkIsGiaohang.Value = o.IsGiaohang.ToString();
                        }
                        else
                        {
                            chkIsGiaohang.Value = "false";
                        }

                        if (!string.IsNullOrEmpty(o.AmountDeposit))
                        {
                            double amountdeposit = Math.Round(Convert.ToDouble(o.AmountDeposit.ToString()), 0);
                            pAmountDeposit.Text = string.Format("{0:N0}", amountdeposit);
                        }
                        else
                        {
                            pAmountDeposit.Text = "0";
                        }
                        if (!string.IsNullOrEmpty(o.TotalPriceReal))
                            rTotalPriceReal.Text = string.Format("{0:N0}", Math.Round(Convert.ToDouble(o.TotalPriceReal)));
                        else
                            rTotalPriceReal.Text = "0";

                        if (!string.IsNullOrEmpty(o.TotalPriceRealCYN))
                            rTotalPriceRealCYN.Text = Math.Round(Convert.ToDouble(o.TotalPriceRealCYN), 2).ToString();
                        else
                            rTotalPriceRealCYN.Text = "0";

                        ddlStatus.SelectedValue = o.Status.ToString();
                        if (!string.IsNullOrEmpty(o.Deposit))
                            pDeposit.Text = string.Format("{0:N0}", Math.Round(Convert.ToDouble(o.Deposit)));

                        double fscn = 0;
                        if (!string.IsNullOrEmpty(o.FeeShipCN))
                        {
                            fscn = Math.Floor(Convert.ToDouble(o.FeeShipCN));
                            pCNShipFeeNDT.Text = (fscn / currency1).ToString();
                            pCNShipFee.Text = string.Format("{0:N0}", Convert.ToDouble(fscn));
                        }

                        double realprice = 0;
                        if (!string.IsNullOrEmpty(o.TotalPriceReal))
                            realprice = Convert.ToDouble(o.TotalPriceReal);

                        txtCurrency.Text = Convert.ToDouble(o.CurrentCNYVN).ToString();

                        double tot = Convert.ToDouble(o.PriceVND) + fscn - realprice;
                        double totCYN = tot / currency1;
                        pHHCYN.Text = totCYN.ToString();
                        pHHVND.Text = string.Format("{0:N0}", Convert.ToDouble(tot));

                        if (!string.IsNullOrEmpty(o.FeeWeight))
                        {
                            pWeight.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeWeight));
                        }
                        else
                        {
                            pWeight.Text = "0";
                        }
                        if (!string.IsNullOrEmpty(o.TQVNWeight))
                        {
                            pWeightNDT.Text = Convert.ToDouble(o.TQVNWeight).ToString();
                        }
                        else
                        {
                            pWeightNDT.Text = "0";
                        }

                        if (!string.IsNullOrEmpty(o.PercentBuyPro))
                        {
                            pBuyNDT.Text = o.PercentBuyPro.ToString();
                        }
                        else
                        {
                            pBuyNDT.Text = "0";
                        }

                        if (!string.IsNullOrEmpty(o.InsurancePercent))
                        {
                            txtPercentInsurance.Text = o.InsurancePercent.ToString();
                        }
                        else
                        {
                            txtPercentInsurance.Text = "0";
                        }

                        if (!string.IsNullOrEmpty(o.FeeBuyPro))
                        {
                            pBuy.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeBuyPro));
                        }
                        else
                        {
                            pBuyNDT.Text = "0";
                        }

                        txtStaffNote.Text = o.StaffNote;

                        double checkproductprice = Convert.ToDouble(o.IsCheckProductPrice);
                        pCheck.Text = string.Format("{0:N0}", checkproductprice);
                        pCheckNDT.Text = (checkproductprice / currency).ToString();

                        double packagedprice = Convert.ToDouble(o.IsPackedPrice);
                        pPacked.Text = string.Format("{0:N0}", packagedprice);
                        pPackedNDT.Text = (packagedprice / currency).ToString();

                        double InsuranceMoney = Convert.ToDouble(o.InsuranceMoney);
                        txtInsuranceMoney.Text = string.Format("{0:N0}", InsuranceMoney);

                        if (!string.IsNullOrEmpty(o.IsFastDeliveryPrice))
                        {
                            pShipHome.Text = string.Format("{0:N0}", Convert.ToDouble(o.IsFastDeliveryPrice));
                        }
                        else
                        {
                            pShipHome.Text = "0";
                        }

                        lblTotalMoneyVND1.Text = string.Format("{0:N0}", Convert.ToDouble(o.PriceVND));
                        lblTotalMoneyCNY1.Text = string.Format("{0:#.##}", Convert.ToDouble(o.PriceVND) / currency);

                        string orderweightfeedc = o.FeeWeightCK;

                        ddlWarehouseFrom.SelectedValue = o.FromPlace.ToString();
                        hdfFromPlace.Value = o.FromPlace.ToString();

                        ddlReceivePlace.SelectedValue = o.ReceivePlace;
                        hdfReceivePlace.Value = o.ReceivePlace;

                        ddlShippingType.SelectedValue = o.ShippingType.ToString();
                        hdfShippingType.Value = o.ShippingType.ToString();

                        if (string.IsNullOrEmpty(orderweightfeedc))
                        {
                            lblCKFeeweightPrice.Text = "0";
                            hdfFeeweightPriceDiscount.Value = "0";
                        }
                        else
                        {
                            lblCKFeeweightPrice.Text = orderweightfeedc;
                            hdfFeeweightPriceDiscount.Value = orderweightfeedc;
                        }
                        double alltotal = Math.Round(Convert.ToDouble(o.TotalPriceVND), 0);
                        ltrlblAllTotal1.Text = string.Format("{0:N0}", alltotal);
                        lblDeposit1.Text = string.Format("{0:N0}", Convert.ToDouble(o.Deposit));
                        lblLeftPay1.Text = string.Format("{0:N0}", alltotal - Convert.ToDouble(o.Deposit));
                        #endregion
                        #region Lấy thông tin nhận hàng
                        StringBuilder customerInfo2 = new StringBuilder();
                        if (RoleID == 3)
                        {
                            //ltr_AddressReceive.Text = "Tài khoản không đủ quyền xem thông tin này";
                            customerInfo2.Append("<span>Tài khoản không đủ quyền xem thông tin này</span>");
                        }
                        else
                        {
                            //ltr_AddressReceive.Text += "<dt>Tên:</dt>";
                            //ltr_AddressReceive.Text += "<dd>" + o.FullName + "</dd>";
                            //ltr_AddressReceive.Text += "<dt>Địa chỉ:</dt>";
                            //ltr_AddressReceive.Text += "<dd>" + o.Address + "</dd>";
                            //ltr_AddressReceive.Text += "<dt>Email:</dt>";
                            //ltr_AddressReceive.Text += "<dd><a href=\"" + o.Email + "\">" + o.Email + "</a></dd>";
                            //ltr_AddressReceive.Text += "<dt>Số dt:</dt>";
                            //ltr_AddressReceive.Text += "<dd><a href=\"tel:+" + o.Phone + "\">" + o.Phone + "</a></dd>";
                            ////ltr_AddressReceive.Text += "<dt>Ghi chú:</dt>";
                            ////ltr_AddressReceive.Text += "<dd>" + o.Note + "</dd>";

                            customerInfo2.Append("<table class=\"table\">");
                            customerInfo2.Append("    <tbody>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Tên</td>");
                            customerInfo2.Append("            <td>" + o.FullName + "</td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Địa chỉ</td>");
                            customerInfo2.Append("            <td>" + o.Address + "</td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Email</td>");
                            customerInfo2.Append("            <td><a href=\"" + o.Email + "\">" + o.Email + "</a></td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Sô´ ÐT</td>");
                            customerInfo2.Append("            <td><a href=\"tel:+" + o.Phone + "\">" + o.Phone + "</a></td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Ghi chú</td>");
                            customerInfo2.Append("            <td>" + o.Note + "</td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("    </tbody>");
                            customerInfo2.Append("</table>");
                        }
                        ltr_AddressReceive.Text = customerInfo2.ToString();
                        #endregion
                        #region Lấy sản phẩm
                        int totalproduct = 0;
                        List<tbl_Order> lo = new List<tbl_Order>();
                        lo = OrderController.GetByMainOrderID(o.ID);
                        if (lo.Count > 0)
                        {
                            int stt = 1;
                            StringBuilder html = new StringBuilder();
                            foreach (var item in lo)
                            {
                                double currentcyt = Convert.ToDouble(item.CurrentCNYVN);
                                double price = 0;
                                double pricepromotion = Convert.ToDouble(item.price_promotion);
                                double priceorigin = Convert.ToDouble(item.price_origin);
                                if (pricepromotion > 0)
                                {
                                    if (priceorigin > pricepromotion)
                                    {
                                        price = pricepromotion;
                                    }
                                    else
                                    {
                                        price = priceorigin;
                                    }
                                }
                                else
                                {
                                    price = priceorigin;
                                }
                                double vndprice = price * currentcyt;
                                html.Append("<div class=\"item-wrap\">");
                                html.Append("    <div class=\"item-name\">");
                                html.Append("        <div class=\"number\">");
                                html.Append("            <span class=\"count\">" + stt + "</span>");
                                html.Append("        </div>");
                                html.Append("        <div class=\"name\">");
                                html.Append("            <span class=\"item-img\">");
                                html.Append("                <a href=\"" + item.link_origin + "\" target=\"_blank\" ><img src=\"" + item.image_origin + "\" alt=\"image\"></a>");
                                html.Append("            </span>");
                                html.Append("            <div class=\"caption\">");
                                html.Append("                <a href=\"" + item.link_origin + "\" target=\"_blank\" class=\"title black-text\">" + item.title_origin + "</a>");
                                html.Append("                <div class=\"item-price mt-1\">");
                                html.Append("                    <span class=\"pr-2 black-text font-weight-600\">Thuộc tính: </span><span class=\"pl-2 black-text font-weight-600\">" + item.property + "</span>");
                                html.Append("                </div>");
                                html.Append("                <div class=\"note\">");
                                html.Append("                    <span class=\"black-text font-weight-500\">Ghi chú: </span>");
                                html.Append("                    <div class=\"input-field inline\">");
                                html.Append("                        <input type=\"text\" value=\"" + item.brand + "\" data-id=\"" + item.ID + "\" class=\"validate brand\">");
                                html.Append("                    </div>");
                                html.Append("                </div>");
                                html.Append("            </div>");
                                html.Append("        </div>");
                                html.Append("    </div>");
                                html.Append("    <div class=\"item-info\">");
                                html.Append("        <div class=\"item-num column\">");
                                html.Append("            <span class=\"black-text\"><strong>Số lượng</strong></span>");
                                html.Append("            <p>" + item.quantity + "</p>");
                                html.Append("            <p></p>");
                                html.Append("        </div>");
                                html.Append("        <div class=\"item-price column\">");
                                html.Append("            <span class=\"black-text\"><strong>Đơn giá</strong></span>");
                                html.Append("            <p class=\"grey-text font-weight-500\">¥" + string.Format("{0:0.##}", price) + "</p>");
                                html.Append("            <p class=\"grey-text font-weight-500\">" + string.Format("{0:N0}", vndprice) + " VNÐ</p>");
                                html.Append("        </div>");
                                html.Append("        <div class=\"item-status column\">");
                                html.Append("            <span class=\"black-text\"><strong>Trạng thái</strong></span>");
                                if (string.IsNullOrEmpty(item.ProductStatus.ToString()))
                                {
                                    html.Append("            <p class=\"green-text\">Còn hàng</p>");
                                }
                                else
                                {
                                    if (item.ProductStatus == 2)
                                        html.Append("            <p class=\"red-text\">Hết hàng</p>");
                                    else
                                        html.Append("            <p class=\"green-text\">Còn hàng</p>");
                                }
                                html.Append("        </div>");
                                html.Append("        <div class=\"delete\">");
                                html.Append("            <a href=\"/manager/ProductEdit.aspx?id=" + item.ID + "\" class=\"btn-update tooltipped\" data-position=\"top\" data-tooltip=\"Sửa\"><i class=\"material-icons\">edit</i></a>");
                                html.Append("        </div>");
                                html.Append("    </div>");
                                html.Append("</div>");
                                totalproduct += Convert.ToInt32(item.quantity);
                                stt++;
                            }
                            ltrProducts.Text = html.ToString();
                        }
                        ltrTotalProduct.Text = totalproduct.ToString();
                        #endregion
                        #region Lấy bình luận nội bộ
                        StringBuilder chathtml = new StringBuilder();
                        var cs = OrderCommentController.GetByOrderIDAndType(o.ID, 2);
                        if (cs != null)
                        {
                            if (cs.Count > 0)
                            {
                                foreach (var item in cs)
                                {
                                    string fullname = "";
                                    int role = 0;
                                    int user_postID = 0;
                                    var user = AccountController.GetByID(Convert.ToInt32(item.CreatedBy));
                                    if (user != null)
                                    {
                                        user_postID = user.ID;
                                        role = Convert.ToInt32(user.RoleID);
                                        var userinfo = AccountController.GetByID(user.ID);
                                        if (userinfo != null)
                                        {
                                            fullname = userinfo.Username;

                                        }
                                    }

                                    if (uid == user_postID)
                                    {
                                        //ltrInComment.Text += "<div class=\"mess-item mymess\">";
                                        chathtml.Append("<div class=\"chat chat-right\">");
                                    }
                                    else
                                    {
                                        //ltrInComment.Text += "<div class=\"mess-item \">";
                                        chathtml.Append("<div class=\"chat\">");
                                    }
                                    chathtml.Append("<div class=\"chat-avatar\">");
                                    chathtml.Append("    <p class=\"name\">" + fullname + "</p>");
                                    //chathtml.Append("    <p class=\"role\">"+RoleController.GetByID(user.RoleID.Value).RoleName+"</p>");
                                    chathtml.Append("</div>");
                                    chathtml.Append("<div class=\"chat-body\">");
                                    chathtml.Append("        <div class=\"chat-text\">");
                                    chathtml.Append("                <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</div>");
                                    chathtml.Append("                <div class=\"text-content\">");
                                    chathtml.Append("                    <div class=\"content\">");
                                    if (!string.IsNullOrEmpty(item.Link))
                                    {
                                        chathtml.Append("<div class=\"content-img\">");
                                        //if (uid == user_postID)
                                        //{
                                        //    chathtml.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        //    //ltrInComment.Text += "<div class=\"mess-item mymess\">";

                                        //}
                                        //else
                                        //{
                                        //    //ltrInComment.Text += "<div class=\"mess-item \">";
                                        //    chathtml.Append("<div class=\"content-img\">");
                                        //}
                                        chathtml.Append("   <div class=\"img-block\">");
                                        if (item.Link.Contains(".doc"))
                                        {
                                            chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (item.Link.Contains(".xls"))
                                        {
                                            chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title =\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }

                                        //chathtml.Append("       <img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"materialboxed\" height=\"50\"/>");
                                        chathtml.Append("   </div>");
                                        chathtml.Append("</div>");
                                    }
                                    else
                                    {
                                        chathtml.Append("                    <p>" + item.Comment + "</p>");
                                    }
                                    chathtml.Append("                    </div>");
                                    chathtml.Append("                </div>");
                                    chathtml.Append("        </div>");
                                    chathtml.Append("</div>");
                                    chathtml.Append("</div>");
                                }
                            }
                            else
                            {
                                //chathtml.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                            }
                        }
                        else
                        {

                            //chathtml.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                        }
                        ltrInComment.Text = chathtml.ToString();
                        #endregion
                        #region Lấy bình luận ngoài
                        StringBuilder chathtml2 = new StringBuilder();
                        var cs1 = OrderCommentController.GetByOrderIDAndType(o.ID, 1);
                        if (cs1 != null)
                        {
                            if (cs1.Count > 0)
                            {
                                foreach (var item in cs1)
                                {
                                    string fullname = "";

                                    int role = 0;
                                    int user_postID = 0;
                                    var user = AccountController.GetByID(Convert.ToInt32(item.CreatedBy));
                                    if (user != null)
                                    {
                                        user_postID = user.ID;
                                        role = Convert.ToInt32(user.RoleID);
                                        var userinfo = AccountController.GetByID(user.ID);
                                        if (userinfo != null)
                                        {
                                            fullname = userinfo.Username;
                                        }
                                    }
                                    if (uid == user_postID)
                                    {
                                        //ltrOutComment.Text += "<div class=\"mess-item mymess\">";
                                        chathtml2.Append("<div class=\"chat chat-right\">");
                                    }
                                    else
                                    {
                                        //ltrOutComment.Text += "<div class=\"mess-item \">";
                                        chathtml2.Append("<div class=\"chat\">");
                                    }
                                    chathtml2.Append("<div class=\"chat-avatar\">");
                                    chathtml2.Append("    <p class=\"name\">" + fullname + "</p>");
                                    //chathtml2.Append("    <p class=\"role\">" + RoleController.GetByID(user.RoleID.Value).RoleName + "</p>");
                                    chathtml2.Append("</div>");
                                    chathtml2.Append("<div class=\"chat-body\">");
                                    chathtml2.Append("        <div class=\"chat-text\">");
                                    chathtml2.Append("                <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</div>");
                                    chathtml2.Append("                <div class=\"text-content\">");
                                    chathtml2.Append("                    <div class=\"content\">");
                                    if (!string.IsNullOrEmpty(item.Link))
                                    {
                                        chathtml2.Append("<div class=\"content-img\">");
                                        //if (uid == user_postID)
                                        //{
                                        //    chathtml2.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        //    //ltrInComment.Text += "<div class=\"mess-item mymess\">";

                                        //}
                                        //else
                                        //{
                                        //    //ltrInComment.Text += "<div class=\"mess-item \">";
                                        //    chathtml2.Append("<div class=\"content-img\">");
                                        //}
                                        chathtml2.Append("<div class=\"img-block\">");
                                        if (item.Link.Contains(".doc"))
                                        {
                                            chathtml2.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (item.Link.Contains(".xls"))
                                        {
                                            chathtml2.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            chathtml2.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }                                        //chathtml2.Append("<img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"materialboxed\" height=\"50\"/>");
                                        chathtml2.Append("</div>");
                                        chathtml2.Append("</div>");
                                    }
                                    else
                                    {
                                        chathtml2.Append("                    <p>" + item.Comment + "</p>");
                                    }
                                    chathtml2.Append("                    </div>");
                                    chathtml2.Append("                </div>");
                                    chathtml2.Append("        </div>");
                                    chathtml2.Append("</div>");
                                    chathtml2.Append("</div>");

                                }
                            }
                            else
                            {
                                //ltrOutComment.Text += "<span class=\"no-comment-cust\">Hiện chưa có đánh giá nào.</span>";
                                //chathtml2.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                            }
                        }
                        else
                        {
                            //ltrOutComment.Text += "<span class=\"no-comment-cust\">Hiện chưa có đánh giá nào.</span>";
                            //chathtml2.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                        }
                        ltrOutComment.Text = chathtml2.ToString();
                        #endregion
                        #region Lấy danh sách bao nhỏ
                        StringBuilder spsList = new StringBuilder();
                        var smallpackages = SmallPackageController.GetByMainOrderID(id);
                        if (smallpackages.Count > 0)
                        {
                            foreach (var s in smallpackages)
                            {
                                int status = Convert.ToInt32(s.Status);
                                double quidoi = 0;
                                double pDai = Convert.ToDouble(s.Length);
                                double pRong = Convert.ToDouble(s.Width);
                                double pCao = Convert.ToDouble(s.Height);

                                if (pDai > 0 && pRong > 0 && pCao > 0)
                                    quidoi = Math.Round((pDai * pRong * pCao) / 6000, 0);

                                double cantinhtien = quidoi;
                                if (Convert.ToDouble(s.Weight) > quidoi)
                                    cantinhtien = Math.Round(Convert.ToDouble(s.Weight), 0);


                                spsList.Append("            <tr class=\"ordercode order-versionnew\" data-packageID=\"" + s.ID + "\">");
                                spsList.Append("                <td>");
                                spsList.Append("                    <input class=\"transactionCode\" type=\"text\" value=\"" + s.OrderTransactionCode + "\"></td>");
                                spsList.Append("                <td style=\"width:7%;\">");
                                spsList.Append("                    <input class=\"transactionWeight\" data-type=\"number\" type=\"text\" value=\"" + s.Weight + "\"></td>");
                                spsList.Append("                <td style=\"width:7%;\">");
                                spsList.Append("                    <input readonly class=\"transactionWeightExchange\" data-type=\"number\" type=\"text\" value=\"" + quidoi + "\"></td>");
                                spsList.Append("                <td style=\"width:7%;\">");
                                spsList.Append("                    <input readonly class=\"transactionWeightPayable\" data-type=\"number\" type=\"text\" value=\"" + cantinhtien + "\"></td>");
                                spsList.Append("                <td style=\"width:10%;\">");
                                spsList.Append("                    <input readonly class=\"transactionVolume\" data-type=\"number\" type=\"text\" value=\"" + pDai + " x " + pRong + " x " + pCao + "\"></td>");
                                spsList.Append("                <td>");
                                spsList.Append("                    <div class=\"input-field\">");
                                spsList.Append("                        <select class=\"transactionCodeMainOrderCode\">");

                                var ListMainOrderCode = MainOrderCodeController.GetAllByMainOrderID(o.ID);
                                if (ListMainOrderCode != null)
                                {

                                    var mainOrderCode = MainOrderCodeController.GetByID(Convert.ToInt32(s.MainOrderCodeID));
                                    if (mainOrderCode != null)
                                    {
                                        spsList.Append("            <option value=\"0\">Chọn mã đơn hàng</option>");
                                        foreach (var item in ListMainOrderCode)
                                        {
                                            if (mainOrderCode.MainOrderCode == item.MainOrderCode)
                                            {
                                                spsList.Append("            <option value=\"" + item.ID + "\" selected>" + item.MainOrderCode + "</option>");
                                            }
                                            else
                                            {
                                                spsList.Append("            <option value=\"" + item.ID + "\">" + item.MainOrderCode + "</option>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        spsList.Append("            <option value=\"0\">Chọn mã đơn hàng</option>");
                                        foreach (var item in ListMainOrderCode)
                                        {
                                            spsList.Append("            <option value=\"" + item.ID + "\">" + item.MainOrderCode + "</option>");
                                        }
                                    }
                                }
                                else
                                {
                                    spsList.Append("            <option value=\"0\">Chọn mã đơn hàng</option>");
                                }

                                spsList.Append("                        </select>");
                                spsList.Append("                    </div>");
                                spsList.Append("                </td>");

                                spsList.Append("                <td>");
                                spsList.Append("                    <div class=\"input-field\">");
                                spsList.Append("                        <select class=\"transactionCodeStatus\">");

                                if (status == 1)
                                    spsList.Append("            <option value=\"1\" selected>Chưa về kho TQ</option>");
                                else
                                    spsList.Append("            <option value=\"1\">Chưa về kho TQ</option>");

                                if (status == 5)
                                    spsList.Append("            <option value=\"5\" selected>Shop phát hàng</option>");
                                else
                                    spsList.Append("            <option value=\"5\">Shop phát hàng</option>");

                                if (status == 2)
                                    spsList.Append("            <option value=\"2\" selected>Hàng về kho TQ</option>");
                                else
                                    spsList.Append("            <option value=\"2\">Hàng về kho TQ</option>");

                                if (status == 3)
                                    spsList.Append("            <option value=\"3\" selected>Hàng về Việt Nam</option>");
                                else
                                    spsList.Append("            <option value=\"3\">Hàng về Việt Nam</option>");

                                if (status == 4)
                                    spsList.Append("            <option value=\"4\" selected>Đã giao khách hàng</option>");
                                else
                                    spsList.Append("            <option value=\"4\">Đã giao khách hàng</option>");

                                if (status == 0)
                                    spsList.Append("            <option value=\"0\" selected>Đã hủy</option>");
                                else
                                    spsList.Append("            <option value=\"0\">Đã hủy</option>");

                                spsList.Append("                        </select>");
                                spsList.Append("                    </div>");
                                spsList.Append("                </td>");


                                spsList.Append("                <td>");
                                spsList.Append("                    <input class=\"transactionDescription\" type=\"text\" value=\"" + s.Description + "\"></td>");
                                spsList.Append("                </td>");
                                spsList.Append("            <td class=\"\">");
                                spsList.Append("                <a href='javascript:;' onclick=\"deleteOrderCode($(this))\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class=\"material-icons valign-center\">remove_circle</i></a>");
                                spsList.Append("            </td>");
                                spsList.Append("            </tr>");
                            }
                            ltrCodeList.Text = spsList.ToString();
                        }
                        #endregion
                        #region Lấy danh sách phụ phí
                        var listsp = FeeSupportController.GetAllByMainOrderID(o.ID);
                        if (listsp.Count > 0)
                        {
                            foreach (var item in listsp)
                            {
                                ltrFeeSupport.Text += "<tr class=\"feesupport fee-versionnew\" data-feesupportid=\"" + item.ID + "\">";
                                ltrFeeSupport.Text += "<td><input class=\"feesupportname\" type=\"text\" value=\"" + item.SupportName + "\"></td>";
                                ltrFeeSupport.Text += "<td><input class=\"feesupportvnd\" type=\"text\" value=\"" + item.SupportInfoVND + "\"></td>";
                                ltrFeeSupport.Text += "<td class=\"\"><a href=\"javascript:;\" onclick=\"deleteSupportFee($(this))\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class=\"material-icons valign-center\">remove_circle</i></a></td>";
                                ltrFeeSupport.Text += "</tr>";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Response.Redirect("/trang-chu");
                    }
                    StringBuilder hisChange = new StringBuilder();
                    var historyorder = HistoryOrderChangeController.GetByMainOrderID(o.ID);
                    if (historyorder.Count > 0)
                    {
                        foreach (var item in historyorder)
                        {
                            string username = item.Username;
                            string rolename = "admin";
                            var acc = AccountController.GetByUsername(username);
                            if (acc != null)
                            {
                                int role = Convert.ToInt32(acc.RoleID);

                                var r = RoleController.GetByID(role);
                                if (r != null)
                                {
                                    rolename = r.RoleDescription;
                                }
                            }
                            hisChange.Append("<tr>");
                            hisChange.Append("    <td class=\"no-wrap\">" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</td>");
                            hisChange.Append("    <td class=\"no-wrap\">" + username + "</td>");
                            hisChange.Append("    <td class=\"no-wrap\">" + rolename + "</td>");
                            hisChange.Append("    <td>" + item.HistoryContent + "</td>");
                            hisChange.Append("</tr>");
                        }

                    }
                    else
                    {
                        hisChange.Append("<tr class=\"noti\">");
                        hisChange.Append("    <td class=\"red-text\" colspan=\"4\">Không có lịch sử thay đổi nào.</td>");
                        hisChange.Append("</tr>");
                    }
                    //lrtHistoryChange.Text = hisChange.ToString();

                }
            }
        }

        #region Button        
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.UtcNow.AddHours(7);
            if (obj_user != null)
            {
                int uid = obj_user.ID;
                int RoleID = obj_user.RoleID.ToString().ToInt();
                var id = Convert.ToInt32(ViewState["ID"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    var oldStatus = o.Status;
                    if (o != null)
                    {
                        int uidmuahang = Convert.ToInt32(o.UID);
                        string usermuahang = "";
                        var accmuahan = AccountController.GetByID(uidmuahang);
                        if (accmuahan != null)
                            usermuahang = accmuahan.Username;

                        bool ischeckmvd = true;
                        string listmvd = "";

                        #region cập nhật và tạo mới smallpackage
                        string tcl = hdfCodeTransactionList.Value;
                        if (!string.IsNullOrEmpty(tcl))
                        {
                            string[] list = tcl.Split('|');
                            for (int i = 0; i < list.Length - 1; i++)
                            {
                                string[] item = list[i].Split(',');
                                int ID = item[0].ToInt(0);
                                string code = item[1].Trim();
                                string weight = item[2];
                                double weightin = 0;
                                if (!string.IsNullOrEmpty(weight))
                                    weightin = Math.Round(Convert.ToDouble(weight), 1);
                                int smallpackage_status = item[3].ToInt(1);
                                string description = item[4];
                                string mainOrderCodeID = item[5];
                                string weightPayable = item[6];
                                double weightPayableIn = 0;
                                if (!string.IsNullOrEmpty(weightPayable))
                                    weightPayableIn = Math.Round(Convert.ToDouble(weightPayable), 1);

                                var MainOrderCode = MainOrderCodeController.GetByID(mainOrderCodeID.ToInt(0));
                                if (MainOrderCode == null)
                                    PJUtils.ShowMessageBoxSwAlert("Lỗi, không có mã đơn hàng", "e", false, Page);

                                int QuantityBarcode = 0;
                                string ListMVD = "";

                                if (ID > 0)
                                {
                                    var smp = SmallPackageController.GetByID(ID);
                                    if (smp != null)
                                    {
                                        int bigpackageID = Convert.ToInt32(smp.BigPackageID);
                                        bool check = false;
                                        var getsmallcheck = SmallPackageController.GetByOrderCode(code);
                                        if (getsmallcheck.Count > 0)
                                        {
                                            foreach (var sp in getsmallcheck)
                                            {
                                                if (sp.ID == ID)
                                                {
                                                    check = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            check = true;
                                        }
                                        if (check)
                                        {
                                            SmallPackageController.UpdateNew(ID, accmuahan.ID, usermuahang, bigpackageID, code,
                                            smp.ProductType, Math.Round(Convert.ToDouble(smp.FeeShip), 0), weightin, Math.Round(Convert.ToDouble(smp.Volume), 1),
                                            smallpackage_status, description, currentDate, username, mainOrderCodeID.ToInt(0), weightPayableIn);

                                            var List = SmallPackageController.GetByMainOrderID(id);
                                            if (List.Count > 0)
                                            {
                                                foreach (var itemsmp in List)
                                                {
                                                    ListMVD += itemsmp.OrderTransactionCode + " | ";
                                                }
                                                QuantityBarcode = List.Count;
                                                MainOrderController.UpdateBarcode(id, ListMVD);
                                                MainOrderController.UpdateQuantityBarcode(id, QuantityBarcode);
                                            }

                                            if (smallpackage_status == 2)
                                            {
                                                SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                            }
                                            else if (smallpackage_status == 3)
                                            {
                                                SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                            }
                                            var bigpack = BigPackageController.GetByID(bigpackageID);
                                            if (bigpack != null)
                                            {
                                                int TotalPackageWaiting = SmallPackageController.GetCountByBigPackageIDStatus(bigpackageID, 1, 2);
                                                if (TotalPackageWaiting == 0)
                                                {
                                                    BigPackageController.UpdateStatus(bigpackageID, 2, currentDate, username);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var checkbarcode = SmallPackageController.GetByOrderTransactionCode(code);
                                        if (checkbarcode == null)
                                        {
                                            SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(id, accmuahan.ID, usermuahang, 0, code, "", 0,
                                                  weightPayableIn, 0, smallpackage_status, description, currentDate, username, mainOrderCodeID.ToInt(0), 0);

                                            var List = SmallPackageController.GetByMainOrderID(id);
                                            if (List.Count > 0)
                                            {
                                                foreach (var itemsmp in List)
                                                {
                                                    ListMVD += itemsmp.OrderTransactionCode + " | ";
                                                }
                                                QuantityBarcode = List.Count;
                                                MainOrderController.UpdateBarcode(id, ListMVD);
                                                MainOrderController.UpdateQuantityBarcode(id, QuantityBarcode);
                                            }

                                            HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã thêm mã vận đơn của đơn hàng ID là: " + o.ID + ", Mã vận đơn: " + code + ", cân nặng: " + weightPayableIn + "", 8, currentDate);

                                            if (smallpackage_status == 2)
                                            {
                                                SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                            }
                                            else if (smallpackage_status == 3)
                                            {
                                                SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                            }
                                        }
                                        else
                                        {
                                            ischeckmvd = false;
                                            listmvd += code;
                                        }
                                    }
                                }
                                else
                                {
                                    var checkbarcode = SmallPackageController.GetByOrderTransactionCode(code);
                                    if (checkbarcode == null)
                                    {
                                        SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(id, accmuahan.ID, usermuahang, 0, code, "", 0, weightPayableIn, 0,
                                        smallpackage_status, description, currentDate, username, mainOrderCodeID.ToInt(0), 0);

                                        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã thêm mã vận đơn của đơn hàng ID là: " + o.ID + ", Mã vận đơn: " + code + ", cân nặng: " + weightPayableIn + "", 8, currentDate);

                                        var List = SmallPackageController.GetByMainOrderID(id);
                                        if (List.Count > 0)
                                        {
                                            foreach (var itemsmp in List)
                                            {
                                                ListMVD += itemsmp.OrderTransactionCode + " | ";
                                            }
                                            QuantityBarcode = List.Count;
                                            MainOrderController.UpdateBarcode(id, ListMVD);
                                            MainOrderController.UpdateQuantityBarcode(id, QuantityBarcode);
                                        }

                                        if (smallpackage_status == 2)
                                        {
                                            SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                        }
                                        else if (smallpackage_status == 3)
                                        {
                                            SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                        }
                                    }
                                    else
                                    {
                                        ischeckmvd = false;
                                        listmvd += code;
                                    }
                                }
                            }
                        }
                        #endregion

                        if (ischeckmvd == true)
                        {
                            #region Cập nhật và tạo mới phụ phí
                            double TotalFeeSupport = 0;
                            string lsp = hdfListFeeSupport.Value;
                            if (!string.IsNullOrEmpty(lsp))
                            {
                                string[] list = lsp.Split('|');
                                for (int i = 0; i < list.Length - 1; i++)
                                {
                                    string[] item = list[i].Split(',');
                                    int ID = item[0].ToInt(0);
                                    string fname = item[1];
                                    double FeeSupport = Convert.ToDouble(item[2]);
                                    TotalFeeSupport += FeeSupport;
                                    if (ID > 0)
                                    {
                                        var check = FeeSupportController.GetByID(ID);
                                        if (check != null)
                                        {
                                            FeeSupportController.Update(check.ID, fname, FeeSupport.ToString(), obj_user.Username, currentDate);
                                            if (check.SupportName != fname)
                                            {
                                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                                 " đã thay đổi tên phụ phí của đơn hàng ID là: " + o.ID + ", Từ: " + check.SupportName + ", Sang: "
                                                     + fname + "", 10, currentDate);
                                            }

                                            if (check.SupportInfoVND != FeeSupport.ToString())
                                            {
                                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                                 " đã thay đổi tiền phụ phí của đơn hàng ID là: " + o.ID + ", Tên phụ phí: " + fname + ", Số tiền từ: "
                                                      + string.Format("{0:N0}", Convert.ToDouble(check.SupportInfoVND)) + ", Sang: "
                                                      + string.Format("{0:N0}", Convert.ToDouble(FeeSupport)) + "", 10, currentDate);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        FeeSupportController.Insert(o.ID, fname, FeeSupport.ToString(), obj_user.Username, currentDate);
                                        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                           " đã thêm phụ phí của đơn hàng ID là: " + o.ID + ", Tên phụ phí: " + fname + ", Số tiền: "
                                           + string.Format("{0:N0}", Convert.ToDouble(FeeSupport)) + "", 10, currentDate);
                                    }
                                }
                            }
                            #endregion
                            #region Lấy ra text của trạng thái đơn hàng
                            string orderstatus = "";
                            int currentOrderStatus = Convert.ToInt32(o.Status);
                            switch (currentOrderStatus)
                            {
                                case 0:
                                    orderstatus = "Mới tạo";
                                    break;
                                case 1:
                                    orderstatus = "Hủy đơn hàng";
                                    break;
                                case 2:
                                    orderstatus = "Đã đặt cọc";
                                    break;
                                case 4:
                                    orderstatus = "Đã mua hàng";
                                    break;
                                case 5:
                                    orderstatus = "Shop phát hàng";
                                    break;
                                case 6:
                                    orderstatus = "Hàng về kho TQ";
                                    break;
                                case 7:
                                    orderstatus = "Hàng về kho VN";
                                    break;
                                case 8:
                                    orderstatus = "Chờ thanh toán";
                                    break;
                                case 9:
                                    orderstatus = "Khách đã thanh toán";
                                    break;
                                case 10:
                                    orderstatus = "Đã hoàn thành";
                                    break;
                                default:
                                    break;
                            }
                            #endregion
                            #region Cập nhật nhân viên KhoTQ và nhân viên KhoVN
                            if (RoleID == 4)
                            {
                                if (o.KhoTQID == uid || o.KhoTQID == 0)
                                {
                                    MainOrderController.UpdateStaff(o.ID, o.SalerID.ToString().ToInt(0), o.DathangID.ToString().ToInt(0), uid, o.KhoVNID.ToString().ToInt(0));
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý", "e", true, Page);
                                }
                            }
                            else if (RoleID == 5)
                            {
                                if (o.KhoVNID == uid || o.KhoTQID == 0)
                                {
                                    MainOrderController.UpdateStaff(o.ID, o.SalerID.ToString().ToInt(0), o.DathangID.ToString().ToInt(0), o.KhoTQID.ToString().ToInt(0), uid);
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý", "e", true, Page);
                                }
                            }
                            #endregion
                            #region cập nhật thông tin của đơn hàng                     
                            int currentstt = Convert.ToInt32(o.Status);
                            int status = ddlStatus.SelectedValue.ToString().ToInt();
                            if (status == 1)
                            {
                                double Deposit = 0;
                                if (o.Deposit.ToFloat(0) > 0)
                                    Deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);
                                if (Deposit > 0)
                                {
                                    var user_order = AccountController.GetByID(o.UID.ToString().ToInt());
                                    if (user_order != null)
                                    {
                                        double wallet = 0;
                                        if (user_order.Wallet.ToString().ToFloat(0) > 0)
                                            wallet = Math.Round(Convert.ToDouble(user_order.Wallet), 0);
                                        wallet = wallet + Deposit;
                                        HistoryPayWalletController.Insert(user_order.ID, user_order.Username, o.ID, Deposit,
                                        "Đơn hàng: " + o.ID + " bị hủy và hoàn tiền cọc cho khách.", wallet, 2, 2, currentDate, obj_user.Username);
                                        AccountController.updateWallet(user_order.ID, wallet, currentDate, obj_user.Username);
                                        MainOrderController.UpdateDeposit(o.ID, "0");
                                    }
                                }
                                MainOrderController.UpdateStatusByID(o.ID, Convert.ToInt32(ddlStatus.SelectedValue));

                                //Fix đổi trạng thái
                                //var smallpackages = SmallPackageController.GetAllByMainOrderID(o.ID);
                                //if (smallpackages.Count > 0)
                                //{
                                //    int minSTT = smallpackages.ElementAt(0).Status.Value;
                                //    for (int i = 1; i < smallpackages.Count; i++)
                                //    {
                                //        if (smallpackages.ElementAt(i).Status.Value < minSTT && smallpackages.ElementAt(i).Status.Value !=4)
                                //            minSTT = smallpackages.ElementAt(i).Status.Value;
                                //    }

                                //    var mainOrderSTTEdit = Convert.ToInt32(ddlStatus.SelectedValue);
                                //    if (minSTT == 1 && mainOrderSTTEdit < 6 && mainOrderSTTEdit != 5)
                                //    {
                                //        MainOrderController.UpdateStatusByID(o.ID, Convert.ToInt32(ddlStatus.SelectedValue));
                                //    }
                                //    else if (minSTT == 5)
                                //    {
                                //        MainOrderController.UpdateStatusByID(o.ID, 5);
                                //        MainOrderController.UpdateDateDelivery(o.ID, currentDate);
                                //    }
                                //    else if (minSTT == 2)
                                //    {
                                //        if (Convert.ToInt32(ddlStatus.SelectedValue) == 3)
                                //        {
                                //            MainOrderController.UpdateStatusByID(o.ID, 3);
                                //            MainOrderController.UpdateDateLoading(o.ID, currentDate);
                                //        }
                                //        else
                                //        {
                                //            MainOrderController.UpdateStatusByID(o.ID, 6);
                                //            MainOrderController.UpdateDateTQ(o.ID, currentDate);
                                //        }
                                //    }
                                //    else if (minSTT == 3)
                                //    {
                                //        if (Convert.ToInt32(ddlStatus.SelectedValue) == 9)
                                //        {
                                //            MainOrderController.UpdateStatusByID(o.ID, 9);
                                //            MainOrderController.UpdatePayDate(o.ID, currentDate);
                                //        }
                                //        else
                                //        {
                                //            MainOrderController.UpdateStatusByID(o.ID, 7);
                                //            MainOrderController.UpdateDateVN(o.ID, currentDate);
                                //        }
                                //    }
                                //    else if (minSTT == 4)
                                //    {
                                //        MainOrderController.UpdateStatusByID(o.ID, 10);
                                //        MainOrderController.UpdateCompleteDate(o.ID, currentDate);
                                //    }
                                //}
                                //else
                                //{
                                //    if (Convert.ToInt32(ddlStatus.SelectedValue) < 6)
                                //    {
                                //        MainOrderController.UpdateStatusByID(o.ID, Convert.ToInt32(ddlStatus.SelectedValue));
                                //    }
                                //}

                                if (status != currentstt)
                                {
                                    OrderCommentController.Insert(id, "Đã có cập nhật mới cho đơn hàng #" + id + " của bạn.", true, 1, DateTime.UtcNow.AddHours(7), uid);
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi trạng thái của đơn hàng ID là: " + o.ID + ", từ: " + orderstatus + ", sang: " + ddlStatus.SelectedItem + "", 0, currentDate);
                                }
                            }
                            else
                            {
                                double CurrentOrderWeight = 0;
                                if (o.TQVNWeight.ToFloat(0) > 0)
                                    CurrentOrderWeight = Math.Round(Convert.ToDouble(o.TQVNWeight), 2);

                                double OCurrent_FeeWeight = 0;
                                if (o.FeeWeight.ToFloat(0) > 0)
                                    OCurrent_FeeWeight = Math.Round(Convert.ToDouble(o.FeeWeight), 0);

                                double OCurrent_FeeBuyPro = 0;
                                if (o.FeeBuyPro.ToFloat(0) > 0)
                                    OCurrent_FeeBuyPro = Math.Round(Convert.ToDouble(o.FeeBuyPro), 2);

                                double Current_Percent = 0;
                                if (o.PercentBuyPro.ToFloat(0) > 0)
                                    Current_Percent = Math.Round(Convert.ToDouble(o.PercentBuyPro), 2);

                                double OCurrent_TotalPriceReal = 0;
                                if (o.TotalPriceReal.ToFloat(0) > 0)
                                    OCurrent_TotalPriceReal = Math.Round(Convert.ToDouble(o.TotalPriceReal), 0);

                                double OCurrent_FeeShipCN = 0;
                                if (o.FeeShipCN.ToFloat(0) > 0)
                                    OCurrent_FeeShipCN = Math.Round(Convert.ToDouble(o.FeeShipCN), 0);

                                double OCurrent_deposit = 0;
                                if (o.Deposit.ToFloat(0) > 0)
                                    OCurrent_deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);

                                double CurrentAmountDeposit = 0;
                                if (o.AmountDeposit.ToFloat(0) > 0)
                                    CurrentAmountDeposit = Math.Round(Convert.ToDouble(o.AmountDeposit), 0);

                                double OCurrent_IsCheckProductPrice = 0;
                                if (o.IsCheckProductPrice.ToFloat(0) > 0)
                                    OCurrent_IsCheckProductPrice = Math.Round(Convert.ToDouble(o.IsCheckProductPrice), 0);

                                double OCurrent_IsPackedPrice = 0;
                                if (o.IsPackedPrice.ToFloat(0) > 0)
                                    OCurrent_IsPackedPrice = Math.Round(Convert.ToDouble(o.IsPackedPrice), 0);

                                double OCurrent_IsFastDeliveryPrice = 0;
                                if (o.IsFastDeliveryPrice.ToFloat(0) > 0)
                                    OCurrent_IsFastDeliveryPrice = Math.Round(Convert.ToDouble(o.IsFastDeliveryPrice), 0);

                                double Current_Insurance = 0;
                                if (o.InsurancePercent.ToFloat(0) > 0)
                                    Current_Insurance = Math.Round(Convert.ToDouble(o.InsurancePercent), 2);

                                double PriceVND = 0;
                                if (o.PriceVND.ToFloat(0) > 0)
                                    PriceVND = Math.Round(Convert.ToDouble(o.PriceVND), 0);

                                double FeeBuyPro = Math.Round(Convert.ToDouble(pBuy.Text), 0);
                                double PercentService = Math.Round(Convert.ToDouble(pBuyNDT.Text), 2);
                                if (Current_Percent != PercentService)
                                {
                                    FeeBuyPro = Math.Round(PriceVND * PercentService / 100, 0);
                                    MainOrderController.UpdatePercentFeeBuyPro(o.ID, PercentService.ToString());
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi tiền phí mua hàng của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_FeeBuyPro) + "," +
                                    " sang: " + string.Format("{0:N0}", FeeBuyPro) + "", 3, currentDate);
                                }
                                double InsurrancePercent = Math.Round(Convert.ToDouble(txtPercentInsurance.Text), 2);
                                if (Current_Insurance != InsurrancePercent)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                   " đã đổi phần trăm bảo hiểm của đơn hàng ID là: " + o.ID + ", từ " + Current_Insurance + "% sang " + InsurrancePercent + "%", 2, currentDate);
                                }


                                double AmountDeposit = Math.Round(Convert.ToDouble(pAmountDeposit.Text), 0);
                                double Deposit = Math.Round(Convert.ToDouble(pDeposit.Text), 0);
                                double FeeShipCN = Math.Round(Convert.ToDouble(pCNShipFee.Text), 0);
                                //double OrderWeight = Math.Round(Convert.ToDouble(pWeightNDT.Text), 2);
                                double OrderWeight = 0;
                                //Lấy danh sách smallpackage của MainOrderId
                                double FeeWeight = Math.Round(Convert.ToDouble(pWeight.Text), 0);

                                var smallPackages = SmallPackageController.GetAllByMainOrderID(id);
                                foreach (var item in smallPackages)
                                {
                                    double canQuiDoi = 0;
                                    int packageType = 0;
                                    if (item.IsHelpMoving != null)
                                    {
                                        if (item.IsHelpMoving.Value)
                                            packageType = 1;
                                    }
                                    if (item.Width != null && item.Height != null && item.Length != null)
                                        canQuiDoi = (item.Width.Value * item.Height.Value * item.Length.Value) / 6000;
                                    if (item.Weight != null)
                                    {
                                        if (canQuiDoi > item.Weight.Value)
                                        {
                                            OrderWeight += canQuiDoi;
                                        }
                                        else
                                        {
                                            OrderWeight += item.Weight.Value;
                                        }
                                    }
                                }
                                OrderWeight = Math.Round(OrderWeight, 2);
                                //double FeeWeight = Math.Round(Convert.ToDouble(pWeight.Text), 0);
                                //double FeeWeight = Math.Round(Convert.ToDouble(pWeight.Text), 0);
                                double TotalPriceReal = Math.Round(Convert.ToDouble(rTotalPriceReal.Text), 0);
                                double TotalPriceRealCYN = Math.Round(Convert.ToDouble(rTotalPriceRealCYN.Text), 2);

                                bool checkpro = new bool();
                                bool Package = new bool();
                                bool MoveIsFastDelivery = new bool();
                                bool baogia = new bool();
                                bool smallPackage = new bool();
                                bool ycg = new bool();
                                bool baohiem = new bool();

                                var listCheck = hdfListCheckBox.Value.Split('|').ToList();
                                foreach (var item in listCheck)
                                {
                                    if (!string.IsNullOrEmpty(item))
                                    {
                                        var ck = item.Split(',').ToList();

                                        if (ck != null)
                                        {
                                            if (ck[0] == "1")
                                            {
                                                smallPackage = Convert.ToBoolean(ck[1].ToInt(0));
                                            }
                                            if (ck[0] == "2")
                                            {
                                                baogia = Convert.ToBoolean(ck[1].ToInt(0));
                                            }
                                            if (ck[0] == "3")
                                            {
                                                checkpro = Convert.ToBoolean(ck[1].ToInt(0));
                                            }
                                            if (ck[0] == "4")
                                            {
                                                Package = Convert.ToBoolean(ck[1].ToInt(0));
                                            }
                                            if (ck[0] == "5")
                                            {
                                                MoveIsFastDelivery = Convert.ToBoolean(ck[1].ToInt(0));
                                            }
                                            if (ck[0] == "6")
                                            {
                                                ycg = Convert.ToBoolean(ck[1].ToInt(0));
                                            }
                                            if (ck[0] == "7")
                                            {
                                                baohiem = Convert.ToBoolean(ck[1].ToInt(0));
                                            }
                                        }
                                    }
                                }

                                double IsCheckProductPrice = 0;
                                double IsFastDeliveryPrice = 0;
                                double IsPackedPrice = 0;
                                double InsurranceMoney = 0;

                                if (checkpro == true)
                                {
                                    var listorder = OrderController.GetByMainOrderID(o.ID);
                                    if (listorder != null)
                                    {
                                        if (listorder.Count > 0)
                                        {
                                            double total = 0;
                                            double counpros_more10 = 0;
                                            double counpros_les10 = 0;
                                            if (listorder.Count > 0)
                                            {
                                                foreach (var item in listorder)
                                                {
                                                    double countProduct = item.quantity.ToInt(1);
                                                    if (Convert.ToDouble(item.price_origin) >= 10)
                                                    {
                                                        counpros_more10 += item.quantity.ToInt(1);
                                                    }
                                                    else
                                                    {
                                                        counpros_les10 += item.quantity.ToInt(1);
                                                    }
                                                }
                                            }
                                            if (counpros_more10 > 0)
                                            {
                                                var feecheck_more10 = FeeCheckProductController.GetAllType(1);
                                                if (feecheck_more10.Count > 0)
                                                {
                                                    foreach (var item in feecheck_more10)
                                                    {
                                                        if (counpros_more10 > item.AmountFrom && counpros_more10 <= item.AmountTo)
                                                        {
                                                            total += counpros_more10 * Convert.ToDouble(item.Price);
                                                            //total = counpros_more10 * Convert.ToDouble(item.Price);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (counpros_les10 > 0)
                                            {
                                                var feecheck_les10 = FeeCheckProductController.GetAllType(2);
                                                if (feecheck_les10.Count > 0)
                                                {
                                                    foreach (var item in feecheck_les10)
                                                    {
                                                        if (counpros_les10 > item.AmountFrom && counpros_les10 <= item.AmountTo)
                                                        {
                                                            total += counpros_les10 * Convert.ToDouble(item.Price);
                                                            //total = counpros_les10 * Convert.ToDouble(item.Price);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            IsCheckProductPrice = Math.Round(total, 0);
                                        }
                                    }
                                }
                                if (MoveIsFastDelivery == true)
                                {
                                    if (pShipHome.Text.ToString().ToFloat(0) > 0)
                                        IsFastDeliveryPrice = Math.Round(Convert.ToDouble(pShipHome.Text), 0);
                                }
                                if (Package == true)
                                {
                                    if (pPacked.Text.ToString().ToFloat(0) > 0)
                                        IsPackedPrice = Math.Round(Convert.ToDouble(pPacked.Text), 0);
                                }
                                if (baohiem == true)
                                {
                                    InsurranceMoney = Convert.ToDouble(PriceVND) * (Convert.ToDouble(InsurrancePercent) / 100);
                                }

                                double TotalPriceVND = FeeShipCN + FeeBuyPro + FeeWeight + IsCheckProductPrice + IsPackedPrice + IsFastDeliveryPrice + PriceVND + TotalFeeSupport + InsurranceMoney;
                                TotalPriceVND = Math.Round(TotalPriceVND, 0);

                                MainOrderController.UpdateFeeOrderDetail(o.ID, Deposit.ToString(), FeeShipCN.ToString(), FeeBuyPro.ToString(), FeeWeight.ToString(),
                                                IsCheckProductPrice.ToString(), IsPackedPrice.ToString(), IsFastDeliveryPrice.ToString(), TotalPriceVND.ToString());
                                MainOrderController.UpdateInsurranceMoney(o.ID, InsurranceMoney.ToString(), InsurrancePercent.ToString());
                                MainOrderController.UpdateCheckService(o.ID, checkpro, baogia, Package, MoveIsFastDelivery, smallPackage, ycg, baohiem);
                                MainOrderController.UpdateAmountDeposit(o.ID, AmountDeposit.ToString());
                                MainOrderController.UpdateTotalFeeSupport(o.ID, TotalFeeSupport.ToString());
                                MainOrderController.UpdateTotalPriceReal(o.ID, TotalPriceReal.ToString(), TotalPriceRealCYN.ToString());
                                MainOrderController.UpdateTQVNWeight(o.ID, o.UID.ToString().ToInt(), OrderWeight.ToString());
                                MainOrderController.UpdateStatusByID(o.ID, Convert.ToInt32(ddlStatus.SelectedValue));

                                //Fix bắt trạng thái
                                //var smallpackages = SmallPackageController.GetAllByMainOrderID(o.ID);
                                //if (smallpackages.Count > 0)
                                //{
                                //    int minSTT = smallpackages.ElementAt(0).Status.Value;
                                //    for (int i = 1; i < smallpackages.Count; i++)
                                //    {
                                //        if (smallpackages.ElementAt(i).Status.Value < minSTT && smallpackages.ElementAt(i).Status.Value != 4)
                                //            minSTT = smallpackages.ElementAt(i).Status.Value;
                                //        else if(smallpackages.ElementAt(i).Status.Value == 5)
                                //            minSTT = 5;
                                //    }

                                //    var mainOrderSTTEdit = Convert.ToInt32(ddlStatus.SelectedValue);
                                //    if (minSTT == 1 && mainOrderSTTEdit < 6 && mainOrderSTTEdit != 5)
                                //    {
                                //        MainOrderController.UpdateStatusByID(o.ID, Convert.ToInt32(ddlStatus.SelectedValue));
                                //    }
                                //    else if (minSTT == 5)
                                //    {
                                //        MainOrderController.UpdateStatusByID(o.ID, 5);
                                //        MainOrderController.UpdateDateDelivery(o.ID, currentDate);
                                //    }
                                //    else if (minSTT == 2)
                                //    {
                                //        if (Convert.ToInt32(ddlStatus.SelectedValue) == 3)
                                //        {
                                //            MainOrderController.UpdateStatusByID(o.ID, 3);
                                //            MainOrderController.UpdateDateLoading(o.ID, currentDate);
                                //        }
                                //        else
                                //        {
                                //            MainOrderController.UpdateStatusByID(o.ID, 6);
                                //            MainOrderController.UpdateDateTQ(o.ID, currentDate);
                                //        }
                                //    }
                                //    else if (minSTT == 3)
                                //    {
                                //        if (Convert.ToInt32(ddlStatus.SelectedValue) == 9)
                                //        {
                                //            MainOrderController.UpdateStatusByID(o.ID, 9);
                                //            MainOrderController.UpdatePayDate(o.ID, currentDate);
                                //        }
                                //        else
                                //        {
                                //            MainOrderController.UpdateStatusByID(o.ID, 7);
                                //            MainOrderController.UpdateDateVN(o.ID, currentDate);
                                //        }
                                //    }
                                //    else if (minSTT == 4)
                                //    {
                                //        MainOrderController.UpdateStatusByID(o.ID, 10);
                                //        MainOrderController.UpdateCompleteDate(o.ID, currentDate);
                                //    }
                                //}
                                //else
                                //{
                                //    if (Convert.ToInt32(ddlStatus.SelectedValue) < 6)
                                //    {
                                //        MainOrderController.UpdateStatusByID(o.ID, Convert.ToInt32(ddlStatus.SelectedValue));
                                //    }
                                //}


                                MainOrderController.UpdateFTS(o.ID, o.UID.ToString().ToInt(), ddlWarehouseFrom.SelectedValue.ToInt(), ddlReceivePlace.SelectedValue, ddlShippingType.SelectedValue.ToInt());

                                if (RoleID == 0)
                                {
                                    if (OCurrent_deposit != Deposit)
                                    {
                                        MainOrderController.UpdateDeposit(o.ID, Deposit.ToString());
                                        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã đổi tiền đặt cọc của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_deposit) + "," +
                                        " sang: " + string.Format("{0:N0}", Deposit) + "", 1, currentDate);
                                    }
                                }

                                if (status == 2)
                                {
                                    if (o.DepostiDate == null)
                                    {
                                        MainOrderController.UpdateDepositDate(o.ID, currentDate);
                                    }
                                }
                                else if (status == 3)
                                {
                                    if (o.DateLoading == null)
                                    {
                                        MainOrderController.UpdateDateLoading(o.ID, currentDate);
                                    }
                                }
                                else if (status == 4)
                                {
                                    if (o.DateBuy == null)
                                    {
                                        MainOrderController.UpdateDateBuy(o.ID, currentDate);
                                    }
                                }
                                else if (status == 5)
                                {
                                    if (o.DateDeliveryShop == null)
                                    {
                                        MainOrderController.UpdateDateDelivery(o.ID, currentDate);
                                    }
                                }
                                else if (status == 6)
                                {
                                    if (o.DateTQ == null)
                                    {
                                        MainOrderController.UpdateDateTQ(o.ID, currentDate);
                                    }
                                }
                                else if (status == 7)
                                {
                                    if (o.DateVN == null)
                                    {
                                        MainOrderController.UpdateDateVN(o.ID, currentDate);
                                    }
                                }
                                else if (status == 8)
                                {
                                    if (o.AdPayDate == null)
                                    {
                                        MainOrderController.UpdateAdminPayDate(o.ID, currentDate);
                                    }
                                }
                                else if (status == 9)
                                {
                                    if (o.PayDate == null)
                                    {
                                        MainOrderController.UpdatePayDate(o.ID, currentDate);
                                    }
                                }
                                else if (status == 10)
                                {
                                    if (o.CompleteDate == null)
                                    {
                                        MainOrderController.UpdateCompleteDate(o.ID, currentDate);
                                    }
                                }

                                #region Ghi lịch sử update status của đơn hàng
                                if (status != currentstt)
                                {
                                    OrderCommentController.Insert(id, "Đã có cập nhật mới cho đơn hàng #" + id + " của bạn.", true, 1, DateTime.UtcNow.AddHours(7), uid);
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi trạng thái của đơn hàng ID là: " + o.ID + ", từ: " + orderstatus + ", sang: " + ddlStatus.SelectedItem + "", 0, currentDate);
                                }
                                if (OCurrent_FeeShipCN != FeeShipCN)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi tiền phí ship Trung Quốc của đơn hàng ID là: " + o.ID + ", từ " + string.Format("{0:N0}", OCurrent_FeeShipCN) + " sang " + string.Format("{0:N0}", FeeShipCN) + "", 2, currentDate);
                                }
                                if (OCurrent_FeeWeight != FeeWeight)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi tiền phí TQ-VN của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_FeeWeight) + ", sang: " + string.Format("{0:N0}", FeeWeight) + "", 4, currentDate);
                                }
                                if (OCurrent_TotalPriceReal != TotalPriceReal)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi tiền phí mua thật của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_TotalPriceReal) + ", sang: " + string.Format("{0:N0}", TotalPriceReal) + "", 3, currentDate);
                                }
                                if (OCurrent_IsCheckProductPrice != IsCheckProductPrice)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi tiền phí kiểm tra sản phẩm của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_IsCheckProductPrice) + ", sang: "
                                            + string.Format("{0:N0}", IsCheckProductPrice) + "", 5, currentDate);
                                }
                                if (OCurrent_IsPackedPrice != IsPackedPrice)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi tiền phí đóng gỗ của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_IsPackedPrice) + ", sang: "
                                            + string.Format("{0:N0}", IsPackedPrice) + "", 6, currentDate);
                                }
                                if (OCurrent_IsFastDeliveryPrice != IsFastDeliveryPrice)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi tiền phí ship giao hàng tận nhà của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_IsFastDeliveryPrice) + ", sang: "
                                            + string.Format("{0:N0}", IsFastDeliveryPrice) + "", 7, currentDate);
                                }
                                if (CurrentAmountDeposit != AmountDeposit)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi số tiền phải cọc của đơn hàng ID là: " + o.ID + ", từ: " + CurrentAmountDeposit + ", sang: " + AmountDeposit + "", 8, currentDate);
                                }
                                if (CurrentOrderWeight != OrderWeight)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi cân nặng của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderWeight + ", sang: " + OrderWeight + "", 9, currentDate);
                                }
                                #endregion

                            }
                            #region Cập nhật thông tin nhân viên sale và đặt hàng
                            int SalerID = ddlSaler.SelectedValue.ToString().ToInt(0);
                            int DathangID = ddlDatHang.SelectedValue.ToString().ToInt(0);
                            int KhoTQID = ddlKhoTQ.SelectedValue.ToString().ToInt(0);
                            int khoVNID = ddlKhoVN.SelectedValue.ToString().ToInt(0);
                            var mo = MainOrderController.GetAllByID(id);
                            if (mo != null)
                            {
                                double feebp = Math.Round(Convert.ToDouble(mo.FeeBuyPro), 0);
                                DateTime CreatedDate = Convert.ToDateTime(mo.CreatedDate);
                                double salepercent = 0;
                                double salepercentaf3m = 0;
                                double dathangpercent = 0;
                                var config = ConfigurationController.GetByTop1();
                                if (config != null)
                                {
                                    salepercent = Convert.ToDouble(config.SalePercent);
                                    salepercentaf3m = Convert.ToDouble(config.SalePercentAfter3Month);
                                    dathangpercent = Convert.ToDouble(config.DathangPercent);
                                }
                                string salerName = "";
                                string dathangName = "";

                                int salerID_old = Convert.ToInt32(mo.SalerID);
                                int dathangID_old = Convert.ToInt32(mo.DathangID);

                                #region Saler
                                if (SalerID > 0)
                                {
                                    if (SalerID == salerID_old)
                                    {
                                        var staff = StaffIncomeController.GetByMainOrderIDUID(id, salerID_old);
                                        if (staff != null)
                                        {
                                            int rStaffID = staff.ID;
                                            int staffstatus = Convert.ToInt32(staff.Status);
                                            if (staffstatus == 1)
                                            {
                                                var sale = AccountController.GetByID(salerID_old);
                                                if (sale != null)
                                                {
                                                    salerName = sale.Username;
                                                    var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                                    int d = CreatedDate.Subtract(createdDate).Days;
                                                    if (d > 90)
                                                    {
                                                        salepercentaf3m = Convert.ToDouble(staff.PercentReceive);
                                                        double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                                        StaffIncomeController.Update(rStaffID, mo.TotalPriceVND, salepercentaf3m.ToString(), 1,
                                                            per.ToString(), false, currentDate, username);
                                                    }
                                                    else
                                                    {
                                                        salepercent = Convert.ToDouble(staff.PercentReceive);
                                                        double per = Math.Round(feebp * salepercent / 100, 0);
                                                        StaffIncomeController.Update(rStaffID, mo.TotalPriceVND, salepercent.ToString(), 1,
                                                            per.ToString(), false, currentDate, username);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var staff = StaffIncomeController.GetByMainOrderIDUID(id, salerID_old);
                                        if (staff != null)
                                        {
                                            StaffIncomeController.Delete(staff.ID);
                                        }
                                        var sale = AccountController.GetByID(SalerID);
                                        if (sale != null)
                                        {
                                            salerName = sale.Username;
                                            var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                            int d = CreatedDate.Subtract(createdDate).Days;
                                            if (d > 90)
                                            {
                                                double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                                StaffIncomeController.Insert(id, per.ToString(), salepercentaf3m.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                                CreatedDate, currentDate, username);
                                            }
                                            else
                                            {
                                                double per = Math.Round(feebp * salepercent / 100, 0);
                                                StaffIncomeController.Insert(id, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                                CreatedDate, currentDate, username);
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #region Đặt hàng
                                if (DathangID > 0)
                                {
                                    if (DathangID == dathangID_old)
                                    {
                                        var staff = StaffIncomeController.GetByMainOrderIDUID(id, dathangID_old);
                                        if (staff != null)
                                        {
                                            if (staff.Status == 1)
                                            {
                                                //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                                double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                                totalPrice = Math.Round(totalPrice, 0);
                                                double totalRealPrice = 0;
                                                if (mo.TotalPriceReal.ToFloat(0) > 0)
                                                    totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                                if (totalRealPrice > 0)
                                                {
                                                    double totalpriceloi = totalPrice - totalRealPrice;
                                                    dathangpercent = Convert.ToDouble(staff.PercentReceive);
                                                    double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                                    //double income = totalpriceloi;
                                                    StaffIncomeController.Update(staff.ID, totalRealPrice.ToString(), dathangpercent.ToString(), 1,
                                                                income.ToString(), false, currentDate, username);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var staff = StaffIncomeController.GetByMainOrderIDUID(id, dathangID_old);
                                        if (staff != null)
                                        {
                                            StaffIncomeController.Delete(staff.ID);
                                        }
                                        var dathang = AccountController.GetByID(DathangID);
                                        if (dathang != null)
                                        {
                                            dathangName = dathang.Username;
                                            //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                            double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                            totalPrice = Math.Round(totalPrice, 0);
                                            double totalRealPrice = 0;
                                            if (mo.TotalPriceReal.ToFloat(0) > 0)
                                                totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                            if (totalRealPrice > 0)
                                            {
                                                double totalpriceloi = totalPrice - totalRealPrice;
                                                double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                                //double income = totalpriceloi;

                                                StaffIncomeController.Insert(id, totalpriceloi.ToString(), dathangpercent.ToString(), DathangID, dathangName, 3, 1,
                                                    income.ToString(), false, CreatedDate, currentDate, username);
                                            }
                                            else
                                            {
                                                StaffIncomeController.Insert(id, "0", dathangpercent.ToString(), DathangID, dathangName, 3, 1, "0", false,
                                                CreatedDate, currentDate, username);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            MainOrderController.UpdateStaff(id, SalerID, DathangID, KhoTQID, khoVNID);
                            #endregion
                            #endregion

                            string StaffNote = "";
                            StaffNote = txtStaffNote.Text;
                            MainOrderController.UpdateStaffNote(id, StaffNote);
                            PJUtils.ShowMsg("Cập nhật thông tin thành công.", true, Page);
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Mã vận đơn " + listmvd + " đã tồn tại.", "i", false, Page);
                        }
                    }
                }
            }
        }
        protected void btnStaffUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            int SalerID = ddlSaler.SelectedValue.ToString().ToInt(0);
            int DathangID = ddlDatHang.SelectedValue.ToString().ToInt(0);
            int KhoTQID = ddlKhoTQ.SelectedValue.ToString().ToInt(0);
            int khoVNID = ddlKhoVN.SelectedValue.ToString().ToInt(0);
            int ID = ViewState["MOID"].ToString().ToInt();
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.UtcNow.AddHours(7);
            var mo = MainOrderController.GetAllByID(ID);
            if (mo != null)
            {
                double feebp = Convert.ToDouble(mo.FeeBuyPro);
                DateTime CreatedDate = Convert.ToDateTime(mo.CreatedDate);
                double salepercent = 0;
                double salepercentaf3m = 0;
                double dathangpercent = 0;
                var config = ConfigurationController.GetByTop1();
                if (config != null)
                {
                    salepercent = Convert.ToDouble(config.SalePercent);
                    salepercentaf3m = Convert.ToDouble(config.SalePercentAfter3Month);
                    dathangpercent = Convert.ToDouble(config.DathangPercent);
                }
                string salerName = "";
                string dathangName = "";

                int salerID_old = Convert.ToInt32(mo.SalerID);
                int dathangID_old = Convert.ToInt32(mo.DathangID);

                var user = AccountController.GetByID(Convert.ToInt32(mo.UID));

                #region Saler
                if (SalerID > 0)
                {
                    if (SalerID == salerID_old)
                    {
                        var staff = StaffIncomeController.GetByMainOrderIDUID(ID, salerID_old);
                        if (staff != null)
                        {
                            int rStaffID = staff.ID;
                            int status = Convert.ToInt32(staff.Status);
                            if (status == 1)
                            {
                                var sale = AccountController.GetByID(salerID_old);
                                if (sale != null)
                                {
                                    salerName = sale.Username;
                                    var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                    int d = CreatedDate.Subtract(createdDate).Days;
                                    if (d > 90)
                                    {
                                        salepercentaf3m = Convert.ToDouble(staff.PercentReceive);
                                        double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                        StaffIncomeController.Update(rStaffID, mo.TotalPriceVND, salepercentaf3m.ToString(), 1,
                                            per.ToString(), false, currentDate, username);
                                    }
                                    else
                                    {
                                        salepercent = Convert.ToDouble(staff.PercentReceive);
                                        double per = Math.Round(feebp * salepercent / 100, 0);
                                        StaffIncomeController.Update(rStaffID, mo.TotalPriceVND, salepercent.ToString(), 1,
                                            per.ToString(), false, currentDate, username);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var sale = AccountController.GetByID(SalerID);
                            if (sale != null)
                            {
                                salerName = sale.Username;
                                var createdDate = Convert.ToDateTime(user.CreatedDate);
                                int d = CreatedDate.Subtract(createdDate).Days;
                                if (d > 90)
                                {
                                    double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                    StaffIncomeController.Insert(ID, per.ToString(), salepercentaf3m.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                    CreatedDate, currentDate, username);
                                }
                                else
                                {
                                    double per = Math.Round(feebp * salepercent / 100, 0);
                                    StaffIncomeController.Insert(ID, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                    CreatedDate, currentDate, username);
                                }
                            }
                        }
                    }
                    else
                    {
                        var staff = StaffIncomeController.GetByMainOrderIDUID(ID, salerID_old);
                        if (staff != null)
                        {
                            StaffIncomeController.Delete(staff.ID);
                        }
                        var sale = AccountController.GetByID(SalerID);
                        if (sale != null)
                        {
                            salerName = sale.Username;
                            var createdDate = Convert.ToDateTime(user.CreatedDate);
                            int d = CreatedDate.Subtract(createdDate).Days;
                            if (d > 90)
                            {
                                double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                StaffIncomeController.Insert(ID, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                CreatedDate, currentDate, username);
                            }
                            else
                            {
                                double per = Math.Round(feebp * salepercent / 100, 0);
                                StaffIncomeController.Insert(ID, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                CreatedDate, currentDate, username);
                            }
                        }
                    }
                }
                #endregion
                #region Đặt hàng
                if (DathangID > 0)
                {
                    if (DathangID == dathangID_old)
                    {
                        var staff = StaffIncomeController.GetByMainOrderIDUID(ID, dathangID_old);
                        if (staff != null)
                        {
                            if (staff.Status == 1)
                            {
                                //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                totalPrice = Math.Round(totalPrice, 0);
                                double totalRealPrice = 0;
                                if (!string.IsNullOrEmpty(mo.TotalPriceReal))
                                    totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                if (totalRealPrice > 0)
                                {
                                    double totalpriceloi = totalPrice - totalRealPrice;
                                    totalpriceloi = Math.Round(totalpriceloi, 0);
                                    dathangpercent = Convert.ToDouble(staff.PercentReceive);
                                    double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                    //double income = totalpriceloi;
                                    StaffIncomeController.Update(staff.ID, totalRealPrice.ToString(), dathangpercent.ToString(), 1,
                                                income.ToString(), false, currentDate, username);
                                }

                            }
                        }
                        else
                        {
                            var dathang = AccountController.GetByID(DathangID);
                            if (dathang != null)
                            {
                                dathangName = dathang.Username;
                                //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                totalPrice = Math.Round(totalPrice, 0);
                                double totalRealPrice = 0;
                                if (!string.IsNullOrEmpty(mo.TotalPriceReal))
                                    totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                if (totalRealPrice > 0)
                                {
                                    double totalpriceloi = totalPrice - totalRealPrice;
                                    totalpriceloi = Math.Round(totalpriceloi, 0);
                                    double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                    //double income = totalpriceloi;
                                    StaffIncomeController.Insert(ID, totalpriceloi.ToString(), dathangpercent.ToString(), DathangID, dathangName, 3, 1,
                                        income.ToString(), false, CreatedDate, currentDate, username);
                                }
                                else
                                {
                                    StaffIncomeController.Insert(ID, "0", dathangpercent.ToString(), DathangID, dathangName, 3, 1, "0", false,
                                    CreatedDate, currentDate, username);
                                }
                            }
                        }
                    }
                    else
                    {
                        var staff = StaffIncomeController.GetByMainOrderIDUID(ID, dathangID_old);
                        if (staff != null)
                        {
                            StaffIncomeController.Delete(staff.ID);
                        }
                        var dathang = AccountController.GetByID(DathangID);
                        if (dathang != null)
                        {
                            dathangName = dathang.Username;
                            //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                            double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                            totalPrice = Math.Round(totalPrice, 0);
                            double totalRealPrice = 0;
                            if (!string.IsNullOrEmpty(mo.TotalPriceReal))
                                totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                            if (totalRealPrice > 0)
                            {
                                double totalpriceloi = totalPrice - totalRealPrice;
                                double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                //double income = totalpriceloi;

                                StaffIncomeController.Insert(ID, totalpriceloi.ToString(), dathangpercent.ToString(), DathangID, dathangName, 3, 1,
                                    income.ToString(), false, CreatedDate, currentDate, username);
                            }
                            else
                            {
                                StaffIncomeController.Insert(ID, "0", dathangpercent.ToString(), DathangID, dathangName, 3, 1, "0", false,
                                CreatedDate, currentDate, username);
                            }
                        }
                    }
                }
                #endregion
            }

            MainOrderController.UpdateStaff(ID, SalerID, DathangID, KhoTQID, khoVNID);
            PJUtils.ShowMsg("Cập nhật nhân viên thành công.", true, Page);
        }
        //protected void btnThanhtoan_Click(object sender, EventArgs e)
        //{
        //    int id = ViewState["MOID"].ToString().ToInt(0);
        //    //var id = Convert.ToInt32(Request.QueryString["id"]);
        //    if (id > 0)
        //    {
        //        var o = MainOrderController.GetAllByID(id);
        //        if (o != null)
        //        {
        //            Response.Redirect("/manager/Pay-Order.aspx?id=" + id);
        //        }
        //    }
        //}
        #endregion
        #region Ajax
        [WebMethod]
        public static string DeleteSmallPackage(string IDPackage)
        {
            if (HttpContext.Current.Session["userLoginSystem"] == null)
            {
                return null;
            }
            else
            {
                int ID = IDPackage.ToInt(0);
                var smallpackage = SmallPackageController.GetByID(ID);
                if (smallpackage != null)
                {
                    string ListMVD = "";
                    int Quantity = 0;
                    string kq = SmallPackageController.Delete(ID);
                    var list = SmallPackageController.GetAllByMainOrderID(smallpackage.MainOrderID.Value);
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            ListMVD += item.OrderTransactionCode + " | ";
                        }
                        Quantity = list.Count;
                        MainOrderController.UpdateBarcode(smallpackage.MainOrderID.Value, ListMVD);
                        MainOrderController.UpdateQuantityBarcode(smallpackage.MainOrderID.Value, Quantity);
                    }
                    else
                    {
                        MainOrderController.UpdateNullBarcode(smallpackage.MainOrderID.Value);
                        MainOrderController.UpdateNullQuantityBarcode(smallpackage.MainOrderID.Value);
                    }
                    return kq;
                }
                else
                {
                    return "null";
                }
            }

        }

        [WebMethod]
        public static string DeleteSupportFee(string IDPackage)
        {
            if (HttpContext.Current.Session["userLoginSystem"] == null)
            {
                return null;
            }
            else
            {
                DateTime currentDate = DateTime.UtcNow.AddHours(7);
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var obj_user = AccountController.GetByUsername(username);
                if (obj_user != null)
                {
                    int ID = IDPackage.ToInt(0);
                    var supportfee = FeeSupportController.GetByID(ID);
                    if (supportfee != null)
                    {
                        string kq = FeeSupportController.Delete(ID);
                        HistoryOrderChangeController.Insert(supportfee.MainOrderID.Value, obj_user.ID, obj_user.Username, obj_user.Username +
                                         " đã xóa tiền phụ phí của đơn hàng ID là: " + supportfee.MainOrderID + ", Tên phụ phí: " + supportfee.SupportName + ", Số tiền: "
                                         + string.Format("{0:N0}", Convert.ToDouble(supportfee.SupportInfoVND)) + "", 10, currentDate);
                        return "ok";
                    }
                    else
                    {
                        return "null";
                    }
                }
                else
                {
                    return "null";
                }
            }

        }

        [WebMethod]
        public static string DeleteMainOrderCode(int IDCode)
        {
            if (HttpContext.Current.Session["userLoginSystem"] == null)
            {
                return null;
            }
            else
            {
                int ID = IDCode;
                var MainOrderCode = MainOrderCodeController.GetByID(ID);
                if (MainOrderCode != null)
                {
                    string ListMDH = "";
                    int Quantity = 0;
                    string kq = MainOrderCodeController.Delete(ID);
                    var list = MainOrderCodeController.GetAllByMainOrderID(MainOrderCode.MainOrderID.Value);
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            ListMDH += item.MainOrderCode + " | ";
                        }
                        Quantity = list.Count;
                        MainOrderController.UpdateMainOrderCode(MainOrderCode.MainOrderID.Value, ListMDH);
                        MainOrderController.UpdateQuantityOrderCode(MainOrderCode.MainOrderID.Value, Quantity);
                    }
                    else
                    {
                        MainOrderController.UpdateNullMainOrderCode(MainOrderCode.MainOrderID.Value);
                        MainOrderController.UpdateNullQuantityOrderCode(MainOrderCode.MainOrderID.Value);
                    }
                    return kq;
                }
                else
                {
                    return null;
                }
            }
        }

        [WebMethod]
        public static string UpdateMainOrderCode(int ID, string MainOrderCode, int MainOrderID)
        {
            string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (ac != null)
            {
                var o = MainOrderController.GetAllByID(MainOrderID);
                if (o != null)
                {
                    string ListMDH = "";
                    int Quantity = 0;
                    var lo = MainOrderCodeController.GetByID(ID);
                    if (!string.IsNullOrEmpty(MainOrderCode))
                    {
                        if (lo == null)
                        {
                            var so = MainOrderCodeController.GetByMainOrderIDANDMainOrderCode(MainOrderID, MainOrderCode);
                            if (so == null)
                            {
                                var kq = MainOrderCodeController.Insert(MainOrderID, MainOrderCode, DateTime.Now, ac.Username);
                                var list = MainOrderCodeController.GetAllByMainOrderID(MainOrderID);
                                if (list.Count > 0)
                                {
                                    foreach (var item in list)
                                    {
                                        ListMDH += item.MainOrderCode + " | ";
                                    }
                                    Quantity = list.Count;
                                    MainOrderController.UpdateMainOrderCode(MainOrderID, ListMDH);
                                    MainOrderController.UpdateQuantityOrderCode(MainOrderID, Quantity);
                                }
                                return kq;
                            }
                            return null;
                        }
                        else
                        {
                            var so = MainOrderCodeController.GetByMainOrderIDANDMainOrderCode(MainOrderID, MainOrderCode);
                            if (so == null)
                            {
                                var kq = MainOrderCodeController.UpdateCode(ID, MainOrderCode, DateTime.Now, ac.Username);
                                var list = MainOrderCodeController.GetAllByMainOrderID(MainOrderID);
                                if (list.Count > 0)
                                {
                                    foreach (var item in list)
                                    {
                                        ListMDH += item.MainOrderCode + " | ";
                                    }
                                    Quantity = list.Count;
                                    MainOrderController.UpdateMainOrderCode(MainOrderID, ListMDH);
                                    MainOrderController.UpdateQuantityOrderCode(MainOrderID, Quantity);
                                }
                                return kq;
                            }
                            return null;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //[WebMethod]
        //public static string UpdateMainOrderCode(int ID, string MainOrderCode, int MainOrderID)
        //{
        //    string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
        //    tbl_Account ac = AccountController.GetByUsername(username_current);
        //    if (ac != null)
        //    {
        //        var o = MainOrderController.GetAllByID(MainOrderID);
        //        if (o != null)
        //        {
        //            var lo = MainOrderCodeController.GetByID(ID);
        //            if (!string.IsNullOrEmpty(MainOrderCode))
        //            {
        //                if (lo == null)
        //                {
        //                    var so = MainOrderCodeController.GetByMainOrderIDANDMainOrderCode(MainOrderID, MainOrderCode);
        //                    if (so == null)
        //                    {
        //                        var kq = MainOrderCodeController.Insert(MainOrderID, MainOrderCode, DateTime.UtcNow.AddHours(7), ac.Username);
        //                        return kq;
        //                    }
        //                    return null;
        //                }
        //                else
        //                {
        //                    var so = MainOrderCodeController.GetByMainOrderIDANDMainOrderCode(MainOrderID, MainOrderCode);
        //                    if (so == null)
        //                    {
        //                        var kq = MainOrderCodeController.UpdateCode(ID, MainOrderCode, DateTime.UtcNow.AddHours(7), ac.Username);
        //                        return kq;
        //                    }
        //                    return null;
        //                }

        //            }
        //            return null;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        [WebMethod]
        public static string CountFeeWeight(int orderID, int receivePlace, int shippingTypeValue, double weight, int WarehouseFrom)
        {
            var order = MainOrderController.GetAllByID(orderID);
            if (order != null)
            {
                double pricePerKg = 0;
                int fromPlace = WarehouseFrom;
                var warehousefee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(
                    fromPlace, receivePlace, shippingTypeValue, false);
                if (warehousefee.Count > 0)
                {
                    foreach (var w in warehousefee)
                    {
                        if (w.WeightFrom < weight && weight <= w.WeightTo)
                        {
                            pricePerKg = Convert.ToDouble(w.Price);
                        }
                    }
                }
                int UID = Convert.ToInt32(order.UID);
                var usercreate = AccountController.GetByID(UID);
                if (!string.IsNullOrEmpty(usercreate.FeeTQVNPerWeight))
                {
                    double feeweightuser = 0;
                    if (usercreate.FeeTQVNPerWeight.ToFloat(0) > 0)
                    {
                        feeweightuser = Convert.ToDouble(usercreate.FeeTQVNPerWeight);
                    }
                    pricePerKg = feeweightuser;
                }

                double ckFeeWeight = 0;
                if (usercreate != null)
                {
                    ckFeeWeight = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeWeight.ToString());
                }
                double currency = Convert.ToDouble(order.CurrentCNYVN);
                double totalPriceFeeweightVN = pricePerKg * weight;

                double discountVN = totalPriceFeeweightVN * ckFeeWeight / 100;
                double discountCYN = discountVN / currency;

                double feeoutVN = totalPriceFeeweightVN - discountVN;
                double feeoutCYN = feeoutVN / currency;

                FeeWeightObj f = new FeeWeightObj();
                f.FeeWeightCYN = Math.Floor(feeoutCYN);
                f.FeeWeightVND = Math.Floor(feeoutVN);
                f.DiscountFeeWeightCYN = Math.Floor(discountCYN);
                f.DiscountFeeWeightVN = Math.Floor(discountVN);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(f);
            }
            return "none";
        }

        [WebMethod]
        public static string sendcustomercomment(string comment, int id, string urlIMG, string real)
        {
            var listLink = urlIMG.Split('|').ToList();
            if (listLink.Count > 0)
            {
                listLink.RemoveAt(listLink.Count - 1);
            }
            var listComment = real.Split('|').ToList();
            if (listComment.Count > 0)
            {
                listComment.RemoveAt(listComment.Count - 1);
            }
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.UtcNow.AddHours(7);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (obj_user != null)
            {
                string ret = "";
                var ai = AccountInfoController.GetByUserID(obj_user.ID);
                if (ai != null)
                {
                    ret += ai.FirstName + " " + ai.LastName + "," + ai.IMGUser + "," + string.Format("{0:dd/MM/yyyy HH:mm}", currentDate);
                }
                int uid = obj_user.ID;
                //var id = Convert.ToInt32(Request.QueryString["id"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {
                        var setNoti = SendNotiEmailController.GetByID(12);
                        int type = 1;
                        if (type > 0)
                        {
                            for (int i = 0; i < listLink.Count; i++)
                            {
                                string kqq = OrderCommentController.InsertNew(id, listLink[i], listComment[i], true, type, DateTime.UtcNow.AddHours(7), uid);
                            }
                            if (!string.IsNullOrEmpty(comment))
                            {
                                string kq = OrderCommentController.Insert(id, comment, true, type, DateTime.UtcNow.AddHours(7), uid);
                                if (type == 1)
                                {
                                    if (setNoti != null)
                                    {
                                        if (setNoti.IsSentNotiUser == true)
                                        {
                                            if (o.OrderType == 1)
                                            {
                                                NotificationsController.Inser(Convert.ToInt32(o.UID),
                                                   AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem",
                                                   12, currentDate, obj_user.Username, true);
                                                //string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                                //string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                //string datalink = "" + strUrl + "chi-tiet-don-hang/" + id;
                                                //PJUtils.PushNotiDesktop(Convert.ToInt32(o.UID), "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                            }
                                            else
                                            {
                                                NotificationsController.Inser(Convert.ToInt32(o.UID),
                                                   AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem",
                                                   13, currentDate, obj_user.Username, true);
                                                //string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                                //string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                //string datalink = "" + strUrl + "chi-tiet-don-hang/" + id;
                                                //PJUtils.PushNotiDesktop(Convert.ToInt32(o.UID), "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                            }

                                        }

                                        //if (setNoti.IsSendEmailUser == true)
                                        //{
                                        //    try
                                        //    {
                                        //        PJUtils.SendMailGmail("monamedia", "uvnajxzzurygsduf",
                                        //        AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                        //        "Thông báo tại Nhập hàng Tiết Kiệm.", "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                        //    }
                                        //    catch { }
                                        //}
                                    }
                                }
                                ChatHub ch = new ChatHub();
                                ch.SendMessenger(uid, id, comment, listLink, listComment);

                                CustomerComment dataout = new CustomerComment();
                                dataout.UID = uid;
                                dataout.OrderID = id;
                                StringBuilder showIMG = new StringBuilder();
                                for (int i = 0; i < listLink.Count; i++)
                                {
                                    showIMG.Append("<div class=\"chat chat-right\">");
                                    showIMG.Append("    <div class=\"chat-avatar\">");
                                    showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("    <div class=\"chat-body\">");
                                    showIMG.Append("        <div class=\"chat-text\">");
                                    showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.UtcNow.AddHours(7)) + "</div>");
                                    showIMG.Append("            <div class=\"text-content\">");
                                    showIMG.Append("                <div class=\"content\">");
                                    showIMG.Append("                    <div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                    showIMG.Append("	                    <div class=\"img-block\">");
                                    if (listLink[i].Contains(".doc"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                    }
                                    else if (listLink[i].Contains(".xls"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    else
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    showIMG.Append("	                    </div>");
                                    showIMG.Append("                    </div>");
                                    showIMG.Append("                </div>");
                                    showIMG.Append("            </div>");
                                    showIMG.Append("        </div>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("</div>");
                                }
                                showIMG.Append("<div class=\"chat chat-right\">");
                                showIMG.Append("    <div class=\"chat-avatar\">");
                                showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                showIMG.Append("    </div>");
                                showIMG.Append("    <div class=\"chat-body\">");
                                showIMG.Append("        <div class=\"chat-text\">");
                                showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.UtcNow.AddHours(7)) + "</div>");
                                showIMG.Append("            <div class=\"text-content\">");
                                showIMG.Append("                <div class=\"content\">");
                                showIMG.Append("                    <p>" + comment + "</p>");
                                showIMG.Append("                </div>");
                                showIMG.Append("            </div>");
                                showIMG.Append("        </div>");
                                showIMG.Append("    </div>");
                                showIMG.Append("</div>");

                                dataout.Comment = showIMG.ToString();
                                return serializer.Serialize(dataout);

                            }
                            else
                            {

                                if (listComment.Count > 0)
                                {
                                    ChatHub ch = new ChatHub();
                                    ch.SendMessenger(uid, id, comment, listLink, listComment);
                                    CustomerComment dataout = new CustomerComment();
                                    StringBuilder showIMG = new StringBuilder();
                                    for (int i = 0; i < listLink.Count; i++)
                                    {

                                        showIMG.Append("<div class=\"chat chat-right\">");
                                        showIMG.Append("<div class=\"chat-avatar\">");
                                        showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("<div class=\"chat-body\">");
                                        showIMG.Append("<div class=\"chat-text\">");
                                        showIMG.Append("<div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.UtcNow.AddHours(7)) + "</div>");
                                        showIMG.Append("<div class=\"text-content\">");
                                        showIMG.Append("<div class=\"content\">");
                                        showIMG.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        showIMG.Append("<div class=\"img-block\">");
                                        if (listLink[i].Contains(".doc"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (listLink[i].Contains(".xls"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                    }
                                    dataout.UID = uid;
                                    dataout.OrderID = id;
                                    dataout.Comment = showIMG.ToString();
                                    return serializer.Serialize(dataout);
                                }

                            }

                        }
                    }
                }
            }
            return serializer.Serialize(null);
        }
        public partial class CustomerComment
        {
            public int UID { get; set; }
            public int OrderID { get; set; }
            public string Comment { get; set; }
            public List<string> Link { get; set; }
            public List<string> CommentName { get; set; }
        }
        [WebMethod]
        public static string sendstaffcomment(string comment, int id, string urlIMG, string real)
        {
            var listLink = urlIMG.Split('|').ToList();
            if (listLink.Count > 0)
            {
                listLink.RemoveAt(listLink.Count - 1);
            }
            var listComment = real.Split('|').ToList();
            if (listComment.Count > 0)
            {
                listComment.RemoveAt(listComment.Count - 1);
            }
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.UtcNow.AddHours(7);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (obj_user != null)
            {
                string ret = "";
                var ai = AccountInfoController.GetByUserID(obj_user.ID);
                if (ai != null)
                {
                    ret += ai.FirstName + " " + ai.LastName + "," + ai.IMGUser + "," + string.Format("{0:dd/MM/yyyy HH:mm}", currentDate);
                }
                int uid = obj_user.ID;
                //var id = Convert.ToInt32(Request.QueryString["id"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {

                        int type = 2;
                        if (type > 0)
                        {
                            for (int i = 0; i < listLink.Count; i++)
                            {
                                string kqq = OrderCommentController.InsertNew(id, listLink[i], listComment[i], true, type, DateTime.UtcNow.AddHours(7), uid);
                            }
                            if (!string.IsNullOrEmpty(comment))
                            {
                                string kq = OrderCommentController.Insert(id, comment, true, type, DateTime.UtcNow.AddHours(7), uid);
                                var sale = AccountController.GetByID(o.SalerID.Value);
                                if (sale != null)
                                {
                                    if (obj_user.ID != sale.ID)
                                    {
                                        NotificationsController.Inser(sale.ID,
                                                                         sale.Username, id,
                                                                         "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                          currentDate, username, false);
                                        string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                        string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                        PJUtils.PushNotiDesktop(sale.ID, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                    }
                                }

                                var dathang = AccountController.GetByID(o.DathangID.Value);
                                if (dathang != null)
                                {
                                    if (obj_user.ID != dathang.ID)
                                    {
                                        NotificationsController.Inser(dathang.ID,
                                                                           dathang.Username, id,
                                                                           "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                            currentDate, username, false);
                                        string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                        string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                        PJUtils.PushNotiDesktop(dathang.ID, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                    }
                                }

                                var admins = AccountController.GetAllByRoleID(0);
                                if (admins.Count > 0)
                                {
                                    foreach (var admin in admins)
                                    {
                                        if (obj_user.ID != admin.ID)
                                        {
                                            NotificationsController.Inser(admin.ID,
                                                                          admin.Username, id,
                                                                          "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                           currentDate, username, false);
                                            string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                            string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                            string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                            PJUtils.PushNotiDesktop(admin.ID, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                        }
                                    }
                                }
                                var managers = AccountController.GetAllByRoleID(2);
                                if (managers.Count > 0)
                                {
                                    foreach (var manager in managers)
                                    {
                                        if (obj_user.ID != manager.ID)
                                        {
                                            NotificationsController.Inser(manager.ID,
                                                                           manager.Username, id,
                                                                           "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                          currentDate, username, false);
                                            string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                            string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                            string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                            PJUtils.PushNotiDesktop(manager.ID, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                        }
                                    }
                                }

                                var cskhs = AccountController.GetAllByRoleID(9);
                                if (cskhs.Count > 0)
                                {
                                    foreach (var cskh in cskhs)
                                    {
                                        if (obj_user.ID != cskh.ID)
                                        {
                                            NotificationsController.Inser(cskh.ID,
                                                                           cskh.Username, id,
                                                                           "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                          currentDate, username, false);
                                            string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                            string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                            string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                            PJUtils.PushNotiDesktop(cskh.ID, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                        }
                                    }
                                }

                                ChatHub ch = new ChatHub();
                                ch.SendMessengerToStaff(uid, id, comment, listLink, listComment);

                                CustomerComment dataout = new CustomerComment();
                                dataout.UID = uid;
                                dataout.OrderID = id;
                                StringBuilder showIMG = new StringBuilder();
                                for (int i = 0; i < listLink.Count; i++)
                                {
                                    showIMG.Append("<div class=\"chat chat-right\">");
                                    showIMG.Append("    <div class=\"chat-avatar\">");
                                    showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("    <div class=\"chat-body\">");
                                    showIMG.Append("        <div class=\"chat-text\">");
                                    showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.UtcNow.AddHours(7)) + "</div>");
                                    showIMG.Append("            <div class=\"text-content\">");
                                    showIMG.Append("                <div class=\"content\">");
                                    showIMG.Append("                    <div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                    showIMG.Append("	                    <div class=\"img-block\">");
                                    if (listLink[i].Contains(".doc"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                    }
                                    else if (listLink[i].Contains(".xls"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    else
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    showIMG.Append("	                    </div>");
                                    showIMG.Append("                    </div>");
                                    showIMG.Append("                </div>");
                                    showIMG.Append("            </div>");
                                    showIMG.Append("        </div>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("</div>");
                                }
                                showIMG.Append("<div class=\"chat chat-right\">");
                                showIMG.Append("    <div class=\"chat-avatar\">");
                                showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                showIMG.Append("    </div>");
                                showIMG.Append("    <div class=\"chat-body\">");
                                showIMG.Append("        <div class=\"chat-text\">");
                                showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.UtcNow.AddHours(7)) + "</div>");
                                showIMG.Append("            <div class=\"text-content\">");
                                showIMG.Append("                <div class=\"content\">");
                                showIMG.Append("                    <p>" + comment + "</p>");
                                showIMG.Append("                </div>");
                                showIMG.Append("            </div>");
                                showIMG.Append("        </div>");
                                showIMG.Append("    </div>");
                                showIMG.Append("</div>");


                                dataout.Comment = showIMG.ToString();
                                return serializer.Serialize(dataout);


                            }
                            else
                            {
                                if (listComment.Count > 0)
                                {
                                    ChatHub ch = new ChatHub();
                                    ch.SendMessengerToStaff(uid, id, comment, listLink, listComment);
                                    CustomerComment dataout = new CustomerComment();
                                    StringBuilder showIMG = new StringBuilder();
                                    for (int i = 0; i < listLink.Count; i++)
                                    {

                                        showIMG.Append("<div class=\"chat chat-right\">");
                                        showIMG.Append("<div class=\"chat-avatar\">");
                                        showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("<div class=\"chat-body\">");
                                        showIMG.Append("<div class=\"chat-text\">");
                                        showIMG.Append("<div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.UtcNow.AddHours(7)) + "</div>");
                                        showIMG.Append("<div class=\"text-content\">");
                                        showIMG.Append("<div class=\"content\">");
                                        showIMG.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        showIMG.Append("<div class=\"img-block\">");
                                        if (listLink[i].Contains(".doc"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (listLink[i].Contains(".xls"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                    }
                                    dataout.UID = uid;
                                    dataout.OrderID = id;
                                    dataout.Comment = showIMG.ToString();
                                    return serializer.Serialize(dataout);
                                }
                            }


                        }
                    }
                }
            }
            return serializer.Serialize(null);
        }

        #endregion
        #region Class
        public class historyCustom
        {
            public int ID { get; set; }
            public string Username { get; set; }
            public string RoleName { get; set; }
            public string Date { get; set; }
            public string Content { get; set; }
        }
        public class FeeWeightObj
        {
            public double FeeWeightVND { get; set; }
            public double FeeWeightCYN { get; set; }
            public double DiscountFeeWeightCYN { get; set; }
            public double DiscountFeeWeightVN { get; set; }
        }
        #endregion

        protected void r_ItemCommand(object sender, GridCommandEventArgs e)
        {
            var g = e.Item as GridDataItem;
            if (g == null) return;
        }
        protected void gr_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {

        }
        protected void gr_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                var o = MainOrderController.GetAllByID(id);
                if (o != null)
                {
                    var historyorder = HistoryOrderChangeController.GetByMainOrderID(o.ID);
                    if (historyorder.Count > 0)
                    {
                        List<historyCustom> hc = new List<historyCustom>();
                        foreach (var item in historyorder)
                        {
                            string username = item.Username;
                            string rolename = "admin";
                            var acc = AccountController.GetByUsername(username);
                            if (acc != null)
                            {
                                int role = Convert.ToInt32(acc.RoleID);

                                var r = RoleController.GetByID(role);
                                if (r != null)
                                {
                                    rolename = r.RoleDescription;
                                }
                            }
                            historyCustom h = new historyCustom();
                            h.ID = item.ID;
                            h.Username = username;
                            h.RoleName = rolename;
                            h.Date = string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate);
                            h.Content = item.HistoryContent;
                            hc.Add(h);
                        }
                        gr.DataSource = hc;
                    }
                }
            }
        }

        protected void btnCurrency_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0 || obj_user.RoleID == 2 || obj_user.RoleID == 3)
                {
                    DateTime currentDate = DateTime.Now;
                    int ID = ViewState["MOID"].ToString().ToInt(0);
                    if (ID > 0)
                    {
                        var mo = MainOrderController.GetAllByID(ID);
                        if (mo != null)
                        {
                            var acc = AccountController.GetByID(Convert.ToInt32(mo.UID));
                            double UL_CKFeeBuyPro = Convert.ToDouble(UserLevelController.GetByID(acc.LevelID.ToString().ToInt()).FeeBuyPro);
                            double LessDeposit = Convert.ToDouble(UserLevelController.GetByID(acc.LevelID.ToString().ToInt()).LessDeposit);
                            if (!string.IsNullOrEmpty(acc.Deposit.ToString()))
                            {
                                if (Convert.ToDouble(acc.Deposit) >= 0)
                                {
                                    LessDeposit = Convert.ToDouble(acc.Deposit);
                                }
                            }

                            double Currency = Convert.ToDouble(txtCurrency.Text);
                            double CurrencyCurrent = Convert.ToDouble(mo.CurrentCNYVN);
                            double InsurancePercent = Convert.ToDouble(mo.InsurancePercent);

                            if (Currency != CurrencyCurrent)
                            {
                                var listorder = OrderController.GetByMainOrderID(ID);
                                if (listorder != null)
                                {
                                    if (listorder.Count > 0)
                                    {
                                        double pricevnd = 0;
                                        double pricecyn = 0;

                                        foreach (var item in listorder)
                                        {
                                            double originprice = Math.Round(Convert.ToDouble(item.price_origin), 2);
                                            double promotionprice = Math.Round(Convert.ToDouble(item.price_promotion), 2);
                                            double oprice = 0;
                                            if (promotionprice > 0)
                                            {
                                                if (promotionprice < originprice)
                                                {
                                                    pricecyn += promotionprice;
                                                    oprice = promotionprice * Convert.ToDouble(item.quantity) * Currency;
                                                }
                                                else
                                                {
                                                    pricecyn += originprice;
                                                    oprice = originprice * Convert.ToDouble(item.quantity) * Currency;
                                                }
                                            }
                                            else
                                            {
                                                pricecyn += originprice;
                                                oprice = originprice * Convert.ToDouble(item.quantity) * Currency;
                                            }
                                            pricevnd += oprice;
                                        }
                                        pricevnd = Math.Round(pricevnd, 0);
                                        pricecyn = Math.Round(pricecyn, 2);

                                        double servicefee = Math.Round(Convert.ToDouble(mo.PercentBuyPro), 0);
                                        double feebpnotdc = pricevnd * servicefee / 100;
                                        double subfeebp = feebpnotdc * UL_CKFeeBuyPro / 100;
                                        double FeeBuyPro = feebpnotdc - subfeebp;
                                        FeeBuyPro = Math.Round(FeeBuyPro, 0);

                                        double InsuranceMoney = 0;
                                        if (mo.IsInsurrance == true)
                                            InsuranceMoney = Math.Round(pricevnd * (InsurancePercent / 100), 0);

                                        double FeeShipCN = Math.Round((Convert.ToDouble(mo.FeeShipCN) / CurrencyCurrent) * Currency, 0);
                                        double FeeWeight = Math.Round(Convert.ToDouble(mo.FeeWeight), 0);
                                        double FeeCheck = Math.Round(Convert.ToDouble(mo.IsCheckProductPrice), 0);
                                        double FeePacked = Math.Round((Convert.ToDouble(mo.IsPackedPrice) / CurrencyCurrent) * Currency, 0);
                                        double FeeShipHome = Math.Round(Convert.ToDouble(mo.IsFastDeliveryPrice), 0);
                                        double FeeAdd = Math.Round(Convert.ToDouble(mo.TotalFeeSupport), 0);
                                        double TotalPriceReal = Math.Round(Convert.ToDouble(mo.TotalPriceRealCYN) * Currency, 0);

                                        double AmountDeposit = Math.Round((pricevnd * LessDeposit) / 100, 0);
                                        double TotalPriceVND = FeeShipCN + FeeBuyPro + FeeWeight + FeeCheck + InsuranceMoney + FeeShipHome + FeeAdd + pricevnd;

                                        MainOrderController.UpdatePriceNotFee(mo.ID, pricevnd.ToString());
                                        MainOrderController.UpdatePriceCYN(mo.ID, pricecyn.ToString());
                                        MainOrderController.UpdateAmountDeposit(mo.ID, AmountDeposit.ToString());
                                        MainOrderController.UpdateFeeCurrentCNYVN(mo.ID, Currency.ToString(), FeeBuyPro.ToString(), InsuranceMoney.ToString(),
                                                             TotalPriceVND.ToString(), FeePacked.ToString(), TotalPriceReal.ToString(), FeeShipCN.ToString());

                                        HistoryOrderChangeController.Insert(mo.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã thay đổi tỷ giá đơn hàng ID là: " + mo.ID + ", từ: " + CurrencyCurrent + ", sang: " + Currency, 1, currentDate);

                                        PJUtils.ShowMessageBoxSwAlert("Cập nhật tỷ giá thành công", "s", true, Page);
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        [WebMethod]
        public static string UpdateBrand(int ID, string Note)
        {
            var Order = OrderController.GetAllByID(ID);
            if (Order != null)
            {
                OrderController.UpdateBrand(ID, Note);
                return "ok";
            }
            else
                return "none";
        }

        protected void btnThanhtoan_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0)
                {
                    int id = ViewState["MOID"].ToString().ToInt(0);
                    if (id > 0)
                    {
                        var o = MainOrderController.GetAllByID(id);
                        if (o != null)
                        {
                            int UIDU = Convert.ToInt32(o.UID);
                            var user = AccountController.GetByID(UIDU);
                            if (user != null)
                            {
                                double deposit = 0;
                                if (o.Deposit.ToFloat(0) > 0)
                                    deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);

                                double wallet = 0;
                                if (user.Wallet.ToString().ToFloat(0) > 0)
                                    wallet = Math.Round(Convert.ToDouble(user.Wallet), 0);

                                double feewarehouse = 0;
                                if (o.FeeInWareHouse.ToString().ToFloat(0) > 0)
                                    feewarehouse = Math.Round(Convert.ToDouble(o.FeeInWareHouse), 0);

                                double totalPriceVND = 0;
                                if (o.TotalPriceVND.ToFloat(0) > 0)
                                    totalPriceVND = Math.Round(Convert.ToDouble(o.TotalPriceVND), 0);
                                double moneyleft = Math.Round((totalPriceVND + feewarehouse) - deposit, 0);

                                string TienConLai = string.Format("{0:N0}", (wallet - moneyleft)) + " VNĐ";

                                if (moneyleft > 0)
                                {
                                    if (wallet >= moneyleft)
                                    {
                                        int st = TransactionController.PayAll(o.ID, wallet, o.Status.ToString().ToInt(0), UIDU, currentDate, username, deposit, 1, moneyleft, 1, 3, 2);
                                        if (st == 1)
                                        {
                                            PJUtils.ShowMessageBoxSwAlert("Thanh toán thành công.", "s", true, Page);
                                        }
                                        else
                                        {
                                            PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý, vui lòng thử lại sau.", "e", true, Page);
                                        }
                                    }
                                    else
                                    {
                                        PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của bạn không đủ để thanh toán đơn hàng. Vui lòng nạp thêm " + TienConLai + " ", "e", true, Page);
                                    }
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlert("Đơn hàng này không còn nợ.", "i", true, Page);
                                }
                            }
                        }
                    }
                }
                else
                    PJUtils.ShowMessageBoxSwAlert("Bạn không có quyền thực hiện chức năng này.", "e", true, Page);
            }
        }

    }
}