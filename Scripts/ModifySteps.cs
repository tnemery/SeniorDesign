using UnityEngine;
using System.Collections;

public class ModifySteps : MonoBehaviour {
	private UserGUI myData;
	private string WhoAmI = "";
	private string numSteps = "0";
	private string total = "";
	
	// Use this for initialization
	void Start () {
		myData = GameObject.Find ("MainDataObject").GetComponent<UserGUI> ();
	}
	
	// Update is called once per frame
	void Update () {
		WhoAmI = myData.getUser ();
	}

	public void GetDataReady(string n){
		numSteps = n;
		if (WhoAmI != "") {
			for (int i = 0; i < myData.UserData2.Length/2; i++) {
				if (WhoAmI == myData.UserData2 [i, 0]) {
					numSteps = (int.Parse(myData.UserData2 [i, 1])+int.Parse (numSteps)).ToString();
				}
			}
			for (int j=0; j< myData.UserData3.Length/4; j++) {
				if (WhoAmI == myData.UserData3 [j, 0]) {
					total = (int.Parse(myData.UserData3 [j, 3])+int.Parse (n)).ToString();
				}
			}
		}
	}
	
	public void SendData(){
		GameObject.Find ("MainDataObject").GetComponent<UnityDataConnector> ().SetUserName (WhoAmI);
		GameObject.Find ("MainDataObject").GetComponent<UnityDataConnector>().AddSteps(numSteps);
		GameObject.Find ("MainDataObject").GetComponent<UnityDataConnector>().MaxSteps(total);
		GameObject.Find ("MainDataObject").GetComponent<UnityDataConnector>().SaveStepsAndHistory();
	}

}
