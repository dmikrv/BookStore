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
    public partial class MainWindow : Window
    {
        //DbContextOptions<BookStoreContext> connectionSettings;
        //DbContextOptions connectionSettings = null;
        public MainWindow()
        {
            InitializeComponent();

            LogEntryList.ItemsSource = LogEntryLoggerProvider.LogEntites;

            //logEntries.Add(new LogEntry
            //{
            //    Timestamp = "35456",
            //    Level = "TEST",
            //    Message = "Hello, codingScars!"
            //});

            //var connectionWindow = new ConnectionWindow();
            //connectionWindow.ShowDialog();
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string header = (tabControl.SelectedItem as TabItem).Header.ToString();

            using (var db = new BookStoreContext())
            {
                if (header == Properties.MainWindowStrings.TabItemBooks)
                {
                    db.Books.Load();
                    dataGridBooks.ItemsSource = db.Books.Local.ToBindingList();
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
    }
}
