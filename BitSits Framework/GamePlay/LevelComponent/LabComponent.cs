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
using GameDataLibrary;

namespace BitSits_Framework
{
    class LabComponent
    {
        GameContent gameContent;
        public readonly int Height, Width;
        public readonly float EqScale, AtomScale;

        public LabComponent(GameContent gameContent, World world)
        {
            this.gameContent = gameContent;

            EqScale = 0.4f; AtomScale = 0.7f;

            Width = (int)(gameContent.viewportSize.X * 2 / EqScale);
            Height = (int)(gameContent.viewportSize.Y * 1 / EqScale);

            gameContent.clampDistance = (int)(gameContent.viewportSize.X / 2 / EqScale);

            PolygonShape ps = new PolygonShape();
            Body ground = world.CreateBody(new BodyDef());

            Vector2 pos = new Vector2(Width / 2, Height) / gameContent.scale;
            ps.SetAsBox(Width / 2 / gameContent.scale, (float)gameContent.labTable.Height 
                / gameContent.scale, pos, 0);
            ground.CreateFixture(ps, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gameContent.menuBackground, new Rectangle(0, 0, Width, Height), Color.White);

            for (int i = 0; i <= (Width / 800 + 1); i++)
            {
                Vector2 pos = new Vector2(i * gameContent.clampDistance + gameContent.clampDistance / 2,
                    Height - gameContent.labTable.Height);

                spriteBatch.Draw(gameContent.clampStand, pos - gameContent.clampStandOrigin, Color.White);
            }

            for (int i = 0; i <= (Width / gameContent.labTable.Width + 1); i++)
            {
                spriteBatch.Draw(gameContent.labTable, new Vector2(i * gameContent.labTable.Width,
                    Height - gameContent.labTable.Height), Color.White);
            }
        }
    }
}
