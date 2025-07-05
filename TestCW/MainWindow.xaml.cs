using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;

namespace TestCW
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void AddUser_Click(object sender, RoutedEventArgs e)
        {
            // Skip if placeholders are still in place
            if (txtName.Text == "Name" || txtEmail.Text == "Email")
            {
                MessageBox.Show("Please enter valid name and email.");
                return;
            }

            var user = new
            {
                name = txtName.Text,
                email = txtEmail.Text
            };

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://localhost:8080/api/users", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("User added successfully!");
                    txtName.Text = "Name";
                    txtName.Foreground = Brushes.Gray;
                    txtEmail.Text = "Email";
                    txtEmail.Foreground = Brushes.Gray;
                }
                else
                {
                    MessageBox.Show("Failed to add user.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text == "Name" || tb.Text == "Email")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                if (tb.Name == "txtName")
                {
                    tb.Text = "Name";
                }
                else if (tb.Name == "txtEmail")
                {
                    tb.Text = "Email";
                }
                tb.Foreground = Brushes.Gray;
            }
        }
    }
}
