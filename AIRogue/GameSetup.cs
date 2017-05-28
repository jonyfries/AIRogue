﻿//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using AIRogue.Agent;
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
            ObjectHandler.gameCamera = new Camera2D(gameViewport);

            //Set game flags
            ObjectHandler.actionDelay = 0.2f;

            //Create the world
            World world = new World();

            //Determine the goal state
            Goal.GoalState targetState = new Goal.NoTreasure(world);
            ObjectHandler.TargetState = targetState;

            //Create the Agents
            Agent.Agent agent;
            bool isCameraTarget;

            //agent = new Agent.Agent();
            //isCameraTarget = true;
            //ControllerUser userBrain = new ControllerUser(agent, targetState);
            //SetupAgent(agent, userBrain, world, isCameraTarget);

            //agent = new Agent.Agent();
            //isCameraTarget = false;
            //ControllerReflex reflexBrain = new ControllerReflex(agent, targetState);
            //SetupAgent(agent, reflexBrain, world, isCameraTarget);

            agent = new Agent.Agent();
            isCameraTarget = true;
            ControllerReflexWithState stateReflexBrain = new ControllerReflexWithState(agent, targetState);
            SetupAgent(agent, stateReflexBrain, world, isCameraTarget);
        }

        static private void SetupAgent(Agent.Agent agent, Controller brain, World world, bool isCameraTarget)
        {
            string image = "agent";
            Vector2 position;
            do
            {
                position = new Vector2(Random.Next(0, world.width + 1), Random.Next(0, world.height + 1));
            } while (!world.GetLocation(position).IsWalkable);
            

            Vision eyes = new Vision(agent);
            Actor body = new Actor(agent, image, position, world);
            agent.IsCameraTarget = isCameraTarget;
            agent.Setup(brain, eyes, body, world);
            ObjectHandler.AddAgent(agent);
        }
    }
}