using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.EntityFrameworkCore;

namespace Book_Store
{
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        private readonly string _connectionString;
        public AddBookWindow(string connectionString)
        {
            //InitializeComponent();
            //_connectionString = connectionString;

            //using (var db = new BookStoreContext(_connectionString))
            //{
            //    authorComboBox.ItemsSource = db.Authors.Include(nameof(Models.Human)).ToList();
            //    publisherComboBox.ItemsSource = db.Publishers.ToList();
            //    genreComboBox.ItemsSource = db.Genres.ToList();
            //}
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FloatValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void addAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            //var addAuthorWindow = new AddAuthorWindow(_connectionString);
            //addAuthorWindow.ShowDialog();
        }

        private void addPublisherButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addBookButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addGenreButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
