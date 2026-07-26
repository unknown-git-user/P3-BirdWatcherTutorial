using UnityEngine;
using System.Collections;

namespace Francotronics.BWT.GUI
{

    public class VariableAudioClip : MonoBehaviour
    {

        public string clipName;
        [Range(0.1f, 5.0f)]
        public float minPitch = 1f;
        [Range(0.1f, 5.0f)]
        public float maxPitch = 1f;
        [Range(0f, 1f)]
        public float minVolume = 1f;
        [Range(0f, 1f)]
        public float maxVolume = 1f;
        public int weight = 1;
        [Range(0f, 10f)]
        public float startOffset = 0f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}