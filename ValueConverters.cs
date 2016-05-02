using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

using System.Windows.Media;
using System.Collections;
using System.Data;

using DLeh.Util;
using DLeh.Util.Extensions;

namespace DLeh.Util.MVVM
{

    public class Int32ToByteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (byte)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return (int)value;
        }
    }

    public class SelectedIndexIsAtLeastZero : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value > -1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            throw new NotImplementedException();
            //   return (int)value;
        }
    }

    public class NullIsFalseNonNullTrue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringIsEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string objValue = (string)value;

            if (string.IsNullOrWhiteSpace(objValue))
                return Visibility.Collapsed;

            return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class EnumToDescriptionAttributeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Enum)value).GetDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // http://stackoverflow.com/questions/24172370/operator-cannot-be-applied-to-operands-of-type-system-enum-and-system-enu
    public class EnumFlagConverter : IValueConverter
    {
        public Enum CurrentValue { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var theEnum = value as Enum;
            CurrentValue = theEnum;
            return theEnum.HasFlag(parameter as Enum);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var theEnum = parameter as Enum;
            if (CurrentValue.HasFlag(theEnum))
                CurrentValue = CurrentValue.And(theEnum.Not());
            else
                CurrentValue = CurrentValue.Or(theEnum);
            return CurrentValue;
        }
    }

    public class MultiArgumentConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.ToArray();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ExpanderToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToBoolean(value)) return parameter;
            return null;
        }
    }

    public class EnumMatchToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            return checkValue.Equals(targetValue,
                     StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            bool useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue)
                return Enum.Parse(targetType, targetValue);

            return null;
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class ParameterizedBoolToVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public ParameterizedBoolToVisibilityConverter()
        {
            // set defaults
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return null;
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (Equals(value, TrueValue))
                return true;
            if (Equals(value, FalseValue))
                return false;
            return null;
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            //this will throw an error if it's wrong anyway. muting this line allows us to use it in 
            // a valueconverter group
            //if (targetType != typeof(bool))
            //    throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    //devon - changed this from bool->bool? to bool?-> bool 12/23/2013
    [ValueConversion(typeof(bool?), typeof(bool))]
    public class InverseNullableBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool)) //changed from bool?
                throw new InvalidOperationException("The target must be bool?");

            return !((bool?)(value) ?? false);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool) && Nullable.GetUnderlyingType(targetType).Name != "Boolean")
                throw new InvalidOperationException("The target must be bool?");

            return ((bool?)(value));
        }

        #endregion
    }

    public class CutoffConverter : IValueConverter
    {
        public int Cutoff { get; set; }

        public object Convert(object obj, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)obj) > Cutoff;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsLessThanCutoffConverter : IValueConverter
    {
        public int Cutoff { get; set; }

        public object Convert(object obj, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)obj) < Cutoff;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    [ValueConversion(typeof(DateTime?), typeof(DateTime))]
    public class NullDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var nullable = (DateTime?)value;
            if (nullable == DateTime.MinValue)
                return (DateTime?)null;
            return (DateTime?)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((DateTime)value == DateTime.MinValue)
                return (DateTime?)null;
            return (DateTime?)value;
        }
    }

    public class CombiningConverter : IValueConverter
    {
        public IValueConverter Converter1 { get; set; }
        public IValueConverter Converter2 { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object convertedValue = Converter1.Convert(value, targetType, parameter, culture);
            return Converter2.Convert(convertedValue, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// If false, returns 0. if true, returns parameter. If the true and false values aren't set, returns parameter.
    /// If parameter is not int or null, returns 0.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(int))]
    public class BoolToIntConverter : IValueConverter
    {
        public int? TrueIntValue { get; set; }
        public int? FalseIntValue { get; set; }

        public int GetTrueValueOrParam(object param)
        {
            if (TrueIntValue != null) return (int)TrueIntValue;
            if (param is int)
                return (int)param;
            return 0;
        }
        public int GetFalseValue()
        {
            if (FalseIntValue != null) return (int)FalseIntValue;
            return 0;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return null;
            return (bool)value == false
                ? GetFalseValue()
                : GetTrueValueOrParam(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(decimal), typeof(Brush))]
    public class NegativeTextRedValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Black);

            decimal decValue = 0m;
            decimal.TryParse(value.ToString(), out decValue);

            if (decValue < 0)
                brush = new SolidColorBrush(Colors.Red);

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(IEnumerable), typeof(bool))]
    public class EnumerableAnyToTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;
            var enumerable = value as IEnumerable;
            if (value is DataTable)
                enumerable = ((DataTable)value).AsEnumerable();
            if (enumerable == null)
                return false;

            var count = 0;
            foreach (var x in enumerable)
            {
                count++;
                break;
            }
            return count != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    [ValueConversion(typeof(IEnumerable), typeof(int))]
    public class EnumerableCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0;
            var enumerable = value as IEnumerable;
            var count = 0;
            foreach (var x in enumerable)
            {
                count++;
            }
            return count;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(decimal), typeof(decimal))]
    public class AddNumberConverter : IValueConverter
    {
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = System.Convert.ToDecimal(value) + System.Convert.ToDecimal(parameter);
            if (MinValue != null)
                if (result < MinValue)
                    result = MinValue.Value;
            if (MaxValue != null)
                if (result > MaxValue)
                    result = MaxValue.Value;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToDecimal(value) - System.Convert.ToDecimal(parameter);
        }
    }
    [ValueConversion(typeof(decimal), typeof(decimal))]
    public class ScaleNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToDecimal(value) * System.Convert.ToDecimal(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToDecimal(value) / System.Convert.ToDecimal(parameter);
        }
    }
    [ValueConversion(typeof(bool?), typeof(string))]
    public class BoolYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var boolValue = ((bool?)value);
            return boolValue == null || boolValue == false ? "No" : "Yes";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((string)value).ToLower() == "yes";
        }
    }

    [ValueConversion(typeof(decimal?), typeof(decimal))]
    public class NullDecimalToZero : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var val = (decimal?)value;
            return val == null ? 0 : val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (decimal)(decimal?)value;
        }
    }

    /// <summary>
    /// Returns true if the provided string value is present in the parameter
    /// </summary>
    [ValueConversion(typeof(string), typeof(bool))]
    public class MultiValueOrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var options = ((string)parameter).Split(';');
            var val = (string)value;

            if (string.IsNullOrEmpty(val))
                return false;

            if (options.Contains(val))
                return true;

            //if the value isn't already present, see if it's spaceless case-insensitive counterpart is present
            //Ex: make "PendingPrinting" match with "Pending Printing"
            return options.Select(x => x.Replace(" ", "").ToUpper()).Contains(val.Replace(" ", "").ToUpper());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }



    /// <summary>
    /// Use this class to help bind things with a source outside the control.
    /// see: http://www.thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/
    /// </summary>
    public class BindingProxy : Freezable
    {
        #region Overrides of Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        #endregion

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }

}


