using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Welcome : MonoBehaviour {
	private UserGUI data;
	private string UserName;

	void Awake(){
		data = GameObject.Find ("MainDataObject").GetComponent<UserGUI>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.GetChild (0).GetComponent<Text> ().text = "Welcome, " + data.getUser ();
	}
}
