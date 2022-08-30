using System.Collections.Generic;

namespace BestellserviceWeb.Models
{
    public class BestellungenList
    {
        public List<int> ProdukteID { get; set; }
        public List<string> ProdukteName { get; set; }

        public List<bool> Active { get; set; }   

        public int KundeID { get; set; }

        public BestellungenList()
        {
            ProdukteID = new List<int>();
            Active = new List<bool>();
            ProdukteName = new List<string>();
        }

    }
}
