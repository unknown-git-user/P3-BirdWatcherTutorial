using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;

using Multimorphic.NetProcMachine.LEDs;
using Multimorphic.P3;

namespace Francotronics.BWT.Modes
{
    public class HighlightRampMode : BWTGameMode
    {       
        private List<KeyValuePair<string, float>> _sortedLEDs;
        private List<List<string>> _entranceLEDs;
        List<LEDScript> LocalLEDScripts;

        public HighlightRampMode (P3Controller controller, int priority, List<List<string>> entranceLEDs )
            : base(controller, priority)
        {
            _entranceLEDs = entranceLEDs;
        }

        public override void mode_started ()
        {
            _sortedLEDs = new List<KeyValuePair<string, float>>();
            AddLedsToList(_entranceLEDs, out _sortedLEDs );
            LocalLEDScripts = new List<LEDScript>();
            foreach (KeyValuePair<string, float> pair in _sortedLEDs)
            {
                LEDScript script = new LEDScript( p3.LEDs[pair.Key], this.Priority);
                script.AddCommand(Multimorphic.P3.Colors.Color.yellow, 0.1, 1);
                script.AddCommand(Multimorphic.P3.Colors.Color.grey, 0.5, 1);
                LocalLEDScripts.Add (script);
                float offset = pair.Value * 4.0f;
                offset -= (float)Math.Floor(offset);
                p3.LEDController.AddScript(script, -1, offset*2);
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

        private void AddLedsToList(List<List<string>> lightList, out List<KeyValuePair<string, float>> output)
        {
            Regex re = new Regex("^(.*)([0-9]+)-([0-9]+)$");
            output = new List<KeyValuePair<string, float>>();

            foreach (List<string> list in lightList)
            {
                int index=0;
                foreach (string name in list)
                {
                    // led string like "rightRampInner0-131"
                    if (name.Contains("-"))
                    {
                        Match m = re.Match(name);
                        string baseName = m.Groups[1].Value;
                        int startIndex = Int32.Parse(m.Groups[2].Value);
                        int endIndex = Int32.Parse(m.Groups[3].Value);
                        Debug.Assert(startIndex <= endIndex, "startIndex is less than endIndex on name:" + name);
                        for (int i = startIndex; i<= endIndex; i++ )
                        {
                            string ledName = baseName + i.ToString();
                            float dist = (float)i / (float)endIndex;
                            output.Add(new KeyValuePair<string, float>(baseName + i.ToString(), dist)); 
                        }
                    }
                    // Named led like "leftRamp"
                    else
                    {
                        output.Add(new KeyValuePair<string, float>(name, (float)index / (float)list.Count));
                    }
                    index++;
                }
            }
        }
    }
}
