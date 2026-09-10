using System;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace SmokeyObfuscator.Protections
{
    internal static class Strings
    {
        public static void Execute(ModuleDefMD module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            MethodDefUser decoder = CreateStringDecoder(module);

            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody)
                        continue;

                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        Instruction instruction = method.Body.Instructions[i];
                        if (instruction.OpCode != OpCodes.Ldstr)
                            continue;

                        string originalValue = instruction.Operand as string;
                        if (originalValue == null)
                            continue;

                        instruction.Operand = Convert.ToBase64String(Encoding.UTF8.GetBytes(originalValue));
                        method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Call, decoder));
                        i++;
                    }

                    method.Body.SimplifyBranches();
                    method.Body.OptimizeBranches();
                }
            }
        }

        private static MethodDefUser CreateStringDecoder(ModuleDefMD module)
        {
            string methodName = "_decodeString" + Guid.NewGuid().ToString("N").Substring(0, 8);
            MethodDefUser decoder = new MethodDefUser(
                methodName,
                MethodSig.CreateStatic(module.CorLibTypes.String, module.CorLibTypes.String),
                MethodImplAttributes.IL | MethodImplAttributes.Managed,
                MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig);

            CilBody body = new CilBody();
            body.Instructions.Add(Instruction.Create(OpCodes.Call, module.Import(typeof(Encoding).GetProperty("UTF8").GetGetMethod())));
            body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            body.Instructions.Add(Instruction.Create(OpCodes.Call, module.Import(typeof(Convert).GetMethod("FromBase64String", new[] { typeof(string) }))));
            body.Instructions.Add(Instruction.Create(OpCodes.Callvirt, module.Import(typeof(Encoding).GetMethod("GetString", new[] { typeof(byte[]) })))) ;
            body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            decoder.Body = body;

            module.GlobalType.Methods.Add(decoder);
            return decoder;
        }
    }
}
