using System;
using Multimorphic.P3;
using Multimorphic.P3App.Modes;
using Multimorphic.NetProcMachine.Machine;
using System.Collections.Generic;

namespace Francotronics.BWT.Modes
{

	public class BWTHomeMode : SceneMode
	{

		// private SomeRelatedMode someRelatedMode;

		public BWTHomeMode (P3Controller controller, int priority, string SceneName)
			: base(controller, priority, SceneName)
		{
			// AddModeEventHandler("Evt_SomeModeEventName", SomeHandlerFunction, priority);
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
			base.SceneLiveEventHandler(evtName, evtData);
			// Add any special setup that the scene requires here, including sending messages to the GUI.
		}

		protected override void StartPlaying()
		{
			base.StartPlaying();

			PostModeEventToGUI("Evt_BWTHomeSetup", 0);

			// PostInstructionEvent("Some instructions");
			BWTBallLauncher.launch ();
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

		public override void End()
		{
			// someRelatedMode.End();

			Pause();
			// Save any remaining stats

			base.End ();
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
