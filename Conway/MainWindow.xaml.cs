using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Conway
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int GRID_HEIGHT = 40;
        const int GRID_WIDTH = 40;
        //Stopwatch stopwatch; //Debug variable
        //int counter = 0; //Debug variable

        Cell[,] cells = new Cell[GRID_HEIGHT, GRID_WIDTH];
        double delay = 0.5f;
        public MainWindow()
        {
            //stopwatch = new Stopwatch(); //Debug variable
            InitializeComponent();
            cellGrid.Width = GRID_WIDTH * 20;
            cellGrid.Height = GRID_HEIGHT * 20;
            
            cellGrid.ShowGridLines = false;
            //this.SizeToContent = SizeToContent.WidthAndHeight;
            
            //Lambda assign Event to clicking the reset BUtton
            resetButton.Click += (o, e) =>
            {
                foreach (Cell cell in cells)
                {
                    cell.kill();
                }
            };

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(delay);

            //stopwatch.Start(); //Debug / performance check stopwatch

            timer.Tick += updateCells;
            timer.Start();

            generateCells();
            collectNeighbors();
            insertCellsIntoGrid();

            this.Content = MainGrid;
            this.Show();
        }

        private void insertCellsIntoGrid()
        {
            foreach (Cell cell in cells)
            {
                cellGrid.Children.Add(cell.rect);
            }
        }


        void generateCells() // run on INIT. Generate a new cell based on the Grid sizes and insert them into the setup cells array
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                cellGrid.ColumnDefinitions.Add(new ColumnDefinition());
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    if (y == 0)
                        cellGrid.RowDefinitions.Add(new RowDefinition());
                    cells[y, x] = new Cell(y,x);
                }
            }
        }

        void collectNeighbors() // run on INIT. make every cell find their own neighbors and save a reference to them.
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    addNeighbors(x,y);
                }
            }

        }

void addNeighbors(int y, int x) // adds all neighbors to each cell. Grid wraps around.
        {
            int yminusone = y-1;
            int yplusone = y+1;
            int xminusone = x-1;
            int xplusone = x+1;

            if (y == 0) {yminusone = GRID_HEIGHT-1;} //-1 because of 0 index
            if (x == 0) {xminusone = GRID_WIDTH-1;}
            if (y == GRID_HEIGHT-1) {yplusone = 0;}
            if (x == GRID_WIDTH-1) {xplusone = 0;}

            //  012
            //  3X4
            //  567
            cells[y, x].addNeighbor(cells[yminusone, xminusone]);//0
            cells[y, x].addNeighbor(cells[yminusone, x]);//1
            cells[y, x].addNeighbor(cells[yminusone, xplusone]);//2

            cells[y, x].addNeighbor(cells[y, xminusone]);//3
            cells[y, x].addNeighbor(cells[y, xplusone]);//4

            cells[y, x].addNeighbor(cells[yplusone, xminusone]);//5
            cells[y, x].addNeighbor(cells[yplusone, x]);//6
            cells[y, x].addNeighbor(cells[yplusone, xplusone]);//7
            
        }

        void updateCells( object sender, EventArgs e)
        {
            if (playingCheckBox.IsChecked == true)
            {
                foreach (Cell cell in cells)
                {
                    cell.calcWillLive(); // first we have to calculate who will survive the next update
                }
                foreach (Cell cell in cells)
                {
                    cell.update(); // then we update everyone to their new state
                }
            }
            /* Debugging / performance checking. compared to imperative programming style.
            counter++;
            if (counter == 10000)
            {
                stopwatch.Stop();
                var test = 0;
            }
            */
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            delay = e.NewValue;
        }
    }
}
