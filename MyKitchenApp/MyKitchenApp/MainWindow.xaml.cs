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

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (log.Equals("") || pass.Equals(""))
            {
                MessageBox.Show("Boxes mustn't be empty");
                return;
            }

            string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT COUNT(*) FROM USERS WHERE LOGIN = '{LoginBox.Text}';";
                if (!command.ExecuteScalar().Equals(0))
                {
                    MessageBox.Show("A user with the same log exist");
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                        SqlCommand comm = cn.CreateCommand();
                        comm.CommandText = $"INSERT INTO USERS(LOGIN, PASSWORD) VALUES('{LoginBox.Text}','{PasswordBox.Password}');";
                        comm.ExecuteNonQuery();
                        MessageBox.Show("Register completed!");
                        con.Close();
                    }
                }
                    
                cn.Close();
            }

            LoginBox.Clear();
            PasswordBox.Clear();
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
                
            string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT COUNT(*) FROM USERS WHERE LOGIN = '{LoginBox.Text}';";
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
    }
}
