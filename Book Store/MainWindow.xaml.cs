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

using Book_Store.Models;

namespace Book_Store
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _connectionString;
        public MainWindow()
        {
            InitializeComponent();
            LogEntryList.ItemsSource = LogEntryLoggerProvider.LogEntites;

            //var connectionWindow = new ConnectionWindow();
            //var result = connectionWindow.ShowDialog();

            //if (result == true)
            //    _connectionString = connectionWindow.ConnectionString;
            //else
            //    this.Close();

            // for develop
            _connectionString = $@"Server=(localdb)\MSSQLLocalDB;Database={Properties.ConnectionWindowStrings.ServerDatabaseNameDefault};"
                + $@"User id=bookadmin;Password=bookadmin";
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string header = (tabControl.SelectedItem as TabItem).Header.ToString();

            if (header == Properties.MainWindowStrings.TabItemBooks)
            {
                UpdateBooks();
            }
            else if (header == Properties.MainWindowStrings.TabItemAuthors)
            {
                UpdateAuthors();
            }
            else if (header == Properties.MainWindowStrings.TabItemPublisher)
            {
                UpdatePublishers();
            }
            else if (header == Properties.MainWindowStrings.TabItemGenre)
            {
                UpdateGenres();
            }

        }
        private void UpdateAuthors()
        {
            using (var db = new BookStoreContext(_connectionString))
            {
                listViewAuthors.ItemsSource = db.Authors.ToList();
            }

            firstNameText.Text = string.Empty;
            lastNameText.Text = string.Empty;
            patronymicText.Text = string.Empty;
        }

        private void UpdateBooks()
        {
            using (var db = new BookStoreContext(_connectionString))
            {
                listViewBooks.ItemsSource = db.Books.Include(nameof(Author)).Include(nameof(Genre)).
                    Include(nameof(Publisher)).ToList();
            }
        }

        private void UpdatePublishers()
        {
            using (var db = new BookStoreContext(_connectionString))
            {
                listViewPublisher.ItemsSource = db.Publishers.ToList();
            }
            publisherNameText.Text = string.Empty;
        }

        private void UpdateGenres()
        {
            using (var db = new BookStoreContext(_connectionString))
            {
                listViewGenre.ItemsSource = db.Genres.ToList();
            }
            genreNameText.Text = string.Empty;
        }

        // ---------------------------------------------------------------------------------------- //


        private void listViewAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (listViewAuthors.SelectedIndex == -1)
                return;

            var author = listViewAuthors.SelectedItem as Author;

            firstNameText.Text = author.FirstName;
            lastNameText.Text = author.LastName ?? string.Empty;
            patronymicText.Text = author.Patronymic ?? string.Empty;
        }

        private void listViewBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (listViewBooks.SelectedIndex == -1)
                return;

            var book = listViewAuthors.SelectedItem as Book;
        }

        private void listViewPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (listViewPublisher.SelectedIndex == -1)
                return;

            var publisher = listViewPublisher.SelectedItem as Publisher;

            publisherNameText.Text = publisher.Name;
        }

        private void listViewGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (listViewGenre.SelectedIndex == -1)
                return;

            var genre = listViewGenre.SelectedItem as Genre;

            genreNameText.Text = genre.Name;
        }


        // ---------------------------------------------------------------------------------------- //


        private void addAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (firstNameText.Text == string.Empty)
            {
                MessageBox.Show(Properties.AddAuthorWindowStrings.MsgEmptyFirstName, Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new BookStoreContext(_connectionString))
            {
                try
                {
                    db.Authors.Add(new Author()
                    {
                        FirstName = firstNameText.Text != string.Empty ? firstNameText.Text : null,
                        LastName = lastNameText.Text != string.Empty ? lastNameText.Text : null,
                        Patronymic = patronymicText.Text != string.Empty ? patronymicText.Text : null,
                    });

                    db.SaveChanges();
                    UpdateAuthors();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void changeAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (firstNameText.Text == string.Empty)
            {
                MessageBox.Show(Properties.AddAuthorWindowStrings.MsgEmptyFirstName, Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new BookStoreContext(_connectionString))
            {
                try
                {
                    var author = db.Authors.First(a => a.Id == (listViewAuthors.SelectedItem as Author).Id);
                    author.FirstName = firstNameText.Text != string.Empty ? firstNameText.Text : null;
                    author.LastName = lastNameText.Text != string.Empty ? lastNameText.Text : null;
                    author.Patronymic = patronymicText.Text != string.Empty ? patronymicText.Text : null;

                    db.SaveChanges();
                    UpdateAuthors();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void deleteAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BookStoreContext(_connectionString))
            {
                try
                {
                    db.Authors.Remove(listViewAuthors.SelectedItem as Author);
                    db.SaveChanges();
                    UpdateAuthors();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void addPublisherButton_Click(object sender, RoutedEventArgs e)
        {
            if (publisherNameText.Text == string.Empty)
            {
                MessageBox.Show("publisher name is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new BookStoreContext(_connectionString))
            {
                try
                {
                    db.Publishers.Add(new Publisher()
                    {
                        Name = publisherNameText.Text
                    });

                    db.SaveChanges();
                    UpdatePublishers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void changePublisherButton_Click(object sender, RoutedEventArgs e)
        {
            if (publisherNameText.Text == string.Empty)
            {
                MessageBox.Show("publisher name is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new BookStoreContext(_connectionString))
            {
                try
                {
                    var publisher = db.Publishers.First(a => a.Id == (listViewPublisher.SelectedItem as Publisher).Id);
                    publisher.Name = publisherNameText.Text;

                    db.SaveChanges();
                    UpdatePublishers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void deletePublisherButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BookStoreContext(_connectionString))
            {
                try
                {
                    db.Publishers.Remove(listViewPublisher.SelectedItem as Publisher);
                    db.SaveChanges();
                    UpdatePublishers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void addGenreButton_Click(object sender, RoutedEventArgs e)
        {
            if (genreNameText.Text == string.Empty)
            {
                MessageBox.Show("genre name is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new BookStoreContext(_connectionString))
            {
                try
                {
                    db.Genres.Add(new Genre()
                    {
                        Name = genreNameText.Text
                    });

                    db.SaveChanges();
                    UpdateGenres();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void changeGenreButton_Click(object sender, RoutedEventArgs e)
        {
            if (genreNameText.Text == string.Empty)
            {
                MessageBox.Show("genre name is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new BookStoreContext(_connectionString))
            {
                try
                {
                    var genre = db.Genres.First(a => a.Id == (listViewGenre.SelectedItem as Genre).Id);
                    genre.Name = genreNameText.Text;

                    db.SaveChanges();
                    UpdateGenres();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void deleteGenreButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BookStoreContext(_connectionString))
            {
                try
                {
                    db.Genres.Remove(listViewGenre.SelectedItem as Genre);
                    db.SaveChanges();
                    UpdateGenres();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
