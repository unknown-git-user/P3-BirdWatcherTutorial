using System;
using Multimorphic.P3;
using Multimorphic.P3App.Modes;
using Multimorphic.NetProcMachine.Machine;
using Multimorphic.NetProcMachine.Config;
using System.Collections.Generic;

namespace Francotronics.BWT.Modes
{

	public class BWTHomeMode : SceneMode
	{
		private bool _ballStarted;
		private BirdRampMode _birdRampMode;
		private TimerMode _mainTimer;
		// private SomeRelatedMode someRelatedMode;

        public BWTHomeMode (P3Controller controller, int priority, string SceneName)
            : base(controller, priority, SceneName)
        {
            _ballStarted = false;
            _birdRampMode = new BirdRampMode(p3,BWTPriorities.PRIORITY_BIRD_RAMPS);

            AddModeEventHandler(BWTEventNames.InitialLaunch, InitialLaunchEventHandler, Priority);

			// Handling Holes
            foreach (BallPathDefinition shot in p3.BallPaths.Values)
            {
                if (shot.ExitType == BallPathExitType.Hole)
                {
                    Multimorphic.P3App.Logging.Logger.Log(
                        Multimorphic.P3App.Logging.LogCategories.Game,
                        "Installing ShotHitHandler for : " + shot.CompletedEvent + " exitType: " + shot.ExitType.ToString());
                    AddModeEventHandler(shot.CompletedEvent, HolePathEventHandler, priority);
                }
            }
            AddModeEventHandler("Evt_TroughLauncherEntry", HolePathEventHandler, priority);
			_mainTimer = new TimerMode(p3, priority, MainTimerExpired, "Main");
        }

        protected override void LaunchCallback()
        {
            Multimorphic.P3App.Logging.Logger.Log("Launch Callback. Ball Launched");
            p3.AddMode(_mainTimer);
#if DEBUG
            _mainTimer.Start(10);
#else
            _mainTimer.Start(45);
#endif

			PostModeEventToGUI("Evt_HideButtonLegend", null);
        }

        public bool sw_launch_active(Switch sw)
        {
            if (!_ballStarted)
            {
                _ballStarted = true;
                PostModeEventToModes(BWTEventNames.InitialLaunch, null);
            }
            return SWITCH_CONTINUE;
        }

        public bool sw_launch_active_for_2s(Switch sw)
        {
            Multimorphic.P3App.Logging.Logger.Log("Debug Launch.");
            BWTBallLauncher.launch();
            return SWITCH_CONTINUE;
        }

        public bool InitialLaunchEventHandler(string eventName, object eventData)
        {
            PostModeEventToModes(BWTEventNames.BallStarted, 0);
            PostModeEventToModes(BWTEventNames.ChangeGameState, GameState.BallInPlay);
            BWTBallLauncher.launch(LaunchCallback);
            return EVENT_STOP;
        }

        public override void mode_started ()
        {
            base.mode_started ();
            p3.AddMode(_birdRampMode);
			DefaultLamps();
        }

        public override void mode_stopped ()
        {
            base.mode_stopped();
            p3.RemoveMode(_birdRampMode);
        }

		public override void LoadPlayerData()
		{
			base.LoadPlayerData();
			// Add any special data loading needed here for this scene and this player
		}

		public override void SavePlayerData()
		{
			base.SavePlayerData();
			// Add any special data loading needed here for this scene and this player
		}

		public override void SceneLiveEventHandler( string evtName, object evtData )
		{
            // We are avoiding calling the the base here because our Home mode does not work like standard "mode" scenes.
            // base.SceneLiveEventHandler(evtName, evtData);
            // Add any special setup that the scene requires here, including sending messages to the GUI.
            StartPlaying();
		}

		protected override void StartPlaying()
        {
            base.StartPlaying();
            _ballStarted = false;

            // Enable the flippers, slings and pop bumpers.
            PostModeEventToModes (BWTEventNames.EnableFlippers, true);
            PostModeEventToModes (BWTEventNames.EnableBumpers, true);
            PostModeEventToModes (BWTEventNames.EnableBallSearch, false);

            // Standard call to the GUI player to initial the Scene
            PostModeEventToGUI(BWTEventNames.BWTHomeSetup, 0);
			ShowButtonLegend();
			PostModeEventToGUI(BWTEventNames.ShowBirdBook, false);
        }


		protected override void Completed(long score)
		{
			base.Completed (score);
			PostModeEventToModes ("Evt_BWTHomeCompleted", 0);
		}


		public bool sw_slingL_active(Switch sw)
		{
			// Add code here to let he GUI side know about that a sling has been hit
			//e.g. PostModeEventToGUI("Evt_BWTHomeSlingHit", false);

			return SWITCH_CONTINUE;   // use SWITCH_STOP to prevent other modes from receiving this notification.
		}

		public bool sw_slingR_active(Switch sw)
		{
			// Add code here to let he GUI side know about that a sling has been hit
			// e.g. PostModeEventToGUI("Evt_BWTHomeSlingHit", false);
			return SWITCH_CONTINUE;   // use SWITCH_STOP to prevent other modes from receiving this notification.
		}

        public bool sw_drain_active(Switch sw)
        {
            BWTBallLauncher.launch();
            return SWITCH_CONTINUE;
        }

        public override void End()
        {
            Pause();
            base.End ();
            PostModeEventToModes (BWTEventNames.EnableFlippers, false);
            PostModeEventToModes (BWTEventNames.EnableBumpers, false);
            PostModeEventToModes (BWTEventNames.EnableBallSearch, false);
            PostModeEventToModes (BWTEventNames.BallEnded, null);
        }

