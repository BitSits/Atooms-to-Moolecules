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
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameDataLibrary;

namespace BitSits_Framework
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        float score;
        int prevScore;

        // Meta-level game state.
        private const int MaxLevelIndex = 4;
        private int levelIndex = 2;
        private Level level;
        GameContent gameContent;
        bool load;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            gameContent = ScreenManager.GameContent;
            levelIndex = gameContent.levelIndex;
            LoadNextLevel();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (load)
            {
                if (level.IsLevelUp)
                {
                    load = false;
                    MessageBoxScreen m = new MessageBoxScreen(gameContent.levelUp, true);
                    m.Accepted += MessageBoxAccepted;
                    m.Cancelled += MessageBoxAccepted;
                    ScreenManager.AddScreen(m, null);

                    if (gameContent.storage.saveData.LevelData[gameContent.levelIndex] < level.Score)
                        gameContent.storage.saveData.LevelData[gameContent.levelIndex] = level.Score;
                }
                else if (level.ReloadLevel)
                {
                    load = false;
                    MessageBoxScreen m = new MessageBoxScreen(gameContent.retry, true);
                    m.Accepted += MessageBoxAccepted;
                    m.Cancelled += MessageBoxAccepted;
                    ScreenManager.AddScreen(m, null);
                }
            }

            if (IsActive) level.Update(gameTime);
        }

        void MessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            if (level.IsLevelUp)
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen(),
                    new LevelMenuScreen());

            else if (level.ReloadLevel) ReloadCurrentLevel();
        }

        private void LoadNextLevel()
        {
            if (level != null) level.Dispose();

            // Load the level.
            level = new Level(ScreenManager.GameContent);
            load = true;
        }

        private void ReloadCurrentLevel()
        {
            --levelIndex;
            LoadNextLevel();
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null) throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
                ScreenManager.AddScreen(new PauseMenuScreen(this), ControllingPlayer);

            level.HandleInput(input, playerIndex);
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            level.Draw(gameTime, spriteBatch);

            //if(!isLAB)
            DrawScore(gameTime, spriteBatch);

            //spriteBatch.Draw(gameContent.blackhole, Vector2.Zero, Color.White);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0) ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }

        private void DrawScore(GameTime gameTime, SpriteBatch spriteBatch)
        {
            score = Math.Min(score + (float)gameTime.ElapsedGameTime.TotalSeconds * 50, prevScore + level.Score);
            spriteBatch.DrawString(gameContent.symbolFont,
                "Atoomic Value\n    " + score.ToString("000"), new Vector2(10, 10), Color.White, 0, Vector2.Zero,
                25f / gameContent.symbolFontSize, SpriteEffects.None, 1);
        }


        #endregion
    }
}
