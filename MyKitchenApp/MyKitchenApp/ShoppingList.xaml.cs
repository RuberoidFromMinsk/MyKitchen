using System.Data.SqlClient;
using System.Windows;
using System.Net;
using System.Net.Mail;
using System.Text;
using System;
using System.Collections.Generic;

namespace MyKitchenApp
{
    public partial class ShoppingList : Window
    {
        public string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";

        public ShoppingList()
        {
            InitializeComponent();
        }

        void ClearAllFields()
        {
            NameBox.Clear();
            DescriptionBox.Clear();
        }

        void Message(string message)
        {
            MailAddress fromMailAddress = new MailAddress("mykitchen_team@mail.ru", "MyKitchen Team");
            MailAddress toAddress = new MailAddress($"{Get_UserMail()}", $"{Get_UserName()}");

            using (MailMessage mailMessage = new MailMessage(fromMailAddress, toAddress))
            using (SmtpClient smtpClient = new SmtpClient())
            {
                mailMessage.Subject = "Your shopping list";
                mailMessage.Body = $"{message}";

                smtpClient.Host = "smtp.mail.ru";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromMailAddress.Address, "lol12345");

                smtpClient.Send(mailMessage);
            }
            MessageBox.Show("Your list sended");
        }

        #region get userdata
        string Get_UserMail()
        {
            string mail = null;
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT EMAIL FROM USERS WHERE LOGIN = '{MainWindow.log}' AND PASSWORD = '{MainWindow.pass}';";
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        mail = reader.GetValue(0).ToString();
                    }
                }
                cn.Close();
                return mail;
            }
        }

        public static string Get_UserName()
        {
            string ConnectionString = @"Data Source=DESKTOP-R0R983R;Initial Catalog=MyKitchen;Integrated Security=True";
            string name = null;
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT NAME FROM USERS WHERE LOGIN = '{MainWindow.log}' AND PASSWORD = '{MainWindow.pass}';";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        name = reader.GetValue(0).ToString();
                    }
                }
                cn.Close();
                return name;
            }
        }
        #endregion

        #region Clicks
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            StartUserWindow startUserWindow = new StartUserWindow();
            startUserWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            startUserWindow.Show();
            Close();
        }

        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            int amount;
            if (AmountBox.Text.Length == 0)
            {
                MessageBox.Show("Please, enter the amount.");
                return;
            }
            else
            {
                if (int.TryParse(AmountBox.Text, out amount))
                { }
                else
                {
                    MessageBox.Show("Incorrect data in amount box.");
                    return;
                }
            }

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"INSERT INTO WISH_LIST(KITCHEN_ID, NAME, DESCRIPTION, AMOUNT) VALUES({StartUserWindow.user_id}, '{NameBox.Text}', '{DescriptionBox.Text}', {amount});";
                command.ExecuteNonQuery();
                cn.Close();
            }
            ClearAllFields();
        }

        private void SendMail_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder myMessage = new StringBuilder();
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();

                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"DELETE FROM SHOPPING_LIST;";
                command.ExecuteNonQuery();

                command.CommandText = $"INSERT INTO SHOPPING_LIST(NAME, AMOUNT, DESCRIPTION) SELECT A.NAME, A.AMOUNT - B.AMOUNT, A.DESCRIPTION FROM WISH_LIST A JOIN PRODUCTS B ON A.NAME = B.NAME WHERE B.KITCHEN_ID = {StartUserWindow.user_id} AND B.AMOUNT < A.AMOUNT;";
                command.ExecuteNonQuery();

                command.CommandText = $"INSERT INTO SHOPPING_LIST(NAME, AMOUNT, DESCRIPTION) SELECT NAME, AMOUNT, DESCRIPTION FROM WISH_LIST A WHERE A.NAME NOT IN(SELECT NAME FROM PRODUCTS WHERE KITCHEN_ID = {StartUserWindow.user_id});";
                command.ExecuteNonQuery();

                command.CommandText = $"SELECT NAME, AMOUNT, DESCRIPTION DESCRIPTION FROM SHOPPING_LIST;";
                SqlDataReader reader = command.ExecuteReader();
                myMessage.Append("Here your items you should buy to be happy:\n\n\n");
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        myMessage.Append("● " + reader.GetValue(0) + "   " + reader.GetValue(1) + "  " + reader.GetValue(2) + "\n");
                    }
                }
                cn.Close();
            }
            myMessage.Append("\n\nThis message was sending from MyKitchen application at " + DateTime.Now.ToString());
            myMessage.Append("\nBest regards, MyKitchen team.");
            Message(myMessage.ToString());
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"DELETE FROM WISH_LIST WHERE NAME = '{NameBox.Text}' AND KITCHEN_ID = {StartUserWindow.user_id};";
                int number = command.ExecuteNonQuery();
                MessageBox.Show("Strings deleted: " + number.ToString());
                cn.Close();
            }
            ClearAllFields();
        }

        private void ShowList_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder myList = new StringBuilder();
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand command = cn.CreateCommand();
                command.CommandText = $"SELECT NAME, AMOUNT, DESCRIPTION FROM WISH_LIST WHERE KITCHEN_ID = {StartUserWindow.user_id};";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        myList.Append("● " + reader.GetValue(0) + "   " + reader.GetValue(1) + "\n");
                    }
                }
                cn.Close();
            }

            MessageBox.Show(myList.ToString());
        }
        #endregion

    }
}
