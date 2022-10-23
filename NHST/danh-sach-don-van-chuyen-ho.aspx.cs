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
    public partial class danh_sach_don_van_chuyen_ho1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session["userLoginSystem"] = "vu221092";
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
                string oc = "";
                if (Request.QueryString["oc"] != null)
                {
                    oc = Request.QueryString["oc"];
                }
                int stt = Request.QueryString["stt"].ToInt(-1);
                string fd = Request.QueryString["fd"];
                string td = Request.QueryString["td"];

                txtOrderCode.Text = oc;

                if (Request.QueryString["stt"] != null)
                    ddlStatus.SelectedValue = stt.ToString();
                if (!string.IsNullOrEmpty(Request.QueryString["fd"]))
                {
                    FD.Text = fd; ;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["td"]))
                    TD.Text = td;
                List<tbl_TransportationOrder> ts = new List<tbl_TransportationOrder>();

                if (!string.IsNullOrEmpty(oc))
                {
                    ts = TransportationOrderController.GetByUIDAndPackageCode(UID, oc);
                }
                else
                {
                    ts = TransportationOrderController.GetByUID(UID);
                }

                if (stt > -1)
                {
                    if (!string.IsNullOrEmpty(fd))
                    {
                        if (!string.IsNullOrEmpty(td))
                        {
                            var fromdate = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                            var todate = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                            ts = ts.Where(o => o.Status == stt && o.CreatedDate >= fromdate && o.CreatedDate <= todate).ToList();
                        }
                        else
                        {
                            var fromdate = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                            ts = ts.Where(o => o.Status == stt && o.CreatedDate >= fromdate).ToList();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(td))
                        {
                            var todate = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                            ts = ts.Where(o => o.Status == stt && o.CreatedDate <= todate).ToList();
                        }
                        else
                        {
                            ts = ts.Where(o => o.Status == stt).ToList();
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(fd))
                    {
                        if (!string.IsNullOrEmpty(td))
                        {
                            var fromdate = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                            var todate = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                            ts = ts.Where(o => o.CreatedDate >= fromdate && o.CreatedDate <= todate).ToList();
                        }
                        else
                        {
                            var fromdate = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                            ts = ts.Where(o => o.CreatedDate >= fromdate).ToList();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(td))
                        {
                            var todate = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                            ts = ts.Where(o => o.CreatedDate <= todate).ToList();
                        }
                    }
                }

                if (ts.Count > 0)
                {
                    List<Danhsachorder> os = new List<Danhsachorder>();
                    foreach (var t in ts)
                    {
                        double totalPackages = 0;
                        double totalWeight = 0;
                        int status = Convert.ToInt32(t.Status);
                        totalWeight = Convert.ToDouble(t.TotalWeight);
                        Danhsachorder d = new Danhsachorder();
                        d.ID = t.ID;
                        d.UID = UID;
                        d.Username = username_current;
                        d.TotalPackage = totalPackages.ToString();
                        d.TotalWeight = totalWeight.ToString();
                        d.IntStatus = status;
                        d.Status = PJUtils.generateTransportationStatusNew(status);
                        d.CreatedDate = string.Format("{0:dd/MM/yyyy}", t.CreatedDate);
                        d.TotalPriceVND = string.Format("{0:N0}", t.TotalPrice);
                        os.Add(d);
                    }
                    pagingall(os);
                }
            }
        }

        #region Paging
        public void pagingall(List<Danhsachorder> acs)
        {
            int PageSize = 15;
            if (acs.Count > 0)
            {
                string oc = "";
                if (Request.QueryString["oc"] != null)
                {
                    oc = Request.QueryString["oc"];
                }
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
                StringBuilder html = new StringBuilder();
                for (int i = FromRow; i < ToRow + 1; i++)
                {
                    var item = acs[i];

                    html.Append("<tr>");
                    html.Append("<td>" + item.ID + "</td>");
                    html.Append("<td>" + item.TotalPriceVND + " VNĐ</td>");
                    html.Append("<td>" + item.TotalWeight + " kg</td>");
                    html.Append("<td class=\"no-wrap\">" + string.Format("{0:dd/MM/yyyy}", item.CreatedDate) + "</td>");
                    html.Append("<td class=\"no-wrap\">" + item.Status + "</td>");

                    var smallpackages = SmallPackageController.GetByTransportationOrderID(item.ID);
                    if (smallpackages.Count > 0)
                    {
                        html.Append("<td class=\"no-wrap\">");
                        foreach (var s in smallpackages)
                        {
                            if (!string.IsNullOrEmpty(oc))
                            {
                                if (oc == s.OrderTransactionCode)
                                {
                                    html.Append("<p style=\"background:wheat;\"><span class=\"bold\">" + s.OrderTransactionCode + "</span> - <span class=\"blue-text\">" + PJUtils.IntToStringStatusSmallPackageTextNew(Convert.ToInt32(s.Status)) + "</span</p>");
                                }
                                else
                                {
                                    html.Append("<p><span class=\"bold\">" + s.OrderTransactionCode + "</span> - <span class=\"blue-text\">" + PJUtils.IntToStringStatusSmallPackageTextNew(Convert.ToInt32(s.Status)) + "</span</p>");
                                }
                            }
                            else
                            {
                                html.Append("<p><span class=\"bold\">" + s.OrderTransactionCode + "</span> - <span class=\"blue-text\">" + PJUtils.IntToStringStatusSmallPackageTextNew(Convert.ToInt32(s.Status)) + "</span</p>");
                            }
                        }
                        html.Append("</td>");
                    }
                    else
                    {
                        html.Append("<td>");
                        html.Append("<span class=\"red-text\">Chờ duyệt</span></td>");
                        html.Append("</td>");
                    }

                    html.Append("<td class=\"tb-date\">");
                    html.Append("<div class=\"action-table\">");
                    html.Append("<a href=\"/chi-tiet-don-hang-van-chuyen-ho/" + item.ID + "\" data-position=\"top\"><i class=\"material-icons\">remove_red_eye</i><span>Chi tiết</span></a>");
                    if (item.IntStatus == 1)
                    {
                        html.Append("    <a href=\"javascript:;\" onclick=\"CancelOrder('" + item.ID + "')\" data-position=\"top\"><i class=\"material-icons\">delete</i><span>Hủy đơn</span></a>");
                    }
                    if (item.IntStatus == 5)
                    {
                        html.Append("    <a href=\"javascript:;\" onclick=\"PaymentOrder('" + item.ID + "')\" data-position=\"top\"><i class=\"material-icons\">attach_money</i><span>Thanh toán</span></a>");
                    }
                    html.Append("</div>");
                    html.Append("</td>");
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                int UID = obj_user.ID;
                int OID = hdfOrderID.Value.ToInt();
                if (OID > 0)
                {
                    var t = TransportationOrderController.GetByIDAndUID(OID, UID);
                    if (t != null)
                    {
                        TransportationOrderController.UpdateStatus(t.ID, 0, DateTime.UtcNow.AddHours(7), username_current);
                        PJUtils.ShowMessageBoxSwAlert("Hủy đơn hàng thành công", "s", true, Page);
                    }
                }
            }
        }

        protected void btnPayment_Click(object sender, EventArgs e)
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
                        {
                            Deposited = Convert.ToDouble(t.Deposited);
                        }
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
                                                    NotificationsController.Inser(admin.ID,
                                                                                       admin.Username, t.ID,
                                                                                        "Đơn hàng vận chuyển hộ " + t.ID + " đã được thanh toán.",
                                                                                       10, currentDate, username_current, false);
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
                                        //                    "Thông báo tại Nhập hàng Tiết Kiệm.", "Đơn hàng vận chuyển hộ " + t.ID + " đã được thanh toán.", "");
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

        public class Danhsachorder
        {
            public int ID { get; set; }
            public int UID { get; set; }
            public string TotalPackage { get; set; }
            public string TotalWeight { get; set; }
            public string TotalPriceVND { get; set; }
            public string Status { get; set; }
            public int IntStatus { get; set; }
            public string CreatedDate { get; set; }
            public string Username { get; set; }
        }

        protected void btnSear_Click(object sender, EventArgs e)
        {
            string ordercode = txtOrderCode.Text;
            string status = ddlStatus.SelectedValue;
            string fd = "";
            string td = "";

            if (!string.IsNullOrEmpty(FD.Text))
            {
                fd = FD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(TD.Text))
            {
                td = TD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(td))
            {
                Response.Redirect("/danh-sach-don-van-chuyen-ho?oc=" + ordercode + "&stt=" + status + "&fd=" + fd + "&td=" + td + "");
            }
            else
            {
                Response.Redirect("/danh-sach-don-van-chuyen-ho?oc=" + ordercode + "&stt=" + status + "&fd=" + fd + "&td=" + td + "");
            }
            //LoadData();
        }

    }
}