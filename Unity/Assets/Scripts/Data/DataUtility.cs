using System;
using System.Reflection;

internal class DataUtility
{
    public static bool EnsureCallerIsAllowed()
    {
        var callerAssembly = Assembly.GetCallingAssembly();
        var allowedAssemblyNames = new[] { "GameSystem","Data" };

        bool isAllowed = false;
        foreach (var allowedName in allowedAssemblyNames)
        {
            if (callerAssembly.FullName.StartsWith(allowedName, StringComparison.Ordinal))
            {
                isAllowed = true;
                break;
            }
        }

        if (!isAllowed)
        {
            throw new UnauthorizedAccessException($"Only assemblies under 'GameSystem' or 'Data' are allowed to modify this data.");
        }

        return isAllowed;
    }
}
