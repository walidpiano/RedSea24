//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RedSea24
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblLanguage
    {
        public tblLanguage()
        {
            this.tblLinkLanguages = new HashSet<tblLinkLanguage>();
        }
    
        public int Id { get; set; }
        public string LanguageName { get; set; }
    
        public virtual ICollection<tblLinkLanguage> tblLinkLanguages { get; set; }
    }
}