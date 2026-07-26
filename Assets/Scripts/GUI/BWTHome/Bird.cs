using UnityEngine;
using Multimorphic.P3App.GUI;
using Francotronics.BWT.Modes;
using System.Collections.Generic;

namespace Francotronics.BWT.GUI
{    
    public class Bird : P3Aware
    {
        public Vector3 Velocity;
        public float MovementSpeed;
        public int Score = 100;
        public GameObject PhotoPrefab;
        private GameObject _photoManager;
        private bool _lockout;
        public string AudioClipGroup;

        public override void Start ()
        {
            base.Start ();
            _photoManager = GameObject.Find("PhotosOverlay");
        }

        protected override void CreateEventHandlers()
        {
            base.CreateEventHandlers ();
        }

        // Update is called once per frame
        public override void Update ()
        {
            base.Update ();
            transform.position = transform.position + new Vector3(
                    Velocity.x * MovementSpeed * Time.deltaTime,
                    Velocity.y * MovementSpeed * Time.deltaTime,
                    Velocity.z * MovementSpeed * Time.deltaTime);
        }

        private float[] ScreenSpaceToModuleSpace(Vector3 pos)
        {
            float width = 1080f;
            float height = 1920f;
            float widthConversion = 20f; // This is probably wrong, but good enough for this example.
            float heightConversion = 20f; // this is wrong, and not used.
            return new float[] {(float)(pos[0]/width)*widthConversion, (float)(pos[1]/height)*heightConversion, 0};
        }

        public void OnTriggerEnter(Collider other)
        {
            if (_lockout)
            {
                return;
            }
            // Only respond to ball hits
            if (other.name == "BallAvatarTrail")
            {
                _lockout = true;
                Vector3 pos = _photoManager.GetComponent<PhotoManager>().GetScreenPosition(transform.position);
                PostGUIEventToModes(
                    BWTEventNames.BirdHit,
                    new KeyValuePair<int, float[]>(Score, ScreenSpaceToModuleSpace(pos)));
                _photoManager.GetComponent<PhotoManager>().TakePhoto(PhotoPrefab, transform.position);
                Destroy(gameObject);
            }
        }

        public void PlayBirdCall()
        {
            BWTAudio.Instance.PlaySound3D(AudioClipGroup, transform, false);
        }
    }
}
