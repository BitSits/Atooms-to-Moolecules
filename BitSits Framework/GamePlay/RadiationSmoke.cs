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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    class RadiationSmoke
    {
        GameContent gameContent;
        Atom atom;
        Vector2 atomPrevPosition;

        const int MaxParticle = 8, VariedAngle = 30;
        const float MaxParticlePos = 60;

        float[] particlePos = new float[MaxParticle], particleRotation = new float[MaxParticle];
        
        public RadiationSmoke(GameContent gameContent, Atom atom)
        {
            this.gameContent = gameContent;
            this.atom = atom;

            for (int i = 0; i < MaxParticle; i++)
                particlePos[i] = MaxParticlePos / (MaxParticle - 1) * i;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 atomPosition = atom.body.Position * gameContent.scale;
            float theta = (float)Math.Atan2(atomPrevPosition.Y - atomPosition.Y, atomPrevPosition.X - atomPosition.X)
                + (float)Math.PI;

            spriteBatch.Draw(gameContent.radSmoke, atomPosition, null, Color.White, 0, gameContent.radSmokeOrigin,
                    1, SpriteEffects.None, 1);

            for (int i = 0; i < MaxParticle; i++)
            {
                particlePos[i] += 4;
                if (particlePos[i] > MaxParticlePos)
                {
                    particlePos[i] = 0;
                    particleRotation[i] = theta
                        + (float)(gameContent.random.Next(2 * VariedAngle) - VariedAngle) / 180 * (float)Math.PI;
                }

                Vector2 localRadPos = atomPosition - new Vector2((float)Math.Cos(particleRotation[i]),
                    (float)Math.Sin(particleRotation[i])) * particlePos[i];
                spriteBatch.Draw(gameContent.radSmoke, localRadPos, null,
                    new Color(Color.White, 1f - particlePos[i] / MaxParticlePos), 0, gameContent.radSmokeOrigin,
                    MathHelper.Clamp(1f - particlePos[i] / MaxParticlePos, 0.2f, 0.9f), SpriteEffects.None, 1);
            }

            atomPrevPosition = atomPosition;
        }
    }
}
