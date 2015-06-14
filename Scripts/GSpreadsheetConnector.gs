function doGet(e)
{
  if (!Pass(e))
    return "Incorrect Password.";
    
  var result = ParseRequest(e);
 
  return result;
}

// ************** Elemental Security Check **************

function Pass(e)
{
  var ss = SpreadsheetApp.openById(e.parameters.ssid);
  var sheet = ss.getSheetByName("passcode");
  var sheetPass = sheet.getDataRange().getValue();
  
  if (e.parameters.pass[0] == sheetPass)
    return true;
  else
    return false;
}

// ************** Parse Flow Init **************

function ParseRequest(e)
{
  var result;

  if (e.parameters.action == "GetData")
    result = GetData(e);
  
  if (e.parameters.action == "SetData")
    result = SetData(e);
  
  if (e.parameters.action == "SetData1")
    result = SetData1(e);
  
  if (e.parameters.action == "SetData2")
    result = SetData2(e);
  
  if (e.parameters.action == "MergeData")
    result = MergeData(e);
  
  if (e.parameters.action == "MergeData2")
    result = MergeData2(e);
  
  if (e.parameters.action == "MergeData3")
    result = MergeData3(e);
  
  
  return ContentService.createTextOutput(JSON.stringify(result)).setMimeType(ContentService.MimeType.JSON);
}

// ************** Retrieving Data **************

function MergeData(e)
{ 
  var ss = SpreadsheetApp.openById(e.parameters.ssid);
  var sheet = ss.getSheetByName(e.parameters.sheet);
  var rowArray = [
      [e.parameters.val1.toString(),e.parameters.val2.toString(),e.parameters.val3.toString(),e.parameters.val4.toString(),e.parameters.val5.toString(),e.parameters.val6.toString(),e.parameters.val7.toString(),e.parameters.val8.toString()]
    ];
  
  
  for (var j = 2; j < sheet.getDataRange().getNumRows();j++){
    var range = sheet.getRange(j,1,j,8);
    var values = sheet.getRange(range.getA1Notation()).getValues();
    for(var k = 0; k < values.length;k++){
      if(values[k][4] == e.parameters.val5.toString()){
        //return sheet.getRange(k+2,1).getA1Notation();
        sheet.getRange(sheet.getRange(k+2,1).getA1Notation()).setValue(e.parameters.val1.toString());
        sheet.getRange(sheet.getRange(k+2,2).getA1Notation()).setValue(e.parameters.val2.toString());
        sheet.getRange(sheet.getRange(k+2,3).getA1Notation()).setValue(e.parameters.val3.toString());
        sheet.getRange(sheet.getRange(k+2,4).getA1Notation()).setValue(e.parameters.val4.toString());
        sheet.getRange(sheet.getRange(k+2,5).getA1Notation()).setValue(e.parameters.val5.toString());
        sheet.getRange(sheet.getRange(k+2,6).getA1Notation()).setValue(e.parameters.val6.toString());
        sheet.getRange(sheet.getRange(k+2,7).getA1Notation()).setValue(e.parameters.val7.toString());
        sheet.getRange(sheet.getRange(k+2,8).getA1Notation()).setValue(e.parameters.val8.toString());
        return "MRG OK";
      }
    }
  } 
}


function MergeData2(e)
{ 
  var ss = SpreadsheetApp.openById(e.parameters.ssid);
  var sheet = ss.getSheetByName(e.parameters.sheet);
  var rowArray = [
      [e.parameters.val1.toString(),e.parameters.val2.toString()]
    ];
  
  
  for (var j = 2; j < sheet.getDataRange().getNumRows();j++){
    var range = sheet.getRange(j,1,j,2);
    var values = sheet.getRange(range.getA1Notation()).getValues();
    for(var k = 0; k < values.length;k++){
      if(values[k][0] == e.parameters.val1.toString()){
        //return sheet.getRange(k+2,1).getA1Notation();
        //sheet.getRange(sheet.getRange(k+2,1).getA1Notation()).setValue(e.parameters.val1.toString());
        sheet.getRange(sheet.getRange(k+2,2).getA1Notation()).setValue(e.parameters.val2.toString());
        return "MRG OK";
      }
    }
  } 
}

