namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RecipientsLists
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RecipientsLists()
        {
            tCampaigns = new HashSet<Campaings>();
            tCampaignSends = new HashSet<tCampaignSend>();
            tRecipientNumbers = new HashSet<tRecipientNumber>();
        }

        [Key]
        public int idrecipient { get; set; }

        [StringLength(100)]
        public string TitleRecipient { get; set; }

        public int? NumberOfRecords { get; set; }

        public DateTime? insertdate { get; set; }

        public DateTime? updatedate { get; set; }

        [StringLength(100)]
        public string updatedbyuser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Campaings> tCampaigns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCampaignSend> tCampaignSends { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tRecipientNumber> tRecipientNumbers { get; set; }
    }
}
