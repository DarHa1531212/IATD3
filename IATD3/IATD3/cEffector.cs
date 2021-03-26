﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IATD3
{
    public abstract class cEffector
    {
        public cEffector(cEnvironment _environment)
        {
            environment = _environment;
        }
        protected cEnvironment environment = new cEnvironment();

        /// <summary>
        ///   Sends the action to the environment (to simulate our agent doing the action).    
        /// </summary>
        /// <returns>
        ///   The cost of the action
        /// </returns>
        public abstract int DoAction();
    }

    public class cEffectorMove : cEffector
    {
        private int movementPosX;
        private int movementPosY;

        public cEffectorMove(cEnvironment environment) : base(environment)
        {

        }
        public int MovementPosX { get => movementPosX; set => movementPosX = value; }
        public int MovementPosY { get => movementPosY; set => movementPosY = value; }

        public override int DoAction()
        {
            int cost = environment.Move(movementPosY, movementPosX);
            return cost;
        }
    }

    public class cEffectorThrowRock : cEffector
    {
        private int launchPosX;
        private int launchPosY;

        public cEffectorThrowRock(cEnvironment environment) : base(environment)
        {

        }
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
        public cEffectorUsePortal(cEnvironment environment) : base(environment)
        {
        }
        public override int DoAction()
        {
            return environment.UsePortal();
        }
    }
}
