namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class panelContactsVariable
    {
        [Key]
        public int idlig { get; set; }

        public int? idContact { get; set; }

        public int? idVar { get; set; }

        public string sValue { get; set; }

        public DateTime? insertdate { get; set; }

        public DateTime? updatedate { get; set; }

        [StringLength(100)]
        public string updatedbyuser { get; set; }

        public virtual Contacts panelContact { get; set; }

        public virtual tVarContact tVarContact { get; set; }
    }
}
