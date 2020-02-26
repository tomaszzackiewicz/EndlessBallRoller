using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessBallRoller {

	public class MainAdTimer : MonoBehaviour {
		
		private float timer = 5.0f;
		private bool timeStarted = false;
		private string timePassed;
		
		void Update () {
			if (timeStarted == true){
				timer -= Time.deltaTime;
				int minutes = Mathf.FloorToInt(timer / 60F);
				int seconds = Mathf.FloorToInt(timer - minutes * 60);
				
				if(seconds <= 0){
					timeStarted = false;
					gameObject.SetActive(false);
				}
			}
		}
	}
}
