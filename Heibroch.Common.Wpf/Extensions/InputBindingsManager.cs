using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Heibroch.Common.Wpf.Extensions
{
    public static class InputBindingsExtension
    {
        public static readonly DependencyProperty UpdatePropertySourceOnEnterProperty = DependencyProperty.RegisterAttached(
                "UpdatePropertySourceOnEnter",
                typeof(DependencyProperty),
                typeof(Control),
                new PropertyMetadata());

        public static void SetUpdatePropertySourceOnEnter(DependencyObject dependencyObject, DependencyProperty value)
        {
            var control = dependencyObject as Control;
            if (control != null)
            {
                control.KeyDown -= OnKeyDown;
                control.KeyDown += OnKeyDown;
            }

            dependencyObject.SetValue(UpdatePropertySourceOnEnterProperty, value);
        }

        public static DependencyProperty GetUpdatePropertySourceOnEnter(DependencyObject dependencyObject)
        {
            return (DependencyProperty)dependencyObject.GetValue(UpdatePropertySourceOnEnterProperty);
        }

        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            var dependencyObject = sender as DependencyObject;
            if (dependencyObject == null) return;
            BindingOperations.GetBindingExpression(dependencyObject, GetUpdatePropertySourceOnEnter(dependencyObject))?.UpdateSource();
        }
    }
}
