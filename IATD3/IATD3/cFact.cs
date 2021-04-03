using System;
using System.Collections.Generic;

namespace IATD3
{

    public class cFact
    {
        #region Attributes

        private String element;
        private Dictionary<String, String> attributes;

        #endregion

        #region Getters / Setters

        public Dictionary<String, String> Attributes { get => attributes; set => attributes = value; }
        public String Element { get => element; set => element = value; }

        #endregion

        #region Constructors

        public cFact(String e)
        {
            element = e;
            attributes = new Dictionary<String, String>();
        }

        #endregion
    }
}
