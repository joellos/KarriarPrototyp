using System.Reflection;
using System.Runtime.Loader;

namespace CC_Karriarpartner.Services.CustomAssembly
{
    // Laddar upp native-DLL som ligger under DikToPdfNative mappen
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllPath)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null!;
        }
    }

}
