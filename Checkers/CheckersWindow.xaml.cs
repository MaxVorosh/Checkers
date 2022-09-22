using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Checkers;

public partial class CheckersWindow : Window
{
    private Mode _gameMode;
    private Difficult _gameDifficult;
    private Gameplay _gameplay;
    //private Image[,] sprites;

    private void BoardClick(object sender, MouseButtonEventArgs e)
    {
        Point p = e.GetPosition(this);
        p = GameWindow.TranslatePoint(p, BoardBorder);
        MessageBox.Show("Координата x=" +p.X.ToString()+ " y="+p.Y.ToString());
    }

    /*private void SetGrid()
    {
        for (int i = 0; i < 8; ++i)
        {
            var columnDefinition = new ColumnDefinition();
            var rowDefinition = new RowDefinition();
            columnDefinition.Width = new GridLength(50);
            rowDefinition.Height = new GridLength(50);
            Board.ColumnDefinitions.Add(columnDefinition);
            Board.RowDefinitions.Add(rowDefinition);
        }

        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                Border border = new Border();
                border.Child = Board;
                border.Background = Brushes.GreenYellow;
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(2);
                border.CornerRadius = new CornerRadius(45);
                border.Padding = new Thickness(25);
            }
        }
    }*/
    
    public CheckersWindow(Mode mode, Difficult difficult, Gameplay gameplay)
    {
        _gameMode = mode;
        _gameDifficult = difficult;
        _gameplay = gameplay;
        //sprites = new Image[8, 8];
        InitializeComponent();
        //SetGrid();
    }
}