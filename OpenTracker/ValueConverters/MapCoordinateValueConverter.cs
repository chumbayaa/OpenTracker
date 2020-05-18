﻿using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Layout;
using OpenTracker.Models.Enums;
using OpenTracker.Views;
using System;
using System.Globalization;

namespace OpenTracker.ValueConverters
{
    public class MapCoordinateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            
            var mapPoint = (ValueTuple<MapID, Point>)value;
            
            double lightWorldXOffset = 0;
            double lightWorldYOffset = 0;
            double darkWorldXOffset = 0;
            double darkWorldYOffset = 0;

            switch (MainWindow.MapOrientationStatic)
            {
                case Orientation.Horizontal:
                    lightWorldXOffset = 10;
                    lightWorldYOffset = 20;
                    darkWorldXOffset = 2037;
                    darkWorldYOffset = 20;
                    break;
                case Orientation.Vertical:
                    lightWorldXOffset = 20;
                    lightWorldYOffset = 10;
                    darkWorldXOffset = 20;
                    darkWorldYOffset = 2037;
                    break;
            }

            if ((string)parameter == "X")
            {
                return mapPoint.Item1 switch
                {
                    MapID.LightWorld => lightWorldXOffset + mapPoint.Item2.X,
                    MapID.DarkWorld => darkWorldXOffset + mapPoint.Item2.X,
                    _ => null,
                };
            }
            else
            {
                return mapPoint.Item1 switch
                {
                    MapID.LightWorld => lightWorldYOffset + mapPoint.Item2.Y,
                    MapID.DarkWorld => darkWorldYOffset + mapPoint.Item2.Y,
                    _ => null,
                };
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
