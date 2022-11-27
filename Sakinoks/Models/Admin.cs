namespace Sakinoks.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Admin()
        {
            Urun = new HashSet<Urun>();
        }

        public int adminID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Lütfen Adýnýzý Giriniz")]
        public string adi_soyadi { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Lütfen Þifrenizi Giriniz")]
        public string sifre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urun> Urun { get; set; }
    }
}
