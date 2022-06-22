using EnumsNET;
namespace FlexAuth;




public static class PolicyNameHelper<T> where T : struct, Enum
{

    public const string DefaultPrefix = "Permissions";

    public static bool IsValidPolicyName(string? policyName, string prefix = "")
    {
        if (string.IsNullOrEmpty(prefix)) { prefix = DefaultPrefix; }
        return policyName != null && policyName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
    }

    public static string GeneratePolicyNameFor(T permissions, string prefix = "")
    {
        if (string.IsNullOrEmpty(prefix)) { prefix = DefaultPrefix; }
        var permissionsInt = EnumsNET.Enums.ToInt32(permissions);
        permissions.AsString();
        return $"{prefix}{permissionsInt}";
    }

    //public static string GeneratePolicyNameFor(EnumMember<T> member )
    //{


    //    return $"{Prefix}{member.ToInt32()}";
    //}

    public static T GetPermissionsFrom(string policyName, string prefix = "")
    {
        if (string.IsNullOrEmpty(prefix)) { prefix = DefaultPrefix; }
        var permissionsValue = int.Parse(policyName[prefix.Length..]!);
        T val = ((T[])Enum.GetValues(typeof(T)))[0];

        foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
        {
            if (Convert.ToInt32(enumValue).Equals(permissionsValue))
            {
                val = enumValue;
                break;
            }
        }
        return val;
    }
}


    ////https://stackoverflow.com/questions/23563960/how-to-get-enum-value-by-string-or-int
    //public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
    //{
    //    if (!typeof(T).IsEnum)
    //    {
    //        throw new Exception("T must be an Enumeration type.");
    //    }
    //    T val = ((T[])Enum.GetValues(typeof(T)))[0];

    //    foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
    //    {
    //        if (Convert.ToInt32(enumValue).Equals(intValue))
    //        {
    //            val = enumValue;
    //            break;
    //        }
    //    }
    //    return val;
    //}
//}


