using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FolderController fileController = new FolderController();

        public MainWindow()
        {
            InitializeComponent();

            DirectoryDisplay.ItemsSource = fileController.Content;
            DirectoryMenu.ItemsSource = fileController.Menu;
            InfoDisplay.ItemsSource = fileController.Info;

            DirectoryMenu.SelectionMode = SelectionMode.Single;
            DirectoryDisplay.SelectionMode = SelectionMode.Single;
            InfoDisplay.SelectionMode = SelectionMode.Single;
        }

        private void DirectoryDisplay_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                fileController.Open(DirectoryDisplay.SelectedIndex);
                DirectoryMenu.SelectedIndex++;
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        private void DirectoryDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                fileController.DisplayInfo(DirectoryDisplay.SelectedIndex);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        private void DirectoryMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                fileController.Select(DirectoryMenu.SelectedIndex);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        private void SearchMenu_SelectionChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                fileController.Search(SearchMenu.Text).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        private static void DisplayError(Exception ex) => MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
