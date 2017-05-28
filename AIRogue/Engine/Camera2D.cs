using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AIRogue.Engine
{
    public class Camera2D
    {
        public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float CameraSpeed { get; set; } //Pixels per Second

        private Vector2 goalPosition;
        private Rectangle Bounds { get; set; }

        public Matrix TransformMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            }
        }

        public Camera2D(Viewport viewport)
        {
            Bounds = viewport.Bounds;
            Position = Vector2.Zero;
            Rotation = 0f;
            Zoom = 1f;
            CameraSpeed = 128;
        }

        /// <summary>
        /// Update Camera position
        /// </summary>
        public void Update()
        {
            if (!(goalPosition == Position))
            {
                float travelPercentage = (CameraSpeed * Clock.GetTick()) / (Position - goalPosition).Length();
                if (travelPercentage >= 1)
                {
                    Position = goalPosition;
                }
                else
                {
                    Position = Vector2.Lerp(Position, goalPosition, travelPercentage);
                }
            }
        }

        /// <summary>
        /// Tell the camera to move towards a position
        /// </summary>
        /// <param name="gotoPosition">Position the camera should move to.</param>
        public void LerpToPosition(Vector2 gotoPosition)
        {
            goalPosition = gotoPosition;
        }

        public Vector2 ScreenToWorldSpace(Vector2 mouseLocation)
        {
            return Vector2.Transform(mouseLocation, Matrix.Invert(this.TransformMatrix));
        }

        public Vector2 WorldToScreenSpace(Vector2 location)
        {
            return Vector2.Transform(location, this.TransformMatrix);
        }
    }
}
