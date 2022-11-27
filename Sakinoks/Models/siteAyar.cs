namespace Sakinoks.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("siteAyar")]
    public partial class siteAyar
    {
        public int siteayarID { get; set; }

        [StringLength(50)]
        public string site_adi { get; set; }

        [StringLength(200)]
        public string slider { get; set; }

        [Column(TypeName = "text")]
        public string hakkinda_baslik { get; set; }

        [Column(TypeName = "text")]
        public string hakkinda_yazi { get; set; }

        [StringLength(200)]
        public string adres { get; set; }

        [StringLength(50)]
        public string mail { get; set; }

        [StringLength(50)]
        public string telefon1 { get; set; }

        [StringLength(50)]
        public string telefon2 { get; set; }

        [StringLength(50)]
        public string telefon3 { get; set; }

        [Column(TypeName = "text")]
        public string harita { get; set; }

        [Column(TypeName = "text")]
        public string keywords { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [Column("abstract", TypeName = "text")]
        public string _abstract { get; set; }
    }
}
