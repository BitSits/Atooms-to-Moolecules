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

namespace BitSits_Framework
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        GameScreen screen;

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen(GameScreen screen)
            : base("Paused", Vector2.Zero)
        {
            // Flag that there is no need for the game to transition
            // off when the pause menu is on top of it.
            IsPopup = true;

            this.screen = screen;
        }

        public override void LoadContent()
        {
            titleTexture = ScreenManager.GameContent.pauseTitle;

            // Create our menu entries.
            MenuEntry resumeMenuEntry = new MenuEntry("Resume Game", new Vector2(320, 300), this);
            MenuEntry quitMenuEntry = new MenuEntry("Back", new Vector2(270, 350), this);

            if (screen is GameplayScreen) quitMenuEntry.Text = "Back to Level Menu";
            if (screen is LabScreen) quitMenuEntry.Text = "Back to Main Menu";

            // Hook up menu event handlers.
            resumeMenuEntry.Selected += OnCancel;
            quitMenuEntry.Selected += QuitMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeMenuEntry);
            MenuEntries.Add(quitMenuEntry);
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (screen is GameplayScreen)
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen(),
                new LevelMenuScreen());

            if (screen is LabScreen)
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }


        #endregion

        #region Draw


        /// <summary>
        /// Draws the pause menu screen. This darkens down the gameplay screen
        /// that is underneath us, and then chains to the base MenuScreen.Draw.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 1 / 3);

            base.Draw(gameTime);
        }


        #endregion
    }
}
