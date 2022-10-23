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
using System.Data;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Services;
using MB.Extensions;
using System.Text.RegularExpressions;

namespace NHST.manager
{
    public partial class Withdraw_List : System.Web.UI.Page
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
                    if (ac.RoleID != 0 && ac.RoleID != 7 && ac.RoleID != 2)
                        Response.Redirect("/trang-chu");
                    LoadData();
                }
            }
        }
        public void LoadData()
        {
            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];
            if (!string.IsNullOrEmpty(fd))
                rFD.Text = fd;
            if (!string.IsNullOrEmpty(td))
                rTD.Text = td;
            string st = Request.QueryString["st"];
            if (!string.IsNullOrEmpty(st))
                select_by.SelectedIndex = st.ToInt(0);

            var la = WithdrawController.GetBySQL_PQD(search, st, page, 10, fd, td);
            int total = WithdrawController.GetTotalSQLPQD(search_name.Text.Trim(), st, fd, td);
            pagingall(la, total);
        }


        #region Webservice
        [WebMethod]
        public static string GetData(int ID)
        {
            var nap = WithdrawController.GetByID(ID);
            if (nap != null)
            {
                NaptienInfo n = new NaptienInfo();
                int UID = Convert.ToInt32(nap.UID);
                double Amount = Convert.ToDouble(nap.Amount);
                var ai = AccountInfoController.GetByUserID(UID);
                if (ai != null)
                {
                    n.FullName = ai.FirstName + " " + ai.LastName;
                    n.Address = ai.Address;
                }
                n.Money = string.Format("{0:N0}", Amount);
                if (!string.IsNullOrEmpty(nap.Note))
                    n.Note = nap.Note;
                DateTime currentDate = DateTime.Now;
                string CreateDate = "Ngày " + currentDate.Day + " tháng " + currentDate.Month + " năm " + currentDate.Year;
                n.CreateDate = CreateDate;
                n.Status = nap.Status.Value;
                n.Amount = nap.Amount.Value;
                n.Username = nap.Username;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(n);
            }
            return "null";
        }
        public class NaptienInfo
        {
            public string FullName { get; set; }
            public string Address { get; set; }
            public string Money { get; set; }
            public string Note { get; set; }
            public string CreateDate { get; set; }
            public int Status { get; set; }
            public string Username { get; set; }
            public double Amount { get; set; }
        }
        #endregion

        #region Pagging
        public void pagingall(List<WithdrawController.ListWithdrawNew> acs, int total)
        {
            int PageSize = 10;
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
                    int status = Convert.ToInt32(item.Status);
                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.Username + "</td>");
                    hcm.Append("<td style=\"font-weight:bold\">");
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Người nhận: " + item.Beneficiary + "</span></p>");
                    hcm.Append("<p class=\"s-txt no-wrap blue-text\"><span class=\"total\">Số tài khoản nhận: " + item.BankNumber + "</span></p>");
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Chi nhánh: " + item.BankAddress + "</span></p>");                    
                    hcm.Append("</td>");
                    hcm.Append("<td>" + item.Content + "</td>");
                    hcm.Append("<td>" + item.AmountString + "</td>");
                    hcm.Append("<td>" + item.StatusName + "</td>");
                    hcm.Append("<td>" + item.CreatedDateString + "</td>");
                    hcm.Append("<td>" + item.AcceptBy + "</td>");
                    hcm.Append("<td>" + item.AcceptDateString + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"action-table\">");
                    hcm.Append("<a href=\"javascript:;\" onclick=\"EditFunction(" + item.ID + ")\" class=\"edit-mode\" data-position=\"top\">");
                    hcm.Append("<i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                    hcm.Append("<a href=\"javascript:;\" onclick=\"printPhieuchi(" + item.ID + ")\" data-position=\"top\">");
                    hcm.Append("<i class=\"material-icons\">print</i><span>In phiếu chi</span></a>");
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchname = search_name.Text.Trim();
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
            string st = select_by.SelectedValue;            

            if (string.IsNullOrEmpty(searchname) == true && fd == "" && td == "" && st == "0")
            {
                Response.Redirect("Withdraw-List");
            }
            else
            {
                Response.Redirect("Withdraw-List?s=" + searchname + " &fd=" + fd + "&td=" + td + "&st=" + st + "");
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];
            if (!string.IsNullOrEmpty(fd))
                rFD.Text = fd;
            if (!string.IsNullOrEmpty(td))
                rTD.Text = td;
            string st = Request.QueryString["st"];
            if (!string.IsNullOrEmpty(st))
                select_by.SelectedIndex = st.ToInt(0);

            var la = WithdrawController.GetBySQL_PQD(search, st, 0, 0, fd, td);
            StringBuilder StrExport = new StringBuilder();
            StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            StrExport.Append("<DIV  style='font-size:15px;'>");
            StrExport.Append("<table border=\"1\">");
            StrExport.Append("  <tr>");
            StrExport.Append("      <th><strong>ID</strong></th>");
            StrExport.Append("      <th><strong>Username</strong></th>");
            StrExport.Append("      <th><strong>Số tiền rút</strong></th>");           
            StrExport.Append("      <th><strong>Trạng thái</strong></th>");
            StrExport.Append("      <th><strong>Ngày nạp</strong></th>");
            StrExport.Append("  </tr>");
            foreach (var item in la)
            {               
                StrExport.Append("  <tr>");
                StrExport.Append("      <td>" + item.ID + "</td>");
                StrExport.Append("      <td>" + item.Username + "</td>");
                StrExport.Append("      <td>" + item.AmountString + "</td>");               
                StrExport.Append("      <td>" + item.StatusName + "</td>");
                StrExport.Append("      <td>" + item.CreatedDateString + "</td>");
                StrExport.Append("  </tr>");
            }
            StrExport.Append("</table>");
            StrExport.Append("</div></body></html>");
            string strFile = "Lich-Su-Rut-Tien.xls";
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
        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username = Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            int id = hdfIDWR.Value.ToInt(0);
            var setNoti = SendNotiEmailController.GetByID(4);
            var w = WithdrawController.GetByID(id);
            string BackLink = "/manager/Withdraw-List.aspx";
            if (w != null)
            {
                if (w.Status < 3)
                {
                    int status = ddlStatus.SelectedValue.ToString().ToInt(1);
                    if (status == 2)
                    {
                        var acc = AccountController.GetByUsername(username);
                        if (acc.RoleID == 7)
                        {
                            int uid_rut = w.UID.ToString().ToInt();
                            var user_rut = AccountController.GetByID(uid_rut);
                            if (user_rut != null)
                            {
                                WithdrawController.UpdateStatus(id, status, currentDate, username);
                                WithdrawController.UpdateAccept(id, currentDate, username);

                                if (setNoti != null)
                                {
                                    if (setNoti.IsSentNotiUser == true)
                                    {
                                        NotificationsController.Inser(user_rut.ID, user_rut.Username, 0, "Admin đã duyệt lệnh rút tiền của bạn.", 3, currentDate, username, true);
                                        //string strPathAndQuery = Request.Url.PathAndQuery;
                                        //string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        //string datalink = "" + strUrl + "rut-tien/";
                                        //PJUtils.PushNotiDesktop(user_rut.ID, "BQT KÝ GỬI NHANH đã duyệt lệnh rút tiền của bạn.", datalink);
                                    }
                                    //if (setNoti.IsSendEmailUser == true)
                                    //{
                                    //    try
                                    //    {
                                    //        PJUtils.SendMailGmail("monamedia", "uvnajxzzurygsduf", user_rut.Email,
                                    //            "Thông báo tại Nhập hàng Tiết Kiệm.", "BQT KÝ GỬI NHANH đã duyệt lệnh rút tiền của bạn.", "");
                                    //    }
                                    //    catch { }
                                    //}
                                }

                                PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật thành công", "s", true, BackLink, Page);
                            }
                        }
                        else if (acc.RoleID == 0)
                        {
                            int uid_rut = w.UID.ToString().ToInt();
                            var user_rut = AccountController.GetByID(uid_rut);
                            if (user_rut != null)
                            {
                                WithdrawController.UpdateStatus(id, status, currentDate, username);
                                WithdrawController.UpdateAccept(id, currentDate, username);

                                if (setNoti != null)
                                {
                                    if (setNoti.IsSentNotiUser == true)
                                    {
                                        NotificationsController.Inser(user_rut.ID, user_rut.Username, 0, "BQT THUẬN ĐẠT EXPRESS đã duyệt lệnh rút tiền của bạn.", 3, currentDate, username, true);                                       
                                    }
                                    //if (setNoti.IsSendEmailUser == true)
                                    //{
                                    //    try
                                    //    {
                                    //        PJUtils.SendMailGmail("monamedia", "uvnajxzzurygsduf", user_rut.Email,
                                    //            "Thông báo tại Nhập hàng Tiết Kiệm.", "BQT KÝ GỬI NHANH đã duyệt lệnh rút tiền của bạn.", "");
                                    //    }
                                    //    catch { }
                                    //}
                                }

                                PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật thành công", "s", true, BackLink, Page);
                            }
                        }
                    }
                    else if (status == 3)
                    {
                        int uid_rut = w.UID.ToString().ToInt();
                        var user_rut = AccountController.GetByID(uid_rut);
                        if (user_rut != null)
                        {
                            double wallet = Convert.ToDouble(user_rut.Wallet.ToString());
                            double amount = Convert.ToDouble(w.Amount.ToString());

                            double newwallet = wallet + amount;

                            //Cập nhật lại ví
                            AccountController.updateWallet(uid_rut, newwallet, currentDate, username);

                            //Thêm vào History Pay wallet
                            HistoryPayWalletController.Insert(uid_rut, username, 0, amount, "BQT THUẬN ĐẠT EXPRESS đã hủy lệnh Rút tiền", newwallet, 2, 6, currentDate, username);

                            //Thêm vào lệnh rút tiền
                            //WithdrawController.UpdateStatus(id, 3, currentDate, username);
                            //WithdrawController.UpdateAccept(id, currentDate, username);

                            if (setNoti != null)
                            {
                                if (setNoti.IsSentNotiUser == true)
                                {
                                    NotificationsController.Inser(user_rut.ID, user_rut.Username, 0, "BQT THUẬN ĐẠT EXPRESS đã hủy lệnh rút tiền của bạn.", 3, currentDate, username, true);                                    
                                }
                                //if (setNoti.IsSendEmailUser == true)
                                //{
                                //    try
                                //    {
                                //        PJUtils.SendMailGmail("monamedia", "uvnajxzzurygsduf", user_rut.Email, "Thông báo tại Nhập hàng Tiết Kiệm.", "BQT THUẬN ĐẠT EXPRESS đã hủy lệnh rút tiền của bạn.", "");
                                //    }
                                //    catch { }
                                //}
                            }
                        }
                        WithdrawController.UpdateStatus(id, status, currentDate, username);
                        WithdrawController.UpdateAccept(id, currentDate, username);

                        PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật thành công", "s", true, BackLink, Page);
                    }
                    else
                    {
                        WithdrawController.UpdateStatus(id, status, currentDate, username);
                        PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật thành công", "s", true, BackLink, Page);
                    }
                }
            }
        }
    }
}