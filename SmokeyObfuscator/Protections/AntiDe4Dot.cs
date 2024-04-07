using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeyObfuscator.Protections
{
    internal class AntiDe4Dot
    {
        public static void Execute(AssemblyDef mod)
        {
            foreach (var module in mod.Modules)
            {
                var interfaceM = new InterfaceImplUser(module.GlobalType);
                for (var i = 0; i < 1; i++)
                {
                    var typeDef1 = new TypeDefUser(string.Empty, $"Form{i}", module.CorLibTypes.GetTypeRef("System", "Attribute"));
                    var interface1 = new InterfaceImplUser(typeDef1);
                    module.Types.Add(typeDef1);
                    typeDef1.Interfaces.Add(interface1);
                    typeDef1.Interfaces.Add(interfaceM);
                }
            }
        }
    }
}
