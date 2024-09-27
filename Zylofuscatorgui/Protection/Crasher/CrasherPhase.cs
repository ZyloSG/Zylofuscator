using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zylofuscatorgui.Protection.Renamer;

namespace Zylofuscatorgui.Protection.Crasher
{
    internal class CrasherPhase
    {
        public static void Execute(ModuleDefMD moduleDef)
        {
            /*
             USE ONLY IF UR USING AN .EXE FILE:
             moduleDef.Name = RenamerPhase.RandomString(99999, RenamerPhase.ChineseCharacters);
            */
            foreach (TypeDef type in moduleDef.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (method.Body == null) continue;
                    for (int x = 0; x < 33333; x++)
                    {
                        method.Body.Instructions.Insert(x, new Instruction(OpCodes.Nop));
                    }
                }
            }
        }
    }
    }

