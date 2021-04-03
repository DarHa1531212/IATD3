using System;

namespace IATD3
{
    public class cAction
    {
        #region Attributes

        private String name;
        private String[] parameters;

        #endregion

        #region Getters / Setters

        public String Name { get => name; set => name = value; }
        public String[] Parameters { get => parameters; set => parameters = value; }

        #endregion

        #region Constructor

        public cAction()
        { }

        #endregion

        #region Public methods

        /// <summary>
        /// Sets the parameters from a single string (with commas to separate them).
        /// </summary>
        /// <param name="actionParams">The parameters as a single string.</param>
        public void SetParameters(string actionParams)
        {
            parameters = actionParams.Split(',');
        }

        #endregion
    }
}
