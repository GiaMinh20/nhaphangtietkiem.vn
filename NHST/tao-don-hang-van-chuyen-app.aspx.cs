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
    public partial class tao_don_hang_van_chuyen_app : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDDL();
                LoadData();
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
                        var getsmallcheck = SmallPackageController.GetByOrderCode(code);
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

        public void LoadData()
        {
            string Key = Request.QueryString["Key"];
            int UID = Request.QueryString["UID"].ToInt(0);
            if (UID > 0)
            {
                var tk = DeviceTokenController.GetByToken(UID, Key);
                if (tk != null)
                {
                    var ac = AccountController.GetByID(UID);
                    if (ac != null)
                    {
                        ViewState["UID"] = UID;
                        ViewState["Key"] = Key;
                        pnMobile.Visible = true;
                        lbUsername.Text = ac.Username;
                    }
                }
                else
                {
                    pnShowNoti.Visible = true;
                }
            }
            else
            {
                pnShowNoti.Visible = true;
            }
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
            int UID = ViewState["UID"].ToString().ToInt(0);
            var obj_user = AccountController.GetByID(UID);
            if (obj_user != null)
            {
                string listPackage = hdfProductList.Value;
                if (!string.IsNullOrEmpty(listPackage))
                {
                    double totalWeight = 0;
                    string[] list = listPackage.Split('|');
                    if (list.Length - 1 > 0)
                    {
                        for (int i = 0; i < list.Length - 1; i++)
                        {
                            string items = list[i];
                            string[] item = items.Split(']');
                            string code = item[0].ToString().Trim();
                            double weight = Convert.ToDouble(item[1].ToString());
                            totalWeight += weight;

                            string tID = TransportationOrderController.InsertNew(obj_user.ID, obj_user.Username, ddlWarehouseFrom.SelectedValue.ToInt(1), ddlReceivePlace.SelectedValue.ToInt(1),
                            ddlShippingType.SelectedValue.ToInt(1), 1, weight, currency, 0, 0, 0, 0, 0, 0, txtNote.Text, currentDate, obj_user.Username);
                            if (tID.ToInt(0) > 0)
                            {
                                TransportationOrderDetailController.InsertNew(tID.ToInt(0), code, weight, "", false, false, false, "0", "0", "", "1", currentDate, obj_user.Username);

                                var setNoti = SendNotiEmailController.GetByID(13);
                                if (setNoti.IsSentNotiAdmin == true)
                                {
                                    var admins = AccountController.GetAllByRoleID(0);
                                    if (admins.Count > 0)
                                    {
                                        foreach (var admin in admins)
                                        {
                                            NotificationsController.Inser(admin.ID,
                                                                               admin.Username, tID.ToInt(),
                                                                                "Có đơn hàng vận chuyển hộ mới ID là: " + tID + ".",
                                                                               10, currentDate, obj_user.Username, false);
                                        }
                                    }
                                    var managers = AccountController.GetAllByRoleID(0);
                                    if (managers.Count > 0)
                                    {
                                        foreach (var manager in managers)
                                        {
                                            NotificationsController.Inser(manager.ID,
                                                                               manager.Username, tID.ToInt(),
                                                                               "Có đơn hàng vận chuyển hộ mới ID là: " + tID + ".",
                                                                               10, currentDate, obj_user.Username, false);
                                        }
                                    }
                                }
                            }
                        }
                        PJUtils.ShowMessageBoxSwAlert("Tạo đơn hàng thành công", "s", true, Page);
                    }
                   
                }
                else
                {
                    //string kq = TransportationOrderController.Insert(obj_user.ID, obj_user.Username, ddlWarehouseFrom.SelectedValue.ToInt(1),
                    //ddlReceivePlace.SelectedValue.ToInt(1), ddlShippingType.SelectedValue.ToInt(1), 1, 0, currency, 0, txtNote.Text, currentDate, obj_user.Username);
                    PJUtils.ShowMessageBoxSwAlert("Không tồn tại kiện trong đơn, vui lòng thêm kiện hàng", "i", true, Page);
                }
            }
        }
    }
}