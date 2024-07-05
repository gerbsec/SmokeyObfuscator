using dnlib.DotNet.Emit;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeyObfuscator.Protections
{
    internal class HideMethods
    {
        public static void Execute(ModuleDef module)
        {
            TypeRef attrRef = module.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "CompilerGeneratedAttribute");
            var ctorRef = new MemberRefUser(module, ".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void), attrRef);
            var attr = new CustomAttribute(ctorRef);

            TypeRef attrRef2 = module.CorLibTypes.GetTypeRef("System", "EntryPointNotFoundException");
            var ctorRef2 = new MemberRefUser(module, ".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.String), attrRef2);

            foreach (var type in module.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    if (method.IsRuntimeSpecialName || method.IsSpecialName || method.Name == "Invoke") continue;
                    method.CustomAttributes.Add(attr);
                    method.Name = "<gerbserv.com>" + method.Name;
                }
            }

            var methImplFlags = MethodImplAttributes.IL | MethodImplAttributes.Managed;
            var methFlags = MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot;
            var meth1 = new MethodDefUser("Main", MethodSig.CreateStatic(module.CorLibTypes.Void, module.CorLibTypes.String), methImplFlags, methFlags);
            module.EntryPoint.DeclaringType.Methods.Add(meth1);
            var body = new CilBody();
            meth1.Body = body;
            meth1.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, "gerbserv.com"));
            meth1.Body.Instructions.Add(Instruction.Create(OpCodes.Newobj, ctorRef2));
            meth1.Body.Instructions.Add(Instruction.Create(OpCodes.Throw));
        }
    }
}
