using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLADIPJ.Business;
using Telerik.Web.UI;
using MB.Extensions;
using System.Text;
using static NHST.Controllers.MainOrderController;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Web.Script.Serialization;

namespace NHST.manager
{
    public partial class OrderList : System.Web.UI.Page
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
                    if (ac != null)
                    {
                        if (ac.RoleID == 1)
                            Response.Redirect("/trang-chu");
                    }
                    loadFilter();
                    LoadData();
                }
            }
        }

        public void loadFilter()
        {
            ddlStatus.SelectedValue = "-1";
            var stafforder = AccountController.GetAllByRoleID(3);
            ddlStaffOrder.Items.Clear();
            ddlStaffOrder.Items.Insert(0, new ListItem("Chọn nhân viên đặt hàng", "0"));
            if (stafforder.Count > 0)
            {
                ddlStaffOrder.DataSource = stafforder;
                ddlStaffOrder.DataBind();
            }

            var staffsale = AccountController.GetAllByRoleID(6);
            ddlStaffSale.Items.Clear();
            ddlStaffSale.Items.Insert(0, new ListItem("Chọn nhân viên kinh doanh", "0"));
            if (staffsale.Count > 0)
            {
                ddlStaffSale.DataSource = staffsale;
                ddlStaffSale.DataBind();
            }

            var warehousefrom = WarehouseFromController.GetAllWithIsHidden(false);
            WarehouseFrom.Items.Clear();
            WarehouseFrom.Items.Insert(0, new ListItem("Chọn kho Trung Quốc", "0"));
            if (warehousefrom.Count > 0)
            {
                WarehouseFrom.DataSource = warehousefrom;
                WarehouseFrom.DataBind();
            }
        }

        #region Button status
        //tất cả
        protected void btnAll_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = -1;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //chưa đặt cọc
        protected void btn0_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 0;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã cọc
        protected void btn2_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 2;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đang vận chuyển quốc tế
        protected void btn3_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 3;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã mua hàng
        protected void btn4_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 4;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //ncc phát hàng
        protected void btn5_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 5;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã về kho tq
        protected void btn6_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 6;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã về kho vn
        protected void btn7_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 7;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //admin thanh toán
        protected void btn8_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 8;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã thanh toán
        protected void btn9_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 9;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã hoàn thành
        protected void btn10_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 10;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã hủy
        protected void btn1_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 1;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        #endregion

        private void LoadData()
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (ac != null)
            {
                int OrderType = 1;
                int stype = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["stype"]))
                {
                    stype = int.Parse(Request.QueryString["stype"]);
                    ddlType.SelectedValue = stype.ToString();
                }

                int sort = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["sort"]))
                {
                    sort = Convert.ToInt32(Request.QueryString["sort"]);
                    ddlSortType.SelectedValue = sort.ToString();
                }

                string fd = Request.QueryString["fd"];
                if (!string.IsNullOrEmpty(fd))
                    rFD.Text = fd;
                string td = Request.QueryString["td"];
                if (!string.IsNullOrEmpty(td))
                    rTD.Text = td;
                string priceTo = Request.QueryString["priceTo"];
                if (!string.IsNullOrEmpty(priceTo))
                    rPriceTo.Text = priceTo;
                string priceFrom = Request.QueryString["priceFrom"];
                if (!string.IsNullOrEmpty(priceFrom))
                    rPriceFrom.Text = priceFrom;
                string search = "";
                int hasVMD = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["hasMVD"]))
                {
                    hasVMD = Request.QueryString["hasMVD"].ToString().ToInt(0);
                    hdfCheckBox.Value = hasVMD.ToString();
                }
                string st = Request.QueryString["st"];
                if (!string.IsNullOrEmpty(st))
                {
                    var list = st.Split(',').ToList();

                    for (int j = 0; j < list.Count; j++)
                    {
                        for (int i = 0; i < ddlStatus.Items.Count; i++)
                        {
                            var item = ddlStatus.Items[i];
                            if (item.Value == list[j])
                            {
                                ddlStatus.Items[i].Selected = true;
                            }
                        }
                    }
                }
                int staff = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["staff"]))
                {
                    staff = Convert.ToInt32(Request.QueryString["staff"]);
                    ddlStaffOrder.SelectedValue = staff.ToString();
                }
                int sale = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["sale"]))
                {
                    sale = Convert.ToInt32(Request.QueryString["sale"]);
                    ddlStaffSale.SelectedValue = sale.ToString();
                }
                int khotq = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["khotq"]))
                {
                    khotq = Convert.ToInt32(Request.QueryString["khotq"]);
                    WarehouseFrom.SelectedValue = khotq.ToString();
                }
                string mvd = "";
                if (!string.IsNullOrEmpty(Request.QueryString["mvd"]))
                {
                    mvd = Request.QueryString["mvd"].ToString().Trim();
                    //txtSearchMVD.Text = mvd;
                }
                string mdh = "";
                if (!string.IsNullOrEmpty(Request.QueryString["mdh"]))
                {
                    mdh = Request.QueryString["mdh"].ToString().Trim();
                    //txtSearchMDH.Text = mdh;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["s"]))
                {
                    search = Request.QueryString["s"].ToString().Trim();
                    tSearchName.Text = search;
                }
                int page = 0;
                Int32 Page = GetIntFromQueryString("Page");
                if (Page > 0)
                {
                    page = Page - 1;
                }
                if (Request.QueryString["ot"] != null)
                {
                    OrderType = Request.QueryString["ot"].ToInt(1);
                }
                if (OrderType > 0)
                {
                    int total = 0;
                    var la = MainOrderController.GetOrderListFixed(Convert.ToInt32(ac.RoleID), Convert.ToInt32(ac.ID), OrderType, search, stype, fd, td, priceFrom, priceTo, st, staff, sale, Convert.ToBoolean(hasVMD), page, 20, mvd, mdh, sort, khotq);
                    if (la.Count > 0)
                        total = la[0].TotalRow;
                    pagingall(la, total);

                    var os = MainOrderController.GetAllByOrderType(OrderType);

                    var sttall = os.ToList();
                    var stt0 = os.Where(o => o.Status == 0).ToList();
                    var stt1 = os.Where(o => o.Status == 1).ToList();
                    var stt2 = os.Where(o => o.Status == 2).ToList();
                    var stt3 = os.Where(o => o.Status == 3).ToList();
                    var stt4 = os.Where(o => o.Status == 4).ToList();
                    var stt5 = os.Where(o => o.Status == 5).ToList();
                    var stt6 = os.Where(o => o.Status == 6).ToList();
                    var stt7 = os.Where(o => o.Status == 7).ToList();
                    var stt8 = os.Where(o => o.Status == 8).ToList();
                    var stt9 = os.Where(o => o.Status == 9).ToList();
                    var stt10 = os.Where(o => o.Status == 10).ToList();
                    if (Convert.ToInt32(ac.RoleID) == 3)
                    {
                        sttall = os.Where(o => o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        stt0 = os.Where(o => o.Status == 0 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        stt1 = os.Where(o => o.Status == 1 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        stt2 = os.Where(o => o.Status == 2 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        stt3 = os.Where(o => o.Status == 3 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        stt4 = os.Where(o => o.Status == 4 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        stt5 = os.Where(o => o.Status == 5 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        stt6 = os.Where(o => o.Status == 6 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        stt7 = os.Where(o => o.Status == 7 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        stt8 = os.Where(o => o.Status == 8 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        stt9 = os.Where(o => o.Status == 9 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        stt10 = os.Where(o => o.Status == 10 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                    }
                    else if (Convert.ToInt32(ac.RoleID) == 6)
                    {
                        sttall = os.Where(o => o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        stt0 = os.Where(o => o.Status == 0 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        stt1 = os.Where(o => o.Status == 1 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        stt2 = os.Where(o => o.Status == 2 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        stt3 = os.Where(o => o.Status == 3 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        stt4 = os.Where(o => o.Status == 4 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        stt5 = os.Where(o => o.Status == 5 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        stt6 = os.Where(o => o.Status == 6 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        stt7 = os.Where(o => o.Status == 7 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        stt8 = os.Where(o => o.Status == 8 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        stt9 = os.Where(o => o.Status == 9 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        stt10 = os.Where(o => o.Status == 10 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                    }
                    bttnAll.Text = "Tất cả (" + sttall.Count + ")";
                    btn0.Text = "Đơn mới (" + stt0.Count + ")";
                    btn1.Text = "Đơn hàng hủy (" + stt1.Count + ")";
                    btn2.Text = "Đã đặt cọc (" + stt2.Count + ")";
                    btn3.Text = "Đang vận chuyển Quốc tế (" + stt3.Count + ")";
                    btn4.Text = "Đã mua hàng (" + stt4.Count + ")";
                    btn5.Text = "Shop phát hàng (" + stt5.Count + ")";
                    btn6.Text = "Hàng về kho TQ (" + stt6.Count + ")";
                    btn7.Text = "Hàng về kho VN (" + stt7.Count + ")";
                    btn8.Text = "Admin đã thanh toán (" + stt8.Count + ")";
                    btn9.Text = "Đã thanh toán (" + stt9.Count + ")";
                    btn10.Text = "Đã hoàn thành (" + stt10.Count + ")";
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string stype = ddlType.SelectedValue;
            string searchname = tSearchName.Text.Trim();
            string fd = "";
            string td = "";
            string priceFrom = "";
            string priceTo = "";
            int SortType = Convert.ToInt32(ddlSortType.SelectedValue);
            //string hasVMD = "0";
            string hasVMD = hdfCheckBox.Value;
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            if (!string.IsNullOrEmpty(rFD.Text))
            {
                fd = rFD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rTD.Text))
            {
                td = rTD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rPriceFrom.Text))
            {
                priceFrom = rPriceFrom.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rPriceTo.Text))
            {
                priceTo = rPriceTo.Text.ToString();
            }
            string st = "";
            if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
            {
                List<string> myValues = new List<string>();
                for (int i = 0; i < ddlStatus.Items.Count; i++)
                {
                    var item = ddlStatus.Items[i];
                    if (item.Selected)
                    {
                        myValues.Add(item.Value);
                    }
                }
                st = String.Join(",", myValues.ToArray());
            }
            int staff = 0;
            if (Convert.ToInt32(ddlStaffOrder.SelectedValue) != 0)
            {
                staff = Convert.ToInt32(ddlStaffOrder.SelectedValue);
            }
            int sale = 0;
            if (Convert.ToInt32(ddlStaffSale.SelectedValue) != 0)
            {
                sale = Convert.ToInt32(ddlStaffSale.SelectedValue);
            }
            int khotq = 0;
            if (Convert.ToInt32(WarehouseFrom.SelectedValue) != 0)
            {
                khotq = Convert.ToInt32(WarehouseFrom.SelectedValue);
            }
            if (string.IsNullOrEmpty(stype) == true && string.IsNullOrEmpty(searchname) == true && fd == "" && td == "" && priceFrom == "" && priceTo == "" && string.IsNullOrEmpty(st) == true && staff == 0 && sale == 0 && khotq == 0 && hasVMD == "0")
            {
                Response.Redirect("orderlist?ot=" + uID + "&sort=" + SortType + "");
            }
            else
            {
                Response.Redirect("orderlist?ot=" + uID + "&stype=" + stype + "&s=" + searchname + "&fd=" + fd + "&td=" + td + "&priceFrom=" + priceFrom + "&priceTo=" + priceTo + "&st=" + st + "&staff=" + staff + "&sale=" + sale + "&khotq=" + khotq + "&hasMVD=" + hasVMD + "&sort=" + SortType + "");
            }
        }

        public class ListID
        {
            public int MainOrderID { get; set; }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            int OrderType = 1;
            int stype = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["stype"]))
            {
                stype = int.Parse(Request.QueryString["stype"]);
                ddlType.SelectedValue = stype.ToString();
            }

            int sort = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["sort"]))
            {
                sort = Convert.ToInt32(Request.QueryString["sort"]);
                ddlSortType.SelectedValue = sort.ToString();
            }

            string fd = Request.QueryString["fd"];
            if (!string.IsNullOrEmpty(fd))
                rFD.Text = fd;
            string td = Request.QueryString["td"];
            if (!string.IsNullOrEmpty(td))
                rTD.Text = td;
            string priceTo = Request.QueryString["priceTo"];
            if (!string.IsNullOrEmpty(priceTo))
                rPriceTo.Text = priceTo;
            string priceFrom = Request.QueryString["priceFrom"];
            if (!string.IsNullOrEmpty(priceFrom))
                rPriceFrom.Text = priceFrom;
            string search = "";
            int hasVMD = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["hasMVD"]))
            {
                hasVMD = Request.QueryString["hasMVD"].ToString().ToInt(0);
                hdfCheckBox.Value = hasVMD.ToString();
            }
            string st = Request.QueryString["st"];
            if (!string.IsNullOrEmpty(st))
            {
                var list = st.Split(',').ToList();

                for (int j = 0; j < list.Count; j++)
                {
                    for (int i = 0; i < ddlStatus.Items.Count; i++)
                    {
                        var item = ddlStatus.Items[i];
                        if (item.Value == list[j])
                        {
                            ddlStatus.Items[i].Selected = true;
                        }
                    }
                }
            }
            int staff = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["staff"]))
            {
                staff = Convert.ToInt32(Request.QueryString["staff"]);
                ddlStaffOrder.SelectedValue = staff.ToString();
            }
            int sale = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["sale"]))
            {
                sale = Convert.ToInt32(Request.QueryString["sale"]);
                ddlStaffSale.SelectedValue = sale.ToString();
            }
            string mvd = "";
            if (!string.IsNullOrEmpty(Request.QueryString["mvd"]))
            {
                mvd = Request.QueryString["mvd"].ToString().Trim();
                //txtSearchMVD.Text = mvd;
            }
            string mdh = "";
            if (!string.IsNullOrEmpty(Request.QueryString["mdh"]))
            {
                mdh = Request.QueryString["mdh"].ToString().Trim();
                //txtSearchMDH.Text = mdh;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                tSearchName.Text = search;
            }
            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }
            if (Request.QueryString["ot"] != null)
            {
                OrderType = Request.QueryString["ot"].ToInt(1);
            }
            if (OrderType > 0)
            {
                var la = MainOrderController.GetOrderListExcel(OrderType, search, stype, fd, td, priceFrom, priceTo, st, staff, sale, Convert.ToBoolean(hasVMD), mvd, mdh, sort);
                StringBuilder StrExport = new StringBuilder();
                StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
                StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
                StrExport.Append("<DIV  style='font-size:12px;'>");
                StrExport.Append("<table border=\"1\">");
                StrExport.Append("  <tr>");
                StrExport.Append("      <th><strong>ID</strong></th>");
                StrExport.Append("      <th><strong>Username</strong></th>");
                StrExport.Append("      <th><strong>Tổng tiền</strong></th>");
                StrExport.Append("      <th><strong>Tiền đã trả</strong></th>");
                StrExport.Append("      <th><strong>Tiền còn lại</strong></th>");
                StrExport.Append("      <th><strong>Tiền hàng trên Web</strong></th>");
                StrExport.Append("      <th><strong>Phí dịch vụ</strong></th>");
                StrExport.Append("      <th><strong>Phí ship TQ</strong></th>");
                StrExport.Append("      <th><strong>Phí kiểm hàng</strong></th>");
                StrExport.Append("      <th><strong>Phí đóng gỗ</strong></th>");
                StrExport.Append("      <th><strong>Phí bảo hiểm</strong></th>");
                StrExport.Append("      <th><strong>Phụ phí</strong></th>");
                //StrExport.Append("      <th><strong>Tiền mua thật</strong></th>");
                StrExport.Append("      <th><strong>Phí vận chuyển</strong></th>");
                //StrExport.Append("      <th><strong>Tổng tiền không cân nặng</strong></th>");
                StrExport.Append("      <th><strong>Cân nặng</strong></th>");
                //StrExport.Append("      <th><strong>Mã đơn hàng</strong></th>");
                //StrExport.Append("      <th><strong>Mã vận đơn</strong></th>");
                StrExport.Append("      <th><strong>Trạng thái</strong></th>");
                StrExport.Append("      <th><strong>NV kinh doanh</strong></th>");
                StrExport.Append("      <th><strong>NV đặt hàng</strong></th>");
                StrExport.Append("      <th><strong>Ngày tạo</strong></th>");
                StrExport.Append("      <th><strong>Ngày đặt cọc</strong></th>");
                StrExport.Append("      <th><strong>Ngày mua hàng</strong></th>");
                StrExport.Append("      <th><strong>Ngày shop phát hàng</strong></th>");
                StrExport.Append("      <th><strong>Ngày về kho TQ</strong></th>");
                StrExport.Append("      <th><strong>Ngày về kho VN</strong></th>");
                //StrExport.Append("      <th><strong>Ngày thanh toán</strong></th>");
                StrExport.Append("      <th><strong>Ngày hoàn thành</strong></th>");
                StrExport.Append("  </tr>");
                //double TotalPriceNotWeight = 0;
                foreach (var item in la)
                {
                    //TotalPriceNotWeight = Convert.ToDouble(item.TotalPriceVND) - Convert.ToDouble(item.FeeWeight);

                    //string TranOrder = "";
                    //string MainCode = "";
                    ////mã vận đơn
                    //var smallpack = SmallPackageController.GetByMainOrderID(item.ID);
                    //if (smallpack != null)
                    //{
                    //    foreach (var itemsm in smallpack)
                    //    {
                    //        TranOrder += "<p>" + itemsm.OrderTransactionCode + "</p>";
                    //    }

                    //}
                    //// mã đơn hàng
                    //var smallpack1 = MainOrderCodeController.GetAllByMainOrderID(item.ID);
                    //if (smallpack1 != null)
                    //{
                    //    foreach (var itemsm in smallpack1)
                    //    {
                    //        MainCode += "<p>" + itemsm.MainOrderCode + "</p>";
                    //    }
                    //}

                    StrExport.Append("  <tr>");
                    StrExport.Append("      <td>" + item.ID + "</td>");
                    StrExport.Append("      <td>" + item.Uname + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPriceVND)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.Deposit)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Math.Round(Convert.ToDouble(item.TotalPriceVND) - Convert.ToDouble(item.Deposit))) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.PriceVND)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.FeeBuyPro)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.FeeShipCN)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.IsCheckProductPrice)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.IsPackedPrice)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.InsuranceMoney)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalFeeSupport)) + "</td>");
                    //StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPriceReal)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.FeeWeight)) + "</td>");
                    //StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(TotalPriceNotWeight)) + "</td>");
                    StrExport.Append("      <td>" + item.Weight + "</td>");
                    //StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + MainCode + "</td>");
                    //StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + TranOrder + "</td>"); 
                    StrExport.Append("      <td>" + PJUtils.IntToRequestAdmin(Convert.ToInt32(item.Status)) + "</td>");
                    StrExport.Append("      <td>" + item.saler + "</td>");
                    StrExport.Append("      <td>" + item.dathang + "</td>");
                    StrExport.Append("      <td>" + item.CreatedDate + "</td>");
                    StrExport.Append("      <td>" + item.DepostiDate + "</td>");
                    StrExport.Append("      <td>" + item.DateBuy + "</td>");
                    StrExport.Append("      <td>" + item.ShopPhatHang + "</td>");
                    StrExport.Append("      <td>" + item.DateTQ + "</td>");
                    StrExport.Append("      <td>" + item.DateVN + "</td>");
                    //StrExport.Append("      <td>" + item.DatePay + "</td>");
                    StrExport.Append("      <td>" + item.CompleteDate + "</td>");
                    StrExport.Append("  </tr>");
                }
                StrExport.Append("</table>");
                StrExport.Append("</div></body></html>");
                string strFile = "ExcelReportOrderList.xls";
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

        //[WebMethod]
        //public static string CheckStaff(int MainOrderID)
        //{
        //    List<ListID> ldep = new List<ListID>();
        //    var list = HttpContext.Current.Session["ListStaff"] as List<ListID>;
        //    if (list != null)
        //    {
        //        if (list.Count > 0)
        //        {
        //            var check = list.Where(x => x.MainOrderID == MainOrderID).FirstOrDefault();
        //            if (check != null)
        //            {
        //                list.Remove(check);
        //            }
        //            else
        //            {
        //                ListID d = new ListID();
        //                d.MainOrderID = MainOrderID;
        //                list.Add(d);
        //            }
        //        }
        //        else
        //        {
        //            ListID d = new ListID();
        //            d.MainOrderID = MainOrderID;
        //            list.Add(d);
        //        }
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        return serializer.Serialize(list);
        //    }
        //    else
        //    {
        //        ListID d = new ListID();
        //        d.MainOrderID = MainOrderID;
        //        ldep.Add(d);
        //        HttpContext.Current.Session["ListStaff"] = ldep;
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        return serializer.Serialize(ldep);
        //    }
        //}

        #region Class
        public partial class MainOrderCodeShow
        {
            public string MainOrderCode { get; set; }
            public int MainOrderID { get; set; }
        }
        public partial class MainBarcodeShow
        {
            public string OrderTransactionCode { get; set; }
            public int MainOrderID { get; set; }
        }
        #endregion

        [WebMethod]
        public static string LoadInfor(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var com = MainOrderController.GetAllByID(ID.ToInt(0));
            if (com != null)
            {
                MainOrderCodeShow l = new MainOrderCodeShow();
                l.MainOrderID = com.ID;
                l.MainOrderCode = "";
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }

        [WebMethod]
        public static string LoadInfor2(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var com = MainOrderController.GetAllByID(ID.ToInt(0));
            if (com != null)
            {
                MainBarcodeShow l = new MainBarcodeShow();
                l.MainOrderID = com.ID;
                l.OrderTransactionCode = "";
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }

        [WebMethod]
        public static string UpdateBarCode(string MainOrderCode, int MainOrderID)
        {
            DateTime currentDate = DateTime.UtcNow.AddHours(7);
            string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (ac != null)
            {
                var o = MainOrderController.GetAllByID(MainOrderID);
                if (o != null)
                {
                    string ListMVD = "";
                    int mainOrderCodeID = 0;
                    int QuantityBarcode = 0;

                    var moc = MainOrderCodeController.GetAllByMainOrderID(MainOrderID);
                    if (moc != null)
                    {
                        if (moc.Count > 0)
                        {
                            for (int i = 0; i < moc.Count; i++)
                            {
                                var item = moc[i];
                                mainOrderCodeID = item.ID;
                            }
                            int UID = Convert.ToInt32(o.UID);
                            var user = AccountController.GetByID(UID);
                            var checkbarcode = SmallPackageController.GetByOrderTransactionCode(MainOrderCode);
                            if (checkbarcode == null)
                            {
                                SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(MainOrderID, UID, user.Username, 0,
                                MainOrderCode, "", 0, 0, 0, 1, "", DateTime.Now, username_current, mainOrderCodeID, 0);

                                var List = SmallPackageController.GetByMainOrderID(MainOrderID);
                                if (List.Count > 0)
                                {
                                    foreach (var itemsmp in List)
                                    {
                                        ListMVD += itemsmp.OrderTransactionCode + " | ";
                                    }
                                    QuantityBarcode = List.Count;
                                    MainOrderController.UpdateBarcode(MainOrderID, ListMVD);
                                    MainOrderController.UpdateQuantityBarcode(MainOrderID, QuantityBarcode);
                                }

                                HistoryOrderChangeController.Insert(MainOrderID, ac.ID, ac.Username, ac.Username +
                                " đã thêm mã vận đơn vào đơn hàng là: " + MainOrderCode, 9, currentDate);

                                return "ok";
                            }
                            else
                                return "exist";
                        }
                        else
                            return "none";
                    }
                    else
                        return "none";
                }
                else
                    return null;
            }
            else
                return null;
        }

        [WebMethod]
        public static string UpdateMainOrderCode(string MainOrderCode, int MainOrderID)
        {
            DateTime currentDate = DateTime.UtcNow.AddHours(7);
            string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (ac != null)
            {
                var o = MainOrderController.GetAllByID(MainOrderID);
                if (o != null)
                {
                    string ListMDH = "";
                    int Quantity = 0;

                    if (!string.IsNullOrEmpty(MainOrderCode))
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
                            HistoryOrderChangeController.Insert(MainOrderID, ac.ID, ac.Username, ac.Username +
                            " đã thêm mã shop Trung Quốc vào đơn hàng là: " + MainOrderCode, 9, currentDate);

                            return "ok";
                        }
                        else
                            return "exist";
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                return null;
        }

        #region Pagging
        public void pagingall(List<OrderGetSQL> acs, int total)
        {
            int PageSize = 20;
            if (total > 0)
            {
                int TotalItems = total;
                if (TotalItems % PageSize == 0)
                    PageCount = TotalItems / PageSize;
                else
                    PageCount = TotalItems / PageSize + 1;

                Int32 Page = GetIntFromQueryString("Page");

                if (Page == -1) Page = 1;
                int FromRow = (Page - 1) * PageSize;
                int ToRow = Page * PageSize - 1;
                if (ToRow >= TotalItems)
                    ToRow = TotalItems - 1;
                StringBuilder hcm = new StringBuilder();

                for (int i = 0; i < acs.Count; i++)
                {
                    var item = acs[i];

                    double TotalPrice = Convert.ToDouble(item.TotalPriceVND);
                    double AmountDeposit = Convert.ToDouble(item.AmountDeposit);
                    double Deposit = Convert.ToDouble(item.Deposit);
                    double MustPay = Math.Round(TotalPrice - Deposit, 0);
                    double MustDeposit = Math.Round(AmountDeposit - Deposit, 0);

                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.anhsanpham + "</td>");
                    hcm.Append("<td style=\"font-weight:bold\">");
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Tỷ giá: </span><span>" + string.Format("{0:N0}", Convert.ToDouble(item.Currency)) + " Đ</span></p>");
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Tổng tiền: </span><span>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPriceVND)) + " Đ</span></p>");
                    hcm.Append("<p class=\"s-txt blue-text no-wrap\"><span class=\"total\">Đã trả: </span><span>" + string.Format("{0:N0}", Convert.ToDouble(item.Deposit)) + " Đ</span></p>");
                    hcm.Append("<p class=\"s-txt red-text no-wrap\"><span class=\"total\">Còn lại: </span><span>" + string.Format("{0:N0}", Math.Round(Convert.ToDouble(item.TotalPriceVND) - Convert.ToDouble(item.Deposit))) + " Đ</span></p>");
                    hcm.Append("</td>");

                    hcm.Append("<td>" + item.Uname + "</td>");

                    #region NV đặt hàng
                    hcm.Append("<td>");
                    hcm.Append("<div>");
                    hcm.Append("<select name=\"\" onchange=\"ChooseDathang('" + item.ID + "', $(this))\" id=\"\">");
                    hcm.Append("  <option value=\"0\">Chọn nhân viên đặt hàng</option>");
                    var dathangs = AccountController.GetAllByRoleID(3);
                    if (dathangs.Count > 0)
                    {
                        foreach (var temp in dathangs)
                        {
                            if (temp.ID == item.DathangID)
                                hcm.Append("  <option selected value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                            else
                                hcm.Append("  <option value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                        }
                    }
                    hcm.Append("</select>");
                    hcm.Append("</div>");

                    hcm.Append("</td>");

                    #endregion
                    #region NV kinh doanh
                    hcm.Append("<td>");
                    hcm.Append("<div>");
                    hcm.Append("<select name=\"\" onchange=\"ChooseSaler('" + item.ID + "', $(this))\"  id=\"\">");
                    hcm.Append("  <option value=\"0\">Chọn nhân viên kinh doanh</option>");
                    var salers = AccountController.GetAllByRoleID(6);
                    if (salers.Count > 0)
                    {
                        foreach (var temp in salers)
                        {
                            if (temp.ID == item.SalerID)
                                hcm.Append("  <option selected value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                            else
                                hcm.Append("  <option value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                        }
                    }
                    hcm.Append("</select>");
                    hcm.Append("</div>");

                    hcm.Append("</td>");
                    #endregion
                    #region mdh--mvd
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"input-mvd\">");
                    hcm.Append("<div class=\"row\">");
                    hcm.Append("<div class=\"col s6 \">");
                    if (!string.IsNullOrEmpty(item.MainOrderCode))
                    {
                        if (item.Site == "1688")
                        {
                            hcm.Append("<a target=\"_blank\" href=\"https://trade.1688.com/order/new_step_order_detail.htm?orderId=" + item.MainOrderCode.Replace('|', ' ') + "\"><span class=\"value\">" + item.MainOrderCode.Replace('|', ' ') + "</span></a>");
                        }
                        else
                        {
                            hcm.Append("<a target=\"_blank\" href=\"https://trade.taobao.com/trade/detail/trade_order_detail.htm?biz_order_id=" + item.MainOrderCode.Replace('|', ' ') + "\"><span class=\"value\">" + item.MainOrderCode.Replace('|', ' ') + "</span></a>");
                        }
                        hcm.Append("<span class=\"value\" style=\"color:#F64302;\" data-fancybox data-src=\"#pop-up-form\" onclick=\"LoadMDH(" + item.ID + ")\" ><i class=\"material-icons\">add</i>Thêm mã shop</span>");
                    }
                    else
                    {
                        hcm.Append("<span class=\"value\" style=\"color:#F64302;\" data-fancybox data-src=\"#pop-up-form\" onclick=\"LoadMDH(" + item.ID + ")\" ><i class=\"material-icons\">add</i>Thêm mã shop</span>");
                    }
                    hcm.Append("</div>");
                    hcm.Append("<div class=\"col s6 \">");
                    if (!string.IsNullOrEmpty(item.Barcode))
                    {
                        hcm.Append("<span class=\"value\">" + item.Barcode.Replace('|', ' ') + "</span>");
                        hcm.Append("<span style=\"color:#F64302;\" class=\"value\" data-fancybox data-src=\"#mvd-up-form\" onclick=\"LoadMVD(" + item.ID + ")\" ><i class=\"material-icons\">add</i>Thêm mã vận đơn</span>");
                    }
                    else
                    {
                        hcm.Append("<span style=\"color:#F64302;\" class=\"value\" data-fancybox data-src=\"#mvd-up-form\" onclick=\"LoadMVD(" + item.ID + ")\" ><i class=\"material-icons\">add</i>Thêm mã vận đơn</span>");
                    }
                    hcm.Append("</div>");
                    hcm.Append("</div>");
                    hcm.Append("</div>");
                    hcm.Append("</td>");
                    #endregion

                    ///Trạng thái
                    hcm.Append("<td>");
                    hcm.Append(item.Cancel);
                    hcm.Append(item.Created);
                    hcm.Append(item.DepostiDate);
                    hcm.Append(item.DateBuy);
                    hcm.Append(item.AdPayDate);
                    hcm.Append(item.DateDelivery);
                    hcm.Append(item.DateTQ);
                    hcm.Append(item.DateLoading);
                    hcm.Append(item.DateVN);
                    hcm.Append(item.DatePay);
                    hcm.Append(item.CompleteDate);
                    hcm.Append("</td>");

                    hcm.Append("<td>");
                    hcm.Append(" <div class=\"action-table\">");
                    hcm.Append("<a href =\"OrderDetail.aspx?id=" + item.ID + "\" target=\"_blank\" data-position=\"top\" ><i class=\"material-icons\">edit</i><span>Sửa</span></a>");

                    if (item.Status == 0)
                    {
                        if (MustDeposit > 0)
                            hcm.Append("    <a href=\"javascript:;\" onclick=\"depositOrder('" + item.ID + "',$(this))\" data-position=\"top\"><i class=\"material-icons\">attach_money</i><span>Đặt cọc</span></a>");
                    }
                    if (item.Status > 6 && item.Status != 8)
                    {
                        if (MustPay > 0)
                            hcm.Append("    <a href=\"javascript:;\" onclick=\"payallorder('" + item.ID + "',$(this))\" data-position=\"top\"><i class=\"material-icons\">payment</i><span>Thanh toán</span></a>");

                    }
                    //hcm.Append("<a href =\"Pay-Order.aspx?id=" + item.ID + "\" target=\"_blank\" data-position=\"top\" ><i class=\"material-icons\">payment</i><span>Thanh toán</span></a>");
                    hcm.Append("</div>");
                    hcm.Append("</td>");
                    hcm.Append("</tr>");
                }
                ltr.Text = hcm.ToString();
            }
        }
        public static Int32 GetIntFromQueryString(String key)
        {
            Int32 returnValue = -1;
            String queryStringValue = HttpContext.Current.Request.QueryString[key];
            try
            {
                if (queryStringValue == null)
                    return returnValue;
                if (queryStringValue.IndexOf("#") > 0)
                    queryStringValue = queryStringValue.Substring(0, queryStringValue.IndexOf("#"));
                returnValue = Convert.ToInt32(queryStringValue);
            }
            catch
            { }
            return returnValue;
        }
        private int PageCount;
        protected void DisplayHtmlStringPaging1()
        {
            Int32 CurrentPage = Convert.ToInt32(Request.QueryString["Page"]);
            if (CurrentPage == -1) CurrentPage = 1;
            string[] strText = new string[4] { "Trang đầu", "Trang cuối", "Trang sau", "Trang trước" };
            if (PageCount > 1)
                Response.Write(GetHtmlPagingAdvanced(6, CurrentPage, PageCount, Context.Request.RawUrl, strText));
        }
        private static string GetPageUrl(int currentPage, string pageUrl)
        {
            pageUrl = Regex.Replace(pageUrl, "(\\?|\\&)*" + "Page=" + currentPage, "");
            if (pageUrl.IndexOf("?") > 0)
            {
                if (pageUrl.IndexOf("Page=") > 0)
                {
                    int a = pageUrl.IndexOf("Page=");
                    int b = pageUrl.Length;
                    pageUrl.Remove(a, b - a);
                }
                else
                {
                    pageUrl += "&Page={0}";
                }

            }
            else
            {
                pageUrl += "?Page={0}";
            }
            return pageUrl;
        }
        public static string GetHtmlPagingAdvanced(int pagesToOutput, int currentPage, int pageCount, string currentPageUrl, string[] strText)
        {
            //Nếu Số trang hiển thị là số lẻ thì tăng thêm 1 thành chẵn
            if (pagesToOutput % 2 != 0)
            {
                pagesToOutput++;
            }

            //Một nửa số trang để đầu ra, đây là số lượng hai bên.
            int pagesToOutputHalfed = pagesToOutput / 2;

            //Url của trang
            string pageUrl = GetPageUrl(currentPage, currentPageUrl);


            //Trang đầu tiên
            int startPageNumbersFrom = currentPage - pagesToOutputHalfed; ;

            //Trang cuối cùng
            int stopPageNumbersAt = currentPage + pagesToOutputHalfed; ;

            StringBuilder output = new StringBuilder();

            //Nối chuỗi phân trang
            //output.Append("<div class=\"paging\">");
            //output.Append("<ul class=\"paging_hand\">");

            //Link First(Trang đầu) và Previous(Trang trước)
            if (currentPage > 1)
            {
                //output.Append("<li class=\"UnselectedPrev \" ><a title=\"" + strText[0] + "\" href=\"" + string.Format(pageUrl, 1) + "\">|<</a></li>");
                //output.Append("<li class=\"UnselectedPrev\" ><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\"><i class=\"fa fa-angle-left\"></i></a></li>");
                output.Append("<a class=\"prev-page pagi-button\" title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\">Prev</a>");
                //output.Append("<span class=\"Unselect_prev\"><a href=\"" + string.Format(pageUrl, currentPage - 1) + "\"></a></span>");
            }

            /******************Xác định startPageNumbersFrom & stopPageNumbersAt**********************/
            if (startPageNumbersFrom < 1)
            {
                startPageNumbersFrom = 1;

                //As page numbers are starting at one, output an even number of pages.  
                stopPageNumbersAt = pagesToOutput;
            }

            if (stopPageNumbersAt > pageCount)
            {
                stopPageNumbersAt = pageCount;
            }

            if ((stopPageNumbersAt - startPageNumbersFrom) < pagesToOutput)
            {
                startPageNumbersFrom = stopPageNumbersAt - pagesToOutput;
                if (startPageNumbersFrom < 1)
                {
                    startPageNumbersFrom = 1;
                }
            }
            /******************End: Xác định startPageNumbersFrom & stopPageNumbersAt**********************/

            //Các dấu ... chỉ những trang phía trước  
            if (startPageNumbersFrom > 1)
            {
                output.Append("<a href=\"" + string.Format(GetPageUrl(currentPage - 1, pageUrl), startPageNumbersFrom - 1) + "\">&hellip;</a>");
            }

            //Duyệt vòng for hiển thị các trang
            for (int i = startPageNumbersFrom; i <= stopPageNumbersAt; i++)
            {
                if (currentPage == i)
                {
                    output.Append("<a class=\"pagi-button current-active\">" + i.ToString() + "</a>");
                }
                else
                {
                    output.Append("<a class=\"pagi-button\" href=\"" + string.Format(pageUrl, i) + "\">" + i.ToString() + "</a>");
                }
            }

            //Các dấu ... chỉ những trang tiếp theo  
            if (stopPageNumbersAt < pageCount)
            {
                output.Append("<a href=\"" + string.Format(pageUrl, stopPageNumbersAt + 1) + "\">&hellip;</a>");
            }

            //Link Next(Trang tiếp) và Last(Trang cuối)
            if (currentPage != pageCount)
            {
                //output.Append("<span class=\"Unselect_next\"><a href=\"" + string.Format(pageUrl, currentPage + 1) + "\"></a></span>");
                //output.Append("<li class=\"UnselectedNext\" ><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\"><i class=\"fa fa-angle-right\"></i></a></li>");
                output.Append("<a class=\"next-page pagi-button\" title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\">Next</a>");
                //output.Append("<li class=\"UnselectedNext\" ><a title=\"" + strText[3] + "\" href=\"" + string.Format(pageUrl, pageCount) + "\">>|</a></li>");
            }
            //output.Append("</ul>");
            //output.Append("</div>");
            return output.ToString();
        }
        #endregion
        public class Danhsachorder
        {
            //public tbl_MainOder morder { get; set; }
            public int ID { get; set; }
            public int STT { get; set; }
            public string ProductImage { get; set; }
            public string ShopID { get; set; }
            public string ShopName { get; set; }
            public string TotalPriceVND { get; set; }
            public string Deposit { get; set; }
            public int UID { get; set; }
            public string CreatedDate { get; set; }
            public string statusstring { get; set; }
            public string username { get; set; }
            public string dathang { get; set; }
            public string kinhdoanh { get; set; }
            public string khotq { get; set; }
            public string khovn { get; set; }
        }

        protected void btnSearchMVD_Click(object sender, EventArgs e)
        {
            string mvd = txtSearchMVD.Text.Trim();
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(0);
            }
            if (!string.IsNullOrEmpty(mvd))
            {
                Response.Redirect("orderlist?ot=" + uID + "&mvd=" + mvd);
            }
            else
            {
                Response.Redirect("orderlist?ot=" + uID);
            }
        }

        protected void btnSearchMDH_Click(object sender, EventArgs e)
        {
            string mdh = txtSearchMDH.Text.Trim();
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            if (!string.IsNullOrEmpty(mdh))
            {
                Response.Redirect("orderlist?ot=" + uID + "&mdh=" + mdh);
            }
            else
            {
                Response.Redirect("orderlist?ot=" + uID);
            }
        }
        protected void btnDeposit_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int OID = hdfOrderID.Value.ToInt();
                if (OID > 0)
                {
                    var o = MainOrderController.GetAllByID(OID);
                    if (o != null)
                    {
                        int UIDU = Convert.ToInt32(o.UID);
                        var user = AccountController.GetByID(UIDU);
                        if (user != null)
                        {
                            double orderdeposited = 0;
                            double amountdeposit = 0;

                            if (o.Deposit.ToFloat(0) > 0)
                                orderdeposited = Math.Round(Convert.ToDouble(o.Deposit), 0);

                            if (o.AmountDeposit.ToFloat(0) > 0)
                                amountdeposit = Math.Round(Convert.ToDouble(o.AmountDeposit), 0);

                            double custDeposit = amountdeposit - orderdeposited;

                            double userwallet = Math.Round(Convert.ToDouble(user.Wallet), 0);

                            if (userwallet >= custDeposit)
                            {
                                double wallet = userwallet - custDeposit;
                                wallet = Math.Round(wallet, 0);

                                int st = TransactionController.DepositAll(UIDU, wallet, currentDate, obj_user.Username, o.ID, 2, o.Status.Value, amountdeposit.ToString(), custDeposit, obj_user.Username + " đã đặt cọc đơn hàng: " + o.ID, 1, 1, 2);
                                if (st == 1)
                                    PJUtils.ShowMessageBoxSwAlert("Đặt cọc đơn hàng thành công.", "s", true, Page);
                                else
                                    PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý.", "e", true, Page);
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của khách hàng này không đủ để đặt cọc đơn hàng.", "e", true, Page);
                            }
                        }
                    }
                }
            }
        }
        protected void btnPay_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                int id = hdfOrderID.Value.ToInt();
                DateTime currentDate = DateTime.Now;
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
                    }
                }
            }
        }


        [WebMethod]
        public static string UpdateStaff(int OrderID, int StaffID, int Type)
        {
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.UtcNow.AddHours(7);
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0 || obj_user.RoleID == 2)
                {
                    var mo = MainOrderController.GetAllByID(OrderID);
                    if (mo != null)
                    {
                        if (Type == 1) //1:saler - 2:dathang
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
                            if (StaffID > 0)
                            {
                                if (StaffID == salerID_old)
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, salerID_old);
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
                                        var sale = AccountController.GetByID(StaffID);
                                        if (sale != null)
                                        {
                                            salerName = sale.Username;
                                            var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                            int d = CreatedDate.Subtract(createdDate).Days;
                                            if (d > 90)
                                            {
                                                double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                                StaffIncomeController.Insert(mo.ID, per.ToString(), salepercentaf3m.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                                CreatedDate, currentDate, username);
                                            }
                                            else
                                            {
                                                double per = Math.Round(feebp * salepercent / 100, 0);
                                                StaffIncomeController.Insert(mo.ID, per.ToString(), salepercent.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                                CreatedDate, currentDate, username);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, salerID_old);
                                    if (staff != null)
                                    {
                                        StaffIncomeController.Delete(staff.ID);
                                    }
                                    var sale = AccountController.GetByID(StaffID);
                                    if (sale != null)
                                    {
                                        salerName = sale.Username;
                                        var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                        int d = CreatedDate.Subtract(createdDate).Days;
                                        if (d > 90)
                                        {
                                            double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                            StaffIncomeController.Insert(mo.ID, per.ToString(), salepercentaf3m.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                            CreatedDate, currentDate, username);
                                        }
                                        else
                                        {
                                            double per = Math.Round(feebp * salepercent / 100, 0);
                                            StaffIncomeController.Insert(mo.ID, per.ToString(), salepercent.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                            CreatedDate, currentDate, username);
                                        }
                                        NotificationsController.Inser(sale.ID,
                                                              sale.Username, mo.ID,
                                                              "Có đơn hàng mới ID là: " + mo.ID,
                                                              1, CreatedDate, username, false);
                                    }
                                }
                            }
                            #endregion

                            MainOrderController.UpdateStaff(mo.ID, StaffID, Convert.ToInt32(mo.DathangID), Convert.ToInt32(mo.KhoTQID), Convert.ToInt32(mo.KhoVNID));
                        }
                        else
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
                            #region Đặt hàng
                            if (StaffID > 0)
                            {
                                if (StaffID == dathangID_old)
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, dathangID_old);
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
                                        var dathang = AccountController.GetByID(StaffID);
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
                                                StaffIncomeController.Insert(mo.ID, totalpriceloi.ToString(), dathangpercent.ToString(), StaffID, dathangName, 3, 1,
                                                    income.ToString(), false, CreatedDate, currentDate, username);
                                            }
                                            else
                                            {
                                                StaffIncomeController.Insert(mo.ID, "0", dathangpercent.ToString(), StaffID, dathangName, 3, 1, "0", false,
                                                CreatedDate, currentDate, username);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, dathangID_old);
                                    if (staff != null)
                                    {
                                        StaffIncomeController.Delete(staff.ID);
                                    }
                                    var dathang = AccountController.GetByID(StaffID);
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

                                            StaffIncomeController.Insert(mo.ID, totalpriceloi.ToString(), dathangpercent.ToString(), StaffID, dathangName, 3, 1,
                                                income.ToString(), false, CreatedDate, currentDate, username);
                                        }
                                        else
                                        {
                                            StaffIncomeController.Insert(mo.ID, "0", dathangpercent.ToString(), StaffID, dathangName, 3, 1, "0", false,
                                            CreatedDate, currentDate, username);
                                        }
                                        NotificationsController.Inser(dathang.ID,
                                                                 dathang.Username, mo.ID,
                                                                 "Có đơn hàng mới ID là: " + mo.ID, 1,
                                                                  CreatedDate, username, false);
                                    }
                                }
                            }
                            #endregion
                            MainOrderController.UpdateStaff(mo.ID, Convert.ToInt32(mo.SalerID), StaffID, Convert.ToInt32(mo.KhoTQID), Convert.ToInt32(mo.KhoVNID));
                        }
                        return "ok";
                    }
                }
                else
                {
                    return "notpermision";
                }
            }
            return "null";
        }

        protected void btnUpdateStaff_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.UtcNow.AddHours(7);
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0 || obj_user.RoleID == 2)
                {
                    int Type = hdfType.Value.ToInt(0);
                    int StaffID = hdfStaffID.Value.ToInt(0);
                    var list = HttpContext.Current.Session["ListStaff"] as List<ListID>;
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            var mo = MainOrderController.GetAllByID(item.MainOrderID);
                            if (mo != null)
                            {
                                if (Type == 2) //1:saler - 2:dathang
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
                                    if (StaffID > 0)
                                    {
                                        if (StaffID == salerID_old)
                                        {
                                            var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, salerID_old);
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
                                                var sale = AccountController.GetByID(StaffID);
                                                if (sale != null)
                                                {
                                                    salerName = sale.Username;
                                                    var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                                    int d = CreatedDate.Subtract(createdDate).Days;
                                                    if (d > 90)
                                                    {
                                                        double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                                        StaffIncomeController.Insert(mo.ID, per.ToString(), salepercentaf3m.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                                        CreatedDate, currentDate, username);
                                                    }
                                                    else
                                                    {
                                                        double per = Math.Round(feebp * salepercent / 100, 0);
                                                        StaffIncomeController.Insert(mo.ID, per.ToString(), salepercent.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                                        CreatedDate, currentDate, username);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, salerID_old);
                                            if (staff != null)
                                            {
                                                StaffIncomeController.Delete(staff.ID);
                                            }
                                            var sale = AccountController.GetByID(StaffID);
                                            if (sale != null)
                                            {
                                                salerName = sale.Username;
                                                var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                                int d = CreatedDate.Subtract(createdDate).Days;
                                                if (d > 90)
                                                {
                                                    double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                                    StaffIncomeController.Insert(mo.ID, per.ToString(), salepercentaf3m.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                                    CreatedDate, currentDate, username);
                                                }
                                                else
                                                {
                                                    double per = Math.Round(feebp * salepercent / 100, 0);
                                                    StaffIncomeController.Insert(mo.ID, per.ToString(), salepercent.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                                    CreatedDate, currentDate, username);
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    MainOrderController.UpdateStaff(mo.ID, StaffID, Convert.ToInt32(mo.DathangID), Convert.ToInt32(mo.KhoTQID), Convert.ToInt32(mo.KhoVNID));
                                }
                                else
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
                                    #region Đặt hàng
                                    if (StaffID > 0)
                                    {
                                        if (StaffID == dathangID_old)
                                        {
                                            var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, dathangID_old);
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
                                                var dathang = AccountController.GetByID(StaffID);
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
                                                        StaffIncomeController.Insert(mo.ID, totalpriceloi.ToString(), dathangpercent.ToString(), StaffID, dathangName, 3, 1,
                                                            income.ToString(), false, CreatedDate, currentDate, username);
                                                    }
                                                    else
                                                    {
                                                        StaffIncomeController.Insert(mo.ID, "0", dathangpercent.ToString(), StaffID, dathangName, 3, 1, "0", false,
                                                        CreatedDate, currentDate, username);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, dathangID_old);
                                            if (staff != null)
                                            {
                                                StaffIncomeController.Delete(staff.ID);
                                            }
                                            var dathang = AccountController.GetByID(StaffID);
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

                                                    StaffIncomeController.Insert(mo.ID, totalpriceloi.ToString(), dathangpercent.ToString(), StaffID, dathangName, 3, 1,
                                                        income.ToString(), false, CreatedDate, currentDate, username);
                                                }
                                                else
                                                {
                                                    StaffIncomeController.Insert(mo.ID, "0", dathangpercent.ToString(), StaffID, dathangName, 3, 1, "0", false,
                                                    CreatedDate, currentDate, username);
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    MainOrderController.UpdateStaff(mo.ID, Convert.ToInt32(mo.SalerID), StaffID, Convert.ToInt32(mo.KhoTQID), Convert.ToInt32(mo.KhoVNID));
                                }
                            }
                        }
                        Session["ListStaff"] = null;
                        PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công!", "s", true, Page);
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Tài khoản của bạn không có quyền", "e", false, Page);
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Tài khoản của bạn không có quyền", "e", false, Page);
                }
            }
        }
    }
}