using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace The_Weed_Server_Mod.SpectateFolder
{
    [HarmonyPatch(typeof(SpectateCamera))]
    public class SpectateCameraPatch
    {
        private static readonly FieldInfo f_isDisabled = AccessTools.Field(typeof(PlayerAvatar), "isDisabled");
        private static readonly MethodInfo m_inputDown = AccessTools.Method(typeof(SemiFunc), "InputDown", (Type[])null, (Type[])null);
        private static readonly MethodInfo m_playerSwitch = AccessTools.Method(typeof(SpectateCamera), "PlayerSwitch", (Type[])null, (Type[])null);

        [HarmonyPatch("StateNormal")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> StateNormal_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList();
            int num = list.FindIndex(ci => CodeInstructionExtensions.Calls(ci, m_inputDown)) - 1;
            if (num <= 0 || list[num].opcode != OpCodes.Ldc_I4_1)
            {
                return instructions;
            }
            int num2 = list.FindIndex(num, ci => CodeInstructionExtensions.Calls(ci, m_playerSwitch));
            if (num2 == -1)
            {
                return instructions;
            }
            list[num].opcode = OpCodes.Nop;
            list.RemoveRange(num + 1, num2 - num);
            return list.AsEnumerable();
        }
    }
}