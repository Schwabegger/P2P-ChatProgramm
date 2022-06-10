// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Basics.Converter
{
    /// <summary>
    /// Uses the selected index of an ItemsControl and converts it to visible if it is >= 0
    /// </summary>
    internal class ListEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int number)
                return number >= 0 ? Visibility.Visible : Visibility.Collapsed;
            else
                return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}