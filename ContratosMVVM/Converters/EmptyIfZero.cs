using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ContratosMVVM.Converters
{
    public class EmptyIfZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal dec)
            {
                if (dec == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return dec.ToString((string)parameter, new CultureInfo("pt-BR"));
                }
            }
            else if (value is int integer)
            {
                if (integer == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return integer.ToString((string) parameter, new CultureInfo("pt-BR"));
                }

            }
            else return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace((string)value))
            {
                return 0M;
            }
            else
            {
                return decimal.Parse((string)value, NumberStyles.Any, new CultureInfo("pt-BR"));
            }
        }
    }
}
