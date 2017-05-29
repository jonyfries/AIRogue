//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using AIRogue.Agent;
using AIRogue.Agent.Brain;
using AIRogue.Engine;
using AIRogue.GameObjects;

namespace AIRogue
{
    public static class GameSetup
    {
        /// <summary>
        /// Setup and create the World and all IGameObjects
        /// </summary>
        public static void Setup() {
            //Setup the viewport and camera.
            Viewport gameViewport = new Viewport(0, 0, 1024, 768);
            ObjectHandler.gameCamera = new ControlledCamera(gameViewport);

            //Set game flags
            ObjectHandler.actionDelay = 0.2f;

            //Create the world
            World world = new World();

            //Determine the goal state
            Goal.GoalState targetState = new Goal.NoTreasure(world);
            ObjectHandler.TargetState = targetState;

            //Create the Agents
            Agent.Agent agent;

            //agent = new Agent.Agent();
            //agent.IsCameraTarget = true;
            //ControllerUser userBrain = new ControllerUser(agent, targetState);
            //SetupAgent(agent, userBrain, world);

            agent = new Agent.Agent();
            agent.IsCameraTarget = false;
            ControllerReflex reflexBrain = new ControllerReflex(agent, targetState);
            SetupAgent(agent, reflexBrain, world);

            agent = new Agent.Agent();
            agent.IsCameraTarget = true;
            ControllerReflexWithState stateReflexBrain = new ControllerReflexWithState(agent, targetState);
            SetupAgent(agent, stateReflexBrain, world);
        }

        static private void SetupAgent(Agent.Agent agent, Controller brain, World world)
        {
            string image = "agent";
            Vector2 position;
            do
            {
                position = new Vector2(Random.Next(0, world.width + 1), Random.Next(0, world.height + 1));
            } while (!world.GetLocation(position).IsWalkable);
            

            Vision eyes = new Vision(agent);
            Actor body = new Actor(agent, image, position, world);
            agent.Setup(brain, eyes, body, world);
            ObjectHandler.AddAgent(agent);

            if (agent.IsCameraTarget)
            {
                ObjectHandler.gameCamera.Target = body;
                ObjectHandler.cameraTarget = agent;
            }
        }
    }
}
