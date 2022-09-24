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
    // Window, that shows game
    private Mode _gameMode;
    private Difficult _gameDifficult;
    private Gameplay _gameplay;
    private CheckerSprite?[,] _sprites; // Checkers images, that consists of ellipses
    private CheckersBoard _board; // Board, that responsible for business-logic
    private bool _gameEnd;

    private void BoardClick(object sender, MouseButtonEventArgs e)
    {
        // Event of clicking on the board

        if (_gameEnd)
        {
            ChangeButtonsAfterGame();
            return;
        }

        // Calculating cursor coordinates
        var p = e.GetPosition(this);
        p = GameWindow.TranslatePoint(p, BoardBorder);
        var x = Convert.ToInt32(p.X - 2);
        var y = Convert.ToInt32(p.Y - 2);
        if (x < 0 || y < 0 || x > 400 || y > 400)
            return;

        // Give coords to the board
        _board.NewCoords(y, x);
        
        // Update interface
        SetIndicatorText();
        UpdateSprites();
        UpdateButtons();
    }

    private void AddSprite(int x, int y, bool isWhite, bool isMissis)
    {
        // Add a checker's image on the board
        
        var checker = new CheckerSprite(isWhite, isMissis);
        _sprites[x, y] = checker;
        
        Board.Children.Add(checker.MainShape);
        Grid.SetRow(checker.MainShape, x);
        Grid.SetColumn(checker.MainShape, y);
        
        Board.Children.Add(checker.MissisShape);
        Grid.SetRow(checker.MissisShape, x);
        Grid.SetColumn(checker.MissisShape, y);
    }

    private void DeleteAllCheckers()
    {
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if (_sprites[i, j] != null)
                {
                    DeleteSprite(i, j);
                }
            }
        }
    }

    private void DeleteSprite(int x, int y)
    {
        // Delete checker's image from the board
        Board.Children.Remove(_sprites[x, y].MainShape);
        Board.Children.Remove(_sprites[x, y].MissisShape);
    }

    private void UpdateSprites()
    {
        // Redrawing all checkers on the board in relation to position on the board
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                // Delete not empty image
                if (_sprites[i, j] != null)
                {
                    DeleteSprite(i, j);
                }

                // Add new image, if it's necessary
                var checker = _board.GetChecker(new Tuple<int, int>(i, j));
                if (checker.IsExists())
                {
                    AddSprite(i, j, checker.IsWhite(), checker.IsMissis());
                }
            }
        }
    }

    private void SetIndicatorText()
    {
        // Set text on the TextBlock in relation to game status
        Indicator.Text = (_board.IsWhiteTurn) ? "White to move" : "Black to move";
        if (_board.GetResult != Result.NotEnd)
        {
            _gameEnd = true;
            Indicator.Text = _board.GetResult switch
            {
                Result.Draw => "Draw",
                Result.BlackWin => "Black wins",
                _ => "White wins"
            };
        }
    }

    private void AddNotationTextBlock(int row, int column, string text, bool isLeft)
    {
        var textBlock = new TextBlock();
        textBlock.Text = text;
        Board.Children.Add(textBlock);
        Grid.SetColumn(textBlock, column);
        Grid.SetRow(textBlock, row);
        if (isLeft)
        {
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.VerticalAlignment = VerticalAlignment.Top;
        }
        else
        {
            textBlock.HorizontalAlignment = HorizontalAlignment.Right;
            textBlock.VerticalAlignment = VerticalAlignment.Bottom;
        }
        textBlock.FontWeight = FontWeights.Bold;
        textBlock.FontSize = 10;
    }

    private void DrawNotation()
    {
        for (int i = 0; i < 8; ++i)
        {
            Char letter = Convert.ToChar('a' + 7 - i);
            AddNotationTextBlock(7, i, i.ToString(), false);
            AddNotationTextBlock(i, 0, letter.ToString(), true);
        }
    }

    public CheckersWindow(Mode mode, Difficult difficult, Gameplay gameplay)
    {
        _gameMode = mode;
        _gameDifficult = difficult;
        _gameplay = gameplay;
        _sprites = new CheckerSprite[8, 8];
        _board = new CheckersBoard(8, 12);
        _gameEnd = false;
        InitializeComponent();
        UpdateSprites();
        SetIndicatorText();
        DrawNotation();
        UpdateButtons();
    }

    private void Resign(object sender, RoutedEventArgs e)
    {
        // Function ends a game, when someone click on "Resign" button
        var btn = (Button)e.Source;
        if (btn.Name == WhiteResign.Name)
        {
            _board.SetResult(Result.BlackWin);
        }
        else
        {
            _board.SetResult(Result.WhiteWin);
        }
        SetIndicatorText();
        ChangeButtonsAfterGame();
    }

    private void Draw(object sender, RoutedEventArgs e)
    {
        // When both CheckBoxes are clicked, than ir's draw
        if (WhiteDraw.IsChecked == true && BlackDraw.IsChecked == true)
        {
            _board.SetResult(Result.Draw);
            SetIndicatorText();
            ChangeButtonsAfterGame();
        }
    }

    private void ChangeButtonsAfterGame()
    {
        WhiteDraw.Visibility = Visibility.Hidden;
        BlackDraw.Visibility = Visibility.Hidden;
        WhiteResign.Visibility = Visibility.Hidden;
        BlackResign.Visibility = Visibility.Hidden;
        WhiteDraw.IsChecked = false;
        BlackDraw.IsChecked = false;
        ShowAfterGameButtons();
    }

    private void UpdateButtons()
    {
        // Hide and show "Resign" and "Draw" buttons
        if (_board.IsWhiteTurn)
        {
            WhiteDraw.Visibility = Visibility.Visible;
            WhiteResign.Visibility = Visibility.Visible;
            BlackDraw.Visibility = Visibility.Hidden;
            BlackResign.Visibility = Visibility.Hidden;
        }
        else
        {
            WhiteDraw.Visibility = Visibility.Hidden;
            WhiteResign.Visibility = Visibility.Hidden;
            BlackDraw.Visibility = Visibility.Visible;
            BlackResign.Visibility = Visibility.Visible;
        }
    }

    private void ShowAfterGameButtons()
    {
        ToMenuButton.Visibility = Visibility.Visible;
        RematchButton.Visibility = Visibility.Visible;
    }

    private void HideAfterGameButtons()
    {
        ToMenuButton.Visibility = Visibility.Hidden;
        RematchButton.Visibility = Visibility.Hidden;
    }

    private void BackToMenu(object sender, RoutedEventArgs e)
    {
        var menu = new MainWindow();
        menu.Show();
        this.Close();
    }

    private void Rematch(object sender, RoutedEventArgs e)
    {
        // Creates new values (start values) and update form
        DeleteAllCheckers();
        _sprites = new CheckerSprite[8, 8];
        _board = new CheckersBoard(8, 12);
        _gameEnd = false;
        UpdateSprites();
        SetIndicatorText();
        UpdateButtons();
        HideAfterGameButtons();
    }
}