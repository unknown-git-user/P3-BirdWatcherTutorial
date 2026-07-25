using Multimorphic.NetProcMachine.Machine;
using Multimorphic.P3;
using Multimorphic.P3App.Modes;
using Multimorphic.P3App.Modes.Data;
using Francotronics.BWT.Modes.Data;
using System;
using System.Collections.Generic;

namespace Francotronics.BWT.Modes.Data
{
    public class BWTGameAttributeManagerMode : GameAttributeManagerMode
	{
		List<HighScoreCategory> highScoreCats;
		public BWTGameAttributeManagerMode (P3Controller controller, int priority)
			: base(controller, priority)
		{
		}

		protected override void InstantiateAndAddModes()
		{
			base.InstantiateAndAddModes();

			settingsMode = new BWTSettingsMode (p3, Priority);
			// Use PRIORITY_HIGH_SCORES mode for the high score logic so the score boards work properly in Attract Mode.
			highScoresMode = new BWTHighScoresMode (p3, Priorities.PRIORITY_HIGH_SCORES);
			statisticsMode = new BWTStatisticsMode (p3, Priority);

			eventProfileManagerMode = new BWTEventProfileManagerMode(p3, Priority+1);
		}

		public override void mode_started ()
		{
			base.mode_started ();
			highScoreCats = BWTHighScoreCategories.GetCategories();
			SendHSCatsToHSModes();
		}


		private void SendHSCatsToHSModes()
		{
			foreach (HighScoreCategory cat in highScoreCats)
			{
				PostModeEventToModes ("Evt_AddHighScoreCategory", cat);
			}
		}

		protected override bool NewEventProfileAddedEventHandler (string evtName, object evtData)
		{
			SendHSCatsToHSModes();
			return base.NewEventProfileAddedEventHandler (evtName, evtData);
		}

		protected override void CreateStockProfiles()
		{
			base.CreateStockProfiles();
            /*
			profileManagerMode.AddStockProfile ("Default");
			profileManagerMode.AddStockProfile ("Easy");
			profileManagerMode.AddStockProfile ("Medium");
			profileManagerMode.AddStockProfile ("Hard");
			profileManagerMode.AddStockProfile ("Custom");
            */

            /*
			settingsMode.UpdateStockProfileAttributes ("Default");
			settingsMode.UpdateStockProfileAttributes ("Easy");
			settingsMode.UpdateStockProfileAttributes ("Medium");
			settingsMode.UpdateStockProfileAttributes ("Hard");
			settingsMode.UpdateStockProfileAttributes ("Custom");
            */
		}

	} 

}

