using dnlib.DotNet.Emit;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeyObfuscator.Protections
{
    internal class Mutation
    {
        public static List<Instruction> instr = new List<Instruction>();
        public static bool CanObfuscateLDCI4(IList<Instruction> instructions, int i)
        {
            if (instructions[i + 1].GetOperand() != null)
                if (instructions[i + 1].Operand.ToString().Contains("bool"))
                    return false;
            if (instructions[i + 1].OpCode == OpCodes.Newobj)
                return false;
            if (instructions[i].GetLdcI4Value() == 0 || instructions[i].GetLdcI4Value() == 1)
                return false;
            return true;
        }
        public static void Execute(ModuleDef module)
        {
            foreach (var type in module.Types)
            {
                foreach (var method in type.Methods)
                {
                    MutationUtils.Br_S(method);
                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        if (method.Body.Instructions[i].OpCode == OpCodes.Ldc_I4)
                        {
                            if (method.Body.Instructions[i].Operand != null)
                            {
                                if (method.Body.Instructions[i].GetLdcI4Value() < int.MaxValue)
                                {
                                    if (CanObfuscateLDCI4(method.Body.Instructions, i))
                                    {
                                        switch (MutationUtils.rnd.Next(0, 7))
                                        {
                                            case 0:
                                                MutationUtils.IntegerParse(method, ref i);
                                                break;
                                            case 1:
                                                MutationUtils.EmptyType(method, ref i);
                                                break;
                                            case 2:
                                                MutationUtils.DoubleParse(method, ref i);
                                                break;
                                            case 3:
                                                MutationUtils.FloorReplacer(method, ref i);
                                                break;
                                            case 4:
                                                MutationUtils.CeilingReplacer(method, ref i);
                                                break;
                                            case 5:
                                                MutationUtils.RoundReplacer(method, ref i);
                                                break;
                                            case 6:
                                                MutationUtils.SqrtReplacer(method, ref i);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    class MutationUtils
    {
        public static Random rnd = new Random();
        private static double RandomDouble(double x, double y)
        {
            return new Random().NextDouble() * (y - x) + x;
        }
        public static void EmptyType(MethodDef method, ref int i)
        {
            int operand = method.Body.Instructions[i].GetLdcI4Value();
            method.Body.Instructions[i].Operand = operand - Type.EmptyTypes.Length;
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
            method.Body.Instructions.Insert(i + 1, OpCodes.Ldsfld.ToInstruction(method.Module.Import(typeof(Type).GetField("EmptyTypes"))));
            method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldlen));
            method.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Add));
            i += 3;
        }
        public static void DoubleParse(MethodDef method, ref int i)
        {
            int operand = method.Body.Instructions[i].GetLdcI4Value();
            double n = RandomDouble(1.0, 1000.0);
            string converter = Convert.ToString(n);
            double nEw = double.Parse(converter);
            int conta = operand - (int)nEw;
            method.Body.Instructions[i].Operand = conta;
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
            method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Ldstr, converter));
            method.Body.Instructions.Insert(i + 2, OpCodes.Call.ToInstruction(method.Module.Import(typeof(double).GetMethod("Parse", new Type[] { typeof(string) }))));
            method.Body.Instructions.Insert(i + 3, OpCodes.Conv_I4.ToInstruction());
            method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Add));
            i += 4;
        }
        public static void FloorReplacer(MethodDef method, ref int i)
        {
            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig + RandomDouble(0.01, 0.99);
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Floor", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void RoundReplacer(MethodDef method, ref int i)
        {
            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig + RandomDouble(0.01, 0.5);
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Round", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void SqrtReplacer(MethodDef method, ref int i)
        {

            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig * orig;
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Sqrt", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void AbsReplacer(MethodDef method, ref int i)
        {

            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig * orig;
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Abs", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void LogReplacer(MethodDef method, ref int i)
        {

            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig * orig;
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Log", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void AtanReplacer(MethodDef method, ref int i)
        {

            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig * orig;
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Atan", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void AsinReplacer(MethodDef method, ref int i)
        {

            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig * orig;
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Asin", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void AcosReplacer(MethodDef method, ref int i)
        {

            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig * orig;
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Acos", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void SingReplacer(MethodDef method, ref int i)
        {

            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig * orig;
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Sing", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void CeilingReplacer(MethodDef method, ref int i)
        {
            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig - 1 + RandomDouble(0.01, 0.99);
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Ceiling", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void MaxReplacer(MethodDef method, ref int i)
        {
            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig - 1 + RandomDouble(0.01, 0.99);
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Max", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void MinReplacer(MethodDef method, ref int i)
        {
            int orig = (int)method.Body.Instructions[i].Operand;
            double m = (double)orig - 1 + RandomDouble(0.01, 0.99);
            method.Body.Instructions[i].OpCode = OpCodes.Ldc_R8;
            method.Body.Instructions[i].Operand = m;
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Math).GetMethod("Min", new Type[] { typeof(double) }))));
            method.Body.Instructions.Insert(i + 2, OpCodes.Conv_I4.ToInstruction());
            i += 2;
        }
        public static void Br_S(MethodDef method)
        {
            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                Instruction instr = method.Body.Instructions[i];
                if (instr.IsLdcI4())
                {
                    int operand = instr.GetLdcI4Value();
                    instr.OpCode = OpCodes.Ldc_I4;
                    instr.Operand = operand - 1;
                    int valor = rnd.Next(100, 500);
                    int valor2 = rnd.Next(1000, 5000);
                    method.Body.Instructions.Insert(i + 1, Instruction.CreateLdcI4(valor));
                    method.Body.Instructions.Insert(i + 2, Instruction.CreateLdcI4(valor2));
                    method.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Clt));
                    method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Conv_I4));
                    method.Body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Add));
                    i += 5;
                }
            }
        }
        public static void IntegerParse(MethodDef method, ref int i)
        {
            int op = method.Body.Instructions[i].GetLdcI4Value();
            method.Body.Instructions[i].OpCode = OpCodes.Ldstr;
            method.Body.Instructions[i].Operand = Convert.ToString(op);
            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method.Module.Import(typeof(Int32).GetMethod("Parse", new Type[] { typeof(string) }))));
            i += 1;
        }
        public static int[] rndsizevalues = new int[] { 1, 2, 4, 8, 12, 16 };
        public static Dictionary<int, Tuple<TypeDef, int>> Dick = new Dictionary<int, Tuple<TypeDef, int>>();
        static int abc = 0;
        public static void StructGenerator(MethodDef method, ref int i)
        {
            ITypeDefOrRef valueTypeRef = new Importer(method.Module).Import(typeof(System.ValueType));
            TypeDef structDef = new TypeDefUser(Guid.NewGuid().ToString(), valueTypeRef);
            Tuple<TypeDef, int> outTuple;
            structDef.ClassLayout = new ClassLayoutUser(1, 0);
            structDef.Attributes |= TypeAttributes.Sealed | TypeAttributes.SequentialLayout | TypeAttributes.Public;
            List<Type> retList = new List<Type>();
            int rand = rndsizevalues[rnd.Next(0, 6)];
            retList.Add(GetType(rand));
            retList.ForEach(x => structDef.Fields.Add(new FieldDefUser(Guid.NewGuid().ToString(), new FieldSig(new Importer(method.Module).Import(x).ToTypeSig()), FieldAttributes.Public)));
            int operand = method.Body.Instructions[i].GetLdcI4Value();
            if (abc < 25)
            {
                method.Module.Types.Add(structDef);
                Dick.Add(abc++, new Tuple<TypeDef, int>(structDef, rand));
                int conta = operand - rand;
                method.Body.Instructions[i].Operand = conta;
                method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, structDef));
                method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Add));
            }
            else
            {
                Dick.TryGetValue(rnd.Next(1, 24), out outTuple);
                int conta = operand - outTuple.Item2;
                method.Body.Instructions[i].Operand = conta;
                method.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, outTuple.Item1));
                method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Add));
            }
            i += 2;
        }
        private static Type GetType(int operand)
        {
            switch (operand)
            {
                case 1:
                    switch (rnd.Next(0, 3))
                    {
                        case 0: return typeof(Boolean);
                        case 1: return typeof(SByte);
                        case 2: return typeof(Byte);
                    }
                    break;
                case 2:
                    switch (rnd.Next(0, 3))
                    {
                        case 0: return typeof(Int16);
                        case 1: return typeof(UInt16);
                        case 2: return typeof(Char);
                    }
                    break;
                case 4:
                    switch (rnd.Next(0, 3))
                    {
                        case 0: return typeof(Int32);
                        case 1: return typeof(Single);
                        case 2: return typeof(UInt32);
                    }
                    break;
                case 8:
                    switch (rnd.Next(0, 5))
                    {
                        case 0: return typeof(DateTime);
                        case 1: return typeof(TimeSpan);
                        case 2: return typeof(Int64);
                        case 3: return typeof(Double);
                        case 4: return typeof(UInt64);
                    }
                    break;

                case 12: return typeof(ConsoleKeyInfo);

                case 16:
                    switch (rnd.Next(0, 2))
                    {
                        case 0: return typeof(Guid);
                        case 1: return typeof(Decimal);
                    }
                    break;
            }

            return null;
        }
        public static List<Type> CreateTypeList(int size, out int total)
        {
            List<Type> retList = new List<Type>();
            int t = 0;
            while (size != 0)
            {
                if (16 <= size)
                {
                    size -= 16;
                    t += 16;
                    retList.Add(GetType(16));
                }
                else if (12 <= size)
                {
                    size -= 12;
                    t += 12;
                    retList.Add(GetType(12));
                }
                else if (8 <= size)
                {
                    size -= 8;
                    t += 8;
                    retList.Add(GetType(8));
                }
                else if (4 <= size)
                {
                    size -= 4;
                    t += 4;

                    retList.Add(GetType(4));
                }
                else if (2 <= size)
                {
                    size -= 2;
                    t += 2;
                    retList.Add(GetType(2));
                }
                else if (1 <= size)
                {
                    size -= 1;
                    t += 1;
                    retList.Add(GetType(1));
                }
            }

            total = t;
            return retList;
        }
    }
}
