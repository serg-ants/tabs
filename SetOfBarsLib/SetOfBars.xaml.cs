﻿using System;
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
using WPFDemos.ViewModels;


using System.Threading.Tasks;

using System.Runtime.CompilerServices;

using System.Security.Cryptography;

/// <summary>
/// Interaction logic for SetOfBars.xaml
namespace SetOfBarsLib
{
    public partial class SetOfBars : UserControl
    {
        public Bar Last_Focused_Bar = null;

        public int totalNumBars;
        public int totalCount = 0;

        private WrapPanel gridAsWP;
        private Button addNewButton;

        private Bar temp = new Bar();
        private Bar temp2 = new Bar();

        MyObject ayayaya = new MyObject();

        public ObservableCollection<MyObject> MyObjects { get; set; }

        public string hexstr()
        {
            byte[] randomBytes = new byte[4];
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            uint num = BitConverter.ToUInt32(randomBytes, 0);
            return num.ToString("X8");
        }


        public SetOfBars()
        {
            InitializeComponent();
            DataContext = new MyViewModel();

            MyObjects = new ObservableCollection<MyObject>();
        }

        private void AddBar_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(tbNumberOfBars.Text, out int numBars))
            {
                totalNumBars = numBars;
                AddBars(numBars);
            }
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            var bar = new Bar();
            bar.Text = "Bar " + hexstr();
            bar.sortOfScoreboard.Value = new Random().Next(1, 11);
            bar.sortOfScoreboard.Naturals1to10Only = true;
            bar.Width = 113;
            bar.Height = 59;
            bar.Margin = new Thickness(5);

            MyObjects.Add(new MyObject { Name = bar.Text, ItemsCount = bar.sortOfScoreboard.Value });


            gridAsWP.Children.Remove(addNewButton);
            gridAsWP.Children.Add(bar);

            addNewButton = new Button();
            addNewButton.Content = "Add new";
            addNewButton.Click += AddNew_Click;
            addNewButton.Width = 113;
            addNewButton.Height = 59;
            addNewButton.Margin = new Thickness(5);

            gridAsWP.Children.Add(addNewButton);

            var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
            bar.Focus();
            bar.OnMouseDown(sender, args);
            //
            //bar.textBox.Focus();
            //temp.OnLostFocus(sender,e);
            //temp = bar;
        }

        private void DeleteBar(object sender, RoutedEventArgs e)
        {
            var bar = ((Button)sender).DataContext as Bar;
            if (bar != null)
            {

                BarsPanel.Children.Remove(bar);
                UpdateTotalCount();
            }
        }

        private void AddBars(int numBars)
        {
            BarsPanel.Children.Clear();

            gridAsWP = new WrapPanel();

            for (int indx = 0; indx < numBars; indx++)
            {
                var bar = new Bar();
                bar.Text = "Bar " + hexstr();
                bar.sortOfScoreboard.Value = new Random().Next(1, 11);
                bar.sortOfScoreboard.Naturals1to10Only = true;
                bar.Width = 113;
                bar.Height = 59;
                bar.Margin = new Thickness(5);

                //bar.DeleteButtonClick += DeleteBar;
                gridAsWP.Children.Add(bar);

                MyObjects.Add(new MyObject { Name = bar.Text, ItemsCount = bar.sortOfScoreboard.Value });
            }

            BarsPanel.Children.Add(gridAsWP);

            addNewButton = new Button();
            addNewButton.Content = "Add new";
            addNewButton.Click += AddNew_Click;
            addNewButton.Width = 113;
            addNewButton.Height = 59;
            addNewButton.Margin = new Thickness(5);

            gridAsWP.Children.Add(addNewButton);

            UpdateTotalCount();
        }


        private void UpdateTotalCount()
        {
            int totalCount = BarsPanel.Children.OfType<Bar>().Sum(b => b.sortOfScoreboard.Value);
            tbTotalCount.Text = $"Total count: {totalCount}";
        }

        private void NumBarsTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (int.TryParse(tbNumberOfBars.Text, out int numBars))
                {
                    AddBars(numBars);
                }
            }
        }

        private void Upd(object sender, MouseButtonEventArgs e)
        {
            totalCountUpd();
            NamesUniqueness();
        }

        private void Upd(object sender, MouseEventArgs e)
        {
            MouseButtonEventArgs args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
            Upd(sender, args);
        }

        public void totalCountUpd()
        {
            if (gridAsWP == null)
            {
                // Handle empty gridAsWP
            }
            else
            {
                int totalCount = 0;
                foreach (var child in gridAsWP.Children)
                {
                    if (child is Bar bar)
                    {
                        totalCount += bar.sortOfScoreboard.Value;
                        //bar.textBlocktextBox.Foreground = Brushes.Red;
                    }
                }
                tbTotalCount.Text = "Total count: " + (totalCount);
            }
        }

        public void NamesUniqueness()
        {
            if (gridAsWP == null)
            {
                // Handle empty gridAsWP
            }
            else
            {
                // get all the Bar elements in the WrapPanel
                var bars = gridAsWP.Children.OfType<Bar>();

                /// <summary>
                /// Create a dictionary to keep track of which texts have already been seen
                /// </summary>
                var seenTextsUnique = new Dictionary<string, bool>();

                // iterate through each Bar and complete a dictionary
                foreach (var bar in bars)
                {
                    // check if we've already seen this text before
                    if (seenTextsUnique.ContainsKey(bar.Text))
                    {
                        // if so, set uniqueness to false
                        seenTextsUnique[bar.Text] = false;
                    }
                    else
                    {
                        // otherwise, mark text as unique
                        seenTextsUnique[bar.Text] = true;
                    }
                }

                // Restoration of valid names
                foreach (var bar in bars)
                {
                    if(seenTextsUnique[bar.Text] == false && bar.textBox.IsVisible)
                    {
                        bar.RestoreValidName();
                    }
                }

            }
        }

        private void Foci_Adjustments(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("Foci Adjustments!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

            // Get the element that the mouse is over
            DependencyObject element = (DependencyObject)Mouse.DirectlyOver;

            // Traverse the visual tree to check if the element is a Bar
            while (element != null && !(element is Bar))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            if (!(element is Bar))
            {
                The_Border.Focus();
            }
        }

        private void TbNumberOfBars_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true; // Ignore the input
            }
        }
    }
}