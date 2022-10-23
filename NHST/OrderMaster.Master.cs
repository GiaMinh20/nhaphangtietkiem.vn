using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class OrderMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadData();
                LoadMenu();
            }
        }
        public void LoadData()
        {
            var confi = ConfigurationController.GetByTop1();
            if (confi != null)
            {
                ltrTopLeft.Text = "<p>Tỉ giá: <span>" + string.Format("{0:N0}", Convert.ToDouble(confi.Currency)) + " VNĐ/NDT</span></p>";
                //ltrTopLeft.Text += "<p>Thời gian: <span>" + confi.TimeWork + "</span></p>";
                //ltrTopLeft.Text += "<p>Email: <span>" + confi.EmailContact + "</span></p>";

                ltrLogo.Text = "<a href=\"/\"><img src=\"" + confi.LogoIMG + "\" alt=\"\"><span>MONA&nbspLOGISTICS</span></a>";
                ltrPhone.Text = "<div class=\"text\"><p>" + confi.Hotline + "</p><span>Hỗ trợ 24/7</span></div>";
                ltrAddress.Text = "<div class=\"text\"><p>Địa chỉ</p><span>" + confi.Address + "</span></div>";

                ltrLogoFt.Text = "<a href=\"/\"><img src=\"" + confi.LogoIMG + "\" alt=\"\"></a>";

                ltrEmailFt.Text = "<div class=\"text\"><a>" + confi.EmailSupport + "</a></div>";
                ltrHotLine.Text = "<div class=\"title-hotline\"><p>Hotline: " + confi.Hotline + "</p></div>";
                ltrPhoneQuang.Text = "<div class=\"text\"><span>CSKH 1: " + confi.HotlineFeedback + "</span></div>";
                ltrPhoneNgoc.Text = "<div class=\"text\"><span>CSKH 2: " + confi.HotlineSupport + "</span></div>";

                ltrAddressHD.Text = "<span>" + confi.Address4 + "</span>";
                ltrAddressTL.Text = "<span>" + confi.Address5 + "</span>";
                ltrAddressDA.Text = "<span>" + confi.Address2 + "</span>";
                ltrAddressHCM.Text = "<span>" + confi.Address3 + "</span>";

                //login
                if (Session["userLoginSystem"] != null)
                {
                    string username = Session["userLoginSystem"].ToString();
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
                        #region phần thông báo
                        decimal levelID = Convert.ToDecimal(acc.LevelID);
                        int levelID1 = Convert.ToInt32(acc.LevelID);
                        string level = "1 Vương Miện";
                        var userLevel = UserLevelController.GetByID(levelID1);
                        if (userLevel != null)
                        {
                            level = userLevel.LevelName;
                        }
                        string userIMG = "";
                        var ai = AccountInfoController.GetByUserID(acc.ID);
                        if (ai != null)
                        {
                            if (!string.IsNullOrEmpty(ai.IMGUser))
                                userIMG = ai.IMGUser;
                        }
                        decimal countLevel = UserLevelController.GetAll("").Count();
                        decimal te = levelID / countLevel;
                        te = Math.Round(te, 2, MidpointRounding.AwayFromZero);
                        decimal tile = te * 100;
                        #endregion

                        #region New
                        ltrLogin.Text += "<div class=\"acc-info\">";
                        ltrLogin.Text += "<a class=\"acc-info-btn\" href=\"/thong-tin-nguoi-dung\"><i class=\"icon fa fa-user\"></i><span>" + username + "</span></a>";
                        ltrLogin.Text += "<div class=\"status-desktop\">";
                        ltrLogin.Text += "<div class=\"status-wrap\">";
                        ltrLogin.Text += "<div class=\"status__header\">";
                        ltrLogin.Text += "<h4>" + level + "</h4>";
                        ltrLogin.Text += "</div>";
                        ltrLogin.Text += "<div class=\"status__body\">";
                        ltrLogin.Text += "<div class=\"level\">";
                        ltrLogin.Text += "<div class=\"level__info\">";
                        ltrLogin.Text += "<p>Level</p>";
                        ltrLogin.Text += "<p class=\"rank\">" + level + "</p>";
                        ltrLogin.Text += "</div>";
                        ltrLogin.Text += "<div class=\"level__process\"><span style=\"width: " + tile + "%\"></span></div>";
                        ltrLogin.Text += "</div>";
                        ltrLogin.Text += "<div class=\"balance\">";
                        ltrLogin.Text += "<p>Số dư:</p>";
                        ltrLogin.Text += "<div class=\"balance__number\"><p class=\"vnd\">" + string.Format("{0:N0}", acc.Wallet) + " vnđ</p></div>";
                        ltrLogin.Text += "</div>";
                        if (acc.RoleID != 1)
                            ltrLogin.Text += "<div class=\"links\"><a href=\"/manager/login\">Quản trị<i class=\"fa fa-caret-right\"></i></a></div>";
                        ltrLogin.Text += "<div class=\"links\"><a href=\"/thong-tin-nguoi-dung\">Thông tin tài khoản<i class=\"fa fa-caret-right\"></i></a></div>";
                        ltrLogin.Text += "<div class=\"links\"><a href=\"/danh-sach-don-hang?t=1\">Đơn hàng của bạn<i class=\"fa fa-caret-right\"></i></a></div>";
                        ltrLogin.Text += "<div class=\"links\"><a href=\"/lich-su-giao-dich\">Lịch sử giao dịch<i class=\"fa fa-caret-right\"></i></a></div>";
                        ltrLogin.Text += "</div>";
                        ltrLogin.Text += "<div class=\"status__footer\"><a href=\"/dang-xuat\" class=\"ft-btn\">ĐĂNG XUẤT</a></div>";
                        ltrLogin.Text += "</div>";
                        ltrLogin.Text += "</div>";
                        ltrLogin.Text += "<div class=\"status-mobile\">";
                        ltrLogin.Text += "<a href=\"/thong-tin-nguoi-dung\" class=\"user-menu-logo\"><img src=\"" + userIMG + "\" alt=\"\"></a>";
                        ltrLogin.Text += "<h3 class=\"username\">" + username + "</h3>";
                        ltrLogin.Text += "<div class=\"user-info\">Số tiền: <span class=\"money\">" + string.Format("{0:N0}", acc.Wallet) + "</span> vnđ | Level <span class=\"vip\">" + level + "</span></div>";
                        ltrLogin.Text += "<div class=\"nav-percent\">";
                        ltrLogin.Text += "<div class=\"nav-percent-ok\" style=\"width: " + tile + "%\"></div>";
                        ltrLogin.Text += "</div>";
                        ltrLogin.Text += "<div class=\"profile-bottom\">";
                        ltrLogin.Text += "<ul class=\"menu-in-profile\">";
                        ltrLogin.Text += "<li><a href=\"/\"><i class=\"fa fa-home\"></i>TRANG CHỦ</a></li>";
                        ltrLogin.Text += "<li><a href=\"/theo-doi-mvd\"><i class=\"fa fa-search\"></i>TRACKING</a></li>";
                        ltrLogin.Text += "<li><a href=\"/danh-sach-don-hang?t=1\"><i class=\"fa fa-home\"></i>MUA HÀNG HỘ</a></li>";
                        ltrLogin.Text += "<li><a href=\"/lich-su-giao-dich\"><i class=\"fa fa-money\"></i>TÀI CHÍNH</a></li>";
                        ltrLogin.Text += "<li><a href=\"/khieu-nai\"><i class=\"fa fa-exclamation\"></i>KHIẾU NẠI</a></li>";
                        ltrLogin.Text += "<li><a href=\"/thong-tin-nguoi-dung\"><i class=\"fa fa-user\"></i>QUẢN LÝ TÀI KHOẢN</a></li>";
                        ltrLogin.Text += "</ul>";
                        ltrLogin.Text += "</div><a href=\"/dang-xuat\" class=\"main-btn\">Đăng xuất</a></div>";
                        ltrLogin.Text += "<div class=\"overlay-status-mobile\"></div>";
                        ltrLogin.Text += "</div>";
                        #endregion
                    }
                }
                else
                {
                    ltrLogin.Text += "<div class=\"login-logout\">";
                    ltrLogin.Text += "<div class=\"item\"><a href =\"/dang-ky\">Đăng ký</a></div>";
                    ltrLogin.Text += "<div class=\"item\"><a href =\"/dang-nhap\">Đăng nhập</a></div>";
                    ltrLogin.Text += "</div>";
                }
            }
        }

        public void LoadMenu()
        {
            string html = "";
            var categories = MenuController.GetByLevel(0);
            if (categories != null)
            {                
                foreach (var c in categories)
                {                    
                    html += " <li>";
                    if (Convert.ToBoolean(c.Target))
                        html += "<a target=\"_blank\" href=\"" + c.MenuLink + "\">" + c.MenuName + "</a>";
                    else
                        html += "<a href=\"" + c.MenuLink + "\">" + c.MenuName + "</a>";
                    html += "</li>";                 
                }
                ltrMenu.Text = html;
            }
        }

    }
}