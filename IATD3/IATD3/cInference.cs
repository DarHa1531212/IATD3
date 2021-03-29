using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IATD3
{

    public class cInference
    {
        private List<cFact> facts;
        private List<cFact> implies;
        private List<cAction> actions;

        public List<cFact> Facts { get => facts; set => facts = value; }
        public List<cFact> Implies { get => implies; set => implies = value; }
        public List<cAction> Actions { get => actions; set => actions = value; }

        private bool isActionInference;
        public bool IsActionInference { get => isActionInference; set => isActionInference = value; }

        public cInference()
        {
            facts = new List<cFact>();
            implies = new List<cFact>();
            actions = new List<cAction>();
        }
    }
}
