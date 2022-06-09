// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Supporters:
// ඞ Hackl Tobias
// ඞ Ratzenböck Peter

using Basics.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Basics.Converter
{
    internal class GroupChatButtonsVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ChatRoom chatroom)
                return chatroom is Groupchat ? Visibility.Visible : Visibility.Collapsed;
            else
                return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
