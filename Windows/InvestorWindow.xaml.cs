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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Lab4.Models;

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для StudentWindow.xaml
    /// </summary>
    public partial class InvestorWindow : Window
    {
        private HttpClient client;
        private Investor? investor;
        public InvestorWindow(String token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => LoadPercents());
        }
        public InvestorWindow(String token, Investor investor)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => LoadPercents());
            Name.Text = investor.FullName;
            InvestmentAmount.Text = investor.DepositAmount;
            Profit.Text = investor.TotalAmount;
            RegistrationDate.SelectedDate = investor.DepositDate;
            cbPercent.SelectedItem = investor.Percent!.DepositName;
        }
        private async void LoadPercents()
        {
            List<Percent>? list = await client.GetFromJsonAsync<List<Percent>>("http://localhost:5224/api/percents");
            Dispatcher.Invoke(() =>
            {
                cbPercent.ItemsSource = list?.Select(p => p.DepositName);
            });
        }
        public string? NameProperty
        {
            get { return Name.Text; }
        }
        public string? InvestmentAmountProperty
        {
            get { return InvestmentAmount.Text; }
        }
        public string? ProfitProperty
        {
            get { return Profit.Text; }
        }
        public DateTime? RegistrationDateProperty
        {
            get { return DateTime.Parse(RegistrationDate.Text); }
        }
        public async Task<string> getIdPercent()
        {
            Percent? percent = await client.GetFromJsonAsync<Percent>("http://localhost:5224/api/percent/" + cbPercent.Text);
            return percent!.Id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
