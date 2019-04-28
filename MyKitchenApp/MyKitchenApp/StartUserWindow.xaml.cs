using System.Data.SqlClient;
using System.Windows;

namespace MyKitchenApp
{
    public partial class StartUserWindow : Window
    {
        public StartUserWindow()
        {
            InitializeComponent();
        }

        public static int Get_UserID()
        {
            string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT USER_ID FROM USERS WHERE LOGIN = '{MainWindow.log}' AND PASSWORD = '{MainWindow.pass}';";
                int user_id = (int)command.ExecuteScalar();
                cn.Close();
                return user_id;
            }
        }
        public static int user_id = Get_UserID();

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addProductWindow.Show();
            Close();
        }

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            AddRecipeWindow addRecipeWindow = new AddRecipeWindow();
            addRecipeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addRecipeWindow.Show();
            Close();
        }
    }
}
