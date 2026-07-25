// Copyright � 2019 Multimorphic, Inc. All Rights Reserved

using Multimorphic.P3;
using Multimorphic.P3App.Modes.Data;

namespace Francotronics.BWT.Modes.Data {

	public class BWTEventProfileManagerMode : EventProfileManagerMode {

		public BWTEventProfileManagerMode(P3Controller controller, int priority)
			: base(controller, priority) {
		}

		protected override void AddEventProfileMode(string eventProfileName) {
			eventProfiles[eventProfileName] = new BWTEventProfileMode(p3, Priority, eventProfileName, GetEventProfileDirectory(eventProfileName));
		}
	}
}