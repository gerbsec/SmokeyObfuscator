using dnlib.DotNet.Emit;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeyObfuscator.Protections
{
    internal class NumberChanger
    {
        private static ProtoRandom.ProtoRandom _random = new ProtoRandom.ProtoRandom(5);

        public static void Process(ModuleDefMD module)
        {
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (method.Body != null)
                    {
                        for (int i = 0; i < method.Body.Instructions.Count; i++)
                        {
                            Instruction instruction = method.Body.Instructions[i];

                            if (instruction.Operand is int && instruction.IsLdcI4() && instruction.OpCode == OpCodes.Ldc_I4)
                            {
                                List<Instruction> instructions = GenerateInstructions(Convert.ToInt32(instruction.Operand));
                                instruction.OpCode = OpCodes.Nop;

                                foreach (Instruction instr in instructions)
                                {
                                    method.Body.Instructions.Insert(i + 1, instr);
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static List<Instruction> GenerateInstructions(int value)
        {
            List<Instruction> instructions = new List<Instruction>();

            int num = _random.GetRandomInt32(100000);
            bool once = _random.GetRandomBoolean();
            int num1 = _random.GetRandomInt32(100000);
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4, value - num + (once ? (0 - num1) : num1)));
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4, num));
            instructions.Add(Instruction.Create(OpCodes.Add));
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4, num1));
            instructions.Add(Instruction.Create(once ? OpCodes.Add : OpCodes.Sub));
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4_0));
            instructions.Add(Instruction.Create(OpCodes.Add));
            return instructions;
        }
    }
}
