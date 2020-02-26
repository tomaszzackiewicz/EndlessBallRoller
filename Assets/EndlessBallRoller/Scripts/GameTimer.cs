using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessBallRoller {

    public class GameTimer : MonoBehaviour {

        public float timer;
        public bool timeStarted = false;
        public Text timeText;
        public GameObject bigAd;

        private string timePassed;
        int minutes = 0;
        int seconds = 0;

        public string TimePassed {
            get {
                return timePassed;
            }
        }

        void Start() {
            timeStarted = true;
            timeText.text = timePassed = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        void Update() {
            if (timeStarted == true) {
                timer += Time.deltaTime;
                minutes = Mathf.FloorToInt(timer / 60F);
                seconds = Mathf.FloorToInt(timer - minutes * 60);
                timePassed = string.Format("{0:00}:{1:00}", minutes, seconds);
                timeText.text = timePassed;
                /* if(seconds % 60 == 0){
                    bigAd.SetActive(true);
                    AdTimer adTimer = bigAd.transform.parent.GetComponentInChildren<AdTimer>();
                    adTimer.Initialize();
                } */
            }
        }
    }
}