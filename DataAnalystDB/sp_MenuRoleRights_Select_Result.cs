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
    
    public partial class sp_MenuRoleRights_Select_Result
    {
        public string MenuName { get; set; }
        public Nullable<int> RefRoleId { get; set; }
        public int RefMenuId { get; set; }
        public bool CanInsert { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }
        public Nullable<int> InsUser { get; set; }
        public Nullable<System.DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }
}
