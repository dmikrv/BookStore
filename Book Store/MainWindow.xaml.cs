﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore;

using Book_Store.Entities;
using Book_Store.Extensions;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;

namespace Book_Store
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Account currentAccount;
        private bool? _hasImageOfBook = null;

        public MainWindow()
        {
            InitializeComponent();

            using (var db = new BookStoreContext())
            {
                db.Database.Migrate();
            }

            LogEntryList.ItemsSource = LogEntryLoggerProvider.LogEntites;

            LogOut();
            Authorization();

            loginText.Text = "admin";
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // skip bubbling event
            if (e.Source is not TabControl)
                return;

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
            else if (header == "Discounts")
            {
                UpdateDiscounts();
            }
            else if (header == "Showcase")
            {
                UpdateShowcase();
            }

        }
        private void UpdateShowcase()
        {
            using var db = new BookStoreContext();

            showcaseListView.ItemsSource = db.Books.Include(x => x.Author).Include(nameof(Genre)).Include(nameof(Publisher))
                .Include(nameof(Entities.Image)).ToList();

            guestFirstName.Text = string.Empty;
            guestLastName.Text = string.Empty;
            guestPhone.Text = string.Empty;
        }

        private void UpdateAuthors()
        {
            using (var db = new BookStoreContext())
            {
                listViewAuthors.ItemsSource = (from author in db.Authors
                                               orderby author.FirstName, author.LastName
                                               select author).ToList();
            }

            firstNameText.Text = string.Empty;
            lastNameText.Text = string.Empty;
            patronymicText.Text = string.Empty;
        }

        private void UpdateBooks()
        {
            using var db = new BookStoreContext();

            var books = db.Books
                .Include(nameof(Author))
                .Include(nameof(Genre))
                .Include(nameof(Publisher))
                .Include(nameof(Entities.Image));

            if (!string.IsNullOrEmpty(searchBookText.Text))
            {
                books = books.Where(x => x.Name.Contains(searchBookText.Text));
            }
            if (showDecommissionBooksCheckBox.IsChecked == false)
            {
                books = books.Where(x => !db.DecommissionedBooks.Select(x => x.BookId).Contains(x.Id));
            }
            listViewBooks.ItemsSource = books.OrderBy(x => x.Name).ToList();

            nameBookText.Text = string.Empty;

            authorComboBox.ItemsSource = db.Authors.ToList();
            authorComboBox.SelectedIndex = 0;

            publisherCheckBox.IsChecked = false;
            publisherComboBox.IsEnabled = false;
            publisherComboBox.ItemsSource = db.Publishers.ToList();
            publisherComboBox.SelectedIndex = -1;

            pagesBookText.Text = string.Empty;

            genreCheckBox.IsChecked = false;
            genreComboBox.IsEnabled = false;
            genreComboBox.ItemsSource = db.Genres.ToList();
            genreComboBox.SelectedIndex = -1;

            yearPublishingText.Text = string.Empty;
            costPriceText.Text = string.Empty;
            priceText.Text = string.Empty;

            previousBookCheckBox.IsChecked = false;
            previousBookComboBox.ItemsSource = db.Books.ToList();

            deleteBookButton.IsEnabled = false;
            changeBookButton.IsEnabled = false;
            decommissionBookButton.IsEnabled = false;

            imageViewer.Source = new BitmapImage().GetImage(Properties.DefaultBookCovers.default_book_cover);
            _hasImageOfBook = false;
        }

        private void UpdatePublishers()
        {
            using (var db = new BookStoreContext())
            {
                listViewPublisher.ItemsSource = (from publisher in db.Publishers
                                                 orderby publisher.Name
                                                 select publisher).ToList();
            }
            publisherNameText.Text = string.Empty;
        }

        private void UpdateGenres()
        {
            using (var db = new BookStoreContext())
            {
                listViewGenre.ItemsSource = (from genre in db.Genres
                                             orderby genre.Name
                                             select genre).ToList();
            }
            genreNameText.Text = string.Empty;
        }

        private void UpdateDiscounts()
        {
            using (var db = new BookStoreContext())
            {
                listViewDiscountsDiscount.ItemsSource = db.Discounts.OrderBy(x => x.Percent).ToList();
            }
            listViewDiscountsBooksIncluded.Items.Clear();
            UpdateDiscountsBooksNoIncluded();

            discountNameText.Text = string.Empty;
            discountPercentUpDown.Value = discountPercentUpDown.Minimum;
            discountStartPicker.SelectedDate = DateTime.Now;
            discountEndPicker.SelectedDate = DateTime.Now.AddDays(1);

            deleteDiscountButton.IsEnabled = false;
            changeDiscountButton.IsEnabled = false;
        }

        private void UpdateDiscountsBooksNoIncluded()
        {
            using (var db = new BookStoreContext())
            {
                var included = listViewDiscountsBooksIncluded.Items.OfType<Book>().Select(x => x.Id).ToArray();

                listViewDiscountsBooksNoIncluded.ItemsSource = db.Books
                    .Include(nameof(Author))
                    .Include(nameof(Genre))
                    .Include(nameof(Publisher))
                    .AsEnumerable()
                    .Where(x => !included.Contains(x.Id))
                    .OrderBy(x => x.Name)
                    .ToList();
            }
        }

        // ---------------------------------------------------------------------------------------- //


        private void listViewAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewAuthors.SelectedIndex == -1)
                return;

            var author = listViewAuthors.SelectedItem as Author;

            firstNameText.Text = author.FirstName;
            lastNameText.Text = author.LastName ?? string.Empty;
            patronymicText.Text = author.Patronymic ?? string.Empty;
        }

        private void listViewBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewBooks.SelectedIndex == -1)
                return;

            var book = listViewBooks.SelectedItem as Book;

            nameBookText.Text = book.Name;
            authorComboBox.SelectedItem = book.Author;
            pagesBookText.Text = book.Pages.ToString();
            yearPublishingText.Text = book.YearPublishing.Year.ToString();
            costPriceText.Text = String.Format("{0:0.00}", book.CostPrice);
            priceText.Text = String.Format("{0:0.00}", book.Price);
            changeBookButton.IsEnabled = true;
            deleteBookButton.IsEnabled = true;

            if (book.Publisher is not null)
            {
                publisherCheckBox.IsChecked = true;
                publisherComboBox.SelectedItem = book.Publisher;
            }
            else
            {
                publisherCheckBox.IsChecked = false;
                publisherComboBox.SelectedIndex = -1;
            }

            if (book.Genre is not null)
            {
                genreCheckBox.IsChecked = true;
                genreComboBox.SelectedItem = book.Genre;
            }
            else
            {
                genreCheckBox.IsChecked = false;
                genreComboBox.SelectedIndex = -1;
            }

            // continuation and decommissioned books, image
            Book previousBook;
            //Entities.Image image;
            bool isDecommissionedBook;

            using (var db = new BookStoreContext())
            {
                previousBookComboBox.ItemsSource = (from x in db.Books
                                                    where x.Id != book.Id && x.Id != (from y in db.ContinuationBooks
                                                                                      where book.Id == y.PredecessorBookId
                                                                                      select y.BookId).FirstOrDefault()
                                                    select x).ToList();

                previousBook = (from x in previousBookComboBox.ItemsSource.OfType<Book>()
                                where x.Id == (from b in db.ContinuationBooks
                                               where b.BookId == book.Id
                                               select b.PredecessorBookId).FirstOrDefault()
                                select x).FirstOrDefault();

                isDecommissionedBook = (from x in db.DecommissionedBooks
                                        where x.BookId == book.Id
                                        select x).Any();

                imageViewer.Source = new BitmapImage().GetImage(book.ImageId != null
                    ? book.Image.ImageData : db.Images.First(x => x.ImageTitle == "default").ImageData);
                _hasImageOfBook = book.ImageId != null;
            }

            if (previousBook is not null)
            {
                previousBookCheckBox.IsChecked = true;
                previousBookComboBox.SelectedItem = previousBook;
            }
            else
            {
                previousBookCheckBox.IsChecked = false;
                previousBookComboBox.SelectedIndex = -1;
            }

            decommissionBookButton.IsEnabled = true;
            decommissionBookButton.Content = isDecommissionedBook ? "Return" : "Decommission";
        }

        private void listViewPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewPublisher.SelectedIndex == -1)
                return;

            var publisher = listViewPublisher.SelectedItem as Publisher;

            publisherNameText.Text = publisher.Name;
        }

        private void listViewGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewGenre.SelectedIndex == -1)
                return;

            var genre = listViewGenre.SelectedItem as Genre;

            genreNameText.Text = genre.Name;
        }


        private void listViewDiscountsDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewDiscountsDiscount.SelectedIndex == -1)
                return;

            var discount = listViewDiscountsDiscount.SelectedItem as Discount;
            discountNameText.Text = discount.Name;
            discountPercentUpDown.Value = (int)discount.Percent;
            discountStartPicker.SelectedDate = discount.StartDate;
            discountEndPicker.SelectedDate = discount.EndDate;

            changeDiscountButton.IsEnabled = true;
            deleteDiscountButton.IsEnabled = true;

            listViewDiscountsBooksIncluded.Items.Clear();

            using (var db = new BookStoreContext())
            {
                var includedBooksId = db.BookDiscounts.Where(x => x.DiscountId == discount.Id).Select(y => y.BookId).ToList();

                var includedBooks = db.Books.Include(nameof(Author))
                    .Include(nameof(Genre)).Include(nameof(Publisher)).AsEnumerable()
                    .Where(x => includedBooksId.Contains(x.Id)).ToList();

                foreach (var book in includedBooks)
                {
                    listViewDiscountsBooksIncluded.Items.Add(book);
                }
            }
            UpdateDiscountsBooksNoIncluded();
        }


        // ---------------------------------------------------------------------------------------- //


        private void addAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (firstNameText.Text == string.Empty)
            {
                MessageBox.Show("First name is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new BookStoreContext())
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
                MessageBox.Show("First name is empty!", Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new BookStoreContext())
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
            using (var db = new BookStoreContext())
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

        private void decommissionBookButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BookStoreContext())
            {
                try
                {
                    if (decommissionBookButton.Content.ToString() == "Decommission")
                    {
                        db.DecommissionedBooks.Add(new DecommissionedBook()
                        {
                            BookId = (listViewBooks.SelectedItem as Book).Id
                        });
                    }
                    else if ((decommissionBookButton.Content.ToString() == "Return"))
                    {
                        db.DecommissionedBooks.Remove(db.DecommissionedBooks.First(
                            x => x.BookId == (listViewBooks.SelectedItem as Book).Id
                            ));
                    }

                    db.SaveChanges();
                    UpdateBooks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void updateDbBookButton_Click(object sender, RoutedEventArgs e)
        {
            searchBookText.Text = string.Empty;
            UpdateBooks();
        }

        private void addPublisherButton_Click(object sender, RoutedEventArgs e)
        {
            if (publisherNameText.Text == string.Empty)
            {
                MessageBox.Show("publisher name is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new BookStoreContext())
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

            using (var db = new BookStoreContext())
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
            using (var db = new BookStoreContext())
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

            using (var db = new BookStoreContext())
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

            using (var db = new BookStoreContext())
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
            using (var db = new BookStoreContext())
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

        private void addBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckTextBoxsOfBook())
                return;

            using (var db = new BookStoreContext())
            {
                try
                {
                    var newBook = db.Books.Add(new Book()
                    {
                        Name = nameBookText.Text,
                        AuthorId = (authorComboBox.SelectedItem as Author).Id,
                        PublisherId = publisherCheckBox.IsChecked == true ? (publisherComboBox.SelectedItem as Publisher).Id : null,
                        Pages = int.Parse(pagesBookText.Text),
                        GenreId = genreCheckBox.IsChecked == true ? (genreComboBox.SelectedItem as Genre).Id : null,
                        YearPublishing = new DateTime(int.Parse(yearPublishingText.Text), 1, 1),
                        CostPrice = decimal.Parse(costPriceText.Text),
                        Price = decimal.Parse(priceText.Text),
                        Image = _hasImageOfBook == true
                            ? new Entities.Image { ImageData = (imageViewer.Source as BitmapImage).GetBytes() } : null
                    });

                    if (previousBookCheckBox.IsChecked == true)
                    {
                        db.ContinuationBooks.Add(new ContinuationBook()
                        {
                            Book = newBook.Entity,
                            PredecessorBookId = (previousBookComboBox.SelectedItem as Book).Id,
                        });
                    }

                    db.SaveChanges();
                    UpdateBooks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void changeBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckTextBoxsOfBook())
                return;

            using (var db = new BookStoreContext())
            {
                try
                {
                    var book = db.Books.First(x => x.Id == (listViewBooks.SelectedItem as Book).Id);

                    book.Name = nameBookText.Text;
                    book.AuthorId = (authorComboBox.SelectedItem as Author).Id;
                    book.PublisherId = publisherCheckBox.IsChecked == true ? (publisherComboBox.SelectedItem as Publisher).Id : null;
                    book.Pages = int.Parse(pagesBookText.Text);
                    book.GenreId = genreCheckBox.IsChecked == true ? (genreComboBox.SelectedItem as Genre).Id : null;
                    book.YearPublishing = new DateTime(int.Parse(yearPublishingText.Text), 1, 1);
                    book.CostPrice = decimal.Parse(costPriceText.Text);
                    book.Price = decimal.Parse(priceText.Text);

                    if (_hasImageOfBook == true)
                        book.Image = new Entities.Image { ImageData = (imageViewer.Source as BitmapImage).GetBytes() };
                    else
                        book.ImageId = null;

                    var continuationBook = (from x in db.ContinuationBooks where book.Id == x.BookId select x).FirstOrDefault();
                    if (previousBookCheckBox.IsChecked == true)
                    {
                        if (continuationBook is not null)
                        {
                            continuationBook.PredecessorBookId = (previousBookComboBox.SelectedItem as Book).Id;
                        }
                        else
                        {
                            db.ContinuationBooks.Add(new ContinuationBook()
                            {
                                Book = book,
                                PredecessorBookId = (previousBookComboBox.SelectedItem as Book).Id,
                            });
                        }
                    }
                    else
                    {
                        if (continuationBook is not null)
                            db.ContinuationBooks.Remove(continuationBook);
                    }

                    db.SaveChanges();
                    UpdateBooks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void deleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BookStoreContext())
            {
                try
                {
                    db.Books.Remove(listViewBooks.SelectedItem as Book);
                    db.SaveChanges();
                    UpdateBooks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void addDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckTextBoxsOfDiscount())
                return;

            using (var db = new BookStoreContext())
            {
                try
                {
                    var discount = db.Discounts.Add(new Discount()
                    {
                        Name = discountNameText.Text,
                        Percent = discountPercentUpDown.Value.Value,
                        StartDate = discountStartPicker.SelectedDate.Value,
                        EndDate = discountEndPicker.SelectedDate.Value,
                    });

                    foreach (Book item in listViewDiscountsBooksIncluded.Items)
                    {
                        db.BookDiscounts.Add(new BookDiscount()
                        {
                            Discount = discount.Entity,
                            BookId = item.Id
                        });
                    }

                    db.SaveChanges();
                    UpdateDiscounts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void deleteDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BookStoreContext())
            {
                try
                {
                    db.Discounts.Remove(listViewDiscountsDiscount.SelectedItem as Discount);

                    db.SaveChanges();
                    UpdateDiscounts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void changeDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckTextBoxsOfDiscount())
                return;

            using (var db = new BookStoreContext())
            {
                var discount = db.Discounts.First(x => x.Id == (listViewDiscountsDiscount.SelectedItem as Discount).Id);

                try
                {
                    discount.Name = discountNameText.Text;
                    discount.Percent = discountPercentUpDown.Value.Value;
                    discount.StartDate = discountStartPicker.SelectedDate.Value;
                    discount.EndDate = discountEndPicker.SelectedDate.Value;

                    db.BookDiscounts.RemoveRange(db.BookDiscounts.Where(x => x.DiscountId == discount.Id));

                    foreach (Book item in listViewDiscountsBooksIncluded.Items)
                    {
                        db.BookDiscounts.Add(new BookDiscount()
                        {
                            Discount = discount,
                            BookId = item.Id
                        });
                    }

                    db.SaveChanges();
                    UpdateDiscounts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        // ---------------------------------------------------------------------------------------- //


        private bool CheckTextBoxsOfBook()
        {
            if (nameBookText.Text == string.Empty)
            {
                MessageBox.Show("book name is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (pagesBookText.Text == string.Empty)
            {
                MessageBox.Show("pages field is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (costPriceText.Text == string.Empty)
            {
                MessageBox.Show("cost price field is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (priceText.Text == string.Empty)
            {
                MessageBox.Show("price field is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (yearPublishingText.Text == string.Empty || int.Parse(yearPublishingText.Text) < 1 
                || int.Parse(yearPublishingText.Text) > 9999)
            {
                MessageBox.Show("year of publishing is not correct!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool CheckTextBoxsOfDiscount()
        {
            if (discountNameText.Text == string.Empty)
            {
                MessageBox.Show("discount name is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (discountEndPicker.SelectedDate < discountStartPicker.SelectedDate)
            {
                MessageBox.Show("end date < start date!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //if ((from x in listViewDiscountsDiscount.ItemsSource.OfType<Discount>() select x.Name).Contains())
            //{
            //    MessageBox.Show("name is already exist!", Properties.MainWindowStrings.WindowTitle,
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}

            return true;
        }

        private bool CheckTextBoxsOfMakeOrder()
        {
            if (guestFirstName.Text == string.Empty)
            {
                MessageBox.Show("first name is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (guestLastName.Text == string.Empty)
            {
                MessageBox.Show("last name is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (guestPhone.Text == string.Empty)
            {
                MessageBox.Show("phone is empty!", Properties.MainWindowStrings.WindowTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        // ---------------------------------------------------------------------------------------- //


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

        private void genreCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            genreComboBox.IsEnabled = true;
        }

        private void publisherCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            publisherComboBox.IsEnabled = true;
        }

        private void publisherCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            publisherComboBox.IsEnabled = false;
        }

        private void genreCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            genreComboBox.IsEnabled = false;
        }

        private void previousBookCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            previousBookComboBox.IsEnabled = true;
        }

        private void previousBookCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            previousBookComboBox.IsEnabled = false;
        }

        private void searchBookText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateBooks();
        }

        private void showDecommissionBooksCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateBooks();
        }

        private void showDecommissionBooksCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateBooks();
        }
        private void updateDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateDiscounts();
        }

        private void discountBookUpButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Book book in listViewDiscountsBooksNoIncluded.SelectedItems)
            {
                listViewDiscountsBooksIncluded.Items.Add(book);
            }
            UpdateDiscountsBooksNoIncluded();
        }

        private void discountBookDownButton_Click(object sender, RoutedEventArgs e)
        {
            var oldBooks = listViewDiscountsBooksIncluded.SelectedItems.OfType<Book>().ToList();
            foreach (Book book in oldBooks)
            {
                listViewDiscountsBooksIncluded.Items.Remove(book);
            }
            UpdateDiscountsBooksNoIncluded();
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            LogIn();
            Authorization();
        }

        private void logOutButton_Click(object sender, RoutedEventArgs e)
        {
            LogOut();
            Authorization();
        }

        private void Authorization()
        {
            foreach(TabItem item in tabControl.Items)
            {
                item.Visibility = Visibility.Collapsed;
            }

            switch ((RoleType)currentAccount.RoleId)
            {
                case RoleType.ADMIN:
                    tabItemBooks.Visibility = Visibility.Visible;
                    tabItemDiscounts.Visibility = Visibility.Visible;
                    tabItemAuthors.Visibility = Visibility.Visible;
                    tabItemPublisher.Visibility = Visibility.Visible;
                    tabItemGenre.Visibility = Visibility.Visible;
                    tabItemAuth.Visibility = Visibility.Visible;
                    tabItemLogs.Visibility = Visibility.Visible;
                    logOutButton.IsEnabled = true;
                    loginLabel.Content = currentAccount.Login;
                    break;
                case RoleType.USER:
                    break;
                case RoleType.GUEST:
                    tabItemAuth.Visibility = Visibility.Visible;
                    tabItemShowcase.Visibility = Visibility.Visible;
                    logOutButton.IsEnabled = false;
                    loginLabel.Content = "guest";
                    break;
                default:
                    break;
            }
        }

        private void LogIn()
        {
            using var db = new BookStoreContext();
            var user = db.Accounts.FirstOrDefault(x => x.Login.ToLower() == loginText.Text.ToLower());

            if (user is not null)
            {
                currentAccount = user;
            }

            loginText.Text = string.Empty;
            passwordText.Password = string.Empty;
        }

        private void LogOut()
        {
            currentAccount = new Account { RoleId = (int)RoleType.GUEST };
            passwordText.Password = string.Empty;
        }

        private void addCoverBookButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();

                imageViewer.Source = bitmap;
                _hasImageOfBook = true;
            }
        }

        private void deleteCoverBookButton_Click(object sender, RoutedEventArgs e)
        {
            imageViewer.Source = new BitmapImage().GetImage(Properties.DefaultBookCovers.default_book_cover);
            _hasImageOfBook = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var book = showcaseListView.SelectedItem as Book;
            showcaseListView.Items.OfType<Book>().First(x => x.Id == book.Id).IsBought = true;
            showcaseListView.Items.Refresh();
        }

        private void makeOrederButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckTextBoxsOfMakeOrder())
                return;

            using (var db = new BookStoreContext())
            {
                try
                {
                    var customer = db.Customers.Add(new Customer { FirstName = guestFirstName.Text, LastName = guestLastName.Text, Phone = guestPhone.Text });
                    foreach (var book in showcaseListView.Items.OfType<Book>().Where(x => x.IsBought).ToList())
                    {
                        db.Orders.Add(new Order { Customer = customer.Entity, BookId = book.Id });
                    }

                    db.SaveChanges();
                    UpdateShowcase();
                    MessageBox.Show("order is made!", Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Properties.MainWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }
    }

    public class SwitchBindingExtension : Binding
    {
        public SwitchBindingExtension()
        {
            Initialize();
        }

        public SwitchBindingExtension(string path)
            : base(path)
        {
            Initialize();
        }

        public SwitchBindingExtension(string path, object valueIfTrue, object valueIfFalse)
            : base(path)
        {
            Initialize();
            this.ValueIfTrue = valueIfTrue;
            this.ValueIfFalse = valueIfFalse;
        }

        private void Initialize()
        {
            this.ValueIfTrue = Binding.DoNothing;
            this.ValueIfFalse = Binding.DoNothing;
            this.Converter = new SwitchConverter(this);
        }

        [System.Windows.Markup.ConstructorArgument("valueIfTrue")]
        public object ValueIfTrue { get; set; }

        [System.Windows.Markup.ConstructorArgument("valueIfFalse")]
        public object ValueIfFalse { get; set; }

        private class SwitchConverter : IValueConverter
        {
            public SwitchConverter(SwitchBindingExtension switchExtension)
            {
                _switch = switchExtension;
            }

            private SwitchBindingExtension _switch;

            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                try
                {
                    bool b = System.Convert.ToBoolean(value);
                    return b ? _switch.ValueIfTrue : _switch.ValueIfFalse.ToString() == "_" ? "" : _switch.ValueIfFalse;
                }
                catch
                {
                    return DependencyProperty.UnsetValue;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return Binding.DoNothing;
            }

            #endregion
        }

    }
}
