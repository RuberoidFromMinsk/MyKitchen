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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyKitchenApp
{
    public partial class AddRecipeWindow : Window
    {
        List<int> products_id = new List<int>();
        string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";

        public AddRecipeWindow()
        {
            InitializeComponent();
            FillCombobox();
            FillDataset();
            Clear_SELECTED_PRODUCTS();
        }

        void FillCombobox()
        {
            string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT NAME FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id};";
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

        void FillDataset()
        {
            string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                DataTable prodTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT NAME, AMOUNT FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id}", ConnectionString);
                adapter.Fill(prodTable);
                AvalibleProducts.DataContext = prodTable.DefaultView; 

                cn.Close();
            }
        }

        void Clear_SELECTED_PRODUCTS()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"DELETE FROM SELECTED_PRODUCTS;";
                command.ExecuteNonQuery();
                cn.Close();
            }
        }

        private void AddProductToRecipe_Click(object sender, RoutedEventArgs e)
        {
            DataTable prodTable = new DataTable();
            int amountForCheck = 0;

            int amount;
            if (int.TryParse(AmountBox.Text, out amount))
            { }
            else
                MessageBox.Show("You have entered a wrong data");

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT AMOUNT FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id} AND NAME = '{ProductItems.Text}';";
                amountForCheck = (int)command.ExecuteScalar();
                cn.Close();
            }

            if (amountForCheck < amount)
                MessageBox.Show("Entered amount more than avalible amount");
            else
            {
                using (SqlConnection cn = new SqlConnection(ConnectionString))
                {
                    cn.Open();

                    SqlCommand command = cn.CreateCommand();
                    command.CommandText = $"INSERT INTO SELECTED_PRODUCTS(NAME, AMOUNT) VALUES('{ProductItems.Text}', {amount});";
                    command.ExecuteNonQuery();

                    /*SqlCommand com_id = cn.CreateCommand();//запоминаем id выбранных продуктов
                    com_id.CommandText = $"SELECT ID FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id} AND NAME = '{ProductItems.Text}';";
                    SqlDataReader reader = com_id.ExecuteReader();
                    while (reader.HasRows)
                    {
                        products_id.Add(int.Parse(reader["ID"].ToString()));
                    }*/
                    SqlDataAdapter adapter_id = new SqlDataAdapter($"SELECT ID FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id} AND NAME = '{ProductItems.Text}';", ConnectionString);
                    DataTable table = new DataTable();
                    adapter_id.Fill(table);
                    foreach (DataRow row in table.Rows)
                    {
                        products_id.Add(int.Parse(row.ToString()));
                    }

                    SqlCommand com = cn.CreateCommand();//обновляем количество продуктов
                    com.CommandText = $"UPDATE PRODUCTS SET AMOUNT = AMOUNT - {amount} WHERE NAME = '{ProductItems.Text}' AND KITCHEN_ID = {StartUserWindow.user_id};";
                    com.ExecuteNonQuery();

                    SqlDataAdapter adapter = new SqlDataAdapter($"SELECT NAME, AMOUNT FROM SELECTED_PRODUCTS", ConnectionString);//заполнение dataset выбранных продуктов
                    adapter.Fill(prodTable);
                    SelectedProducts.DataContext = prodTable.DefaultView;

                    /*SqlCommand comm = cn.CreateCommand();
                    comm.CommandText = $"SELECT AMOUNT FROM PRODUCTS WHERE NAME = '{ProductItems.Text}' AND KITCHEN_ID = {StartUserWindow.user_id};";
                    SqlDataReader reader;
                    reader = comm.ExecuteReader();
                    while (reader.HasRows)
                    {
                        try
                        {
                            object amountisnull = reader["AMOUNT"];
                            if (amountisnull.Equals(0))
                            {
                                SqlCommand del = cn.CreateCommand();
                                del.CommandText = $"DELETE * FROM PRODUCTS WHERE NAME = '{ProductItems.Text}' AND KITCHEN_ID = {StartUserWindow.user_id};";
                                del.ExecuteNonQuery();
                            }
                        }
                        catch { }
                    }*/
                    
                    
                    cn.Close();
                }
                FillDataset();
            }

            
        }

        


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int amount;
            if (int.TryParse(AmountBox.Text, out amount))
            { }
            else
                MessageBox.Show("You have entered a wrong data");

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();

                SqlCommand command_1 = cn.CreateCommand();
                command_1.CommandText = $"INSERT INTO RECIPES(KITCHEN_ID, NAME, AMOUNT, DESCRIPTION) VALUES({StartUserWindow.user_id},'{RecipeNameBox.Text}', {amount}, '{DescriptionBox.Text}');";
                command_1.ExecuteNonQuery();

                
                command_1.CommandText = $"SELECT ID FROM RECIPES WHERE NAME = '{RecipeNameBox.Text}' AND KITCHEN_ID = {StartUserWindow.user_id};";
                command_1.ExecuteNonQuery();
                int id;
                SqlDataReader reader = command_1.ExecuteReader();
                while(reader.HasRows)
                {
                    id = Convert.ToInt32(reader.GetValue(0));
                    for(int i=0;i>products_id.Count();i++)
                    {
                        command_1.CommandText = $"INSERT INTO PRODUCTS_RECIPES(PRODUCT_ID, RECIPE_ID) VALUES({id}, {products_id.ElementAt(i)});";
                        command_1.ExecuteNonQuery();
                    }
                    
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
    }
}
