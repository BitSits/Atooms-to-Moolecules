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
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Box2D.XNA;
using GameDataLibrary;

namespace BitSits_Framework
{
    /// <summary>
    /// All the Contents of the Game is loaded and stored here
    /// so that all other screen can copy from here
    /// </summary>
    public class GameContent
    {
        public readonly ContentManager content;
        public readonly Vector2 viewportSize;

        // Level Data
        const int MaxLevelIndex = 8;
        public int levelIndex = 7;
        public readonly Storage storage;
        //GenerateData gd = new GenerateData();

        public readonly int scale = 30;

        public readonly Random random = new Random();

        // Textures
        public readonly Texture2D blank, gradient, blackhole;

        public readonly Texture2D logo;
        public readonly Texture2D credits, levelUp, retry, menuBackground, mainMenuTitle, levelMenuTitle, pauseTitle;

        public readonly Texture2D[] levelBackground = new Texture2D[MaxLevelIndex];
        public readonly Texture2D labTable;
        public readonly Texture2D labNextButton, labPrevButton, labClampButton, labAtoomButton,
            labEquipButton, labClearButton;

        public readonly Texture2D[] tutorial = new Texture2D[6];

        public readonly Texture2D[] levelButton = new Texture2D[8];
        public readonly Texture2D[] labEquipButtons = new Texture2D[9];

        public readonly int symbolCount = 6;
        public readonly float atomRadius = 25;
        public readonly Vector2 atomOrigin;
        public readonly Texture2D[] atom;
        public readonly Texture2D shine;

        public readonly Texture2D[,] equipment = new Texture2D[9, 3];
        public readonly Texture2D clamp, clampJoint, clampRod, clampStand;
        public readonly Vector2 clampOrigin, clampJointOrigin, clampRodOrigin, clampStandOrigin;

        public int clampDistance;

        public readonly Texture2D thermometerThread, pointer, tick, arrow;
        public readonly Vector2 thermoThreadOrigin, pointerOrigin, tickOrigin, arrowOrigin;

        public readonly Texture2D equipMoveButton, equipRotationButton;
        public readonly Vector2 equipMoveButtOrigin, equipRotButtOrigin;

        public readonly Texture2D[,] eyes = new Texture2D[2, 6];

        public readonly Texture2D[] bond = new Texture2D[7];
        public readonly Vector2[] bondOrigin = new Vector2[7];

        public readonly Texture2D electroShock, radSmoke;
        public readonly Vector2 radSmokeOrigin;

        public readonly Texture2D cross;

        // Fonts
        public readonly SpriteFont debugFont;
        public readonly SpriteFont symbolFont, thermometerFont;
        public readonly int symbolFontSize;

        // Audio objects
        public readonly AudioEngine audioEngine;
        public readonly SoundBank soundBank;
        public readonly WaveBank waveBank;
        public Cue menuCue, gameCue;

        // Color Effect
        BasicEffect simpleColorEffect;
        VertexDeclaration vertexDecl;


