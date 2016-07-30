﻿using System;

namespace DL.Core.Geospatial
{
    public class CoordinateFactory
    {
        public CoordinateBase Make(CoordinateType coordinateType)
        {
            switch (coordinateType)
            {
                case CoordinateType.Latitude:
                    return new Latitude();
                case CoordinateType.Longitude:
                    return new Longitude();
                default:
                    throw new ArgumentOutOfRangeException("coordinateType");
            }
        }
    }
}
