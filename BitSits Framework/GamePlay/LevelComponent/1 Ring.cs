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

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;

namespace BitSits_Framework
{
    class Ring : LevelComponent
    {
        int numberOfRings = 0, totalRings = 8;

        public Ring(GameContent gameContent, World world)
            : base(gameContent, world) { }

        public override bool UpdateNewFormula(Formula formula)
        {
            if (formula != null && formula.numberOfRings > 0)
            {
                numberOfRings += 1;

                if (numberOfRings == totalRings) IsLevelUp = true;

                return true;
            }            

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.DrawString(gameContent.symbolFont, totalRings.ToString(), new Vector2(100, 244),
                Color.Gainsboro, (float)MathHelper.Pi / 20, Vector2.Zero, 40f / gameContent.symbolFontSize,
                SpriteEffects.None, 1);

            if (numberOfRings > 0)
                spriteBatch.DrawString(gameContent.symbolFont,
                    numberOfRings + " of " + totalRings, new Vector2(100, 440),
                    Color.Gainsboro, -(float)MathHelper.Pi / 20, Vector2.Zero, 40f / gameContent.symbolFontSize,
                    SpriteEffects.None, 1);

            if (numberOfRings == 0)
                spriteBatch.Draw(gameContent.tutorial[5], new Vector2(50, 400), null, Color.Gainsboro,
                    -0.008f, Vector2.Zero, 0.6f, SpriteEffects.None, 1);
        }
    }
}
