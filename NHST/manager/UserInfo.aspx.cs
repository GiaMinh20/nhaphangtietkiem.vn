using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MB.Extensions;
using NHST.Controllers;
using NHST.Bussiness;
using NHST.Models;
using System.Text;

namespace NHST.manager
{
    public partial class UserInfo : System.Web.UI.Page
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
                    string page1 = Request.Url.ToString();
                    string page2 = Request.UrlReferrer.ToString();
                    if (page1 != page2)
                        Session["PrePage"] = page2;

                    if (ac.RoleID == 1)
                        Response.Redirect("/trang-chu");
                }
                loadPrefix();
                LoadSaler();
                loaddata();
            }
        }
        public void loadPrefix()
        {            
            var Levels = UserLevelController.GetAll("");
            if (Levels.Count > 0)
            {
                foreach (var item in Levels)
                {
                    ListItem listitem = new ListItem(item.LevelName, item.ID.ToString());
                    ddlLevelID.Items.Add(listitem);
                }

            }
        }
        public void LoadSaler()
        {
            var salers = AccountController.GetAllByRoleID(6);
            ddlSale.Items.Clear();
            ddlSale.Items.Insert(0, "Chọn nhân viên kinh doanh");
            if (salers.Count > 0)
            {
                ddlSale.DataSource = salers;
                ddlSale.DataBind();
            }
            var dathangs = AccountController.GetAllByRoleID(3);
            ddlDathang.Items.Clear();
            ddlDathang.Items.Insert(0, "Chọn nhân viên đặt hàng");
            if (dathangs.Count > 0)
            {
                ddlDathang.DataSource = dathangs;
                ddlDathang.DataBind();
            }


            var khotq = WarehouseFromController.GetAllWithIsHidden(false);
            ddlWareHouseTQ.Items.Clear();
            ddlWareHouseTQ.Items.Insert(0, "Chọn kho TQ");
            if (khotq.Count > 0)
            {
                ddlWareHouseTQ.DataSource = khotq;
                ddlWareHouseTQ.DataBind();
            }

            var khovn = WarehouseController.GetAllWithIsHidden(false);
            ddlWareHouseVN.Items.Clear();
            ddlWareHouseVN.Items.Insert(0, "Chọn kho VN");
            if (khovn.Count > 0)
            {
                ddlWareHouseVN.DataSource = khovn;
                ddlWareHouseVN.DataBind();
            }
        }
        public void loaddata()
        {
            var id = Request.QueryString["i"].ToInt(0);
            if (id > 0)
            {
                string username_current = Session["userLoginSystem"].ToString();
                tbl_Account ac = AccountController.GetByUsername(username_current);
                if (ac.RoleID == 0)
                {
                    pnAdmin.Visible = true;
                }
                else
                {
                    if (ac.ID != id)
                    {
                        Response.Redirect("/trang-chu");
                    }
                }
                ViewState["UID"] = id;
                var a = AccountController.GetByID(id);
                if (a != null)
                {          
                    txtUsername.Text = a.Username;
                    txtEmail.Text = a.Email;
                    var ai = AccountInfoController.GetByUserID(id);
                    if (ai != null)
                    {
                        txtFirstName.Text = ai.FirstName;
                        txtLastName.Text = ai.LastName;                       
                        txtPhone.Text = ai.Phone;
                        txtAddress.Text = ai.Address;
                       
                        if (ai.BirthDay != null)
                            txtBirthday.Text = ai.BirthDay.Value.ToString("dd/MM/yyyy HH:mm");
                        if (ai.Gender != null)
                            ddlGender.SelectedValue = ai.Gender.ToString();
                    }
                    ddlRole.SelectedValue = a.RoleID.ToString();
                    ddlStatus.SelectedValue = a.Status.ToString();
                    ddlLevelID.SelectedValue = a.LevelID.ToString();
                    ddlVipLevel.SelectedValue = a.VIPLevel.ToString();
                    ddlSale.SelectedValue = a.SaleID.ToString();
                    ddlDathang.SelectedValue = a.DathangID.ToString();
                    ddlWareHouseTQ.SelectedValue = a.WareHouseTQ.ToString();
                    ddlWareHouseVN.SelectedValue = a.WareHouseVN.ToString();

                    if (a.Currency != null)
                        rCurrency.Text = Convert.ToDouble(a.Currency).ToString();
                    else
                        rCurrency.Text = "0";

                    if (!string.IsNullOrEmpty(a.FeeBuyPro))
                        txtFeebuypro.Text = a.FeeBuyPro;
                    else
                        txtFeebuypro.Text = "";

                    if (!string.IsNullOrEmpty(a.FeeTQVNPerWeight))
                        txtFeeWeight.Text = a.FeeTQVNPerWeight;
                    else
                        txtFeeWeight.Text = "";

                    if (a.Deposit != null)
                        rDeposit.Text = Convert.ToDouble(a.Deposit).ToString();
                    else
                        rDeposit.Text = "0";
                }
                else
                {
                    Response.Redirect("/manager/Home.aspx");
                }
            }
        }
        protected void btncreateuser_Click(object sender, EventArgs e)
        {           
            int UID = ViewState["UID"].ToString().ToInt(0);
            string username_current = Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.UtcNow;
           
            string pass = txtpass.Text.Trim();
            string confirmpass = txtconfirmpass.Text.Trim();
            string Email = txtEmail.Text.Trim();
            int Status = ddlStatus.SelectedValue.ToString().ToInt();
            int RoleID = ddlRole.SelectedValue.ToString().ToInt();
            int LevelID = ddlLevelID.SelectedValue.ToString().ToInt();
            int SaleID = ddlSale.SelectedValue.ToString().ToInt(0);           
            int DathangID = ddlDathang.SelectedValue.ToString().ToInt(0);
            int WareHouseTQ = ddlWareHouseTQ.SelectedValue.ToString().ToInt(0);
            int WareHouseVN = ddlWareHouseVN.SelectedValue.ToString().ToInt(0);

            bool checkemailbool = false;
            bool checkpass = false;
            bool checknotpass = false;
            bool checkpasskhacnhau = false;

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
                        AccountController.updateLevelID(UID, LevelID, currentDate, username_current);
                        AccountController.updatestatus(UID, Status, currentDate, username_current);
                        AccountController.UpdateRole(UID, RoleID, currentDate, username_current);
                        AccountController.UpdateSaleID(UID, SaleID, currentDate, username_current);
                        AccountController.UpdateDathangID(UID, DathangID, currentDate, username_current);
                        AccountController.UpdateFee(UID, txtFeebuypro.Text, txtFeeWeight.Text);
                        AccountController.UpdateScanWareHouse(UID, WareHouseTQ, WareHouseVN);
                        AccountController.UpdatePassword(UID, pass);
                        AccountInfoController.UpdateNew(UID, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtPhone.Text, txtAddress.Text.Trim(),
                        DateTime.ParseExact(txtBirthday.Text, "dd/MM/yyyy HH:mm", null), ddlGender.SelectedValue.ToInt(), "", "", currentDate, username_current);
                        checkpass = true;
                    }  
                    else
                    {
                        checkpasskhacnhau = true;
                    }    
                }  
                else
                {
                    checknotpass = true;
                }    
            }
            else
            {
                AccountController.updateLevelID(UID, LevelID, currentDate, username_current);
                AccountController.updatestatus(UID, Status, currentDate, username_current);
                AccountController.UpdateRole(UID, RoleID, currentDate, username_current);
                AccountController.UpdateSaleID(UID, SaleID, currentDate, username_current);
                AccountController.UpdateDathangID(UID, DathangID, currentDate, username_current);
                AccountController.UpdateFee(UID, txtFeebuypro.Text, txtFeeWeight.Text);
                AccountController.UpdateScanWareHouse(UID, WareHouseTQ, WareHouseVN);
                AccountInfoController.UpdateNew(UID, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtPhone.Text, txtAddress.Text.Trim(),
                DateTime.ParseExact(txtBirthday.Text, "dd/MM/yyyy HH:mm", null), ddlGender.SelectedValue.ToInt(), "", "", DateTime.UtcNow.AddHours(7), username_current);
                checkpass = true;
            }

            if (checkpass == true && checkemailbool == false)
            {
                PJUtils.ShowMsg("Cập nhật thành công", true, Page);
            }    
            else if (checkemailbool == true)
            {
                PJUtils.ShowMessageBoxSwAlert("Email này đã tồn tại trong hệ thống.", "i", true, Page);
            }    
            else if (checknotpass == true)
            {
                PJUtils.ShowMessageBoxSwAlert("Không để trống xác nhận mật khẩu", "i", false, Page);
            }
            else if (checkpasskhacnhau == true)
            {
                PJUtils.ShowMessageBoxSwAlert("Mật khẩu xác nhận không trùng nhau.", "i", false, Page);
            }             
        }

        protected void btnBackLink_Click(object sender, EventArgs e)
        {
            string prepage = Session["PrePage"].ToString();
            if (!string.IsNullOrEmpty(prepage))
            {
                Response.Redirect(prepage);
            }
            else
            {
                Response.Redirect(Request.Url.ToString());
            }
        }
    }
}