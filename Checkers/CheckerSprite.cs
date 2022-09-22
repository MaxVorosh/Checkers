using System.Windows.Media;
using System.Windows.Shapes;

namespace Checkers;

public class CheckerSprite
{
    public Ellipse MainShape;
    public Ellipse MissisShape;

    public CheckerSprite(bool isWhite, bool isMissis)
    {
        MainShape = DrawEllipse(isWhite, 40);
        if (isMissis)
        {
            MissisShape = DrawEllipse(!isWhite, 10);
        }
        else
        {
            MissisShape = DrawEllipse(isWhite, 10);
        }
    }

    public Ellipse DrawEllipse(bool isWhite, int size)
    {
        Ellipse el = new Ellipse();
        el.Width = size;
        el.Height = size;
        el.Fill = (isWhite) ? Brushes.White : Brushes.Black;
        return el;
    }
}