using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace MyKitchenApp
{
    public partial class RegisterWindow : Window
    {
        string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text.Length == 0 || PasswordBox.Password.Length == 0 || NameBox.Text.Length == 0 || EmailBox.Text.Length == 0)
            {
                MessageBox.Show("Boxes mustn't be empty");
                return;
            }


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
                        comm.CommandText = $"INSERT INTO USERS(LOGIN, PASSWORD, NAME, EMAIL) VALUES('{LoginBox.Text}','{PasswordBox.Password}', '{NameBox.Text}', '{EmailBox.Text}');";
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
