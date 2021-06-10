using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using log4net;
using log4net.Config;

namespace FormsManager
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // ReSharper disable once UnusedMember.Local
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnStartup(StartupEventArgs e)
        {
            XmlConfigurator.Configure();
            //Log.Info("Hello World");

            EventManager.RegisterClassHandler(typeof(DatePicker), UIElement.PreviewKeyDownEvent,
                new KeyEventHandler(DatePicker_PreviewKeyDown));

            //works for tab into textbox
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotFocusEvent,
                new RoutedEventHandler(TextBox_GotFocus));
            //works for click textbox
            EventManager.RegisterClassHandler(typeof(Window), UIElement.GotMouseCaptureEvent,
                new RoutedEventHandler(Window_MouseCapture));

            base.OnStartup(e);
        }

        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var dp = sender as DatePicker;
            if (dp == null)
                return;

            if (e.Key == Key.D && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                dp.SetValue(DatePicker.SelectedDateProperty, DateTime.Today);
                return;
            }

            if (!dp.SelectedDate.HasValue)
                return;

            var date = dp.SelectedDate.Value;
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                dp.SetValue(DatePicker.SelectedDateProperty, date.AddDays(1));
            }
            else if (e.Key == Key.Down)
            {
                e.Handled = true;
                dp.SetValue(DatePicker.SelectedDateProperty, date.AddDays(-1));
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // ReSharper disable once PossibleNullReferenceException
            (sender as TextBox).SelectAll();
        }

        private void Window_MouseCapture(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }
    }
}