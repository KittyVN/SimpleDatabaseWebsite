﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BestellserviceWeb.Models
{
    [Table("tblKunde")]
    public partial class TblKunde
    {
        public TblKunde()
        {
            TblBestellung = new HashSet<TblBestellung>();
            TblBilder = new HashSet<TblBilder>();
            TblDokumente = new HashSet<TblDokumente>();
        }

        [Key]
        [Column("kunID")]
        public int KunId { get; set; }
        [Required]
        [Column("kunVorname")]
        public string KunVorname { get; set; }
        [Required]
        [Column("kunNachname")]
        public string KunNachname { get; set; }
        [Column("kunGeburtsdatum", TypeName = "datetime")]
        public DateTime? KunGeburtsdatum { get; set; }
        [Column("kunVIP")]
        public bool KunVip { get; set; }
        [Column("kunGeschlecht")]
        public int? KunGeschlecht { get; set; }

        [ForeignKey(nameof(KunGeschlecht))]
        [InverseProperty(nameof(TblGeschlecht.TblKunde))]
        public virtual TblGeschlecht KunGeschlechtNavigation { get; set; }
        [InverseProperty("BeskunIdrefNavigation")]
        public virtual ICollection<TblBestellung> TblBestellung { get; set; }
        [InverseProperty("BildKundeNavigation")]
        public virtual ICollection<TblBilder> TblBilder { get; set; }
        [InverseProperty("DokKundeNavigation")]
        public virtual ICollection<TblDokumente> TblDokumente { get; set; }
    }
}
