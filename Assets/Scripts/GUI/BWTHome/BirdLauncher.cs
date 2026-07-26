using UnityEngine;
using Multimorphic.P3App.GUI;
using Francotronics.BWT.Modes;

namespace Francotronics.BWT.GUI
{
    public class BirdLauncher : P3Aware
    {
        public GameObject[] Bushes;
        public GameObject BirdPrefab;

        public override void Start ()
        {
            base.Start ();
        }

        protected override void CreateEventHandlers()
        {
            base.CreateEventHandlers ();
            AddModeEventHandler(BWTEventNames.SpawnBird, SpawnBirdEventHandler);
            AddModeEventHandler(BWTEventNames.SpawnBirdFromIndex, SpawnBirdFromIndexEventHandler);
        }

        // This is a simple helper that can be called from AppConfig keyboard shortcut.
        private void SpawnBirdEventHandler(string eventName, object eventData)
        {
            SpawnBirdFromIndexEventHandler(eventName, 0);
        }

        private void SpawnBirdFromIndexEventHandler(string eventName, object eventData)
        {
            int bushIndex = (int)eventData;
            // Generate a random flight vector. We constrain it to be upwards but dont go off screen right away.
            Vector3 velocity = Bushes[bushIndex].GetComponent<Bush>().RandomDirection();            // To make the bird face the direction of of movement, we use LookRotation.
            GameObject bird = Instantiate(BirdPrefab, Bushes[bushIndex].transform.position + new Vector3(0,0.5f,0), Quaternion.LookRotation(velocity, new Vector3(0,1,0)));
            bird.transform.SetParent(transform,true);
            bird.GetComponent<Bird>().Velocity = velocity;
            Destroy(bird, 5);
        }

        // Update is called once per frame
        public override void Update ()
        {
            base.Update ();
        }
    }
}
