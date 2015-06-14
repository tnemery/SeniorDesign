using System.Collections;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
 

public class UnityDataConnector : MonoBehaviour
{
	public string webServiceUrl = "";
	public string spreadsheetId = "";
	public string worksheetName = "";
	public string worksheetName2 = "";
	public string worksheetName3 = "";
	public string password = "";
	public float maxWaitTime = 10f;
	public GameObject dataDestinationObject;
	private string statisticsWorksheetName = "UserData";
	public bool debugMode;
	public string FirstName = "";
	public string LastName = "";
	public string UserPassword = "";
	public string Email = "";
	static private string UserName = "";
	public string Activities = "";
	public string Times = "";
	public string Days = "";
	private bool allowRegister = false;
	private bool BadUsername = false;
	private bool BadEmail = false;
	public GameObject WarningBox;
	public GameObject RegInfo;
	private GameObject User;
	private string StepAdd;
	private string SetMaxStep;
	private UserGUI myData;

	bool updating;
	string currentStatus;
	JsonData[] ssObjects;
	bool saveToGS; 

	Rect guiBoxRect;
	Rect guiButtonRect;
	Rect guiButtonRect2;
	Rect guiButtonRect3;
	
	void Start ()
	{
		updating = false;
		currentStatus = "Offline";
		saveToGS = true;
		Connect ();
		User = this.gameObject;
	}

	public void SetFirstName(string name){
		FirstName = name;
	}

	public void SetLastName(string name){
		LastName = name;
	}

	public void SetUserName(string name){
		UserName = name;
	}

	public void SetPassword(string name){
		UserPassword = name;
	}

	public void SetActivities(string name){
		Activities = name;
	}

	public void SetDays(string name){
		Days = name;
	}

	public void SetTimes(string name){
		Times = name;
	}

	public void NewPassword(string name){
		if (allowRegister) {
			UserPassword = name;
		}
	}


	public void AddSteps(string n){
		StepAdd = n;
	}

	public void MaxSteps(string g){
		SetMaxStep = g;
	}

	public void CheckPassword(string name){

		if (UserPassword == name) {
			allowRegister = true;
		} else {
			WarningBox.SetActive (true);
			WarningBox.transform.GetChild(2).gameObject.GetComponent<Text>().text = "The passwords you entered do not match.";
			RegInfo.transform.GetChild(1).GetComponent<InputField>().text = ""; //text of the Password field
			RegInfo.transform.GetChild(7).GetComponent<InputField>().text = ""; //text of the Password_reentry field
		}
	}

	public void CheckUserName (){
		int count = User.GetComponent<UserGUI> ().UserData.Length/8;
		for (int i =0; i< count; i++) {
			if(UserName == User.GetComponent<UserGUI>().UserData[i,4]){
				WarningBox.SetActive (true);
				WarningBox.transform.GetChild(2).gameObject.GetComponent<Text>().text = "The User Name you entered is already taken.";
				RegInfo.transform.GetChild(0).GetComponent<InputField>().text = ""; //text of the Password field
				BadUsername = true;
				break;
			}else{
				if(i == count-1){
					BadUsername = false; //
				}
			}
		}

	}

	public void SetEmail(string name){
		Email = name;
	}

	public void CheckEmail (){
		if ((Email.Length - 20) < 1) {
			BadEmail = true;
			WarningBox.SetActive (true);
			WarningBox.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Please use a valid Oregonstate Email Address.\n Examples: doej@oregonstate.edu, doej@onid.oregonstate.edu, doej@onid.orst.edu";
			RegInfo.transform.GetChild(8).GetComponent<InputField>().text = ""; //text of the Password field
			BadEmail = true;
		}else if (Email.Substring (Email.Length - 20, 20) == "onid.oregonstate.edu") {
			BadEmail = false;
		} else if (Email.Substring (Email.Length - 13, 13) == "onid.orst.edu") {
			BadEmail = false;
		} else if (Email.Substring (Email.Length - 15, 15) == "oregonstate.edu") {
			BadEmail = false;
		} else {
			WarningBox.SetActive (true);
			WarningBox.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Please use a valid Oregonstate Email Address.\n Examples: doej@oregonstate.edu, doej@onid.oregonstate.edu, doej@onid.orst.edu";
			RegInfo.transform.GetChild(8).GetComponent<InputField>().text = ""; //text of the Password field
			BadEmail = true;
		}
		
	}

	void Connect()
	{
		if (updating)
			return;
		
		updating = true;
		StartCoroutine(GetData(worksheetName));   
		StartCoroutine(GetData(worksheetName2));
		StartCoroutine(GetData(worksheetName3));
	}
	
	IEnumerator GetData(string WSNAME)
	{	
		string connectionString = webServiceUrl + "?ssid=" + spreadsheetId + "&sheet=" + WSNAME + "&pass=" + password + "&action=GetData";
		if (debugMode)
			Debug.Log("Connecting to webservice on " + connectionString);

		WWW www = new WWW(connectionString);
		
		float elapsedTime = 0.0f;
		currentStatus = "Stablishing Connection... ";
		
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				currentStatus = "Max wait time reached, connection aborted.";
				Debug.Log(currentStatus);
				updating = false;
				break;
			}
			
