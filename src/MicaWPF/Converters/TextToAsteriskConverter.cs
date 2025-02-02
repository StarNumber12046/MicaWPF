﻿using System.Windows.Data;

namespace MicaWPF.Converters;

/// <summary>
/// Converts the current text to asterisks.
/// </summary>
public sealed class TextToAsteriskConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return new string('*', value?.ToString()?.Length ?? 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
