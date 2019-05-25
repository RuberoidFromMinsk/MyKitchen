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
    public partial class MyRecipes : Window
    {
        string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";

        public MyRecipes()
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
                command.CommandText = $"SELECT NAME FROM RECIPES WHERE KITCHEN_ID = {StartUserWindow.user_id};";
                SqlDataReader reader;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        string result = reader.GetString(0);
                        RecipeItems.Items.Add(result);
                    }
                    catch { }
                }
                cn.Close();
            }
        }

        int Get_RecipeID()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT ID FROM RECIPES WHERE KITCHEN_ID = {StartUserWindow.user_id} AND NAME = '{RecipeItems.Text}';";
                int id = (int)command.ExecuteScalar();
                cn.Close();
                return id;
            }
        }

        void AddIngredientsToSB(StringBuilder sb)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand comm = cn.CreateCommand();
                comm.CommandText = $"SELECT MAX(A.AMOUNT), B.NAME FROM PRODUCTS_RECIPES A JOIN PRODUCTS B ON A.PRODUCT_ID = B.ID WHERE A.RECIPE_ID = {Get_RecipeID()} GROUP BY B.NAME;";
                SqlDataReader reader_products = comm.ExecuteReader();
                if (reader_products.HasRows)
                {
                    while (reader_products.Read())
                    {
                        try
                        {
                            object amount = reader_products.GetValue(0);
                            object name = reader_products.GetValue(1);
                            sb.Append("● Name: " + name.ToString() + " , amount: " + amount.ToString() + "\n");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                cn.Close();
            }
        }
    

        #region Clicks
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            StartUserWindow startUserWindow = new StartUserWindow();
            startUserWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            startUserWindow.Show();
            Close();
        }
        #endregion

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT PORTIONS, DESCRIPTION FROM RECIPES WHERE KITCHEN_ID = {StartUserWindow.user_id} AND NAME = '{RecipeItems.Text}';";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            object portions = reader.GetValue(0);
                            object description = reader.GetValue(1);
                            builder.Append("Portions: " + portions.ToString() + "\nDescription: " + description.ToString());
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                builder.Append("\nIngedients:\n");
                AddIngredientsToSB(builder);

                cn.Close();
            }
            ResultBox.Text = builder.ToString();
        }
    }
}
