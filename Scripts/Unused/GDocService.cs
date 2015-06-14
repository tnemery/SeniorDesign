﻿/*

// To prepare a Google Spreadsheet for this:
// make sure the spreadsheet has been published to web (FILE >> PUBLISH TO WEB), which is NOT the same as sharing to web

// To use in a different script:
// 1) include "using Google.GData.Client;"
// 2) include "using Google.GData.Spreadsheets;"
// 3) call "GDocService.GetSpreadsheet( string spreadsheetKey );", see DebugTest section for sample usage

using UnityEngine;
using System.Collections;
using Google.GData.Client;
using Google.GData.Spreadsheets;

// for "InsecureSecurityPolicy" class at the end
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class GDocService {
	// grab your spreadsheet's ID / "key" from the URL to access your doc...
	// e.g. everything after "key=" in the URL: https://docs.google.com/spreadsheet/ccc?key=0Ak-N8rbAmu7WdGRFdllybTBIaU1Ic0FxYklIbk1vYlE
	// make sure stop as soon as you hit an ampersand, those are additional URL parameters we don't need
	public static ListFeed GetSpreadsheet (string spreadsheetID) {
		// We need this fake certificate to trick Mono's security to use HTTPS... doesn't work in webplayer's security sandbox
		InsecureSecurityCertificatePolicy.Instate();
		
		SpreadsheetsService service = new SpreadsheetsService( "UnityConnect" );
		
		ListQuery listQuery = new ListQuery( "https://spreadsheets.google.com/feeds/list/" + spreadsheetID + "/default/public/values" );
		
		const bool debugTest = false; // change to TRUE to enable debug output
		if ( debugTest ) {
			ListFeed listFeed = service.Query( listQuery );
			Debug.Log( "loaded Google Doc Spreadsheet: " + listFeed.Title.Text );
			// Iterate through each row, printing its cell values.
			foreach ( ListEntry row in listFeed.Entries ) {
				// Print the first column's cell value
				Debug.Log( row.Title.Text );
				// Iterate over the remaining columns, and print each cell value
				foreach ( ListEntry.Custom element in row.Elements ) {
					Debug.Log( element.Value );
				}
			}
		}
		
		return service.Query( listQuery );
	}
}

// from http://answers.unity3d.com/questions/249052/accessing-google-apis-in-unity.html
public class InsecureSecurityCertificatePolicy {
	public static bool Validator(
		object sender,
		X509Certificate certificate,
		X509Chain chain,
		SslPolicyErrors policyErrors ) {
		// Just accept and move on...
		return true;
	}
	
	public static void Instate() {
		ServicePointManager.ServerCertificateValidationCallback = Validator;
	}
}

*/