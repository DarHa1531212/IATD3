using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IATD3
{
    public class cAction
    {
        private String name;
        private String[] parameters;

        public String Name { get => name; set => name = value; }
        public String[] Parameters { get => parameters; set => parameters = value; }

        public cAction()
        {
        }

        public void getParameters(string actionParams)
        {
            parameters = actionParams.Split(',');
        }


    }
}
