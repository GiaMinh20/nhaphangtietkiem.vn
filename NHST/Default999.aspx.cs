using NHST.Bussiness;
using NHST.Controllers;
using Supremes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class Default1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Redirect("/dang-nhap");
                //LoadData();
            }
        }

        public void LoadData()
        {
            //var confi = ConfigurationController.GetByTop1();
            //if (confi != null)
            //{
            //    ltrEmail.Text = "<div class=\"desc\"><p>" + confi.EmailContact + "</p></div>";
            //    ltrTimeWork.Text = "<div class=\"desc\"><p>" + confi.TimeWork + "</p></div>";
            //    ltrHotline.Text = "<div class=\"desc\"><p>" + confi.Hotline + "</p></div>";
            //}
            var news = NewsController.GetAllNotHiddenType(1);
            if (news.Count > 0)
            {
                foreach (var item in news)
                {
                    ltrNews.Text += "<div class=\"col-4\">";
                    ltrNews.Text += "<div class=\"box-news\">";
                    ltrNews.Text += "<div class=\"img-news\"><a href=\"" + item.NewsLink + "\"><img src=\"" + item.NewsIMG + "\" alt=\"\"></a></div>";
                    ltrNews.Text += "<div class=\"text-news\">";
                    ltrNews.Text += "<div class=\"title\">";
                    ltrNews.Text += "<a href=\"" + item.NewsLink + "\"><h4>" + item.NewsTitle + "</a></h4>";
                    ltrNews.Text += "</div>";
                    ltrNews.Text += "<div class=\"content\">";
                    ltrNews.Text += "<p>" + item.NewsDesc + "</p>";
                    ltrNews.Text += "</div>";
                    ltrNews.Text += "</div>";
                    ltrNews.Text += "</div>";
                    ltrNews.Text += "</div>";
                }
            }
            //var news2 = NewsController.GetAllNotHiddenType(2);
            //if (news2.Count > 0)
            //{
            //    foreach (var item in news2)
            //    {
            //        ltrNews2.Text += "<div class=\"col-4\">";
            //        ltrNews2.Text += "<div class=\"box-card\">";
            //        ltrNews2.Text += "<div class=\"content-card\">";
            //        ltrNews2.Text += "<div class=\"img-card\"><img src=\"" + item.NewsIMG + "\" alt=\"\"></div>";
            //        ltrNews2.Text += "<div class=\"text-card\">";
            //        ltrNews2.Text += "<div class=\"title-card\">";
            //        ltrNews2.Text += "<h3>" + item.NewsTitle + "</h3>";
            //        ltrNews2.Text += "</div>";
            //        ltrNews2.Text += "<div class=\"desc-card\">";
            //        ltrNews2.Text += "<p>" + item.NewsDesc + "</p>";
            //        ltrNews2.Text += "</div>";
            //        ltrNews2.Text += "</div>";
            //        ltrNews2.Text += "</div>";
            //        ltrNews2.Text += "</div>";
            //        ltrNews2.Text += "</div>";
            //    }
            //}
            var steps = StepController.GetAll("");
            if (steps.Count > 0)
            {
                foreach (var item in steps)
                {
                    ltrStep.Text += "<div class=\"col-6\">";
                    ltrStep.Text += "<div class=\"box-timeline\">";
                    ltrStep.Text += "<p>" + item.StepName + "</p>";
                    ltrStep.Text += "</div>";
                    ltrStep.Text += "</div>";
                }
            }
            var services = ServiceController.GetAll();
            if (services.Count > 0)
            {
                foreach (var item in services)
                {
                    ltrServices.Text += "<div class=\"col-4\">";
                    ltrServices.Text += "<div class=\"box-sv\">";
                    ltrServices.Text += "<div class=\"img-sv\"><img src=\"" + item.ServiceIMG + "\" alt=\"\"></div>";
                    ltrServices.Text += "<div class=\"text-sv\">";
                    ltrServices.Text += "<p>" + item.ServiceName + "</p>";
                    ltrServices.Text += "<span>" + item.ServiceContent + "</span>";
                    ltrServices.Text += "</div>";
                    ltrServices.Text += "</div>";
                    ltrServices.Text += "</div>";
                }
            }
            var ck = CustomerBenefitsController.GetAllByItemType(2);
            if (ck.Count > 0)
            {
                foreach (var item in ck)
                {
                    ltrCamKet.Text += "<div class=\"col-3\">";
                    ltrCamKet.Text += "<a class=\"box-commit\">";
                    ltrCamKet.Text += "<div class=\"img-commit\"><img src=\"" + item.Icon + "\" alt=\"\"></div>";
                    ltrCamKet.Text += "<div class=\"title-commit\"><p>" + item.CustomerBenefitName + "</p></div>";
                    ltrCamKet.Text += "</a>";
                    ltrCamKet.Text += "</div>";
                }
            }
            var cus = ContactController.GetAllNotHidden();
            if (cus.Count > 0)
            {
                foreach (var item in cus)
                {
                    ltrCustomers.Text += "<div class=\"swiper-slide\">";
                    ltrCustomers.Text += "<div class=\"box-review\">";
                    ltrCustomers.Text += "<div class=\"top\">";
                    ltrCustomers.Text += "<div class=\"left\">";
                    ltrCustomers.Text += "<div class=\"img-review\"><img src=\"" + item.Images + "\" alt=\"\"></div>";
                    ltrCustomers.Text += "</div>";
                    ltrCustomers.Text += "<div class=\"right\">";
                    ltrCustomers.Text += "<div class=\"title\"><h4>" + item.Fullname + "</h4></div>";
                    ltrCustomers.Text += "<div class=\"review-star\">";
                    ltrCustomers.Text += "  <span><i class=\"fa fa-star\" aria-hidden=\"true\"></i></span>";
                    ltrCustomers.Text += "  <span> <i class=\"fa fa-star\" aria-hidden=\"true\"></i></span>";
                    ltrCustomers.Text += "  <span> <i class=\"fa fa-star\" aria-hidden=\"true\"></i></span>";
                    ltrCustomers.Text += "  <span> <i class=\"fa fa-star\" aria-hidden=\"true\"></i></span>";
                    ltrCustomers.Text += "  <span> <i class=\"fa fa-star\" aria-hidden=\"true\"></i></span>";
                    ltrCustomers.Text += "</div>";
                    ltrCustomers.Text += "</div>";
                    ltrCustomers.Text += "</div>";
                    ltrCustomers.Text += "<div class=\"bot\">";
                    ltrCustomers.Text += "<div class=\"text-review\"><p>" + item.ContactContent + "</p></div>";
                    ltrCustomers.Text += "</div>";
                    ltrCustomers.Text += "</div>";
                    ltrCustomers.Text += "</div>";
                }
            }
            //var quest = QuestionController.GetAllNotHiden();
            //if (quest.Count > 0)
            //{
            //    foreach (var item in quest)
            //    {
            //        ltrQuestions.Text += "<div class=\"box-i5\">";
            //        ltrQuestions.Text += "<div class=\"question\">";
            //        ltrQuestions.Text += "<p>" + item.QuesTitle + "</p>";
            //        ltrQuestions.Text += "</div>";
            //        ltrQuestions.Text += "<div class=\"answer\">";
            //        ltrQuestions.Text += "<p>" + item.QuesDesc + "</p>";
            //        ltrQuestions.Text += "</div>";
            //        ltrQuestions.Text += "</div>";
            //    }
            //}
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string text = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    string a = PJUtils.TranslateTextNew(text, "vi", "zh");
                    a = a.Replace("[", "").Replace("]", "").Replace("\"", "");
                    string[] ass = a.Split(',');
                    string page = hdfWebsearch.Value;
                    SearchPage(page, PJUtils.RemoveHTMLTags(ass[0]));
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA1.Create();
            byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(inputString);
            return bytes;
        }
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append("%" + b.ToString("X2"));

            return sb.ToString();
        }
        public void SearchPage(string page, string text)
        {
            string linkgo = "";
            if (page == "tmall")
            {
                string a = text;
                string textsearch_tmall = GetHashString(a);
                //string fullLinkSearch_tmall = "https://list.tmall.com/search_product.htm?q=" + textsearch_tmall + "&type=p&vmarket=&spm=875.7931836%2FB.a2227oh.d100&from=mallfp..pc_1_searchbutton";
                linkgo = "https://list.tmall.com/search_product.htm?q=" + textsearch_tmall + "&type=p&vmarket=&spm=875.7931836%2FB.a2227oh.d100&from=mallfp..pc_1_searchbutton";
            }
            else if (page == "taobao")
            {
                string a = text;
                string textsearch_taobao = GetHashString(a);
                //string fullLinkSearch_taobao = "https://world.taobao.com/search/search.htm?q=" + textsearch_taobao + "&navigator=all&_input_charset=&spm=a21bp.7806943.20151106.1";
                linkgo = "https://world.taobao.com/search/search.htm?q=" + textsearch_taobao + "&navigator=all&_input_charset=&spm=a21bp.7806943.20151106.1";
                //https://world.taobao.com/search/search.htm?q=%B9%AB%BC%A6&navigator=all&_input_charset=&spm=a21bp.7806943.20151106.1
            }
            else if (page == "1688")
            {
                string a = text;
                string textsearch_1688 = GetHashString(a);
                //string fullLinkSearch_1688 = "https://s.1688.com/selloffer/offer_search.htm?keywords=" + textsearch_1688 + "&button_click=top&earseDirect=false&n=y";
                linkgo = "https://s.1688.com/selloffer/offer_search.htm?keywords=" + textsearch_1688 + "&button_click=top&earseDirect=false&n=y";
            }
            Response.Redirect(linkgo);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "redirect('" + linkgo + "')", true);
            //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "redirect('" + linkgo + "');", true);
        }

        [WebMethod]
        public static string getPopup()
        {
            if (HttpContext.Current.Session["notshowpopup"] == null)
            {
                var conf = ConfigurationController.GetByTop1();
                string popup = conf.NotiPopupTitle;
                if (!string.IsNullOrEmpty(popup))
                {
                    NotiInfo n = new NotiInfo();
                    n.NotiTitle = conf.NotiPopupTitle;
                    n.NotiEmail = conf.NotiPopupEmail;
                    n.NotiContent = conf.NotiPopup;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    return serializer.Serialize(n);
                }
                else
                    return "null";
            }
            else
                return null;
        }
        [WebMethod]
        public static void setNotshow()
        {
            HttpContext.Current.Session["notshowpopup"] = "1";
        }
        [WebMethod]
        public static string closewebinfo()
        {
            HttpContext.Current.Session["infoclose"] = "ok";
            return "ok";
        }
        public class NotiInfo
        {
            public string NotiTitle { get; set; }
            public string NotiContent { get; set; }
            public string NotiEmail { get; set; }
        }

    }
}