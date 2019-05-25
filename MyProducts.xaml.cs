using System;
using System.Collections.Generic;
using System.Data;
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
using System.Configuration;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyKitchenApp
{
    //ConfigurationManager.ConnectionStrings["connect"].ConnectionString
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

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            StartUserWindow startUserWindow = new StartUserWindow();
            startUserWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            startUserWindow.Show();
            Close();
        }
       
        void FillDataset()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();

                DataTable prodTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT NAME, AMOUNT, CALORIFIC, DATE FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id};", ConnectionString);
                adapter.Fill(prodTable);
                products.DataContext = prodTable.DefaultView;

                cn.Close();
            }
        }
    }
}
