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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class lich_su_gioi_thieu : System.Web.UI.Page
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
        protected void btnSear_Click(object sender, EventArgs e)
        {
            string fd = "";
            string td = "";
            if (!string.IsNullOrEmpty(rFD.Text))
            {
                fd = rFD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rTD.Text))
            {
                td = rTD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(td))
            {
                Response.Redirect("/lich-su-gioi-thieu?fd=" + fd + "&td=" + td);
            }
            else
            {
                Response.Redirect("/lich-su-giao-dich?fd=" + fd + "&td=" + td);
            }
        }
        public void LoadData()
        {
            string username = Session["userLoginSystem"].ToString();
            var acc = AccountController.GetByUsername(username);
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];

            if (!string.IsNullOrEmpty(fd))
                rFD.Text = fd;
            if (!string.IsNullOrEmpty(td))
                rTD.Text = td;

            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }
            var confi = ConfigurationController.GetByTop1();
            double TotalPrice = 0;
            if (acc != null)
            {
                if (!string.IsNullOrEmpty(acc.XuTichLuy))
                {
                    TotalPrice = Convert.ToDouble(acc.XuTichLuy);
                }

                lblQuyDoi.Text = confi.XuCurrency;
                lblAccount.Text = TotalPrice.ToString();

                var la = HistoryIntroduceController.GetFromDateTodate(fd, td, 20, page, acc.ID);
                var total = HistoryIntroduceController.GetTotalFromDateTodate(fd, td, acc.ID);
                pagingall(la, total);
            }
        }

        #region Pagging
        public void pagingall(List<tbl_HistoryIntroduce> acs, int total)
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

                StringBuilder html = new StringBuilder();
                for (int i = 0; i < acs.Count; i++)
                {
                    var item = acs[i];

                    string Amount = "";
                    if (item.Type == 2)
                        Amount = " - " + string.Format("{0:N0}", item.Amount);
                    else
                        Amount = " + " + string.Format("{0:N0}", item.Amount);

                    html.Append("<tr>");
                    html.Append(" <td><span>" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</span></td>");
                    html.Append(" <td><span>" + item.TradeContent + "</span></td>");
                    html.Append("<td><span class=\"red-text\">" + Amount + "</span></td>");
                    html.Append("</tr>");
                }
                ltr.Text = html.ToString();
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
                pageUrl += "&Page={0}";
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            DateTime currentdate = DateTime.Now;
            string username = Session["userLoginSystem"].ToString();
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                double TotalPrice = 0;
                double TienNhan = 0;
                double TiGia = 0;

                double Wallet = Convert.ToDouble(acc.Wallet);
                var confi = ConfigurationController.GetByTop1();
                TiGia = Convert.ToDouble(confi.XuCurrency);

                if (!string.IsNullOrEmpty(acc.XuTichLuy))
                {
                    TotalPrice = Convert.ToDouble(acc.XuTichLuy);
                }    

                if (TotalPrice > 0)
                {
                    TienNhan = Math.Round(TotalPrice * TiGia, 0);
                    Wallet = Math.Round(Wallet + TienNhan, 0);

                    AccountController.updateWallet(acc.ID, Wallet, currentdate, username);
                    AccountController.UpdateXu(acc.ID, "0", currentdate, username);
                    HistoryPayWalletController.Insert(acc.ID, acc.Username, 0, TienNhan, acc.Username + " đã được quy đổi xu vào tài khoản.", Wallet, 2, 4, currentdate, username);
                    HistoryIntroduceController.Insert(acc.ID, acc.Username, TotalPrice.ToString(), acc.Username + " đã được quy đổi xu vào tài khoản.", "0", 2, currentdate, username);
                    PJUtils.ShowMessageBoxSwAlert("Quy đổi xu thành công.", "s", true, Page);

                    //if (TotalPrice > 99999)
                    //{
                    //    TienNhan = Math.Round(TotalPrice * TiGia, 0);
                    //    Wallet = Math.Round(Wallet + TienNhan, 0);

                    //    AccountController.updateWallet(acc.ID, Wallet, currentdate, username);
                    //    AccountController.UpdateXu(acc.ID, "0", currentdate, username);
                    //    HistoryPayWalletController.Insert(acc.ID, acc.Username, 0, TienNhan, acc.Username + " đã được quy đổi xu vào tài khoản.", Wallet, 2, 4, currentdate, username);
                    //    HistoryIntroduceController.Insert(acc.ID, acc.Username, TotalPrice.ToString(), acc.Username + " đã được quy đổi xu vào tài khoản.", "0", 2, currentdate, username);
                    //    PJUtils.ShowMessageBoxSwAlert("Quy đổi xu thành công.", "s", true, Page);
                    //} 
                    //else
                    //{
                    //    PJUtils.ShowMessageBoxSwAlert("Xu của bạn chưa đạt giới hạn được quy đổi.", "i", true, Page);
                    //}    
                }
                else
                    PJUtils.ShowMessageBoxSwAlert("Xu của bạn hiện tại không còn.", "i", true, Page);
            }    
        }
    }
}