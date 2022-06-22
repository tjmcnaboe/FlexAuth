namespace FlexAuth.Interfaces;



public interface IPermissionRoleProvider<T> where T : System.Enum 
{
    public List<IGenericPermissionRole<T>> GetRoles();
}
public abstract class MyGenericStaticRoleProvider<T> where T : System.Enum
{
    public abstract List<IGenericPermissionRole<T>> GetRoles();
}


