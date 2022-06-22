

namespace FlexAuth.Interfaces;

public interface IGenericPermissionRoleProvider<T> where T :System.Enum
{
    List<IGenericPermissionRole<T>> GetRoles();
}
public interface IGenericPermissionRole<T> where T : System.Enum
{
    public string Name { get; set; }
    public T Permissions { get; set; }
}

public interface IRequestRoleProvider
{
    public List<string> GetRoles();
}
