using System;
using Multimorphic.P3;
using Multimorphic.P3App.Modes;
using Multimorphic.NetProcMachine.Machine;
using System.Collections.Generic;

namespace Francotronics.BWT.Modes
{

	public class BWTHomeMode : SceneMode
	{
		private bool _ballStarted;
		// private SomeRelatedMode someRelatedMode;

        public BWTHomeMode (P3Controller controller, int priority, string SceneName)
                : base(controller, priority, SceneName)
        {
            _ballStarted = false;
            AddModeEventHandler(BWTEventNames.InitialLaunch, InitialLaunchEventHandler, Priority);
        }

        protected override void LaunchCallback()
        {
            Multimorphic.P3App.Logging.Logger.Log("Launch Callback. Ball Launched");
            PostModeEventToModes (BWTEventNames.EnableBallSearch, true);
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

			// p3.AddMode(someRelatedMode);

			// AddGUIEventHandler ("Evt_SomeGUIEventName", SomeHandlerFunction);
			// AddModeEventHandler ("SomeModeEventName", SomeHandlerFunction, priority);
		}

		public override void mode_stopped ()
		{
			// p3.RemoveMode (someRelatedMode);
			// RemoveGUIEventHandler ("Evt_SomeGUIEventName", SomeHandlerFunction);
			// RemoveModeEventHandler ("Evt_SomeModeEventName", SomeHandlerFunction, priority);
			// p3.RemoveMode(someRelatedMode);
			base.mode_stopped();
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
            PostModeEventToModes (BWTEventNames.BallDrained, 0);
            End();

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

	}
}