        /// <summary>
        /// Load GameContents
        /// </summary>
        public GameContent(GameComponent screenManager)
        {
            content = screenManager.Game.Content;
            Viewport viewport = screenManager.Game.GraphicsDevice.Viewport;
            viewportSize = new Vector2(viewport.TitleSafeArea.Width, viewport.TitleSafeArea.Height);

            storage = new Storage(Path.Combine(content.RootDirectory, "save data.bin"), MaxLevelIndex);

            blank = content.Load<Texture2D>("Graphics/blank");
            blackhole = content.Load<Texture2D>("Graphics/blackhole");
            gradient = content.Load<Texture2D>("Graphics/gradient");

            menuBackground = content.Load<Texture2D>("Graphics/menuBackground");
            credits = content.Load<Texture2D>("Graphics/credits");
            mainMenuTitle = content.Load<Texture2D>("Graphics/mainMenuTitle");
            levelMenuTitle = content.Load<Texture2D>("Graphics/levelMenuTitle");
            pauseTitle = content.Load<Texture2D>("Graphics/pauseTitle");

            levelUp = content.Load<Texture2D>("Graphics/levelUp");
            retry = content.Load<Texture2D>("Graphics/retry");

            for (int i = 0; i < levelButton.Length; i++)
                levelButton[i] = content.Load<Texture2D>("Graphics/levelButton" + i);

            for (int i = 0; i < labEquipButtons.Length; i++)
                labEquipButtons[i] = content.Load<Texture2D>("Graphics/labButton" + i);

            logo = content.Load<Texture2D>("Graphics/BitSitsGamesLogo");

            for (int i = 0; i < levelBackground.Length; i++)
                levelBackground[i] = content.Load<Texture2D>("Graphics/levelBackground" + i);

            for (int i = 0; i < tutorial.Length; i++)
                tutorial[i] = content.Load<Texture2D>("Graphics/tutorial" + i);

            labTable = content.Load<Texture2D>("Graphics/labTable");
            labNextButton = content.Load<Texture2D>("Graphics/labNextButton");
            labPrevButton = content.Load<Texture2D>("Graphics/labPrevButton");
            labClampButton = content.Load<Texture2D>("Graphics/labClampButton");
            labAtoomButton = content.Load<Texture2D>("Graphics/labAtoomButton");
            labEquipButton = content.Load<Texture2D>("Graphics/labEquipButton");
            labClearButton = content.Load<Texture2D>("Graphics/labClearButton");

            atom = new Texture2D[symbolCount];
            for (int i = 0; i < atom.Length; i++)
                atom[i] = content.Load<Texture2D>("Graphics/atom" + i);

            atomOrigin = new Vector2(atom[0].Width, atom[0].Height) / 2;
            shine = content.Load<Texture2D>("Graphics/atomShine");

            for (int i = 0; i < eyes.GetLength(0); i++)
                for (int j = 0; j < eyes.GetLength(1); j++)
                    eyes[i, j] = content.Load<Texture2D>("Graphics/eye" + i + j);

            for (int i = 0; i < bond.Length; i++)
            {
                bond[i] = content.Load<Texture2D>("Graphics/bond" + i);
                bondOrigin[i] = new Vector2(bond[i].Width, bond[i].Height) / 2;
            }

            bondOrigin[0].X = 0;

            electroShock = content.Load<Texture2D>("Graphics/shock");
            radSmoke = content.Load<Texture2D>("Graphics/radiationSmoke");
            radSmokeOrigin = new Vector2(radSmoke.Width, radSmoke.Height) / 2;

            for (int i = 0; i < equipment.GetLength(0); i++) for (int j = 0; j < equipment.GetLength(1); j++)
                    equipment[i, j] = content.Load<Texture2D>("Graphics/" + (EquipmentName)i + j);

            clamp = content.Load<Texture2D>("Graphics/clamp");
            clampOrigin = new Vector2(clamp.Width, clamp.Height) / 2;

            clampJoint = content.Load<Texture2D>("Graphics/clampJoint");
            clampJointOrigin = new Vector2(clampJoint.Width, clampJoint.Height) / 2;

            clampRod = content.Load<Texture2D>("Graphics/clampRod");
            clampRodOrigin = new Vector2(clampRod.Width, clampRod.Height) / 2;

            clampStand = content.Load<Texture2D>("Graphics/clampStand");
            clampStandOrigin = new Vector2(clampStand.Width / 2, clampStand.Height);

            equipMoveButton = content.Load<Texture2D>("Graphics/equipMoveButton");
            equipMoveButtOrigin = new Vector2(equipMoveButton.Width, equipMoveButton.Height) / 2;
            equipRotationButton = content.Load<Texture2D>("Graphics/equipRotationButton");
            equipRotButtOrigin = new Vector2(equipRotationButton.Width, equipRotationButton.Height) / 2;

            thermometerThread = content.Load<Texture2D>("Graphics/thermometerThread");
            thermoThreadOrigin = new Vector2(thermometerThread.Width, 0) / 2;
            pointer = content.Load<Texture2D>("Graphics/pointer");
            pointerOrigin = new Vector2(0, pointer.Height) / 2;

            tick = content.Load<Texture2D>("Graphics/tick");
            tickOrigin = new Vector2(tick.Width, tick.Height) / 2;
            arrow = content.Load<Texture2D>("Graphics/arrow");
            arrowOrigin = new Vector2(arrow.Width, arrow.Height) / 2;

            cross = content.Load<Texture2D>("Graphics/cross");

            debugFont = content.Load<SpriteFont>("Fonts/debugFont");
            symbolFontSize = 50;
            symbolFont = content.Load<SpriteFont>("Fonts/AklatanicTSO" + symbolFontSize);

            thermometerFont = content.Load<SpriteFont>("Fonts/thermometerFont");

            simpleColorEffect = new BasicEffect(screenManager.Game.GraphicsDevice, null);
            simpleColorEffect.VertexColorEnabled = true;
            vertexDecl = new VertexDeclaration(screenManager.Game.GraphicsDevice, VertexPositionColor.VertexElements);


            // Initialize audio objects.
            audioEngine = new AudioEngine("Content/Audio/Audio.xgs");
            soundBank = new SoundBank(audioEngine, "Content/Audio/Sound Bank.xsb");
            waveBank = new WaveBank(audioEngine, "Content/Audio/Wave Bank.xwb");

            menuCue = soundBank.GetCue("menu music"); menuCue.Play();
            gameCue = soundBank.GetCue("game music");

            AudioCategory defaultCategory = audioEngine.GetCategory("Default"),
                musicCategory = audioEngine.GetCategory("Music");

#if DEBUG
            defaultCategory.SetVolume(.1f); musicCategory.SetVolume(0.05f);
#else
            defaultCategory.SetVolume(1f); musicCategory.SetVolume(.5f);
#endif

            //Thread.Sleep(2000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            screenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload GameContents
        /// </summary>
        public void UnloadContent()
        {
            storage.Save();
            content.Unload();
        }
    }
}
