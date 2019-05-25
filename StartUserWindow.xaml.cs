using System.Data.SqlClient;
using System.Windows;

namespace MyKitchenApp
{
    public partial class StartUserWindow : Window
    {
        public static int? user_id = Get_UserID();

        public StartUserWindow()
        {
            InitializeComponent();
            HelloBox.Content = $"Hi, {ShoppingList.Get_UserName()}";
            user_id = Get_UserID();
        }

        public static int Get_UserID()
        {
            string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT USER_ID FROM USERS WHERE LOGIN = '{MainWindow.log}' AND PASSWORD = '{RegisterWindow.GetHashString(MainWindow.pass)}';";
                int user_id = (int)command.ExecuteScalar();
                cn.Close();
                return user_id;
            }
        }

        #region Windows
        private void logout(object sender, RoutedEventArgs e)
        {
            user_id = null;
            MainWindow mainWindow = new MainWindow();
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Show();
            Close();
        }
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

        private void ShoppingList_Click(object sender, RoutedEventArgs e)
        {
            ShoppingList shoppingList = new ShoppingList();
            shoppingList.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            shoppingList.Show();
            Close();
        }

        private void GoCalc_Click(object sender, RoutedEventArgs e)
        {
            CalcWindow calcWindow = new CalcWindow();
            calcWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            calcWindow.Show();
            Close();
        }

        private void GoMyRecipes_Click(object sender, RoutedEventArgs e)
        {
            MyRecipes myRecipes = new MyRecipes();
            myRecipes.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            myRecipes.Show();
            Close();
        }

        private void MyProducts_Click(object sender, RoutedEventArgs e)
        {
            MyProducts myProducts = new MyProducts();
            myProducts.Show();
            Close();
        }
        #endregion


    }
}
