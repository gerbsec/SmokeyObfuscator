using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeyObfuscator.Protections
{
    internal class Renamer
    {
        public static void Execute(ModuleDef module)
        {
            foreach (TypeDef type in module.Types)
            {
                type.Name = Random.GetRandomString();
                type.Namespace = Random.GetRandomString();
                if (type.IsGlobalModuleType || type.IsRuntimeSpecialName || type.IsSpecialName || type.IsWindowsRuntime || type.IsInterface)
                {
                    continue;
                }
                foreach (MethodDef method in type.Methods)
                {
                    if (method.IsConstructor || method.IsRuntimeSpecialName || method.IsRuntime || method.IsStaticConstructor || method.IsVirtual) continue;
                    method.Name = Random.GetRandomString(); ;
                    foreach (var field in type.Fields)
                    {
                        field.Name = Random.GetRandomString(); ;
                        foreach (EventDef eventdef in type.Events)
                        {
                            eventdef.Name = Random.GetRandomString(); ;
                            foreach (PropertyDef property in type.Properties)
                            {
                                if (property.IsRuntimeSpecialName) continue;
                                property.Name = Random.GetRandomString(); ;
                            }
                        }
                    }
                    foreach (ParamDef p in method.ParamDefs)
                    {
                        p.Name = Random.GetRandomString(); ;
                    }
                }
            }
        }
    }
}
