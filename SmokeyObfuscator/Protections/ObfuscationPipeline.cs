using System;

namespace SmokeyObfuscator.Protections
{
    internal static class ObfuscationPipeline
    {
        public static void Execute(dnlib.DotNet.ModuleDefMD module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            NumberChanger.Process(module);
            Strings.Execute(module);
            ProxyInts.Execute(module);
            HideMethods.Execute(module);
        }
    }
}
