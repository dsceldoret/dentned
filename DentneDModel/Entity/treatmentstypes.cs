//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DG.DentneD.Model.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class treatmentstypes
    {
        public treatmentstypes()
        {
            this.treatments = new HashSet<treatments>();
        }
    
        public int treatmentstypes_id { get; set; }
        public string treatmentstypes_name { get; set; }
    
        public virtual ICollection<treatments> treatments { get; set; }
    }
}