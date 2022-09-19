using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BestellserviceWeb.Models
{
    [Table("tblProjekt")]
    public partial class TblProjekt
    {
        public TblProjekt()
        {
            TblLeistungsbilderProjekt = new HashSet<TblLeistungsbilderProjekt>();
        }

        [Key]
        [Column("projID")]
        public int ProjId { get; set; }
        [Required]
        [Column("projName")]
        public string ProjName { get; set; }

        [InverseProperty("LeistpProjektNavigation")]
        public virtual ICollection<TblLeistungsbilderProjekt> TblLeistungsbilderProjekt { get; set; }
    }
}
