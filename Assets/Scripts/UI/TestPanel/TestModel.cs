using UnityTools.UI;
public class TestModel : BaseModel
{
    private static TestModel _instance;
    public static TestModel instance
    {
        get
        {
            if (_instance == null) _instance = new();
            return _instance;
        }
    }
    private TestModel()
    {
        CreateModel(this);
    }
    protected override void Disable()
    {
        _instance = null;
    }
}