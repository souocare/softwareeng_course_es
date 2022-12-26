namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tVarContact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tVarContact()
        {
            panelContactsVariables = new HashSet<panelContactsVariable>();
        }

        [Key]
        public int idVar { get; set; }

        public bool? visible { get; set; }

        [StringLength(255)]
        public string VarName { get; set; }

        public int? colNumber { get; set; }

        [StringLength(50)]
        public string colTypeType { get; set; }

        [StringLength(50)]
        public string colTypeFilter { get; set; }

        public DateTime? insertdate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<panelContactsVariable> panelContactsVariables { get; set; }
    }
}
