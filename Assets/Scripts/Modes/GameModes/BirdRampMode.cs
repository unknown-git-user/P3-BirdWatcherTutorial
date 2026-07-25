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

            foreach (BallPathDefinition shot in p3.BallPaths.Values)
            {   
                if (shot.ExitName == "LeftInlane" || shot.ExitName == "RightInlane")
                {
                    Multimorphic.P3App.Logging.Logger.Log("[BirdRampMode] Setup shot: " + shot.CompletedEvent);
                    AddModeEventHandler(shot.CompletedEvent, RampHitEventHandler, priority);
                }
            }            
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

        private bool RampHitEventHandler(string eventName, object eventData)
        {
            PostModeEventToGUI(BWTEventNames.SpawnBirdFromIndex, UnityEngine.Random.Range(0,4));
            return EVENT_CONTINUE;
        }
    }
}
