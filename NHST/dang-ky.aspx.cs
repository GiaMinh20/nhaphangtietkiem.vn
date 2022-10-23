using MB.Extensions;
using Microsoft.AspNet.SignalR;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Hubs;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ZLADIPJ.Business;

namespace NHST
{
    public partial class dang_ky2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                LoadDDL();
            }
        }
        
        public void LoadDDL()
        {            
            ddlWarehouseFrom.Items.Clear();
            ddlWarehouseFrom.Items.Insert(0, "Chọn kho TQ");           
            var warehousefrom = WarehouseFromController.GetAllWithIsHidden(false);
            if (warehousefrom.Count > 0)
            {
                ddlWarehouseFrom.DataSource = warehousefrom;
                ddlWarehouseFrom.DataBind();
            }
            
            var shipping = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shipping.Count > 0)
            {
                ddlShippingType.DataSource = shipping;
                ddlShippingType.DataBind();
            }

            var warehouse = WarehouseController.GetAllWithIsHidden(false);
            if (warehouse.Count > 0)
            {
                ddlReceivePlace.DataSource = warehouse;
                ddlReceivePlace.DataBind();
            }
            ListItem sleTTKho = new ListItem("Chọn kho Việt Nam", "0");
            ddlReceivePlace.Items.Insert(0, sleTTKho);

            var sale = AccountController.GetListRoleID(6);
            if (sale.Count > 0)
            {
                foreach (var item in sale)
                {
                    ListItem us = new ListItem(item.Username, item.ID.ToString());
                    ddlSaler.Items.Add(us);
                }
            }
            ListItem sleTT = new ListItem("Chọn nhân viên sale", "0");
            ddlSaler.Items.Insert(0, sleTT);
        }
        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string Username = txtUsername.Text.Trim().ToLower();
                string Email = txtEmail.Text.Trim();
                var checkuser = AccountController.GetByUsername(Username);
                var checkemail = AccountController.GetByEmail(Email);
                string Phone = txtPhone.Text.Trim().Replace(" ", "");
                var getaccountinfor = AccountInfoController.GetByPhone(Phone);
                bool checkusernamebool = false;
                bool checkemailbool = false;
                bool checkphonebool = false;
                string error = "";
                bool check = PJUtils.CheckUnicode(Username);
                DateTime currentDate = DateTime.Now;
                DateTime bir = DateTime.Now;
                int nvkdID = ddlSaler.SelectedValue.ToInt(0);              
                var setNoti = SendNotiEmailController.GetByID(1);                
                if (!string.IsNullOrEmpty(rBirthday.Text.ToString()))
                {
                    bir = DateTime.ParseExact(rBirthday.Text, "dd/MM/yyyy", null);
                }
                if (Username.Contains(" "))
                {                
                    PJUtils.ShowMessageBoxSwAlert("Tên đăng nhập không được có dấu cách.", "e", false, Page);
                }
                else if (check == true)
                {                   
                    PJUtils.ShowMessageBoxSwAlert("Tên đăng nhập không được có dấu tiếng Việt.", "e", false, Page);
                }
                else
                {
                    if (checkuser != null)
                    {                        
                        error += "Tên đăng nhập / Nickname đã được sử dụng vui lòng chọn Tên đăng nhập / Nickname khác.<br/>";
                        checkusernamebool = true;
                    }
                    if (checkemail != null)
                    {
                        //lblcheckemail.Visible = true;
                        error += "Email đã được sử dụng vui lòng chọn Email khác.<br/>";
                        checkemailbool = true;
                    }
                    if (getaccountinfor != null)
                    {
                        //lblcheckemail.Visible = true;
                        error += "Số điện thoại đã được sử dụng vui lòng chọn Số điện thoại khác.<br/>";
                        checkphonebool = true;
                    }
                    if (checkusernamebool == false && checkemailbool == false && checkphonebool == false)
                    {
                        string Token = PJUtils.RandomStringWithText(16);
                        string id = AccountController.Insert(Username, Email, txtpass.Text.Trim(), 1, 1, 1, 2, nvkdID, 0, DateTime.UtcNow.AddHours(7), "", DateTime.UtcNow.AddHours(7), "", Token);
                        if (Convert.ToInt32(id) > 0)
                        {
                            string MaGioiThieu = "MONA" + id + "";
                            AccountController.UpdateMaGioiThieu(id.ToInt(0), MaGioiThieu);

                            AccountController.updatewarehouseFromwarehouseTo(id.ToInt(0), ddlWarehouseFrom.SelectedValue.ToInt(0), ddlReceivePlace.SelectedValue.ToInt(0));
                            AccountController.updateshipping(id.ToInt(0), ddlShippingType.SelectedValue.ToInt(0));
                            AccountController.UpdateScanWareHouse(id.ToInt(0), 0, 0);

                            int UID = Convert.ToInt32(id);
                            string idai = AccountInfoController.Insert(UID, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), "",
                                Phone, Email, txtPhone.Text.Trim(), txtAddress.Text, "", "", bir, ddlGender.SelectedValue.ToString().ToInt(1),
                                DateTime.UtcNow.AddHours(7), "", DateTime.UtcNow.AddHours(7), "");
                            if (idai == "1")
                            {
                                tbl_Account ac = AccountController.Login(Username, txtpass.Text.Trim());
                                if (ac != null)
                                {                                   
                                    if (ac.Status == 2)
                                    {                                       
                                        Session["userLoginSystem"] = ac.Username;
                                        if (setNoti != null)
                                        {
                                            if (setNoti.IsSentNotiAdmin == true)
                                            {
                                                var admins = AccountController.GetAllByRoleID(0);
                                                if (admins.Count > 0)
                                                {
                                                    foreach (var admin in admins)
                                                    {
                                                        NotificationsController.Inser(admin.ID, admin.Username, 0, "Khách hàng mới có username là: " + ac.Username,
                                                                                      6, currentDate, ac.Username, false);                                                       
                                                    }
                                                }

                                                var managers = AccountController.GetAllByRoleID(2);
                                                if (managers.Count > 0)
                                                {
                                                    foreach (var manager in managers)
                                                    {
                                                        NotificationsController.Inser(manager.ID, manager.Username, 0, "Khách hàng mới có username là: " + ac.Username,
                                                                                      6, currentDate, ac.Username, false);                                                        
                                                    }
                                                }                                                
                                            }
                                            if (setNoti.IsSendEmailUser == true)
                                            {
                                                try
                                                {
                                                    PJUtils.SendMailGmail("no-reply@mona-media.com", "demonhunter", ac.Email,
                                                        "Thông báo tại Nhập hàng Tiết Kiệm.", "Chào mừng bạn đã đến với THUẬN ĐẠT EXPRESS.", "");
                                                }
                                                catch { }
                                            }
                                        }
                                       
                                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                                        hubContext.Clients.All.addNewMessageToPage("", "");
                                        Response.Redirect("/thong-tin-nguoi-dung");
                                    }                                    
                                }                                
                            }
                        }
                    }
                    else
                    {                      
                        PJUtils.ShowMessageBoxSwAlert(error, "e", false, Page);
                    }
                }
            }
        }
    }
}