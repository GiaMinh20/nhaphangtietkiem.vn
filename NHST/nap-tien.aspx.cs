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
    public partial class nap_tien : System.Web.UI.Page
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
                LoadBank();
            }
        }

        public void LoadBank()
        {
            List<Bank> lb = new List<Bank>();
            var bank = BankController.GetAllNotIsHidden();
            if (bank.Count > 0)
            {
                foreach (var item in bank)
                {
                    Bank nb = new Bank();
                    nb.ID = item.ID;
                    nb.BankInfo = item.BankName + " - " + item.AccountHolder + " - " + item.BankNumber + " - " + item.Branch;
                    lb.Add(nb);

                    ltrBank.Text += "<div class=\"col s12 m6 l4\">";
                    ltrBank.Text += "<div class=\"bank-wrap card-panel hoverable\">";
                    ltrBank.Text += "<p class=\"teal-text text-darken-4 font-weight-800 mt-0 display-flex space-between\"><span>" + item.BankName + "</span>";
                    ltrBank.Text += "<span class=\"icons\"><img src=\"" + item.IMG + "\" alt=\"icon\"></span>";
                    ltrBank.Text += "</p>";
                    ltrBank.Text += "<hr />";
                    ltrBank.Text += "<div class=\"flex-row\">";
                    ltrBank.Text += "<div class=\"black-text font-weight-700 lbl-fixed\">";
                    ltrBank.Text += "Chủ tài khoản</div>";
                    ltrBank.Text += "<div class=\"text-grow\">" + item.AccountHolder + "</div>";
                    ltrBank.Text += "</div>";
                    ltrBank.Text += "<div class=\"flex-row\">";
                    ltrBank.Text += "<div class=\"black-text font-weight-700 lbl-fixed\">";
                    ltrBank.Text += "Số tài khoản</div>";
                    ltrBank.Text += "<div class=\"text-grow\">" + item.BankNumber + "</div>";
                    ltrBank.Text += "</div>";
                    ltrBank.Text += "<div class=\"flex-row\">";
                    ltrBank.Text += "<div class=\"black-text font-weight-700 lbl-fixed\">";
                    ltrBank.Text += "Chi nhánh</div>";
                    ltrBank.Text += "<div class=\"text-grow\">" + item.Branch + "";
                    ltrBank.Text += "</div>";
                    ltrBank.Text += "</div>";
                    ltrBank.Text += "</div>";
                    ltrBank.Text += "</div>";
                }
            }

            if (lb.Count > 0)
            {
                ddlBank.DataSource = lb;
                ddlBank.DataBind();
            }
        }

        public void LoadData()
        {
            var page = PageController.GetByID(49);
            if (page != null)
            {
                //ltrInfo.Text = page.PageContent;
            }
            string username = Session["userLoginSystem"].ToString();
            string html = "";
            html += username;

            var user = AccountController.GetByUsername(username);
            if (user != null)
            {
                lblAccount.Text = string.Format("{0:N0}", user.Wallet) + "";
                var userinfo = AccountInfoController.GetByUserID(user.ID);
                //html += userinfo.Phone;
                var listhist = HistoryPayWalletController.GetByUIDASC(user.ID);
                double totalp = 0;
                foreach (var item in listhist)
                {
                    if (item.TradeType == 4)
                    {
                        totalp += Convert.ToDouble(item.Amount);
                    }
                }
                lblTotalIncome.Text = string.Format("{0:N0}", totalp) + "";

                var tradehistory = AdminSendUserWalletController.GetByUID(user.ID);
                if (tradehistory.Count > 0)
                {
                    pagingall(tradehistory);
                }
            }
        }

        #region Paging
        public void pagingall(List<tbl_AdminSendUserWallet> acs)
        {
            int PageSize = 15;
            if (acs.Count > 0)
            {
                int TotalItems = acs.Count;
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

                int UID = Convert.ToInt32(ViewState["UID"]);
                for (int i = FromRow; i < ToRow + 1; i++)
                {
                    var item = acs[i];
                    int status = Convert.ToInt32(item.Status);

                    ltr.Text += "<tr>";
                    ltr.Text += "<td>" + string.Format("{0:dd/MM/yyyy}", item.CreatedDate) + "</td>";
                    ltr.Text += "<td class=\"no-wrap\">" + string.Format("{0:N0}", item.Amount).Replace(",", ".") + " VNĐ</td>";
                    ltr.Text += "<td>" + item.TradeContent + "</td>";
                    ltr.Text += "<td class=\"no-wrap\">" + PJUtils.AddWalletStatus(item.Status.Value) + "</td>";
                    ltr.Text += "   <td>";
                    if (status == 1)
                        ltr.Text += "<a href=\"javascript:;\" onclick=\"deleteTrade('" + item.ID + "')\" data-position=\"top\"><i class=\"material-icons\">delete</i><span>Hủy yêu cầu</span></a>";
                    ltr.Text += "   </td>";
                    ltr.Text += "</tr>";
                }
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

        public class Bank
        {
            public int ID { get; set; }
            public string BankInfo { get; set; }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            DateTime currentDate = DateTime.Now;
            string username = Session["userLoginSystem"].ToString();
            var u = AccountController.GetByUsername(username);
            if (u != null)
            {
                string kq = AdminSendUserWalletController.Insert(u.ID, u.Username, Convert.ToDouble(txtAmount.Text), 1, Convert.ToInt32(ddlBank.SelectedValue), txtNote.Text, currentDate, username);
                if (kq.ToInt(0) > 0)
                {
                    var setNoti = SendNotiEmailController.GetByID(3);
                    if (setNoti != null)
                    {
                        if (setNoti.IsSentNotiAdmin == true)
                        {

                            var admins = AccountController.GetAllByRoleID(0);
                            if (admins.Count > 0)
                            {
                                foreach (var admin in admins)
                                {
                                    NotificationsController.Inser(admin.ID, admin.Username, kq.ToInt(), "Có yêu cầu nạp tiền mới.", 2, currentDate, u.Username, false);
                                    //string strPathAndQuery = Request.Url.PathAndQuery;
                                    //string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                    //string datalink = "" + strUrl + "manager/HistorySendWallet/";
                                    //PJUtils.PushNotiDesktop(admin.ID, " có yên cầu nạp tiền mới.", datalink);
                                }
                            }

                            var managers = AccountController.GetAllByRoleID(2);
                            if (managers.Count > 0)
                            {
                                foreach (var manager in managers)
                                {
                                    NotificationsController.Inser(manager.ID, manager.Username, kq.ToInt(), "Có yêu cầu nạp tiền mới.", 2, currentDate, u.Username, false);
                                    //string strPathAndQuery = Request.Url.PathAndQuery;
                                    //string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                    //string datalink = "" + strUrl + "manager/HistorySendWallet/";
                                    //PJUtils.PushNotiDesktop(manager.ID, " có yên cầu nạp tiền mới.", datalink);
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
                        //                    "Thông báo tại Nhập hàng Tiết Kiệm.", "Có yêu cầu nạp tiền mới.", "");
                        //            }
                        //            catch { }
                        //        }
                        //    }

                        //    //var managers = AccountController.GetAllByRoleID(2);
                        //    //if (managers.Count > 0)
                        //    //{
                        //    //    foreach (var manager in managers)
                        //    //    {
                        //    //        try
                        //    //        {
                        //    //            PJUtils.SendMailGmail("monamedia", "uvnajxzzurygsduf", manager.Email,
                        //    //                "Thông báo tại Nhập hàng Tiết Kiệm.", "Có yêu cầu nạp tiền mới.", "");
                        //    //        }
                        //    //        catch { }
                        //    //    }
                        //    //}

                        //}
                    }
                    PJUtils.ShowMessageBoxSwAlert("Gửi thông tin thành công, vui lòng chờ admin kiểm duyệt", "s", true, Page);
                }
            }
        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            var u = AccountController.GetByUsername(username);
            if (u != null)
            {
                int UID = u.ID;

                int ID = hdfTradeID.Value.ToInt(0);
                if (ID > 0)
                {
                    var t = AdminSendUserWalletController.GetByID(ID);
                    if (t != null)
                    {
                        AdminSendUserWalletController.UpdateStatus(ID, 3, t.TradeContent, DateTime.UtcNow.AddHours(7), username);
                        PJUtils.ShowMessageBoxSwAlert("Hủy yêu cầu thành công", "s", true, Page);
                    }
                }
            }
        }
    }
}