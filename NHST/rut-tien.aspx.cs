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

namespace NHST
{
    public partial class rut_tien : System.Web.UI.Page
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
            string username = Session["userLoginSystem"].ToString();
            string html = "";
            html += username;

            var user = AccountController.GetByUsername(username);
            if (user != null)
            {
                int uid = user.ID;
                lblAccount.Text = string.Format("{0:N0}", user.Wallet) + " VNĐ";
                var userinfo = AccountInfoController.GetByUserID(user.ID);
                var ws = WithdrawController.GetBuyUID(uid);
                if (ws.Count > 0)
                {
                    pagingall(ws);
                }
            }
        }

        #region Paging
        public void pagingall(List<tbl_Withdraw> acs)
        {
            int PageSize = 10;
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
                    var w = acs[i];
                    int status = w.Status.ToString().ToInt();

                    ltr.Text += "<tr>";
                    ltr.Text += "<td>" + string.Format("{0:dd/MM/yyyy HH:mm}", w.CreatedDate) + "</td>";
                    ltr.Text += "<td>" + string.Format("{0:N0}", w.Amount) + " VNĐ</td>";
                    ltr.Text += "<td>" + w.Note + "</td>";
                    ltr.Text += "<td class=\"no-wrap\">" + PJUtils.ReturnStatusWithdrawNew(status) + "</td>";
                    if (status == 1)
                        ltr.Text += "   <td><a id=\"w_id_" + w.ID + "\" href=\"javascript:;\" onclick=\"cancelwithdraw(" + w.ID + ")\" data-position=\"top\"><i class=\"material-icons\">delete</i><span>Hủy yêu cầu</span></a></td>";
                    else
                        ltr.Text += "   <td></td>";
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

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username = Session["userLoginSystem"].ToString();
            var user = AccountController.GetByUsername(username);
            DateTime currendDate = DateTime.Now;
            if (user != null)
            {
                int uid = user.ID;
                double Amount = pAmount.Value.ToString().ToFloat(100000);
                double wallet = Convert.ToDouble(user.Wallet);
                if (wallet >= Amount)
                {
                    //Cho rút
                    double leftwallet = wallet - Amount;

                    //Cập nhật lại ví
                    AccountController.updateWallet(uid, leftwallet, currendDate, username);

                    //Thêm vào History Pay wallet
                    HistoryPayWalletController.Insert(uid, username, 0, Amount, "Rút tiền", leftwallet, 1, 5, currendDate, username);

                    //Thêm vào lệnh rút tiền                  
                    string kq = WithdrawController.InsertNote(uid, username, Amount, 1, txtContent.Text, currendDate, username, txtBankNumber.Text, txtBankAddress.Text, txtBeneficiary.Text);
                    if (kq.ToInt(0) > 0)
                    {
                        var setNoti = SendNotiEmailController.GetByID(4);
                        if (setNoti != null)
                        {
                            if (setNoti.IsSentNotiAdmin == true)
                            {
                                var admins = AccountController.GetAllByRoleID(0);
                                if (admins.Count > 0)
                                {
                                    foreach (var admin in admins)
                                    {
                                        NotificationsController.Inser(admin.ID, admin.Username, kq.ToInt(), "Có yêu cầu rút tiền mới.", 3, currendDate, username, false);
                                        //string strPathAndQuery = Request.Url.PathAndQuery;
                                        //string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        //string datalink = "" + strUrl + "manager/Withdraw-List/";
                                        //PJUtils.PushNotiDesktop(admin.ID, " có yêu cầu rút tiền mới.", datalink);
                                    }
                                }

                                var managers = AccountController.GetAllByRoleID(7);
                                if (managers.Count > 0)
                                {
                                    foreach (var manager in managers)
                                    {
                                        NotificationsController.Inser(manager.ID, manager.Username, kq.ToInt(), "Có yêu cầu rút tiền mới.", 3, currendDate, username, false);
                                        //string strPathAndQuery = Request.Url.PathAndQuery;
                                        //string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        //string datalink = "" + strUrl + "manager/Withdraw-List/";
                                        //PJUtils.PushNotiDesktop(manager.ID, " có yêu cầu cầu rút tiền mới.", datalink);
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
                            //                    "Thông báo tại Nhập hàng Tiết Kiệm.", "Có yêu cầu rút tiền mới.", "");
                            //            }
                            //            catch { }
                            //        }
                            //    }

                            //    var managers = AccountController.GetAllByRoleID(7);
                            //    if (managers.Count > 0)
                            //    {
                            //        foreach (var manager in managers)
                            //        {
                            //            try
                            //            {
                            //                PJUtils.SendMailGmail("monamedia", "uvnajxzzurygsduf", manager.Email,
                            //                    "Thông báo tại Nhập hàng Tiết Kiệm.", "Có yêu cầu rút tiền mới.", "");
                            //            }
                            //            catch { }
                            //        }
                            //    }
                            //}
                        }
                    }
                    PJUtils.ShowMessageBoxSwAlert("Tạo lệnh rút tiền thành công", "s", true, Page);
                }
                else
                {
                    lblError.Text = "Số tiền trong tài khoản không đủ để lập lệnh rút, vui lòng kiểm tra lại.";
                    lblError.Visible = true;
                }
            }
        }

        [WebMethod]
        public static string cancelwithdraw(int ID)
        {
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var u = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            if (u != null)
            {
                int uid = u.ID;
                double wallet = u.Wallet.ToString().ToFloat();
                var w = WithdrawController.GetByID(ID);
                if (w != null)
                {
                    double amount = w.Amount.ToString().ToFloat();
                    if (w.UID == uid)
                    {
                        double newwallet = wallet + amount;

                        //Cập nhật lại ví
                        AccountController.updateWallet(uid, newwallet, currentDate, username);

                        //Thêm vào History Pay wallet
                        HistoryPayWalletController.Insert(uid, username, 0, amount, "Hủy lệnh Rút tiền", newwallet, 2, 6, currentDate, username);

                        //Thêm vào lệnh rút tiền
                        WithdrawController.UpdateStatus(ID, 3, currentDate, username);

                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                    return "0";
            }
            else
            {
                return "0";
            }
        }
    }
}