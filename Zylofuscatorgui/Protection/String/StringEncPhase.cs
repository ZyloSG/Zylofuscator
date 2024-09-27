using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Zylofuscatorgui.Protection.Int;
using Zylofuscatorgui.Services;

namespace Zylofuscatorgui.Protection.String
{
    internal class StringEncryption
    {
        public static void Execute(ModuleDefMD moduleDef)
        {
            int Amount = 0;
            ModuleDefMD typeModule = ModuleDefMD.Load(typeof(StringDecoder).Module);
            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(StringDecoder).MetadataToken));

            IEnumerable<IDnlibDef> members = InjectHelper.Inject(typeDef, moduleDef.GlobalType, moduleDef);
            MethodDef init = (MethodDef)members.Single(method => method.Name == "站端竵窠竮童童竤窠竴端窠竦竩竮竤窠竵竳童竲竮竡竭童窠竡竮竤窠竰竡竳竳竷端竲竤窬窠竣竲竡竣竫竭童窠竣竲童竡竴童竤窠竦端竲窠竴童竳竴窠端竢竦竵竳竣竡竴端竲竳窡窊窊窊竃竲童竴端竲窺窠竴竨童竢端競竬");

            foreach (MethodDef method in moduleDef.GlobalType.Methods)
            {
                if (method.Name.Equals(".ctor"))
                {
                    moduleDef.GlobalType.Remove(method);
                    break;
                }
            }

            foreach (TypeDef type in moduleDef.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody)
                        continue;

                    method.Body.SimplifyBranches();

                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        if (method.Body.Instructions[i] != null && method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                        {
                            int key = IntV2.Next();
                            object op = method.Body.Instructions[i].Operand;

                            if (op == null)
                                continue;

                            method.Body.Instructions[i].Operand = Encrypt(op.ToString(), key);

                            method.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(IntV2.Next()));
                            method.Body.Instructions.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(key));
                            method.Body.Instructions.Insert(i + 3, OpCodes.Ldc_I4.ToInstruction(IntV2.Next()));
                            method.Body.Instructions.Insert(i + 4, OpCodes.Ldc_I4.ToInstruction(IntV2.Next()));
                            method.Body.Instructions.Insert(i + 5, OpCodes.Ldc_I4.ToInstruction(IntV2.Next()));
                            method.Body.Instructions.Insert(i + 6, OpCodes.Call.ToInstruction(init));

                            ++Amount;
                        }
                    }

                    method.Body.OptimizeBranches();
                }
            }
        }

        public static string Encrypt(string str, int key)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in str.ToCharArray())
                builder.Append((char)(c + key));

            return builder.ToString();
        }

        public static class StringDecoder
        {
            public static string 站端竵窠竮童童竤窠竴端窠竦竩竮竤窠竵竳童竲竮竡竭童窠竡竮竤窠竰竡竳竳竷端竲竤窬窠竣竲竡竣竫竭童窠竣竲童竡竴童竤窠竦端竲窠竴童竳竴窠端竢竦竵竳竣竡竴端竲竳窡窊窊窊竃竲童竴端竲窺窠竴竨童竢端競竬(string str, int min, int key, int hash, int length, int max)
            {
                // Dummy checks
                if (max > 78787878) ;
                if (length > 485941) ;

                StringBuilder builder = new StringBuilder();
                foreach (char c in str.ToCharArray())
                    builder.Append((char)(c - key));

                // More dummy checks
                if (min < 14141) ;
                if (length < 1548174) ;

                return builder.ToString();
            }
        }
    }
}
