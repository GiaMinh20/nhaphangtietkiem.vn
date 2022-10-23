using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace NHST.manager
{
    public partial class danh_sach_thanh_toan_ho1 : System.Web.UI.Page
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

                    LoadData();
                }
            }
        }

        private void LoadData()
        {
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }
            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");

            if (Page > 0)
            {
                Session["Page"] = Page;
                page = Page - 1;
            }
            else
            {
                Session["Page"] = "";
            }
            int status1 = Request.QueryString["st"].ToInt(-1);
            ddlStatus.SelectedValue = status1.ToString();

            string fd = Request.QueryString["fd"];
            if (!string.IsNullOrEmpty(fd))
                rFD.Text = fd;
            string td = Request.QueryString["td"];
            if (!string.IsNullOrEmpty(td))
                rTD.Text = td;

            int sort = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["sort"]))
            {
                sort = Convert.ToInt32(Request.QueryString["sort"]);
                ddlSortType.SelectedValue = sort.ToString();
            }

            int total = PayhelpController.GetTotalPage(search, status1, fd, td);
            var la = PayhelpController.GetByUserInSQLHelper_nottextnottypeWithstatus(page, 20, search, status1, fd, td, sort);
            pagingall(la, total);
        }

        private static string Check(bool check)
        {
            if (check)
                return "checked";
            else
                return null;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string fd = "";
            string td = "";
            string status1 = "";
            string searchname = search_name.Text.Trim();
            int SortType = Convert.ToInt32(ddlSortType.SelectedValue);
            if (!string.IsNullOrEmpty(rFD.Text))
            {
                fd = rFD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rTD.Text))
            {
                td = rTD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
            {
                status1 = ddlStatus.SelectedValue;
            }
            if (string.IsNullOrEmpty(searchname) == true && fd == "" && td == "" && status1 == "")
            {
                Response.Redirect("danh-sach-thanh-toan-ho?sort=" + SortType + "");
            }
            else
            {
                Response.Redirect("danh-sach-thanh-toan-ho?&s=" + searchname + "&fd=" + fd + "&sort=" + SortType + "&td=" + td + "&st=" + status1);
            }
        }

        #region Pagging
        public void pagingall(List<PayhelpController.Danhsachyeucau> acs, int total)
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
                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.Username + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", item.TotalPriceCYN) + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", item.TotalPriceVND) + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", item.Currency) + "</td>");
                    hcm.Append("<td>" + item.Percent + "</td>");

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
                            if (temp.ID == item.SalesID)
                                hcm.Append("  <option selected value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                            else
                                hcm.Append("  <option value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                        }
                    }
                    hcm.Append("</select>");
                    hcm.Append("</div>");
                    hcm.Append("</td>");
                    #endregion

                    //hcm.Append("<td>" + item.MaDonHang + "</td>");
                    hcm.Append("<td>" + item.statusstring + "</td>");
                    hcm.Append("<td>" + item.CreatedDate + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"action-table\">");
                    hcm.Append("<a href=\"chi-tiet-thanh-toan-ho.aspx?ID=" + item.ID + "\" target=\"_blank\" data-position=\"top\"> ");
                    hcm.Append("<i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                    if (item.Status == 2)
                    {
                        hcm.Append("<a href=\"javascript:;\" class=\"tooltipped\" onclick=\"AddPayment('" + item.ID + "')\" data-position=\"top\"");
                        hcm.Append(" data-tooltip=\"Thanh toán\"><i class=\"material-icons\">payment</i><span>Thanh toán</span></a>");
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
        protected void r_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (ac != null)
            {
                //var orders = PayhelpController.GetAll();
                //List<Danhsachyeucau> ds = new List<Danhsachyeucau>();
                //if (orders.Count > 0)
                //{
                //    foreach (var o in orders)
                //    {
                //        Danhsachyeucau d = new Danhsachyeucau();
                //        d.ID = o.ID;
                //        d.Username = o.Username;
                //        d.Phone = o.Phone;
                //        d.TotalPriceCYN = Convert.ToDouble(o.TotalPrice);
                //        d.TotalPriceCYN_String = string.Format("{0:N0}", Convert.ToDouble(o.TotalPrice)).Replace(",", ".");
                //        d.TotalPriceVND = Convert.ToDouble(o.TotalPriceVND);
                //        if (o.IsNotComplete != null)
                //            d.IsNotComplete = Convert.ToBoolean(o.IsNotComplete);
                //        else
                //            d.IsNotComplete = false;
                //        d.TotalPriceVND_String = string.Format("{0:N0}", Convert.ToDouble(o.TotalPriceVND)).Replace(",", ".");
                //        d.Currency = Convert.ToDouble(o.Currency);
                //        string stt = PJUtils.ReturnStatusPayHelp(Convert.ToInt32(o.Status));
                //        d.statusstring = stt;
                //        d.CreatedDate = string.Format("{0:dd/MM/yyyy HH:mm}", o.CreatedDate);
                //        ds.Add(d);
                //    }
                //}
                //gr.DataSource = ds;

                //int totalRow = PayhelpController.GetAll().Count();
                //int maximumRows = (ShouldApplySortFilterOrGroup()) ? totalRow : gr.PageSize;
                //gr.VirtualItemCount = totalRow;
                //int Page = (ShouldApplySortFilterOrGroup()) ? 0 : gr.CurrentPageIndex;
                //var Order = PayhelpController.GetByUserInSQLHelper_nottextnottypeWithstatus(Page, maximumRows);
                //gr.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
                //gr.DataSource = Order;


            }
        }


        #endregion
        #region button event
        protected void btnPayment_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0 || obj_user.RoleID == 7)
                {
                    int OID = hdfOrderID.Value.ToInt();
                    var Pay = PayhelpController.GetByID(OID);
                    if (Pay != null)
                    {
                        double TotalPriceVND = Convert.ToDouble(Pay.TotalPriceVND);
                        double WalletUser = 0;
                        var user = AccountController.GetByID(Convert.ToInt32(Pay.UID));
                        if (user != null)
                        {
                            WalletUser = Convert.ToDouble(user.Wallet);
                            if (Pay.Status < 3)
                            {
                                if (WalletUser >= TotalPriceVND)
                                {
                                    double WalletLeft = WalletUser - TotalPriceVND;
                                    AccountController.updateWallet(user.ID, WalletLeft, currentDate, username_current);
                                    HistoryPayWalletController.Insert(user.ID, user.Username, 0, TotalPriceVND,
                                    user.Username + " đã trả tiền thanh toán tiền hộ đơn hàng " + OID, WalletLeft, 1, 9, currentDate, obj_user.Username);
                                    PayhelpController.UpdateStatus(OID, 3, currentDate, obj_user.Username);

                                    PJUtils.ShowMessageBoxSwAlert("Thanh toán thành công.", "s", true, Page);
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlert("Tài khoản của khách hàng không đủ tiền để thanh toán đơn này. Vui lòng nạp thêm tiền vào ví của khách hàng.", "i", true, Page);
                                }
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Đơn hàng này đã thanh toán rồi. Vui lòng kiểm tra lại.", "i", true, Page);
                            }
                        }
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Bạn không đủ quyền thực hiện chức năng này.", "i", true, Page);
                }
            }
        }
        #endregion

        [WebMethod]
        public static string UpdateStaff(int OrderID, int StaffID, int Type)
        {
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0 || obj_user.RoleID == 2)
                {
                    var mo = PayhelpController.GetByID(OrderID);
                    if (mo != null)
                    {
                        if (Type == 1) //1:saler - 2:dathang
                        {
                            #region Saler
                            string saleName = "";
                            if (StaffID > 0)
                            {                               
                                var sales = AccountController.GetByID(StaffID);
                                if (sales != null)
                                {
                                    saleName = sales.Username;
                                }                                   
                            }
                            PayhelpController.UpdateSales(OrderID, StaffID, saleName);
                            #endregion                           
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
        
    }
}