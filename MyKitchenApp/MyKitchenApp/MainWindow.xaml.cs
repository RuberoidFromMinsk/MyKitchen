using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace MyKitchenApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public static string log;
        public static string pass;

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            log = LoginBox.Text;
            pass = PasswordBox.Password;

            if (log.Equals("") || pass.Equals(""))
            {
                MessageBox.Show("Fill in boxes");
                return;
            }
                
            string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT COUNT(*) FROM USERS WHERE LOGIN = '{LoginBox.Text}' AND PASSWORD = '{PasswordBox.Password}';";
                if (command.ExecuteScalar().Equals(0))
                {
                    MessageBox.Show("User not exist");
                }
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
