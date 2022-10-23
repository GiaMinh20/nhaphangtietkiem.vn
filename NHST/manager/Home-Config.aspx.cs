using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace NHST.manager
{
    public partial class Home_Config : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session["userLoginSystem"] = "phuongnguyen";
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                else
                {
                    string username_current = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(username_current);
                    if (ac.RoleID != 0)
                        Response.Redirect("/trang-chu");
                }
                LoadData();
                LoadStep();
                //LoadService();
                LoadCustomersBenefit();
                LoadNews();
                //LoadCustomers();
                LoadQuestion();
            }
        }

        public string html = "";
        public string htmlList = "";

        private void LoadNews()
        {
            var listNews = NewsController.GetAll();
            StringBuilder hcm = new StringBuilder();
            for (int i = 0; i < listNews.Count; i++)
            {
                var item = listNews[i];
                string Type = "";
                if (item.Type == 1)
                {
                    Type = "Tin tức mới";
                }
                else
                {
                    Type = "Vận chuyển hàng Trung Quốc";
                }
                hcm.Append("<tr>");
                hcm.Append("<td>" + item.NewsTitle + "</td>");
                hcm.Append("<td>" + item.NewsDesc + "</td>");
                hcm.Append("<td>" + item.NewsPosition + "</td>");
                hcm.Append("<td>" + Type + "</td>");
                hcm.Append("<td>" + PJUtils.BoolToStatusShow(item.IsHidden.ToString()) + "</td>");
                //hcm.Append("<td>" + item.ModifiedDate + "</td>");
                hcm.Append("<td>");
                hcm.Append("<div class=\"action-table\">");
                hcm.Append("<a href=\"#modalEditNews\" id=\"EditNews-" + item.ID + "\" onclick=\"EditNews(" + item.ID + ")\" class=\" modal-trigger\" data-position=\"top\" ><i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                hcm.Append("</div>");
                hcm.Append("</td>");
                hcm.Append("</tr>");
            }
            ltrListNews.Text = hcm.ToString();
        }

        public void LoadData()
        {
            var categories = MenuController.GetByLevel(0).OrderBy(c => c.Position).ToList();
            if (categories != null)
            {

                html += "<ul>";
                foreach (var c in categories)
                {
                    //html += "<li data-jstree='{ \"opened\" : false}'>" + c.CategoryName + "";
                    html += "<li id=\"" + c.ID + "\" data-jstree='{ \"opened\" : false}'>" + c.MenuName + " - <span class=\"register-link\" onclick=\"editMenu('" + c.ID + "')\">Edit</span> | <span class=\"register-link\" onclick=\"AddChildMenu('" + c.ID + "')\">Add Child</span> | <span class=\"register-link\" onclick=\"DeleteMenu('" + c.ID + "')\">Delete</span>";
                    Loadtree(c.ID);
                    html += "</li>";
                }
                html += "</ul>";
                ltrTree.Text = html;
            }
        }

        public void Loadtree(int UID)
        {
            var categories = MenuController.GetByLevel(UID);
            if (categories != null)
            {
                html += "<ul>";
                foreach (var c in categories)
                {
                    var ui = MenuController.GetByID(c.ID);
                    html += "<li id=\"" + c.ID + "\" data-jstree='{ \"opened\" : false}'>" + c.MenuName + " - <span class=\"register-link\" onclick=\"editMenu('" + c.ID + "')\">Edit</span>  | <span class=\"register-link\" onclick=\"DeleteMenu('" + c.ID + "')\">Delete</span>";
                    Loadtree(c.ID);
                    html += "</li>";
                }
                html += "</ul>";
            }
        }



        public void LoadStep()
        {
            var listStep = StepController.GetAll("");

            StringBuilder hcm = new StringBuilder();
            for (int i = 0; i < listStep.Count; i++)
            {

                var item = listStep[i];
                hcm.Append("<tr>");
                hcm.Append("<td>" + item.StepName + "</td>");
                hcm.Append("<td>" + PJUtils.ShowIMG40x40(item.StepIMG) + "</td>");
                hcm.Append("<td>" + item.StepLink + "</td>");
                hcm.Append("<td>" + item.StepIndex + "</td>");
                hcm.Append("<td>" + PJUtils.BoolToStatusShow(item.IsHidden.ToString()) + "</td>");
                hcm.Append("<td>" + item.CreatedDate + "</td>");
                hcm.Append("<td>");
                hcm.Append("<div class=\"action-table\">");
                hcm.Append("<a href=\"#modalEditStep\" id=\"EditStep-" + item.ID + "\" onclick=\"EditStep(" + item.ID + ")\" class=\" modal-trigger\" data-position=\"top\"><i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                hcm.Append("</div>");
                hcm.Append("</td>");
                hcm.Append("</tr>");
            }
            ltrListStep.Text = hcm.ToString();
        }
        [WebMethod]
        public static string loadinfoServices(string ID)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var p = ServiceController.GetByID(ID.ToInt(0));
            if (p != null)
            {

                tbl_Service l = new tbl_Service();
                l.ID = p.ID;
                l.ServiceName = p.ServiceName;
                l.ServiceContent = p.ServiceContent;
                l.ServiceLink = p.ServiceLink;
                l.ServiceIMG = p.ServiceIMG;
                l.CreatedDate = p.CreatedDate;
                l.IsHidden = p.IsHidden;
                l.Position = p.Position;
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }
        [WebMethod]
        public static string loadinfoStep(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var p = StepController.GetByID(ID.ToInt(0));
            if (p != null)
            {

                tbl_Step l = new tbl_Step();
                l.ID = p.ID;
                l.StepName = p.StepName;
                l.StepLink = p.StepLink;
                l.StepIndex = p.StepIndex;
                l.StepIMG = p.StepIMG;
                l.StepContent = p.StepContent;
                l.ClassIcon = p.ClassIcon;
                l.CreatedDate = p.CreatedDate;
                l.IsHidden = p.IsHidden;
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }

        [WebMethod]
        public static string loadinfoBenefits(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var p = CustomerBenefitsController.GetByID(ID.ToInt(0));
            if (p != null)
            {

                tbl_CustomerBenefits l = new tbl_CustomerBenefits();
                l.ID = p.ID;
                l.CustomerBenefitName = p.CustomerBenefitName;
                l.CustomerBenefitLink = p.CustomerBenefitLink;
                l.CustomerBenefitDescription = p.CustomerBenefitDescription;
                l.Icon = p.Icon;
                l.ItemType = p.ItemType;
                l.CreatedDate = p.CreatedDate;
                l.IsHidden = p.IsHidden;
                l.Position = p.Position;
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }
        private void LoadService()
        {
            var listService = ServiceController.GetAllAD();

            StringBuilder hcm = new StringBuilder();
            for (int i = 0; i < listService.Count; i++)
            {
                var item = listService[i];
                hcm.Append("<tr>");
                hcm.Append("<td>" + item.Position + "</td>");
                hcm.Append("<td>" + item.ServiceName + "</td>");
                hcm.Append("<td>" + PJUtils.ShowIMG40x40(item.ServiceIMG) + "</td>");
                hcm.Append("<td>" + PJUtils.BoolToStatusShow(item.IsHidden.ToString()) + "</td>");
                hcm.Append("<td>" + item.CreatedDate + "</td>");
                hcm.Append("<td>");
                hcm.Append("<div class=\"action-table\">");
                hcm.Append("<a href=\"#modalEditServices\" id=\"EditServices-" + item.ID + "\" onclick=\"EditServices(" + item.ID + ")\" class=\" modal-trigger\" data-position=\"top\" ><i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                hcm.Append("</div>");
                hcm.Append("</td>");
                hcm.Append("</tr>");
            }
            ltrListService.Text = hcm.ToString();
        }

        private void LoadCustomers()
        {
            StringBuilder hcm = new StringBuilder();
            var listCustomers = ContactController.GetAll();
            for (int i = 0; i < listCustomers.Count; i++)
            {
                var item = listCustomers[i];
                hcm.Append("<tr>");
                hcm.Append("<td>" + item.Fullname + "</td>");
                hcm.Append("<td>" + item.ContactContent + "</td>");
                hcm.Append("<td>" + PJUtils.BoolToStatusShow(item.IsHidden.ToString()) + "</td>");
                hcm.Append("<td>" + string.Format("{0:dd/MM/yyyy HH:mm}", item.ModifiedDate) + "</td>");
                hcm.Append("<td>");
                hcm.Append("<div class=\"action-table\">");
                hcm.Append("<a href=\"#modalEditCustomers\" id=\"EditCustomers-" + item.ID + "\" onclick=\"EditCustomers(" + item.ID + ")\" class=\" modal-trigger\" data-position=\"top\" ><i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                hcm.Append("</div>");
                hcm.Append("</td>");
                hcm.Append("</tr>");
            }
            ltrCustomers.Text = hcm.ToString();
        }

        private void LoadCustomersBenefit()
        {
            var customerBenefits = CustomerBenefitsController.GetAllAD("");
            StringBuilder hcm = new StringBuilder();
            for (int i = 0; i < customerBenefits.Count; i++)
            {

                var item = customerBenefits[i];
                string tempType = "";
                if (item.ItemType == 1)
                {
                    tempType = "Cam kết của chúng tôi";
                }
                else
                {
                    tempType = "Quyền lợi của khách hàng";
                }
                hcm.Append("<tr>");
                hcm.Append("<td>" + item.CustomerBenefitName + "</td>");
                hcm.Append("<td>" + PJUtils.BoolToStatusShow(item.IsHidden.ToString()) + "</td>");
                hcm.Append("<td>" + item.Position + "</td>");
                hcm.Append("<td>" + tempType + "</td>");
                hcm.Append("<td>" + item.CreatedDate + "</td>");
                hcm.Append("<td>");
                hcm.Append("<div class=\"action-table\">");
                hcm.Append("<a href=\"#modalEditBenefits\" id=\"EditBenefits-" + item.ID + "\" onclick=\"EditBenefits(" + item.ID + ")\" class=\" modal-trigger\" data-position=\"top\"><i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                hcm.Append("</div>");
                hcm.Append("</td>");
                hcm.Append("</tr>");
            }
            ltrListCustomersBenefit.Text = hcm.ToString();
        }

        private void LoadQuestion()
        {
            var listNews = QuestionController.GetAll();
            StringBuilder hcm = new StringBuilder();
            for (int i = 0; i < listNews.Count; i++)
            {
                var item = listNews[i];
                hcm.Append("<tr>");
                hcm.Append("<td>" + item.QuesTitle + "</td>");
                hcm.Append("<td>" + item.QuesDesc + "</td>");
                hcm.Append("<td>" + item.QuesPosition + "</td>");
                hcm.Append("<td>" + PJUtils.BoolToStatusShow(item.IsHiden.ToString()) + "</td>");
                hcm.Append("<td>" + item.ModifiedDate + "</td>");
                hcm.Append("<td>");
                hcm.Append("<div class=\"action-table\">");
                hcm.Append("<a href=\"#modalEditQuest\" id=\"EditQuest-" + item.ID + "\" onclick=\"EditQuest(" + item.ID + ")\" class=\" modal-trigger\" data-position=\"top\" ><i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                hcm.Append("</div>");
                hcm.Append("</td>");
                hcm.Append("</tr>");
            }
            ltrQuestion.Text = hcm.ToString();
        }

        [WebMethod]
        public static string loadinfoQuest(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var p = QuestionController.GetByID(ID.ToInt(0));
            if (p != null)
            {
                tbl_Question l = new tbl_Question();
                l.ID = p.ID;
                l.QuesTitle = p.QuesTitle;
                l.QuesDesc = p.QuesDesc;
                l.QuesLink = p.QuesLink;
                l.IsHiden = p.IsHiden;
                l.CreatedDate = p.CreatedDate;
                l.ModifiedDate = p.ModifiedDate;
                l.QuesPosition = p.QuesPosition;
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }

        [WebMethod]
        public static string loadinfoNews(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var p = NewsController.GetByID(ID.ToInt(0));
            if (p != null)
            {
                tbl_News l = new tbl_News();
                l.ID = p.ID;
                l.NewsTitle = p.NewsTitle;
                l.NewsDay = p.NewsDay;
                l.NewsMonth = p.NewsMonth;
                l.NewsDesc = p.NewsDesc;
                l.IsHidden = p.IsHidden;
                l.NewsLink = p.NewsLink;
                l.Type = p.Type;
                l.CreatedDate = p.CreatedDate;
                l.NewsIMG = p.NewsIMG;
                l.ModifiedDate = p.ModifiedDate;
                l.NewsPosition = p.NewsPosition;
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }

        [WebMethod]
        public static string loadinfoCustomers(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var p = ContactController.GetByID(ID.ToInt(0));
            if (p != null)
            {
                tbl_Contact l = new tbl_Contact();
                l.ID = p.ID;
                l.Fullname = p.Fullname;
                l.ContactContent = p.ContactContent;
                l.Images = p.Images;
                l.IsHidden = p.IsHidden;
                l.CreatedDate = p.CreatedDate;
                l.ModifiedDate = p.ModifiedDate;
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }

        protected void btnUpCustomers_Click(object sender, EventArgs e)
        {
            string Username = Session["userLoginSystem"].ToString();
            int CBID = hdfIDCustomers.Value.ToInt(0);
            if (CBID > 0)
            {
                var p = ContactController.GetByID(CBID);
                if (p != null)
                {
                    string Name = txtNameCustomers.Text;
                    string Content = txtContentCustomers.Text;
                    bool IsHidden = Convert.ToBoolean(hdfEditCustomersStatus.Value.ToInt(0));

                    string IMG = "";
                    if (EditCustomersIMG.HasFiles)
                    {
                        foreach (HttpPostedFile f in EditCustomersIMG.PostedFiles)
                        {
                            string fileContentType = f.ContentType; // getting ContentType

                            byte[] tempFileBytes = new byte[f.ContentLength];

                            var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                            string fileName = f.FileName; // getting File Name
                            string fileExtension = Path.GetExtension(fileName).ToLower();

                            var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                            if (result)
                            {
                                if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                                {
                                    if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                                    {
                                        //var o = KhieuNaiIMG + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                        try
                                        {
                                            //f.SaveAs(Server.MapPath(o));
                                            IMG = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                    }
                    else
                        IMG = EditCustomersIMGBefore.ImageUrl;

                    var kq = ContactController.Update(CBID, Name, Content, IsHidden, IMG, DateTime.Now, Username);
                    if (kq != null)
                    {
                        PJUtils.ShowMessageBoxSwAlert("Cập nhật trang thành công.", "s", true, Page);
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý.", "i", true, Page);
                    }
                }

            }
        }

        protected void btnUpNews_Click(object sender, EventArgs e)
        {
            string Email = Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            int CBID = hdfEditNewsID.Value.ToInt(0);
            string BackLink = "/manager/Home-Config.aspx";

            string CustomerNewsName = EditNewsTitle.Text;
            string CustomerNewsDescription = EditNewsSummary.Text;
            int Position = Convert.ToInt32(EditNewsPosition.Text.ToInt(0));
            int Type = Convert.ToInt32(ddlTypeNews.SelectedValue.ToInt(1));
            bool IsHidden = Convert.ToBoolean(hdfEditNewsStatus.Value.ToInt(0));

            string IMG = "";
            if (EditNewsIMG.HasFiles)
            {
                foreach (HttpPostedFile f in EditNewsIMG.PostedFiles)
                {
                    string fileContentType = f.ContentType; // getting ContentType

                    byte[] tempFileBytes = new byte[f.ContentLength];

                    var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                    string fileName = f.FileName; // getting File Name
                    string fileExtension = Path.GetExtension(fileName).ToLower();

                    var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                    if (result)
                    {
                        if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                        {
                            if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                            {
                                //var o = KhieuNaiIMG + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                try
                                {
                                    //f.SaveAs(Server.MapPath(o));
                                    IMG = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            else
                IMG = EditNewsIMGBefore.ImageUrl;

            var kq = NewsController.Update(CBID, CustomerNewsName, EditNewsLink.Text, IsHidden, CustomerNewsDescription, IMG, Position, Email, Type);

            if (kq != null)
            {
                PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật trang thành công.", "s", true, BackLink, Page);
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình cập nhật trang. Vui lòng thử lại.", "e", true, Page);
            }
        }


        protected void BtnUpStep_Click(object sender, EventArgs e)
        {
            string Username = Session["userLoginSystem"].ToString();
            int StepID = hdfStepID.Value.ToInt(0);
            string IMG = "";
            string KhieuNaiIMG = "/Uploads/Images/";
            string BackLink = "/manager/Home-Config.aspx";
            bool IsHidden = Convert.ToBoolean(hdfStepStatus.Value.ToInt(0));
            if (EditStepIMG.HasFiles)
            {
                foreach (HttpPostedFile f in EditStepIMG.PostedFiles)
                {
                    string fileContentType = f.ContentType; // getting ContentType

                    byte[] tempFileBytes = new byte[f.ContentLength];

                    var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                    string fileName = f.FileName; // getting File Name
                    string fileExtension = Path.GetExtension(fileName).ToLower();

                    var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                    if (result)
                    {
                        if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                        {
                            if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                            {
                                //var o = KhieuNaiIMG + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                try
                                {
                                    //f.SaveAs(Server.MapPath(o));
                                    IMG = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                }
            }
            else
                IMG = EditIMGBefore.ImageUrl;

            string kq = StepController.Update(StepID, EditStepName.Text, IMG, Convert.ToInt32(EditStepIndex.Text), EditStepLink.Text, DateTime.UtcNow.AddHours(7), Username, EditStepSumary.Text, EditStepClassIcon.Text, IsHidden);
            if (Convert.ToInt32(kq) > 0)
            {
                PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật thành công.", "s", true, BackLink, Page);
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình Cập nhật. Vui lòng thử lại.", "e", true, Page);
            }
        }

        protected void btnUpServices_Click(object sender, EventArgs e)
        {
            string Email = Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            int CBID = hdfEditServicesID.Value.ToInt(0);
            string BackLink = "/manager/Home-Config.aspx";

            string CustomerBenefitName = EditServicesTitle.Text;
            string CustomerBenefitDescription = EditServicesSummary.Text;
            string CustomerBenefitLink = EditServicesLink.Text;
            int Position = Convert.ToInt32(EditServicesPosition.Text.ToInt(0));

            bool IsHidden = Convert.ToBoolean(hdfEditServicesStatus.Value.ToInt(0));
            string IMG = "";
            string KhieuNaiIMG = "/Uploads/Images/";

            if (EditServicesIMG.HasFiles)
            {
                foreach (HttpPostedFile f in EditServicesIMG.PostedFiles)
                {
                    string fileContentType = f.ContentType; // getting ContentType

                    byte[] tempFileBytes = new byte[f.ContentLength];

                    var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                    string fileName = f.FileName; // getting File Name
                    string fileExtension = Path.GetExtension(fileName).ToLower();

                    var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                    if (result)
                    {
                        if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                        {
                            if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                            {
                                //var o = KhieuNaiIMG + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                try
                                {
                                    //f.SaveAs(Server.MapPath(o));
                                    IMG = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            else
                IMG = EditBenefitIMGBefore.ImageUrl;

            var kq = ServiceController.Update(CBID, CustomerBenefitName, CustomerBenefitDescription, CustomerBenefitLink, IMG, IsHidden, Position, Email);

            if (kq != null)
            {
                PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật trang thành công.", "s", true, BackLink, Page);
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình cập nhật trang. Vui lòng thử lại.", "e", true, Page);
            }
        }

        protected void btnUpBenefit_Click(object sender, EventArgs e)
        {
            string Email = Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            int CBID = hdfEditBenefitID.Value.ToInt(0);
            string BackLink = "/manager/Home-Config.aspx";

            string CustomerBenefitName = EditBenefitName.Text;
            string CustomerBenefitDescription = EditBenefitDescription.Text;
            string CustomerBenefitLink = EditBenefitLink.Text;
            int Position = Convert.ToInt32(EditBenefitPosition.Text);

            bool IsHidden = Convert.ToBoolean(hdfEditBenefitStatus.Value.ToInt(0));
            string IMG = "";
            string KhieuNaiIMG = "/Uploads/Images/";

            if (EditBenefitIMG.HasFiles)
            {
                foreach (HttpPostedFile f in EditBenefitIMG.PostedFiles)
                {
                    string fileContentType = f.ContentType; // getting ContentType

                    byte[] tempFileBytes = new byte[f.ContentLength];

                    var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                    string fileName = f.FileName; // getting File Name
                    string fileExtension = Path.GetExtension(fileName).ToLower();

                    var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                    if (result)
                    {
                        if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                        {
                            if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                            {
                                //var o = KhieuNaiIMG + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                try
                                {
                                    //f.SaveAs(Server.MapPath(o));
                                    IMG = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            else
                IMG = EditBenefitIMGBefore.ImageUrl;


            var kq = CustomerBenefitsController.Update(CBID, IMG, CustomerBenefitName,
                CustomerBenefitDescription, CustomerBenefitLink, IsHidden, Position, Email, ddlEditBenefitType.SelectedValue.ToInt());

            if (kq != null)
            {
                PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật trang thành công.", "s", true, BackLink, Page);
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình cập nhật trang. Vui lòng thử lại.", "e", true, Page);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string t = hdfTest.Value;
            string[] list = t.Split('*');

            for (int i = 0; i < list.Length - 1; i++)
            {
                string[] value = list[i].Split('-');
                var root = MenuController.UpdateIndex(value[0].ToInt(), i, 0);
                if (root != null)
                {
                    if (!string.IsNullOrEmpty(value[1]))
                    {
                        string[] vl1 = value[1].Split('|');
                        for (int j = 0; j < vl1.Length - 1; j++)
                        {
                            var child = MenuController.UpdateIndex(vl1[j].ToInt(), j, root.ID);
                        }
                    }
                }

            }

            string Backlink = "/manager/Home-Config.aspx";
            PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật thành công.", "s", true, Backlink, Page);
        }

        protected void btnDeleteMenu_Click(object sender, EventArgs e)
        {
            string Email = Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            int CBID = hdfMenuID.Value.ToInt(0);

            var menu = MenuController.GetByID(CBID);
            if (menu != null)
            {
                MenuController.Delete(menu.ID);
                PJUtils.ShowMessageBoxSwAlert("Xóa thành công.", "s", true, Page);
            }
        }

        protected void btnUpQuest_Click(object sender, EventArgs e)
        {
            string Email = Session["userLoginSystem"].ToString();
            int CBID = hdfEditQuestID.Value.ToInt(0);
            string BackLink = "/manager/Home-Config.aspx";

            string Name = txtNameQuest.Text;
            string Content = txtContentQuest.Text;
            string Link = txtLinkQuest.Text;
            int Position = Convert.ToInt32(txtIndexQuest.Text.ToInt(0));
            bool IsHidden = Convert.ToBoolean(hdfEditQuestStatus.Value.ToInt(0));

            string IMG = "";
            if (EditNewsIMG.HasFiles)
            {
                foreach (HttpPostedFile f in EditNewsIMG.PostedFiles)
                {
                    string fileContentType = f.ContentType; // getting ContentType

                    byte[] tempFileBytes = new byte[f.ContentLength];

                    var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                    string fileName = f.FileName; // getting File Name
                    string fileExtension = Path.GetExtension(fileName).ToLower();

                    var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                    if (result)
                    {
                        if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                        {
                            if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                            {
                                //var o = KhieuNaiIMG + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                try
                                {
                                    //f.SaveAs(Server.MapPath(o));
                                    IMG = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            else
                IMG = EditNewsIMGBefore.ImageUrl;

            var kq = QuestionController.Update(CBID, Name, Content, Link, IsHidden, IMG, Position, Email);

            if (kq != null)
            {
                PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật trang thành công.", "s", true, BackLink, Page);
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình cập nhật trang. Vui lòng thử lại.", "e", true, Page);
            }
        }
    }
}