using System.Data.SqlClient;
using System.Windows;

namespace MyKitchenApp
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int amount;
            if (int.TryParse(AmountBox.Text, out amount))
            { }
            else
                MessageBox.Show("You have entered a wrong data");

            int cal;
            if (int.TryParse(CalorificBox.Text, out cal))
            { }
            else
                MessageBox.Show("You have entered a wrong data");

            string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";
            string sqlExpression = $"INSERT INTO PRODUCTS(KITCHEN_ID, NAME, AMOUNT, CALORIFIC, DATE, EXP_DATE) VALUES({StartUserWindow.user_id},'{ProductNameBox.Text}', {amount}, {cal}, '{PurchaseDateBox.Text}', '{ExpDateBox.Text}');";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
                MessageBox.Show("Your product added");
                connection.Close();
            }
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
