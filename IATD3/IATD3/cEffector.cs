using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IATD3
{
    public abstract class cEffector
    {
        protected cEnvironment environment = new cEnvironment();

        /// <summary>
        ///   Sends the action to the environment (to simulate our agent doing the action).    
        /// </summary>
        /// <returns>
        ///   The cost of the action
        /// </returns>
        public abstract int DoAction();
    }

    public class cEffectorUp : cEffector
    {
        public override int DoAction()
        {
            return environment.Move(0, -1);
        }
    }

    public class cEffectorRight : cEffector
    {

        public override int DoAction()
        {
            return environment.Move(1, 0);
            //return 1; //placeholder
        }
    }

    public class cEffectorLeft : cEffector
    {

        public override int DoAction()
        {
            return environment.Move(-1, 0);
        }
    }

    public class cEffectorDown : cEffector
    {

        public override int DoAction()
        {
            return environment.Move(0, 1);
        }
    }

    public class cEffectorThrowRock : cEffector
    {
        private int launchPosX;
        private int launchPosY;

        public int LaunchPosX { get => launchPosX; set => launchPosX = value; }
        public int LaunchPosY { get => launchPosY; set => launchPosY = value; }

        public override int DoAction()
        {
            int cost = environment.Throw(launchPosY, launchPosX);

            // Add that there is no monster on selected cell (and delete other fact if exists)
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            attributes.Add("presence", "False");

            FactTableManager.AddOrReplaceFactAtLocation("Monster", launchPosX, launchPosY, attributes);
            return cost;

        }
    }

    public class cEffectorUsePortal : cEffector
    {

        public override int DoAction()
        {
            return environment.UsePortal();
        }
    }
}
