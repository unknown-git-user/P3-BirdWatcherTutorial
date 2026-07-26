using UnityEngine;
using System.Collections;
using Multimorphic.P3App.GUI;
using Multimorphic.P3App.Modes;
using Multimorphic.P3App.Data;
using Francotronics.BWT.Modes;

namespace Francotronics.BWT.GUI {

	public class BWTHomeSceneController : BWTSceneController {
	
		// Use this for initialization
		public override void Start () {
			base.Start ();
		}

		protected override void CreateEventHandlers() {
			base.CreateEventHandlers ();
			// AddModeEventHandler(("Evt_SomeEventName", SomeHandlerFunction);
		}

		public override void SlingshotHitEventHandler(string evtName, object evtData)
		{
			LocationScore info = (LocationScore) evtData;
			bool right = info.location == "slingR";
			PlayEdgeSound(slingshotSound, right);
		}
		
		// Update is called once per frame
		public override void Update () {
			base.Update ();
		}

		protected override void SceneLive() {
			base.SceneLive();
		}
	}
}
