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

namespace NHST
{
    public partial class tao_don_hang_khac1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                LoadPrefix();
                loaddata();
            }
        }

        public void LoadPrefix()
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

        public void loaddata()
        {
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                int id = obj_user.ID;
                ViewState["UID"] = id;
            }
        }

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            bool isFastDelivery = false;
            if (cbIsFastDelivery.Checked)
            {
                isFastDelivery = true;
            }
            bool isCheck = false;
            if (cbIsCheck.Checked)
            {
                isCheck = true;
            }
            bool isPackage = false;
            if (cbIsPackage.Checked)
            {
                isPackage = true;
            }
            DateTime currentDate = DateTime.Now;
            string product = hdfProductList.Value;
            int UIDCreate = 0;
            string userBuy = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(userBuy);
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

                UIDCreate = obj_user.ID;
                var ai = AccountInfoController.GetByUserID(UIDCreate);
                if (ai != null)
                {
                    fullname = ai.FirstName + " " + ai.LastName;
                    address = ai.Address;
                    email = ai.Email;
                    phone = ai.Phone;
                }

                int UID = obj_user.ID;
                int salerID = Convert.ToInt32(obj_user.SaleID);
                int dathangID = Convert.ToInt32(obj_user.DathangID);
                double UL_CKFeeBuyPro = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeBuyPro);
                double UL_CKFeeWeight = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeWeight);
                double LessDeposit = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).LessDeposit);

                double priceCYN = 0;
                double priceVND = 0;

                string[] products = product.Split('|');
                double feeKD = 0;
                int feeCheckType = 1;
                if (products.Length - 1 > 0)
                {
                    for (int i = 0; i < products.Length - 1; i++)
                    {
                        string[] item = products[i].Split(']');

                        double productpriceCNY = 0;
                        string productlink = item[0];
                        string productname = item[1];
                        if (item[2].ToFloat(0) > 0)
                        {
                            productpriceCNY = Convert.ToDouble(item[2]);
                        }
                        if (productpriceCNY >= 10)
                            feeCheckType = 1;
                        else if (productpriceCNY < 10)
                            feeCheckType = 2;

                        string productvariable = item[3];
                        double productquantity = 0;
                        if (item[4].ToFloat(0) > 0)
                            productquantity = Convert.ToDouble(item[4]);
                        var productnote = item[5];
                        string productimage = item[6];

                        priceCYN += (productpriceCNY * productquantity);
                        var feeCheckDefault = FeeCheckProductController.GetByBetweenAmountAndType(productquantity, feeCheckType);
                        if (feeCheckDefault.Price != null)
                            feeKD += Math.Round((feeCheckDefault.Price.Value * productquantity), 1);
                    }
                }

                priceVND = priceCYN * currency;

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

                double TotalPriceVND = (priceCYN * currency) + feebp;
                string AmountDeposit = (priceVND * LessDeposit / 100).ToString();

                //Tính phí kiểm đếm rồi thêm vào

                string kq = MainOrderController.Insert(UID, "", "", "", false, "0", isFastDelivery, "0", isCheck, string.Format("{0:N0}", feeKD), isPackage, "0", false, "0",
                    priceVND.ToString(), priceCYN.ToString(), "0", feebp.ToString(), "0", "", fullname, address, email, phone, 0,
                    "0", currency.ToString(), TotalPriceVND.ToString(), salerID, dathangID, currentDate, UIDCreate, AmountDeposit, 3);

                int idkq = Convert.ToInt32(kq);
                if (idkq > 0)
                {
                    int z = 1;
                    double feeCheckOrder = 0;
                    for (int i = 0; i < products.Length - 1; i++)
                    {
                        string[] item = products[i].Split(']');

                        string productlink = item[0];
                        string productname = item[1];

                        double CNYPrice = 0;
                        if (item[2].ToFloat(0) > 0)
                            CNYPrice = Convert.ToDouble(item[2]);

                        if (CNYPrice >= 10)
                            feeCheckType = 1;
                        else if (CNYPrice < 10)
                            feeCheckType = 2;

                        string productvariable = item[3];
                        double productquantity = 0;

                        if (item[4].ToFloat(0) > 0)
                            productquantity = Convert.ToDouble(item[4]);


                        var productnote = item[5];
                        string productimage = item[6];

                        int quantity = item[4].ToInt(0);

                        var feeCheckDefault = FeeCheckProductController.GetByBetweenAmountAndType(quantity, feeCheckType);
                        if (feeCheckDefault.Price != null)
                            feeCheckOrder = Math.Round((feeCheckDefault.Price.Value * quantity), 1);

                        double e_pricebuy = CNYPrice * productquantity;
                        double e_pricevn = e_pricebuy * currency;

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
                                if (postedFile.FileName.ToLower().Contains(".jpg") || postedFile.FileName.ToLower().Contains(".png") || postedFile.FileName.ToLower().Contains(".jpeg"))
                                {
                                    if (postedFile.ContentType == "image/png" || postedFile.ContentType == "image/jpeg" || postedFile.ContentType == "image/jpg")
                                    {
                                        imageinput = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                    }
                                }
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
                        productnote, "", "0", "Web", "", false, false, "0", isFastDelivery, "0", isCheck, string.Format("{0:N0}", feeCheckOrder), isPackage, "0", false, "0", e_pricevn.ToString(), e_pricebuy.ToString(),
                        productnote, fullname, address, email, phone, 0, "0", currency.ToString(), TotalPriceVND.ToString(), idkq, DateTime.Now, UIDCreate);
                    }

                    MainOrderController.UpdateFromPlace(idkq, UID, 1, 1);
                    MainOrderController.UpdateReceivePlace(idkq, UID, "1", 1);
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
                                                                       1, currentDate, userBuy, false);
                                    //string strPathAndQuery = Request.Url.PathAndQuery;
                                    //string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                    //string datalink = "" + strUrl + "manager/OrderDetail/" + idkq;
                                    //PJUtils.PushNotiDesktop(admin.ID, "Có đơn hàng TMĐT mới ID là: " + idkq, datalink);
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
                                                                       1, currentDate, userBuy, false);
                                    //string strPathAndQuery = Request.Url.PathAndQuery;
                                    //string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                    //string datalink = "" + strUrl + "manager/OrderDetail/" + idkq;
                                    //PJUtils.PushNotiDesktop(manager.ID, "Có đơn hàng TMĐT mới ID là: " + idkq, datalink);
                                }
                            }
                        }
                    }

                    double salepercent = 0;
                    double salepercentaf3m = 0;
                    double dathangpercent = 0;
                    var confignew = ConfigurationController.GetByTop1();
                    if (confignew != null)
                    {
                        salepercent = Convert.ToDouble(confignew.SalePercent);
                        salepercentaf3m = Convert.ToDouble(confignew.SalePercentAfter3Month);
                        dathangpercent = Convert.ToDouble(confignew.DathangPercent);
                    }

                    string salerName = "";
                    string dathangName = "";
                    if (salerID > 0)
                    {
                        var sale = AccountController.GetByID(salerID);
                        if (sale != null)
                        {
                            salerName = sale.Username;
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

                            NotificationsController.Inser(sale.ID, sale.Username, idkq, "Có đơn hàng TMĐT mới ID là: " + idkq, 1, currentDate, userBuy, false);
                        }
                    }
                    if (dathangID > 0)
                    {
                        var dathang = AccountController.GetByID(dathangID);
                        if (dathang != null)
                        {
                            dathangName = dathang.Username;
                            StaffIncomeController.Insert(idkq, "0", dathangpercent.ToString(), dathangID, dathangName, 3, 1, "0", false, currentDate, currentDate, obj_user.Username);
                            NotificationsController.Inser(dathang.ID, dathang.Username, idkq, "Có đơn hàng TMĐT mới ID là: " + idkq, 1, currentDate, obj_user.Username, false);
                        }
                    }
                }
                PJUtils.ShowMessageBoxSwAlert("Tạo đơn hàng thành công", "s", true, Page);
            }
        }

    }
}