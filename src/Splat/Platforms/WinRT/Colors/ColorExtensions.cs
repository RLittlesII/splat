﻿using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Splat.Platforms.WinRT.Colors
{
    /// <summary>
    /// Provides extension methods for interacting with colors, to and from the XAML colors.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts a <see cref="System.Drawing.Color"/> to a XAML native color.
        /// </summary>
        /// <param name="value">The System.Drawing.Color to convert.</param>
        /// <returns>A native XAML color.</returns>
        public static Color ToNative(this System.Drawing.Color value)
        {
            return Color.FromArgb(value.A, value.R, value.G, value.B);
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Color"/> into the XAML <see cref="SolidColorBrush"/>.
        /// </summary>
        /// <param name="value">The color to convert.</param>
        /// <returns>The <see cref="SolidColorBrush"/> generated.</returns>
        public static SolidColorBrush ToNativeBrush(this System.Drawing.Color value)
        {
            return new SolidColorBrush(value.ToNative());
        }

        /// <summary>
        /// Converts a XAML color into the XAML <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="value">The color to convert.</param>
        /// <returns>The <see cref="System.Drawing.Color"/> generated.</returns>
        public static System.Drawing.Color FromNative(this Color value)
        {
            return System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
        }
    }
}