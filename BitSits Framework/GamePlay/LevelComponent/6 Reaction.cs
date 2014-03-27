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
using GameDataLibrary;

namespace BitSits_Framework
{
    class Reaction : LevelComponent
    {
        int index = 0;
        List<string> reactStr = new List<string>();
        List<Formula> reactFor = new List<Formula>();
        List<bool> ticked = new List<bool>();
        List<Vector2> tickPos = new List<Vector2>();

        public Reaction(GameContent gameContent, World world)
            : base(gameContent, world)
        {
            reactStr = gameContent.content.Load<List<string>>("Levels/reactions");
            GetNewReactionFormulas();
        }

        void GetNewReactionFormulas()
        {
            if (index == reactStr.Count) { IsLevelUp = true; return; }

            reactFor.Clear(); tickPos.Clear(); ticked.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (reactStr[index] != "")
                {
                    reactFor.Add(new Formula(reactStr[index], new Vector2(200 + 150 * i, 500), gameContent));
                    ticked.Add(false);
                }

                index += 1;
            }
        }

        public override bool UpdateNewFormula(Formula formula)
        {
            for (int i = 0; i < reactFor.Count; i++)
            {
                if (reactFor[i].strFormula == formula.strFormula && ticked[i] == false)
                {
                    tickPos.Add(new Vector2(200 + 150 * i, 500));
                    ticked[i] = true;

                    if (tickPos.Count == reactFor.Count) GetNewReactionFormulas();
                    return true;
                }
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.Draw(gameContent.menuBackground, Vector2.Zero, Color.White);

            Vector2 v = new Vector2(358, 300) - new Vector2(445, 300);
            spriteBatch.Draw(gameContent.equipment[(int)EquipmentName.clipboard, 0], v, Color.White);
            //spriteBatch.Draw(gameContent.equipment[(int)EquipmentName.clipboard, 1], v, Color.White);

            foreach (Formula f in reactFor) f.Draw(spriteBatch, gameTime);

            for (int i = 0; i < reactFor.Count - 2; i++)
                spriteBatch.DrawString(gameContent.symbolFont, "+", new Vector2(180 + 150 / 2 + 150 * 2 * i, 475),
                    Color.Gainsboro, 0, Vector2.Zero, 30f / gameContent.symbolFontSize, SpriteEffects.None, 1);

            spriteBatch.Draw(gameContent.arrow, new Vector2(350 + 150 / 2, 500) - gameContent.arrowOrigin,
                Color.Gainsboro);

            foreach (Vector2 u in tickPos)
                spriteBatch.Draw(gameContent.tick, u - gameContent.tickOrigin, new Color(Color.Gainsboro, .5f));

            spriteBatch.DrawString(gameContent.symbolFont,
                "\"Where are my Moolecules!!\n   Make them fast..\"",
                new Vector2(120, 130), Color.Gainsboro, (float)MathHelper.Pi / 20,
                Vector2.Zero, 35f / gameContent.symbolFontSize, SpriteEffects.None, 1);
        }
    }
}
