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
    class Electro : LevelComponent
    {
        int a = 6, max = 16;

        public Electro(GameContent gameContent, World world)
            : base(gameContent, world) { }

        public override bool UpdateNewFormula(Formula formula)
        {
            int total = 0;
            for (int i = 0; i < formula.atomCount.Length; i++)
            {
                total += formula.atomCount[i];
            }

            if (total >= a)
            {
                if (a >= max) { IsLevelUp = true; return true; }

                a += 2; return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.DrawString(gameContent.symbolFont, a.ToString(), new Vector2(330, 315), Color.Gainsboro,
                -(float)Math.PI / 20, Vector2.Zero, 50f / gameContent.symbolFontSize, SpriteEffects.None, 1);
        }
    }
}
