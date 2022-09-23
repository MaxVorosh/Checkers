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
    private CheckerSprite?[,] _sprites;
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
        Indicator.Text = (_board.IsWhiteTurn) ? "Ход белых" : "Ход чёрных";
    }

    public void AddSprite(int x, int y, bool isWhite, bool isMissis)
    {
        var checker = new CheckerSprite(isWhite, isMissis);
        _sprites[x, y] = checker;
        Board.Children.Add(checker.MainShape);
        Grid.SetRow(checker.MainShape, x);
        Grid.SetColumn(checker.MainShape, y);
        Board.Children.Add(checker.MissisShape);
        Grid.SetRow(checker.MissisShape, x);
        Grid.SetColumn(checker.MissisShape, y);
    }

    public void DeleteSprite(int x, int y)
    {
        Board.Children.Remove(_sprites[x, y].MainShape);
        Board.Children.Remove(_sprites[x, y].MissisShape);
    }

    public void UpdateSprites()
    {
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if (_sprites[i, j] != null)
                {
                    DeleteSprite(i, j);
                }

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
        _sprites = new CheckerSprite[8, 8];
        _board = new CheckersBoard(8, 12);
        InitializeComponent();
        UpdateSprites();
        Indicator.Text = (_board.IsWhiteTurn) ? "Ход белых" : "Ход чёрных";
        //SetGrid();
    }
}