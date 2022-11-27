namespace Sakinoks.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Urun")]
    public partial class Urun
    {
        public int urunID { get; set; }

        [StringLength(50)]
        public string isim { get; set; }

        [StringLength(200)]
        public string resim { get; set; }

        [Column(TypeName = "text")]
        public string aciklama { get; set; }

        [StringLength(50)]
        public string urunkodu { get; set; }

        [StringLength(50)]
        public string kod { get; set; }

        public int? kategoriID { get; set; }

        public int? adminID { get; set; }

        public bool? aktif { get; set; }

        public DateTime? tarih { get; set; }

        public virtual Admin Admin { get; set; }

        public virtual Kategoriler Kategoriler { get; set; }
    }
}
