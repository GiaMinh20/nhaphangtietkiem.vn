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
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST.manager
{
    public partial class HistorySendWallet1 : System.Web.UI.Page
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
                    if (ac.RoleID == 0 || ac.RoleID == 2 || ac.RoleID == 7)
                    {
                        LoadData();
                        loadFilter();
                    }
                    else
                        Response.Redirect("/trang-chu");

                }
            }
        }
        public void loadFilter()
        {
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
        }
        public void LoadData()
        {
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];
            if (!string.IsNullOrEmpty(fd))
                rFD.Text = fd;
            if (!string.IsNullOrEmpty(td))
                rTD.Text = td;
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }

            string st = Request.QueryString["st"];
            if (!string.IsNullOrEmpty(st))
                select_by.SelectedIndex = st.ToInt(0);

            string ip = "-1";
            if (Request.QueryString["ip"] != null)
            {
                ip = Request.QueryString["ip"];
                ddlIsPayLoan.SelectedValue = ip;
            }
            else
            {
                ddlIsPayLoan.SelectedValue = ip;
            }

            int sale = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["sale"]))
            {
                sale = Convert.ToInt32(Request.QueryString["sale"]);
                ddlStaffSale.SelectedValue = sale.ToString();
            }

            int staff = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["staff"]))
            {
                staff = Convert.ToInt32(Request.QueryString["staff"]);
                ddlStaffOrder.SelectedValue = staff.ToString();
            }

            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }
            var la = AdminSendUserWalletController.GetBySQL_DK(search, st, page, fd, td, 20, ip, staff, sale);
            int total = AdminSendUserWalletController.GetTotalList(search, st, fd, td, ip, staff, sale);

            double TotalAmount = 0;
            double amount = AdminSendUserWalletController.GetTotalAmount(search, st, fd, td, staff, sale);
            if (amount > 0)
            {
                TotalAmount = amount;
            }
            lblTotalPrice.Text = string.Format("{0:N0}", TotalAmount) + " VNĐ";

            pagingall(la, total);
        }
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
            string ip = ddlIsPayLoan.SelectedValue;
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

            Response.Redirect("HistorySendWallet?s=" + searchname + "&fd=" + fd + "&td=" + td + "&st=" + st + "&ip=" + ip + "&staff=" + staff + "&sale=" + sale + "");
        }

        #region Webservice
        [WebMethod]
        public static string loadinfo(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var f = AdminSendUserWalletController.GetByID(ID.ToInt(0));
            if (f != null)
            {
                tbl_AdminSendUserWallet l = new tbl_AdminSendUserWallet();
                l.Username = f.Username;
                l.TradeContent = f.TradeContent;
                l.Amount = f.Amount;
                l.Status = f.Status;
                l.IsLoan = Convert.ToBoolean(f.IsLoan);
                l.IsPayLoan = Convert.ToBoolean(f.IsPayLoan);
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }
        [WebMethod]
        public static string GetData(int ID)
        {
            var nap = AdminSendUserWalletController.GetByID(ID);
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
                if (!string.IsNullOrEmpty(nap.TradeContent))
                    n.Note = nap.TradeContent;
                DateTime currentDate = DateTime.Now;
                string CreateDate = "Ngày " + currentDate.Day + " tháng " + currentDate.Month + " năm " + currentDate.Year;
                n.CreateDate = CreateDate;
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
        }
        #endregion

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];
            if (!string.IsNullOrEmpty(fd))
                rFD.Text = fd;
            if (!string.IsNullOrEmpty(td))
                rTD.Text = td;
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }
            string st = Request.QueryString["st"];
            if (!string.IsNullOrEmpty(st))
                select_by.SelectedIndex = st.ToInt(0);

            string ip = "-1";
            if (Request.QueryString["ip"] != null)
            {
                ip = Request.QueryString["ip"];
                ddlIsPayLoan.SelectedValue = ip;
            }
            else
            {
                ddlIsPayLoan.SelectedValue = ip;
            }
            int sale = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["sale"]))
            {
                sale = Convert.ToInt32(Request.QueryString["sale"]);
                ddlStaffSale.SelectedValue = sale.ToString();
            }

            int staff = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["staff"]))
            {
                staff = Convert.ToInt32(Request.QueryString["staff"]);
                ddlStaffOrder.SelectedValue = staff.ToString();
            }
            var la = AdminSendUserWalletController.GetBySQL_DK(search, st, 0, fd, td, 0, ip, staff, sale);
            StringBuilder StrExport = new StringBuilder();
            StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            StrExport.Append("<DIV  style='font-size:15px;'>");
            StrExport.Append("<table border=\"1\">");
            StrExport.Append("  <tr>");
            StrExport.Append("      <th><strong>ID</strong></th>");
            StrExport.Append("      <th><strong>Username</strong></th>");
            StrExport.Append("      <th><strong>Số tiền nạp</strong></th>");
            StrExport.Append("      <th><strong>Ngân hàng</strong></th>");
            StrExport.Append("      <th><strong>Trạng thái</strong></th>");
            StrExport.Append("      <th><strong>Ngày nạp</strong></th>");
            StrExport.Append("  </tr>");
            foreach (var item in la)
            {
                string Bank = PJUtils.ReturnBank(item.BankID);
                StrExport.Append("  <tr>");
                StrExport.Append("      <td>" + item.ID + "</td>");
                StrExport.Append("      <td>" + item.UserName + "</td>");
                StrExport.Append("      <td>" + item.AmountString + "</td>");
                StrExport.Append("      <td>" + Bank + "</td>");
                StrExport.Append("      <td>" + item.StatusName + "</td>");
                StrExport.Append("      <td>" + item.CreatedDateString + "</td>");
                StrExport.Append("  </tr>");
            }
            StrExport.Append("</table>");
            StrExport.Append("</div></body></html>");
            string strFile = "Lich-Su-Nap-Tien.xls";
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

        #region Pagging
        public void pagingall(List<AdminSendUserWalletController.ListShowNew> acs, int total)
        {
            //double ToTalMoney = 0;
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

                    string Bank = PJUtils.ReturnBank(item.BankID);
                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.UserName + "</td>");
                    hcm.Append("<td>" + item.AmountString + "</td>");
                    hcm.Append("<td>" + Bank + "</td>");
                    hcm.Append("<td>" + item.StatusName + "</td>");
                    hcm.Append("<td>" + item.CreatedDateString + "</td>");
                    hcm.Append("<td>" + item.AcceptBy + "</td>");
                    hcm.Append("<td>" + item.AcceptDate + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"action-table\">");
                    hcm.Append("<a href=\"javascript:;\" onclick=\"EditFunction(" + item.ID + ")\" class=\"edit-mode\" data-position=\"top\">");
                    hcm.Append("<i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                    hcm.Append("<a href=\"javascript:;\" onclick=\"printPhieuthu(" + item.ID + ")\" data-position=\"top\">");
                    hcm.Append("<i class=\"material-icons\">print</i><span>In phiếu thu</span></a>");
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

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username_current = Session["userLoginSystem"].ToString();
            int role = 0;
            var u_loginin = AccountController.GetByUsername(username_current);
            if (u_loginin != null)
            {
                role = u_loginin.RoleID.ToString().ToInt(0);
            }
            int id = hdfIDHSW.Value.ToInt(0);
            var h = AdminSendUserWalletController.GetByID(id);
            int UID = h.UID.Value;
            var user_wallet = AccountController.GetByID(UID);

            double money = h.Amount.Value; //Convert.ToDouble(pWallet.Text);          
            int status = ddlStatus.SelectedValue.ToString().ToInt(1);
            bool IsLoan = Convert.ToBoolean(hdfIsLoan.Value.ToInt(0));
            bool IsPayLoan = Convert.ToBoolean(hdfIsPayLoan.Value.ToInt(0));
            DateTime currentdate = DateTime.Now;
            string content = pContent.Text;
            var setNoti = SendNotiEmailController.GetByID(3);
            string BackLink = "/manager/HistorySendWallet.aspx";
            if (h != null)
            {
                if (h.Status != 1)
                {
                    AdminSendUserWalletController.UpdateCongNo(id, IsLoan, IsPayLoan);
                    PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công.", "s", true, Page);
                }
                else
                {
                    if (money > 0)
                    {
                        if (user_wallet != null)
                        {
                            double wallet = Convert.ToDouble(user_wallet.Wallet);
                            wallet = wallet + money;
                            if (role == 0 || role == 2 || role == 7)
                            {
                                if (status == 2)
                                {
                                    AdminSendUserWalletController.UpdateStatus(id, status, content, currentdate, username_current);
                                    AdminSendUserWalletController.UpdateCongNo(id, IsLoan, IsPayLoan);
                                    AccountController.updateWallet(user_wallet.ID, wallet, currentdate, username_current);

                                    if (string.IsNullOrEmpty(content))
                                        HistoryPayWalletController.Insert(user_wallet.ID, user_wallet.Username, 0, money, user_wallet.Username + " đã được nạp tiền vào tài khoản.", wallet, 2, 4, currentdate, username_current);
                                    else
                                        HistoryPayWalletController.Insert(user_wallet.ID, user_wallet.Username, 0, money, content, wallet, 2, 4, currentdate, username_current);

                                    NotificationController.Inser(u_loginin.ID, u_loginin.Username,
                                        Convert.ToInt32(user_wallet.ID), user_wallet.Username, 0,
                                        user_wallet.Username + " đã được nạp tiền vào tài khoản.", 0, 2,
                                        DateTime.UtcNow.AddHours(7), u_loginin.Username, true);

                                    AdminSendUserWalletController.UpdateAccept(id, username_current, currentdate);
                                }
                                else
                                {
                                    AdminSendUserWalletController.UpdateStatus(id, status, content, currentdate, username_current);
                                    AdminSendUserWalletController.UpdateCongNo(id, IsLoan, IsPayLoan);
                                }
                            }
                            PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật thành công.", "s", true, BackLink, Page);
                        }
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Vui lòng nhập số tiền lớn hơn 0.", "e", true, Page);
                    }
                }
            }
        }
    }
}