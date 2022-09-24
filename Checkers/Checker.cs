namespace Checkers;

public class Checker
{
    // Class, that stores information about every checker
    
    private readonly bool _white;
    private bool _missis, _exists;

    public Checker(bool isWhite, bool isMissis, bool isExists = false)
    {
        _white = isWhite; // is checker white
        _missis = isMissis; // is checker missis
        _exists = isExists; // is checker exists
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

    public void BecomeMissis()
    {
        _missis = true;
    }
}