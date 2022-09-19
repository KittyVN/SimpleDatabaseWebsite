using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BestellserviceWeb.Models
{
    [Table("tblLeistungsbilderProjekt")]
    public partial class TblLeistungsbilderProjekt
    {
        [Key]
        [Column("leistpID")]
        public int LeistpId { get; set; }
        [Column("leistpProjekt")]
        public int LeistpProjekt { get; set; }
        [Column("leistpLeistungsbild")]
        public int LeistpLeistungsbild { get; set; }
        [Column("leistpAmount", TypeName = "money")]
        public decimal? LeistpAmount { get; set; }

        [ForeignKey(nameof(LeistpLeistungsbild))]
        [InverseProperty(nameof(TblLeistungsbilder.TblLeistungsbilderProjekt))]
        public virtual TblLeistungsbilder LeistpLeistungsbildNavigation { get; set; }
        [ForeignKey(nameof(LeistpProjekt))]
        [InverseProperty(nameof(TblProjekt.TblLeistungsbilderProjekt))]
        public virtual TblProjekt LeistpProjektNavigation { get; set; }
    }
}
