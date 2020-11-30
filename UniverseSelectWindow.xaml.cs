using System.Linq;
using System.Windows;
using GameOfLife.Models;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for UniverseSelectWindow.xaml
    /// </summary>
    public partial class UniverseSelectWindow : Window
    {
        public string jsonArray;
        public UniverseSelectWindow()
        {
            InitializeComponent();
            using (LifeInfo db = new LifeInfo())
            {
                UniverseList.ItemsSource = db.GOLDatas.ToList();
            }
        }

        //Добавление
        private void UniverseList_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (UniverseList.SelectedItem != null)
            {
                jsonArray = ((GOLData)UniverseList.SelectedItem).state;
                this.Close();
            }
        }

        //Удаление
        private void UniverseList_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (UniverseList.SelectedItem != null)
            {
                using (LifeInfo db = new LifeInfo())
                {
                    if (MessageBox.Show("Вы точно хотите отправить выбранную вселенную в бездну?", "Удаление вселенной", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        db.GOLDatas.Attach((GOLData)UniverseList.SelectedItem);
                        db.GOLDatas.Remove((GOLData)UniverseList.SelectedItem);
                        UniverseList.ItemsSource = db.GOLDatas.ToList();
                    }
                }
            }
        }
    }
}
