// ***********************************************************************
// Assembly         : RIAP
// Author           : Jai Brown
// Created          : 14-11-2020
//
// Last Modified By : Jai Brown
// Last Modified On : 14-11-2020
// ***********************************************************************
// <copyright file="InverseBooleanConverter.cs" company="Jai Brown">
//     Copyright (c) Jai Brown. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Usings

using System;
using System.Globalization;
using System.Windows.Data;

#endregion Usings

namespace JaINTP.RIAP.Utilities.Converters
{
    /// <summary>
    ///     Handles the conversion from a boolean value to its opposite value provided by the converter parameter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public sealed class InverseBooleanConverter : IValueConverter
    {
        #region Public Methods

        /// <summary>
        ///     Converts the boolean value to its opposite value;
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The CultureInfo.</param>
        /// <returns>The <see cref="object" />.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target has to be of type boolean.");

            return !(bool)value;
        }

        /// <summary>
        ///     Converts the boolean value to its opposite value;
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The CultureInfo.</param>
        /// <returns>The <see cref="object" />.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotSupportedException();
        }

        #endregion Public Methods
    }
}