﻿using System;
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

using Microsoft.Win32;

using System.ComponentModel;

namespace WpfApp1
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string Default_Window_Name;
        bool GSP_Success;
        bool File_contents_IsEmpty;
        bool File_Name_IsDefault;

        public MainWindow()
        {
            InitializeComponent();


            Default_Window_Name = File_Title.Text;
            File_contents_IsEmpty = string.IsNullOrWhiteSpace(File_contents.Text.Trim());
            File_Name_IsDefault =  (File_Title.Text == Default_Window_Name) ? true : false;
            GSP_Success = false;
        }




        private Editing_params _editingWindow;

        private void EditingWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            _editingWindow.Visibility = Visibility.Hidden;
        }


        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (_editingWindow == null)
            {
                _editingWindow = new Editing_params(this);
                _editingWindow.Closing += EditingWindow_Closing;
                _editingWindow.Show();
            }
            else
            {
                _editingWindow.Visibility = Visibility.Visible;
            }
        }

        private void File_New_Click(object sender, RoutedEventArgs e)
        {
            switch (OnSaving())
            {
                case MessageBoxResult.Yes:
                    // Saving procedure
                    bool GSP_Success = false;
                    Generalized_Saving_procedure(sender, e, ref GSP_Success);
                    if (GSP_Success)
                    {
                        File_contents.Clear();
                        File_Title.Text = "New document";
                    }
                  
                    break;

                case MessageBoxResult.No:

                    File_contents.Clear();
                    File_Title.Text = "New document";
                    break;
                case MessageBoxResult.Cancel:
                    //args_.Handled = true;
                    e.Handled = true;
                    break;
            }

        }

        private void File_Open_Click(object sender, RoutedEventArgs e)
        {
            switch (OnSaving())
            {
                case MessageBoxResult.Yes:
                    // Saving procedure
                    GSP_Success = false;
                    Generalized_Saving_procedure(sender, e, ref GSP_Success);
                    if(GSP_Success)
                    { Browsing_procedure(sender, e); }
                   
                    break;
                case MessageBoxResult.No:
                    Browsing_procedure(sender, e);
                    break;
                case MessageBoxResult.Cancel:
                    //args_.Handled = true;
                    e.Handled = true; 
                    break;
            }
        }

        private void Generalized_Saving_procedure(object sender, RoutedEventArgs e)
        {
            string fileName = File_Title.Text;

            if (!(File_contents_IsEmpty && File_Name_IsDefault))
            {
                if (fileName == "New document")
                {
                    File_Save_As_Click(sender, e);
                }
                else
                {
                    string temp_File_Path;
                    temp_File_Path = File_Title.Text;
                    if (temp_File_Path != "")
                    {
                        //var result = MessageBox.Show("Do you want to save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        //if (result == MessageBoxResult.Yes)
                        //{
                        File.WriteAllText(File_Title.Text, File_contents.Text);
                        //}
                    }
                }
            }
            else { }
        }

        private void File_Save_Click(object sender, RoutedEventArgs e)
        {
            Generalized_Saving_procedure(sender, e);
        }

        private void File_Save_As_Click(object sender, RoutedEventArgs e)
        {
            //Save as
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Save text file";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    writer.Write(File_contents.Text);
                }
                File_Title.Text = saveFileDialog.FileName;
            }
            else if (result == false)
            {
                
                // User clicked the Cancel button or closed the dialog without saving
                // Handle this case as needed
            }
            else
            {
                // Dialog was closed programmatically, not by user interaction
                // Handle this case as needed
            }
        }

        private void File_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Browsing_procedure(object sender, RoutedEventArgs e)
        {
            //Opening
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                File_Title.Text = openFileDialog.FileName;
            }
            //ColorListBox.SelectedItem = new SolidColorBrush(Colors.Black);
            OpenButton_Click(sender, e);
        }


        

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (true)
            {
               
                switch (OnSaving())
                {
                    case MessageBoxResult.Yes:
                        // Saving procedure

                        bool GSP_Success = false;
                        RoutedEventArgs args_ = new RoutedEventArgs();
                        object obj_ = new object();
                        Generalized_Saving_procedure(obj_, args_, ref GSP_Success);



                        if (!GSP_Success)
                        { e.Cancel = true; }
                        else
                        {
                            CloseAndShutDown();
                        }

                        break;
                    case MessageBoxResult.No:
                        {
                            CloseAndShutDown();
                        }
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true; // Cancel the closing event
                        break;
                }
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string stringPath = File_Title.Text;

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
                File_Title.Text = stringPath;
                
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


        private void Generalized_Saving_procedure(object sender, RoutedEventArgs e, ref bool Success)
        {
            Success = false;

            string fileName = File_Title.Text;
            string contts = File_contents.Text;

            if (!(fileName == "New document" && contts == ""))
            {
                if (fileName == "New document")
                {
                    File_Save_As_Click(sender, e, ref Success);
                }
                else
                {
                    string temp_File_Path;
                    temp_File_Path = File_Title.Text;
                    if (temp_File_Path != "")
                    {
                        //var result = MessageBox.Show("Do you want to save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        //if (result == MessageBoxResult.Yes)
                        //{
                        File.WriteAllText(File_Title.Text, File_contents.Text);
                        //}

                        Success = true;
                    }
                }
            }
          
        }

        private void File_Save_As_Click(object sender, RoutedEventArgs e, ref bool Success)
        {
            //Save as
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Save text file";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    writer.Write(File_contents.Text);
                }
                File_Title.Text = saveFileDialog.FileName;
            }
            else if (result == false)
            {

                // User clicked the Cancel button or closed the dialog without saving
                // Handle this case as needed
            }
            else
            {
                // Dialog was closed programmatically, not by user interaction
                // Handle this case as needed
            }

            Success = result.Value;
        }

        public void CloseAndShutDown()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Editing_params)
                {
                    window.Close();
                    break;
                }
            }
            Application.Current.Shutdown();
        }

        private void File_contents_TextChanged(object sender, TextChangedEventArgs e)
        {
            File_contents_IsEmpty = string.IsNullOrWhiteSpace(File_contents.Text.Trim());
        }

        private void File_Title_TextChanged(object sender, TextChangedEventArgs e)
        {
            File_Name_IsDefault=(File_Title.Text == Default_Window_Name) ? true : false;
        }

        private MessageBoxResult OnSaving()
        {
            MessageBoxResult result;
            if (!File_contents_IsEmpty || !File_Name_IsDefault)
            {
                result = MessageBox.Show("Do you want to save your changes?", "Save Changes", MessageBoxButton.YesNoCancel);
            }
            else
            {
                result = MessageBoxResult.No;
            }
            return result;
        }
    }


}
