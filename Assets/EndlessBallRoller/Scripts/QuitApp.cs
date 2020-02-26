using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessBallRoller {

    public class QuitApp : MonoBehaviour {
		
		bool isInputDisabled;
		bool isCursor;

		void OnEnable () {
			isInputDisabled = false;
			isCursor = true;
		}
		
		void Update () {
			if(Input.GetKeyDown(KeyCode.Q) && !isInputDisabled){
				Application.Quit();
				Debug.Log(Application.streamingAssetsPath);
			}
			
			if(Input.GetKeyDown(KeyCode.Escape) && !isInputDisabled){
				isCursor = !isCursor;
				if(isCursor){
					OnCursorVisible();
				}else{
					OnCursorInvisible();
				}
				
			}
		}
		
		public void OnCursorVisible(){
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		public void OnCursorInvisible(){
			Cursor.visible = false;
			  Cursor.lockState = CursorLockMode.Locked;
		}
		
		void DisableInput(bool isInput){
			isInputDisabled = isInput;
		}
		
		
	}
}

