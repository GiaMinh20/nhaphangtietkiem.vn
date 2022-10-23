using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Services;
using System.Web.Script.Serialization;

namespace NHST.manager
{
    public partial class import_smallpackage : System.Web.UI.Page
    {
        string currFilePath = string.Empty;
        string currFileExtension = string.Empty;  //File Extension
        protected DataTable _FileTempPlan
        {
            get { return (DataTable)this.Session["FileTempDb992"]; }
            set
            {
                this.Session["FileTempDb992"] = value;
            }
        }

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
                    _FileTempPlan = null;
                }
                LoadDDL();
            }
        }

        [WebMethod]
        public static string GetBigPackage()
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                var bs = BigPackageController.GetAllWithStatus(1);
                if (bs.Count > 0)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    return serializer.Serialize(bs);
                }
                else
                    return null;
            }
            else
                return null;
        }

        [WebMethod]
        public static string AddBigPackage(string PackageCode, string Weight, string Volume)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var user = AccountController.GetByUsername(username);
                if (user != null)
                {
                    var check = BigPackageController.GetByPackageCode(PackageCode);
                    if (check != null)
                    {
                        return "existCode";
                    }
                    else
                    {
                        double volume = 0;
                        double weight = 0;

                        if (Convert.ToDouble(Volume) > 0)
                            volume = Convert.ToDouble(Volume);

                        if (Convert.ToDouble(Weight) > 0)
                            weight = Convert.ToDouble(Weight);

                        string kq = BigPackageController.Insert(PackageCode, weight, volume, 1,
                            DateTime.Now, username);

                        if (kq.ToInt(0) > 0)
                            return kq;
                        else
                            return null;
                    }
                }
                else
                    return null;
            }
            else
                return null;
        }
        public void LoadDDL()
        {
            var bs = BigPackageController.GetAllWithStatus(1);
            ddlBigpack.Items.Clear();
            ddlBigpack.Items.Insert(0, new ListItem("Chọn bao lớn", "0"));
            if (bs.Count > 0)
            {
                foreach (var b in bs)
                {
                    ListItem listitem = new ListItem(b.PackageCode, b.ID.ToString());
                    ddlBigpack.Items.Add(listitem);
                }
            }
            ddlBigpack.DataBind();
        }

        private void UploadToTemp()
        {
            HttpPostedFile file = this.FileUpload1.PostedFile;
            string fileName = file.FileName;
            string tempPath = System.IO.Path.GetTempPath();
            fileName = Path.GetFileName(fileName);
            this.currFileExtension = Path.GetExtension(fileName);
            this.currFilePath = tempPath + fileName;
            file.SaveAs(this.currFilePath);
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile)
                {
                    string username_current = Session["userLoginSystem"].ToString();                   
                    var obj_user = AccountController.GetByUsername(username_current);
                    DateTime currentDate = DateTime.Now;

                    UploadToTemp();
                    if (this.currFileExtension == ".xlsx" || this.currFileExtension == ".xls")
                    {
                        _FileTempPlan = PJUtils.ReadDataExcel(currFilePath, Path.GetExtension(FileUpload1.PostedFile.FileName), "Yes");// WebUtil.ReadDataFromExcel(currFilePath);
                        File.Delete((Path.GetTempPath() + FileUpload1.FileName));

                        var rs = string.Empty;                        
                        if (_FileTempPlan != null)
                        {
                            string checkMVD = "";
                            bool checktb = true;
                            var t = 0;
                            foreach (DataRow drRow in _FileTempPlan.Rows)
                            {
                                try
                                {
                                    checkMVD = drRow["MaVanDon"].ToString().Trim();
                                    double Weight = 0;
                                    if (drRow["CanNang"] != DBNull.Value)
                                    {
                                        Weight = Convert.ToDouble(drRow["CanNang"]);
                                    }
                                    int BaoLon = Convert.ToInt32(ddlBigpack.SelectedValue);
                                    if (!string.IsNullOrEmpty(checkMVD))
                                    {
                                        var check = SmallPackageController.GetByOrderTransactionCode(checkMVD);
                                        if (check != null)
                                        {
                                            var package = SmallPackageController.GetByID(Convert.ToInt32(check.ID));
                                            if (package != null)
                                            {
                                                SmallPackageController.UpdateImportKhoTQ(package.ID, Weight, 2, obj_user.Username, false, currentDate, obj_user.Username, currentDate, BaoLon);
                                                if (Convert.ToInt32(package.MainOrderID) > 0)
                                                {
                                                    var mo = MainOrderController.GetAllByID(Convert.ToInt32(package.MainOrderID));
                                                    if (mo != null)
                                                    {
                                                        string orderstatus = "";
                                                        int currentOrderStatus = Convert.ToInt32(mo.Status);
                                                        switch (currentOrderStatus)
                                                        {
                                                            case 0:
                                                                orderstatus = "Mới tạo";
                                                                break;
                                                            case 1:
                                                                orderstatus = "Hủy đơn hàng";
                                                                break;
                                                            case 2:
                                                                orderstatus = "Đã đặt cọc";
                                                                break;
                                                            case 4:
                                                                orderstatus = "Đã mua hàng";
                                                                break;
                                                            case 5:
                                                                orderstatus = "Shop phát hàng";
                                                                break;
                                                            case 6:
                                                                orderstatus = "Hàng về kho TQ";
                                                                break;
                                                            case 7:
                                                                orderstatus = "Hàng về kho VN";
                                                                break;
                                                            case 3:
                                                                orderstatus = "Đang vận chuyển Quốc tế";
                                                                break;
                                                            case 9:
                                                                orderstatus = "Khách đã thanh toán";
                                                                break;
                                                            case 10:
                                                                orderstatus = "Đã hoàn thành";
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                        if (mo.Status < 6)
                                                        {
                                                            MainOrderController.UpdateStatus(mo.ID, mo.UID.Value, 6);
                                                            if (mo.DateTQ == null)
                                                            {
                                                                MainOrderController.UpdateDateTQ(mo.ID, currentDate);
                                                                HistoryOrderChangeController.Insert(mo.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                                                " đã import đổi trạng thái của đơn hàng ID là: " + mo.ID + ", từ: " + orderstatus + ", sang: Hàng về kho TQ", 0, currentDate);
                                                            }
                                                        }                                                        
                                                    }
                                                }
                                                else if (package.TransportationOrderID > 0)
                                                {
                                                    var trans = TransportationOrderController.GetByID(package.TransportationOrderID.Value);
                                                    if (trans != null)
                                                    {
                                                        if (trans.Status < 4)
                                                        {
                                                            TransportationOrderController.UpdateStatus(trans.ID, 4, currentDate, obj_user.Username);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (check == null)
                                        {
                                            SmallPackageController.InsertTroiNoi(checkMVD, Weight, obj_user.Username, currentDate, currentDate, obj_user.Username, BaoLon);
                                        }
                                        else
                                        {
                                            checkMVD += drRow["MaVanDon"].ToString().Trim() + " - ";
                                            checktb = false;
                                        }
                                        t++;
                                    }    
                                    
                                }
                                catch (Exception a)
                                {
                                    //rs = rs + drRow["Title"].ToString() + " : lỗi :" + a.Message + "<br/>";
                                }                               
                            }
                            if (checktb)
                            {
                                PJUtils.ShowMessageBoxSwAlert("Import thành công " + t + " mã vận đơn.", "s", true, Page);
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert(t + " Mã vận đơn thành công, trong đó có " + checkMVD + " bị trùng.", "e", true, Page);
                            }
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Không có dữ liệu.", "s", true, Page);
                        }
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình import.", "e", true, Page);
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            StringBuilder StrExport = new StringBuilder();
            StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            StrExport.Append("<DIV  style='font-size:12px;'>");
            StrExport.Append("<table border=\"1\">");
            StrExport.Append("  <tr>");
            StrExport.Append("      <th><strong>MaVanDon</strong></th>");
            StrExport.Append("      <th><strong>CanNang</strong></th>");
            StrExport.Append("  </tr>");
            StrExport.Append("  <tr>");
            StrExport.Append("      <td>123456789</td>");
            StrExport.Append("      <td>0.5</td>");
            StrExport.Append("  </tr>");
            StrExport.Append("</table>");
            StrExport.Append("</div></body></html>");
            string strFile = "danh-sach-ma-van-don.xls";
            string strcontentType = "application/vnd.ms-excel";
            Response.ClearContent();
            Response.ClearHeaders();
            Response.BufferOutput = true;
            Response.ContentType = strcontentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
            Response.Write(StrExport.ToString());
            Response.Flush();
            Response.End();
        }
    }
}