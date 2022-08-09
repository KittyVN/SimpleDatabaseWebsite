using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BestellserviceWeb.Models
{
    [Table("tblDokumente")]
    public partial class TblDokumente
    {
        [Key]
        [Column("dokID")]
        public int DokId { get; set; }
        [Column("dokKunde")]
        public int DokKunde { get; set; }
        [Required]
        [Column("dokName")]
        public string DokName { get; set; }
        [Required]
        [Column("dokDatei")]
        public byte[] DokDatei { get; set; }

        [ForeignKey(nameof(DokKunde))]
        [InverseProperty(nameof(TblKunde.TblDokumente))]
        public virtual TblKunde DokKundeNavigation { get; set; }
    }
}
