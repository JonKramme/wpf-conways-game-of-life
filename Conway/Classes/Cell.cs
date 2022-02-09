using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;

public class Cell
{
    private bool isAlive;
    private bool willLive;
    private ArrayList neighbors = new ArrayList();
    private SolidColorBrush deadColor = new SolidColorBrush(Colors.AliceBlue);
    private SolidColorBrush liveColor = new SolidColorBrush(Colors.BurlyWood);

    public Rectangle rect = new Rectangle() //Rectangle as the visual representation of the Cell
    {
        Height = 20,
        Width = 20,
        Fill = new SolidColorBrush(Colors.AliceBlue)
    };

    public Cell(int y,int x)
    {
        Grid.SetRow(rect, y);
        Grid.SetColumn(rect, x);
        this.isAlive = false;
        this.willLive = false;

        
        rect.MouseEnter += (o, i) => // Lambda for MouseEnter Event. Checks if Mouse is pressed when entering the cell rect -> toggles state
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.isAlive = !this.isAlive;
                if (this.isAlive)
                {
                    this.rect.Fill = liveColor;
                }
                else
                {
                    this.rect.Fill = deadColor;
                }
            }
        };
        
    }

    public void addNeighbor(Cell cell) { neighbors.Add(cell); } 

    public void update() //update the cells state and color depending on the "willLive" boolean
    {
        isAlive = willLive;
        if (isAlive)
        {
            this.rect.Fill = liveColor;
        }
        else
        {
            this.rect.Fill = deadColor;
        }
    }

    public void kill() // set the state and color of the cell to "dead"
    {
        isAlive = false;
        this.rect.Fill = deadColor;
    }
    public void makeLive() // set the state and color of the cell to "alive"
    {
        isAlive = true;
        this.rect.Fill = liveColor;
    }
    public void calcWillLive() // calculate if the cell will live next update based on neighbors and Conway's Game of Life Rules.
    {
        int count = 0;
        foreach (Cell cell in neighbors){
            if (cell.isAlive)
                count++;
        }
        if (isAlive && (count >= 2 && count <= 3))
            willLive = true;
        else if (count == 3 && !isAlive)
            willLive = true;
        else
        {
            willLive = false;
        }
    }

}
