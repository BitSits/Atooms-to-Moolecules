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
using Box2D.XNA;

namespace BitSits_Framework
{
    class Tutorial : LevelComponent
    {
        List<string> instructions;
        int index = 0;

        string msg, tutFormula;

        public Tutorial(GameContent gameContent, World world)
            : base(gameContent, world)
        {
            instructions = gameContent.content.Load<List<string>>("Levels/tutorial");

            NextMsg();
        }

        public override bool UpdateNewFormula(Formula formula)
        {
            if (formula.strFormula == tutFormula)
            {
                if (index == instructions.Count) IsLevelUp = true;
                else NextMsg();

                return true;
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            int maxAtoms = 4 * (index / 2);

            if (maxAtoms <= atoms.Count) return;

            int[] atomsPresent = new int[gameContent.symbolCount];
            foreach (Atom a in atoms) atomsPresent[(int)a.symbol] += 1;

            for (int i = 1; i <= (index / 2); i++)
            {
                if (i >= atomsPresent.Length) break;

                for (int j = 0; j < 4 - atomsPresent[i]; j++)
                {
                    atoms.Add(new Atom((Symbol)i, entryPoint, gameContent, world));
                }
            }
        }

        void NextMsg()
        {
            msg = instructions[index]; tutFormula = instructions[index + 1]; index += 2;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.DrawString(gameContent.symbolFont, msg, new Vector2(20, 250), Color.Gainsboro,
                -(float)Math.PI / 40, Vector2.Zero, 30f / gameContent.symbolFontSize, SpriteEffects.None, 1);

            spriteBatch.DrawString(gameContent.symbolFont, "Make", new Vector2(50, 390),
                Color.WhiteSmoke, -(float)Math.PI / 20f, Vector2.Zero, 30f / gameContent.symbolFontSize,
                SpriteEffects.None, 1);

            spriteBatch.Draw(gameContent.tutorial[index / 2 - 1], new Vector2(150, 360), null,
                Color.Gainsboro, 0, Vector2.Zero, .65f, SpriteEffects.None, 1);
        }
    }
}
