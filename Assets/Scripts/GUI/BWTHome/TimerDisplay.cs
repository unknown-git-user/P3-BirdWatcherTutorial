using UnityEngine;
using UnityEngine.UI;
using Multimorphic.P3App.GUI;

namespace Francotronics.BWT.GUI
{
    public class TimerDisplay : P3Aware {

        public string TimerName;

        public override void Start ()
        {
            base.Start ();
        }

        protected override void CreateEventHandlers()
        {
            base.CreateEventHandlers ();
            AddModeEventHandler("Evt_" + TimerName + "TimerTick", UpdateTimeEventHandler);
            AddModeEventHandler("Evt_" + TimerName + "TimerStart", UpdateTimeEventHandler);
        }

        public void UpdateTimeEventHandler(string eventName, object eventData)
        {
            GetComponent<Text>().text = ((int)eventData).ToString();
        }

        // Update is called once per frame
        public override void Update ()
        {
            base.Update ();
        }
    }
}
