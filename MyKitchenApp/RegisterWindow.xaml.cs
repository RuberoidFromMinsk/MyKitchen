using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace MyKitchenApp
{
    public partial class RegisterWindow : Window
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;

        public RegisterWindow()
        {
            InitializeComponent();
        }

        public static string GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s); 
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider(); 
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = string.Empty; 
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);
            return hash;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            #region data checks
            Regex regex = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
            if(!regex.IsMatch(EmailBox.Text))
            {
                MessageBox.Show("Incorrect email");
                return;
            }

            if (PasswordBox.Password.Trim().Length == 0 || NameBox.Text.Trim().Length == 0 || EmailBox.Text.Trim().Length == 0 || LoginBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("The box must not be full of spaces");
                return;
            }

            if(PasswordBox.Password.Length < 4 || LoginBox.Text.Length < 4)
            {
                MessageBox.Show("Login and pass must contain 4 and more symbols");
                return;
            }

            if (NameBox.Text.Length > 20)
            {
                MessageBox.Show("Too long name, change it");
                return;
            }
            #endregion

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT COUNT(*) FROM USERS WHERE LOGIN = '{LoginBox.Text}';";
                if (!command.ExecuteScalar().Equals(0))
                {
                    MessageBox.Show("User with the same login exist. Change your login.");
                    return;
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                        SqlCommand comm = cn.CreateCommand();
                        comm.CommandText = $"INSERT INTO USERS(LOGIN, PASSWORD, NAME, EMAIL) VALUES('{LoginBox.Text}','{GetHashString(PasswordBox.Password)}', '{NameBox.Text}', '{EmailBox.Text}');";
                        comm.ExecuteNonQuery();
                        MessageBox.Show("Register completed!");
                        con.Close();
                    }
                }

                cn.Close();
            }

            LoginBox.Clear();
            PasswordBox.Clear();
            NameBox.Clear();
            EmailBox.Clear();

            MainWindow mainWindow = new MainWindow();
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Show();
            Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Show();
            Close();
        }
    }
}
