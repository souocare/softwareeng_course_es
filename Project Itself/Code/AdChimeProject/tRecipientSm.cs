//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class tRecipientSm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tRecipientSm()
        {
            this.tCampaigns = new HashSet<tCampaign>();
            this.tCampaignSends = new HashSet<tCampaignSend>();
            this.tRecipientNumbers = new HashSet<tRecipientNumber>();
        }
    
        public int idrecipient { get; set; }
        public string TitleRecipient { get; set; }
        public Nullable<int> NumberOfRecords { get; set; }
        public Nullable<System.DateTime> insertdate { get; set; }
        public Nullable<System.DateTime> updatedate { get; set; }
        public string updatedbyuser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCampaign> tCampaigns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCampaignSend> tCampaignSends { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tRecipientNumber> tRecipientNumbers { get; set; }
    }
}