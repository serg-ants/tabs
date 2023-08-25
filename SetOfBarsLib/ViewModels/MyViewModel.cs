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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using WPFDemos.ViewModels;

namespace SetOfBarsLib
{
    //public class MyViewModel : INotifyPropertyChanged
    //{
    //    private ObservableCollection<MyObject> _myObjects;

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public ObservableCollection<MyObject> MyObjects
    //    {
    //        get { return _myObjects; }
    //        set
    //        {
    //            _myObjects = value;
    //            OnPropertyChanged(nameof(MyObjects));
    //        }
    //    }

    //    public MyViewModel()
    //    {
    //        MyObjects = new ObservableCollection<MyObject>
    //    {
    //        new MyObject { Name = "Object 1", ItemsCount = 5 },
    //        new MyObject { Name = "Object 2", ItemsCount = 10 },
    //        // add more objects here...
    //    };
    //    }

    //    protected void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //}

    public class MyViewModel
    {
        public ObservableCollection<MyObject> MyObjects { get; set; }

        public MyViewModel()
        {
            MyObjects = new ObservableCollection<MyObject>();
            MyObjects.Add(new MyObject { Name = "Object 1", ItemsCount = 5 });
            MyObjects.Add(new MyObject { Name = "Object 2", ItemsCount = 10 });
            // Add more objects as needed
        }
    }
}