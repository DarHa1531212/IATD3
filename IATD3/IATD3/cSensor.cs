using System;
using System.Collections.Generic;

namespace IATD3
{
    public abstract class cSensor
    {
        protected cEnvironment environment = new cEnvironment();
        public cSensor(cEnvironment _environment)
        {
            environment = _environment;
        }

        /// <summary>
        /// Senses the position attributes in the environment.    
        /// </summary>
        /// <returns>If the position contains a spcific attribute.</returns>
        public abstract bool Sense();
    }

    class cSensorLight : cSensor
    {
        public cSensorLight(cEnvironment _environment) : base(_environment)
        { }

        public override bool Sense()
        {
            return environment.IsAgentOnPortal();
        }
    }

    class cSensorOdour : cSensor
    {
        public cSensorOdour(cEnvironment _environment) : base(_environment)
        { }

        public override bool Sense()
        {
            return environment.IsAgentCellSmelly();
        }
    }

    class cSensorWind : cSensor
    {
        public cSensorWind(cEnvironment _environment) : base(_environment)
        { }
        
        public override bool Sense()
        {
            return environment.IsAgentCellWindy();
        }
    }
    public class cSensorNeighbours
    {
        protected cEnvironment environment = new cEnvironment();
        public cSensorNeighbours(cEnvironment _environment)
        {
            environment = _environment;
        }

        public List<Tuple<int, int>> Get(int posX, int posY)
        {
            return environment.GetNeighbouringPositions(posX, posY);
        }
    }

    public class cSensorAbyss : cSensor
    {
        public cSensorAbyss(cEnvironment _environment) : base(_environment)
        { }
        public override bool Sense()
        {
            return environment.IsAgentOnAbyss();
        }
    }

    public class cSensorMonster : cSensor
    {
        public cSensorMonster(cEnvironment _environment) : base(_environment)
        { }
        public override bool Sense()
        {
            return environment.IsAgentOnMonster();
        }
    }
}
