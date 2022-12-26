namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sLink
    {
        [Key]
        public int idlink { get; set; }

        public string sOriginalLink { get; set; }

        [StringLength(50)]
        public string sShorterLink { get; set; }
    }
}
