using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IATD3
{

    public class cFact
    {
        private String element;
        private Dictionary<String, String> attributs;

        public Dictionary<String, String> Attributs { get => attributs; set => attributs = value; }
        public String Element { get => element; set => element = value; }

        public cFact(String e)
        {
            element = e;
            attributs = new Dictionary<String, String>();
        }

        /*public bool CorrespondsTo(cFact other)
        {
            bool correspondsTo = true;
            foreach (var attribut in Attributs)
            {
                if (attribut.Key != "locationX" && attribut.Key != "locationY")
                {
                    correspondsTo = correspondsTo && (attribut.Value == other.Attributs[attribut.Key]);
                }
            }
            return correspondsTo;
        }*/
    }
}
