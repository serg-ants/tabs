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

using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Editing_params.xaml
    /// </summary>
    public partial class Editing_params : Window
    {
        //
        private MainWindow mainWindow;

       
        //

       


        public List<int> fontSizes { get; set; }
        public Editing_params(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
           
            DataContext = this;
            Input_Dir_Default_txt = mainWindow.Title;

            Loaded += MainWindow_Loaded;
            
            foreach (var color in typeof(Colors).GetProperties())
            {
                ColorListBox.Items.Add(color.Name);
            }

            fontSizes = new List<int> { 8, 9, 10, 12, 14, 16, 18, 20, 24, 26, 28, 30 };

            Font_Family_ComboBox.SelectedItem = Fonts.SystemFontFamilies.FirstOrDefault(f => f.Source == "Segoe UI");
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ColorListBox.SelectedItem = new SolidColorBrush(Colors.Black);
        }




        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var italicChecked = italicCheckBox.IsChecked ?? false;
            var underlChecked = underlinedCheckBox.IsChecked ?? false;
            var boldChecked = boldCheckBox.IsChecked ?? false;

            if (italicChecked)
            {
                mainWindow.File_contents.FontStyle = FontStyles.Italic;
                Sample_text.FontStyle = FontStyles.Italic;
            }
            else
            {
                mainWindow.File_contents.FontStyle = FontStyles.Normal;
                Sample_text.FontStyle = FontStyles.Normal;
            }
            if (underlChecked)
            {
                mainWindow.File_contents.TextDecorations = TextDecorations.Underline;
                Sample_text.TextDecorations = TextDecorations.Underline;
            }
            else
            {
                mainWindow.File_contents.TextDecorations = null;
                Sample_text.TextDecorations = null;
            }
            if (boldChecked)
            {
                mainWindow.File_contents.FontWeight = FontWeights.Bold;
                Sample_text.FontWeight = FontWeights.Bold;
            }
            else
            {
                mainWindow.File_contents.FontWeight = FontWeights.Normal;
                Sample_text.FontWeight = FontWeights.Normal;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox_Checked(sender, e);
        }
        private void SaveButton_Click(object sender, RoutedEventArgs s)
        {
            string temp_File_Path;
            temp_File_Path = mainWindow.Title;
            if (temp_File_Path != "")
            {
                var result = MessageBox.Show("Do you want to save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    File.WriteAllText(mainWindow.Title, mainWindow.File_contents.Text);
                }
            }
            else
            {
                var result = MessageBox.Show("No file opened", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainWindow.File_contents != null)
            {
                if (sizeComboBox.SelectedItem != null)
                {
                    int selectedSize = (int)sizeComboBox.SelectedItem;
                    if (!fontSizes.Contains(selectedSize))
                    {
                        int closestSize = fontSizes.Aggregate((x, y) => Math.Abs(x - selectedSize) < Math.Abs(y - selectedSize) ? x : y);
                        mainWindow.File_contents.FontSize = closestSize;
                        Sample_text.FontSize = closestSize;

                        sizeComboBox.SelectedItem = closestSize;
                    }
                    else
                    {
                        mainWindow.File_contents.FontSize = selectedSize;
                        Sample_text.FontSize = selectedSize;
                    }
                }
                else
                {
                    mainWindow.File_contents.FontSize = 10;
                    Sample_text.FontSize = 10;
                }
            }
        }

        private void ColorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string colorName = ColorListBox.SelectedItem.ToString();
            Color selectedColor = (Color)ColorConverter.ConvertFromString(colorName);
            mainWindow.File_contents.Foreground = new SolidColorBrush(selectedColor);
            Sample_text.Foreground = new SolidColorBrush(selectedColor);
        }

        public IEnumerable<double> FontSizeList => new List<double> { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject source = e.OriginalSource as DependencyObject;
            if (source != null && !IsTextBoxOrChild(source))
            {
                //Set focus to a different control or the main window
                Keyboard.ClearFocus();
            }
        }

        private bool IsTextBoxOrChild(DependencyObject obj)
        {
            if (obj == null) return false;
            if (obj is TextBox) return true;
            return IsTextBoxOrChild(VisualTreeHelper.GetParent(obj));
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mainWindow.Title = openFileDialog.FileName;
            }
            ColorListBox.SelectedItem = new SolidColorBrush(Colors.Black);
        }

       

        private
         static string Input_Dir_Default_txt;

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string colorName = ColorListBox.SelectedItem.ToString();
            Color selectedColor = (Color)ColorConverter.ConvertFromString(colorName);
            mainWindow.File_contents.Foreground = new SolidColorBrush(selectedColor);
            Sample_text.Foreground = new SolidColorBrush(selectedColor);
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            mainWindow.File_contents.FontFamily = (FontFamily)Font_Family_ComboBox.SelectedItem;
        }

        private void Application_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("There is no help :(");
        }

        private void ColorListBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string colorName = ColorListBox.SelectedItem.ToString();
            Color selectedColor = (Color)ColorConverter.ConvertFromString(colorName);

            mainWindow.File_contents.Foreground = new SolidColorBrush(selectedColor);
            Sample_text.Foreground= new SolidColorBrush(selectedColor);
        }
    }

    public static class ComboBoxBehavior
    {
        public static bool GetSingleClickSelection(DependencyObject obj)
        {
            return (bool)obj.GetValue(SingleClickSelectionProperty);
        }

        public static void SetSingleClickSelection(DependencyObject obj, bool value)
        {
            obj.SetValue(SingleClickSelectionProperty, value);
        }

        public static readonly DependencyProperty SingleClickSelectionProperty = DependencyProperty.
            RegisterAttached("SingleClickSelection", typeof(bool), typeof(ComboBoxBehavior),
            new PropertyMetadata(false, OnSingleClickSelectionChanged));

        private static void OnSingleClickSelectionChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var comboBox = d as ComboBox;
            if (comboBox != null)
            {
                if ((bool)e.NewValue)
                {
                    comboBox.PreviewMouseDown += OnPreviewMouseDown;
                }
                else
                {
                    comboBox.PreviewMouseDown -= OnPreviewMouseDown;
                }

            }
        }

        private static void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.IsDropDownOpen)
            {
                var item = ItemsControl.ContainerFromElement(comboBox, e.OriginalSource as DependencyObject)
                    as ComboBoxItem;
                if (item != null)
                {
                    item.IsSelected = true;
                    comboBox.IsDropDownOpen = false;
                    e.Handled = true;
                }
            }
        }
    }

}
