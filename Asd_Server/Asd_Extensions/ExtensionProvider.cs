using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Asd_Extensions
{
    public static class ExtensionProvider
    {
        public static string GetCallerMethodName([CallerMemberName]string name = "")
        {
            try
            {
                return name.Replace("Async", "");
            }
            catch
            {
                return name;
            }
        }

        public static string ToTrimString(this string value)
        {
            try
            {
                return string.IsNullOrEmpty(value)
                    ? null
                    : value.Trim()
                        .Replace("\t", "")
                        .Replace("\n", "")
                        .Replace("\r", "");
            }
            catch { return string.Empty; }
        }

        public static string ToTabString(this string value)
        {
            try
            {
                value = Environment.NewLine + value;
                return value.Replace(Environment.NewLine, Environment.NewLine + "\t");
            }
            catch { return string.Empty; }
        }

        public static string ToTabString(this string value, int count)
        {
            try
            {
                var tabs = new List<string>();
                for (var i = 1; i <= count; i++) tabs.Add("\t");
                value = Environment.NewLine + value;
                return value.Replace(Environment.NewLine, Environment.NewLine + string.Join("", tabs));
            }
            catch { return string.Empty; }
        }
        
        public static string Base64Encode(string text)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(text);
                return Convert.ToBase64String(bytes);
            }
            catch { return string.Empty; }
        }

        public static string Base64Decode(string base64String)
        {
            try
            {
                var bytes = Convert.FromBase64String(base64String);
                return Encoding.UTF8.GetString(bytes);
            }
            catch { return string.Empty; }
        }

        public static IEnumerable<Asd_FontFimily> GetFontFamilies()
        {
            return new List<Asd_FontFimily>
            {
                new Asd_FontFimily { Value = "Arial", Text = "Arial" },
                new Asd_FontFimily { Value = "Verdana", Text = "Verdana" }
            };
        }

        public static string ToFontFamily(this string value)
        {
            return GetFontFamilies()?.FirstOrDefault(i => i.Value == value)?.Text ??
                   GetFontFamilies()?.FirstOrDefault()?.Value;
        }

        public static IEnumerable<Asd_FontSize> GetFontSizes()
        {
            return new List<Asd_FontSize>
            {
                new Asd_FontSize { Value = "10", Text = "10px" },
                new Asd_FontSize { Value = "20", Text = "20px" }
            };
        }

        public static string ToFontSize(this string value)
        {
            return GetFontSizes()?.FirstOrDefault(i => i.Value == value)?.Text ??
                   GetFontSizes()?.FirstOrDefault()?.Value;
        }

        public static string ToColor(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "#FFFFFF";
            try
            {
                var colorConverter = new ColorConverter();
                var color = (Color)(colorConverter.ConvertFromString(value) ?? "#FFFFFF");
                return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            }
            catch
            {
                return "#FFFFFF";
            }
        }
    }

    public class Asd_FontFimily
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class Asd_FontSize
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
