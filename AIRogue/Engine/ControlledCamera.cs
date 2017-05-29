using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using AIRogue.GameObjects;

namespace AIRogue.Engine
{
    public class ControlledCamera : Camera2D
    {
        private bool trackTarget = false;
        public IGameObject Target { private get;  set; }
        public bool TrackTarget
        {
            get
            {
                return Target != null && trackTarget;
            }
            private set
            {
                trackTarget = value;
            }
        }

        public ControlledCamera(Viewport viewport) : base(viewport)
        {
            InputHandler.KeyDownListeners.Add(OnKeyDown);
        }

        public void ToggleWorldCam(Agent.Agent targetAgent)
        {
            TrackTarget = !TrackTarget;
            Vector2 offset;
            if (TrackTarget)
            {
                offset = Target.Position - targetAgent.MemoryPosition;
            }
            else
            {
                offset = targetAgent.MemoryPosition - Target.Position;
            }
            Position += offset * 64;
            this.goalPosition = Position;
        }

        /// <summary>
        /// Handle OnKeyDown events.
        /// </summary>
        /// <param name="key">Key that caused the event.</param>
        public void OnKeyDown(Keys key)
        {
            if (key == Keys.OemPlus)
                this.Zoom *= .9f;
            else if (key == Keys.OemMinus)
            {
                this.Zoom *= 1.1f;
            } else if (key == Keys.Back)
            {
                this.Zoom = 1f;
            }
        }

        /// <summary>
        /// Update Camera position
        /// </summary>
        public new void Update()
        {
            if (TrackTarget)
            {
                LerpToPosition(Target.Position * 64);
            }
            base.Update();
        }
    }
}