function MergeData3(e)
{ 
  var ss = SpreadsheetApp.openById(e.parameters.ssid);
  var sheet = ss.getSheetByName(e.parameters.sheet);
  var rowArray = [
      [e.parameters.val1.toString(),e.parameters.val2.toString(),e.parameters.val3.toString()]
    ];
  
  
  for (var j = 2; j < sheet.getDataRange().getNumRows();j++){
    var range = sheet.getRange(j,1,j,4);
    var values = sheet.getRange(range.getA1Notation()).getValues();
    for(var k = 0; k < values.length;k++){
      if(values[k][0] == e.parameters.val1.toString()){
        //return sheet.getRange(k+2,1).getA1Notation();
        //sheet.getRange(sheet.getRange(k+2,1).getA1Notation()).setValue(e.parameters.val1.toString());
        sheet.getRange(sheet.getRange(k+2,3).getA1Notation()).setValue(e.parameters.val2.toString());
        sheet.getRange(sheet.getRange(k+2,4).getA1Notation()).setValue(e.parameters.val3.toString());
        return "MRG OK";
      }
    }
  } 
}

function GetData(e)
{ 
  var o = QueryDataFromSS(e.parameters.ssid, e.parameters.sheet);
  
  return o;
}

function QueryDataFromSS(id, sheetName)
{
  var ss = SpreadsheetApp.openById(id);
  var sheet = ss.getSheetByName(sheetName);

  var dataRange = sheet.getDataRange().offset(1, 0, sheet.getDataRange().getNumRows()-1);
  var objects = getRowsData(sheet, dataRange);

  return objects;
}


function getRowsData(sheet, range, columnHeadersRowIndex)
{
  if (!columnHeadersRowIndex)
  {
    if (range.getRowIndex() - 1 != 0)
      columnHeadersRowIndex = range.getRowIndex() - 1;
    else
      columnHeadersRowIndex = 1;
  }
   
  var numColumns = range.getLastColumn() - range.getColumn() + 1;
  var headersRange = sheet.getRange(columnHeadersRowIndex, range.getColumn(), 1, numColumns);
  var headers = headersRange.getValues()[0];
  return getObjects(range.getValues(), headers);
}

function getObjects(data, keys)
{
  var objects = [];
  for (var i = 0; i < data.length; ++i) {
    var object = {};
    var hasData = false;
    for (var j = 0; j < data[i].length; ++j) {
      var cellData = data[i][j];
      if (isCellEmpty(cellData)) {
        continue;
      }
      object[keys[j]] = cellData;
      hasData = true;
    }
    if (hasData) {
      objects.push(object);
    }
  }
  return objects;
}

function isCellEmpty(cellData)
{
  return typeof(cellData) == "string" && cellData == "";
}

// ************** Saving Data **************

function SetData(e)
{   
  var ss = SpreadsheetApp.openById(e.parameters.ssid);
  var sheet = ss.getSheetByName(e.parameters.sheet);
  
  var firstName = e.parameters.val1;
  var lastName = e.parameters.val2;
  var email = e.parameters.val3;
  var userpassword = e.parameters.val4;
  var username = e.parameters.val5;
  var activities = e.parameters.val6;
  var times = e.parameters.val7;
  var days = e.parameters.val8;
  
  var rowArray = [firstName.toString(), lastName.toString(), email.toString(), userpassword.toString(), username.toString(), activities.toString(), times.toString(), days.toString()];
  
  sheet.appendRow(rowArray);
  
  return "RCVD OK";
}


function SetData1(e)
{   
  var ss = SpreadsheetApp.openById(e.parameters.ssid);
  var sheet = ss.getSheetByName(e.parameters.sheet);
  
  var username = e.parameters.val1;
  var steps = e.parameters.val2;
  
  var rowArray = [username.toString(),steps.toString()];
  
  sheet.appendRow(rowArray);
  
  return "RCVD OK";
}


function SetData2(e)
{   
  var ss = SpreadsheetApp.openById(e.parameters.ssid);
  var sheet = ss.getSheetByName(e.parameters.sheet);
  
  var username = e.parameters.val1;
  var steps = e.parameters.val4;
  var startdate = e.parameters.val2;
  var lastdate = e.parameters.val3;
  
  var rowArray = [username.toString(), startdate.toString(), lastdate.toString(),steps.toString()];
  
  sheet.appendRow(rowArray);
  
  return "RCVD OK";
}