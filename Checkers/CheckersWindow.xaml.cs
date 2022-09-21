using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Checkers;

public partial class CheckersWindow : Window
{
    private Mode GameMode { get; init; }
    private Difficult GameDifficult { get; init; }
    
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
    
    public CheckersWindow(Mode mode, Difficult difficult)
    {
        GameMode = mode;
        GameDifficult = difficult;
        InitializeComponent();
        //SetGrid();
    }
}