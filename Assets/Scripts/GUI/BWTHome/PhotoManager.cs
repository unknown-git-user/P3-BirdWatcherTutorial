using UnityEngine;
using Multimorphic.P3App.GUI;

namespace Francotronics.BWT.GUI
{
    public class PhotoManager : P3Aware
    {
        public Camera MainCamera;
        public override void Start ()
        {
            base.Start ();
        }

        protected override void CreateEventHandlers()
        {
            base.CreateEventHandlers ();
        }

        // Update is called once per frame
        public override void Update ()
        {
            base.Update ();
        }

        public void TakePhoto(GameObject prefab, Vector3 pos)
        {
            BWTAudio.Instance.PlaySound3D("CameraSound", pos);
            Vector3 screenPos = MainCamera.WorldToScreenPoint(pos);
            GameObject photo = Instantiate(prefab, screenPos, Quaternion.identity);
            photo.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            photo.transform.SetParent(transform, true);
            Destroy(photo, 3);
        }
    }
}
