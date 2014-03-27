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

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    class DebugComponent : DrawableGameComponent
    {
        //public static bool DebugMode = false;

        public static bool ShowBonds = false;

        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Point mousePos;

        int bloomSettingsIndex;

        KeyboardState prevKeyboardState;

        public DebugComponent(Game game)
            : base(game)
        {
            content = game.Content;
        }

        protected override void LoadContent()
        {
            font = content.Load<SpriteFont>("Fonts/DebugFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            MouseState mouseState = Mouse.GetState();
            mousePos = new Point(mouseState.X, mouseState.Y);

            //if (keyboardState.IsKeyDown(Keys.F1) && prevKeyboardState.IsKeyUp(Keys.F1))
            //    DebugMode = !DebugMode;

            if (keyboardState.IsKeyDown(Keys.F2) && prevKeyboardState.IsKeyUp(Keys.F2))
            {
                BitSitsGames.bloom.Visible = !BitSitsGames.bloom.Visible;
                BitSitsGames.bloom.Settings = BloomSettings.PresetSettings[5];
            }

            if (keyboardState.IsKeyDown(Keys.F3) && prevKeyboardState.IsKeyUp(Keys.F3))
            {
                bloomSettingsIndex = (bloomSettingsIndex + 1) %
                                     BloomSettings.PresetSettings.Length;

                BitSitsGames.bloom.Settings = BloomSettings.PresetSettings[bloomSettingsIndex];
            }

            if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyDown(Keys.Space))
                ShowBonds = !ShowBonds;

            prevKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            float fps = (1000.0f / (float)gameTime.ElapsedRealTime.TotalMilliseconds);
            spriteBatch.DrawString(font, "fps : " + fps.ToString("00"), Vector2.Zero, Color.White);

            spriteBatch.DrawString(font, "X = " + mousePos.X + " Y = " + mousePos.Y, new Vector2(0, 20),
                Color.White);

            string text = "F2 toggle bloom = " + (BitSitsGames.bloom.Visible ? "on" : "off") + "\n" +
                "F3 bloom settings = " + BitSitsGames.bloom.Settings.Name + "\n" +                
                "Space toggle ShowBond = " + (ShowBonds ? "on" : "off");

            spriteBatch.DrawString(font, text, new Vector2(0, 40), Color.White);

            spriteBatch.End();
        }
    }
}