		public void Pause()
		{
			p3.ModesToGUIEventManager.Post("Evt_ScenePause", null);
		}

		public override ModeSummary getModeSummary()
		{
			ModeSummary modeSummary = new ModeSummary();
			modeSummary.Title = sceneName;
			modeSummary.Completed = modeCompleted;
			if (modeCompleted) 
				modeSummary.SetItemAndValue(0, "BWTHome completed!", "");
			else
				modeSummary.SetItemAndValue(1, "BWTHome not yet completed!", "");
			modeSummary.SetItemAndValue(2, "", "");
			return modeSummary;
		}

        // We are just going to default to launching a new ball if the Ball ends up in any of the Holes or the Through
        public bool HolePathEventHandler(string eventName, object eventData)
        {
            BWTBallLauncher.launch();
            return EVENT_STOP;
        }

        private void MainTimerExpired()
        {
            PostModeEventToModes(BWTEventNames.MainTimerExpired, null);
            EndOfBallAnimation();
			AddModeEventHandler("DoubleFlip", DoubleFlipEventHandler, Priority);
        }

		private bool DoubleFlipEventHandler(string eventName, object eventData)
        {
            CompleteGame();
            return EVENT_STOP;
        }

		private void EndOfBallAnimation()
        {
            PostModeEventToModes ("Evt_EnableFlippers", false);
            PostModeEventToModes ("Evt_EnableBumpers", false);
            AddGUIEventHandler(BWTEventNames.PhotoAnimationComplete, AnimationCompleteEventHandler);
            PostModeEventToGUI(BWTEventNames.ShowBirdBook, true);
            PostModeEventToGUI(BWTEventNames.AnimatePhotos, _birdRampMode.GetPhotoCount());
        }

        public void AnimationCompleteEventHandler(string eventName, object eventData)
        {
            this.delay("EndOfGame", Multimorphic.NetProc.EventType.None, (double)(5), new Multimorphic.P3.VoidDelegateNoArgs (CompleteGame));
        }

        public void CompleteGame()
        {
            cancel_delayed("EndOfGame");
            End();
        }

		private void ShowButtonLegend()
        {
            Dictionary<string, string> buttonLegend = new Dictionary<string, string>();
            // TODO: Fix for upper flipper games.
            buttonLegend["LeftWhiteButton"] = "";
            buttonLegend["RightWhiteButton"] = "";
            buttonLegend["LeftRedButton"] = "Left Flipper\n(+Start): Menu";
            buttonLegend["RightRedButton"] = "Right Flipper\n(+Start): Menu";
            buttonLegend["LeftYellowButton"] = "";
            buttonLegend["RightYellowButton"] = "";
            buttonLegend["LaunchButton"] = "Launch Ball";

            if (data.ball == 1)
            {
                if (data.Players.Count != data.GetGameAttributeValue("MaxPlayers").ToInt())
                {
                    buttonLegend["StartButton"] = "Add Player";
                    if (data.Players.Count > 1)
                        buttonLegend["StartButton"] += "\n(Hold 2s): Remove Player";

                }
                else if (data.Players.Count > 1)
                    buttonLegend["StartButton"] = "(Hold 2s): Remove Player";
            }
            else if (data.GetGameAttributeValue("SoftRestartEnabled").ToBool())
                buttonLegend["StartButton"] = "(Hold 2s): Reset Game";

            PostModeEventToGUI("Evt_ShowButtonLegend", buttonLegend);
        }

		public override bool sw_buttonLeft0_active(Switch sw)
        {
            if (p3.Switches["buttonRight0"].IsActive())
            {
                PostModeEventToModes("DoubleFlip", null);
            }
            return SWITCH_CONTINUE;
        }

        public override bool sw_buttonRight0_active(Switch sw)
        {
            if (p3.Switches["buttonLeft0"].IsActive())
            {
                PostModeEventToModes("DoubleFlip", null);
            }
            return SWITCH_CONTINUE;
        }

		private void DefaultLamps()
        {
            for (int i=0; i<LEDScripts.Count; i++)
            {
                if (LEDScripts[i].led.Name.Contains ("SideModule"))
                    LEDScripts[i] = LEDHelpers.OnLED (p3, LEDScripts[i], Multimorphic.P3.Colors.Color.green);
                else if (BRWDLedManager.IsPlayfiedInsert(LEDScripts[i].led.Name))
                    LEDScripts[i] = LEDHelpers.OnLED (p3, LEDScripts[i], BRWDLedManager.DarkGrey);
                else if (LEDScripts[i].led.Name.Contains ("flasher"))
                    LEDScripts[i] = LEDHelpers.OnLED (p3, LEDScripts[i], BRWDLedManager.DarkGrey);
                else if (LEDScripts[i].led.Name.Contains ("wall") || LEDScripts[i].led.Name.Contains("scoop"))
                    LEDScripts[i] = LEDHelpers.OnLED (p3, LEDScripts[i], Multimorphic.P3.Colors.Color.green);
                else if (LEDScripts[i].led.Name == "playfield")
                    LEDScripts[i] = LEDHelpers.OnLED(p3, LEDScripts[i], BRWDLedManager.Sunlight);
                else if (Array.Exists<string>(LEDScripts[i].led.Tags, x => x == "BaseP3"))
                    LEDScripts[i] = LEDHelpers.OnLED(p3, LEDScripts[i], Multimorphic.P3.Colors.Color.lightorange );
                else 
                    LEDScripts[i] = LEDHelpers.OnLED (p3, LEDScripts[i], BRWDLedManager.Sunlight);
            }
        }
	}
}
