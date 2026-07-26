using UnityEngine;
using Multimorphic.P3App.GUI;
using Francotronics.BWT.Modes;

namespace Francotronics.BWT.GUI
{
    public class BirdBook : P3Aware
    {
        public GameObject PhotoLayout;
        public GameObject PhotoPrefab;
        private bool _animatePhotos;
        private double _timeToNextPhoto;
        private int _photosLeft;

        public override void Start ()
        {
            base.Start ();
        }

        protected override void CreateEventHandlers()
        {
            base.CreateEventHandlers ();
            AddModeEventHandler(BWTEventNames.AnimatePhotos, AnimatePhotosEventHandler);
            AddModeEventHandler(BWTEventNames.ShowBirdBook, ShowBirdBookEventHandler);
        }

        // Update is called once per frame
        public override void Update ()
        {
            base.Update ();
            if (_animatePhotos)
            {
                _timeToNextPhoto -= Time.deltaTime;
                if (_timeToNextPhoto <= 0)
                {
                    AddPhoto();
                    _photosLeft--;
                    if (_photosLeft <= 0)
                    {
                        _animatePhotos = false;
                        PostGUIEventToModes(BWTEventNames.PhotoAnimationComplete, null);
                    }
                    else
                    {
                        _timeToNextPhoto = 0.2;
                    }
                }
            }
        }

        public void ShowBirdBookEventHandler(string eventName, object eventData)
        {
            bool enable = (bool)eventData;
            GetComponentInParent<Canvas>().enabled = enable;
        }

        public void AnimatePhotosEventHandler(string eventName, object eventData)
        {
            _photosLeft = (int)eventData;
            // if there are no Photos, just short circuit the animation call.
            if (_photosLeft <= 0)
            {
                _animatePhotos = false;
                PostGUIEventToModes(BWTEventNames.PhotoAnimationComplete, null);
            }
            else
            {
                _animatePhotos = true;
                _timeToNextPhoto = 0.2;
            }
        }

        public void AddPhoto()
        {
            GameObject photo = Instantiate(PhotoPrefab, Vector3.zero, Quaternion.identity);
            photo.transform.SetParent(PhotoLayout.transform, false);
        }
    }
}
