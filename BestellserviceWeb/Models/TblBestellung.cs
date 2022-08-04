using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BestellserviceWeb.Models
{
    [Table("tblBestellung")]
    public partial class TblBestellung
    {
        [Key]
        [Column("besID")]
        public int BesId { get; set; }
        [Column("beskunIDRef")]
        public int BeskunIdref { get; set; }
        [Column("besproIDRef")]
        public int BesproIdref { get; set; }
        [Column("besMenge")]
        public double BesMenge { get; set; }
        [Required]
        [Column("besZeitstempel")]
        public byte[] BesZeitstempel { get; set; }

        [ForeignKey(nameof(BeskunIdref))]
        [InverseProperty(nameof(TblKunde.TblBestellung))]
        public virtual TblKunde BeskunIdrefNavigation { get; set; }
        [ForeignKey(nameof(BesproIdref))]
        [InverseProperty(nameof(TblProdukte.TblBestellung))]
        public virtual TblProdukte BesproIdrefNavigation { get; set; }
    }
}
