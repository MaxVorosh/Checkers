using System.Windows.Media;
using System.Windows.Shapes;

namespace Checkers;

public class CheckerSprite
{
    // Ellipsis, that shows checkers on the board
    public Ellipse MainShape; // Ellipse for all checkers
    public Ellipse MissisShape; // Ellipse, that shows, that this checker is missis

    public CheckerSprite(bool isWhite, bool isMissis)
    {
        MainShape = CreateEllipse(isWhite, 40);
        if (isMissis)
        {
            MissisShape = CreateEllipse(!isWhite, 10);
        }
        else
        {
            MissisShape = CreateEllipse(isWhite, 10);
        }
    }

    public Ellipse CreateEllipse(bool isWhite, int size)
    {
        Ellipse el = new Ellipse();
        el.Width = size;
        el.Height = size;
        el.Fill = (isWhite) ? Brushes.White : Brushes.Black;
        return el;
    }
}