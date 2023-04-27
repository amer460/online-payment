namespace XCoreAssignment.Helpers;

public class ViewResultDTO
{
    public ViewResultDTO(string view, object? model = null)
    {
        View = view;
        Model = model;
    }

    public string View { get;  }
    public object? Model { get;  }
}
