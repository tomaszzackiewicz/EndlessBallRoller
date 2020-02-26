using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

namespace EndlessBallRoller {

    public class GameHistory : MonoBehaviour {

        public static List<GameSessions> gameSessions = new List<GameSessions>();
        public GameObject itemPrefab;
        public GameObject itemParent;
        public GameObject gameHistoryPanel;
        private string gameHistoryFolder = "GameHistory";
        public static GameHistory instance;
        public bool isPersistant = true;

        public virtual void Awake() {
            if (!instance) {
                instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        void Start() {
            gameHistoryPanel.SetActive(false);
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.L)) {

            }
        }

        public void LoadFile() {
            string directPath = Application.persistentDataPath + "/" + gameHistoryFolder;
            if (!Directory.Exists(directPath)) {
                Directory.CreateDirectory(directPath);
            }
            string destination = Path.Combine(directPath, "GameHistory.bin");
            FileStream file;

            if (File.Exists(destination)) {
                file = File.OpenRead(destination);
            } else {
                Debug.LogError("File not found");
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            gameSessions.Clear();
            ClearItems();
            gameSessions = (List<GameSessions>)bf.Deserialize(file);
            file.Close();

            //score = data.score;
            //ball = data.ball;
            //time = data.time;
            //StartCoroutine(FillCor());
        }

        public void FillItems() {
            ClearItems();
            foreach (GameSessions gs in gameSessions) {
                GameObject item = Instantiate(itemPrefab) as GameObject;
                item.transform.SetParent(itemParent.transform, false);
                TextMeshProUGUI dateText = item.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                dateText.text = gs.date;
                TextMeshProUGUI ballText = item.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                ballText.text = gs.ball;
                TextMeshProUGUI scoreText = item.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                scoreText.text = gs.score.ToString();
                TextMeshProUGUI timeText = item.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
                timeText.text = gs.time;
            }
        }

        public void ClearHistoryButton() {
            ClearItems();
            gameSessions.Clear();
        }

        void ClearItems() {
            foreach (Transform child in itemParent.transform) {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void CancelHistoryButton() {
            gameHistoryPanel.SetActive(false);
        }


    }

    [System.Serializable]
    public class GameSessions {
        public string date;
        public string ball;
        public int score;
        public string time;

        public GameSessions(string date, string ball, int score, string time) {
            this.date = date;
            this.ball = ball;
            this.score = score;
            this.time = time;
        }
    }
}