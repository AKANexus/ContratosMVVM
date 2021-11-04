using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ContratosMVVM.Controls
{
    public class CurrencyBox : TextBox/*, INotifyPropertyChanged*/
    {
        public static readonly CultureInfo ptBR = CultureInfo.GetCultureInfo("pt-BR");
        public static readonly CultureInfo enUS = CultureInfo.GetCultureInfo("en-US");
        private static readonly System.Text.RegularExpressions.Regex _regex = new("[^0-9]+");
        private string country;
        private bool caretIsOnDecimal = false;

        public CurrencyBox()
        {
            HorizontalContentAlignment = HorizontalAlignment.Right;
            VerticalContentAlignment = VerticalAlignment.Center;
        }
        public string moeda
        {
            get
            {
                return Country switch
                {
                    "en-US" => "$",
                    _ => "R$ "
                };
            }
        }

        public string Country
        {
            get => country;
            set => country = value;
        }

        public decimal Value
        {
            get => (decimal)GetValue(ValueProperty);
            set
            {
                Text = value.ToString("C2", cultureInfo);
                SetValue(ValueProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MyCustomProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(decimal), typeof(CurrencyBox), new UIPropertyMetadata(MyPropertyChangedHandler));

        public static void MyPropertyChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //// Get instance of current control from sender
            //// and property value from e.NewValue

            // Set public property on TaregtCatalogControl, e.g.

            ((CurrencyBox)sender).Value = (decimal)e.NewValue;
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if ((decimal)GetValue(ValueProperty) == 0M)
            {
                Text = string.Empty;
            }
            else Text = Value.ToString("C2", cultureInfo);
            base.OnLostFocus(e);
        }


        public bool EnterToMoveNext { get; set; } = true;

        private CultureInfo cultureInfo
        {
            get
            {
                return country switch
                {
                    "en-US" => enUS,
                    _ => ptBR
                };
            }
        }



        private int decimalCaretIndex()
        {
            if (!String.IsNullOrEmpty(Text))
            {
                return Text.Length - 3;
            }
            else return 0;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (CaretIndex <= Text.Length - moeda.Length)
            {
                caretIsOnDecimal = false;
            }
            else
            {
                caretIsOnDecimal = true;
            }

            if (e.Key == Key.Delete)
            {
                e.Handled = true;
                Value = 0;
                caretIsOnDecimal = false;
            }

            if (e.Key == Key.Back)
            {
                if (!caretIsOnDecimal)
                {
                    if (Text.Length == 7)
                    {
                        Value = decimal.Parse("0," + Text.Substring(decimalCaretIndex() + 1), ptBR);
                        e.Handled = true;
                        CaretIndex = Text.Length - 3;

                    }
                    else
                    {
                        Value = decimal.Parse($"{Text.Substring(moeda.Length, Text.Length - (4 + moeda.Length))},{Text.Substring(decimalCaretIndex() + 1)}", ptBR);
                        e.Handled = true;
                        CaretIndex = Text.Length - 3;

                    }
                }
                else
                {
                    Value = decimal.Parse($"{Text.Substring(moeda.Length, Text.Length - (3 + moeda.Length))},0{Text.Substring(Text.Length - 2, 1)}", ptBR);
                    e.Handled = true;
                    CaretIndex = Text.Length;
                }
            }

            else if (e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Enter || e.Key == Key.Tab || e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {

            }
            else if (e.Key == Key.Space) e.Handled = true;
            else
            {
                if (SelectionLength == Text.Length)
                {
                    Value = 0;
                }
            }
            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (e.Text == "," || e.Text == ".")
            {
                caretIsOnDecimal = true;
                CaretIndex = Text.Length;
            }
            string inteiros = Text.Substring(moeda.Length, Text.Length - (3 + moeda.Length));

            string decimais = Text.Substring(decimalCaretIndex() + 1);
            base.OnPreviewTextInput(e);

            if (ContemSoNumeros(e.Text))
            {
                if (!caretIsOnDecimal)
                {
                    if (decimal.Parse(inteiros) == 0)
                    {
                        Value = decimal.Parse($"{e.Text},{decimais}", ptBR);
                    }
                    else
                    {
                        Value = decimal.Parse($"{inteiros}{e.Text},{decimais}", ptBR);
                    }
                    CaretIndex = Text.Length - 3;
                }
                else
                {
                    Value = decimal.Parse($"{inteiros},{decimais.Substring(1)}{e.Text}", ptBR);
                    CaretIndex = Text.Length;
                }
            }
            e.Handled = true;
        }


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if ((decimal)GetValue(ValueProperty) == 0M)
            {
                Text = 0M.ToString("C2", cultureInfo);
            }
            caretIsOnDecimal = false;
            SelectAll();
            e.Handled = true;
            base.OnGotFocus(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (!IsFocused)
            {
                Focus();
                e.Handled = true;
            }
            else
            {
                if (CaretIndex <= Text.Length - 3)
                {
                    caretIsOnDecimal = false;
                }
                else
                {
                    caretIsOnDecimal = true;
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var element = this as UIElement;
            if (e.Key == Key.Enter && EnterToMoveNext)
            {
                element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            base.OnKeyDown(e);
        }


        private static bool ContemSoNumeros(string texto)
        {
            return !_regex.IsMatch(texto);
        }
    }

}
