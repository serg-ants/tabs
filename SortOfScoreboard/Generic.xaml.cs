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


namespace SortOfScoreboard
{
    public partial class SortOfScoreboard : Control
    {


        public Button Inc;
        public Button Dec;
        public TextBox Nums;

        public bool _naturals1to10Only;
        private int _maxValue;
        private int _minValue;

        public bool Naturals1to10Only
        {
            get { return _naturals1to10Only; }
            set
            {
                _naturals1to10Only = value;
                if (!_naturals1to10Only)
                {
                    _maxValue = 50000;
                    _minValue = -50000;
                }
                else
                {
                    _maxValue = 10;
                    _minValue = 1;
                }
            }
        }

        //public int MaxValue
        //{
        //    get { return _maxValue; }
        //    set { _maxValue = value; }
        //}

        //public int MinValue
        //{
        //    get { return _minValue; }
        //    set { _minValue = value; }
        //}

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(SortOfScoreboard), new PropertyMetadata(50000));

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(SortOfScoreboard), new PropertyMetadata(-50000));

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }



        static SortOfScoreboard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SortOfScoreboard), new FrameworkPropertyMetadata(typeof(SortOfScoreboard)));
        }

        //TextBox Nums = null;
        //Button Inc = null;
        //Button Dec = null;
        

        public override void OnApplyTemplate()
        {
            
            Nums = GetTemplateChild("Nums") as TextBox;
            Nums.PreviewTextInput += TextBox_PreviewTextInput;
            Nums.TextChanged += TextBox_TextChanged;

            //Inc = GetTemplateChild("Inc") as Button;
            //Dec = GetTemplateChild("Dec") as Button;
        }
   
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Check if the entered text is a digit

            if (Naturals1to10Only)
            {
                 if (!char.IsDigit(e.Text[0]))
                {
                    e.Handled = true; // Ignore the input
                }
            }
            else
            {
                TextBox textBox = sender as TextBox;

                if (!(char.IsDigit(e.Text[0])||(e.Text[0] == '-' && textBox.CaretIndex == 0)))
                {
                    e.Handled = true; // Ignore the input
                }
            }

               
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
    "Value", typeof(int), typeof(SortOfScoreboard), new PropertyMetadata(0, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int newValue = (int)e.NewValue;
            int tempMaxV = ((SortOfScoreboard)d).MaxValue;
            int tempMinV = ((SortOfScoreboard)d).MinValue;

            if (newValue > tempMaxV)
            {
                ((SortOfScoreboard)d).Value = tempMaxV;
            }
            else if (newValue < tempMinV)
            {
                ((SortOfScoreboard)d).Value = tempMinV;
            }


            //Button Inc = ((SortOfScoreboard)d).Inc as Button;
            //Button Dec = ((SortOfScoreboard)d).Dec as Button;

            //if (((SortOfScoreboard)d).Value == tempMaxV)
            //{ ((SortOfScoreboard)d).Inc.IsEnabled = false; ((SortOfScoreboard)d).Inc.Background = Brushes.Red; }
            //if (((SortOfScoreboard)d).Value == tempMinV)
            //{ ((SortOfScoreboard)d).Dec.IsEnabled = false; ((SortOfScoreboard)d).Inc.Background = Brushes.Red; }
        
        }


        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty IncreaseCommandProperty =
            DependencyProperty.Register(nameof(IncreaseCommand), typeof(ICommand), typeof(SortOfScoreboard), new PropertyMetadata(null));

        public ICommand IncreaseCommand
        {
            get => (ICommand)GetValue(IncreaseCommandProperty);
            set => SetValue(IncreaseCommandProperty, value);



        }

        public static readonly DependencyProperty DecreaseCommandProperty =
            DependencyProperty.Register(nameof(DecreaseCommand), typeof(ICommand), typeof(SortOfScoreboard), new PropertyMetadata(null));

        public ICommand DecreaseCommand
        {
            get => (ICommand)GetValue(DecreaseCommandProperty);
            set => SetValue(DecreaseCommandProperty, value);
        }

        public SortOfScoreboard()
        {
           
            IncreaseCommand = new RelayCommand(() => Value++);
            DecreaseCommand = new RelayCommand(() => Value--);
            //Inc = new Button();
            //Dec = new Button();
            Naturals1to10Only = false;
        }

    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    
}
