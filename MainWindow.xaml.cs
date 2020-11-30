using System.Windows;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System;
using GameOfLife.Models;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (DataRow d in SqlDataSourceEnumerator.Instance.GetDataSources().Rows)
            {
                Servers.Items.Add($"{d["ServerName"]}\\{d["InstanceName"]}");
            }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection sql = new SqlConnection($"Data Source={Servers.SelectedItem.ToString()};Initial Catalog={Database.Text};User ID={Username.Text};Password={Pass.Text};Persist Security Info = True"))
            {
                try
                {
                    sql.Open();
                    sql.Close();
                    LifeInfo.connStr = sql.ConnectionString;
                    UniverseWindow universeWindow = new UniverseWindow();
                    universeWindow.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
