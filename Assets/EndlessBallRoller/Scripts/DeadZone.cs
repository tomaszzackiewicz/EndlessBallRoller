using UnityEngine;
using System;

namespace EndlessBallRoller {

    public class DeadZone : MonoBehaviour {

        public static event Action<float, bool> RestartGameEvent;
        public float waitTime = 2.0f;

        void OnTriggerEnter(Collider col) {
            if (col.gameObject.CompareTag("Player")) {
                OnRestartGameEvent();
            }
        }

        protected virtual void OnRestartGameEvent() {
            if (RestartGameEvent != null) {
                RestartGameEvent(waitTime, true);
            }
        }
    }
}
