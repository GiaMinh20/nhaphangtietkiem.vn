using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHST.Models;
using NHST.Bussiness;
using System.Data;
using WebUI.Business;
using MB.Extensions;
using static NHST.WebService1;

namespace NHST.Controllers
{
    public class SmallPackageController
    {
        #region CRUD
        public static string Insert(int BigPackageID, string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
            int Status, DateTime CreatedDate, string CreatedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.BigPackageID = BigPackageID;
                a.OrderTransactionCode = OrderTransactionCode;
                a.ProductType = ProductType;
                a.FeeShip = FeeShip;
                a.Weight = Weight;
                a.Volume = Volume;
                a.Status = Status;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }
        public static string InsertAll(int MainOrderID, int BigPackageID, string OrderTransactionCode, string ProductType,
            double FeeShip, double Weight, double Volume, int Status, bool isTemp, bool IsHelpMoving, int TransportationOrderID,
             DateTime CreatedDate, string CreatedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.MainOrderID = MainOrderID;
                a.BigPackageID = BigPackageID;
                a.OrderTransactionCode = OrderTransactionCode;
                a.ProductType = ProductType;
                a.FeeShip = FeeShip;
                a.Weight = Weight;
                a.Volume = Volume;
                a.Status = Status;
                a.IsTemp = isTemp;
                a.IsHelpMoving = IsHelpMoving;
                a.TransportationOrderID = TransportationOrderID;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }
        public static string InsertWithMainOrderID(int MainOrderID, int BigPackageID, string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
            int Status, string Description, DateTime CreatedDate, string CreatedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.MainOrderID = MainOrderID;
                a.BigPackageID = BigPackageID;
                a.OrderTransactionCode = OrderTransactionCode;
                a.ProductType = ProductType;
                a.FeeShip = FeeShip;
                a.Weight = Weight;
                a.Volume = Volume;
                a.Status = Status;
                a.Description = Description;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }
        public static string InsertWithMainOrderIDUIDUsername(int MainOrderID, int UID, string Username, int BigPackageID, string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
            int Status, string Description, DateTime CreatedDate, string CreatedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.MainOrderID = MainOrderID;
                a.UID = UID;
                a.Username = Username;
                a.BigPackageID = BigPackageID;
                a.OrderTransactionCode = OrderTransactionCode;
                a.ProductType = ProductType;
                a.FeeShip = FeeShip;
                a.Weight = Weight;
                a.Volume = Volume;
                a.Status = Status;
                a.Description = Description;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }
        public static string InsertWithMainOrderIDAndIsTemp(int MainOrderID, int BigPackageID, string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
            int Status, bool isTemp, int TransportationOrderID, DateTime CreatedDate, string CreatedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.MainOrderID = MainOrderID;
                a.BigPackageID = BigPackageID;
                a.OrderTransactionCode = OrderTransactionCode;
                a.ProductType = ProductType;
                a.FeeShip = FeeShip;
                a.Weight = Weight;
                a.Volume = Volume;
                a.Status = Status;
                a.IsTemp = isTemp;
                a.TransportationOrderID = TransportationOrderID;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }
        public static string InsertWithMainOrderIDAndIsTempAndIMG(int MainOrderID, int BigPackageID, string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
          int Status, bool isTemp, int TransportationOrderID, DateTime CreatedDate, string CreatedBy, string IMG, string Note)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.MainOrderID = MainOrderID;
                a.BigPackageID = BigPackageID;
                a.OrderTransactionCode = OrderTransactionCode;
                a.ProductType = ProductType;
                a.FeeShip = FeeShip;
                a.Weight = Weight;
                a.Volume = Volume;
                a.Status = Status;
                a.IsTemp = isTemp;
                a.Description = Note;
                a.TransportationOrderID = TransportationOrderID;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                if (!string.IsNullOrEmpty(IMG))
                    a.ListIMG = IMG;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }
        public static string InsertWithTransportationID(int TransportationOrderID, int BigPackageID,
             string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
             int Status, DateTime CreatedDate, string CreatedBy, int UID, string Username)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.MainOrderID = 0;
                a.TransportationOrderID = TransportationOrderID;
                a.BigPackageID = BigPackageID;
                a.OrderTransactionCode = OrderTransactionCode;
                a.ProductType = ProductType;
                a.FeeShip = FeeShip;
                a.Weight = Weight;
                a.Volume = Volume;
                a.Status = Status;
                a.IsHelpMoving = true;
                a.UID = UID;
                a.Username = Username;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }
        public static string InsertWithTransportationIDNew(int TransportationOrderID, int BigPackageID,
             string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
             bool IsCheckProduct, bool IsPackaged, bool IsInsurrance, string CODTQCYN, string CODTQVND,
             string UserNote, string StaffNoteCheck, string ProductQuantity,
             int Status, DateTime CreatedDate, string CreatedBy, int UID, string Username)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.MainOrderID = 0;
                a.TransportationOrderID = TransportationOrderID;
                a.BigPackageID = BigPackageID;
                a.OrderTransactionCode = OrderTransactionCode;
                a.ProductType = ProductType;
                a.FeeShip = FeeShip;
                a.Weight = Weight;
                a.Volume = Volume;
                a.IsCheckProduct = IsCheckProduct;
                a.IsPackaged = IsPackaged;
                a.IsInsurrance = IsInsurrance;
                a.CODTQCYN = CODTQCYN;
                a.CODTQVND = CODTQVND;
                a.UserNote = UserNote;
                a.StaffNoteCheck = StaffNoteCheck;
                a.ProductQuantity = ProductQuantity;
                a.Status = Status;
                a.IsHelpMoving = true;
                a.UID = UID;
                a.Username = Username;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }
        public static string UpdateNote(int ID, string Description)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_SmallPackage.Where(x => x.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    if (!string.IsNullOrEmpty(Description))
                        a.Description = Description;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateWarehouseNote(int ID, string warehouseNote)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_SmallPackage.Where(x => x.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    if (!string.IsNullOrEmpty(warehouseNote))
                        a.WarehouseNote = warehouseNote;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateInforUser(int ID, int UID, string Username, string UserPhone)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_SmallPackage.Where(x => x.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.UID = UID;
                    a.Username = Username;
                    a.UserPhone = UserPhone;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateUserPhoneAndUsername(int ID, string Username, string UserPhone)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_SmallPackage.Where(x => x.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Username = Username;
                    a.UserPhone = UserPhone;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateCurrentPlace(int ID, string currentPlace, int currentPlaceID)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_SmallPackage.Where(x => x.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.CurrentPlace = currentPlace;
                    a.CurrentPlaceID = currentPlaceID;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateIMG(int ID, string IMG, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(x => x.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.ListIMG = IMG;
                    a.ModifiedDate = ModifiedDate;
                    a.ModifiedBy = ModifiedBy;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                return null;
            }
        }
        public static string Update(int ID, int BigPackageID, string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
            int Status, string Description, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.BigPackageID = BigPackageID;
                    a.OrderTransactionCode = OrderTransactionCode;
                    a.ProductType = ProductType;
                    a.FeeShip = FeeShip;
                    a.Weight = Weight;
                    a.Volume = Volume;
                    a.Status = Status;
                    a.Description = Description;
                    a.ModifiedDate = ModifiedDate;
                    a.ModifiedBy = ModifiedBy;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string Update(int ID, int UID, string Username, int BigPackageID, string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
    int Status, string Description, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.BigPackageID = BigPackageID;
                    a.UID = UID;
                    a.Username = Username;
                    a.OrderTransactionCode = OrderTransactionCode;
                    a.ProductType = ProductType;
                    a.FeeShip = FeeShip;
                    a.Weight = Weight;
                    a.Volume = Volume;
                    a.Status = Status;
                    a.Description = Description;
                    a.ModifiedDate = ModifiedDate;
                    a.ModifiedBy = ModifiedBy;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateWeightStatus(int ID, double Weight, int Status, int BigPackageID, double Length, double Width,
            double Height)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.BigPackageID = BigPackageID;
                    a.Weight = Weight;
                    a.Status = Status;
                    a.Length = Length;
                    a.Width = Width;
                    a.Height = Height;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateStaffNoteCustdescproducttype(int ID, string StaffNoteCheck,
            string UserNote, string ProductType)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.StaffNoteCheck = StaffNoteCheck;
                    a.UserNote = UserNote;
                    a.ProductType = ProductType;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateMainOrderID(int ID, int MainOrderID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.MainOrderID = MainOrderID;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateMainOrderCodeID(int ID, int MainOrderCodeID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.MainOrderCodeID = MainOrderCodeID;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateTransportationOrderID(int ID, int TransportationOrderID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.TransportationOrderID = TransportationOrderID;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateIsLost(int ID, bool IsLost, int bigPackageID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.IsLost = IsLost;
                    a.BigPackageID = bigPackageID;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateTotalPrice(int ID, double TotalPrice)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.TotalPrice = TotalPrice;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateWeightStatusAndDateInLasteWareHouseIsLost(int ID, double Weight, int Status,
            DateTime DateInLasteWareHouse, bool IsLost, double Length, double Width, double Height)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.DateInLasteWareHouse = DateInLasteWareHouse;
                    a.IsLost = IsLost;
                    a.Weight = Weight;
                    a.Status = Status;
                    a.Length = Length;
                    a.Width = Width;
                    a.Height = Height;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string Delete(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    dbe.tbl_SmallPackage.Remove(a);
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateStatus(int ID, int Status, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Status = Status;
                    a.ModifiedDate = ModifiedDate;
                    a.ModifiedBy = ModifiedBy;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateDateCancelWareHouse(int ID, string StaffCancelWarehouse, DateTime DateCancelWarehouse)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.StaffCancel = StaffCancelWarehouse;
                    a.CancelDate = DateCancelWarehouse;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateDateInTQWareHouse(int ID, string StaffTQWarehouse, DateTime DateInTQWarehouse)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.StaffTQWarehouse = StaffTQWarehouse;
                    a.DateInTQWarehouse = DateInTQWarehouse;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateDateInVNWareHouse(int ID, string StaffVNWarehouse, DateTime DateInLasteWareHouse)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.StaffVNWarehouse = StaffVNWarehouse;
                    a.DateInLasteWareHouse = DateInLasteWareHouse;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateNotTemp(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.IsTemp = false;
                    a.IsHelpMoving = true;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateStatusAndIsLostAndDateInKhoDich(int ID, int Status, bool IsLost,
            DateTime DateInLasteWareHouse, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Status = Status;
                    a.IsLost = IsLost;
                    a.DateInLasteWareHouse = DateInLasteWareHouse;
                    a.ModifiedDate = ModifiedDate;
                    a.ModifiedBy = ModifiedBy;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateBigPackageID(int ID, int BigPackageID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.BigPackageID = BigPackageID;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateWarehouseFeeDateOutWarehouse(int ID, double WarehouseFee, DateTime DateOutWarehouse)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.WarehouseFee = WarehouseFee;
                    a.DateOutWarehouse = DateOutWarehouse;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateDateOutWarehouse(int ID, string StaffVNOutWarehouse, DateTime DateOutWarehouse)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.StaffVNOutWarehouse = StaffVNOutWarehouse;
                    a.DateOutWarehouse = DateOutWarehouse;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        #endregion
        #region Select
        public static List<tbl_SmallPackage> GetByOrderCode(string OrderTransactionCode)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.OrderTransactionCode == OrderTransactionCode).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetByOrderCodeUser(string OrderTransactionCode)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.OrderTransactionCode == OrderTransactionCode && p.IsTemp == true).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetByTransportationOrderIDAndFromStatus(int TransportationOrderID, int fromStatus)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.TransportationOrderID == TransportationOrderID && p.Status > fromStatus).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetAll(string s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.OrderTransactionCode.Contains(s)).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }

        public static List<smallpackage> GetAllSQLHelper(string searchtext, int status, string fd, string td)
        {
            var list = new List<smallpackage>();
            var sql = @"SELECT * from tbl_SmallPackage";
            sql += "    Where OrderTransactionCode like N'%" + searchtext + "%' ";
            if (status > -1)
            {
                sql += " AND Status = " + status + "";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = Convert.ToDateTime(fd).Date.ToString("yyyy-MM-dd HH:mm:ss");
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = Convert.ToDateTime(td).Date.ToString("yyyy-MM-dd HH:mm:ss");
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113)";
            }

            sql += " ORDER BY ID DESC";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new smallpackage();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                int bigPackageID = 0;
                if (reader["BigPackageID"] != DBNull.Value)
                    bigPackageID = reader["BigPackageID"].ToString().ToInt(0);
                string bigPackage = "";
                if (bigPackageID > 0)
                {
                    var bPackage = BigPackageController.GetByID(bigPackageID);
                    if (bPackage != null)
                    {
                        bigPackage = bPackage.PackageCode;
                    }
                }

                entity.BigPackageID = bigPackageID;
                entity.BigPackage = bigPackage;
                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                if (reader["MainOrderID"] != DBNull.Value)
                    entity.MainOrderID = reader["MainOrderID"].ToString().ToInt(0);
                if (reader["ProductType"] != DBNull.Value)
                    entity.ProductType = reader["ProductType"].ToString();
                if (reader["FeeShip"] != DBNull.Value)
                    entity.FeeShip = reader["FeeShip"].ToString().ToFloat(0);
                double weight = 0;
                if (reader["Weight"] != DBNull.Value)
                    weight = Convert.ToDouble(reader["Weight"]);
                entity.Weight = Math.Round(weight, 1);
                //entity.Weight = reader["Weight"].ToString().ToFloat(0);

                if (reader["Volume"] != DBNull.Value)
                    entity.Volume = reader["Volume"].ToString().ToFloat(0);
                int statuss = 0;
                if (reader["Status"] != DBNull.Value)
                {
                    statuss = Convert.ToInt32(reader["Status"].ToString());
                }
                string statusString = PJUtils.IntToStringStatusSmallPackage(statuss);
                entity.Status = statuss;
                entity.StatusString = statusString;
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);

                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static List<tbl_SmallPackage> GetAllWithIsLost(string s, bool isLost)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.OrderTransactionCode.Contains(s) && p.IsLost == isLost).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetAllTroinoi(string s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.MainOrderID == 0 && p.TransportationOrderID == 0).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetByMainOrderIDAndCode(int MainOrderID, string TransactionCode)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.MainOrderID == MainOrderID && p.OrderTransactionCode == TransactionCode).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetBuyBigPackageID(int BigPackageID, string text)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.BigPackageID == BigPackageID && p.OrderTransactionCode.Contains(text)).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetByTransportationOrderID(int TransportationOrderID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.TransportationOrderID == TransportationOrderID).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetByTransportationOrderIDAndStatus(int TransportationOrderID, int Status)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.TransportationOrderID == TransportationOrderID && p.Status == Status).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetByMainOrderID(int MainOrderID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.MainOrderID == MainOrderID).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetByMainOrderIDAndStatus(int MainOrderID, int Status)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.MainOrderID == MainOrderID && p.Status == Status).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetAllWithoutAddtoBigpacage()
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.BigPackageID == 0).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetAllByUID(int UID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.UID == UID).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }
        public static List<tbl_SmallPackage> GetAllByUIDAndStatus(int UID, int Status)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.UID == UID && p.Status == Status).OrderByDescending(p => p.ID).ToList();
                return ps;
            }
        }
        public static tbl_SmallPackage GetByID(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    return a;
                }
                else
                    return null;
            }
        }

        public static tbl_SmallPackage GetByOrderTransactionCode(string OrderTransactionCode)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.OrderTransactionCode == OrderTransactionCode).FirstOrDefault();
                if (a != null)
                {
                    return a;
                }
                else
                    return null;
            }
        }

        public static tbl_SmallPackage GetTroiNoi(string OrderTransactionCode)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.OrderTransactionCode == OrderTransactionCode && ad.MainOrderID == 0 && ad.TransportationOrderID == 0).FirstOrDefault();
                if (a != null)
                {
                    return a;
                }
                else
                    return null;
            }
        }

        public static List<tbl_SmallPackage> GetListByOrderTransactionCode(string OrderTransactionCode)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> lmls = new List<tbl_SmallPackage>();
                lmls = dbe.tbl_SmallPackage.Where(ad => ad.OrderTransactionCode == OrderTransactionCode).ToList();
                return lmls;
            }
        }

        public static List<tbl_SmallPackage> GetListByOrderTransactionCodeKhoVN(string OrderTransactionCode)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> lmls = new List<tbl_SmallPackage>();
                lmls = dbe.tbl_SmallPackage.Where(ad => ad.OrderTransactionCode == OrderTransactionCode && (ad.Status != 4)).ToList();
                return lmls;
            }
        }

        public static tbl_SmallPackage GetCodeWithdoutadd(string OrderTransactionCode)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.OrderTransactionCode == OrderTransactionCode && ad.BigPackageID == 0).FirstOrDefault();
                if (a != null)
                {
                    return a;
                }
                else
                    return null;
            }
        }
        public static List<tbl_SmallPackage> CheckCodeExist(string OrderTransactionCode)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> smalls = new List<tbl_SmallPackage>();
                smalls = dbe.tbl_SmallPackage.Where(ad => ad.OrderTransactionCode == OrderTransactionCode && ad.BigPackageID == 0).ToList();
                return smalls;
            }
        }
        public static int GetCountByBigPackageIDStatus(int BigPackageID, int statusf, int statust)
        {
            var sql = @"SELECT Count(*) as TotalPackages FROM dbo.tbl_SmallPackage Where BigPackageID = " + BigPackageID + " and status >= " + statusf + " and status <= " + statust + "";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int count = 0;
            while (reader.Read())
            {
                if (reader["TotalPackages"] != DBNull.Value)
                    count = reader["TotalPackages"].ToString().ToInt(0);
            }
            reader.Close();
            return count;
        }

        #endregion
        #region New
        public static List<tbl_SmallPackage> GetAllByMainOrderIDAndMainOrderCodeID(int MainOrderID, int MainOrderCodeID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.MainOrderID == MainOrderID && p.MainOrderCodeID == MainOrderCodeID).ToList();
                return ps;
            }
        }

        public static int GetTotalBuyBigPackage(int BigPackageID, string s)
        {
            var sql = @"select Total=Count(*) ";
            sql += "from tbl_SmallPackage ";
            sql += "where BigPackageID=" + BigPackageID + " and OrderTransactionCode like N'%" + s + "%' ";
            int a = 0;
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }
        public static List<ShowBigPackage> GetBuyBigPackageBySQL_DK(int BigPackageID, string s, int pageIndex, int pageSize)
        {
            var sql = @"select ID, BigPackageID,ProductType, MainOrderID, OrderTransactionCode, FeeShip, Weight, Volume, Status, CreatedDate, WarehouseNote, Description, UserNote, ";
            sql += "Case Status when 0 then N'<span class=\"white-text badge red darken-2\">Đã hủy</span>' ";
            sql += "when 1 then N'<span class=\"white-text badge blue darken-2\">Chưa về kho TQ</span>' ";
            sql += "when 2 then N'<span class=\"white-text badge orange darken-2\">Hàng về kho TQ</span>' ";
            sql += "when 3 then N'<span class=\"white-text badge yellow darken-2\">Hàng về kho VN</span>' ";
            sql += "When 4 then N'<span class=\"white-text badge green darken-2\">Đã giao cho khách</span>' ";
            sql += "end as StatusString ";
            sql += "from tbl_SmallPackage ";
            sql += "where BigPackageID=" + BigPackageID + " and OrderTransactionCode like N'%" + s + "%' ";
            sql += "order by ID desc, CreatedDate desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<ShowBigPackage> list = new List<ShowBigPackage>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new ShowBigPackage();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["BigPackageID"] != DBNull.Value)
                    entity.BigPackageID = reader["BigPackageID"].ToString().ToInt(0);

                if (reader["MainOrderID"] != DBNull.Value)
                    entity.MainOrderID = reader["MainOrderID"].ToString().ToInt(0);

                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();

                if (reader["FeeShip"] != DBNull.Value)
                    entity.FeeShip = Convert.ToDouble(reader["FeeShip"].ToString());

                if (reader["Weight"] != DBNull.Value)
                    entity.Weight = Convert.ToDouble(reader["Weight"].ToString());

                if (reader["Volume"] != DBNull.Value)
                    entity.Volume = Convert.ToDouble(reader["Volume"].ToString());

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDateString = Convert.ToDateTime(reader["CreatedDate"]).ToString("dd/MM/yyyy HH:mm");

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);

                if (reader["ProductType"] != DBNull.Value)
                    entity.ProductType = reader["ProductType"].ToString();

                if (reader["StatusString"] != DBNull.Value)
                    entity.StatusString = reader["StatusString"].ToString();

                if (reader["WarehouseNote"] != DBNull.Value)
                    entity.WarehouseNote = reader["WarehouseNote"].ToString();

                if (reader["Description"] != DBNull.Value)
                    entity.Description = reader["Description"].ToString();

                if (reader["UserNote"] != DBNull.Value)
                    entity.UserNote = reader["UserNote"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }


        public static int GetTotalBy_DK(int searchtype, string searchtext, string status, string fd, string td)
        {
            var sql = @"SELECT Total=Count(*) from tbl_SmallPackage ";
            sql += " Where OrderTransactionCode like N'%" + searchtext + "%' ";
            if (searchtype == 1)
                sql += " OR MainOrderID Like N'%" + searchtext + "%'";
            if (searchtype == 2)
                sql += " OR ID Like N'%" + searchtext + "%' ";

            if (!string.IsNullOrEmpty(status))
            {
                sql += " AND Status in (" + status + ")";

            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            return a;
        }
        public static List<smallpackage> GetAllSQLHelperWith_DK(int searchtype, string searchtext, string status, string fd, string td, int pageIndex, int pageSize)
        {
            var list = new List<smallpackage>();
            var sql = @"SELECT * from tbl_SmallPackage ";
            sql += " Where OrderTransactionCode like N'%" + searchtext + "%' ";
            if (searchtype == 1)
                sql += " OR MainOrderID Like N'%" + searchtext + "%'";
            if (searchtype == 2)
                sql += " OR ID Like N'%" + searchtext + "%' ";
            if (!string.IsNullOrEmpty(status))
            {
                sql += " AND Status in (" + status + ")";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            if (pageSize != 0)
            {
                sql += "order by id DESC OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            }
            else
            {
                sql += "order by id DESC";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new smallpackage();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                int bigPackageID = 0;
                if (reader["BigPackageID"] != DBNull.Value)
                    bigPackageID = reader["BigPackageID"].ToString().ToInt(0);
                string bigPackage = "";
                if (bigPackageID > 0)
                {
                    var bPackage = BigPackageController.GetByID(bigPackageID);
                    if (bPackage != null)
                    {
                        bigPackage = bPackage.PackageCode;
                    }
                }

                entity.BigPackageID = bigPackageID;
                entity.BigPackage = bigPackage;
                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                if (reader["MainOrderID"] != DBNull.Value)
                    entity.MainOrderID = reader["MainOrderID"].ToString().ToInt(0);
                if (reader["TransportationOrderID"] != DBNull.Value)
                    entity.TransportationOrderID = reader["TransportationOrderID"].ToString().ToInt(0);
                if (reader["ProductType"] != DBNull.Value)
                    entity.ProductType = reader["ProductType"].ToString();
                if (reader["Description"] != DBNull.Value)
                    entity.Description = reader["Description"].ToString();
                if (reader["FeeShip"] != DBNull.Value)
                    entity.FeeShip = reader["FeeShip"].ToString().ToFloat(0);
                double weight = 0;
                if (reader["Weight"] != DBNull.Value)
                    weight = Convert.ToDouble(reader["Weight"]);
                entity.Weight = Math.Round(weight, 1);

                if (reader["Volume"] != DBNull.Value)
                    entity.Volume = reader["Volume"].ToString().ToFloat(0);
                if (reader["Length"] != DBNull.Value)
                    entity.Length = reader["Length"].ToString().ToFloat(0);
                if (reader["Width"] != DBNull.Value)
                    entity.Width = reader["Width"].ToString().ToFloat(0);
                if (reader["Height"] != DBNull.Value)
                    entity.Height = reader["Height"].ToString().ToFloat(0);

                int statuss = 0;
                if (reader["Status"] != DBNull.Value)
                {
                    statuss = Convert.ToInt32(reader["Status"].ToString());
                }
                string statusString = PJUtils.IntToStringStatusSmallPackageWithBG45(statuss);
                entity.Status = statuss;
                entity.StatusString = statusString;
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<tbl_SmallPackage> GetAllLostBySQL(string s, int pageSize, int pageIndex)
        {
            var sql = @"select * ";
            sql += "from tbl_SmallPackage ";
            sql += "Where IsLost = 1 ";
            sql += "and OrderTransactionCode like N'%" + s + "%' ";
            sql += "Order by id DESC OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<tbl_SmallPackage> list = new List<tbl_SmallPackage>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new tbl_SmallPackage();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["MainOrderID"] != DBNull.Value)
                    entity.MainOrderID = reader["MainOrderID"].ToString().ToInt(0);

                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();

                if (reader["ProductType"] != DBNull.Value)
                    entity.ProductType = reader["ProductType"].ToString();

                if (reader["BigPackageID"] != DBNull.Value)
                    entity.BigPackageID = reader["BigPackageID"].ToString().ToInt(0);

                if (reader["FeeShip"] != DBNull.Value)
                    entity.FeeShip = Convert.ToDouble(reader["FeeShip"].ToString());

                if (reader["Weight"] != DBNull.Value)
                    entity.Weight = Convert.ToDouble(reader["Weight"].ToString());

                if (reader["Volume"] != DBNull.Value)
                    entity.Volume = Convert.ToDouble(reader["Volume"].ToString());

                if (reader["Description"] != DBNull.Value)
                    entity.Description = reader["Description"].ToString();

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static int GetTotalLostBySQL(string s)
        {
            var sql = @"select Total=Count(*) ";
            sql += "from tbl_SmallPackage ";
            sql += "Where IsLost = 1 ";
            sql += "and OrderTransactionCode like N'%" + s + "%' ";
            int a = 0;
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }
        public static int GetTotalTroiNoiBySQL(string s)
        {
            var sql = @"select Total=Count(*) ";
            sql += "from tbl_SmallPackage ";
            sql += "Where MainOrderID = 0 And TransportationOrderID = 0 ";
            sql += "and OrderTransactionCode like N'%" + s + "%' ";
            int a = 0;
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }

        public static int GetTotalTroiNoiUser(string s, string sttconfirm)
        {
            var sql = @"select Total=Count(*) ";
            sql += "from tbl_SmallPackage ";
            sql += "Where MainOrderID = 0 And TransportationOrderID = 0 and Status != 0";
            sql += "and OrderTransactionCode like N'%" + s + "%' ";
            if (!string.IsNullOrEmpty(sttconfirm))
            {
                if (sttconfirm != "-1")
                    sql += " AND StatusConfirm in (" + sttconfirm + ")";
            }
            int a = 0;
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }
        public static List<tbl_SmallPackage> GetAllTroiNoiUser(string s, string sttconfirm, int pageSize, int pageIndex)
        {
            var sql = @"select * ";
            sql += "from tbl_SmallPackage ";
            sql += "Where MainOrderID = 0 And TransportationOrderID = 0 and Status != 0";
            sql += "and OrderTransactionCode like N'%" + s + "%' ";
            if (!string.IsNullOrEmpty(sttconfirm))
            {
                if (sttconfirm != "-1")
                    sql += " AND StatusConfirm in (" + sttconfirm + ")";
            }
            sql += "Order by id DESC OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<tbl_SmallPackage> list = new List<tbl_SmallPackage>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new tbl_SmallPackage();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["MainOrderID"] != DBNull.Value)
                    entity.MainOrderID = reader["MainOrderID"].ToString().ToInt(0);

                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();

                if (reader["ProductType"] != DBNull.Value)
                    entity.ProductType = reader["ProductType"].ToString();

                if (reader["BigPackageID"] != DBNull.Value)
                    entity.BigPackageID = reader["BigPackageID"].ToString().ToInt(0);

                if (reader["FeeShip"] != DBNull.Value)
                    entity.FeeShip = Convert.ToDouble(reader["FeeShip"].ToString());

                if (reader["Weight"] != DBNull.Value)
                    entity.Weight = Convert.ToDouble(reader["Weight"].ToString());

                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();

                if (reader["UserPhone"] != DBNull.Value)
                    entity.UserPhone = reader["UserPhone"].ToString();

                if (reader["Description"] != DBNull.Value)
                    entity.Description = reader["Description"].ToString();

                if (reader["Volume"] != DBNull.Value)
                    entity.Volume = Convert.ToDouble(reader["Volume"].ToString());

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);

                if (reader["StatusConfirm"] != DBNull.Value)
                    entity.StatusConfirm = reader["StatusConfirm"].ToString().ToInt(0);

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static List<tbl_SmallPackage> GetAllTroiNoiBySQL(string s, int pageSize, int pageIndex)
        {
            var sql = @"select * ";
            sql += "from tbl_SmallPackage ";
            sql += "Where MainOrderID = 0 And TransportationOrderID = 0 ";
            sql += "and OrderTransactionCode like N'%" + s + "%' ";
            sql += "Order by id DESC OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<tbl_SmallPackage> list = new List<tbl_SmallPackage>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new tbl_SmallPackage();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["MainOrderID"] != DBNull.Value)
                    entity.MainOrderID = reader["MainOrderID"].ToString().ToInt(0);

                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();

                if (reader["ProductType"] != DBNull.Value)
                    entity.ProductType = reader["ProductType"].ToString();

                if (reader["Description"] != DBNull.Value)
                    entity.Description = reader["Description"].ToString();

                if (reader["BigPackageID"] != DBNull.Value)
                    entity.BigPackageID = reader["BigPackageID"].ToString().ToInt(0);

                if (reader["FeeShip"] != DBNull.Value)
                    entity.FeeShip = Convert.ToDouble(reader["FeeShip"].ToString());

                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();

                if (reader["Weight"] != DBNull.Value)
                    entity.Weight = Convert.ToDouble(reader["Weight"].ToString());

                if (reader["Volume"] != DBNull.Value)
                    entity.Volume = Convert.ToDouble(reader["Volume"].ToString());

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);

                if (reader["StatusConfirm"] != DBNull.Value)
                    entity.StatusConfirm = reader["StatusConfirm"].ToString().ToInt(0);

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static string InsertWithMainOrderIDUIDUsernameNew(int MainOrderID, int UID, string Username, int BigPackageID, string OrderTransactionCode,
        string ProductType, double FeeShip, double Weight, double Volume, int Status, string Description, DateTime CreatedDate, string CreatedBy, int MainOrderCodeID, int Trans)
        {
            //Cập nhật tổng tiền cân nặng
            var usercreate = AccountController.GetByID(UID);
            var mainorderCode = MainOrderCodeController.GetByID(MainOrderCodeID);
            var mainorder = MainOrderController.GetByMainOrderCode(mainorderCode.MainOrderCode + " | ");
            double pricePerWeight = 0;
            double returnprice = 0;
            int shipping = Convert.ToInt32(mainorder.ShippingType);

            if (!string.IsNullOrEmpty(usercreate.FeeTQVNPerWeight))
            {
                //double feetqvn = 0;
                if (usercreate.FeeTQVNPerWeight.ToFloat(0) > 0)
                {
                    pricePerWeight = Convert.ToDouble(usercreate.FeeTQVNPerWeight);
                }
                returnprice = Weight * pricePerWeight;
            }
            else
            {
                var fee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(Convert.ToInt32(mainorder.FromPlace), mainorder.ReceivePlace.ToInt(1), shipping, false);
                if (fee.Count > 0)
                {
                    foreach (var f in fee)
                    {
                        if (Weight > f.WeightFrom && Weight <= f.WeightTo)
                        {
                            pricePerWeight = Convert.ToDouble(f.Price);
                            returnprice = Weight * Convert.ToDouble(f.Price);
                            break;
                        }
                    }
                }
            }
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.MainOrderID = MainOrderID;
                a.TransportationOrderID = Trans;
                a.UID = UID;
                a.Username = Username;
                a.BigPackageID = BigPackageID;
                a.OrderTransactionCode = OrderTransactionCode;
                a.ProductType = ProductType;
                a.FeeShip = FeeShip;
                a.Weight = Weight;
                a.Volume = Volume;
                a.Status = Status;
                a.Description = Description;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                a.MainOrderCodeID = MainOrderCodeID;
                a.TotalPrice = returnprice;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }

        public static string InsertOrderTransactionCodeAuto(int MainOrderID, int UID, string Username, int BigPackageID, string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
        int Status, string Description, DateTime CreatedDate, string CreatedBy, int MainOrderCodeID, string IMG)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.MainOrderID = MainOrderID;
                a.UID = UID;
                a.Username = Username;
                a.BigPackageID = BigPackageID;
                a.OrderTransactionCode = OrderTransactionCode;
                a.ProductType = ProductType;
                a.FeeShip = FeeShip;
                a.Weight = Weight;
                a.Volume = Volume;
                a.Status = Status;
                a.ListIMG = IMG;
                a.Description = Description;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                a.MainOrderCodeID = MainOrderCodeID;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }

        public static string UpdateNew(int ID, int UID, string Username, int BigPackageID, string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
   int Status, string Description, DateTime ModifiedDate, string ModifiedBy, int MainOrderCodeID, double WeightPayable)
        {
            using (var dbe = new NHSTEntities())
            {
                //Cập nhật tổng tiền cân nặng
                var usercreate = AccountController.GetByID(UID);
                var mainorderCode = MainOrderCodeController.GetByID(MainOrderCodeID);
                var mainorder = MainOrderController.GetByMainOrderCode(mainorderCode.MainOrderCode + " | ");
                var smallPackage = GetByID(ID);
                double pricePerWeight = 0;
                double returnprice = 0;
                int shipping = Convert.ToInt32(mainorder.ShippingType);

                if (!string.IsNullOrEmpty(usercreate.FeeTQVNPerWeight))
                {
                    //double feetqvn = 0;
                    if (usercreate.FeeTQVNPerWeight.ToFloat(0) > 0)
                    {
                        pricePerWeight = Convert.ToDouble(usercreate.FeeTQVNPerWeight);
                    }
                    returnprice = WeightPayable * pricePerWeight;
                }
                else
                {
                    var fee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(Convert.ToInt32(mainorder.FromPlace), mainorder.ReceivePlace.ToInt(1), shipping, false);
                    if (fee.Count > 0)
                    {
                        foreach (var f in fee)
                        {
                            if (Weight > f.WeightFrom && Weight <= f.WeightTo)
                            {
                                pricePerWeight = Convert.ToDouble(f.Price);
                                returnprice = WeightPayable * Convert.ToDouble(f.Price);
                                break;
                            }
                        }
                    }
                }
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.BigPackageID = BigPackageID;
                    a.UID = UID;
                    a.Username = Username;
                    a.OrderTransactionCode = OrderTransactionCode;
                    a.ProductType = ProductType;
                    a.FeeShip = FeeShip;
                    a.Weight = Weight;
                    a.Volume = Volume;
                    a.Status = Status;
                    a.Description = Description;
                    a.ModifiedDate = ModifiedDate;
                    a.ModifiedBy = ModifiedBy;
                    a.MainOrderCodeID = MainOrderCodeID;
                    a.TotalPrice = returnprice;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public partial class ShowBigPackage
        {
            public int ID { get; set; }
            public int BigPackageID { get; set; }
            public int MainOrderID { get; set; }
            public string OrderTransactionCode { get; set; }
            public string ProductType { get; set; }
            public double FeeShip { get; set; }
            public double Weight { get; set; }
            public double Volume { get; set; }
            public double Height { get; set; }
            public double Width { get; set; }
            public double Length { get; set; }
            public int Status { get; set; }
            public string StatusString { get; set; }
            public string CreatedDateString { get; set; }
            public string WarehouseNote { get; set; }
            public string Description { get; set; }
            public string UserNote { get; set; }
        }
        #endregion

        public static List<ShowBigPackage> GetAllByBigPackageIDNotPage(int BigPackageID)
        {
            var sql = @"select ID, BigPackageID, ProductType, MainOrderID, OrderTransactionCode, FeeShip, Weight, Volume, Status, CreatedDate, Length, Width, Height, WarehouseNote, Description,UserNote, ";
            sql += "Case Status when 0 then N'<span class=\"white-text badge red darken-2\">Đã hủy</span>' ";
            sql += "when 1 then N'<span class=\"white-text badge blue darken-2\">Chưa về kho TQ</span>' ";
            sql += "when 2 then N'<span class=\"white-text badge orange darken-2\">Hàng về kho TQ</span>' ";
            sql += "When 6 then N'<span class=\"white-text badge blue darken-2\">Đang vận chuyển Quốc tế</span>' ";
            sql += "when 3 then N'<span class=\"white-text badge yellow darken-2\">Hàng về kho VN</span>' ";
            sql += "When 4 then N'<span class=\"white-text badge green darken-2\">Đã giao cho khách</span>' ";
            sql += "end as StatusString ";
            sql += "from tbl_SmallPackage ";
            sql += "where BigPackageID=" + BigPackageID + "";
            sql += "order by ID desc ";
            List<ShowBigPackage> list = new List<ShowBigPackage>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new ShowBigPackage();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["BigPackageID"] != DBNull.Value)
                    entity.BigPackageID = reader["BigPackageID"].ToString().ToInt(0);

                if (reader["MainOrderID"] != DBNull.Value)
                    entity.MainOrderID = reader["MainOrderID"].ToString().ToInt(0);

                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();

                if (reader["FeeShip"] != DBNull.Value)
                    entity.FeeShip = Convert.ToDouble(reader["FeeShip"].ToString());

                if (reader["Weight"] != DBNull.Value)
                    entity.Weight = Convert.ToDouble(reader["Weight"].ToString());

                if (reader["Volume"] != DBNull.Value)
                    entity.Volume = Convert.ToDouble(reader["Volume"].ToString());

                if (reader["Length"] != DBNull.Value)
                    entity.Length = Convert.ToDouble(reader["Length"].ToString());

                if (reader["Width"] != DBNull.Value)
                    entity.Width = Convert.ToDouble(reader["Width"].ToString());

                if (reader["Height"] != DBNull.Value)
                    entity.Height = Convert.ToDouble(reader["Height"].ToString());

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDateString = Convert.ToDateTime(reader["CreatedDate"]).ToString("dd/MM/yyyy HH:mm");

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);

                if (reader["ProductType"] != DBNull.Value)
                    entity.ProductType = reader["ProductType"].ToString();

                if (reader["StatusString"] != DBNull.Value)
                    entity.StatusString = reader["StatusString"].ToString();

                if (reader["WarehouseNote"] != DBNull.Value)
                    entity.WarehouseNote = reader["WarehouseNote"].ToString();

                if (reader["Description"] != DBNull.Value)
                    entity.Description = reader["Description"].ToString();

                if (reader["UserNote"] != DBNull.Value)
                    entity.UserNote = reader["UserNote"].ToString();

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public class smallpackage
        {
            public int ID { get; set; }
            public int BigPackageID { get; set; }
            public string BigPackage { get; set; }
            public string OrderTransactionCode { get; set; }
            public int MainOrderID { get; set; }
            public string ProductType { get; set; }
            public double FeeShip { get; set; }
            public double Weight { get; set; }
            public double Volume { get; set; }
            public double Length { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public int Status { get; set; }
            public string StatusString { get; set; }
            public DateTime CreatedDate { get; set; }
            public string Description { get; set; }
            public int TransportationOrderID { get; set; }
        }

        #region Ký gửi end
        public static string InsertWithTransportationID(int TransportationOrderID, int BigPackageID,
          string OrderTransactionCode, string ProductType, double FeeShip, double Weight, double Volume,
          int Status, DateTime CreatedDate, string CreatedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.MainOrderID = 0;
                a.TransportationOrderID = TransportationOrderID;
                a.BigPackageID = BigPackageID;
                a.OrderTransactionCode = OrderTransactionCode;
                a.ProductType = ProductType;
                a.FeeShip = FeeShip;
                a.Weight = Weight;
                a.Volume = Volume;
                a.Status = Status;
                a.IsHelpMoving = true;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }

        #endregion

        public static tbl_SmallPackage GetByOrderTransactionCodeAndMainOrderCodeID(string OrderTransactionCode, int MainOrderCodeID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.OrderTransactionCode == OrderTransactionCode && ad.MainOrderCodeID == MainOrderCodeID).FirstOrDefault();
                if (a != null)
                {
                    return a;
                }
                else
                    return null;
            }
        }

        public static tbl_SmallPackage GetByOrderTransactionCodeAndMainOrderCode(string OrderTransactionCode, string MainOrderCode)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.OrderTransactionCode == OrderTransactionCode && ad.MainOrderCode == MainOrderCode).FirstOrDefault();
                if (a != null)
                {
                    return a;
                }
                else
                    return null;
            }
        }

        public static string UpdateMainOrderCode(int ID, string MainOrderCode, int Status)
        {
            using (var db = new NHSTEntities())
            {
                var a = db.tbl_SmallPackage.Where(x => x.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.MainOrderCode = MainOrderCode;
                    a.Status = Status;
                    db.SaveChanges();
                    return a.ID.ToString();
                }
                else return null;
            }
        }

        public static string UpdateMainOrderForIsTemp(int ID, int UID, string Username, int MainOrderID, string ModifiedBy, DateTime ModifiedDate)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.UID = UID;
                    a.Username = Username;
                    a.MainOrderID = MainOrderID;
                    a.IsTemp = false;
                    a.IsHelpMoving = false;
                    a.ModifiedBy = ModifiedBy;
                    a.ModifiedDate = ModifiedDate;
                    dbe.SaveChanges();
                    return a.ID.ToString();
                }
                else return null;
            }
        }

        public static List<tbl_SmallPackage> ListSmallPackage()
        {
            var list = new List<tbl_SmallPackage>();
            var sql = @"SELECT s.* FROM dbo.tbl_SmallPackage AS s";
            sql += "  LEFT OUTER JOIN(SELECT * FROM dbo.tbl_MainOder) AS mo ON mo.ID = s.MainOrderID ";
            sql += "  WHERE s.MainOrderID > 0 AND mo.Status > 2 AND mo.Status <7 AND s.Status < 2 ";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new tbl_SmallPackage();

                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["MainOrderID"] != DBNull.Value)
                    entity.MainOrderID = reader["MainOrderID"].ToString().ToInt(0);

                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<tbl_SmallPackage> GetAllByMainOrderID(int MainOrderID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_SmallPackage> ps = new List<tbl_SmallPackage>();
                ps = dbe.tbl_SmallPackage.Where(p => p.MainOrderID == MainOrderID).ToList();
                return ps;
            }
        }

        public static string UpdateImportKhoTQ(int ID, double Weight, int Status, string StaffTQWarehouse, bool IsTemp, DateTime DateInTQWarehouse, string ModifiedBy, DateTime ModifiedDate, int BigPackageID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Weight = Weight;
                    a.Status = Status;
                    a.StaffTQWarehouse = StaffTQWarehouse;
                    a.IsTemp = IsTemp;
                    a.DateInTQWarehouse = DateInTQWarehouse;
                    a.ModifiedBy = ModifiedBy;
                    a.ModifiedDate = ModifiedDate;
                    a.BigPackageID = BigPackageID;
                    dbe.SaveChanges();
                    return a.ID.ToString();
                }
                else return null;
            }
        }
        public static string InsertTroiNoi(string OrderTransactionCode, double Weight, string StaffTQWarehouse, DateTime DateInTQWarehouse, DateTime CreatedDate, string CreatedBy, int BigPackageID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = new tbl_SmallPackage();
                a.OrderTransactionCode = OrderTransactionCode;
                a.Weight = Weight;
                a.MainOrderID = 0;
                a.TransportationOrderID = 0;
                a.BigPackageID = 0;
                a.Status = 2;
                a.StaffTQWarehouse = StaffTQWarehouse;
                a.DateInTQWarehouse = DateInTQWarehouse;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                a.BigPackageID = BigPackageID;
                dbe.tbl_SmallPackage.Add(a);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }
        public static string UpdateUserPhoneUIDUsernameStatusConfirm(int ID, int UID, string Username, string ReceiptPhone, string ReceiptNotes, int StatusConfirm, int Quantity, string ProductName)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_SmallPackage.Where(x => x.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.UID = UID;
                    a.Username = Username;
                    a.ReceiptPhone = ReceiptPhone;
                    a.ReceiptNotes = ReceiptNotes;
                    a.StatusConfirm = StatusConfirm;
                    a.Quantity = Quantity;
                    a.ProductName = ProductName;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateStatusConfirm(int ID, int StatusConfirm, string StaffNoteCheck, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.StatusConfirm = StatusConfirm;
                    a.StaffNoteCheck = StaffNoteCheck;
                    a.ModifiedDate = ModifiedDate;
                    a.ModifiedBy = ModifiedBy;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateCancelConfirm(int ID, string StaffNoteCheck, int StatusConfirm, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_SmallPackage a = dbe.tbl_SmallPackage.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Username = null;
                    a.UserPhone = null;
                    a.StaffNoteCheck = StaffNoteCheck;
                    a.StatusConfirm = StatusConfirm;
                    a.ModifiedDate = ModifiedDate;
                    a.ModifiedBy = ModifiedBy;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
    }
}