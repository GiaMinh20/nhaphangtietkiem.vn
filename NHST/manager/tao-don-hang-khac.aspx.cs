using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST.manager
{
    public partial class tao_don_hang_khac : System.Web.UI.Page
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
                        if (obj_user.RoleID != 6 && obj_user.RoleID != 0 && obj_user.RoleID != 2)
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
            var khotq = WarehouseFromController.GetAllWithIsHidden(false);
            if (khotq.Count > 0)
            {
                foreach (var item in khotq)
                {
                    ListItem us = new ListItem(item.WareHouseName, item.ID.ToString());
                    ddlKhoTQ.Items.Add(us);
                }
            }
            ListItem sleTT = new ListItem("Chọn kho Trung Quốc", "0");
            ddlKhoTQ.Items.Insert(0, sleTT);

            var acc = AccountController.GetAllCustomers();
            if (acc.Count > 0)
            {
                ddlUsername.DataSource = acc;
                ddlUsername.DataBind();
            }

            var khovn = WarehouseController.GetAllWithIsHidden(false);
            if (khovn.Count > 0)
            {
                foreach (var item in khovn)
                {
                    ListItem us = new ListItem(item.WareHouseName, item.ID.ToString());
                    ddlKhoVN.Items.Add(us);
                }
            }
            ListItem sleTT1 = new ListItem("Chọn kho Việt Nam", "0");
            ddlKhoVN.Items.Insert(0, sleTT1);

            var shipping = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shipping.Count > 0)
            {
                foreach (var item in shipping)
                {
                    ListItem us = new ListItem(item.ShippingTypeName, item.ID.ToString());
                    ddlShipping.Items.Add(us);
                }
            }
            ListItem sleTT2 = new ListItem("Chọn phương thức vận chuyển", "0");
            ddlShipping.Items.Insert(0, sleTT2);
        }

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string Username = Session["userLoginSystem"].ToString();
            var Created = AccountController.GetByUsername(Username);
            int UID = Convert.ToInt32(ddlUsername.SelectedValue.ToInt());
            if (UID > 0)
            {
                var obj_user = AccountController.GetByID(UID);
                if (obj_user != null)
                {
                    double currency = Convert.ToDouble(ConfigurationController.GetByTop1().Currency);
                    if (!string.IsNullOrEmpty(obj_user.Currency.ToString()))
                    {
                        if (Convert.ToDouble(obj_user.Currency) > 0)
                        {
                            currency = Convert.ToDouble(obj_user.Currency);
                        }
                    }

                    string fullname = "";
                    string address = "";
                    string email = "";
                    string phone = "";
                    string LinkImage = "";

                    var ai = AccountInfoController.GetByUserID(UID);
                    if (ai != null)
                    {
                        fullname = ai.FirstName + " " + ai.LastName;
                        address = ai.Address;
                        email = ai.Email;
                        phone = ai.Phone;
                    }
                    int salerID = 0;
                    int dathangID = 0;
                    var sale = AccountController.GetByID(Convert.ToInt32(obj_user.SaleID));
                    if (sale != null)
                    {
                        salerID = sale.ID;
                    }
                    var dathang = AccountController.GetByID(Convert.ToInt32(obj_user.DathangID));
                    if (dathang != null)
                    {
                        dathangID = dathang.ID;
                    }
                    double UL_CKFeeBuyPro = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeBuyPro);
                    double UL_CKFeeWeight = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeWeight);
                    double LessDeposit = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).LessDeposit);

                    double priceCYN = 0;
                    double priceVND = 0;

                    string product = hdfProductList.Value;
                    string[] products = product.Split('|');

                    if (products.Length - 1 > 0)
                    {
                        for (int i = 0; i < products.Length - 1; i++)
                        {
                            string[] item = products[i].Split(']');
                            string productlink = item[0];
                            string productname = item[1];
                            string productvariable = item[2];

                            double productquantity = 0;
                            if (item[3].ToFloat(0) > 0)
                                productquantity = Convert.ToDouble(item[3]);

                            var productnote = item[4];
                            string productimage = item[5];

                            double productpriceCNY = 0;
                            if (item[6].ToFloat(0) > 0)
                            {
                                productpriceCNY = Convert.ToDouble(item[6]);
                            }
                            priceCYN += (productpriceCNY * productquantity);
                        }
                    }

                    priceCYN = Math.Round(priceCYN, 2);
                    priceVND = Math.Round(priceCYN * currency, 0);

                    double servicefee = 0;
                    double feebpnotdc = 0;
                    double percentservice = 0;
                    var adminfeebuypro = FeeBuyProController.GetAll();
                    if (adminfeebuypro.Count > 0)
                    {
                        foreach (var item in adminfeebuypro)
                        {
                            if (priceVND >= item.AmountFrom && priceVND < item.AmountTo)
                            {
                                double feepercent = 0;
                                if (item.FeePercent.ToString().ToFloat(0) > 0)
                                    feepercent = Convert.ToDouble(item.FeePercent);
                                servicefee = feepercent / 100;
                                percentservice = feepercent;
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(obj_user.FeeBuyPro))
                    {
                        if (obj_user.FeeBuyPro.ToFloat(0) > 0)
                        {
                            feebpnotdc = priceVND * Convert.ToDouble(obj_user.FeeBuyPro) / 100;
                            percentservice = Convert.ToDouble(obj_user.FeeBuyPro);
                        }
                        else
                            feebpnotdc = priceVND * servicefee;
                    }
                    else
                        feebpnotdc = priceVND * servicefee;

                    double subfeebp = feebpnotdc * UL_CKFeeBuyPro / 100;
                    double feebp = feebpnotdc - subfeebp;
                    feebp = Math.Round(feebp, 0);
                    if (feebp < 9000)
                        feebp = 9000;

                    double TotalPriceVND = priceVND + feebp;
                    string AmountDeposit = Math.Round((priceVND * LessDeposit / 100), 0).ToString();

                    string kq = MainOrderController.Insert(UID, "", "", "", false, "0", false, "0", false, "0", false, "0", false, "0",
                     priceVND.ToString(), priceCYN.ToString(), "0", feebp.ToString(), "0", "", fullname, address, email, phone, 0, "0",
                     currency.ToString(), TotalPriceVND.ToString(), salerID, dathangID, currentDate, Created.ID, AmountDeposit, 3);

                    int idkq = Convert.ToInt32(kq);
                    if (idkq > 0)
                    {
                        int z = 1;
                        for (int i = 0; i < products.Length - 1; i++)
                        {
                            string[] item = products[i].Split(']');
                            string productlink = item[0];
                            string productname = item[1];
                            string productvariable = item[2];
                            double productquantity = 0;
                            if (item[3].ToFloat(0) > 0)
                                productquantity = Convert.ToDouble(item[3]);
                            var productnote = item[4];
                            string productimage = item[5];
                            int quantity = item[3].ToInt(0);
                            double CNYPrice = 0;
                            if (item[6].ToFloat(0) > 0)
                                CNYPrice = Convert.ToDouble(item[6]);

                            double e_pricebuy = 0;
                            double e_pricevn = 0;

                            e_pricebuy = CNYPrice * quantity;
                            e_pricevn = e_pricebuy * quantity;

                            string image = productimage;
                            if (image.Contains("%2F"))
                            {
                                image = image.Replace("%2F", "/");
                            }
                            if (image.Contains("%3A"))
                            {
                                image = image.Replace("%3A", ":");
                            }
                            HttpPostedFile postedFile = Request.Files["" + productimage + ""];
                            string imageinput = "";
                            if (postedFile != null && postedFile.ContentLength > 0)
                            {                               
                                string fileContentType = postedFile.ContentType; // getting ContentType

                                byte[] tempFileBytes = new byte[postedFile.ContentLength];

                                var data = postedFile.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(postedFile.ContentLength));

                                string fileName = postedFile.FileName; // getting File Name
                                string fileExtension = Path.GetExtension(fileName).ToLower();

                                var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                                if (result)
                                {                                   
                                    imageinput = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                }
                            }
                            string imagein = "";
                            if (!string.IsNullOrEmpty(imageinput))
                            {
                                imagein = imageinput;
                            }
                            else if (!string.IsNullOrEmpty(image))
                            {
                                imagein = FileUploadCheck.ConvertBase64ToImageCustom(productimage, z);
                            }

                            LinkImage = imagein;
                            z++;

                            string ret = OrderController.Insert(UID, productname, productname, CNYPrice.ToString(), CNYPrice.ToString(), productvariable,
                            productvariable, productvariable, imagein, imagein, "", "", "", "", quantity.ToString(), "", "", "", "", "", productlink, "", "", "", "", "",
                            productnote, "", "0", "Web", "", false, false, "0", false, "0", false, "0", false, "0", false, "0", e_pricevn.ToString(), e_pricebuy.ToString(),
                            productnote, fullname, address, email, phone, 0, "0", currency.ToString(), TotalPriceVND.ToString(), idkq, DateTime.Now, UID);
                        }

                        MainOrderController.UpdateReceivePlace(idkq, UID, ddlKhoVN.SelectedValue, Convert.ToInt32(ddlShipping.SelectedValue));
                        MainOrderController.UpdateFromPlace(idkq, UID, Convert.ToInt32(ddlKhoTQ.SelectedValue), Convert.ToInt32(ddlShipping.SelectedValue));
                        MainOrderController.UpdateIsCheckNotiPrice(idkq, UID, true);
                        MainOrderController.UpdatePercentDepost(idkq, LessDeposit.ToString());
                        MainOrderController.UpdatePercentFeeBuyPro(idkq, percentservice.ToString());
                        MainOrderController.UpdateLinkImage(idkq, LinkImage);

                        var setNoti = SendNotiEmailController.GetByID(5);
                        if (setNoti != null)
                        {
                            if (setNoti.IsSentNotiAdmin == true)
                            {
                                var admins = AccountController.GetAllByRoleID(0);
                                if (admins.Count > 0)
                                {
                                    foreach (var admin in admins)
                                    {
                                        NotificationsController.Inser(admin.ID,
                                                                           admin.Username, idkq,
                                                                           "Có đơn hàng TMĐT mới ID là: " + idkq,
                                                                           1, currentDate, Username, false);
                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        string datalink = "" + strUrl + "manager/OrderDetail/" + idkq;
                                        PJUtils.PushNotiDesktop(admin.ID, "Có đơn hàng TMĐT mới ID là: " + idkq, datalink);
                                    }
                                }

                                var managers = AccountController.GetAllByRoleID(2);
                                if (managers.Count > 0)
                                {
                                    foreach (var manager in managers)
                                    {
                                        NotificationsController.Inser(manager.ID,
                                                                           manager.Username, idkq,
                                                                           "Có đơn hàng TMĐT mới ID là: " + idkq,
                                                                           1, currentDate, Username, false);
                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        string datalink = "" + strUrl + "manager/OrderDetail/" + idkq;
                                        PJUtils.PushNotiDesktop(manager.ID, "Có đơn hàng TMĐT mới ID là: " + idkq, datalink);
                                    }
                                }
                            }
                        }
                        double salepercent = 0;
                        double salepercentaf3m = 0;
                        double dathangpercent = 0;
                        var config = ConfigurationController.GetByTop1();
                        if (config != null)
                        {
                            salepercent = Convert.ToDouble(config.SalePercent);
                            salepercentaf3m = Convert.ToDouble(config.SalePercentAfter3Month);
                            dathangpercent = Convert.ToDouble(config.DathangPercent);
                        }
                        string salerName = "";
                        string dathangName = "";
                        if (salerID > 0)
                        {
                            var sales = AccountController.GetByID(salerID);
                            if (sales != null)
                            {
                                salerName = sales.Username;
                                var createdDate = Convert.ToDateTime(sale.CreatedDate);
                                int d = currentDate.Subtract(createdDate).Days;
                                if (d > 90)
                                {
                                    double per = feebp * salepercentaf3m / 100;
                                    per = Math.Round(per, 0);
                                    StaffIncomeController.Insert(idkq, "0", salepercentaf3m.ToString(), salerID, salerName, 6, 1, per.ToString(), false,
                                    currentDate, currentDate, obj_user.Username);
                                }
                                else
                                {
                                    double per = feebp * salepercent / 100;
                                    per = Math.Round(per, 0);
                                    StaffIncomeController.Insert(idkq, "0", salepercent.ToString(), salerID, salerName, 6, 1, per.ToString(), false,
                                    currentDate, currentDate, obj_user.Username);
                                }
                            }
                        }
                        if (dathangID > 0)
                        {
                            var dathangs = AccountController.GetByID(dathangID);
                            if (dathangs != null)
                            {
                                dathangName = dathangs.Username;
                                StaffIncomeController.Insert(idkq, "0", dathangpercent.ToString(), dathangID, dathangName, 3, 1, "0", false, currentDate, currentDate, obj_user.Username);
                                if (setNoti != null)
                                {
                                    if (setNoti.IsSentNotiAdmin == true)
                                    {
                                        NotificationsController.Inser(dathang.ID,
                                                               dathang.Username, idkq,
                                                               "Có đơn hàng mới ID là: " + idkq, 1,
                                                                currentDate, obj_user.Username, false);
                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        string datalink = "" + strUrl + "manager/OrderDetail/" + idkq;
                                        PJUtils.PushNotiDesktop(dathang.ID, "Có đơn hàng mới ID là: " + idkq, datalink);
                                    }
                                }
                            }
                        }
                    }
                    PJUtils.ShowMessageBoxSwAlert("Tạo đơn hàng thành công", "s", true, Page);
                }
            }

        }
    }
}