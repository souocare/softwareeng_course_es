namespace AdChimeProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Contacts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contacts()
        {
            panelContactsVariables = new HashSet<panelContactsVariable>();
            tRecipientNumbers = new HashSet<tRecipientNumber>();
        }

        [Key]
        public int idContact { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        public bool? bActive { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(20)]
        public string CountryCodePhone { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        public int? optinSMS { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updatedate { get; set; }

        [StringLength(100)]
        public string updatedbyuser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<panelContactsVariable> panelContactsVariables { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tRecipientNumber> tRecipientNumbers { get; set; }
    }
}
