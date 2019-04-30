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
        List<string> names = new List<string>();

        string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";

        public AddRecipeWindow()
        {
            InitializeComponent();
            Clear_SELECTED_PRODUCTS();
            Clear_AVALIBLE_PRODUCTS();
            Insert_AVALIBLE_PRODUCTS();
            FillDataset();
            FillCombobox();
        }

        #region Fill dataset and combobox
        void FillCombobox()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT NAME FROM AVALIBLE_PRODUCTS WHERE AMOUNT > 0;";
                SqlDataReader reader;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        object name = reader.GetValue(0);
                        ProductItems.Items.Add(name.ToString());
                    }
                    catch { }
                }
                cn.Close();
            }
        }

        void FillDataset()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();

                DataTable prodTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT NAME, AMOUNT FROM AVALIBLE_PRODUCTS WHERE AMOUNT > 0", ConnectionString);
                adapter.Fill(prodTable);
                AvalibleProducts.DataContext = prodTable.DefaultView;

                cn.Close();
            }
        }
        #endregion

        #region clear-insert operations
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

        void Clear_AVALIBLE_PRODUCTS()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"DELETE FROM AVALIBLE_PRODUCTS";
                command.ExecuteNonQuery();
                cn.Close();
            }
        }

        void Insert_AVALIBLE_PRODUCTS()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"INSERT INTO AVALIBLE_PRODUCTS(ID, NAME, AMOUNT) SELECT ID, NAME, AMOUNT FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id} AND AMOUNT != 0;";
                command.ExecuteNonQuery();
                cn.Close();
            }
        }
        #endregion

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
                command.CommandText = $"SELECT AMOUNT FROM AVALIBLE_PRODUCTS WHERE NAME = '{ProductItems.Text}';";
                amountForCheck = (int)command.ExecuteScalar();
                cn.Close();
            }

            if (amountForCheck < amount)
            {
                MessageBox.Show("Entered amount more than avalible amount");
                return;
            }
                
            else
            {
                using (SqlConnection cn = new SqlConnection(ConnectionString))
                {
                    cn.Open();

                    SqlCommand command = cn.CreateCommand();
                    command.CommandText = $"SELECT COUNT(*) FROM SELECTED_PRODUCTS WHERE NAME = '{ProductItems.Text}';";
                    int count = (int)command.ExecuteScalar();
                    if(count == 0)
                    {
                        //заполнение таблицы SELECTED_PRODUCTS
                        command.CommandText = $"INSERT INTO SELECTED_PRODUCTS(NAME, AMOUNT) VALUES('{ProductItems.Text}', {amount});";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        //update таблицы SELECTED_PRODUCTS
                        command.CommandText = $"UPDATE SELECTED_PRODUCTS SET AMOUNT = AMOUNT + {amount} WHERE NAME = '{ProductItems.Text}';";
                        command.ExecuteNonQuery();
                    }

                    

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

                    //обновляем количество продуктов в AVALIBLE_PRODUCTS
                    SqlCommand command_1 = cn.CreateCommand();
                    command_1.CommandText = $"UPDATE AVALIBLE_PRODUCTS SET AMOUNT = AMOUNT - {amount} WHERE NAME = '{ProductItems.Text}';";
                    command_1.ExecuteNonQuery();

                    cn.Close();
                }
                FillDataset();
                ProductItems.Items.Clear();
                FillCombobox();
            }
        }

        


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(RecipeNameBox.Text.Length == 0 || 
               PortionsBox.Text.Length == 0 || 
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

                //запоминаем имена selected products
                DataTable _table = new DataTable();
                SqlDataAdapter adapter_names = new SqlDataAdapter($"SELECT NAME FROM SELECTED_PRODUCTS", ConnectionString);
                adapter_names.Fill(_table);
                foreach (DataRow row in _table.Rows)
                    names.Add(row.ItemArray.First().ToString());


                cn.Open();
                SqlCommand command = cn.CreateCommand();
                #region проверка на имя рецепта
                command.CommandText = $"SELECT COUNT(*) FROM RECIPES WHERE NAME = '{RecipeNameBox.Text}';";
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Recipe with the same name exist. Change it.");
                    return;
                }
                #endregion

                for (int i = 0; i < names.Count(); i++)
                {
                    command.CommandText = $"UPDATE PRODUCTS SET AMOUNT = AMOUNT - {amounts.ElementAt(i)} WHERE NAME = '{names.ElementAt(i)}';";
                    command.ExecuteNonQuery();
                }

                command.CommandText = $"INSERT INTO RECIPES(KITCHEN_ID, NAME, PORTIONS, DESCRIPTION) VALUES({StartUserWindow.user_id},'{RecipeNameBox.Text}', {portions}, '{DescriptionBox.Text}');";
                command.ExecuteNonQuery();

                command.CommandText = $"SELECT ID FROM RECIPES WHERE NAME = '{RecipeNameBox.Text}' AND KITCHEN_ID = {StartUserWindow.user_id};";
                SqlDataReader reader = command.ExecuteReader();
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
            Clear_AVALIBLE_PRODUCTS();
            Insert_AVALIBLE_PRODUCTS();
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
