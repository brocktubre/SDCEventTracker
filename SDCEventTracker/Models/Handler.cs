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
    
    public partial class Handler
    {
        public Handler()
        {
            this.Dogs = new HashSet<Dog>();
            this.Results = new HashSet<Result>();
        }
    
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    
        public virtual ICollection<Dog> Dogs { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
