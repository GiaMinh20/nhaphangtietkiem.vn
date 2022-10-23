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
    public partial class UserWallet : System.Web.UI.Page
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
                    if (ac.RoleID == 0 || ac.RoleID == 7 || ac.RoleID == 2)
                    {

                    }
                    else
                        Response.Redirect("/trang-chu");
                }
                LoadDLL();
                loaddata();
            }
        }


        public class Bank
        {
            public int ID { get; set; }
            public string BankInfo { get; set; }
        }

        public void LoadDLL()
        {
            List<Bank> lb = new List<Bank>();

            Bank nb1 = new Bank();
            nb1.ID = 30;
            nb1.BankInfo = "Trực tiếp tại văn phòng";
            lb.Add(nb1);

            var bank = BankController.GetAll();
            if (bank.Count > 0)
            {
                foreach (var item in bank)
                {
                    Bank nb = new Bank();
                    nb.ID = item.ID;
                    nb.BankInfo = item.BankName + " - " + item.AccountHolder + " - " + item.BankNumber + " - " + item.Branch;
                    lb.Add(nb);
                }
            }

            if (lb.Count > 0)
            {
                ddlBank.DataSource = lb;
                ddlBank.DataBind();
            }
        }


        public void loaddata()
        {
            var id = Request.QueryString["i"].ToInt(0);
            string urlName = Request.UrlReferrer.ToString();
            ltr.Text = "<a href=\"" + urlName + "\" class=\"btn\">Trở về</a>";

            if (id > 0)
            {
                string username_current = Session["userLoginSystem"].ToString();
                tbl_Account ac = AccountController.GetByUsername(username_current);
                int role = ac.RoleID.ToString().ToInt();

                if (role == 0 || role == 2)
                    ddlStatus.Visible = true;
                else
                    ddlStatus.Visible = false;

                ViewState["UID"] = id;
                var a = AccountController.GetByID(id);
                if (a != null)
                {
                    rp_username.Text = a.Username;
                    rp_textarea.Text = a.Username + " đã được nạp tiền vào tài khoản.";

                }
                else
                {
                    Response.Redirect("/manager/Home.aspx");
                }
            }
        }
        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username_current = Session["userLoginSystem"].ToString();
            int role = 0;
            bool IsLoan = Convert.ToBoolean(hdfStatus.Value.ToInt(0));
            var u_loginin = AccountController.GetByUsername(username_current);
            if (u_loginin != null)
            {
                role = u_loginin.RoleID.ToString().ToInt(0);
            }
            if (!string.IsNullOrEmpty(rp_vnd.Text))
            {
                double money = Math.Round(Convert.ToDouble(rp_vnd.Text), 0);
                int UID = ViewState["UID"].ToString().ToInt(0);
                var user_wallet = AccountController.GetByID(UID);
                int status = ddlStatus.SelectedValue.ToString().ToInt(1);
                string content = rp_textarea.Text;
                DateTime currentdate = DateTime.Now;
                string BackLink = "";
                if (role == 7)
                    BackLink = "/manager/historysendwalletaccountant.aspx";
                else
                    BackLink = "/manager/HistorySendWallet.aspx";
                if (money > 0)
                {
                    if (user_wallet != null)
                    {
                        double wallet = Math.Round(Convert.ToDouble(user_wallet.Wallet), 0);
                        money = Math.Round(money, 0);
                        wallet = wallet + money;
                        wallet = Math.Round(wallet, 0);
                        if (role == 0)
                        {
                            if (status == 2)
                            {
                                string kq = AdminSendUserWalletController.Insert(user_wallet.ID, user_wallet.Username, money, status, Convert.ToInt32(ddlBank.SelectedValue), content, currentdate, username_current);
                                AdminSendUserWalletController.UpdateCongNo(kq.ToInt(0), IsLoan, false);
                                AccountController.updateWallet(user_wallet.ID, wallet, currentdate, username_current);
                                if (string.IsNullOrEmpty(content))
                                    HistoryPayWalletController.Insert(user_wallet.ID, user_wallet.Username, 0, money, user_wallet.Username + " đã được nạp tiền vào tài khoản.", wallet, 2, 4, currentdate, username_current);
                                else
                                    HistoryPayWalletController.Insert(user_wallet.ID, user_wallet.Username, 0, money, content, wallet, 2, 4, currentdate, username_current);

                                var setNoti = SendNotiEmailController.GetByID(3);
                                if (setNoti != null)
                                {
                                    if (setNoti.IsSentNotiUser == true)
                                    {
                                        NotificationsController.Inser(Convert.ToInt32(user_wallet.ID),
                                                                user_wallet.Username, 0,
                                                                "Bạn vừa được nạp " + string.Format("{0:N0}", money) + " VNĐ vào tài khoản.",
                                                                2, currentdate, u_loginin.Username, true);
                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        string datalink = "" + strUrl + "nap-tien/";
                                        PJUtils.PushNotiDesktop(user_wallet.ID, "Bạn vừa được nạp " + string.Format("{0:N0}", money) + " VNĐ vào tài khoản.", datalink);
                                    }

                                    if (setNoti.IsSendEmailUser == true)
                                    {
                                        try
                                        {
                                            PJUtils.SendMailGmail("monamedia", "uvnajxzzurygsduf", user_wallet.Email,
                                                "Thông báo tại Nhập hàng Tiết Kiệm.",
                                                "Bạn vừa được nạp " + string.Format("{0:N0}", money) + " VNĐ vào tài khoản.", "");
                                        }
                                        catch { }
                                    }
                                }
                            }
                            else
                            {
                              string kq =  AdminSendUserWalletController.Insert(user_wallet.ID, user_wallet.Username, money, status, Convert.ToInt32(ddlBank.SelectedValue), content, currentdate, username_current);
                                AdminSendUserWalletController.UpdateCongNo(kq.ToInt(0), IsLoan, false);
                            }
                        }
                        else
                        {
                            string kq = AdminSendUserWalletController.Insert(user_wallet.ID, user_wallet.Username, money, 1, Convert.ToInt32(ddlBank.SelectedValue), content, currentdate, username_current);
                            AdminSendUserWalletController.UpdateCongNo(kq.ToInt(0), IsLoan, false);
                        }
                        PJUtils.ShowMessageBoxSwAlertBackToLink("Tạo lệnh nạp tiền thành công.", "s", true, BackLink, Page);
                        //if(role == 7)
                        //    Response.Redirect("/Admin/historysendwalletaccountant.aspx");
                        //else
                        //    Response.Redirect("/Admin/HistorySendWallet.aspx");
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Vui lòng nhập số tiền lớn hơn 0.", "e", true, Page);
                }
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Chưa nhập số tiền cần rút.", "e", true, Page);
            }

        }
    }
}