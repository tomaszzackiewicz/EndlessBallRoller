using System;
using UnityEngine;

#if UNITY_ADS

using UnityEngine.Advertisements;

#endif

namespace EndlessBallRoller {

    public class UnityAdController : MonoBehaviour {
        public static bool showAds = true;
        public static DateTime? nextRewardTime = null;


        public static void ShowAd() {

#if UNITY_ADS

        ShowOptions options = new ShowOptions();
        options.resultCallback = Unpause;

        if (Advertisement.IsReady()){
            Advertisement.Show(options);
        }
		
        PauseScreenBehaviour.paused = true;
        Time.timeScale = 0f;

#endif

        }

#if UNITY_ADS

    public static void Unpause(ShowResult result){
        PauseScreenBehaviour.paused = false;
        Time.timeScale = 1f;
    }

#endif


        public static void ShowRewardAd() {
#if UNITY_ADS

        nextRewardTime = DateTime.Now.AddSeconds(15);

        if (Advertisement.IsReady()){
            PauseScreenBehaviour.paused = true;
            Time.timeScale = 0f;

            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(options);
        }

#endif
        }

        public static ObstacleBehaviour obstacle;
#if UNITY_ADS
    private static void HandleShowResult(ShowResult result){

        switch (result){
            case ShowResult.Finished:
                obstacle.Continue();
                break;
            case ShowResult.Skipped:
                Debug.Log("Ad skipped, do nothing");
                break;
            case ShowResult.Failed:
                Debug.LogError("Ad failed to show, do nothing");
                break;
        }

        PauseScreenBehaviour.paused = false;
        Time.timeScale = 1f;

    }
#endif
    }
}