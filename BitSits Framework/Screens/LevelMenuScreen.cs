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

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using GameDataLibrary;

namespace BitSits_Framework
{
    class LevelMenuScreen : MenuScreen
    {
        #region Initialization


        public LevelMenuScreen()
            : base("Level Menu", Vector2.Zero) { }


        public override void LoadContent()
        {
            GameContent gameContent = ScreenManager.GameContent;
            camera = new Camera2D(gameContent.viewportSize, false);
            camera.ScrollWidth = 1600;
            camera.ScrollBar = new Vector2(225, 0);
            camera.Speed = 5;

            titleTexture = gameContent.levelMenuTitle;

            MenuEntry backMenuEntry = new MenuEntry("Back to Main Menu", new Vector2(400, 50), this);
            backMenuEntry.Selected += OnCancel;
            //MenuEntries.Add(backMenuEntry);

            List<Vector2> v = gameContent.content.Load<List<Vector2>>("Graphics/levelButton");

            for (int i = 0; i < gameContent.storage.saveData.LevelData.Count; i++)
            {
                MenuEntry me = new MenuEntry(gameContent.levelButton[i], v[i], this);
                
                me.UserData = i;
                if (gameContent.storage.saveData.LevelData[i] > 0)
                    me.footers = "Atoomic Value " + gameContent.storage.saveData.LevelData[i].ToString();

                me.Selected += LoadLevelMenuEntrySelected;
                MenuEntries.Add(me);
            }
        }


        #endregion

        #region Handle Input


        void LoadLevelMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.GameContent.levelIndex = (int)((MenuEntry)sender).UserData;
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new GameplayScreen());
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive && !ScreenManager.GameContent.gameCue.IsPlaying)
            {
                if (ScreenManager.GameContent.menuCue.IsPlaying)
                    ScreenManager.GameContent.menuCue.Stop(AudioStopOptions.AsAuthored);

                ScreenManager.GameContent.gameCue = ScreenManager.GameContent.soundBank.GetCue("game music");
                ScreenManager.GameContent.gameCue.Play();
            }
        }


        #endregion
    }
}
