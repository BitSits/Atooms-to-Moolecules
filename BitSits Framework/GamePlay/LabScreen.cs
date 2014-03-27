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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using GameDataLibrary;

namespace BitSits_Framework
{
    class LabScreen : MenuScreen
    {
        GameContent gameContent;
        Level level;

        bool editMode = true;

        int numberOfEntries = 2, maxEntries, startEntryIndex = 0;
        List<MenuEntry> eqMenuEntry = new List<MenuEntry>();
        List<string> eqipFooters = new List<string>();

        public LabScreen()
            : base(" ", Vector2.Zero)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            gameContent = ScreenManager.GameContent;

            maxEntries = gameContent.labEquipButtons.Length;

            gameContent.levelIndex = -1;
            level = new Level(gameContent);

            eqipFooters = gameContent.content.Load<List<string>>("Graphics/labEquipFooters");

            AddEntries();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive) level.Update(gameTime);

            if (IsActive && !ScreenManager.GameContent.gameCue.IsPlaying)
            {
                if (ScreenManager.GameContent.menuCue.IsPlaying)
                    ScreenManager.GameContent.menuCue.Stop(AudioStopOptions.AsAuthored);

                ScreenManager.GameContent.gameCue = ScreenManager.GameContent.soundBank.GetCue("game music");
                ScreenManager.GameContent.gameCue.Play();
            }
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

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

            level.isMenuEntrySelected = isMouseOver; //Dont handle if menu entries are selected
            level.HandleInput(input, playerIndex);
        }

        void ToggleEditMode(object sender, PlayerIndexEventArgs e)
        {
            editMode = !editMode;

            AddEntries();

            level.ToggleEditMode();
        }

        void AddEntries()
        {
            MenuEntries.Clear();
            MenuEntry menuEntry;

            if (editMode)
            {
                menuEntry = new MenuEntry(gameContent.labAtoomButton, new Vector2(600, 50), this);
                menuEntry.Selected += ToggleEditMode;
                menuEntry.footers = "Atoomic View: left click to add and right click to remove atooms";
                MenuEntries.Add(menuEntry);

                menuEntry = new MenuEntry(gameContent.labClampButton, new Vector2(380, 50), this);
                menuEntry.Selected += level.ClampEquipment;
                menuEntry.footers = "Clamp It: clamp some of the lab equipments";
                MenuEntries.Add(menuEntry);

                menuEntry = new MenuEntry(gameContent.labPrevButton, new Vector2(150, 60), this);
                menuEntry.Selected += GetPrev;
                MenuEntries.Add(menuEntry);

                menuEntry = new MenuEntry(gameContent.labNextButton, new Vector2(345, 60), this);
                menuEntry.Selected += GetNext;
                MenuEntries.Add(menuEntry);

                GetEquipMenuEntries();
            }
            else
            {
                menuEntry = new MenuEntry(gameContent.labEquipButton, new Vector2(600, 50), this);
                menuEntry.Selected += ToggleEditMode;
                menuEntry.footers = "Equipment View: add, move and rotate lab equipments";
                MenuEntries.Add(menuEntry);
            }

            menuEntry = new MenuEntry(gameContent.labClearButton, new Vector2(500, 55), this);
            menuEntry.Selected += level.ClearLAB;
            if (editMode) menuEntry.footers = "Clear all the equipments form Lab";
            else menuEntry.footers = "Clear all Atooms from Lab";
            MenuEntries.Add(menuEntry);

            foreach (MenuEntry m in MenuEntries) m.footerPosition = new Vector2(100, 550);
        }

        void GetPrev(object sender, PlayerIndexEventArgs e)
        {
            if (startEntryIndex - numberOfEntries >= 0) startEntryIndex -= numberOfEntries;
            GetEquipMenuEntries();
        }

        void GetNext(object sender, PlayerIndexEventArgs e)
        {
            if (startEntryIndex + numberOfEntries < maxEntries) startEntryIndex += numberOfEntries;
            GetEquipMenuEntries();
        }

        void GetEquipMenuEntries()
        {
            //Remove previous ones
            for (int i = eqMenuEntry.Count - 1; i >= 0; i--) MenuEntries.Remove(eqMenuEntry[i]);

            for (int i = 0; i < numberOfEntries; i++)
            {
                int equipIndex = startEntryIndex + i;
                if (equipIndex == maxEntries) break;

                MenuEntry menuEntry = new MenuEntry(gameContent.labEquipButtons[equipIndex],
                    new Vector2(180 + i * 80, 50), this);
                menuEntry.UserData = (EquipmentName)(equipIndex);
                menuEntry.footers = eqipFooters[equipIndex];
                menuEntry.footerPosition = new Vector2(100, 550);
                menuEntry.Selected += level.AddEqipment;

                MenuEntries.Add(menuEntry); eqMenuEntry.Add(menuEntry);
            }
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            // Do nothing
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            level.Draw(gameTime, ScreenManager.SpriteBatch);

            //Hud back
            //if (editMode)
            //    spriteBatch.Draw(gameContent.blank, new Rectangle(150, 50, 530, 80), new Color(Color.Black, 0.2f));

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
