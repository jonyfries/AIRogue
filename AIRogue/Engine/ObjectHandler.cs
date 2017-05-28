using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using AIRogue.GameObjects;

namespace AIRogue.Engine
{
    public static class ObjectHandler
    {
        public enum GameObjectLayer {
            BACKGROUND,
            ACTOR,
            ITEM,
        };

        static List<List<IGameObject>> gameObjectGroupList;
        static List<Agent.Agent> agentList = new List<Agent.Agent>();
        static Dictionary<string, Texture2D> resourceDictionary = new Dictionary<string, Texture2D>();
        static public Goal.GoalState TargetState { get; set; }
        static public ContentManager content;
        static public Camera2D gameCamera;
        static public float actionDelay;
        static private float nextTime;

        static ObjectHandler() {
            gameObjectGroupList = new List<List<IGameObject>>();
            foreach (GameObjectLayer listNumber in Enum.GetValues(typeof(GameObjectLayer))){
                gameObjectGroupList.Add(new List<IGameObject>());
            }
        }

        /// <summary>
        /// Get a Texture by name.
        /// </summary>
        /// <param name="key">String path to the image.</param>
        /// <returns></returns>
        public static Texture2D GetTexture(string key) {
            return content.Load<Texture2D>(key);
        }

        /// <summary>
        /// Get the SpriteFont.
        /// </summary>
        /// <returns>The SpriteFont for the game.</returns>
        public static SpriteFont GetFont()
        {
            return content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// Add an agent to the board. Used to define what is drawn on the screen.
        /// </summary>
        /// <param name="_agent">The agent to be added.</param>
        public static void AddAgent(Agent.Agent _agent)
        {
            agentList.Add(_agent);
        }

        /// <summary>
        /// Add a IGameObject to be drawn and updated.
        /// </summary>
        /// <param name="_gameObject">The IGameObject to be added.</param>
        /// <param name="layer">The layer that the IGameObject belongs to. Determines draw order.</param>
        public static void AddGameObject(IGameObject _gameObject, GameObjectLayer layer)
        {
            gameObjectGroupList[(int)layer].Add(_gameObject);
        }

        /// <summary>
        /// Remove an IGameObject from the list to be drawn and updated.
        /// </summary>
        /// <param name="_gameObject">The IGameObject that should be removed</param>
        /// <param name="layer">The layer that the IGameObject was originally added to.</param>
        public static void RemoveGameObject(IGameObject _gameObject, GameObjectLayer layer)
        {
            gameObjectGroupList[(int)layer].Remove(_gameObject);
        }

        /// <summary>
        /// Draw all IGameObjects.
        /// </summary>
        public static void Draw()
        {
            foreach (Agent.Agent agent in agentList)
            {
                if (agent.IsCameraTarget)
                {
                    agent.DisplayWorld();
                }
            }
        }

        /// <summary>
        /// Have controllers set their next actions and have all IGameObjects update.
        /// </summary>
        /// <returns>Bool</returns>
        public static bool Update()
        {
            InputHandler.HandleInput();

            if (nextTime < Clock.GetTime())
            {
                foreach (List<IGameObject> gameObjectList in gameObjectGroupList)
                {
                    foreach (IGameObject gameObject in gameObjectList)
                    {
                        gameObject.Update();
                    }
                }

                foreach (Agent.Agent curAgent in agentList)
                {
                    curAgent.GetNextAction();
                }

                nextTime = Clock.GetTime() + actionDelay;
            }

            gameCamera.Update();

            return !TargetState.IsGoalState();
        }
    }
}
