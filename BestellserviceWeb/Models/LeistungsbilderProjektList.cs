using System.Collections.Generic;

namespace BestellserviceWeb.Models
{
    public class LeistungsbilderProjektList
    {
        //public List<int> LeistungsbildID { get; set; }
        public List<decimal> MoneyAmount { get; set; }

        //public List<string> LeistungsbildName { get; set; }

        public List<TblLeistungsbilder> Leistungsbild { get; set; }
        public List<int> LeistungsbildDepth { get; set; }

        public List<bool> LeistungsbildActive { get; set; }

        public int ProjektID { get; set; }

        public LeistungsbilderProjektList()
        {
            //LeistungsbildID = new List<int>();
            MoneyAmount = new List<decimal>();
            //LeistungsbildName = new List<string>();
            LeistungsbildDepth = new List<int>();
            Leistungsbild = new List<TblLeistungsbilder>();
            LeistungsbildActive = new List<bool>();
        }
    }
}
