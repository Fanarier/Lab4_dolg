using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private HttpClient httpClient;
        private MainWindow mainWindow;
        private string? token;
        public Main(Response response, MainWindow window)
        {
            InitializeComponent();
            this.mainWindow = window;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + response.access_token);
            token = response.access_token;
            Task.Run(() => Load());
        }
        private async Task Load()
        {
            List<Investor>? list = await httpClient.GetFromJsonAsync<List<Investor>>("http://localhost:5224/api/investors");
            foreach (Investor i in list!)
            {
                i.Percent = await httpClient.GetFromJsonAsync<Models.Percent>("http://localhost:5224/api/percent/" + i.Percent);
            }
            Dispatcher.Invoke(() =>
            {
                ListInvestors.ItemsSource = null;
                ListInvestors.Items.Clear();
                ListInvestors.ItemsSource = list;
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.mainWindow.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PercentWindow percentWindow = new PercentWindow(token!);
            percentWindow.ShowDialog();
        }
        //добавление
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InvestorWindow investorWindow = new InvestorWindow(token!);
            if (investorWindow.ShowDialog() == true)
            {
                Investor investor = new Investor
                {
                    FullName = investorWindow.NameProperty,
                    DepositAmount = investorWindow.InvestmentAmountProperty,
                    TotalAmount = investorWindow.ProfitProperty,
                    DepositDate = investorWindow.RegistrationDateProperty,
                    InterestRate = await investorWindow.getIdPercent()
                };
                JsonContent content = JsonContent.Create(investor);
                using var response = await httpClient.PostAsync("http://localhost:5224/api/investor", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }
        //изменение
        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Investor? st = ListInvestors.SelectedItem as Investor;
            InvestorWindow investorWindow = new InvestorWindow(token!, st!);
            if (investorWindow.ShowDialog() == true)
            {
                st!.FullName = investorWindow.NameProperty;
                st!.TotalAmount = investorWindow.ProfitProperty;
                st!.DepositAmount = investorWindow.InvestmentAmountProperty;
                st!.DepositDate = investorWindow.RegistrationDateProperty;
                st!.InterestRate = await investorWindow.getIdPercent();
                JsonContent content = JsonContent.Create(st);
                using var response = await httpClient.PutAsync("http://localhost:5224/api/investor", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }
        //удаление
        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Investor? st = ListInvestors.SelectedItem as Investor;
            JsonContent content = JsonContent.Create(st);
            using var response = await httpClient.DeleteAsync("http://localhost:5224/api/investor/" + st!.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
    }
}

