using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAnalyst.Models
{
    public class SupplierMasterModel
    {
        public SupplierMasterModel()
        {
            IsActive = true;
        }

        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string RespPerson { get; set; }
        public string Telephone { get; set; }
        public string FaxNo { get; set; }
        [EmailAddress]
        public string EmailID { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> InsUser { get; set; }
        public Nullable<DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }

    }
}