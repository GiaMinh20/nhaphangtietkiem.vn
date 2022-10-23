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

namespace NHST.manager
{
    public partial class add_withdraw : System.Web.UI.Page
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
                    if (ac.RoleID != 0 && ac.RoleID !=7)
                        Response.Redirect("/trang-chu");
                }
                LoadData();
            }
        }
        public void LoadData()
        {
            string urlName = Request.UrlReferrer.ToString();
            ltr.Text = "<a href=\"" + urlName + "\" class=\"btn\">Trở về</a>";
            if (Request.QueryString["u"] != null)
            {
                string username = Request.QueryString["u"];
                string username_current = Session["userLoginSystem"].ToString();
                var ac = AccountController.GetByUsername(username_current);
                var u = AccountController.GetByUsername(username);
                if (u != null)
                {
                    rp_username.Text = username;
                    ViewState["Username"] = rp_username.Text;
                    rp_textarea.Text = ac.Username + " đã rút tiền từ tài khoản.";
                }
            }
        }

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(rp_vnd.Text))
            {
                string username_current = Session["userLoginSystem"].ToString();
                string username_bitru = ViewState["Username"].ToString();
                var acc = AccountController.GetByUsername(username_bitru);
                string BackLink = "/manager/Withdraw-List.aspx";
                if (acc != null)
                {
                    int UID = acc.ID;
                    double wallet = Convert.ToDouble(acc.Wallet);
                    double amount = Convert.ToDouble(rp_vnd.Text);
                    //int status = ddlStatus.SelectedValue.ToInt();
                    DateTime currentDate = DateTime.Now;
                    if (wallet >= amount)
                    {
                        //Cho rút
                        double leftwallet = wallet - amount;

                        //Cập nhật lại ví
                        AccountController.updateWallet(UID, leftwallet, currentDate, username_current);

                        //Thêm vào History Pay wallet
                        HistoryPayWalletController.Insert(UID, username_bitru, 0, amount, rp_textarea.Text, leftwallet, 1, 5, currentDate, username_current);

                        //Thêm vào lệnh rút tiền
                        WithdrawController.Insert(UID, username_bitru, amount, 2, rp_textarea.Text, currentDate, username_current);
                        PJUtils.ShowMessageBoxSwAlertBackToLink("Tạo lệnh rút tiền thành công", "s", true, BackLink, Page);
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Số tiền không đủ để tạo lệnh rút", "e", true, Page);
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Không tồn tại tài khoản trên", "e", true, Page);
                }
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Chưa nhập số tiền cần rút", "e", true, Page);
            }

        }
    }
}