			yield return null;  
		}
	
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			currentStatus = "Connection error after" + elapsedTime.ToString() + "seconds: " + www.error;
			Debug.LogError(currentStatus);
			updating = false;
			yield break;
		}
	
		string response = www.text;
		Debug.Log(elapsedTime + " : " + response);
		currentStatus = "Connection stablished, parsing data...";

		if (response == "\"Incorrect Password.\"")
		{
			currentStatus = "Connection error: Incorrect Password.";
			Debug.LogError(currentStatus);
			updating = false;
			yield break;
		}

		try 
		{
			ssObjects = JsonMapper.ToObject<JsonData[]>(response);
		}
		catch
		{
			currentStatus = "Data error: could not parse retrieved data as json.";
			Debug.LogError(currentStatus);
			updating = false;
			yield break;
		}

		currentStatus = "Data Successfully Retrieved!";
		updating = false;
		
		// Finally use the retrieved data as you wish.
		dataDestinationObject.SendMessage("GetWS", WSNAME);
		dataDestinationObject.SendMessage("DoSomethingWithTheData", ssObjects);
	}




	public void SaveDataOnTheCloud()
	{
		CheckEmail ();
		CheckUserName ();
		if (saveToGS && allowRegister && !BadEmail && !BadUsername) {
			StartCoroutine (SendData ());
			StartCoroutine (SendData2 ());
			StartCoroutine (SendData3 ());
			GameObject.Find ("LoginCanvus").GetComponent<LoginScript>().CloseCanvusWithUser(); //user registered send data
		}
		StartCoroutine(GetData(worksheetName));   
		StartCoroutine(GetData(worksheetName2));
		StartCoroutine(GetData(worksheetName3));
	} 

	IEnumerator SendData()
	{
		print ("checking: "+UserPassword);
		if (!saveToGS)
			yield break;

		string connectionString = 	webServiceUrl +
									"?ssid=" + spreadsheetId +
									"&sheet=" + statisticsWorksheetName +
									"&pass=" + password +
									"&val1=" + FirstName +
									"&val2=" + LastName +
									"&val3=" + Email +
									"&val4=" + UserPassword +
									"&val5=" + UserName +
									"&val6=" + "null" +
									"&val7=" + "null" +
									"&val8=" + "null" +
									"&action=SetData";

		if (debugMode)
			Debug.Log("Connection String: " + connectionString);
		WWW www = new WWW(connectionString);
		float elapsedTime = 0.0f;

		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				print ("wait time too long");
				// Error handling here.
				break;
			}

			yield return null;  
		}
		
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			print ("something went wrong");
			// Error handling here.
			yield break;
		}
		
		string response = www.text;

		if (response.Contains("Incorrect Password"))
		{
			print ("wrong password");
			// Error handling here.
			yield break;
		}

		if (response.Contains("RCVD OK"))
		{
			print ("check reciever");
			// Data correctly sent!
			yield break;
		}
	}

	IEnumerator SendData2()
	{
		if (!saveToGS)
			yield break;
		
		string connectionString = 	webServiceUrl +
			"?ssid=" + spreadsheetId +
				"&sheet=" + "Steps" +
				"&pass=" + password +
				"&val1=" + UserName +
				"&val2=" + "0" +
				"&action=SetData1";
		
		if (debugMode)
			Debug.Log("Connection String: " + connectionString);
		WWW www = new WWW(connectionString);
		float elapsedTime = 0.0f;
		
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				print ("wait time too long");
				// Error handling here.
				break;
			}
			
			yield return null;  
		}
		
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			print ("something went wrong");
			// Error handling here.
			yield break;
		}
		
		string response = www.text;
		
		if (response.Contains("Incorrect Password"))
		{
			print ("wrong password");
			// Error handling here.
			yield break;
		}
		
		if (response.Contains("RCVD OK"))
		{
			print ("check reciever");
			// Data correctly sent!
			yield break;
		}
	}

	IEnumerator SendData3()
	{
		if (!saveToGS)
			yield break;
		
		string connectionString = 	webServiceUrl +
			"?ssid=" + spreadsheetId +
				"&sheet=" + "History" +
				"&pass=" + password +
				"&val1=" + UserName +
				"&val2=" + System.DateTime.Now.ToShortDateString () +
				"&val3=" + System.DateTime.Now.ToShortDateString () +
				"&val4=" + "0" +
				"&action=SetData2";
		
		if (debugMode)
			Debug.Log("Connection String: " + connectionString);
		WWW www = new WWW(connectionString);
		float elapsedTime = 0.0f;
		
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				print ("wait time too long");
				// Error handling here.
				break;
			}
			
			yield return null;  
		}
		
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			print ("something went wrong");
			// Error handling here.
			yield break;
		}
		
		string response = www.text;
		
		if (response.Contains("Incorrect Password"))
		{
			print ("wrong password");
			// Error handling here.
			yield break;
		}
		
		if (response.Contains("RCVD OK"))
		{
			print ("check reciever");
			// Data correctly sent!
			yield break;
		}
	}

	public void MergeDataOnTheCloud()
	{
		//CheckPassword ();
		//if (saveToGS && allowRegister && !BadEmail && !BadUsername) {
			StartCoroutine (MergeData ());
		StartCoroutine(GetData(worksheetName));   
		StartCoroutine(GetData(worksheetName2));
		StartCoroutine(GetData(worksheetName3));
		//	GameObject.Find ("LoginCanvus").GetComponent<LoginScript>().CloseCanvusWithUser(); //user registered send data
		//}
	}

	public void SaveStepsAndHistory()
	{
		print (UserName);
		//CheckPassword ();
		//if (saveToGS && allowRegister && !BadEmail && !BadUsername) {
		StartCoroutine (MergeData2 ());
		StartCoroutine (MergeData3 ());
		StartCoroutine(GetData(worksheetName));   
		StartCoroutine(GetData(worksheetName2));
		StartCoroutine(GetData(worksheetName3));
		//	GameObject.Find ("LoginCanvus").GetComponent<LoginScript>().CloseCanvusWithUser(); //user registered send data
		//}
	} 

	IEnumerator MergeData()
	{
		int count = 0;
		myData = GameObject.Find ("MainDataObject").GetComponent<UserGUI> ();
		//print ("checking: "+UserPassword);
		for (int i = 0; i < myData.UserData2.Length/2; i++) {
			if (UserName == myData.UserData2 [i, 4]) {
				count = i;
				break; 
			}
		}

		if (!saveToGS)
			yield break;

		if (password == "") {
			password = myData.UserData2 [count, 3];
		}
		if (FirstName == "") {
			FirstName = myData.UserData2 [count, 0];
		}
		if (LastName == "") {
			LastName = myData.UserData2 [count, 1];
		}
		if (Email == "") {
			Email = myData.UserData2 [count, 2];
		}

		string connectionString = 	webServiceUrl +
			"?ssid=" + spreadsheetId +
				"&sheet=" + statisticsWorksheetName +
				"&pass=" + password +
				"&val1=" + FirstName +
				"&val2=" + LastName +
				"&val3=" + Email +
				"&val4=" + UserPassword +
				"&val5=" + UserName +
				"&val6=" + Activities +
				"&val7=" + Times +
				"&val8=" + Days +
				"&action=MergeData";
		
		if (debugMode)
			Debug.Log("Connection String: " + connectionString);
		WWW www = new WWW(connectionString);
		float elapsedTime = 0.0f;
		
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				print ("wait time too long");
				// Error handling here.
				break;
			}
			
			yield return null;  
		}
		
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			print ("something went wrong");
			// Error handling here.
			yield break;
		}
		
		string response = www.text;
		
		if (response.Contains("Incorrect Password"))
		{
			print ("wrong password");
			// Error handling here.
			yield break;
		}
		
		if (response.Contains("MRG OK"))
		{
			print ("Data done");
			// Data correctly sent!
			yield break;
		}
	}


	IEnumerator MergeData2()
	{
		//print ("checking: "+UserPassword);
		if (!saveToGS)
			yield break;
		
		string connectionString = 	webServiceUrl +
			"?ssid=" + spreadsheetId +
				"&sheet=" + "Steps" +
				"&pass=" + password +
				"&val1=" + UserName +
				"&val2=" + StepAdd +
				"&action=MergeData2";
		
		if (debugMode)
			Debug.Log("Connection String: " + connectionString);
		WWW www = new WWW(connectionString);
		float elapsedTime = 0.0f;
		
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				print ("wait time too long");
				// Error handling here.
				break;
			}
			
			yield return null;  
		}
		
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			print ("something went wrong");
			// Error handling here.
			yield break;
		}
		
		string response = www.text;
		
		if (response.Contains("Incorrect Password"))
		{
			print ("wrong password");
			// Error handling here.
			yield break;
		}
		
		if (response.Contains("MRG OK"))
		{
			print ("Data done");
			// Data correctly sent!
			yield break;
		}
	}

	IEnumerator MergeData3()
	{
		//print ("checking: "+UserPassword);
		if (!saveToGS)
			yield break;
		
		string connectionString = 	webServiceUrl +
			"?ssid=" + spreadsheetId +
				"&sheet=" + "History" +
				"&pass=" + password +
				"&val1=" + UserName +
				"&val2=" + System.DateTime.Now.ToShortDateString () +
				"&val3=" + SetMaxStep +
				"&action=MergeData3";
		
		if (debugMode)
			Debug.Log("Connection String: " + connectionString);
		WWW www = new WWW(connectionString);
		float elapsedTime = 0.0f;
		
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= maxWaitTime)
			{
				print ("wait time too long");
				// Error handling here.
				break;
			}
			
			yield return null;  
		}
		
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			print ("something went wrong");
			// Error handling here.
			yield break;
		}
		
		string response = www.text;
		
		if (response.Contains("Incorrect Password"))
		{
			print ("wrong password");
			// Error handling here.
			yield break;
		}
		
		if (response.Contains("MRG OK"))
		{
			print ("Data done");
			// Data correctly sent!
			yield break;
		}
	}

}
	
	