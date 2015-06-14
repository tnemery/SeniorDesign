using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System.IO;

public class BuddySearch : MonoBehaviour {
	public GameObject SearchList;
	private UserGUI myData;
	private string searchItem;
	private string[] activities;
	private string[] times;
	private string[] days;
	private bool found = false;

	void Start(){
		myData = GameObject.Find ("MainDataObject").GetComponent<UserGUI> ();
	}

	public void GetSearchString(string n){
		searchItem = n;
	}

	public void Search() {
		int count = 0;
		for (int i = 0; i< myData.UserData.Length/8; i++) {
			activities = myData.UserData[i,5].Split(',');
			times = myData.UserData[i,6].Split(',');
			days = myData.UserData[i,7].Split(',');
			print (searchItem == myData.UserData[i,0]);
			if((searchItem == myData.UserData[i,0] ||
			   searchItem == myData.UserData[i,1] ||
			   searchItem == myData.UserData[i,4]) && !found){
				SearchList.transform.GetChild(count).GetComponent<Text>().text = myData.UserData[i,0]+"  "
																				+myData.UserData[i,1]+",     "
																				+myData.UserData[i,4]+",     "
																				+myData.UserData[i,2]+",     "
																				+myData.UserData[i,5]+",     "
																				+myData.UserData[i,6]+",     "
																				+myData.UserData[i,7];
				count++;
				found = true;
			}
			for(int j = 0; j < activities.Length;j++){
				if(searchItem == activities[j] && !found){
					SearchList.transform.GetChild(count).GetComponent<Text>().text = myData.UserData[i,0]+"  "
						+myData.UserData[i,1]+",     "
							+myData.UserData[i,4]+",     "
							+myData.UserData[i,2]+",     "
							+myData.UserData[i,5]+",     "
							+myData.UserData[i,6]+",     "
							+myData.UserData[i,7];
					count++;
					found = true;
				}
			}
			for(int k = 0; k < times.Length;k++){
				if(searchItem == times[k] && !found){
					SearchList.transform.GetChild(count).GetComponent<Text>().text = myData.UserData[i,0]+"  "
						+myData.UserData[i,1]+",     "
							+myData.UserData[i,4]+",     "
							+myData.UserData[i,2]+",     "
							+myData.UserData[i,5]+",     "
							+myData.UserData[i,6]+",     "
							+myData.UserData[i,7];
					count++;
					found = true;
				}
			}
			for(int h = 0; h < days.Length;h++){
				if(searchItem == days[h] && !found){
					SearchList.transform.GetChild(count).GetComponent<Text>().text = myData.UserData[i,0]+"  "
						+myData.UserData[i,1]+",     "
							+myData.UserData[i,4]+",     "
							+myData.UserData[i,2]+",     "
							+myData.UserData[i,5]+",     "
							+myData.UserData[i,6]+",     "
							+myData.UserData[i,7];
					count++;
					found = true;
				}
			} 
			found = false;
		}
		
	}
}
