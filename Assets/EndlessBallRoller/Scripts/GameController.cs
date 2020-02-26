using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;
using Random=UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace EndlessBallRoller {

    public class GameController : MonoBehaviour {

        public static event Action<bool> DisableAdEvent;
        public static event Action<bool> ShowAddEvent;
        public GameObject player;
        public GameObject tile;
        public Transform flowerPot;
        public Transform coin;
        public Transform shield;
        public Transform trashBin;
        public GameObject timerUI;
        public GameObject mainUI;
        public Vector3 startPoint = new Vector3(0, 0, -5);
        [Range(1, 100)]
        public int initSpawnNum = 10;
        public int initNoflowerPots = 4;
        public GameObject explosion;
        public List<GameObject> flowerPotSpawnPoints = new List<GameObject>();
        public List<GameObject> trashBinSpawnPoints = new List<GameObject>();
        public List<GameObject> coinSpawnPoints = new List<GameObject>();
        public List<GameObject> shieldSpawnPoints = new List<GameObject>();
        public Text endBallText;
        public Text endScoreText;
        public Text endTimeText;
        public GameObject tileParent;
        private Vector3 nextTileLocation;
        private Quaternion nextTileRotation;
        private GameTimer gameTimer;
        private int tileNumber = 0;

        void Start() {
            nextTileLocation = startPoint;
            nextTileRotation = Quaternion.identity;

            for (int i = 0; i < initSpawnNum; ++i) {
                SpawnNextTile(i >= initNoflowerPots);
            }
            PlayerBehaviour.RestartGameEvent += ResetGame;
            DeadZone.RestartGameEvent += ResetGame;
            gameTimer = gameObject.GetComponent<GameTimer>();
            OnShowAddEvent(true);
        }

        protected virtual void OnShowAddEvent(bool isShown) {
            if (ShowAddEvent != null) {
                ShowAddEvent(isShown);
            }
        }

        public void SpawnNextTile(bool spawnflowerPots = true) {
            GameObject newTile = Instantiate(tile, nextTileLocation, nextTileRotation) as GameObject;
            TileController tileController = newTile.GetComponent<TileController>();
            tileController.TileNumber = tileNumber++;
            if (tileNumber % 10 == 0) {
                tileController.SetTileVolume();
            } else {
                tileController.UnSetTileVolume();
            }
            newTile.name = tile.name + "_" + tileNumber;
            var nextTile = newTile.transform.Find("Next Spawn Point");
            newTile.transform.SetParent(tileParent.transform, true);

            nextTileLocation = nextTile.position;
            nextTileRotation = nextTile.rotation;

            if (!spawnflowerPots) {
                return;
            }


            flowerPotSpawnPoints.RemoveAll(item => item == null);
            coinSpawnPoints.RemoveAll(item => item == null);
            shieldSpawnPoints.RemoveAll(item => item == null);
            trashBinSpawnPoints.RemoveAll(item => item == null);

            foreach (Transform child in tileController.flowerPotParent.transform) {
                if (child.CompareTag("FlowerPotSpawn")) {
                    flowerPotSpawnPoints.Add(child.gameObject);
                }
            }

            foreach (Transform child in tileController.coinParent.transform) {
                if (child.CompareTag("CoinSpawn")) {
                    coinSpawnPoints.Add(child.gameObject);
                }
            }

            foreach (Transform child in tileController.shieldParent.transform) {
                if (child.CompareTag("ShieldSpawn")) {
                    shieldSpawnPoints.Add(child.gameObject);
                }
            }

            foreach (Transform child in tileController.trashBinParent.transform) {
                if (child.CompareTag("TrashBinSpawn")) {
                    trashBinSpawnPoints.Add(child.gameObject);
                }
            }

            if (flowerPotSpawnPoints.Count > 0) {
                GameObject spawnPoint = flowerPotSpawnPoints[Random.Range(0, flowerPotSpawnPoints.Count)];
                while (spawnPoint.transform.childCount > 0) {
                    spawnPoint = flowerPotSpawnPoints[Random.Range(0, flowerPotSpawnPoints.Count)];
                }
                var spawnPos = spawnPoint.transform.position;
                var newflowerPot = Instantiate(flowerPot, spawnPos, Quaternion.identity);
                newflowerPot.localScale = new Vector3(1, 1, 1);
                newflowerPot.SetParent(spawnPoint.transform);
            }

            if (coinSpawnPoints.Count > 0) {
                GameObject spawnPointCoin = coinSpawnPoints[Random.Range(0, coinSpawnPoints.Count)];
                while (spawnPointCoin.transform.childCount > 0) {
                    spawnPointCoin = coinSpawnPoints[Random.Range(0, coinSpawnPoints.Count)];
                }
                var spawnPosCoin = spawnPointCoin.transform.position;
                var newCoint = Instantiate(coin, spawnPosCoin, Quaternion.identity);
                newCoint.SetParent(spawnPointCoin.transform);
            }

            if (shieldSpawnPoints.Count > 0) {
                GameObject spawnPointShield = shieldSpawnPoints[Random.Range(0, shieldSpawnPoints.Count)];
                while (spawnPointShield.transform.childCount > 0) {
                    spawnPointShield = shieldSpawnPoints[Random.Range(0, shieldSpawnPoints.Count)];
                }
                var spawnPosShield = spawnPointShield.transform.position;
                var newShield = Instantiate(shield, spawnPosShield, Quaternion.identity);
                newShield.SetParent(spawnPointShield.transform);
            }

            if (trashBinSpawnPoints.Count > 0) {
                GameObject spawnPointTrashBin = trashBinSpawnPoints[Random.Range(0, trashBinSpawnPoints.Count)];
                while (spawnPointTrashBin.transform.childCount > 0) {
                    spawnPointTrashBin = trashBinSpawnPoints[Random.Range(0, trashBinSpawnPoints.Count)];
                }
                var spawnPosTrashBin = spawnPointTrashBin.transform.position;
                var newShield = Instantiate(trashBin, spawnPosTrashBin, Quaternion.identity);
                newShield.SetParent(spawnPointTrashBin.transform);
            }
        }

        public void Continue() {
            var go = GetGameOverMenu();
            go.SetActive(false);
            player.SetActive(true);
            PlayerTouch();
        }

        void EndGame() {

            timerUI.SetActive(false);
            mainUI.SetActive(false);
            PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
            string dateValue = System.DateTime.Now.ToString("MM/dd/yy");
            int scoreValue = playerBehaviour.Score;
            string ballTypeValue = playerBehaviour.BallType;
            string timeValue = gameTimer.TimePassed;
            PauseScreenBehaviour.score = scoreValue;
            PauseScreenBehaviour.time = timeValue;
            PauseScreenBehaviour.ballType = ballTypeValue;

            endScoreText.text = scoreValue.ToString();
            endBallText.text = ballTypeValue;
            endTimeText.text = timeValue;

            SaveGameHistory.instance.SaveFile(dateValue, ballTypeValue, scoreValue, timeValue);
        }

        void ResetGame(float waitTime, bool isReset) {
            EndGame();
            if (isReset) {
                StartCoroutine(ResetGameCor(waitTime));
            } else {
                StartCoroutine(ResetGameCor(waitTime));
                StartCoroutine(WinGame(waitTime));
            }
        }

        IEnumerator WinGame(float waitTime) {
            yield return new WaitForSeconds(waitTime);
            GameObject go = GameObject.Find("GameOverText");
            Text txt = go.GetComponent<Text>();
            txt.text = "Winner!!!";
        }

        IEnumerator ResetGameCor(float waitTime) {
            yield return new WaitForSeconds(waitTime);
            var go = GetGameOverMenu();
            go.SetActive(true);
            OnDisableAdEvent();
            var buttons = go.transform.GetComponentsInChildren<Button>();
            UnityEngine.UI.Button continueButton = null;

            foreach (var button in buttons) {
                if (button.gameObject.name == "Continue Button") {
                    continueButton = button;
                    break;
                }
            }
            if (continueButton) {
#if UNITY_ADS
            StartCoroutine(ShowContinue(continueButton));
#else
                continueButton.gameObject.SetActive(false);
#endif
            }
        }

        GameObject GetGameOverMenu() {
            return GameObject.Find("Canvas").transform.Find("Game Over").gameObject;
        }

        public IEnumerator ShowContinue(UnityEngine.UI.Button contButton) {

            while (true) {
                //var btnText = contButton.GetComponentInChildren<Text>();

                // Check if we haven't reached the next reward time yet  
                // (if one exists) 
                /* if (UnityAdController.nextRewardTime.HasValue &&
                    (DateTime.Now <
                        UnityAdController.nextRewardTime.Value)){
                    // Unable to click on the button 
                    contButton.interactable = false;

                    // Get the time remaining until we get to the next  
                    // reward time 
                    TimeSpan remaining = UnityAdController.nextRewardTime.Value
                                            - DateTime.Now;

                    // Get the time left in the following format 99:99 
                    var countdownText = string.Format("{0:D2}:{1:D2}",
                                        remaining.Minutes,
                                        remaining.Seconds);

                    // Set our button's text to reflect the new time 
                    btnText.text = countdownText;

                    // Come back after 1 second and check again 
                    yield return new WaitForSeconds(1f);
                }
                else if (!UnityAdController.showAds)
                {
                    // It's valid to click the button now
                    contButton.interactable = true;

                    // If player clicks on button we want to just continue
                    contButton.onClick.AddListener(Continue);

                    UnityAdController.flowerPot = this;

                    // Change text to allow continue
                    btnText.text = "Free Continue";

                    // We can now leave the coroutine
                    break;
                } 
                else
                {
                    // It's valid to click the button now 
                    contButton.interactable = true;

                    // If player clicks on button we want to play ad and  
                    // then continue 
                    contButton.onClick.AddListener(UnityAdController.ShowRewardAd);
                    UnityAdController.flowerPot = this;

                    // Change text to its original version 
                    btnText.text = "Continue (Play Ad)";

                    // We can now leave the coroutine 
                    break;
                } */
            }

        }

        void PlayerTouch() {
            if (explosion != null) {
                var particles = Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(particles, 1.0f);
            }

            Destroy(this.gameObject);
        }

        protected void OnDisableAdEvent() {
            if (DisableAdEvent != null) {
                DisableAdEvent(true);
            }
        }

        void OnDisable() {
            PlayerBehaviour.RestartGameEvent -= ResetGame;
            DeadZone.RestartGameEvent -= ResetGame;
        }
    }
}
