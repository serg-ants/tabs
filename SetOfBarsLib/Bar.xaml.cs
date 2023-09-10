using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;
using System.Collections.ObjectModel;

using System.Threading.Tasks;

using System.Runtime.CompilerServices;



namespace SetOfBarsLib
{
    public partial class Bar : UserControl
    {
        /// <summary>
        /// Last valid name
        /// </summary>
        public string previousValidName = "";

        public Bar()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        public event EventHandler DeleteRequested;

        ////////////
        //public static readonly DependencyProperty NameProperty =
        //DependencyProperty.Register("Name", typeof(string), typeof(Bar));

        public static readonly DependencyProperty ItemsCountProperty =
            DependencyProperty.Register("ItemsCount", typeof(int), typeof(Bar));

        //public string Name
        //{
        //    get { return (string)GetValue(NameProperty); }
        //    set { SetValue(NameProperty, value); }
        //}

        public int ItemsCount
        {
            get { return (int)GetValue(ItemsCountProperty); }
            set { SetValue(ItemsCountProperty, value); }
        }

        ////////////


        public void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Bar b = sender as Bar;
            //b.BorderBrush = Brushes.DeepSkyBlue;
            //b.textBox.Visibility = Visibility.Visible;
            //b.sortOfScoreboard.Visibility = Visibility.Visible;

            //b.sortOfScoreboard.Nums.Focus();
            //b.sortOfScoreboard.Nums.CaretIndex = sortOfScoreboard.Nums.Text.Length;


            //  if (!(textBox.Visibility == Visibility.Visible))
            if (!(textBox.Visibility == Visibility.Visible))
            {
                BorderBrush = Brushes.DeepSkyBlue;
                textBox.Visibility = Visibility.Visible;
                sortOfScoreboard.Visibility = Visibility.Visible;

                //textBox.Focus();
                this.Focus();
                //e.Handled=true;
            }


            //sortOfScoreboard.Nums.Focus();



            //sortOfScoreboard.Nums.Focus();
            //sortOfScoreboard.Nums.CaretIndex = sortOfScoreboard.Nums.Text.Length;



            //TextBox Ae = sortOfScoreboard.Nums as TextBox;
            //Ae.Focus();



        }

        public void OnLostFocus(object sender, RoutedEventArgs e)
        {
            //bool Tout = textBox.IsFocused == false && sortOfScoreboard.Nums.IsFocused == false &&
            //    sortOfScoreboard.Dec.IsFocused == false && sortOfScoreboard.Inc.IsFocused == false;

            if (!(IsMouseOver == true))
            {
                BorderBrush = Brushes.Gray;
                textBox.Visibility = Visibility.Hidden;
                sortOfScoreboard.Visibility = Visibility.Hidden;
            }
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            var parentWP = this.Parent as WrapPanel;

            var test = VisualTreeHelper.GetParent(this);
            while (test != null && !(test is SetOfBars))
            {
                test = VisualTreeHelper.GetParent(test);
            }
            var parentSetOfBars = test as SetOfBars;

            if (parentWP != null)
            {
                parentWP.Children.Remove(this);
                DeleteRequested?.Invoke(this, EventArgs.Empty);
            }


            //parentSetOfBars.Rearrange();
            parentSetOfBars.totalNumBars--;
        }

        private void ValidNameSave(object sender, RoutedEventArgs e)
        {
            previousValidName = textBlocktextBox.Text;
        }

        public void RestoreValidName()
        {
            textBlocktextBox.Text = previousValidName;
        }

        //private void TextBlocktextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (textBlocktextBox.Foreground == Brushes.Red)
        //        textBlocktextBox.Text = previousValidName;
        //}




        //private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    // Check if the entered text is a digit
        //    if (!char.IsDigit(e.Text[0]))
        //    {
        //        e.Handled = true; // Ignore the input
        //    }
        //}
    }
}