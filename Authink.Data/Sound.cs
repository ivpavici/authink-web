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
    
    public partial class Sound
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsHidden { get; set; }
        public string Type { get; set; }
    
        public virtual Task Task { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
