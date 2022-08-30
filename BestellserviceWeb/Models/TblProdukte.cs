using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BestellserviceWeb.Models
{
    [Table("tblProdukte")]
    public partial class TblProdukte
    {
        public TblProdukte()
        {
            TblBestellung = new HashSet<TblBestellung>();
        }

        [Key]
        [Column("proID")]
        public int ProId { get; set; }
        [Column("proKosten", TypeName = "money")]
        public decimal ProKosten { get; set; }
        [Required]
        [Column("proBezeichnung")]
        public string ProBezeichnung { get; set; }
        [Required]
        [Column("proZeitstempel")]
        public byte[] ProZeitstempel { get; set; }

        [InverseProperty("BesproIdrefNavigation")]
        public virtual ICollection<TblBestellung> TblBestellung { get; set; }

        public override string ToString()
        {
            return this.ProBezeichnung;
        }
    }
}
