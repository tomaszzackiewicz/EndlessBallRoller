using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EndlessBallRoller {

    public class MainMenuBehaviour : MonoBehaviour {

        [Header("Object References")]
        public GameObject mainMenu;
        public GameObject facebookLogin;
        public Image profilePic;
        public Text greeting;
        public string levelName;
        public GameObject gameHistoryPanel;
        public GameObject infoPanel;

        public void LoadLevelButton() {
            SceneManager.LoadScene(levelName);

#if UNITY_ADS
        if (UnityAdController.showAds){
            UnityAdController.ShowAd();
        }
#endif
        }

        public void DisableAds() {

            PlayerPrefs.SetInt("Show Ads", 0);
        }

        virtual protected void Start() {

            if (facebookLogin != null) {
                facebookLogin.SetActive(true);
                mainMenu.SetActive(false);
            }

            Time.timeScale = 1;
        }

        #region Facebook 

        public void Awake() {

            //if (!FB.IsInitialized){
            //    FB.Init(OnInitComplete, OnHideUnity);
            //}

            //if(facebookLogin != null){

            //}

            //if(mainMenu != null){

            //}

        }

        void OnInitComplete() {
            //if (FB.IsLoggedIn){
            //    print("Logged into Facebook");

            // Close Login and open Main Menu 
            //PlayAsGuestButton();
            // }
        }

        void OnHideUnity(bool active) {

            Time.timeScale = (active) ? 1 : 0;
        }

        //public void FacebookLoginButton(){
        //    List<string> permissions = new List<string>();

        //    // Add permissions we want to have here 
        //    permissions.Add("public_profile");

        //    FB.LogInWithReadPermissions(permissions, FacebookCallback);
        //}

        //void FacebookCallback(IResult result){
        //    if (result.Error == null){
        //        OnInitComplete();
        //    }else{
        //        print(result.Error);
        //    }
        //}

        public void PlayAsGuestButton() {
            if (facebookLogin != null && mainMenu != null) {
                facebookLogin.SetActive(false);
                mainMenu.SetActive(true);
                GameHistory.instance.LoadFile();

                //if (FB.IsLoggedIn){
                //    FB.API("/me?fields=name", HttpMethod.GET, SetName);
                //    FB.API("/me/picture?width=256&height=256", HttpMethod.GET, SetProfilePic);
                //}
            }
        }

        //void SetName(IResult result){
        //    if (result.Error != null){
        //        print(result.Error);
        //        return;
        //    }

        //    string playerName = result.ResultDictionary["name"].ToString();
        //    greeting.text = "Hello, " + playerName + "!";

        //    greeting.gameObject.SetActive(true);
        //}


        //void SetProfilePic(IGraphResult result){
        //    if (result.Error != null){
        //        print(result.Error);
        //        return;
        //    }

        //    Sprite fbImage = Sprite.Create(result.Texture, new Rect(0, 0, 256, 256), Vector2.zero);
        //    profilePic.sprite = fbImage;

        //    profilePic.gameObject.SetActive(true);
        //}

        #endregion



        void OnHidden(object obj) {
            GameObject go = obj as GameObject;

            if (go != null) {
                go.SetActive(false);
            }
        }

        public void OpenGameHistoryPanelButton() {
            gameHistoryPanel.SetActive(true);
            GameHistory.instance.FillItems();

        }

        public void OpenInfoPanelButton() {
            infoPanel.SetActive(true);
        }

        public void CancelInfoPanelButton() {
            infoPanel.SetActive(false);
        }

    }
}