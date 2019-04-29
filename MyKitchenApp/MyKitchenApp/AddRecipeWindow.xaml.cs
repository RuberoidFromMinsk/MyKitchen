using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;

namespace MyKitchenApp
{
    public partial class AddRecipeWindow : Window
    {
        List<int> products_id = new List<int>();
        List<int> amounts = new List<int>();

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
            {
                MessageBox.Show("You entered wrong data");
                return;
            }

            if(amount <= 0)
            {
                MessageBox.Show("You entered wrong data");
                return;
            }

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

                    SqlCommand command = cn.CreateCommand();//заполнение таблицы SELECTED_PRODUCTS
                    command.CommandText = $"INSERT INTO SELECTED_PRODUCTS(NAME, AMOUNT) VALUES('{ProductItems.Text}', {amount});";
                    command.ExecuteNonQuery();

                    SqlCommand com = cn.CreateCommand();//обновляем количество продуктов
                    com.CommandText = $"UPDATE PRODUCTS SET AMOUNT = AMOUNT - {amount} WHERE NAME = '{ProductItems.Text}' AND KITCHEN_ID = {StartUserWindow.user_id};";
                    com.ExecuteNonQuery();

                    SqlDataAdapter adapter = new SqlDataAdapter($"SELECT NAME, AMOUNT FROM SELECTED_PRODUCTS", ConnectionString);//заполнение dataset выбранных продуктов
                    adapter.Fill(prodTable);
                    SelectedProducts.DataContext = prodTable.DefaultView;

                    
                    

                    

                    //запомнить id выбранных item'ов
                    SqlDataAdapter adapter_id = new SqlDataAdapter($"SELECT ID FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id} AND NAME = '{ProductItems.Text}';", ConnectionString);
                    DataTable table = new DataTable();
                    adapter_id.Fill(table);
                    try
                    {
                        foreach (DataRow row in table.Rows)
                            products_id.Add(Convert.ToInt32(row.ItemArray.First()));
                    }
                    catch(Exception a)
                    {
                        MessageBox.Show(a.Message);
                    }

                    DataTable _table = new DataTable();//запоминаем amount выбранных продуктов
                    for (int i = 0; i < products_id.Count(); i++)
                    {
                        _table.Clear();
                        SqlDataAdapter adapter_amount = new SqlDataAdapter($"SELECT A.AMOUNT FROM SELECTED_PRODUCTS AS A JOIN PRODUCTS AS B ON A.NAME = B.NAME WHERE B.ID = {products_id.ElementAt(i)};", ConnectionString);
                        adapter_amount.Fill(_table);
                    }
                    foreach (DataRow row in _table.Rows)
                        amounts.Add(Convert.ToInt32(row.ItemArray.Reverse().Take(1).Single()));

                    /*StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < amounts.Count(); i++)
                    {
                            builder.Append(amounts.ElementAt(i).ToString() + ' ');
                    }
                    MessageBox.Show(builder.ToString());*/
                    /*SqlCommand insert_del = cn.CreateCommand(); //удаление 0
                    insert_del.CommandText = $"INSERT INTO DELETED_PRODUCTS(ID, NAME) VALUES({})";
                    insert_del.ExecuteNonQuery();

                    SqlCommand del = cn.CreateCommand();
                    del.CommandText = $"DELETE FROM PRODUCTS WHERE AMOUNT = 0;";
                    del.ExecuteNonQuery();*/



                    cn.Close();
                }
                FillDataset();
            }
        }

        


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(RecipeNameBox.Text.Length == 0 || 
               PortionsBox.Text.Length == 0 || 
               ProductItems.Text.Length == 0 || 
               AmountBox.Text.Length == 0 || 
               DescriptionBox.Text.Length == 0)
            {
                MessageBox.Show("Invalid data. Check it.");
                return;
            }

            int portions;
            if (int.TryParse(PortionsBox.Text, out portions))
            { }
            else
            {
                MessageBox.Show("Invalid data. Check it.");
                return;
            }

            if (portions <= 0)
            {
                MessageBox.Show("Invalid data. Check it.");
                return;
            }

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command_1 = cn.CreateCommand();

                command_1.CommandText = $"INSERT INTO RECIPES(KITCHEN_ID, NAME, PORTIONS, DESCRIPTION) VALUES({StartUserWindow.user_id},'{RecipeNameBox.Text}', {portions}, '{DescriptionBox.Text}');";
                command_1.ExecuteNonQuery();

                command_1.CommandText = $"SELECT ID FROM RECIPES WHERE NAME = '{RecipeNameBox.Text}' AND KITCHEN_ID = {StartUserWindow.user_id};";
                
                SqlDataReader reader = command_1.ExecuteReader();
                object id = null;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = reader.GetValue(0);
                    }
                }

                SqlCommand command_insert = cn.CreateCommand();
                for (int i = 0; i < products_id.Count(); i++)
                {
                    SqlConnection connection = new SqlConnection(ConnectionString);
                    connection.Open();
                    SqlCommand cmd = new SqlCommand($"INSERT INTO PRODUCTS_RECIPES(RECIPE_ID, PRODUCT_ID, AMOUNT) VALUES({Convert.ToInt32(id)}, {products_id.ElementAt(i)}, {amounts.ElementAt(i)});", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                cn.Close();
            }

            MessageBox.Show("Your recipe '" + $"{RecipeNameBox.Text}" + "' added");

            ClearAllFields();
            
        }

        void ClearAllFields()
        {
            RecipeNameBox.Clear();
            AmountBox.Clear();
            ProductItems.Text = "";
            PortionsBox.Clear();
            DescriptionBox.Clear();
            Clear_SELECTED_PRODUCTS();
            FillDataset();

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT NAME, AMOUNT FROM SELECTED_PRODUCTS", ConnectionString);
            DataTable prodTable = new DataTable();
            adapter.Fill(prodTable);
            SelectedProducts.DataContext = prodTable.DefaultView;
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
