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

namespace GameDataLibrary
{
    public class Box
    {
        public float Width, Height, RotationInDeg;
        public Vector2 Position;
    }

    public class Circle
    {
        public float Diameter;
        public Vector2 Position;
    }

    public class ClampData
    {
        public bool ClampEnabled;
        public float RightClampPositionX;
        public float RotationInDeg;
    }

    public class EquipmentData
    {
        public Vector2 TopLeftVertex, Origin, RotationButtonPosition;

        public ClampData ClampData;

        public List<Vector2> ContinuousEdges;

        public List<Box> Boxes;

        public List<Circle> Circles;
    }
}
