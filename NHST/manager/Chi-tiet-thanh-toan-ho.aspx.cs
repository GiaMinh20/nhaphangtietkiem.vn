using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHST.Bussiness;
using NHST.Controllers;
using Telerik.Web.UI;
using MB.Extensions;
using NHST.Models;
using NHST.Controllers;
using System.Text;
using System.IO;

namespace NHST.manager
{
    public partial class Chi_tiet_thanh_toan_ho : System.Web.UI.Page
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
                    string page1 = Request.Url.ToString();
                    string page2 = Request.UrlReferrer.ToString();
                    if (page1 != page2)
                        Session["PrePage"] = page2;

                    string username_current = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(username_current);
                    if (ac.RoleID == 1)
                        Response.Redirect("/trang-chu");
                }

                LoadData();

            }
        }

        public void LoadData()
        {
            string urlName = Request.UrlReferrer.ToString();
            ltrBack.Text = "<a href=\"" + urlName + "\" class=\"btn mt-2\">Trở về</a>";
            var id = Request.QueryString["ID"].ToInt(0);
            if (id > 0)
            {
                var re = PayhelpController.GetByID(id);
                if (re != null)
                {
                    ViewState["ID"] = id;
                    lbID.Text = re.Username + "_" + id.ToString();
                    txtUserName.Text = re.Username;
                    txtCurrency.Text = Convert.ToDouble(re.CurrencyGiagoc).ToString();
                    txtTotalPriceCNY.Text = Convert.ToDouble(re.TotalPrice).ToString();
                    lbTongTien.Text = string.Format("{0:N0} VNĐ", Convert.ToDouble(re.TotalPriceVND));
                    ltrStatus.Text = PJUtils.ReturnStatusPayHelpNew(re.Status.ToString().ToInt(0));
                    txtTotalPriceVND.Text = string.Format("{0:N0}", Convert.ToDouble(re.TotalPriceVND));
                    txtSummary.Text = re.Note;
                    txtPhoneNumber.Text = re.Phone;
                    txtMaDonHang.Text = re.MaDonHang;
                    txtCanNang.Text = re.CanNang;
                    txtPercent.Text = re.PercentPay;
                    txtFeeBuyPro.Text = string.Format("{0:N0}", Convert.ToDouble(re.FeeBuyPro));
                    ddlStatusDetail.SelectedValue = re.Status.ToString();
                    txtPriceVND.Text = Convert.ToDouble(re.Currency).ToString();
                    txtFinalPriceCNY.Text = Convert.ToDouble(re.FinalPayPrice).ToString();
                    txtFinalPriceVND.Text = string.Format("{0:N0}", Convert.ToDouble(re.FinalPayPriceVND));

                    #region Hình ảnh
                    if (!string.IsNullOrEmpty(re.ImagePay))
                    {
                        var b = re.ImagePay.Split('|').ToList();
                        for (var i = 0; i < b.Count - 1; i++)
                        {
                            ltrImagePay.Text += "<div class=\"img-block\"><a href=\"" + b[i] + "\" data-fancybox ><img src=\"" + b[i] + "\" alt=\"image\" style=\"width: 50%;\"></a></div>";
                        }
                    }
                    else
                    {
                        ltrImagePay.Text += "<div></div>";
                    }
                    #endregion

                    var pd = PayhelpDetailController.GetByPayhelpID(id);
                    StringBuilder html = new StringBuilder();
                    if (pd.Count > 0)
                    {
                        foreach (var item in pd)
                        {
                            html.Append("<div class=\"row order-wrap itemyeuau\" data-id=\"" + item.ID + "\">");
                            html.Append("<div class=\"input-field col s12 m6\">");
                            html.Append("<input type=\"number\" class=\"txtDesc2\" min=\"0\" disabled value=\"" + item.Desc2 + "\">");
                            html.Append("<label>Giá tiền</label>");
                            html.Append("</div>");
                            html.Append("<div class=\"input-field col s12 m6\">");
                            html.Append("<input type=\"text\" class=\"txtDesc1\" disabled value=\"" + item.Desc1 + "\">");
                            html.Append("<label>Nội dung</label>");
                            html.Append("</div>");
                            html.Append("</div>");
                        }
                    }
                    ltrList.Text = html.ToString();
                }
            }
        }

        protected void btnPayment_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0 || obj_user.RoleID == 7)
                {
                    int OID = ViewState["ID"].ToString().ToInt(0);
                    var Pay = PayhelpController.GetByID(OID);
                    if (Pay != null)
                    {
                        double TotalPriceVND = Convert.ToDouble(Pay.TotalPriceVND);
                        double WalletUser = 0;
                        var user = AccountController.GetByID(Convert.ToInt32(Pay.UID));
                        if (user != null)
                        {
                            WalletUser = Convert.ToDouble(user.Wallet);
                            if (Pay.Status == 2)
                            {
                                if (WalletUser >= TotalPriceVND)
                                {
                                    double WalletLeft = WalletUser - TotalPriceVND;
                                    AccountController.updateWallet(user.ID, WalletLeft, currentDate, username_current);
                                    HistoryPayWalletController.Insert(user.ID, user.Username, 0, TotalPriceVND,
                                    user.Username + " đã trả tiền thanh toán tiền hộ đơn hàng " + OID, WalletLeft, 1, 9, currentDate, obj_user.Username);
                                    PayhelpController.UpdateStatus(OID, 3, currentDate, obj_user.Username);

                                    PJUtils.ShowMessageBoxSwAlert("Thanh toán thành công.", "s", true, Page);
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlert("Tài khoản của khách hàng không đủ tiền để thanh toán đơn này. Vui lòng nạp thêm tiền vào ví của khách hàng.", "i", true, Page);
                                }
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Đơn hàng chưa xác nhận nên không thẻ thanh toán. Vui lòng kiểm tra lại.", "i", true, Page);
                            }
                        }
                    }
                }    
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Bạn không đủ quyền thực hiện chức năng này.", "i", true, Page);
                }    
            }    
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username_current = Session["userLoginSystem"].ToString();
            var ac = AccountController.GetByUsername(username_current);
            int ID = ViewState["ID"].ToString().ToInt(0);
            var p = PayhelpController.GetByID(ID);
            if (p != null)
            {
                int stt_current = Convert.ToInt32(p.Status);
                int status = ddlStatusDetail.SelectedValue.ToInt(0);

                string link = "";
                string value = hdfListIMG.Value;
                string[] listIMG = value.Split('|');
                for (int i = 0; i < listIMG.Length - 1; i++)
                {
                    string imageData = listIMG[i];
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/KhieuNaiIMG/");
                    string date = DateTime.Now.ToString("dd-MM-yyyy");
                    string time = DateTime.Now.ToString("hh:mm tt");
                    Page page = (Page)HttpContext.Current.Handler;

                    string k = i.ToString();
                    string fileNameWitPath = path + k + "-" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "") + ".png";
                    string linkIMG = "/Uploads/KhieuNaiIMG/" + k + "-" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "") + ".png";

                    byte[] data;
                    string convert;
                    string contenttype;

                    using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            if (imageData.Contains("data:image/png"))
                            {
                                convert = imageData.Replace("data:image/png;base64,", String.Empty);
                                data = Convert.FromBase64String(convert);
                                contenttype = ".png";
                                var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                if (result)
                                {
                                    bw.Write(data);
                                    link += linkIMG + "|";
                                }
                            }
                            else if (imageData.Contains("data:image/jpeg"))
                            {
                                convert = imageData.Replace("data:image/jpeg;base64,", String.Empty);
                                data = Convert.FromBase64String(convert);
                                contenttype = ".jpeg";
                                var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                if (result)
                                {
                                    bw.Write(data);
                                    link += linkIMG + "|";
                                }
                            }
                            else if (imageData.Contains("data:image/gif"))
                            {
                                convert = imageData.Replace("data:image/gif;base64,", String.Empty);
                                data = Convert.FromBase64String(convert);
                                contenttype = ".gif";
                                var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                if (result)
                                {
                                    bw.Write(data);
                                    link += linkIMG + "|";
                                }
                            }
                            else if (imageData.Contains("data:image/jpg"))
                            {
                                convert = imageData.Replace("data:image/jpg;base64,", String.Empty);
                                data = Convert.FromBase64String(convert);
                                contenttype = ".jpg";
                                var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                if (result)
                                {
                                    bw.Write(data);
                                    link += linkIMG + "|";
                                }
                            }
                        }
                    }
                }

                string MaDonHang = txtMaDonHang.Text;
                string CanNang = txtCanNang.Text;
                double FeeBuyPro = 0;
                double TotalPriceCYN = 0;
                double TotalPriceVND = 0;

                double FeeFinalPrice = Convert.ToDouble(p.FinalPayPrice);
                double FeeFinalPriceVND = 0;
                double Percent = Convert.ToDouble(txtPercent.Text);               
                double Currency = Convert.ToDouble(txtPriceVND.Text);
                double CurrencyReal = Convert.ToDouble(txtCurrency.Text);

                FeeFinalPriceVND = FeeFinalPrice * Currency;

                if (Percent > 0)
                {
                    FeeBuyPro = FeeFinalPriceVND * Percent / 100;
                }

                TotalPriceVND = FeeFinalPriceVND + FeeBuyPro;
                TotalPriceCYN = TotalPriceVND / Currency;

                PayhelpController.UpdatePanda(ID, link, MaDonHang, CanNang, currentDate, ac.Username, Percent.ToString(), 
                FeeBuyPro.ToString(), TotalPriceVND.ToString(), TotalPriceCYN.ToString(), status, CurrencyReal.ToString(), Currency.ToString(), FeeFinalPriceVND.ToString());

                PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công", "s", true, Page);
            }
        }        

        protected void btnBack_Click(object sender, EventArgs e)
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