using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace NHST
{
    public partial class thong_tin_nguoi_dung : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                loadPrefix();
                loaddata();
            }
        }
        public void loadPrefix()
        {
            var khotq = WarehouseFromController.GetAllWithIsHidden(false);
            if (khotq.Count > 0)
            {
                foreach (var item in khotq)
                {
                    ListItem us = new ListItem(item.WareHouseName, item.ID.ToString());
                    ddlWareHouseFrom.Items.Add(us);
                }
            }
            ListItem sleTT = new ListItem("Chọn kho TQ", "0");
            ddlWareHouseFrom.Items.Insert(0, sleTT);



            var khovn = WarehouseController.GetAllWithIsHidden(false);
            if (khovn.Count > 0)
            {
                foreach (var item in khovn)
                {
                    ListItem us = new ListItem(item.WareHouseName, item.ID.ToString());
                    ddlWareHouse.Items.Add(us);
                }
            }
            ListItem sleTT1 = new ListItem("Chọn kho VN", "0");
            ddlWareHouse.Items.Insert(0, sleTT1);

            var shipping = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shipping.Count > 0)
            {
                foreach (var item in shipping)
                {
                    ListItem us = new ListItem(item.ShippingTypeName, item.ID.ToString());
                    ddlShipping.Items.Add(us);
                }
            }
            ListItem sleTT2 = new ListItem("Chọn phương thức VC", "0");
            ddlShipping.Items.Insert(0, sleTT2);
        }
        public void loaddata()
        {
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                int id = obj_user.ID;
                ViewState["UID"] = id;
                txtUsername.Text = username;
                txtEmail.Text = obj_user.Email;
                var ai = AccountInfoController.GetByUserID(id);
                if (ai != null)
                {
                    txtFirstName.Text = ai.FirstName;
                    txtLastName.Text = ai.LastName;
                    //ddlPrefix.SelectedValue = ai.MobilePhonePrefix;
                    txtPhone.Text = ai.Phone;
                    txtAddress2.Text = ai.Address;
                    //if (ai.BirthDay != null)
                    //    rBirthday.SelectedDate = ai.BirthDay;
                    if (ai.Gender != null)
                        ddlGender.SelectedValue = ai.Gender.ToString();
                    //txtEmail.Text = ai.Email;
                    txtBirthDay.Text = string.Format("{0:dd/MM/yyyy HH:mm}", ai.BirthDay);
                    if (!string.IsNullOrEmpty(ai.IMGUser))
                    {
                        UpIMGBefore.ImageUrl = ai.IMGUser;
                    }

                    ddlShipping.SelectedValue = obj_user.ShippingType.ToString();
                    ddlWareHouse.SelectedValue = obj_user.WarehouseTo.ToString();
                    ddlWareHouseFrom.SelectedValue = obj_user.WarehouseFrom.ToString();

                    ltrProfile.Text += "<div class=\"card-image waves-effect waves-block waves-light\">";
                    ltrProfile.Text += "<img class=\"activator\" src=\"/App_Themes/UserNew45/assets/images/gallery/19.png\" alt=\"user bg\">";
                    ltrProfile.Text += "</div>";
                    ltrProfile.Text += "<div class=\"card-content\">";
                    if (!string.IsNullOrEmpty(ai.IMGUser))
                    {
                        ltrProfile.Text += "<img src=\"" + ai.IMGUser + "\" alt=\"\" class=\"circle responsive-img activator card-profile-image green lighten-5 padding-2\">";
                    }
                    else
                    {
                        ltrProfile.Text += "<img src=\"NO-IMAGE.jpg\" alt=\"\" class=\"circle responsive-img activator card-profile-image green lighten-5 padding-2\">";
                    }
                    ltrProfile.Text += "<h5 class=\"card-title activator grey-text text-darken-4\">" + ai.FirstName + " " + ai.LastName + "</h5>";
                    //ltrProfile.Text += "<p><i class=\"material-icons profile-card-i\">star</i>Mã giới thiệu của bạn: " + obj_user.MaGioiThieu + "</p>";
                    if (ai.Gender == 1)
                        ltrProfile.Text += "<p><i class=\"material-icons profile-card-i\">perm_identity</i> Nam</p>";
                    else
                        ltrProfile.Text += "<p><i class=\"material-icons profile-card-i\">perm_identity</i> Nữ</p>";
                    ltrProfile.Text += "<p><i class=\"material-icons profile-card-i\">perm_phone_msg</i> " + ai.Phone + "</p>";
                    ltrProfile.Text += "<p><i class=\"material-icons profile-card-i\">email</i> " + obj_user.Email + "</p>";
                    ltrProfile.Text += "<p><i class=\"material-icons profile-card-i\">location_on</i> " + ai.Address + "</p>";

                    //if (!string.IsNullOrEmpty(obj_user.GioiThieuID))
                    //{
                    //    ltrProfile.Text += "<p><i class=\"material-icons profile-card-i\">code</i>Bạn đã được giới thiệu bởi mã: " + obj_user.GioiThieuID + "</p>";
                    //    ltrProfile.Text += "<p>";
                    //}
                    //else
                    //{
                    //    ltrProfile.Text += "<p><i class=\"material-icons profile-card-i\">code</i>Nhập mã giới thiệu</p>";
                    //    ltrProfile.Text += "<p><input class=\"txtNote\">" + obj_user.GioiThieuID + "</input>";
                    //    ltrProfile.Text += "<a style=\"margin-top: 5px;\" href =\"javascript:;\" class=\"btn btn-info\" onclick=\"updateNote($(this),'" + obj_user.ID + "')\">Cập nhật</a>";
                    //    ltrProfile.Text += "<span class=\"update-info\" style=\"width:100%; clear:both; float:left; color blue; display:none\">Cập nhật thành công</span></td>";
                    //    ltrProfile.Text += "<p>";
                    //}

                    ltrProfile.Text += "</div>";
                }
            }
        }

        [WebMethod]
        public static string UpdateStaffNote(int ID, string staffNote)
        {
            string Username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var ac = AccountController.GetByID(ID);
            DateTime currentdate = DateTime.Now;
            if (ac != null)
            {
                var a = AccountController.GetByMaGioiThieu(staffNote);
                if (ac.MaGioiThieu != a.MaGioiThieu)
                {
                    AccountController.UpdateGioiThieuID(ID, staffNote);
                    HistoryIntroduceController.Insert(a.ID, a.Username, "0", "Tài khoản " + ac.Username + " đã nhập mã giới thiệu của bạn.", "0", 1, currentdate, Username);
                    return "ok";
                }
                else
                {
                    return "wrong";
                }
            }
            return "none";
        }

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);          
            int UID = ViewState["UID"].ToString().ToInt(0);
            string pass = txtpass.Text.Trim();
            string confirmpass = txtconfirmpass.Text.Trim();
            DateTime birthday = DateTime.Now;
            if (!string.IsNullOrEmpty(txtBirthDay.Text.ToString()))
            {
                birthday = DateTime.ParseExact(txtBirthDay.Text.ToString(), "dd/MM/yyyy HH:mm", null);
            }

            bool checkemailbool = false;
            bool checkpass = false;

            string Email = txtEmail.Text.Trim();
            var CheckEmail = AccountController.GetByEmailNotMe(Email, UID);
            if (CheckEmail != null)
            {
                checkemailbool = true;
            }
            else
            {
                AccountController.UpdateEmail(UID, Email);
                AccountInfoController.UpdateEmail(UID, Email);
                checkemailbool = false;
            }
            if (!string.IsNullOrEmpty(pass))
            {
                if (!string.IsNullOrEmpty(confirmpass))
                {
                    if (confirmpass == pass)
                    {
                        string IMG = "";                       
                        if (UpIMG.PostedFiles.Count > 0)
                        {
                            foreach (HttpPostedFile f in UpIMG.PostedFiles)
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
                            IMG = UpIMGBefore.ImageUrl;

                        AccountController.updatewarehouseFromwarehouseTo(UID, Convert.ToInt32(ddlWareHouseFrom.SelectedValue), Convert.ToInt32(ddlWareHouse.SelectedValue));
                        AccountController.updateshipping(UID, Convert.ToInt32(ddlShipping.SelectedValue));
                        AccountInfoController.UpdateIMGUser(UID, IMG);
                        AccountController.UpdatePassword(UID, pass);
                        AccountInfoController.UpdateNew(UID, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtPhone.Text,
                        txtAddress2.Text.Trim(), birthday, ddlGender.SelectedValue.ToInt(), "", "", DateTime.UtcNow.AddHours(7), txtUsername.Text);
                        checkpass = true;
                    }
                    else
                    {
                        lblConfirmpass.Text = "Mật khẩu mới trùng với mật khẩu cũ.";
                        lblConfirmpass.Visible = true;
                    }
                }
                else
                {
                    lblConfirmpass.Text = "Không để trống xác nhận mật khẩu";
                    lblConfirmpass.Visible = true;
                }
            }
            else
            {
                string IMG = "";               
                if (UpIMG.PostedFiles.Count > 0)
                {
                    foreach (HttpPostedFile f in UpIMG.PostedFiles)
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
                    IMG = UpIMGBefore.ImageUrl;
                AccountController.updatewarehouseFromwarehouseTo(UID, Convert.ToInt32(ddlWareHouseFrom.SelectedValue), Convert.ToInt32(ddlWareHouse.SelectedValue));
                AccountController.updateshipping(UID, Convert.ToInt32(ddlShipping.SelectedValue));
                AccountInfoController.UpdateIMGUser(UID, IMG);
                AccountInfoController.UpdateNew(UID, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtPhone.Text,
                txtAddress2.Text.Trim(), birthday, ddlGender.SelectedValue.ToInt(), "", "", DateTime.UtcNow.AddHours(7), txtUsername.Text);
                checkpass = true;
            }    

            if (checkemailbool == true)
            {
                PJUtils.ShowMessageBoxSwAlert("Email này đã tồn tại trong hệ thống.", "i", true, Page);
            }    
            else if (checkemailbool == false && checkpass == true)
            {
                PJUtils.ShowMessageBoxSwAlert("Cập nhật thông tin thành công.", "s", true, Page);
            }    
        }
    }
}