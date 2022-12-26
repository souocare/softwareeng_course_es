namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AppUsers
    {
        [Key]
        public int idUser { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public string Password { get; set; }

        public Guid? iDLogin { get; set; }

        public bool? isadmin { get; set; }
    }
}
