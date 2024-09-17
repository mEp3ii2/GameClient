/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: AllFieldsFilledConverter
Purpose: Implements an IMultiValueConverter that checks if all bound input fields are filled with non-empty strings. It is used in the XAML UI to enable/disable buttons or controls based on user input.
Notes: This converter is mainly used for checking multiple input fields in forms and ensuring that all necessary fields have valid entries before enabling actions such as submitting a form.
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace GameClient
{
    internal class AllFieldsFilledConverter : IMultiValueConverter
    {
        /**
        Method: Convert
        Imports: object[] values (Array of bound values), Type targetType (The target type), object parameter (Additional parameters), CultureInfo culture (Cultural info for conversion)
        Exports: object (Returns a boolean value indicating whether all input fields are filled)
        Notes: Checks if all values in the provided array are non-null, non-empty strings. Returns true if all conditions are met, otherwise returns false.
        Algorithm: Iterates over the input values and verifies each one is a non-null and non-empty string using LINQ's All method.
        */
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.All(value => value is string str && !string.IsNullOrEmpty(str));
        }

        /**
        Method: ConvertBack
        Imports: object value (The value to convert back), Type[] targetTypes (The target types), object parameter (Additional parameters), CultureInfo culture (Cultural info for conversion)
        Exports: object[] (Throws a NotImplementedException)
        Notes: This method is not implemented because the conversion back is not necessary in this case.
        Algorithm: Simply throws a NotImplementedException since backward conversion is not required for this converter.
        */
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
