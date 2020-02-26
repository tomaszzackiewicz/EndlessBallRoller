using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

namespace EndlessBallRoller {

    public class PasswordGenerator : MonoBehaviour {

        public TMP_InputField wordIF;
        public TMP_InputField passIF;
        string passwordFolder = "PasswordFolder";

        public void GeneratePaswordButton() {
            int pass = 0;
            passIF.text = "";
            string word = "";

            string temp = wordIF.text;
            word = temp;
            word = word.ToUpper();

            Debug.Log(word);
            for (int i = 0; i < word.Length; i++) {
                pass += (int)word[i];
                pass *= word.Length;
            }
            string password = pass.ToString();
            passIF.text = password;
            SaveToFile(password, word);
        }

        void SaveToFile(string password, string word) {

            string directPath = Application.persistentDataPath + "/" + passwordFolder;
            if (!Directory.Exists(directPath)) {
                Directory.CreateDirectory(directPath);
            }
            string destination = Path.Combine(directPath, word + ".txt");
            StreamWriter writer = new StreamWriter(destination, true);
            writer.WriteLine(password);
            writer.Close();
        }
    }
}
