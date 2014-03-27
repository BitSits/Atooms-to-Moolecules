/*
 * Copyright (c) 2011 BitSits Games
 *  
 * Shubhajit Saha    http://bitsits.blogspot.com/
 * Maya Agarwal      http://bitsitsgames.blogspot.com/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    /// <summary>
    /// Very basic sample program for demonstrating a 2D Camera
    /// Controls are WASD for movement, QE for rotation, and ZC for zooming.
    /// </summary>
    class Camera2D
    {
        Vector2 viewportSize;
        public Vector2 Position;
        public float Rotation, Scale, Speed;
        public bool ManualCamera;
        public int ScrollHeight, ScrollWidth;
        public Vector2 ScrollBar;

        public Camera2D(Vector2 viewportSize, bool manualcamera)
        {
            this.ManualCamera = manualcamera;
            this.viewportSize = viewportSize;

            ScrollWidth = (int)viewportSize.X; ScrollHeight = (int)viewportSize.Y;

            //Position = viewportSize / 2;
            Scale = 1;
            Speed = 10;
        }

        public Matrix Transform
        {
            get
            {
                return Matrix.CreateScale(new Vector3(Scale, Scale, 0))
                    * Matrix.CreateRotationZ(Rotation)
                    * Matrix.CreateTranslation(new Vector3(viewportSize.X / 2 - Position.X * Scale,
                    viewportSize.Y / 2 - Position.Y * Scale, 0));
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void HandleInput(InputState input, PlayerIndex? controllingPlayer)
        {
            if (ManualCamera)
            {
                //translation controls, left stick xbox or WASD keyboard
                if (Keyboard.GetState().IsKeyDown(Keys.A)
                    || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed
                    || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0) { Position.X += Speed; }
                if (Keyboard.GetState().IsKeyDown(Keys.D)
                    || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed
                    || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0) { Position.X -= Speed; }
                if (Keyboard.GetState().IsKeyDown(Keys.S)
                    || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed
                    || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0) { Position.Y -= Speed; }
                if (Keyboard.GetState().IsKeyDown(Keys.W)
                    || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed
                    || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0) { Position.Y += Speed; }

                //rotation controls, right stick or QE keyboard
                if (Keyboard.GetState().IsKeyDown(Keys.Q)
                    || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < 0) { Rotation += 0.01f; }
                if (Keyboard.GetState().IsKeyDown(Keys.E)
                    || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0) { Rotation -= 0.01f; }

                //zoom/scale controls, left/right triggers or CZ keyboard
                if (Keyboard.GetState().IsKeyDown(Keys.C)
                    || GamePad.GetState(PlayerIndex.One).Triggers.Right > 0) { Scale += 0.001f; }
                if (Keyboard.GetState().IsKeyDown(Keys.Z)
                    || GamePad.GetState(PlayerIndex.One).Triggers.Left > 0) { Scale -= 0.001f; }
            }

            Vector2 mousePos = new Vector2(input.CurrentMouseState[0].X, input.CurrentMouseState[0].Y);
            if (mousePos.X < ScrollBar.X) Position.X -= Speed;
            else if (mousePos.X > viewportSize.X - ScrollBar.X) Position.X += Speed;

            if (mousePos.Y < ScrollBar.Y) Position.Y -= Speed;
            else if (mousePos.Y > viewportSize.Y - ScrollBar.Y) Position.Y += Speed;

            // Clamp
            Position.X = MathHelper.Clamp(Position.X, viewportSize.X / 2 / Scale,
                (ScrollWidth - viewportSize.X / 2 / Scale));
            Position.Y = MathHelper.Clamp(Position.Y, viewportSize.Y / 2 / Scale,
                (ScrollHeight - viewportSize.Y / 2 / Scale));
        }
    }
}
