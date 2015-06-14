using UnityEngine;
using System.Collections;
using LitJson;

public class UserGUI : MonoBehaviour {
	private string user;
	public static string WORKSHEET = "";
	public string[,] UserData;
	public string[,] UserData2;
	public string[,] UserData3;


	public void GetWS(string WS){
		WORKSHEET = WS;
	}

	public void DoSomethingWithTheData(JsonData[] ssObjects)
	{
		if (WORKSHEET == "UserData") {
			UserData = new string[ssObjects.Length,8];
			for (int i = 0; i < ssObjects.Length; i++) {	
				if (ssObjects [i].Keys.Contains ("First Name"))
					UserData[i,0] = ssObjects [i] ["First Name"].ToString ();
			
				if (ssObjects [i].Keys.Contains ("Last Name"))
					UserData[i,1] = ssObjects [i] ["Last Name"].ToString ();
			
				if (ssObjects [i].Keys.Contains ("Email"))
					UserData[i,2] = ssObjects [i] ["Email"].ToString ();
			
				if (ssObjects [i].Keys.Contains ("Password"))
					UserData[i,3] = ssObjects [i] ["Password"].ToString ();

				if (ssObjects [i].Keys.Contains ("User Name"))
					UserData[i,4] = ssObjects [i] ["User Name"].ToString ();

				if (ssObjects [i].Keys.Contains ("Activities"))
					UserData[i,5] = ssObjects [i] ["Activities"].ToString ();

				if (ssObjects [i].Keys.Contains ("Times Available"))
					UserData[i,6] = ssObjects [i] ["Times Available"].ToString ();

				if (ssObjects [i].Keys.Contains ("Days Available"))
					UserData[i,7] = ssObjects [i] ["Days Available"].ToString ();
			
			}
		}
		if(WORKSHEET == "Steps"){
			UserData2 = new string[ssObjects.Length,2];
			for (int i = 0; i < ssObjects.Length; i++) {
				if (ssObjects [i].Keys.Contains ("User Name"))
					UserData2[i,0] = ssObjects [i] ["User Name"].ToString ();
				if (ssObjects [i].Keys.Contains ("Step Count"))
					UserData2[i,1] = ssObjects [i] ["Step Count"].ToString ();
			}
			
		}
		if(WORKSHEET == "History"){
			UserData3 = new string[ssObjects.Length,4];
			for (int i = 0; i < ssObjects.Length; i++) {
				if (ssObjects [i].Keys.Contains ("User Name"))
					UserData3[i,0] = ssObjects [i] ["User Name"].ToString ();
				if (ssObjects [i].Keys.Contains ("Start Date"))
					UserData3[i,1] = ssObjects [i] ["Start Date"].ToString ();
				if (ssObjects [i].Keys.Contains ("Last Date"))
					UserData3[i,2] = ssObjects [i] ["Last Date"].ToString ();
				if (ssObjects [i].Keys.Contains ("All Steps"))
					UserData3[i,3] = ssObjects [i] ["All Steps"].ToString ();
			}
		}

	}


	public void SetUser(string curUser){
		user = curUser;
	}

	public void voidUser(){
		user = "";
	}

	public string getUser(){
		return user;
	}
}