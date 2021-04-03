using System.Collections.Generic;

namespace IATD3
{
    public class cInference
    {
        #region Attributes

        private List<cFact> facts;
        private List<cFact> implies;
        private List<cAction> actions;

        private bool isActionInference;
        private bool isMarked;

        #endregion

        #region Getters / Setters

        public List<cFact> Facts { get => facts; set => facts = value; }
        public List<cFact> Implies { get => implies; set => implies = value; }
        public List<cAction> Actions { get => actions; set => actions = value; }
        public bool IsActionInference { get => isActionInference; set => isActionInference = value; }
        public bool IsMarked { get => isMarked; set => isMarked = value; }

        #endregion

        #region Constructor

        public cInference()
        {
            facts = new List<cFact>();
            implies = new List<cFact>();
            actions = new List<cAction>();
        }

        #endregion
    }
}
