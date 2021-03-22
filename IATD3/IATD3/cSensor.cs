using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IATD3
{
    abstract class cSensor
    {
        protected cEnvironment environment = new cEnvironment();

        public abstract bool Sense();
    }

    class cSensorLight : cSensor
    {
        public override bool Sense()
        {
            return environment.IsAgentOnPortal();
        }
    }

    class cSensorOdour : cSensor
    {
        public override bool Sense()
        {
            return environment.IsAgentCellSmelly();
        }
    }

    class cSensorWind : cSensor
    {
        public override bool Sense()
        {
            return environment.IsAgentCellWindy();
        }
    }
}
