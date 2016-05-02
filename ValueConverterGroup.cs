using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DLeh.Util.MVVM
{
    //src http://stackoverflow.com/questions/2607490/is-there-a-way-to-chain-multiple-value-converters-in-xaml
    /// <summary>
    /// When using the value converter group, it does not make any assumptions about the types you feed it,
    /// and may send the wrong targetType to a converter. Therefore, you should only feed it converters
    /// that don't do targetType checking. Also, you shouldn't need to write converts that do targetType
    /// checking anyway, as it will raise an error on its own when it's an issue.
    /// </summary>
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //this target type can cause issues if the converter has type validation
            // the targetType variable doesn't change, so some converters might pitch a hissy fit.
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
