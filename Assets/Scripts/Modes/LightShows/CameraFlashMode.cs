using System;
using System.Collections.Generic;

using Multimorphic.NetProcMachine.Machine;
using Multimorphic.NetProcMachine.LEDs;
using Multimorphic.P3;

namespace Francotronics.BWT.Modes
{
    public class CameraFlashMode : BWTGameMode
    {
        List<LEDScript> LocalLEDScripts;
        private float[] _center;

        public CameraFlashMode (P3Controller controller, int priority, float[] center)
            : base(controller, priority)
        {
            _center = center;
        }

        public override void mode_started ()
        {
            LocalLEDScripts = new List<LEDScript>();

            foreach (LED led in p3.LEDs.Values) {
                if (BRWDLedManager.IsCabinetLed(led))
                {
                    continue;
                }

                LEDScript script = new LEDScript(led, this.Priority);
                double delay = 0;

                if (led.Location != null)
                {
                    delay = Math.Abs((_center[0] - led.Location[0]) / 10.0)*0.1;
                }

                script.AddCommand(Multimorphic.P3.Colors.Color.off, 0, 0.05);                
                script.AddCommand(Multimorphic.P3.Colors.Color.on, 0, 0.1);
                script.AddCommand(BRWDLedManager.DarkGrey, .3, 5);

                script.autoRemove = true;
                LocalLEDScripts.Add (script);
                p3.LEDController.AddScript(script, 1.2 - delay, delay);
            }
        }

        public override void mode_stopped ()
        {
            for (int i=0; i<LocalLEDScripts.Count; i++)
            {
                p3.LEDController.RemoveScript(LocalLEDScripts[i]);
            }
            LocalLEDScripts.Clear();
            base.mode_stopped();
        }
    }
}
