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
using GameDataLibrary;

namespace BitSits_Framework
{
    /// <summary>
    /// Radio Active, Hydrogen, Oxygen, Nitrogen, Carbon, ElementX
    /// </summary>
    enum Symbol { Ra, H, O, N, C, X, }

    class Formula
    {
        Vector2 position;
        List<Vector2> pos = new List<Vector2>();

        Vector2 origin;
        GameContent gameContent;
        float charSize = 20f;

        const float MaxTime = 3.0f;
        float time = 0.0f;

        public readonly string strFormula;
        public readonly int[] atomCount;
        public readonly int twiceNumberOfBonds, numberOfRings, score;

        public string strScore = "";

        public bool IsActive = true;

        // For Tutorial Level
        public Formula(string strFormula, Vector2 position, GameContent gameContent)
        {
            this.position = position;
            this.gameContent = gameContent;

            this.strFormula = strFormula;

            time = float.NegativeInfinity;

            charSize = 20;
            SetPos();
        }

        public Formula(int[] atomCount, int twiceNumberOfBonds, int numberOfRings, BonusType bonusType,
            Vector2 position, GameContent gameContent)
        {
            this.position = position;
            this.gameContent = gameContent;
            this.twiceNumberOfBonds = twiceNumberOfBonds;
            this.numberOfRings = numberOfRings;

            this.atomCount = new int[gameContent.symbolCount];
            atomCount.CopyTo(this.atomCount, 0);

            int bonus = 1;

            if (bonusType == BonusType.Ring)
                bonus = numberOfRings + 1;
            else if (bonusType == BonusType.Hydrogen)
                bonus = atomCount[(int)Symbol.H];

            score = twiceNumberOfBonds * bonus; strScore = "+" + twiceNumberOfBonds;
            if (bonus > 1) strScore += " x" + bonus;

            Symbol[] symbolPref = new Symbol[atomCount.Length]; // C_H_N_O_X_Ra
            symbolPref[0] = Symbol.C;
            symbolPref[1] = Symbol.H;
            symbolPref[2] = Symbol.N;
            symbolPref[3] = Symbol.O;
            symbolPref[4] = Symbol.X;
            symbolPref[5] = Symbol.Ra;

            for (int i = 0; i < symbolPref.Length; i++)
            {
                int count = atomCount[(int)symbolPref[i]];
                if (count > 0)
                {
                    strFormula += symbolPref[i];
                    if (count > 1) strFormula += count;
                }
            }

            SetPos();
        }

        void SetPos()
        {
            float x = 0, y = 0;
            for (int i = 0; i < strFormula.Length; i++)
            {
                pos.Add(new Vector2(x, strFormula[i] >= 'A' ? 0 : y / 2));
                Vector2 size = gameContent.symbolFont.MeasureString(strFormula[i].ToString()) * charSize
                    / gameContent.symbolFontSize;
                x += size.X;
                y = size.Y;
                origin.X += size.X;
            }

            origin.Y = y * 1.5f;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (time > MaxTime) { IsActive = false; return; }

            for (int i = 0; i < strFormula.Length; i++)
            {
                spriteBatch.DrawString(gameContent.symbolFont, strFormula[i].ToString(),
                    position - origin / 2 + pos[i], Color.White, 0, Vector2.Zero,
                    charSize / gameContent.symbolFontSize, SpriteEffects.None, 1);
            }

            //spriteBatch.DrawString(gameContent.symbolFont, strFormula, position, Color.White, 0,
            //    Vector2.Zero, charSize / gameContent.symbolFontSize, SpriteEffects.None, 1);

            // Score
            spriteBatch.DrawString(gameContent.symbolFont, strScore,
                position + new Vector2(0, origin.Y * 0.5f), Color.White, 0,
                Vector2.Zero, 14f / gameContent.symbolFontSize, SpriteEffects.None, 1);
        }
    }
}
