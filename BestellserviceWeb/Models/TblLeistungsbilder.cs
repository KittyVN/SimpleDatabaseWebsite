using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BestellserviceWeb.Models
{
    [Table("tblLeistungsbilder")]
    public partial class TblLeistungsbilder
    {
        public TblLeistungsbilder()
        {
            TblLeistungsbilderProjekt = new HashSet<TblLeistungsbilderProjekt>();
        }

        [Key]
        [Column("leistID")]
        public int LeistId { get; set; }
        [Column("leistParent")]
        public int? LeistParent { get; set; }
        [Required]
        [Column("leistName")]
        public string LeistName { get; set; }

        [InverseProperty("LeistpLeistungsbildNavigation")]
        public virtual ICollection<TblLeistungsbilderProjekt> TblLeistungsbilderProjekt { get; set; }
    }
}
