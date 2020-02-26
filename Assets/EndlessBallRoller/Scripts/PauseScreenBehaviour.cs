using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace EndlessBallRoller {

    public class PauseScreenBehaviour : MonoBehaviour {

        public static event Action<bool> DisableAdEvent;
        public static bool paused;
        public GameObject pauseMenu;
        public string levelName;
        private const string tweetTextAddress = "http://twitter.com/intent/tweet?text=";
        private string appStoreLink = "https://tomaszzackiewicz.wordpress.com/";
        public PlayerBehaviour playerBehaviour;
        public GameTimer gameTimer;
        public static string ballType;
        public static int score;
        public static string time;

        public void RestartButton() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            OnDisableAdEvent();
        }

        public void SetPauseMenuButton(bool isPaused) {
            paused = isPaused;
            OnDisableAdEvent();
            Time.timeScale = (paused) ? 0 : 1;

            if (paused) {
                pauseMenu.SetActive(true);
            } else {
                pauseMenu.SetActive(false);
            }
        }

        public void LoadLevelButton() {
            SceneManager.LoadScene(levelName);

#if UNITY_ADS

        if (UnityAdController.showAds){
            UnityAdController.ShowAd();
        }

#endif
        }

        void Start() {

            paused = false;

#if !UNITY_ADS
            SetPauseMenuButton(false);
#else
 
        if (!UnityAdController.showAds){ 
            SetPauseMenu(false); 
        } 
 
#endif
        }

        #region Share Score via Twitter 

        public void TweetScore() {

            string tweet = "I've got " + string.Format("{0:0}", score)
        + " points with " + ballType + " in " + time + " in Endless Ball Roller! Who is better?";

            Application.OpenURL(tweetTextAddress + WWW.EscapeURL(tweet + "\n" + appStoreLink));
        }

        #endregion

        protected void OnDisableAdEvent() {
            if (DisableAdEvent != null) {
                DisableAdEvent(true);
            }
        }

    }
}