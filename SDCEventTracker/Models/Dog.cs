//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SDCEventTracker.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dog
    {
        public Dog()
        {
            this.Results = new HashSet<Result>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> HandlerID { get; set; }
    
        public virtual Handler Handler { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
