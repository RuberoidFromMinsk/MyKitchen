using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MyKitchenApp
{
    public partial class MainWindow : Window
    {
        public static string log;
        public static string pass;
        string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            log = LoginBox.Text;
            pass = PasswordBox.Password;

            if (log.Equals("") || pass.Equals(""))
            {
                MessageBox.Show("Fill in boxes");
                return;
            }
            
           using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT COUNT(*) FROM USERS WHERE LOGIN = '{LoginBox.Text}' AND PASSWORD = '{RegisterWindow.GetHashString(PasswordBox.Password)}';";
                if (command.ExecuteScalar().Equals(0))
                    MessageBox.Show("User not exists");
                else
                {
                    StartUserWindow startUserWindow = new StartUserWindow();
                    startUserWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    startUserWindow.Show();
                    Close();
                }
                cn.Close();
            }
        }

        private void GoRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            registerWindow.Show();
            Close();
        }
    }
}
