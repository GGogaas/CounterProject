using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counter.models;

public class HexToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is string hex ? Color.FromArgb(hex) : Colors.Transparent;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value is Color c ? c.ToArgbHex() : "#000000";
}

static class ColorExtensions
{
    public static string ToArgbHex(this Color c)
        => $"#{(byte)(c.Alpha * 255):X2}{(byte)(c.Red * 255):X2}{(byte)(c.Green * 255):X2}{(byte)(c.Blue * 255):X2}";
}
