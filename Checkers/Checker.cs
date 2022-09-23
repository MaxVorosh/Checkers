namespace Checkers;

public class Checker
{
    private bool _white, _missis, _exists;

    public Checker(bool isWhite, bool isMissis, bool isExists = false)
    {
        _white = isWhite;
        _missis = isMissis;
        _exists = isExists;
    }

    public bool IsWhite()
    {
        return _white;
    }

    public bool IsMissis()
    {
        return _missis;
    }

    public bool IsExists()
    {
        return _exists;
    }

    public void Delete()
    {
        _exists = false;
    }

    public Checker Copy()
    {
        return new Checker(_white, _missis, _exists);
    }

    public void BecomeMisis()
    {
        _missis = true;
    }
}