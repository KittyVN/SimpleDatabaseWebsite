using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BestellserviceWeb.Models
{
    [Table("tblBilder")]
    public partial class TblBilder
    {
        [Key]
        [Column("bildID")]
        public int BildId { get; set; }
        [Column("bildKunde")]
        public int BildKunde { get; set; }
        [Required]
        [Column("bildName")]
        public string BildName { get; set; }
        [Required]
        [Column("bildDatei")]
        public byte[] BildDatei { get; set; }
        [Column("bildHaupt")]
        public bool BildHaupt { get; set; }

        [ForeignKey(nameof(BildKunde))]
        [InverseProperty(nameof(TblKunde.TblBilder))]
        public virtual TblKunde BildKundeNavigation { get; set; }
    }
}
