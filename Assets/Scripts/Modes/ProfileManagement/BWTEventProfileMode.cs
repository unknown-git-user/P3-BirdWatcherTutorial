// Copyright � 2019 Multimorphic, Inc. All Rights Reserved

using Multimorphic.P3;
using Multimorphic.P3App.Modes;
using Multimorphic.P3App.Modes.Data;

namespace Francotronics.BWT.Modes.Data {

	public class BWTEventProfileMode : EventProfileMode {

		public BWTEventProfileMode(P3Controller controller, int priority, string eventProfileName, string eventProfileDir)
			: base(controller, priority, eventProfileName, eventProfileDir) {
			highScoresMode = new BWTHighScoresMode(p3, Priorities.PRIORITY_HIGH_SCORES, eventProfileName, eventProfileDir);
			statisticsMode = new BWTStatisticsMode(p3, Priority, eventProfileName, eventProfileDir);
		}
	}
}