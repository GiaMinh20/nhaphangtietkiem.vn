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
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Web.Script.Serialization;

namespace NHST.manager
{
    public partial class transportation_list : System.Web.UI.Page
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
                        if (ac.RoleID == 1)
                            Response.Redirect("/trang-chu");
                    LoadDDL();
                    LoadData();
                }
            }
        }


        private void LoadData()
        {
            ddlStatus.SelectedValue = "-1";
            string stype = Request.QueryString["stype"];
            if (!string.IsNullOrEmpty(stype))
            {
                select_byType.SelectedValue = stype;

            }
            string wfrom = Request.QueryString["wfrom"];
            if (!string.IsNullOrEmpty(wfrom))
                ddlWarehouseFrom.SelectedValue = wfrom;

            string wto = Request.QueryString["wto"];
            if (!string.IsNullOrEmpty(wto))
                ddlWarehouseTo.SelectedValue = wto;

            string shippingtype = Request.QueryString["ship"];
            if (!string.IsNullOrEmpty(shippingtype))
                ddlShippingType.SelectedValue = shippingtype;

            string priceTo = Request.QueryString["priceTo"];
            if (!string.IsNullOrEmpty(priceTo))
                rPriceTo.Text = priceTo;

            string priceFrom = Request.QueryString["priceFrom"];
            if (!string.IsNullOrEmpty(priceFrom))
                rPriceFrom.Text = priceFrom;

            string status1 = Request.QueryString["st"];
            if (!string.IsNullOrEmpty(status1))
            {
                var listSTT = status1.Split(',').ToList();
                foreach (var item in listSTT)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        ddlStatus.Items.FindByValue(item).Selected = true;
                    }
                }
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

            string search = "";
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
            var la = TransportationOrderController.GetBySQLWithDK(stype, search, fd, td, wfrom, wto, shippingtype, status1, priceFrom, priceTo, page, 20, sort);
            int total = TransportationOrderController.GetTotalBySQL(stype, search, fd, td, wfrom, wto, shippingtype, status1, priceFrom, priceTo);
            pagingall(la, total);
        }

        #region Pagging
        public void pagingall(List<TransportationOrderController.TransportationOrderList> acs, int total)
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
                var list = HttpContext.Current.Session["ListStaff"] as List<ListID>;
                for (int i = 0; i < acs.Count; i++)
                {
                    var item = acs[i];

                    double MustPay = 0;
                    double TotalPrice = 0;
                    if (Convert.ToDouble(item.TotalPrice) > 0)
                        TotalPrice = Convert.ToDouble(item.TotalPrice);

                    double Deposited = 0;
                    if (Convert.ToDouble(item.Deposited) > 0)
                        Deposited = Convert.ToDouble(item.Deposited);

                    MustPay = Math.Round(TotalPrice - Deposited, 0);

                    hcm.Append("<tr>");
                    hcm.Append("<td>");
                    if (list != null)
                    {
                        var check = list.Where(x => x.MainOrderID == item.ID).SingleOrDefault();
                        if (check != null)
                        {
                            hcm.Append(" <label><input type=\"checkbox\" checked onchange=\"CheckStaff(" + item.ID + ")\"  data-id=\"" + item.ID + "\"><span></span></label>");
                        }
                        else
                        {
                            hcm.Append(" <label><input type=\"checkbox\" onchange=\"CheckStaff(" + item.ID + ")\"  data-id=\"" + item.ID + "\"><span></span></label>");
                        }
                    }
                    else
                    {
                        hcm.Append(" <label><input type=\"checkbox\" onchange=\"CheckStaff(" + item.ID + ")\"  data-id=\"" + item.ID + "\"><span></span></label>");
                    }

                    hcm.Append("</td>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.Username + "</td>");
                    hcm.Append("<td>" + item.StatusName + "</td>");
                    hcm.Append("<td>" + item.Barcode + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPrice)) + " VNĐ</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.Deposited)) + " VNĐ</td>");
                    hcm.Append("<td>" + Math.Round(Convert.ToDouble(item.TotalWeight), 2) + " Kg</td>");
                    hcm.Append("<td>" + WarehouseFromController.GetByID(item.WarehouseFromID).WareHouseName + "</td>");
                    hcm.Append("<td>" + WarehouseController.GetByID(item.WarehouseID).WareHouseName + "</td>");
                    hcm.Append("<td>" + ShippingTypeToWareHouseController.GetByID(item.ShippingTypeID).ShippingTypeName + "</td>");
                    hcm.Append("<td>" + PJUtils.generateTransportationStatusNew(item.Status) + "</td>");
                    hcm.Append("<td>" + item.CreatedDateString + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"action-table\">");
                    hcm.Append("<a href=\"transportationdetail.aspx?id=" + item.ID + "\" target=\"_blank\" data-position=\"top\"> ");
                    hcm.Append(" <i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                    if (item.Status > 4 && MustPay > 0)
                    {
                        hcm.Append("    <a href=\"javascript:;\" onclick=\"payallorder('" + item.ID + "',$(this))\" data-position=\"top\"><i class=\"material-icons\">payment</i><span>Thanh toán</span></a>");
                    }   
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

        #region grid event
        public void LoadDDL()
        {
            ddlWarehouseFrom.Items.Clear();
            ddlWarehouseFrom.Items.Insert(0, new ListItem("---Tất cả---", "0"));
            ddlWarehouseFrom.SelectedIndex = 0;
            var warehousefrom = WarehouseFromController.GetAllWithIsHidden(false);
            if (warehousefrom.Count > 0)
            {
                foreach (var item in warehousefrom)
                {
                    ListItem listitem = new ListItem(item.WareHouseName, item.ID.ToString());
                    ddlWarehouseFrom.Items.Add(listitem);
                }
            }
            ddlWarehouseFrom.DataBind();
            ddlWarehouseTo.Items.Clear();
            ddlWarehouseTo.Items.Insert(0, new ListItem("---Tất cả---", "0"));
            ddlWarehouseTo.SelectedIndex = 0;
            var warehouse = WarehouseController.GetAllWithIsHidden(false);
            if (warehouse.Count > 0)
            {
                foreach (var item in warehouse)
                {
                    ListItem listitem = new ListItem(item.WareHouseName, item.ID.ToString());
                    ddlWarehouseTo.Items.Add(listitem);
                }
            }
            ddlWarehouseTo.DataBind();
            ddlShippingType.Items.Clear();
            ddlShippingType.Items.Insert(0, new ListItem("---Tất cả---", "0"));
            ddlShippingType.SelectedIndex = 0;

            var shippingtype = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shippingtype.Count > 0)
            {
                foreach (var item in shippingtype)
                {
                    ListItem listitem = new ListItem(item.ShippingTypeName, item.ID.ToString());
                    ddlShippingType.Items.Add(listitem);
                }
            }
            ddlShippingType.DataBind();
        }

        protected void r_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //string username_current = Session["userLoginSystem"].ToString();
            //tbl_Account ac = AccountController.GetByUsername(username_current);
            //if (ac != null)
            //{
            //    string s = tSearchName.Text.Trim();
            //    int wfrom = ddlWarehouseFrom.SelectedValue.ToInt();
            //    int wto = ddlWarehouseTo.SelectedValue.ToInt();
            //    int shippingtype = ddlShippingType.SelectedValue.ToInt();
            //    double priceFrom = !string.IsNullOrEmpty(rPriceFrom.Text) ? Convert.ToDouble(rPriceFrom.Text) : 0;
            //    double priceTo = !string.IsNullOrEmpty(rPriceTo.Text) ? Convert.ToDouble(rPriceTo.Text) : 0;
            //    string fromdate = rFD.Text.ToString();
            //    string todate = rFD.Text.ToString();
            //    string status1 = hdfStatus.Value;
            //    List<tbl_TransportationOrder> tList = new List<tbl_TransportationOrder>();
            //    var ts = TransportationOrderController.GetAll("");
            //    if (!string.IsNullOrEmpty(s))
            //    {
            //        foreach (var t in ts)
            //        {
            //            int tID = t.ID;
            //            var check = false;
            //            var transportationDetails = TransportationOrderDetailController.GetByTransportationOrderID(tID);
            //            if (transportationDetails.Count > 0)
            //            {
            //                foreach (var d in transportationDetails)
            //                {
            //                    if (d.TransportationOrderCode == s)
            //                    {
            //                        check = true;
            //                    }
            //                }
            //            }
            //            if (check == false)
            //            {
            //                var smallpackages = SmallPackageController.GetByTransportationOrderID(tID);
            //                if (smallpackages.Count > 0)
            //                {
            //                    foreach (var small in smallpackages)
            //                    {
            //                        if (small.OrderTransactionCode == s)
            //                        {
            //                            check = true;
            //                        }
            //                    }
            //                }
            //            }
            //            if (check == true)
            //            {
            //                tList.Add(t);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        tList = ts;
            //    }
            //    if (wfrom > 0)
            //    {
            //        tList = tList.Where(t => t.WarehouseFromID == wfrom).ToList();
            //    }
            //    if (wto > 0)
            //    {
            //        tList = tList.Where(t => t.WarehouseID == wto).ToList();
            //    }
            //    if (shippingtype > 0)
            //    {
            //        tList = tList.Where(t => t.ShippingTypeID == shippingtype).ToList();
            //    }
            //    if (priceTo > 0)
            //    {
            //        tList = tList.Where(t => t.TotalPrice >= priceFrom && t.TotalPrice <= priceTo).ToList();
            //    }
            //    if (!string.IsNullOrEmpty(fromdate))
            //    {
            //        if (!string.IsNullOrEmpty(todate))
            //        {
            //            DateTime fd = DateTime.Parse(fromdate);
            //            DateTime td = DateTime.Parse(todate);
            //            tList = tList.Where(t => t.CreatedDate >= fd && t.CreatedDate <= td).ToList();
            //        }
            //        else
            //        {
            //            DateTime fd = DateTime.Parse(fromdate);
            //            tList = tList.Where(t => t.CreatedDate >= fd).ToList();
            //        }
            //    }
            //    else
            //    {
            //        if (!string.IsNullOrEmpty(todate))
            //        {
            //            DateTime td = DateTime.Parse(todate);
            //            tList = tList.Where(t => t.CreatedDate <= td).ToList();
            //        }
            //    }
            //    if (status1 != "-1")
            //    {
            //        var la1 = new List<tbl_TransportationOrder>();
            //        string[] sts = status1.Split(',');
            //        for (int i = 0; i < sts.Length; i++)
            //        {
            //            int stat = sts[i].ToInt();
            //            if (stat > -1)
            //            {
            //                var la2 = new List<tbl_TransportationOrder>();
            //                la2 = tList.Where(o => o.Status == stat).ToList();
            //                if (la2.Count > 0)
            //                {
            //                    foreach (var item in la2)
            //                    {
            //                        la1.Add(item);
            //                    }
            //                }
            //            }
            //        }
            //        la1 = la1.OrderByDescending(o => o.ID).ToList();
            //        gr.VirtualItemCount = la1.Count;
            //        gr.DataSource = la1;
            //    }
            //    else
            //    {
            //        if(tList.Count>0)
            //        {
            //            gr.VirtualItemCount = tList.Count;
            //            gr.DataSource = tList;
            //        }

            //    }

            //}
        }

        protected void r_ItemCommand(object sender, GridCommandEventArgs e)
        {
            var g = e.Item as GridDataItem;
            if (g == null) return;
        }

        protected void gr_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {

        }
        #endregion
        #region button event
        protected void btnPay_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                int id = hdfOrderID.Value.ToInt();
                if (id > 0)
                {
                    var t = TransportationOrderController.GetByID(id);
                    if (t != null)
                    {
                        int UID = Convert.ToInt32(t.UID);
                        if (t.Status != 0)
                        {
                            double MustPay = 0;
                            double TotalPrice = 0;
                            if (Convert.ToDouble(t.TotalPrice) > 0)
                                TotalPrice = Convert.ToDouble(t.TotalPrice);

                            double Deposited = 0;
                            if (Convert.ToDouble(t.Deposited) > 0)
                                Deposited = Convert.ToDouble(t.Deposited);

                            MustPay = Math.Round(TotalPrice - Deposited, 0);

                            var userDathang = AccountController.GetByID(Convert.ToInt32(t.UID));
                            double currentWallet = 0;
                            if (Convert.ToDouble(userDathang.Wallet) > 0)
                                currentWallet = Convert.ToDouble(userDathang.Wallet);

                            if (MustPay > 0)
                            {
                                if (currentWallet >= MustPay)
                                {
                                    double walletLeft = Math.Round(currentWallet - MustPay, 0);
                                    int a = TransactionController.PayVanChuyenHo(t.ID, TotalPrice, 6, currentDate, userDathang.Username, UID, walletLeft, 0, MustPay, username + " đã thanh toán đơn hàng vận chuyển hộ: " + t.ID + ".", 1, 8);
                                    if (a == 1)
                                    {
                                        //PayAllOrderHistoryController.Insert(id, MustPay, UID, "", pContent1.Text, 9, currentDate, username_current);
                                        //PayOrderHistoryController.Insert(id, UID, 9, MustPay, 2, currentDate, username_current);
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
                                                        NotificationsController.Inser(admin.ID,
                                                        admin.Username, t.ID, "Đơn hàng vận chuyển hộ " + t.ID + " đã được thanh toán.", 10, currentDate, username, false);
                                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                        string datalink = "" + strUrl + "manager/chi-tet-don-hang-van-chuyen-ho/" + t.ID;
                                                        PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng vận chuyển hộ " + t.ID + " đã được thanh toán.", datalink);
                                                    }
                                                }
                                            }
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
                                    PJUtils.ShowMessageBoxSwAlert("Số dư trong ví của khách hàng đủ để thanh toán", "i", true, Page);
                                }
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Đơn hàng không còn nợ để thanh toán", "i", true, Page);
                            }
                        }    
                    }    
                }    
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchname = tSearchName.Text.Trim();
            string fd = "";
            string td = "";
            string stype = "";
            string wfrom = "";
            string wto = "";
            string shippingtype = "";
            string priceTo = "";
            string priceFrom = "";
            string status1 = "";
            int SortType = Convert.ToInt32(ddlSortType.SelectedValue);

            if (!string.IsNullOrEmpty(rFD.Text))
            {
                fd = rFD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rTD.Text))
            {
                td = rTD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(select_byType.SelectedValue))
            {
                stype = select_byType.SelectedValue;
            }
            if (!string.IsNullOrEmpty(ddlWarehouseFrom.SelectedValue))
            {
                wfrom = ddlWarehouseFrom.SelectedValue;
            }
            if (!string.IsNullOrEmpty(ddlWarehouseTo.SelectedValue))
            {
                wto = ddlWarehouseTo.SelectedValue;
            }
            if (!string.IsNullOrEmpty(ddlShippingType.SelectedValue))
            {
                shippingtype = ddlShippingType.SelectedValue;
            }
            if (!string.IsNullOrEmpty(rPriceFrom.Text))
            {
                priceFrom = rPriceFrom.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rPriceTo.Text))
            {
                priceTo = rPriceTo.Text.ToString();
            }
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
                status1 = String.Join(",", myValues.ToArray());
            }
            if (searchname == "" && fd == "" && td == "" && stype == "" && wfrom == "0" && wto == "0" && shippingtype == "0" && priceTo == "" && priceTo == "" && status1 == "")
            {
                Response.Redirect("transportation-list?sort=" + SortType + "");
            }
            else
            {
                Response.Redirect("transportation-list?stype=" + stype + "&s=" + searchname + "&fd=" + fd + "&td=" + td + "&wfrom=" + wfrom + "&wto=" + wto + "&priceFrom=" + priceFrom + "&priceTo=" + priceTo + "&st=" + status1 + "&ship=" + shippingtype + "&sort=" + SortType);
            }
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

        public class ListID
        {
            public int MainOrderID { get; set; }
        }

        [WebMethod]
        public static string CheckStaff(int MainOrderID)
        {
            List<ListID> ldep = new List<ListID>();
            var list = HttpContext.Current.Session["ListStaff"] as List<ListID>;
            if (list != null)
            {
                if (list.Count > 0)
                {
                    var check = list.Where(x => x.MainOrderID == MainOrderID).FirstOrDefault();
                    if (check != null)
                    {
                        list.Remove(check);
                    }
                    else
                    {
                        ListID d = new ListID();
                        d.MainOrderID = MainOrderID;
                        list.Add(d);
                    }
                }
                else
                {
                    ListID d = new ListID();
                    d.MainOrderID = MainOrderID;
                    list.Add(d);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(list);
            }
            else
            {
                ListID d = new ListID();
                d.MainOrderID = MainOrderID;
                ldep.Add(d);
                HttpContext.Current.Session["ListStaff"] = ldep;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(ldep);
            }
        }

        protected void btnUpdateStaff_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0 || obj_user.RoleID == 2)
                {
                    int status = hdfStaffID.Value.ToInt(0);
                    List<ListID> list = new List<ListID>();
                    if (HttpContext.Current.Session["ListStaff"] != null)
                    {
                        list = (List<ListID>)HttpContext.Current.Session["ListStaff"];
                    }
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            var mo = TransportationOrderController.GetByID(item.MainOrderID);
                            if (mo != null)
                            {
                                TransportationOrderController.UpdateStatus(item.MainOrderID, status, currentDate, obj_user.Username);
                                if (status == 0)
                                {
                                    //HistoryOrderChangeController.InsertTrans(item.MainOrderID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    //    " đã đổi trạng thái của đơn hàng ID là: " + item.MainOrderID + ", từ: " + "chờ duyệt" + ", sang: " + "hủy đơn" + "", 0, currentDate);

                                    var smallpacs = SmallPackageController.GetByTransportationOrderID(item.MainOrderID);
                                    if (smallpacs.Count > 0)
                                    {
                                        foreach (var items in smallpacs)
                                        {
                                            SmallPackageController.Delete(items.ID);
                                        }
                                    }
                                }
                                if (status == 2)
                                {
                                    //HistoryOrderChangeController.InsertTrans(item.MainOrderID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    //    " đã đổi trạng thái của đơn hàng ID là: " + item.MainOrderID + ", từ: " + "chờ duyệt" + ", sang: " + "đã duyệt" + "", 0, currentDate);.
                                    var smallpackages1 = SmallPackageController.GetByTransportationOrderID(item.MainOrderID);
                                    if (smallpackages1.Count == 0)
                                    {
                                        var transportationDetail = TransportationOrderDetailController.GetByTransportationOrderID(item.MainOrderID);
                                        if (transportationDetail.Count > 0)
                                        {
                                            foreach (var p in transportationDetail)
                                            {
                                                SmallPackageController.InsertWithTransportationIDNew(item.MainOrderID, 0, p.TransportationOrderCode, p.ProductType,
                                                0, Convert.ToDouble(p.Weight), 0, Convert.ToBoolean(p.IsCheckProduct), Convert.ToBoolean(p.IsPackaged), Convert.ToBoolean(p.IsInsurrance),
                                                p.CODTQCYN.ToString(), p.CODTQVND.ToString(), p.UserNote, "", p.ProductQuantity.ToString(), 1, currentDate, obj_user.Username, Convert.ToInt32(mo.UID), mo.Username);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        Session["ListStaff"] = null;
                        PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công.", "s", true, Page);
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Không có đơn hàng nào được chọn.", "e", true, Page);
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Bạn đủ quyền thực hiện chức năng này.", "e", true, Page);
                }
            }
        }

    }
}