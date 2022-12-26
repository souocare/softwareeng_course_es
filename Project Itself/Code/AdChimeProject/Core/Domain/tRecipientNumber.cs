namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tRecipientNumber
    {
        [Key]
        public int idrecipientnumber { get; set; }

        public int? idrecipient { get; set; }

        public int? idContact { get; set; }

        public DateTime? insertdate { get; set; }

        public virtual Contacts panelContact { get; set; }

        public virtual RecipientsLists tRecipientSm { get; set; }
    }
}
