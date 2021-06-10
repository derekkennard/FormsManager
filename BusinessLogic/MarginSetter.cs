using System.Windows;
using System.Windows.Controls;

namespace BusinessLogic
{
    public class MarginSetter
    {
        // Using a DependencyProperty as the backing store for Margin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached("Margin", typeof (Thickness), typeof (MarginSetter),
                            new UIPropertyMetadata(new Thickness(), CreateThicknesForChildren));

        public static Thickness GetMargin(DependencyObject obj)
        {
            return (Thickness) obj.GetValue(MarginProperty);
        }

        public static void SetMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(MarginProperty, value);
        }

        public static void CreateThicknesForChildren(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;

            if (panel == null)
                return;

            foreach (var child in panel.Children)
            {
                var fe = child as FrameworkElement;

                if (fe == null)
                    continue;

                fe.Margin = GetMargin(panel);
            }
        }
    }
}