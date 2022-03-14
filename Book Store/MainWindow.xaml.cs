using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using Microsoft.EntityFrameworkCore;

namespace Book_Store
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        private readonly string _connectionString;
        public MainWindow()
        {
            InitializeComponent();
            LogEntryList.ItemsSource = LogEntryLoggerProvider.LogEntites;

            var connectionWindow = new ConnectionWindow();
            var result = connectionWindow.ShowDialog();

            if (result == true)
                _connectionString = connectionWindow.ConnectionString;
            else
                this.Close();
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string header = (tabControl.SelectedItem as TabItem).Header.ToString();

            using (var db = new BookStoreContext(_connectionString))
            {
                if (header == Properties.MainWindowStrings.TabItemBooks)
                {
                    //try
                    //{
                        listViewBooks.ItemsSource = db.Books.Include(nameof(Models.Author)).Include(nameof(Models.Genre)).
                            Include(nameof(Models.Publisher)).ToList();
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message);
                    //}

                    //Include(nameof(Models.ContinuationBook)).
                }
                else if (header == Properties.MainWindowStrings.TabItemAuthors)
                {
                    db.Authors.Load();
                    dataGridAuthors.ItemsSource = db.Authors.Local.ToBindingList();
                }
                else if (header == Properties.MainWindowStrings.TabItemPublisher)
                {
                    db.Publishers.Load();
                    dataGridPublisher.ItemsSource = db.Publishers.Local.ToBindingList();
                }
                else if (header == Properties.MainWindowStrings.TabItemGenre)
                {
                    db.Genres.Load();
                    dataGridGenre.ItemsSource = db.Genres.Local.ToBindingList();
                }
            }
        }

        private void addBookButton_Click(object sender, RoutedEventArgs e)
        {
            var addBookWindow = new AddBookWindow(_connectionString);
            addBookWindow.ShowDialog();
        }
    }
}
