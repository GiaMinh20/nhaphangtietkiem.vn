using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class tao_don_thanh_toan_ho : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                if (Session["userLoginSystem"] != null)
                {
                    LoadData();
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
            }
        }

        public void LoadData()
        {
            string username = Session["userLoginSystem"].ToString();
            var u = AccountController.GetByUsername(username);
            if (u != null)
            {
                txtUsername.Text = username;
                
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            var u = AccountController.GetByUsername(username);
            if (u != null)
            {
                int level = Convert.ToInt32(u.LevelID);
                int SaleID = Convert.ToInt32(u.SaleID);
                string SalesName = "";
                if (SaleID > 0)
                {
                    var sales = AccountController.GetByID(SaleID);
                    if (sales != null)
                    {
                        SalesName = sales.Username;
                    }    
                }    
                int UID = u.ID;
                double pc_config = 0;
                double currencygiagoc = 0;
                var config = ConfigurationController.GetByTop1();
                if (config != null)
                {
                    pc_config = Convert.ToDouble(config.PricePayHelpDefault);
                    currencygiagoc = Convert.ToDouble(config.PricePayHelpDefault);
                }

                double amount = 0;
                if (!string.IsNullOrEmpty(hdfAmount.Value))
                    amount = Convert.ToDouble(hdfAmount.Value);

                if (amount > 0)
                {
                    double totalpriceVNDGiagoc = currencygiagoc * amount;

                    string note = txtNote.Text;
                    string list = hdflist.Value;
                    var pricechange = PriceChangeController.GetByPriceFT(amount);
                    double pc = 0;
                    if (pricechange != null)
                    {
                        if (level == 1)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip0);
                        }
                        else if (level == 2)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip1);
                        }
                        else if (level == 3)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip2);
                        }
                        else if (level == 4)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip3);
                        }
                        else if (level == 5)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip4);
                        }
                        else if (level == 6)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip5);
                        }
                        else if (level == 7)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip6);
                        }
                        else if (level == 8)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip7);
                        }
                        else if (level == 9)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip8);
                        }
                        else
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip8);
                        }                        
                    }

                    double totalpricevnd = pc * amount;
                    string kq = PayhelpController.Insert(UID, username, note, amount.ToString(), totalpricevnd.ToString(), pc.ToString(),
                    currencygiagoc.ToString(), totalpriceVNDGiagoc.ToString(), 0, txtPhone.Text, currentDate, username);

                    int pID = kq.ToInt(0);
                    if (pID > 0)
                    {
                        PayhelpController.UpdateSales(pID, SaleID, SalesName);

                        string[] items = list.Split('|');
                        for (int i = 0; i < items.Length - 1; i++)
                        {
                            string[] item = items[i].Split(',');
                            string des1 = item[0];
                            string des2 = item[1];
                            if (!string.IsNullOrEmpty(des1) || !string.IsNullOrEmpty(des2))
                            {
                                PayhelpDetailController.Insert(pID, des1, des2, currentDate, username);
                            }
                        }

                        var setNoti = SendNotiEmailController.GetByID(18);
                        if (setNoti != null)
                        {
                            if (setNoti.IsSentNotiAdmin == true)
                            {
                                var admins = AccountController.GetAllByRoleID(0);
                                if (admins.Count > 0)
                                {
                                    foreach (var admin in admins)
                                    {
                                        NotificationsController.Inser(admin.ID, admin.Username, pID, "Có đơn thanh toán hộ mới ID là: " + pID + "",
                                        8, currentDate, username, false);
                                    }
                                }
                            }

                            if (setNoti.IsSentEmailAdmin == true)
                            {
                                var admins = AccountController.GetAllByRoleID(0);
                                if (admins.Count > 0)
                                {
                                    foreach (var admin in admins)
                                    {

                                        try
                                        {
                                            PJUtils.SendMailGmail("pandaorder.com@gmail.com", "xxx", admin.Email,
                                                "Thông báo tại Panda Order.", "Có đơn thanh toán hộ mới ID là: " + pID, "");
                                        }
                                        catch { }

                                    }
                                }
                            }

                        }
                    }
                    PJUtils.ShowMessageBoxSwAlert("Gửi yêu cầu thành công", "s", true, Page);
                }
                else
                    PJUtils.ShowMessageBoxSwAlert("Vui lòng nhập số tiền", "e", false, Page);
            }
            else
                PJUtils.ShowMessageBoxSwAlert("Không tìm thấy user", "e", false, Page);
        }

        [WebMethod]
        public static string getCurrency(string totalprice)
        {
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var u = AccountController.GetByUsername(username);
            if (u != null)
            {
                int level = Convert.ToInt32(u.LevelID);
                double pc_config = 0;
                var config = ConfigurationController.GetByTop1();
                if (config != null)
                {
                    pc_config = Convert.ToDouble(config.PricePayHelpDefault);
                }

                double amount = Convert.ToDouble(totalprice);
                if (amount > 0)
                {
                    var pricechange = PriceChangeController.GetByPriceFT(amount);
                    double pc = 0;
                    if (pricechange != null)
                    {
                        if (level == 1)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip0);
                        }
                        else if (level == 2)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip1);
                        }
                        else if (level == 3)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip2);
                        }
                        else if (level == 4)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip3);
                        }
                        else if (level == 5)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip4);
                        }
                        else if (level == 6)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip5);
                        }
                        else if (level == 7)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip6);
                        }
                        else if (level == 8)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip7);
                        }
                        else if (level == 9)
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip8);
                        }
                        else
                        {
                            pc = pc_config + Convert.ToDouble(pricechange.Vip8);
                        }
                        return pc.ToString();
                    }
                }
            }
            return "0";
        }
    }
}