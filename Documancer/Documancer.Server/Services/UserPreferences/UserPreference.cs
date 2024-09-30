using System.Drawing;

namespace Documancer.Server.Services.UserPreferences
{
    public class UserPreference
    {
        public static readonly List<string> PrimaryColors = new()
    {
        "#2d4275",
        "#6A1B9A",
        "#4CAF50",
        "#FF9800",
        "#F44336",
        "#FF69B4"
    };

        public static readonly List<string> DarkPrimaryColors = new()
    {
        "#0077b6",
        "#a541be",
        "#388E3C",
        "#FB8C00",
        "#ca322d",
        "#cf2d86",
    };

        /// <summary>
        ///     Set the direction layout of the docs to RTL or LTR. If true RTL is used
        /// </summary>
        public bool RightToLeft { get; set; }

        /// <summary>
        ///     If true DarkTheme is used. LightTheme otherwise
        /// </summary>
        public bool IsDarkMode { get; set; }

        public string PrimaryColor { get; set; } = "#2d4275";
        public string DarkPrimaryColor { get; set; } = "#8b9ac6";
        public string PrimaryDarken => AdjustBrightness(PrimaryColor, 0.8);
        public string PrimaryLighten => AdjustBrightness(PrimaryColor, 0.7);
        public string SecondaryColor { get; set; } = "#ff4081ff";
        public double BorderRadius { get; set; } = 4;
        public double DefaultFontSize { get; set; } = 0.8125;
        public double LineHeight => Math.Min(1.7, Math.Max(1.3, 1.5 * (DefaultFontSize / 0.875)));
        public double LetterSpacing => 0.00938 * (DefaultFontSize / 0.875);

        public double Body1FontSize => DefaultFontSize;
        public double Body1LineHeight => LineHeight;
        public double Body1LetterSpacing => LetterSpacing;
        public double Body2FontSize => DefaultFontSize - 0.0625;
        public double Body2LineHeight => LineHeight;
        public double Body2LetterSpacing => LetterSpacing;
        public double ButtonFontSize => DefaultFontSize;
        public double ButtonLineHeight => 1.75 * (DefaultFontSize / 0.875);
        public double CaptionFontSize => DefaultFontSize + 0.0625;
        public double CaptionLineHeight => Math.Min(1.8, Math.Max(1.4, 1.66 * (DefaultFontSize / 0.75)));
        public double OverlineFontSize => DefaultFontSize - 0.0625;
        public double Subtitle1FontSize => DefaultFontSize + 0.125;
        public double Subtitle2FontSize => DefaultFontSize;
        public DarkLightMode DarkLightTheme { get; set; }

        private string AdjustBrightness(string hexColor, double factor)
        {
            if (string.IsNullOrWhiteSpace(hexColor))
                throw new ArgumentException("Color code cannot be null or empty.", nameof(hexColor));

            var color = ColorTranslator.FromHtml(hexColor);

            double h, s, l;
            ColorToHsl(color, out h, out s, out l);

            l = Math.Clamp(l * factor, 0.0, 1.0);

            var adjustedColor = HslToColor(h, s, l);

            return ColorTranslator.ToHtml(adjustedColor);
        }

        private void ColorToHsl(System.Drawing.Color color, out double h, out double s, out double l)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            h = s = l = (max + min) / 2.0;

            if (max == min)
            {
                h = s = 0.0;
            }
            else
            {
                double delta = max - min;
                s = l > 0.5 ? delta / (2.0 - max - min) : delta / (max + min);

                if (max == r)
                    h = (g - b) / delta + (g < b ? 6.0 : 0.0);
                else if (max == g)
                    h = (b - r) / delta + 2.0;
                else
                    h = (r - g) / delta + 4.0;

                h /= 6.0;
            }
        }

        private System.Drawing.Color HslToColor(double h, double s, double l)
        {
            double r, g, b;

            if (s == 0.0)
            {
                r = g = b = l;
            }
            else
            {
                Func<double, double, double, double> hue2rgb = (p, q, t) =>
                {
                    if (t < 0.0) t += 1.0;
                    if (t > 1.0) t -= 1.0;
                    if (t < 1.0 / 6.0) return p + (q - p) * 6.0 * t;
                    if (t < 1.0 / 2.0) return q;
                    if (t < 2.0 / 3.0) return p + (q - p) * (2.0 / 3.0 - t) * 6.0;
                    return p;
                };

                double q = l < 0.5 ? l * (1.0 + s) : l + s - l * s;
                double p = 2.0 * l - q;

                r = hue2rgb(p, q, h + 1.0 / 3.0);
                g = hue2rgb(p, q, h);
                b = hue2rgb(p, q, h - 1.0 / 3.0);
            }

            return System.Drawing.Color.FromArgb(
                (int)Math.Round(r * 255.0),
                (int)Math.Round(g * 255.0),
                (int)Math.Round(b * 255.0)
            );
        }
    }

    public enum DarkLightMode
    {
        System = 0,
        Light = 1,
        Dark = 2
    }
}