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
    class pH : LevelComponent
    {
        float  value, currentValue;

        public pH(GameContent gameContent, World world)
            : base(gameContent, world)
        {
            value = 9.9f; currentValue = 2.1f;
        }

        public override bool UpdateNewFormula(Formula formula)
        {
            if (formula.atomCount[(int)Symbol.H] > 0)
            {
                currentValue = Math.Min(currentValue + formula.score * 0.01f, pHscale.MaxPhValue);
                return true;
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds * 1f;

            if (value > currentValue) value = Math.Max(value - dt, currentValue);

            else if (value < currentValue) value = Math.Min(value + dt, currentValue);

            else currentValue = Math.Max(currentValue - (float)gameTime.ElapsedGameTime.TotalSeconds * 0.03f, 
                pHscale.MinPhValue);

            if (pHscale != null) pHscale.pHvalue = value;

            if (value == pHscale.MaxPhValue) IsLevelUp = true;
            else if (value == pHscale.MinPhValue) ReloadLevel = true;

            
            int hydrogenCount = 0;
            for (int i = 0; i < atoms.Count; i++) if (atoms[i].symbol == Symbol.H) hydrogenCount += 1;

            for (int i = (10 - hydrogenCount) - 1; i >= 0; i--)
                atoms.Add(new Atom(Symbol.H, entryPoint, gameContent, world));

            base.Update(gameTime);
        }
    }
}
