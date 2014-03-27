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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    /// <summary>
    /// Sample showing how to manage different game states, with transitions
    /// between menu screens, a loading screen, the game itself, and a pause
    /// menu. This main game class is extremely simple: all the interesting
    /// stuff happens in the ScreenManager component.
    /// </summary>
    public class BitSitsGames : Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        public static BloomComponent bloom;

        #endregion

        #region Initialization


        /// <summary>
        /// The main game constructor.
        /// </summary>
        public BitSitsGames()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            IsMouseVisible = true;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            graphics.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;

            bloom = new BloomComponent(this);
            Components.Add(bloom);

            bloom.Settings = BloomSettings.PresetSettings[5 % BloomSettings.PresetSettings.Length];

#if DEBUG
            Components.Add(new DebugComponent(this));

            //Level Menu
            LoadingScreen.Load(screenManager, false, null, new BackgroundScreen(), new MainMenuScreen()
            );//    , new LevelMenuScreen());

            //LoadingScreen.Load(screenManager, false, PlayerIndex.One, new GameplayScreen());
#else
            graphics.IsFullScreen = true;
            LoadingScreen.Load(screenManager, true, null, new BackgroundScreen(), new MainMenuScreen());
#endif
        }


        #endregion

        #region Draw


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }


        #endregion
    }


    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        static void Main()
        {
            using (BitSitsGames game = new BitSitsGames())
            {
                game.Run();
            }
        }
    }

    #endregion
}
