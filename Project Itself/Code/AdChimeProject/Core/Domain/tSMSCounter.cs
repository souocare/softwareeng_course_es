namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tSMSCounter")]
    public partial class tSMSCounter
    {
        [Key]
        public int idcounter { get; set; }

        public int? Counter { get; set; }
    }
}
