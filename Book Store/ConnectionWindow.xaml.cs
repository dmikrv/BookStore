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
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        public string ConnectionString { get; private set; }
        public ConnectionWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // MSSQL db
            var connectionString = $@"Server={serverName.Text};Database={Properties.ConnectionWindowStrings.ServerDatabaseNameDefault};"
                + $@"User id={serverLogin.Text};Password={serverPassword.Password}";

            using BookStoreContext db = new BookStoreContext(connectionString);

            var canConnection = db.Database.CanConnect();
            if (canConnection)
            {
                ConnectionString = connectionString;
                this.DialogResult = true;
                MessageBox.Show(Properties.ConnectionWindowStrings.MsgConnectedDb, Properties.ConnectionWindowStrings.WindowTitle, 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(Properties.ConnectionWindowStrings.MsgNoConnectedDb, Properties.ConnectionWindowStrings.WindowTitle, 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
