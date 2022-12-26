namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tCampaignSend")]
    public partial class tCampaignSend
    {
        [Key]
        public int idEnvioEmail { get; set; }

        public int? idcampaing { get; set; }

        [StringLength(100)]
        public string tEmailtoGroup { get; set; }

        public int? idrecipient { get; set; }

        public bool? bSucess { get; set; }

        public int? nEnvio { get; set; }

        public DateTime? sDatetoSend { get; set; }

        public int? idlink { get; set; }

        [StringLength(50)]
        public string sShorterLink { get; set; }

        [StringLength(100)]
        public string sSendbyWho { get; set; }

        public DateTime? insertdate { get; set; }

        public virtual Campaings tCampaign { get; set; }

        public virtual RecipientsLists tRecipientSm { get; set; }
    }
}
