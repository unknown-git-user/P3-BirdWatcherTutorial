using UnityEngine;
using System.Collections;
using System;
using Multimorphic.P3App.GUI;
using Francotronics.BWT.Modes;

namespace Francotronics.BWT.GUI
{
	public class BWTSetup : Setup {

		public override void Awake() {
			baseAppModeType = typeof(BWTBaseGameMode);
			base.Awake();

            if (Application.isEditor)   // Only filter the log in the Unity editor
            {
                // Filter the log here.  For performance reasons, don't overdo it.
                // P3App.Logging.Logger.IncludeOnlyMessagesContaining.Add("InterestingFoo");
                // P3App.Logging.Logger.IncludeOnlyMessagesContaining.Add("InterestingBar");
                // P3App.Logging.Logger.ExcludeMessagesContaining.Add("AnnoyingThing");
                // P3App.Logging.Logger.ExcludeMessagesContaining.Add("AnotherAnnoyingThing");
            }
        }

        // Use this for initialization
        public override void Start () {
			base.Start();

			if (GameObject.FindObjectOfType<BWTAudio>() == null ) {
				GameObject mainCamera = GameObject.Find ("Main Camera");
				UnityEngine.Object resource = Resources.Load<GameObject>("Prefabs/BWTAudio");
				GameObject audio = (GameObject) GameObject.Instantiate(resource, mainCamera.transform.position, mainCamera.transform.localRotation);
				GameObject.DontDestroyOnLoad(audio);
			}
		}
	}
}
