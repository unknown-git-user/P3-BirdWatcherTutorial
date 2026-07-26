using System;
using System.Collections.Generic;
using Multimorphic.NetProcMachine.Machine;

namespace Francotronics.BWT.Modes
{
    public class BRWDLedManager 
    {       
        public static readonly HashSet<string> Inserts = new HashSet<string>
        {
            // Heist
            "sideLoop", "jailRight", "jailLeft",
            // WAMONH
            "sideLoopLeft", "viP", "miniLoopLeft", "cameraTarget", "sideLoopRight", "ticketCounterArrow",
            "vIp", "hamsterTarget", "rightRamp", "suppliesLower", "miniLoopRight", "rightLoop", "Vip",
            "leftRamp", "leftLoop", "suppliesArrow", "suppliesUpper", //TODO finish
        };
        public static readonly ushort[] DarkGrey = new ushort[] {10, 10, 10, 255};
        public static readonly ushort[] Sunlight = new ushort[] {253, 251, 211, 255};

        public static bool IsPlayfiedInsert(string name)
        {
            return Inserts.Contains(name);
        }
        public static bool IsCabinetLed(LED led)
        {
            if (Array.Exists<string>(led.Tags, x => x == "BaseP3"))
            {
                if (led.Name.Contains("scoop") || led.Name.Contains("wall") || led.Name.Contains("SideModule"))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
