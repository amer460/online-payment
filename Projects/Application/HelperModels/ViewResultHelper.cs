namespace Application.HelperModels;

public class ViewResultHelper
{
    public ViewResultHelper(string view, object model = null)
    {
        View = view;
        Model = model;
    }

    public string View { get; }
    public object Model { get; }
}
