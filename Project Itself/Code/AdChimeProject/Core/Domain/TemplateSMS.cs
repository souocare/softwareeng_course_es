namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TemplateSMS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TemplateSMS()
        {
            tCampaigns = new HashSet<Campaings>();
        }

        [Key]
        public int idtemplate { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public string Text { get; set; }

        public bool isaproved { get; set; }

        public DateTime? insertdate { get; set; }

        public DateTime? updatedate { get; set; }

        [StringLength(100)]
        public string updatedbyuser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Campaings> tCampaigns { get; set; }
    }
}
