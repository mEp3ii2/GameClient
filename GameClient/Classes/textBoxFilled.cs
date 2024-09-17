/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: textBoxFilled
Purpose: Implements a value converter that checks whether a TextBox input is filled (non-empty and non-whitespace).
Notes: This converter is used in data bindings to enable or disable UI elements based on whether a TextBox has valid input.
*/

using System;
using System.Globalization;
using System.Windows.Data;

namespace GameClient
{
    public class textBoxFilled : IValueConverter
    {
        /**
        Method: Convert
        Imports: object value (The value to be converted), Type targetType, object parameter, CultureInfo culture
        Exports: object (Returns a boolean indicating whether the TextBox contains non-whitespace text)
        Notes: This method checks if the input value is a non-empty, non-whitespace string.
        Algorithm: The method casts the value to a string, and then uses the `IsNullOrWhiteSpace` function to determine if the string is empty or contains only whitespace.
        */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrWhiteSpace(value as string);
        }

        /**
        Method: ConvertBack
        Imports: object value, Type targetType, object parameter, CultureInfo culture
        Exports: None
        Notes: The ConvertBack method is not implemented because it is not required in this use case.
        Algorithm: This method throws a NotImplementedException since the conversion back from boolean to string is not needed in the application.
        */
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
