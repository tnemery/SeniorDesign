using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour {
//	private string masterString = "Super";
	//private string masterPassword = "asd123";
	//private bool AdminMode = false;
	//private bool UserMode = false;
	public GameObject User;
	public GameObject UserCanvus;
	public GameObject AdminCanvus;
	public GameObject LoginCanvus;
	public GameObject WarningBox;
	public string username = "";
	private string password = "";

	public void SetUser(string user){
		username = user;
		User.gameObject.GetComponent<UserGUI> ().SetUser (username);
	}

	public void SetPass(string pass){
		password = pass;
	}

	public void LoginCheck(){
		int count = User.GetComponent<UserGUI> ().UserData.Length/8; //divide result by 8 because there are 8 cells per row
		//print (count);
		for (int i = 0; i < count; i++) {
			if (username == User.GetComponent<UserGUI> ().UserData[i,4] && password == User.GetComponent<UserGUI> ().UserData[i,3]) {
				if(username == "Super"){
					CloseCanvusWithAdmin ();
					break;
				}else{
					CloseCanvusWithUser ();
					break;
				}
			}else{
				if(i == count-1){
					WarningBox.SetActive (true);
					WarningBox.transform.GetChild(2).gameObject.GetComponent<Text>().text = "The Username or password you entered is incorrect, please try again.";
					this.transform.GetChild(0).transform.GetChild (0).GetComponent<InputField>().text = ""; //text of the Username field
					this.transform.GetChild(0).transform.GetChild (1).GetComponent<InputField>().text = ""; //text of the Password field
				}
			}
		}
	}

	public void LogOut(){
		//GameObject.Find ("Login/registration").transform.GetComponent<Canvas> ().enabled = true;
		username = "";
     	password = "";
	}

	public void CloseCanvusWithAdmin(){
		LoginCanvus.SetActive(false);
		//AdminMode = true;
		//GameObject.Find ("AdminView").transform.GetComponent<Canvas> ().enabled = true;
		AdminCanvus.SetActive (true);
		//GameObject.Find ("UserData").GetComponent<UserGUI> ().SetUser (username);
		//GameObject.Find ("WelcomeText").GetComponent<Text> ().text = "Welcome, " + username;
	}

	public void CloseCanvusWithUser(){
		LoginCanvus.SetActive(false);
		//AdminMode = true;
		//GameObject.Find ("AdminView").transform.GetComponent<Canvas> ().enabled = true;
		UserCanvus.SetActive (true);
		//GameObject.Find ("UserData").GetComponent<UserGUI> ().SetUser (username);
		//GameObject.Find ("WelcomeText").GetComponent<Text> ().text = "Welcome, " + username;
	}

}