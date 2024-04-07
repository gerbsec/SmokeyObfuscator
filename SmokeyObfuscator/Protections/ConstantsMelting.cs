using dnlib.DotNet.Emit;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeyObfuscator.Protections
{
    internal class ConstantsMelting
    {
        public static void Execute(ModuleDefMD module)
        {
            foreach (TypeDef type in module.Types)
            {
                if (type.IsGlobalModuleType) continue;
                foreach (MethodDef method in type.Methods)
                {
                    if (method.FullName.Contains("My.")) continue;
                    if (method.IsConstructor) continue;
                    if (!method.HasBody) continue;
                    var instr = method.Body.Instructions;
                    for (int c = 0; c < 10; c++)
                    {
                        for (int i = 0; i < method.Body.Instructions.Count; i++)
                        {
                            if (instr[i].OpCode == OpCodes.Ldstr)
                            {
                                Random rn = new Random();
                                for (int j = 1; j < 1; j++)
                                {
                                    if (j != 1) j += 1;
                                    Local new_local = new Local(module.CorLibTypes.String);
                                    Local new_local2 = new Local(module.CorLibTypes.String);
                                    method.Body.Variables.Add(new_local);
                                    method.Body.Variables.Add(new_local2);
                                    instr.Insert(i + j, Instruction.Create(OpCodes.Stloc_S, new_local));
                                    instr.Insert(i + (j + 1), Instruction.Create(OpCodes.Ldloc_S, new_local));
                                }
                            }
                            if (instr[i].OpCode == OpCodes.Ldc_I4)
                            {
                                Random rn = new Random();
                                for (int j = 1; j < 1; j++)
                                {
                                    if (j != 1) j += 1;
                                    Local new_local = new Local(module.CorLibTypes.Int32);
                                    Local new_local2 = new Local(module.CorLibTypes.Int32);
                                    method.Body.Variables.Add(new_local);
                                    method.Body.Variables.Add(new_local2);
                                    instr.Insert(i + j, Instruction.Create(OpCodes.Stloc_S, new_local));
                                    instr.Insert(i + (j + 1), Instruction.Create(OpCodes.Ldloc_S, new_local));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
