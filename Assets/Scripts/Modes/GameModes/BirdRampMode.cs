using System;
using Multimorphic.P3;
using Multimorphic.P3App.Modes;
using Multimorphic.NetProcMachine.Config;

namespace Francotronics.BWT.Modes
{
    public class BirdRampMode : BWTGameMode
    {       
        public BirdRampMode (P3Controller controller, int priority)
            : base(controller, priority)
        {
            AddModeEventHandler(BWTEventNames.SpawnBird, SpawnBirdEventHandler, Priority);
            AddGUIEventHandler(BWTEventNames.BirdHit, BirdHitEventHandler);
        }

        public override void mode_started ()
        {
            base.mode_started ();
        }

        public void BirdHitEventHandler(string eventName, object eventData)
        {
            int score = (int)eventData;
            ScoreManager.Score(score);          
        }

        public bool SpawnBirdEventHandler(string eventName, object eventData)
        {
            int bushIndex = Int32.Parse((string)eventData);
            PostModeEventToGUI(BWTEventNames.SpawnBirdFromIndex, bushIndex);
            return EVENT_STOP;
        }
    }
}
