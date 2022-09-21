namespace Checkers;

public class Checker
{
    private bool white, missis, exists;

    public Checker(bool isWhite, bool isMissis, bool isExists=false)
    {
        white = isWhite;
        missis = isMissis;
        exists = isExists;
    }

    public bool IsWhite()
    {
        return white;
    }

    public bool IsMissis()
    {
        return missis;
    }

    public bool IsExists()
    {
        return exists;
    }

    public void Delete()
    {
        exists = false;
    }

    public Checker Copy()
    {
        return new Checker(white, missis, exists);
    }

    public void BecomeMisis()
    {
        missis = true;
    }
}