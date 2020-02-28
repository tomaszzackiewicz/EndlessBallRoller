using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Random = UnityEngine.Random;

namespace EndlessBallRoller {

    public class AdTimer : MonoBehaviour {

        public GameObject addPanel;
        public GameObject removeAdDialog;
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI wordText;
        public TextMeshProUGUI removeAdMessageText;
        public GameObject skipButton;
        public GameObject removeAddButton;
        public List<string> characters;
        public int pass;
        public string password;

        private bool isBigAdDisabled = false;
        private bool isBigAdRemoved = false;
        private List<string> words = new List<string>();
        private int indexOfWords;
        private string word;
        private float timer;
        private bool timeStarted = false;
        private string timePassed;
        private int minutes;
        private int seconds;
        private string doNotRemoveFolder = "DoNotRemove";
        private string[] array = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        public string TimePassed {
            get {
                return timePassed;
            }
        }

        public bool TimeStarted {
            get {
                return timeStarted;
            }
            set {
                timeStarted = value;
            }
        }

        void Awake() {
            LoadFile();
        }

        void OnEnable() {
            GameController.ShowAddEvent += Initialize;
        }

        void Start() {
            characters = new List<string>(array);
            if (!isBigAdRemoved) {
                InitializePass();
            }

            removeAdDialog.SetActive(false);

            if (isBigAdDisabled && isBigAdRemoved) {
                skipButton.SetActive(true);
                removeAddButton.SetActive(false);
            }
        }

        public void Initialize(bool isShown) {
            if (!isBigAdRemoved) {
                if (!isBigAdDisabled) {
                    DoAdStuff();
                }
            }
        }

        void DoAdStuff() {
            if (!addPanel.activeSelf) {
                minutes = 0;
                seconds = 0;
                timer = 0;
                addPanel.SetActive(true);
                skipButton.SetActive(false);
                removeAddButton.SetActive(true);
                timeStarted = true;
                timeText.text = timePassed = string.Format("{0:00}:{1:00}", minutes, seconds);
            } else {
                return;
            }
        }

        void InitializePass() {
            int counter = 0;
            while (counter < 5) {
                StringBuilder builder = new StringBuilder();
                indexOfWords = Random.Range(0, characters.Count);
                builder.Append(GetWord(indexOfWords));
                word += builder;
                counter++;
            }

            for (int i = 0; i < word.Length; i++) {
                pass += (int)word[i];
                pass *= word.Length;
            }
            wordText.text = word;
        }

        void Update() {
            if (timeStarted) {
                timer += Time.deltaTime;
                minutes = Mathf.FloorToInt(timer / 60F);
                seconds = Mathf.FloorToInt(timer - minutes * 60);
                timeText.text = timePassed = string.Format("{0:00}:{1:00}", minutes, seconds);

                if (seconds >= 5) {
                    timeStarted = false;
                    removeAddButton.SetActive(false);
                    skipButton.SetActive(true);
                }
            }

        }

        public void SkipAdButton() {
            addPanel.SetActive(false);
            removeAdDialog.SetActive(false);
            timeText.text = timePassed = string.Format("{0:00}:{1:00}", 0, 0);

        }

        public void RemoveAdButton() {
            removeAdDialog.SetActive(true);
        }

        public void RemoveAdIF(string code) {
            removeAdMessageText.text = "";
            if (code.Length <= 10) {
                password = code;
            } else {
                removeAdMessageText.text = "Code too long. Please try again.";
            }
        }

        public void RemoveAdIFButton() {
            removeAdMessageText.text = "";
            if (password != "") {
                int removeCode = int.Parse(password);
                if (pass == removeCode) {
                    isBigAdRemoved = true;
                    addPanel.SetActive(false);
                    SaveFile();
                } else {
                    removeAdMessageText.text = "Invalid Code. Please try again.";
                }
            } else {
                removeAdDialog.SetActive(false);
            }
        }

        string GetWord(int index) {

            for (int i = 0; i < characters.Count; i++) {
                if (i == index) {
                    return characters[i];
                }
            }
            return null;
        }

        public void SaveFile() {
            string directPath = Application.persistentDataPath + "/" + doNotRemoveFolder;
            if (!Directory.Exists(directPath)) {
                Directory.CreateDirectory(directPath);
            }
            string destination = Path.Combine(directPath, "Save.bin");

            FileStream file;

            if (File.Exists(destination)) {
                file = File.OpenWrite(destination);
            } else {
                file = File.Create(destination);
            }

            GameData data = new GameData(isBigAdRemoved);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }

        public void LoadFile() {
            string directPath = Application.persistentDataPath + "/" + doNotRemoveFolder;
            if (!Directory.Exists(directPath)) {
                Directory.CreateDirectory(directPath);
            }
            string destination = Path.Combine(directPath, "Save.bin");

            FileStream file;

            if (File.Exists(destination)) {
                file = File.OpenRead(destination);
            } else {
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            isBigAdRemoved = data.isAdRemoved;
            isBigAdDisabled = data.isAdRemoved;
            Debug.Log("is removed: " + isBigAdRemoved);
        }

        void OnDisable() {
            GameController.ShowAddEvent -= Initialize;
        }


    }

    [System.Serializable]
    public class GameData {
        public bool isAdRemoved;

        public GameData(bool isAdRem) {
            isAdRemoved = isAdRem;

        }
    }
}