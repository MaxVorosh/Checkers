using System.Windows;

namespace Checkers;

public partial class CheckersWindow : Window
{
    public Mode GameMode { get; init; }
    public Difficult GameDifficult { get; init; }
    public CheckersWindow(Mode mode, Difficult difficult)
    {
        GameMode = mode;
        GameDifficult = difficult;
        InitializeComponent();
    }
}