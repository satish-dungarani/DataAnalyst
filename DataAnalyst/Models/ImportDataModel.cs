using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalyst.Models
{
    public class ImportDataModel
    {
        public int ImportId { get; set; }
        public string DataYear { get; set; }
        public string DataMonth { get; set; }
        public string PharmacyName { get; set; }
        public string AccountNo { get; set; }
        public string FilePath { get; set; }
        public decimal? Generic { get; set; }
        public decimal? EthicalPI { get; set; }
        public decimal? SurgicalDressing { get; set; }
        public decimal? NonGeneric { get; set; }
        public decimal? NonPrescription { get; set; }
        public decimal? Insulin { get; set; }
        public decimal? Status { get; set; }
        public decimal? Electrical { get; set; }
        public decimal? Drinks { get; set; }
        public decimal? NonDiscount { get; set; }
        public decimal? OTC { get; set; }
        public decimal? Mobility { get; set; }
        public decimal? Specials { get; set; }
        public decimal? NP8 { get; set; }
        public decimal? Other1 { get; set; }
        public decimal? Other2 { get; set; }
        public decimal? Other3 { get; set; }
        public decimal TotalSpend { get; set; }
        public decimal TotalRebate { get; set; }
        public Nullable<int> InsUser { get; set; }
        public Nullable<DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }

    public class ProcessDataModel : ImportDataModel
    {
        public int ValueId { get; set; }
        public string ValueName { get; set; }
        public int refPhatmacyId { get; set; }
        public int refSupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Duration { get; set; }
        public string YearMon { get; set; }

        public decimal FinalSpend { get; set; }
        public decimal FinalRebate { get; set; }
    }
}