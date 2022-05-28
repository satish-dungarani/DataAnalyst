//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAnalystDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProcessData
    {
        public int ProcessID { get; set; }
        public string DataYear { get; set; }
        public string DataMonth { get; set; }
        public int refPharmacyID { get; set; }
        public int refSupplierID { get; set; }
        public string AccountNo { get; set; }
        public Nullable<decimal> Generic { get; set; }
        public Nullable<decimal> EthicalPI { get; set; }
        public Nullable<decimal> SurgicalDressing { get; set; }
        public Nullable<decimal> NonGeneric { get; set; }
        public Nullable<decimal> NonPrescription { get; set; }
        public Nullable<decimal> Insulin { get; set; }
        public Nullable<decimal> Status { get; set; }
        public Nullable<decimal> Electrical { get; set; }
        public Nullable<decimal> Drinks { get; set; }
        public Nullable<decimal> NonDiscount { get; set; }
        public Nullable<decimal> OTC { get; set; }
        public Nullable<decimal> Mobility { get; set; }
        public Nullable<decimal> Specials { get; set; }
        public Nullable<decimal> NP8 { get; set; }
        public Nullable<decimal> Other1 { get; set; }
        public Nullable<decimal> Other2 { get; set; }
        public Nullable<decimal> Other3 { get; set; }
        public decimal TotalSpend { get; set; }
        public decimal TotalRebate { get; set; }
        public int InsUser { get; set; }
        public System.DateTime InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
        public string FinYear { get; set; }
        public string QuaterType { get; set; }
        public string HalfYearType { get; set; }
    
        public virtual PharmacyMas PharmacyMas { get; set; }
        public virtual SupplierMas SupplierMas { get; set; }
    }
}
