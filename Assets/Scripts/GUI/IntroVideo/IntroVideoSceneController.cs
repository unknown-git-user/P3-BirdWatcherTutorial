using UnityEngine;
using System.Collections;
using Francotronics.BWT.GUI;
using Multimorphic.P3App.GUI;

namespace Francotronics.BWT.GUI {

	public class IntroVideoSceneController : BWTSceneController {

		public bool useAlternateAudio;
		private AudioSource alternateAudio;
		private GameObject P3PlayfieldObj;
		private GameObject BWTAudioObj;

		// Use this for initialization
		public override void Start () {
			base.Start ();

			GameObject obj = GameObject.Find ("AlternateAudio");
			if (obj) {
				alternateAudio = obj.GetComponent<AudioSource>();
				alternateAudio.gameObject.SetActive (useAlternateAudio);
			}

			// Turn off playfield so that video runs smoothly
			P3PlayfieldObj = GameObject.Find ("P3Playfield(Clone)");
			if (P3PlayfieldObj)
				P3PlayfieldObj.SetActive(false);

			BWTAudioObj = GameObject.Find ("BWTAudio(Clone)");
			if (BWTAudioObj) {
				BWTAudioObj.SetActive(!useAlternateAudio);
				BWTAudioObj.GetComponent<BWTAudio>().moveWithCamera = false;
			}

			BWTAudio.Instance.ChangePlaylistByName("IntroAnimAudio");
		}

		protected override void CreateEventHandlers() {
			base.CreateEventHandlers ();
			AddModeEventHandler("Evt_StopIntroVideo", StopVideo);
		}
		
		// Update is called once per frame
		public override void Update () {
			IntroActive = false;
			base.Update ();
		}

		protected override void SceneLive() {
			base.SceneLive();
		}

		private void StopVideo(string eventName, object eventObject) {
            if (BWTAudio.Instance)
                BWTAudio.Instance.StopAllPlaylists();
		}

		protected override void OnDestroy () {
            if (BWTAudio.Instance)
                BWTAudio.Instance.StopAllPlaylists();

			// Reneable objects for normal gameplay
			if (P3PlayfieldObj)
				P3PlayfieldObj.SetActive(true);

			if (BWTAudioObj) {
				BWTAudioObj.SetActive(true);
				BWTAudioObj.GetComponent<BWTAudio>().moveWithCamera = true;
			}

			base.OnDestroy();
		}
	}
}