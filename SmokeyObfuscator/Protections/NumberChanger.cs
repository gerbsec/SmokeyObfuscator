using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace SmokeyObfuscator.Protections
{
    internal static class NumberChanger
    {
        public static void Process(ModuleDefMD module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (method.Body == null)
                        continue;

                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        Instruction instruction = method.Body.Instructions[i];
                        if (instruction.OpCode != OpCodes.Ldc_I4 || !(instruction.Operand is int))
                            continue;

                        int originalValue = (int)instruction.Operand;
                        int offset = Math.Abs(originalValue) + 17;

                        instruction.Operand = originalValue + offset;
                        method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Ldc_I4, offset));
                        method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Sub));
                        i += 2;
                    }
                }
            }
        }
    }
}
