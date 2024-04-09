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
                type.Name = Random2.GetRandomString();
                type.Namespace = Random2.GetRandomString();
                if (type.IsGlobalModuleType || type.IsRuntimeSpecialName || type.IsSpecialName || type.IsWindowsRuntime || type.IsInterface)
                {
                    continue;
                }
                foreach (MethodDef method in type.Methods)
                {
                    if (method.IsConstructor || method.IsRuntimeSpecialName || method.IsRuntime || method.IsStaticConstructor || method.IsVirtual) continue;
                    method.Name = Random2.GetRandomString(); ;
                    foreach (var field in type.Fields)
                    {
                        field.Name = Random2.GetRandomString(); ;
                        foreach (EventDef eventdef in type.Events)
                        {
                            eventdef.Name = Random2.GetRandomString(); ;
                            foreach (PropertyDef property in type.Properties)
                            {
                                if (property.IsRuntimeSpecialName) continue;
                                property.Name = Random2.GetRandomString(); ;
                            }
                        }
                    }
                    foreach (ParamDef p in method.ParamDefs)
                    {
                        p.Name = Random2.GetRandomString(); ;
                    }
                }
            }
        }
    }
}
