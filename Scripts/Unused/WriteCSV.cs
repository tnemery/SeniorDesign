/*
using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class WriteCSV : MonoBehaviour {
	private string username;
	private string steps;

	public void GetName(string name){
		username = name; 
	}

	public void GetSteps(string num){
		steps = num;
	}


	public void Savecsv() {

		string filePath = @"/Data/Master_data_steps.csv";  
		string delimiter = ",";  
		//File.Open (filePath, FileMode.Append);
		string[][] output = new string[][]{ 
			new string[]{username, steps.ToString()}  
		};  
		int length = output.GetLength(0);  
		StringBuilder sb = new StringBuilder();  
		for (int index = 0; index < length; index++)  
			sb.AppendLine(string.Join(delimiter, output[index]));  
		/*
		if (!Directory.Exists ("C:/Data/")) {
			
			Directory.CreateDirectory ("C:/Data/");
		}*/ /*
		print (filePath);
		File.AppendAllText (filePath, sb.ToString ());
		//File.WriteAllText(filePath, sb.ToString());
	}
	/*
	public void MergeCSV(){
		string filePath = @"/Data/";  
		string delimiter = ",";

		File.Open (filePath + "Master_data_steps", FileMode.Open);
		string[][] output = new string[][]{  
			new string[]{"Name", "Steps"},  
			new string[]{username, steps.ToString()}  
		}; 


	}
*/
//}
//class CompileBreaker { public CompileBreaker() { var i = 0 / 0; } }