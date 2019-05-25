using System.Data.SqlClient;
using System.Drawing;
using System.Windows;

namespace MyKitchenApp
{
    /// <summary>
    /// Логика взаимодействия для CalcWindow.xaml
    /// </summary>
    public partial class CalcWindow : Window
    {
        string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";

        public CalcWindow()
        {
            InitializeComponent();
            FillCombobox();
        }

        void FillCombobox()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT NAME FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id} AND AMOUNT != 0;";
                SqlDataReader reader;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        string result = reader.GetString(0);
                        ProductItems.Items.Add(result);
                    }
                    catch { }
                }
                cn.Close();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            StartUserWindow startUserWindow = new StartUserWindow();
            startUserWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            startUserWindow.Show();
            Close();
        }

        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            int? result = null;
            int? cal = null;
            int userCal;
            if (int.TryParse(AmountBox.Text, out userCal))
            {
                if(userCal > 100000 || userCal <= 0)
                {
                    MessageBox.Show("Incorrect amount");
                    return;
                }
            }
            else
            {
                MessageBox.Show("You entered wrong data");
                return;
            }

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT CALORIFIC FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id} AND NAME = '{ProductItems.Text}';";
                cal = (int)command.ExecuteScalar();
                cn.Close();
            }
            result = (cal * userCal) / 100;
            //ResultLabel.TextAlign = ContentAlignment.MiddleCenter;

            ResultLabel.Text = result.ToString() + " ccal";
        }
    }
}
