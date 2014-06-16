using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WinRTSettingsExplorer
{
    /// <summary>
    /// Interaction logic for ValueEditorWindow.xaml
    /// </summary>
    public abstract partial class ValueEditorWindow : Window
    {
        public ValueEditorWindow()
        {
            InitializeComponent();
        }

        private void BtnOK_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }



        public string ValueName
        {
            get { return (string)GetValue(ValueNameProperty); }
            set { SetValue(ValueNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueNameProperty =
            DependencyProperty.Register("ValueName", typeof(string), typeof(ValueEditorWindow), new PropertyMetadata(null));




        public string ValueType
        {
            get { return (string)GetValue(ValueTypeProperty); }
            set { SetValue(ValueTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueTypeProperty =
            DependencyProperty.Register("ValueType", typeof(string), typeof(ValueEditorWindow), new PropertyMetadata(null));

        public abstract object Value { get; set; }

    }

    public class ValueEditorWindow<T> : ValueEditorWindow
    {


        public T TypedValue
        {
            get { return (T)GetValue(TypedValueProperty); }
            set { SetValue(TypedValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypedValueProperty =
            DependencyProperty.Register("TypedValue", typeof(T), typeof(ValueEditorWindow<T>), new PropertyMetadata(default(T)));

        public override object Value
        {
            get { return TypedValue; }
            set { TypedValue = (T)value; }
        }
    }
}
