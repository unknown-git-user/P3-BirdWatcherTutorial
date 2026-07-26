using UnityEngine;
using Multimorphic.P3App.GUI;
using Francotronics.BWT.Modes;

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
                PostGUIEventToModes(BWTEventNames.BirdHit, Score);
                _photoManager.GetComponent<PhotoManager>().TakePhoto(PhotoPrefab, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
