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
    class Bond
    {
        public readonly int numberOfBonds;
        public Atom atom, other;
        public Joint shortJoint, midJoint;

        float jointAngle;
        public float finalJointAngle;

        const float factor = 10f;

        GameContent gameContent;

        public Bond(Atom atom, Atom other, Joint shortJoint, Joint midJoint, 
            int numberOfBonds, GameContent gameContent)
        {
            this.numberOfBonds = numberOfBonds;
            this.atom = atom;
            this.other = other;
            this.shortJoint = shortJoint;
            this.midJoint = midJoint;
            this.gameContent = gameContent;

            Vector2 localAnchor;
            if (atom.body == shortJoint.GetBodyA())
            {
                localAnchor = atom.body.GetLocalPoint(shortJoint.GetAnchorA());
                midJoint.SetUserData(this);
            }
            else
                localAnchor = atom.body.GetLocalPoint(shortJoint.GetAnchorB());

            float angle = (float)Math.Atan2(localAnchor.Y, localAnchor.X);
            if (angle < 0) angle = 2 * (float)Math.PI + angle;

            jointAngle = finalJointAngle = angle;
        }

        public void Update(GameTime gameTime)
        {
            if (jointAngle != finalJointAngle)
            {
                if (jointAngle > finalJointAngle)
                    jointAngle = Math.Max(finalJointAngle,
                        jointAngle - (float)gameTime.ElapsedGameTime.TotalSeconds * factor);

                else if (jointAngle < finalJointAngle)
                    jointAngle = Math.Min(finalJointAngle,
                        jointAngle + (float)gameTime.ElapsedGameTime.TotalSeconds * factor);

                Vector2 localAnchor = new Vector2((float)Math.Cos(jointAngle),
                    (float)Math.Sin(jointAngle)) * gameContent.atomRadius / gameContent.scale;

                if (atom.body == shortJoint.GetBodyA())
                    ((DistanceJoint)shortJoint).SetAnchorA(localAnchor);
                else ((DistanceJoint)shortJoint).SetAnchorB(localAnchor);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 a = midJoint.GetAnchorA() * gameContent.scale;
            Vector2 b = midJoint.GetAnchorB() * gameContent.scale;

            DrawBond(spriteBatch, gameContent, a, b, numberOfBonds);
        }

        public static void DrawBond(SpriteBatch spriteBatch, GameContent gameContent,
            Vector2 a, Vector2 b, int numberOfBonds)
        {
            float scale = Vector2.Distance(a, b) / (gameContent.bondOrigin[numberOfBonds].X * 2);
            float rotation = (float)Math.Atan((b.Y - a.Y) / (b.X - a.X));

            spriteBatch.Draw(gameContent.bond[numberOfBonds], (a + b) / 2, null, Color.White, rotation,
                gameContent.bondOrigin[numberOfBonds], new Vector2(scale, 1), SpriteEffects.None, 1);
        }
    }
}
