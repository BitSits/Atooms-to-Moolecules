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
    public enum EquipmentName // in layer order
    {
        electrodes, thermometer, pHscale, funnel, testTube, conicalFlask, measuringCylinder, beaker, clipboard,
    }

    public enum BonusType
    {
        None, Ring, Hydrogen
    }

    public class EquipmentDetails
    {
        public EquipmentName EquipmentName;
        public Vector2 Position;
        public float RotationInDeg;
        public bool IsClamped;
    }

    public class LevelData
    {
        public int MaxAtoms;

        public int[] AtomProbability;

        public Vector2 Entry;

        public BonusType BonusType;

        public List<Vector2> ContinuousBoundry;

        public List<EquipmentDetails> EquipmentDetails;
    }
}
