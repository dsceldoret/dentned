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
    
    public partial class treatmentspriceslists
    {
        public treatmentspriceslists()
        {
            this.patients = new HashSet<patients>();
            this.treatmentsprices = new HashSet<treatmentsprices>();
        }
    
        public int treatmentspriceslists_id { get; set; }
        public string treatmentspriceslists_name { get; set; }
        public decimal treatmentspriceslists_multiplier { get; set; }
    
        public virtual ICollection<patients> patients { get; set; }
        public virtual ICollection<treatmentsprices> treatmentsprices { get; set; }
    }
}