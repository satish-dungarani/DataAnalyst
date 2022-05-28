using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.ComponentModel;

namespace DataAnalyst.Models
{
    public class PharmacyMasterModel
    {
        public PharmacyMasterModel()
        {
            IsActive = true;
            this.CSAssociation = new List<CustSuppAssociationModel>();
        }
        public int? PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string RespPerson { get; set; }
        public string Telephone { get; set; }
        public string FaxNo { get; set; }
        [EmailAddress]
        public string EmailID { get; set; }
        public string DCRegNo { get; set; }
        public string PharmacoRegNo { get; set; }
        //[DefaultValue(true)]
        public bool IsActive { get; set; }

        public Nullable<int> InsUser { get; set; }
        public Nullable<DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }

        public List<CustSuppAssociationModel> CSAssociation { get; set; }

    }

    public class CustSuppAssociationModel
    {
        public int Id { get; set; }
        public int refPharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public int refSupplierId { get; set; }
        public string SupplierName { get; set; }
        public string AccountNo { get; set; }
    }
}