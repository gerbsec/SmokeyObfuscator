using dnlib.DotNet.Emit;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmokeyObfuscator.Protections
{
    internal class JunkCode
    {
        public static void Execute(ModuleDefMD md)
        {
            List<uint> junkclasses = new List<uint>();
            int classnumber = 25;
            for (int i = 0; i < classnumber; i++)
            {
                TypeDefUser newtype = new TypeDefUser(Random2.GetRandomString(), Random2.GetRandomString());
                md.Types.Add(newtype);
                int methodcount = 25;
                for (int x = 0; x < methodcount; x++)
                {
                    MethodDefUser newmethod = new MethodDefUser(Random2.GetRandomString(), new MethodSig(CallingConvention.Default, 0, md.CorLibTypes.Void), MethodAttributes.Public | MethodAttributes.Static);
                    newtype.Methods.Add(newmethod);
                    newmethod.Body = new CilBody();
                    int localcount = 25;
                    for (int j = 0; j < localcount; j++)
                    {
                        Local lcl = new Local(md.CorLibTypes.Int32);
                        newmethod.Body.Variables.Add(lcl);
                        newmethod.Body.Instructions.Add(new Instruction(OpCodes.Ldc_I4, Random2.GetRandomInt()));
                        newmethod.Body.Instructions.Add(new Instruction(OpCodes.Stloc, lcl));
                    }
                    newmethod.Body.Instructions.Add(new Instruction(OpCodes.Ret));
                }
                junkclasses.Add(newtype.Rid);
            }
        }
    }
}
