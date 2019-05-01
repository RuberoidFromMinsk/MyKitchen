using System.Data.SqlClient;
using System.Text;
using System.Windows;

namespace MyKitchenApp
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";

        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(ProductNameBox.Text.Length == 0)
            {
                MessageBox.Show("Please, enter product name.");
                return;
            }

            int amount;
            if (AmountBox.Text.Length == 0)
            {
                MessageBox.Show("Please, enter product amount.");
                return;
            }
            else
            {
                if (int.TryParse(AmountBox.Text, out amount))
                {
                    if(amount <= 0)
                    {
                        MessageBox.Show("You entered incorrect amount");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You entered incorrect amount");
                    return;
                }
            }

            string date;
            if (PurchaseDateBox.Text.Length == 0)
                date = "GETDATE()";
            else
                date = PurchaseDateBox.Text;

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT COUNT(*) FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id} AND NAME = '{ProductNameBox.Text}';";
                int count = (int)command.ExecuteScalar();
                if(count == 0)
                {
                    #region calorific
                    int cal;
                    if (CalorificBox.Text.Length == 0)
                    {
                        MessageBox.Show("Please, enter product calorific.");
                        return;
                    }
                    else
                    {
                        if (int.TryParse(CalorificBox.Text, out cal))
                        {
                            if (cal <= 0)
                            {
                                MessageBox.Show("You entered incorrect calorific");
                                return;
                            }
                            if(cal >= 950)
                            {
                                MessageBox.Show("That's too big calorific. Change it.");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("You entered incorrect calorific");
                            return;
                        }
                    }
                    #endregion

                    if(date.Equals("GETDATE()"))
                        command.CommandText = $"INSERT INTO PRODUCTS(KITCHEN_ID, NAME, AMOUNT, CALORIFIC, DATE) VALUES({StartUserWindow.user_id},'{ProductNameBox.Text}', {amount}, {cal}, {date});";
                    else
                        command.CommandText = $"INSERT INTO PRODUCTS(KITCHEN_ID, NAME, AMOUNT, CALORIFIC, DATE) VALUES({StartUserWindow.user_id},'{ProductNameBox.Text}', {amount}, {cal}, '{date}');";
                    command.ExecuteNonQuery();
                    MessageBox.Show("Your product added");
                }
                else
                {
                    if (date.Equals("GETDATE()"))
                        command.CommandText = $"UPDATE PRODUCTS SET AMOUNT = AMOUNT + {amount} AND DATE = {date} WHERE KITCHEN_ID = {StartUserWindow.user_id} AND NAME = '{ProductNameBox.Text}';";
                    else
                        command.CommandText = $"UPDATE PRODUCTS SET AMOUNT = AMOUNT + {amount} AND DATE = '{date}' WHERE KITCHEN_ID = {StartUserWindow.user_id} AND NAME = '{ProductNameBox.Text}';";
                    command.ExecuteNonQuery();
                    MessageBox.Show("Your product updated");
                }
                cn.Close();
            }

            ProductNameBox.Clear();
            CalorificBox.Clear();
            AmountBox.Clear();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            StartUserWindow startWindow = new StartUserWindow();
            startWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            startWindow.Show();
            this.Close();
        }
    }

}
