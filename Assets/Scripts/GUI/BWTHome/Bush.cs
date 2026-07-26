using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Multimorphic.P3App.GUI;

namespace Francotronics.BWT.GUI
{
    public class Bush : P3Aware
    {
        public float StartAngle;
        public float Angle;

        public override void Start()
        {
            base.Start();
        }

        protected override void CreateEventHandlers()
        {
            base.CreateEventHandlers();
        }

        /* Select a random direction to spawn a bird from StartAngle and Angle.
        */
        public Vector3 RandomDirection()
        {
            float t = Random.Range(0.0f, Angle);
            Vector3 arcVector = Quaternion.AngleAxis(StartAngle, Vector3.up) * Vector3.forward;
            arcVector.Normalize();
            arcVector = Quaternion.AngleAxis(t, Vector3.up) * arcVector;

            // Select a random vertical velocity
            arcVector.y = Random.Range(0.3f, 1.0f);
            arcVector.Normalize();
            return arcVector;
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        public void OnValidate()
        {
            while (StartAngle < 0)
            {
                StartAngle = StartAngle + 360;
            }
            while (StartAngle > 360)
            {
                StartAngle = StartAngle - 360;
            }
            if (Angle < 0)
            {
                Angle = 0;
            }
            if (Angle > 360)
            {
                Angle = 360;
            }
        }

        public void Shake()
        {
            GetComponent<Animator>().SetTrigger("Shake");
        }

#if UNITY_EDITOR
        /*
        Draws the launch range arc in the Scene view.
        */
        public void OnDrawGizmosSelected()
        {
            Handles.color = new Color(1, 0, 1, 0.2f);

            Vector3 arcVectorMin = Quaternion.AngleAxis(StartAngle, Vector3.up) * Vector3.forward;

            Handles.DrawBezier(
                transform.position,
                transform.position + arcVectorMin * 3,
                transform.position,
                transform.position + arcVectorMin * 3,
                Color.magenta,
                null,
                7.0f
            );

            Vector3 arcVectorMax = Quaternion.AngleAxis(Angle, Vector3.up) * arcVectorMin;

            Handles.DrawBezier(
                transform.position,
                transform.position + arcVectorMax * 3,
                transform.position,
                transform.position + arcVectorMax * 3,
                Color.magenta,
                null,
                7.0f
            );

            Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                arcVectorMin,
                Angle,
                3f
            );
        }
#endif
    }
}
