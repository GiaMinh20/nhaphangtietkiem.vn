using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace NHST.manager
{
    public partial class ConfigWarehouse : System.Web.UI.Page
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
                    if (ac.RoleID != 0)
                        Response.Redirect("/trang-chu");
                }
                LoadKhoChina();
                LoadKhoVietNam();
                LoadHinhThuc();
            }
        }

        public void LoadKhoChina()
        {
            var khochina = WarehouseFromController.GetAll("");
            StringBuilder hcm = new StringBuilder();
            if (khochina.Count > 0)
            {
                for (int i = 0; i < khochina.Count; i++)
                {
                    var item = khochina[i];
                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.WareHouseName + "</td>");
                    //hcm.Append("<td>" + item.Address + "</td>");
                    hcm.Append("<td>" + PJUtils.BoolToStatusShow(item.IsHidden.ToString()) + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"action-table\">");
                    hcm.Append("<a href=\"#modalEditKhoChina\" id=\"EditKhoChina-" + item.ID + "\" onclick=\"EditKhoChina(" + item.ID + ")\" class=\" modal-trigger\" data-position=\"top\"><i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                    hcm.Append("</div>");
                    hcm.Append("</td>");
                    hcm.Append("</tr>");
                }
            }
            ltrListKhoChina.Text = hcm.ToString();
        }

        [WebMethod]
        public static string LoadInforChina(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var p = WarehouseFromController.GetByID(ID.ToInt(0));
            if (p != null)
            {
                tbl_WarehouseFrom l = new tbl_WarehouseFrom();
                l.ID = p.ID;
                l.WareHouseName = p.WareHouseName;
                l.IsHidden = p.IsHidden;
                l.CreatedDate = p.CreatedDate;
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }

        public void LoadKhoVietNam()
        {
            var khovn = WarehouseController.GetAll("");
            StringBuilder hcm = new StringBuilder();
            if (khovn.Count > 0)
            {
                for (int i = 0; i < khovn.Count; i++)
                {
                    var item = khovn[i];
                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.WareHouseName + "</td>");
                    //hcm.Append("<td>" + item.Address + "</td>");
                    hcm.Append("<td>" + PJUtils.BoolToStatusShow(item.IsHidden.ToString()) + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"action-table\">");
                    hcm.Append("<a href=\"#modalEditKhoVietNam\" id=\"EditKhoVN-" + item.ID + "\" onclick=\"EditKhoVN(" + item.ID + ")\" class=\" modal-trigger\" data-position=\"top\"><i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                    hcm.Append("</div>");
                    hcm.Append("</td>");
                    hcm.Append("</tr>");
                }
            }
            ltrListKhoVietNam.Text = hcm.ToString();
        }

        [WebMethod]
        public static string LoadInforVN(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var p = WarehouseController.GetByID(ID.ToInt(0));
            if (p != null)
            {
                tbl_Warehouse l = new tbl_Warehouse();
                l.ID = p.ID;
                l.WareHouseName = p.WareHouseName;
                l.IsHidden = p.IsHidden;
                l.CreatedDate = p.CreatedDate;
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }

        public void LoadHinhThuc()
        {
            var hinhthuc = ShippingTypeToWareHouseController.GetAll("");
            StringBuilder hcm = new StringBuilder();
            if (hinhthuc.Count > 0)
            {
                for (int i = 0; i < hinhthuc.Count; i++)
                {
                    var item = hinhthuc[i];
                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.ShippingTypeName + "</td>");
                    hcm.Append("<td>" + PJUtils.BoolToStatusShow(item.IsHidden.ToString()) + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"action-table\">");
                    hcm.Append("<a href=\"#modalEditShippingType\" id=\"EditShippingType-" + item.ID + "\" onclick=\"EditShippingType(" + item.ID + ")\" class=\" modal-trigger\" data-position=\"top\"><i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                    hcm.Append("</div>");
                    hcm.Append("</td>");
                    hcm.Append("</tr>");
                }
            }
            ltrListHinhThuc.Text = hcm.ToString();
        }

        [WebMethod]
        public static string LoadInforShippingType(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var p = ShippingTypeToWareHouseController.GetByID(ID.ToInt(0));
            if (p != null)
            {
                tbl_ShippingTypeToWareHouse l = new tbl_ShippingTypeToWareHouse();
                l.ID = p.ID;
                l.ShippingTypeName = p.ShippingTypeName;
                l.IsHidden = p.IsHidden;
                l.CreatedDate = p.CreatedDate;
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }

        protected void btnCreateKhoChina_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string Username = Session["userLoginSystem"].ToString();
            string NameKhoChina = txtNameKhoChina.Text;
            string kq = WarehouseFromController.Insert(NameKhoChina, 0, "", "", "", "", "", false, currentDate, Username);
            if (kq != null)
            {
                PJUtils.ShowMessageBoxSwAlert("Thêm kho Trung Quốc thành công.", "s", true, Page);
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình cập nhật trang. Vui lòng thử lại.", "e", true, Page);
            }
        }

        protected void btnCreateKhoVietNam_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string Username = Session["userLoginSystem"].ToString();
            string NameKhoViet = txtNameKhoVietNam.Text;
            string kq = WarehouseController.Insert(NameKhoViet, 0, "", "", "", "", "", false, currentDate, Username);
            if (kq != null)
            {
                PJUtils.ShowMessageBoxSwAlert("Thêm kho Việt Nam thành công.", "s", true, Page);
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình cập nhật trang. Vui lòng thử lại.", "e", true, Page);
            }
        }

        protected void btnCreateShippingType_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string Username = Session["userLoginSystem"].ToString();
            string NameShippingType = txtNameShippingType.Text;
            string kq = ShippingTypeToWareHouseController.Insert(NameShippingType, "", false, currentDate, Username);
            if (kq != null)
            {
                PJUtils.ShowMessageBoxSwAlert("Thêm kho hình thức vận chuyển thành công.", "s", true, Page);
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình cập nhật trang. Vui lòng thử lại.", "e", true, Page);
            }
        }

        protected void BtnUpKhoChina_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string Username = Session["userLoginSystem"].ToString();
            int ID = hdfKhoChinaID.Value.ToInt(0);
            if (ID > 0)
            {
                string Name = pNameKhoChina.Text;
                bool IsHidden = Convert.ToBoolean(hdfKhoChinaStatus.Value.ToInt(0));
                var khochina = WarehouseFromController.GetByID(ID);
                if (khochina != null)
                {
                    string kq = WarehouseFromController.Update(ID, Name, 0, "", "", "", "", "", IsHidden, currentDate, Username);

                    if (kq != null)
                    {
                        PJUtils.ShowMessageBoxSwAlert("Cập nhật kho Trung Quốc thành công.", "s", true, Page);
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình cập nhật trang. Vui lòng thử lại.", "e", true, Page);
                    }
                }
            }
        }

        protected void BtnUpKhoVietNam_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string Username = Session["userLoginSystem"].ToString();
            int ID = hdfKhoVietNamID.Value.ToInt(0);
            if (ID > 0)
            {
                string Name = pNameKhoVietNam.Text;
                bool IsHidden = Convert.ToBoolean(hdfKhoVietNamStatus.Value.ToInt(0));
                var khovn = WarehouseController.GetByID(ID);
                if (khovn != null)
                {
                    string kq = WarehouseController.Update(ID, Name, 0, "", "", "", "", "", IsHidden, currentDate, Username);

                    if (kq != null)
                    {
                        PJUtils.ShowMessageBoxSwAlert("Cập nhật kho Việt Nam thành công.", "s", true, Page);
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình cập nhật trang. Vui lòng thử lại.", "e", true, Page);
                    }
                }
            }
        }

        protected void BtnUpShippingType_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string Username = Session["userLoginSystem"].ToString();
            int ID = hdfShippingTypeID.Value.ToInt(0);
            if (ID > 0)
            {
                string Name = pNameShippingType.Text;
                bool IsHidden = Convert.ToBoolean(hdfShippingTypeStatus.Value.ToInt(0));
                var shipping = ShippingTypeToWareHouseController.GetByID(ID);
                if (shipping != null)
                {
                    string kq = ShippingTypeToWareHouseController.Update(ID, Name, "", IsHidden, currentDate, Username);

                    if (kq != null)
                    {
                        PJUtils.ShowMessageBoxSwAlert("Cập nhật hình thức vận chuyển thành công.", "s", true, Page);
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình cập nhật trang. Vui lòng thử lại.", "e", true, Page);
                    }
                }
            }
        }
    }
}