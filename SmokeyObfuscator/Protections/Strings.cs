using dnlib.DotNet.Emit;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeyObfuscator.Protections
{
    internal class Strings
    {
        private static Random random = new Random();
        public static void Execute(ModuleDefMD module)
        {
            
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string randName = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            MethodDefUser TTH = new MethodDefUser(randName, MethodSig.CreateStatic(module.CorLibTypes.String, module.CorLibTypes.String), MethodImplAttributes.IL | MethodImplAttributes.Managed, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot); ;
            module.GlobalType.Methods.Add(TTH);
            CilBody body = new CilBody();
            TTH.Body = body;
            body.Instructions.Add(OpCodes.Nop.ToInstruction());
            body.Instructions.Add(OpCodes.Call.ToInstruction(module.Import(typeof(Encoding).GetMethod("get_UTF8", new Type[] { }))));
            body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            body.Instructions.Add(OpCodes.Call.ToInstruction(module.Import(typeof(System.Convert).GetMethod("FromBase64String", new Type[] { typeof(string) }))));
            body.Instructions.Add(OpCodes.Callvirt.ToInstruction(module.Import(typeof(System.Text.Encoding).GetMethod("GetString", new Type[] { typeof(byte[]) }))));
            body.Instructions.Add(OpCodes.Ret.ToInstruction());
            foreach (TypeDef type in module.Types)
            {
                if (type.Name != "Resources" || type.Name != "Settings")
                {
                    foreach (MethodDef method in type.Methods)
                    {
                        if (!method.HasBody)
                            continue;
                        for (int i = 0; i < method.Body.Instructions.Count(); i++)
                        {
                            if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                            {
                                method.Body.Instructions[i].Operand = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(method.Body.Instructions[i].Operand.ToString()));
                                method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Call, TTH));
                                i += 1;
                            }
                        }
                        method.Body.SimplifyBranches();
                        method.Body.OptimizeBranches();
                    }
                }
            }
        }
    }
}
