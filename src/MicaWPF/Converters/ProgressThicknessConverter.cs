﻿using System.Windows.Data;

namespace MicaWPF.Converters;

/// <summary>
/// Converts the current double to a thickness value for the <see cref="MicaWPF.Controls.ProgressRing"/>.
/// </summary>
public sealed class ProgressThicknessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return value is double height ? height / 11 : (object)12.0d;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
