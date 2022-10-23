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

namespace NHST.manager
{
    public partial class Add_Transport_Order : System.Web.UI.Page
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
                //loaddata();
            }
        }

        public void loadPrefix()
        {
            var acc = AccountController.GetAllCustomers();
            if (acc.Count > 0)
            {
                ddlUsername.DataSource = acc;
                ddlUsername.DataBind();
            }
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

        [WebMethod]
        public static string checkbefore(string listStr)
        {
            string returns = "";
            if (!string.IsNullOrEmpty(listStr))
            {
                string checkCode = "";
                string[] list = listStr.Split('|');
                bool checkConflitCode = false;
                if (list.Length - 1 > 0)
                {
                    for (int i = 0; i < list.Length - 1; i++)
                    {
                        string items = list[i];
                        string[] item = items.Split(']');
                        string code = item[0].Replace(" ", String.Empty);
                        var getsmallcheck = SmallPackageController.GetByOrderCodeUser(code);
                        if (getsmallcheck.Count > 0)
                        {
                            checkConflitCode = true;
                            returns += code + "; ";
                        }
                        var detail = TransportationOrderDetailController.GetAllByBarcode(code);
                        if (detail.Count > 0)
                        {
                            checkConflitCode = true;
                            returns += code + "; ";
                        }
                        if (i > 0 && checkCode == code)
                        {
                            checkConflitCode = true;
                            returns += code + "; ";
                        }
                        checkCode = item[0].Replace(" ", String.Empty);
                    }
                }
                if (checkConflitCode == true)
                {
                    return returns;
                }
                else
                {
                    return "ok";
                }
            }
            return "ok";
        }

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            string Username = Session["userLoginSystem"].ToString();
            int UID = Convert.ToInt32(ddlUsername.SelectedValue.ToInt());
            if (UID > 0)
            {
                var obj_user = AccountController.GetByID(UID);
                if (obj_user != null)
                {
                    DateTime currentDate = DateTime.Now;

                    double currency = 0;
                    var config = ConfigurationController.GetByTop1();
                    if (config != null)
                        currency = Convert.ToDouble(config.Currency);

                    string SaleName = "";
                    int SaleID = Convert.ToInt32(obj_user.SaleID);
                    if (SaleID > 0)
                    {
                        var sale = AccountController.GetByID(SaleID);
                        if (sale != null)
                        {
                            SaleName = sale.Username;
                        }
                    }   
                    string listPackage = hdfProductList.Value;
                    if (!string.IsNullOrEmpty(listPackage))
                    {
                        double totalCodTQCYN = 0;
                        double totalCodTQVND = 0;
                        double totalPriceVND = 0;
                        string[] list = listPackage.Split('|');
                        if (list.Length - 1 > 0)
                        {
                            for (int z = 0; z < list.Length - 1; z++)
                            {
                                string items = list[z];
                                string[] item = items.Split(']');
                                string code = item[0].ToString().Trim();
                                string packageType = item[1].ToString();
                                string quantity = item[2];
                                double weight = Math.Round(Convert.ToDouble(item[3].ToString()), 1);

                                bool isCheckProduct = false;
                                if (item[4].ToInt(0) == 1)
                                {
                                    isCheckProduct = true;
                                }

                                bool isPackage = false;
                                if (item[5].ToInt(0) == 1)
                                {
                                    isPackage = true;
                                }

                                bool isInsurrance = false;
                                if (item[6].ToInt(0) == 1)
                                {
                                    isInsurrance = true;
                                }

                                double codeTQ = 0;
                                if (item[7].ToFloat(0) > 0)
                                    codeTQ = Math.Round(Convert.ToDouble(item[7]), 2);
                                totalCodTQVND = codeTQ * currency;
                                totalPriceVND = Math.Round(totalCodTQVND, 0);
                                string note = item[8].ToString();

                                string tID = TransportationOrderController.InsertNew(obj_user.ID, obj_user.Username, ddlKhoTQ.SelectedValue.ToInt(1), ddlKhoVN.SelectedValue.ToInt(1),
                                ddlShipping.SelectedValue.ToInt(1), 1, weight, currency, 0, 0, 0, totalCodTQCYN, totalCodTQVND, totalPriceVND, "", currentDate, Username);

                                if (tID.ToInt(0) > 0)
                                {
                                    TransportationOrderDetailController.InsertNew(tID.ToInt(0), code, weight, packageType, isCheckProduct, isPackage, isInsurrance,
                                                                     codeTQ.ToString(), totalCodTQVND.ToString(), note, quantity.ToString(), currentDate, Username);
                                    TransportationOrderController.UpdatSale(tID.ToInt(0), SaleID, SaleName);
                                }
                            }
                            PJUtils.ShowMessageBoxSwAlert("Tạo đơn hàng thành công", "s", true, Page);
                        }
                    }
                }
            }
        }

    }
}