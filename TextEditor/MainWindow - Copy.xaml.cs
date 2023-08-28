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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;


namespace WpfApp1
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<int> fontSizes { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Input_Dir_Default_txt = Input_Dir.Text;

            Loaded += MainWindow_Loaded;

            foreach (var color in typeof(Colors).GetProperties())
            {
                ColorListBox.Items.Add(color.Name);
            }

            fontSizes = new List<int> { 8, 9, 10, 12, 14, 16, 18, 20, 24, 26, 28, 30 };
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
                File_contents.FontStyle = FontStyles.Italic;
            else
                File_contents.FontStyle = FontStyles.Normal;
            if (underlChecked)
                File_contents.TextDecorations = TextDecorations.Underline;
            else
                File_contents.TextDecorations = null;
            if (boldChecked)
                File_contents.FontWeight = FontWeights.Bold;
            else
                File_contents.FontWeight = FontWeights.Normal;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox_Checked(sender, e);
        }
        private void SaveButton_Click(object sender, RoutedEventArgs s)
        {
            string temp_File_Path;
            temp_File_Path = curr_file.Text;
            if (temp_File_Path != "")
            {
                var result = MessageBox.Show("Do you want to save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    File.WriteAllText(curr_file.Text, File_contents.Text);
                }
            }
            else
            {
                var result = MessageBox.Show("No file opened", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (File_contents != null)
            {
                if (sizeComboBox.SelectedItem != null)
                {
                    int selectedSize = (int)sizeComboBox.SelectedItem;
                    if (!fontSizes.Contains(selectedSize))
                    {
                        int closestSize = fontSizes.Aggregate((x, y) => Math.Abs(x - selectedSize) < Math.Abs(y - selectedSize) ? x : y);
                        File_contents.FontSize = closestSize;

                        sizeComboBox.SelectedItem = closestSize;
                    }
                    else
                    {
                        File_contents.FontSize = selectedSize;
                    }
                }
                else
                {
                    File_contents.FontSize = 10;
                }
            }
        }

        private void ColorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string colorName = ColorListBox.SelectedItem.ToString();
            Color selectedColor = (Color)ColorConverter.ConvertFromString(colorName);
            File_contents.Foreground = new SolidColorBrush(selectedColor);
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

        private void Input_Dir_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Input_Dir.Text == Input_Dir_Default_txt)
            {
                Input_Dir.Text = "";
            }
        }

        private void Input_Dir_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Input_Dir.Text))
            {
                Input_Dir.Text = Input_Dir_Default_txt;
            }
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
                Input_Dir.Text = openFileDialog.FileName;
            }
            ColorListBox.SelectedItem = new SolidColorBrush(Colors.Black);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string stringPath = Input_Dir.Text.Trim();

                // Validity-of-the-pathway check
                if (!File.Exists(stringPath))
                {
                    throw new FileNotFoundException("File not found", stringPath);
                }

                // Check if the File's extension matches with demanded .txt
                if (System.IO.Path.GetExtension(stringPath) != ".txt")
                {
                    throw new FileFormatException("Wrong format. Only .txt files are supported.");
                }

                // Check directory name for validity
                string directoryName = System.IO.Path.GetDirectoryName(stringPath);
                if (!Directory.Exists(directoryName))
                {
                    throw new DirectoryNotFoundException("No such directory");
                }

                // Upload file's contents into a TextBax after reading it
                string content = File.ReadAllText(stringPath);
                File_contents.Text = content;
                curr_file.Text = stringPath;
                CRFLtext.Text = "Current file:";
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FileFormatException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            File_contents.Foreground = new SolidColorBrush(selectedColor);
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Application_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ColorListBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string colorName = ColorListBox.SelectedItem.ToString();
            Color selectedColor = (Color)ColorConverter.ConvertFromString(colorName);
            File_contents.Foreground = new SolidColorBrush(selectedColor);
        }
    }

    //public class ColorListConverter : System.Windows.Data.IValueConverter
    //{
    //    public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return typeof(Colors).GetProperties();
    //    }

    //    public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return null;
    //    }
    //}



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
