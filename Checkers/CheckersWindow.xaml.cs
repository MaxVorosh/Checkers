using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Checkers;

public partial class CheckersWindow : Window
{
    private Mode _gameMode;
    private Difficult _gameDifficult;
    private Gameplay _gameplay;
    private Ellipse[,] _sprites;
    private CheckersBoard _board;

    private void BoardClick(object sender, MouseButtonEventArgs e)
    {
        Point p = e.GetPosition(this);
        p = GameWindow.TranslatePoint(p, BoardBorder);
        int x = Convert.ToInt32(p.X - 2);
        int y = Convert.ToInt32(p.Y - 2);
        if (x < 0 || y < 0 || x > 400 || y > 400)
        {
            return;
        }
        _board.NewCoords(y, x);
        UpdateSprites();
    }

    public void AddSprite(int x, int y, bool isWhite, bool isMissis)
    {
        var checker = new Ellipse();
        checker.Width = 40;
        checker.Height = 40;
        checker.Fill = (isWhite) ? Brushes.White : Brushes.Black;
        _sprites[x, y] = checker;
        Board.Children.Add(checker);
        Grid.SetRow(checker, x);
        Grid.SetColumn(checker, y);
    }

    public void DeleteSprite(int x, int y)
    {
        Board.Children.Remove(_sprites[x, y]);
    }

    public void UpdateSprites()
    {
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                DeleteSprite(i, j);
                var checker = _board.GetChecker(new Tuple<int, int>(i, j));
                if (checker.IsExists())
                {
                    AddSprite(i, j, checker.IsWhite(), checker.IsMissis());
                }
            }
        }
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
        _sprites = new Ellipse[8, 8];
        _board = new CheckersBoard(8, 12);
        InitializeComponent();
        UpdateSprites();
        //SetGrid();
    }
}