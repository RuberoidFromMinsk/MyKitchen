using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Configuration;

namespace MyKitchenApp
{
    /// <summary>
    /// Логика взаимодействия для MyProducts.xaml
    /// </summary>
    public partial class MyProducts : Window
    {
        public MyProducts()
        {
            InitializeComponent();
            FillDataset();
        }

        string ConnectionString = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;

        void FillDataset()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();

                DataTable prodTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT NAME, AMOUNT, CALORIFIC, DATE FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id} AND AMOUNT > 0;", ConnectionString);
                adapter.Fill(prodTable);
                products.DataContext = prodTable.DefaultView;

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
    }
}
