

namespace FlexAuth;

public static class GenericPermissionsProvider<T> where T : System.Enum
{

    public static List<T> GetAll()
    {
        return Enum.GetValues(typeof(T))
            .OfType<T>()
            .ToList();
    }
}

