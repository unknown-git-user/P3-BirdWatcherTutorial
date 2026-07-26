using System;
using Multimorphic.P3;
using Multimorphic.P3App.Modes;
using Multimorphic.NetProcMachine.Config;

namespace Francotronics.BWT.Modes
{
    public class BirdRampMode : BWTGameMode
    {       
        private bool _bushReady;
        private int _nextSpawnIndex;
        private System.Random _rand;
        public static readonly string RandomBirdLaunch = "Timer_RandomBirdLaunch";

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

            AddModeEventHandler(BWTEventNames.InlaneRightLower, InlaneEventHandler, Priority);
            AddModeEventHandler(BWTEventNames.InlaneLeftLower, InlaneEventHandler, Priority);

            _bushReady = false;
            _nextSpawnIndex = -1;
            _rand = new System.Random();    
        }

        public void LaunchRandomBird()
        {
            int B1 = _rand.Next(5);
            SpawnBirdEventHandler(RandomBirdLaunch, B1.ToString());
            int timeToNextBird = _rand.Next(7) + 3;
            this.delay(RandomBirdLaunch, Multimorphic.NetProc.EventType.None, (double)(timeToNextBird), new Multimorphic.P3.VoidDelegateNoArgs (LaunchRandomBird));
        }

        public override void mode_started ()
        {
            base.mode_started ();
            int timeToNextBird = _rand.Next(7) + 3;
            this.delay(RandomBirdLaunch, Multimorphic.NetProc.EventType.None, (double)(timeToNextBird), new Multimorphic.P3.VoidDelegateNoArgs (LaunchRandomBird));
        }

        private bool MainTimerExpiredEventHandler(string eventName, object eventData)
        {
            cancel_delayed(RandomBirdLaunch);
            return EVENT_CONTINUE;
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
            Multimorphic.P3App.Logging.Logger.Log("Ramp hit. Spawning Bird.");

            // We will generate 2 possible places to spawn.
            int B1 = _rand.Next(5);
            int B2 = _rand.Next(4);
            // We select [0,3] and if we selected the same thing as before change it to 4. This does sampling without replacement.
            if (B1 == B2)
            {
                B2 = 4;
            }
            // Indicate that to inlane that we are ready to lauch
            _bushReady = true;
            // Post visual hints to the GUI layer
            PostModeEventToGUI(BWTEventNames.Shake, B1);
            PostModeEventToGUI(BWTEventNames.Shake, B2);
            _nextSpawnIndex = B1;
            return EVENT_CONTINUE;
        }

        private bool InlaneEventHandler(string eventName, object eventData) {
            if (_bushReady)
            {
                Multimorphic.P3App.Logging.Logger.Log("Inlane: " + eventName + "Crossed. Spawning Bird from index: " + _nextSpawnIndex.ToString());
                _bushReady = false;
                // We are using a string because of how we configured the Spawn event in AppConfig.
                SpawnBirdEventHandler(eventName, _nextSpawnIndex.ToString());
            }
            return EVENT_CONTINUE;
        }
    }
}
