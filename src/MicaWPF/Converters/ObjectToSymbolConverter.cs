﻿using System.Windows.Data;
using MicaWPF.Extensions;
using MicaWPF.Symbols;

namespace MicaWPF.Converters;

internal class ObjectToSymbolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is FluentSystemIcons.Regular symbol)
            return symbol;

        if (value is FluentSystemIcons.Filled symbolFilled)
            return symbolFilled.Swap();

        return FluentSystemIcons.Regular.Empty;
    }

    /// <summary>
    /// Not Implemented.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
