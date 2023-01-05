using System.Reflection;

namespace Presentation
{
    public class AssemblyReference
    {
        public static String GetAssemblyNameContainingType(String typeName)
        {
            foreach (Assembly currentassembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t = currentassembly.GetType(typeName, false, true);
                if (t != null) { return currentassembly.FullName; }
            }

            return "not found";
        }

        string GetAssemblyLocationOfObject(object o)
        {
            return Assembly.GetAssembly(o.GetType()).Location;
        }
    }
}
