namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tCampaign")]
    public partial class Campaings
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Campaings()
        {
            tCampaignSends = new HashSet<tCampaignSend>();
        }

        [Key]
        public int idcampaign { get; set; }

        [StringLength(100)]
        public string TitleCampaing { get; set; }

        public string Description { get; set; }

        [StringLength(100)]
        public string Sender { get; set; }

        [StringLength(159)]
        public string Text { get; set; }

        public int? idtemplate { get; set; }

        public DateTime? insertdate { get; set; }

        public DateTime? updatedate { get; set; }

        [StringLength(100)]
        public string updatedbyuser { get; set; }
        

        public virtual TemplateSMS tTemplateSm { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCampaignSend> tCampaignSends { get; set; }
    }
}
