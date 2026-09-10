using System;
using System.Text;
using dnlib.DotNet;

namespace SmokeyObfuscator.Protections
{
    internal static class HideMethods
    {
        public static void Execute(ModuleDef module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            foreach (TypeDef type in module.GetTypes())
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody)
                        continue;

                    if (method == module.EntryPoint)
                        continue;

                    if (method.IsConstructor)
                        continue;

                    if (method.IsRuntimeSpecialName || method.IsSpecialName)
                        continue;

                    method.Name = GenerateObfuscatedName();
                }
            }
        }

        private static string GenerateObfuscatedName()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            StringBuilder builder = new StringBuilder();
            Random random = new Random();

            builder.Append("_m");
            for (int i = 0; i < 12; i++)
            {
                builder.Append(chars[random.Next(chars.Length)]);
            }

            return builder.ToString();
        }
    }
}
