using System.Data.SqlClient;
using System.Windows;

namespace MyKitchenApp
{
    public partial class StartUserWindow : Window
    {
        public StartUserWindow()
        {
            InitializeComponent();
            HelloBox.Content = $"Hi, {ShoppingList.Get_UserName()}";
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

        #region Windows
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
        #endregion


    }
}
