//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NHST.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_MainOder
    {
        public int ID { get; set; }
        public Nullable<int> UID { get; set; }
        public string ShopID { get; set; }
        public string ShopName { get; set; }
        public string Site { get; set; }
        public Nullable<bool> IsForward { get; set; }
        public string IsForwardPrice { get; set; }
        public Nullable<bool> IsFastDelivery { get; set; }
        public string IsFastDeliveryPrice { get; set; }
        public Nullable<bool> IsCheckProduct { get; set; }
        public string IsCheckProductPrice { get; set; }
        public Nullable<bool> IsPacked { get; set; }
        public string IsPackedPrice { get; set; }
        public Nullable<bool> IsFast { get; set; }
        public string IsFastPrice { get; set; }
        public string PriceVND { get; set; }
        public string PriceCNY { get; set; }
        public string FeeShipCN { get; set; }
        public string FeeBuyPro { get; set; }
        public string FeeBuyProCK { get; set; }
        public string FeeWeight { get; set; }
        public string FeeWeightCK { get; set; }
        public string Note { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<int> Status { get; set; }
        public string Deposit { get; set; }
        public string CurrentCNYVN { get; set; }
        public string TotalPriceVND { get; set; }
        public Nullable<int> SalerID { get; set; }
        public Nullable<int> DathangID { get; set; }
        public Nullable<int> KhoTQID { get; set; }
        public Nullable<int> KhoVNID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> DepostiDate { get; set; }
        public Nullable<bool> IsHidden { get; set; }
        public string AmountDeposit { get; set; }
        public string OrderTransactionCode { get; set; }
        public string OrderTransactionCode2 { get; set; }
        public string OrderTransactionCode3 { get; set; }
        public string OrderTransactionCode4 { get; set; }
        public string OrderTransactionCode5 { get; set; }
        public string FeeShipCNToVN { get; set; }
        public string OrderWeight { get; set; }
        public string OrderTransactionCodeWeight { get; set; }
        public string OrderTransactionCodeWeight2 { get; set; }
        public string OrderTransactionCodeWeight3 { get; set; }
        public string OrderTransactionCodeWeight4 { get; set; }
        public string OrderTransactionCodeWeight5 { get; set; }
        public string ReceivePlace { get; set; }
        public string TQVNWeight { get; set; }
        public Nullable<bool> IsGiaohang { get; set; }
        public string TotalPriceReal { get; set; }
        public string TotalPriceRealCYN { get; set; }
        public string MainOrderCode { get; set; }
        public string AdditionFeeForSensorProduct { get; set; }
        public string AdditionFeeForFuel { get; set; }
        public Nullable<int> ShippingType { get; set; }
        public Nullable<int> OrderType { get; set; }
        public Nullable<bool> IsCheckNotiPrice { get; set; }
        public Nullable<int> FromPlace { get; set; }
        public Nullable<double> FeeInWareHouse { get; set; }
        public Nullable<bool> IsDoneSmallPackage { get; set; }
        public Nullable<bool> IsUpdatePrice { get; set; }
        public Nullable<System.DateTime> PayDate { get; set; }
        public string TotalFeeSupport { get; set; }
        public Nullable<System.DateTime> DateBuy { get; set; }
        public Nullable<System.DateTime> DateVN { get; set; }
        public Nullable<System.DateTime> DateTQ { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public Nullable<bool> IsInsurrance { get; set; }
        public string InsuranceMoney { get; set; }
        public string InsurancePercent { get; set; }
        public Nullable<System.DateTime> ExpectedDate { get; set; }
        public Nullable<bool> IsFlow { get; set; }
        public Nullable<int> StatusPackage { get; set; }
        public string TimeLine { get; set; }
        public string FeeVolume { get; set; }
        public string FeeVolumeCK { get; set; }
        public string OrderVolume { get; set; }
        public string TQVNVolume { get; set; }
        public Nullable<System.DateTime> DateDeliveryShop { get; set; }
        public string Barcode { get; set; }
        public Nullable<int> QuantityBarcode { get; set; }
        public Nullable<int> QuantityMainOrderCode { get; set; }
        public Nullable<System.DateTime> DateLoading { get; set; }
        public string LinkImage { get; set; }
        public string PercentDeposit { get; set; }
        public string PercentBuyPro { get; set; }
        public Nullable<System.DateTime> AdPayDate { get; set; }
        public string StaffNote { get; set; }
    }
}
