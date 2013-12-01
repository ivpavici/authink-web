//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Authink.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task
    {
        public Task()
        {
            this.Pictures = new HashSet<Picture>();
            this.Tests = new HashSet<Test>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public bool IsHidden { get; set; }
        public int Difficulty { get; set; }
        public string ProfilePictureUrl { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual Sound Sound { get; set; }
    }
}
