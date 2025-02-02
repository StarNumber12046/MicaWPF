﻿using System.Windows.Data;

namespace MicaWPF.Converters;

/// <summary>
/// Null to bool.
/// </summary>
public sealed class IsNullConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return (value != null && value.ToString() == string.Empty) || value is null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
