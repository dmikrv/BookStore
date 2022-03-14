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
using System.Windows.Shapes;

namespace Book_Store
{
    /// <summary>
    /// Interaction logic for AddAuthorWindow.xaml
    /// </summary>
    public partial class AddAuthorWindow : Window
    {
        private readonly string _connectionString;
        public AddAuthorWindow(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
        }

        private void addAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            //if (firstNameText.Text == string.Empty) 
            //{
            //    MessageBox.Show(Properties.AddAuthorWindowStrings.MsgEmptyFirstName, Properties.AddAuthorWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            //using (var db = new BookStoreContext(_connectionString))
            //{
            //    db.Authors.Add(new Models.Author()
            //    {
            //        Human = new Models.Human()
            //        {
            //            FirstName = firstNameText.Text,
            //            LastName = lastNameText.Text != string.Empty ? lastNameText.Text : null,
            //            Patronymic = patronymicText.Text != string.Empty ? patronymicText.Text : null,
            //        }
            //    });

            //    try
            //    {
            //        db.SaveChanges();
            //        firstNameText.Text = string.Empty;
            //        lastNameText.Text = string.Empty;
            //        patronymicText.Text = string.Empty;
            //        MessageBox.Show(Properties.AddAuthorWindowStrings.MsgAddedAuthor, Properties.AddAuthorWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Information);

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message, Properties.AddAuthorWindowStrings.WindowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
        }
    }
}
