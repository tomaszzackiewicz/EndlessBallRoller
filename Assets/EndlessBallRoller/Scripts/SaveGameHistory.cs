using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EndlessBallRoller {

    public class SaveGameHistory : MonoBehaviour {

        public static SaveGameHistory instance;
        public bool isPersistant = true;

        private string gameHistoryFolder = "GameHistory";

        public virtual void Awake() {
            if (!instance) {
                instance = this;
            } else {
                GameObject.Destroy(gameObject);
            }
        }

        public void SaveFile(string dateValue, string ballTypeValue, int scoreValue, string timeValue) {
            string directPath = Application.persistentDataPath + "/" + gameHistoryFolder;
            if (!Directory.Exists(directPath)) {
                Directory.CreateDirectory(directPath);
            }
            string destination = Path.Combine(directPath, "GameHistory.bin");

            FileStream file;

            if (File.Exists(destination)) {
                file = File.OpenWrite(destination);
            } else {
                file = File.Create(destination);
            }

            GameSessions data = new GameSessions(dateValue, ballTypeValue, scoreValue, timeValue);
            GameHistory.gameSessions.Add(data);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, GameHistory.gameSessions);
            file.Close();
        }


    }
}