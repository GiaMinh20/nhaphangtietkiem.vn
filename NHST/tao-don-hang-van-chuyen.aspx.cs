using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class tao_don_hang_van_chuyen1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] != null)
                {
                    LoadDDL();
                    loaddata();
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
            }
        }

        public void LoadDDL()
        {
            var warehousefrom = WarehouseFromController.GetAllWithIsHidden(false);
            if (warehousefrom.Count > 0)
            {
                ddlWarehouseFrom.DataSource = warehousefrom;
                ddlWarehouseFrom.DataBind();
            }
            var warehouse = WarehouseController.GetAllWithIsHidden(false);
            if (warehouse.Count > 0)
            {
                ddlReceivePlace.DataSource = warehouse;
                ddlReceivePlace.DataBind();
            }

            var shippingtype = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shippingtype.Count > 0)
            {
                ddlShippingType.DataSource = shippingtype;
                ddlShippingType.DataBind();
            }
        }

        public void loaddata()
        {
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                int id = obj_user.ID;
                ViewState["UID"] = id;
                lblUsername.Text = username;
            }
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
            double currency = 0;
            var config = ConfigurationController.GetByTop1();
            if (config != null)
            {
                currency = Convert.ToDouble(config.Currency);
            }
            DateTime currentDate = DateTime.Now;
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
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
                            string note = item[7].ToString();
                            //double codeTQ = 0;
                            //if (item[7].ToFloat(0) > 0)
                            //    codeTQ = Math.Round(Convert.ToDouble(item[7]), 2);

                            string tID = TransportationOrderController.InsertNew(obj_user.ID, username, ddlWarehouseFrom.SelectedValue.ToInt(1),
                            ddlReceivePlace.SelectedValue.ToInt(1), ddlShippingType.SelectedValue.ToInt(1), 1, weight, currency, 0, 0, 0, 0, 0, 0, txtNote.Text, currentDate, username);

                            if (tID.ToInt(0) > 0)
                            {
                                TransportationOrderDetailController.InsertNew(tID.ToInt(0), code, weight, packageType, isCheckProduct, isPackage, isInsurrance, "0", "0", note, quantity.ToString(), currentDate, username);
                                TransportationOrderController.UpdatSale(tID.ToInt(0), SaleID, SaleName);
                            }
                        }
                        PJUtils.ShowMessageBoxSwAlert("Tạo đơn hàng thành công", "s", true, Page);
                    }
                }
                else
                    PJUtils.ShowMessageBoxSwAlert("Không tồn tại kiện trong đơn, vui lòng thêm kiện hàng.", "e", true, Page);
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Không tồn tại kiện trong đơn, vui lòng thêm kiện hàng.", "e", true, Page);
            }
        }
    }
}
