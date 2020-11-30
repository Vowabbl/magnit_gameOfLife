using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GameOfLife.Models;
using System.Linq;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for UniverseWindow.xaml
    /// </summary>
    public partial class UniverseWindow : Window
    {        
        //Размер вселенной - cellColumns - по вертикали, cellsRows - по горизонтали
        static int cellsColumns = 50;
        static int cellsRows = 50;

        ///<summary>
        ///Двоичный массив, в котором хранятся клетки для вселенной (поколение)
        ///</summary>
        public bool[,] cells = new bool[cellsColumns, cellsRows];        

        public UniverseWindow()
        {
            InitializeComponent();
            ResetUniverse();
        }

        ///<summary>
        ///Полный перезапуск вселенной (заполнение поколения новыми, случайными клетками)
        ///</summary>
        void ResetUniverse(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            for (int y = 0; y < cellsColumns; y++)
            {
                for (int x = 0; x < cellsRows; x++)
                {
                    cells[y, x] = rng.Next(0, 2) > 0;
                }
            }
            RenderCells();
        }

        ///<summary>
        ///Полный перезапуск вселенной (заполнение поколения новыми, случайными клетками)
        ///</summary>
        void ResetUniverse()
        {
            ResetUniverse(null, null);
        }

        ///<summary>
        ///Создаёт новое поколение для клеток
        ///</summary>
        void AdvanceEvolution(object sender, RoutedEventArgs e)
        {
            for (int y = 0; y < cellsColumns; y++)
            {
                for (int x = 0; x < cellsRows; x++)
                {
                    var neighbourCells = 0;
                    for (var ny = -1; ny <= 1; ny++)
                    {
                        if (ny == -1 && y == 0 || y == cellsColumns - 1 && ny == 1)
                            continue;
                        for (var nx = -1; nx <= 1; nx++)
                        {
                            if (nx == -1 && x == 0 || nx == 1 && x == cellsRows - 1 || nx == 0)
                                continue;
                            neighbourCells += cells[y + ny, x + nx] == true ? 1 : 0;
                        }
                    }
                    
                    if (neighbourCells == 2 || neighbourCells == 3)
                        cells[y, x] = true;
                    else
                        cells[y, x] = false;
                }
            }
            RenderCells();
        }

        ///<summary>
        ///Вывод поколения на вселенную
        ///</summary> 
        private void RenderCells()
        {
            Universe.Children.Clear();
            for (int y = 0; y < cellsColumns; y++)
            {
                for (int x = 0; x < cellsRows; x++)
                {
                    Rectangle rectangle = new Rectangle { Height = 10, Width = 10 };
                    rectangle.Fill = cells[y, x] ? Brushes.Black : Brushes.White;
                    Universe.Children.Add(rectangle);
                    Canvas.SetLeft(rectangle, x * (15));
                    Canvas.SetTop(rectangle, y * (15));
                }
            }
        }

        ///<summary>
        ///Сохранение текущего поколения на базу данных
        ///</summary>
        private void SaveToDB(object sender, RoutedEventArgs e)
        {
            using (LifeInfo db = new LifeInfo())
            {
                string universeJson = JsonConvert.SerializeObject(cells, Formatting.Indented);
                
                if (!db.GOLDatas.Any(x => x.state == universeJson))
                {
                    GOLData data = new GOLData();
                    data.state = universeJson; data.stateDate = DateTime.Now;
                    db.GOLDatas.Add(data);
                    db.SaveChanges();
                }
            }
        }

        ///<summary>
        ///Загрузка поколения из базы данных
        ///</summary>
        private void LoadFromDB(object sender, RoutedEventArgs e)
        {
            UniverseSelectWindow universeSelect = new UniverseSelectWindow();

            if(universeSelect.ShowDialog() != true && universeSelect.jsonArray != "")
            {
                cells = JsonConvert.DeserializeObject<bool[,]>(universeSelect.jsonArray);
            }
            RenderCells();
        }       
    }
}
