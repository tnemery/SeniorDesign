/*
using UnityEngine;
using System.Collections;
using System.Net;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;

//https://docs.google.com/spreadsheets/d/1cpyb3eMGHS0dL9zCF6OVPjIZin2S6OR2X1Apg-8-2jM/edit?usp=sharing
//https://docs.google.com/spreadsheets/d/1cpyb3eMGHS0dL9zCF6OVPjIZin2S6OR2X1Apg-8-2jM/pubhtml
//https://docs.google.com/spreadsheets/d/1cpyb3eMGHS0dL9zCF6OVPjIZin2S6OR2X1Apg-8-2jM/pubhtml
public class GoogleTest : MonoBehaviour {

	void Start(){

		SpreadsheetsService myService = new SpreadsheetsService("Test");
		myService.setUserCredentials("beaverstrides@gmail.com", "L0vetoWalk");
		ListFeed feed = GDocService.GetSpreadsheet("1cpyb3eMGHS0dL9zCF6OVPjIZin2S6OR2X1Apg-8-2jM");

		SpreadsheetQuery query = new SpreadsheetQuery();
		//SpreadsheetFeed feed = myService.Query(query);
		
		print("Your spreadsheets:");

		const bool debugTest = true; // change to TRUE to enable debug output
		if ( debugTest ) {
			//ListFeed listFeed = service.Query( listQuery );
			Debug.Log( "loaded Google Doc Spreadsheet: " + feed.Title.Text );
			// Iterate through each row, printing its cell values.
			foreach ( ListEntry row in feed.Entries ) {
				// Print the first column's cell value
				Debug.Log( row.Title.Text );
				// Iterate over the remaining columns, and print each cell value
				foreach ( ListEntry.Custom element in row.Elements ) {
					Debug.Log( element.Value );
					if(element.Value == "3"){
						element.Value = "Hello";
					}
				}
			}
		}
		/*
		foreach (SpreadsheetEntry entry in feed.Entries)
		{
			print(entry.Title.Text);
		}
*/
//	}

//}
//*/