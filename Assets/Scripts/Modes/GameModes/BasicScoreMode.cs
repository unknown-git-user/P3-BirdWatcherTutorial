using System;
using System.Collections.Generic;
using Multimorphic.NetProcMachine.Machine;
using Multimorphic.P3;
using Multimorphic.P3App.Modes;
using Multimorphic.P3App.Modes.Data;
using Multimorphic.NetProcMachine.Config;

namespace Francotronics.BWT.Modes
{
    public class BasicScoreMode : P3Mode
    {
        public BasicScoreMode (P3Controller controller, int priority)
            : base(controller, priority)
        {
            AddModeEventHandler(BWTEventNames.LeftSlingHit, ShotsModeEventHandler, priority);
            AddModeEventHandler(BWTEventNames.RightSlingHit, ShotsModeEventHandler, priority);
            foreach (BallPathDefinition shot in p3.BallPaths.Values)
            {
                if (shot.ExitType == BallPathExitType.Target )
                {
                    AddModeEventHandler(shot.CompletedEvent, TargetHitEventHandler, priority);
                }
                else if (shot.ExitType == BallPathExitType.PlayfieldLocation)
                {
                    foreach (string tag in shot.Tags)
                    {
                        if (tag == "Loop")
                        {
                            AddModeEventHandler(shot.CompletedEvent, LoopHitEventHandler, priority);
                            break;
                        }
                    }
                }
            }
        }

        public bool TargetHitEventHandler(string eventName, object eventData) {
            ScoreManager.Score(Scores.TARGET_HIT_SCORE);
            return EVENT_CONTINUE;
        }

        public bool LoopHitEventHandler(string eventName, object eventData) {
            ScoreManager.Score(Scores.LOOP_HIT_SCORE);
            return EVENT_CONTINUE;
        }

        public override void mode_started ()
        {
            base.mode_started ();
        }

        public bool ShotsModeEventHandler(string eventName, object eventData)
        {
            long score = 0;
            bool rightSide = false;
            switch (eventName)
            {
                case BWTEventNames.RightSlingHit:
                    score = Scores.SLING_HITS_MIN;
                    rightSide = true;
                    break;
                case BWTEventNames.LeftSlingHit:
                    score = Scores.SLING_HITS_MIN;
                    rightSide = false;
                    break;
                default:
                    score = 0;
                    break;
            }
            ScoreManager.Score(score);
            PostSlingshotHitToGUI(score, rightSide, eventData);
            FlashSling(rightSide);
            return EVENT_CONTINUE;
        }

        private void FlashSling(bool rightSide)
        {
            string[] candidates = rightSide
                ? new string[] { "flasherBBRight", "wall5", "wall4", "flasherSideModuleRight0" }
                : new string[] { "flasherBBLeft", "wall0", "wall1", "flasherSideModuleLeft0" };

            foreach (string ledName in candidates)
            {
                if (p3.LEDs.ContainsKey(ledName))
                {
                    LEDHelpers.LEDFlashPop(p3, ledName, Multimorphic.P3.Colors.Color.yellow, 2, Priority + 1);
                }
            }
        }

        private void PostSlingshotHitToGUI(long score, bool rightSide, object eventData)
        {
            LocationScore info = new LocationScore();
            info.score = score;
            if (eventData is Switch)
            {
                info.location = ((Switch)eventData).Name;
            }
            else
            {
                info.location = rightSide ? "slingR" : "slingL";
            }
            PostModeEventToGUI("Evt_SlingshotHit", info);
        }
    }
}
