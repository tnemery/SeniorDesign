using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserHistory : MonoBehaviour {
	private UserGUI myData;
	private string WhoAmI = "";
	private string curSteps;
	private string totalSteps;
	private bool runOnce = true;
	private string numSteps = "0";
	private string total = "";

	// Use this for initialization
	void Start () {
		myData = GameObject.Find ("MainDataObject").GetComponent<UserGUI> ();
	}

	// Update is called once per frame
	void Update () {
		if (runOnce) {
			WhoAmI = myData.getUser ();
			for (int i = 0; i < myData.UserData2.Length/2; i++) {
				if (WhoAmI == myData.UserData2 [i, 0]) {
					this.transform.GetChild (1).GetComponent<Text> ().text = "Current Steps: " + myData.UserData2 [i, 1]; 
				}
			}
			for (int j=0; j< myData.UserData3.Length/4; j++) {
				if (WhoAmI == myData.UserData3 [j, 0]) {
					this.transform.GetChild (2).GetComponent<Text> ().text = "Total Steps: " + myData.UserData3 [j, 3];
				}
			}
			runOnce = false;
		}
	}
}
