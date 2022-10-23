using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST.manager
{
    public partial class Saler_AddUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/manager/Login.aspx");
                }
                else
                {
                    string Username = Session["userLoginSystem"].ToString();
                    var obj_user = AccountController.GetByUsername(Username);
                    if (obj_user != null)
                    {
                        if (obj_user.RoleID != 6)
                        {
                            Response.Redirect("/trang-chu");
                        }
                    }

                }
                loadPrefix();               
            }
        }
        public void loadPrefix()
        {
            var Levels = UserLevelController.GetAll("");
            if (Levels.Count > 0)
            {
                ddlLevelID.DataSource = Levels;
                ddlLevelID.DataBind();
            }
        }
       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string Username = Session["userLoginSystem"].ToString();
            string Email = txtEmail.Text.Trim();
            string nickname = txtUsername.Text.Trim();           
            int SaleID = 0;
            var obj_user = AccountController.GetByUsername(Username);
            if (obj_user != null)
            {
                SaleID = obj_user.ID;
            }
            bool check = PJUtils.CheckUnicode(nickname);
            int LevelID = ddlLevelID.SelectedValue.ToString().ToInt();           
            int VIPLevel = 0;
            var checkuser = AccountController.GetByUsername(nickname);
            var checkemail = AccountController.GetByEmail(Email);           
            int RoleID = 1;
            var getaccountinfor = AccountInfoController.GetByPhone(txtPhone.Text.Trim());

            if (nickname.Contains(" "))
            {
                PJUtils.ShowMessageBoxSwAlert("Tên đăng nhập không được có dấu cách.", "i", false, Page);
            }
            else if (check == true)
            {
                PJUtils.ShowMessageBoxSwAlert("Tên đăng nhập không được có dấu tiếng Việt.", "i", false, Page);
            }
            else if (checkuser != null)
            {
                PJUtils.ShowMessageBoxSwAlert("Tên đăng nhập / đã được sử dụng vui lòng chọn Tên đăng nhập khác.", "i", false, Page);
            }
            else if (checkemail != null)
            {
                PJUtils.ShowMessageBoxSwAlert("Email đã được sử dụng vui lòng chọn Email khác.", "i", false, Page);
            }
            else if (getaccountinfor != null)
            {
                PJUtils.ShowMessageBoxSwAlert("Số điện thoại đã được sử dụng vui lòng chọn Số điện thoại khác.", "i", false, Page);
            }
            else
            {
                string Token = PJUtils.RandomStringWithText(16);
                string id = AccountController.Insert(nickname, Email, txt_Password.Text.Trim(), RoleID, LevelID, VIPLevel, Convert.ToInt32(ddlStatus.SelectedValue),
                                                                    SaleID, 0, DateTime.UtcNow.AddHours(7), Username, DateTime.UtcNow.AddHours(7), Username, Token);
                int UID = Convert.ToInt32(id);
                if (UID > 0)
                {
                    AccountController.UpdateScanWareHouse(UID, 0, 0);
                    string idai = AccountInfoController.Insert(UID, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), "", txtPhone.Text.Trim(), Email, txtPhone.Text.Trim(), "", "", "",
                        DateTime.ParseExact(rBirthday.Text, "dd/MM/yyyy HH:mm", null), ddlGender.SelectedValue.ToInt(1), DateTime.UtcNow.AddHours(7), "", DateTime.UtcNow.AddHours(7), "");
                    if (idai == "1")
                    {
                        PJUtils.ShowMsg("Tạo tài khoản thành công.", true, Page);
                    }
                }
            }
        }
    }
}