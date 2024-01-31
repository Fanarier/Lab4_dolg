using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
using Lab4.Models;

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для GroupWindow.xaml
    /// </summary>
    public partial class PercentWindow : Window
    {
        private HttpClient client;
        private Percent? percent;
        public PercentWindow(string token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => Load());
        }
        private async Task Load()
        {
            List<Percent>? list = await client.GetFromJsonAsync<List<Percent>>("http://localhost:5224/api/percents");
            Dispatcher.Invoke(() =>
            {
                ListPercents.ItemsSource = null;
                ListPercents.Items.Clear();
                ListPercents.ItemsSource = list;
            });
        }
        private async Task Save()
        {
            Percent percent = new Percent
            {
                Id = Kod.Text,
                DepositName = NamePercent.Text,
                InterestRate = Price.Text
            };
            JsonContent content = JsonContent.Create(percent);
            using var response = await client.PostAsync("http://localhost:5224/api/percent", content);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Save();
        }

        private void ListPercents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            percent = ListPercents.SelectedItem as Percent;
            Kod.Text = percent?.Id;
            NamePercent.Text = percent?.DepositName;
            Price.Text = percent?.InterestRate;
        }

        private async Task Edit()
        {
            percent!.Id = Kod.Text;
            percent!.DepositName = NamePercent.Text;
            percent!.InterestRate = Price.Text;
            JsonContent content = JsonContent.Create(percent);
            using var response = await client.PutAsync("http://localhost:5224/api/percent", content);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async Task Delete()
        {
            using var response = await client.DeleteAsync("http://localhost:5224/api/percent/" + percent?.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Edit();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await Delete();
        }
    }
}

