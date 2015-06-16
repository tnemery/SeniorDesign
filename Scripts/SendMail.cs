using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine;
using System.Collections;

public class SendMail : MonoBehaviour {
	public string sender = "";
	public string receiver = "";
	public string password = "";
	public string smtpHost = "";
	private string msgSub = "";
	private string msgBod = "";
	private GameObject User;
	private string forgottenUser = "";
	private int msgNum = 2; //wait time between message sends
	public GameObject Sending;
	public GameObject UserWindow;
	public GameObject AdminWindow;
	public GameObject UserDefault;
	void Start(){
		User = this.gameObject;
	}

	public void GetSubject(string sub){
		msgSub = sub;
	}

	public void GetBody(string bod){
		msgBod = bod;
	}

	public void SendMsg(){
		int count = User.GetComponent<UserGUI> ().UserData.Length/8;
		for (int i = 0; i <count; i++) { 
			if (User.GetComponent<UserGUI> ().UserData [i, 2] != "null") {
				receiver = User.GetComponent<UserGUI> ().UserData [i, 2];
				Sending.SetActive(true);
				StartCoroutine (SendEmails (msgSub, msgBod, receiver));
				AdminWindow.SetActive(false);
			}
		}
	}

	public void UserToSend(string n){
		if (n != "") {
			for(int i = 0; i< User.transform.GetComponent<UserGUI>().UserData.Length/8;i++){
				if(n == User.transform.GetComponent<UserGUI>().UserData[i,4]){
					receiver = User.transform.GetComponent<UserGUI>().UserData[i,2];
					break;
				}
			}
		}
	}

	public void SendAdminUserMessage(){
		int count = User.GetComponent<UserGUI> ().UserData.Length/8;
		for (int i = 0; i <count; i++) { 
			if (User.GetComponent<UserGUI> ().UserData [i, 2] == receiver) {
				Sending.SetActive(true);
				StartCoroutine (SendEmails (msgSub, msgBod, receiver));
				AdminWindow.SetActive(false);
			}
		}
	}

	public void SendUserMessage(){
		string Additional;
		string curEmail = "";
		string Me = User.transform.GetComponent<UserGUI> ().getUser ();
		for (int i = 0; i< User.transform.GetComponent<UserGUI>().UserData.Length/8; i++) {
			if(Me == User.transform.GetComponent<UserGUI>().UserData[i,4]){
				curEmail = User.transform.GetComponent<UserGUI>().UserData[i,2];
				break;
			}
		}
		Additional = "\n\nThis message was sent from User: "+Me+"\nYou may Contact them at this address: "+curEmail;
		Sending.SetActive(true);
		StartCoroutine (SendEmails ("A User is trying to connect with you", msgBod+Additional, receiver));
		UserWindow.SetActive (false);
		UserDefault.SetActive (true);
	}


	public void ForgotPassword(){
		print (forgottenUser);
		if (forgottenUser != "") {
			for(int i = 0; i< User.transform.GetComponent<UserGUI>().UserData.Length/8;i++){
				if(forgottenUser == User.transform.GetComponent<UserGUI>().UserData[i,4]){
					print ("Email: "+User.transform.GetComponent<UserGUI>().UserData[i,2]);
					Sending.SetActive(true);
					StartCoroutine (SendEmails("Forgotten Password","Hello, \nYou are receiving this message because you requested your password, your password is: \n"+User.transform.GetComponent<UserGUI>().UserData[i,3]+" \nPlease make sure to change your password when you log in again.\nThank You,\nBeaver Strides\nOregon State University",User.transform.GetComponent<UserGUI>().UserData[i,2]));
					break;
				}
			}
		}
	}

	public void GetUser(string n){
		forgottenUser = n;
	}


	IEnumerator SendEmails(string Sub, string Bod, string Reciepient){
		SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
		smtpServer.Port = 587;
		smtpServer.Credentials = new System.Net.NetworkCredential(sender, password) as ICredentialsByHost;
		smtpServer.EnableSsl = true;
		ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyError)
		{
			return true;
		};


			using (var mail = new MailMessage {
				From = new MailAddress(sender),
				Subject = Sub,
				Body = Bod
			}) {
				mail.To.Add (Reciepient);
			print ("sending.....");
				smtpServer.Send (mail);
			print ("sent");
			Sending.SetActive(false);
			}

		yield return new WaitForSeconds(msgNum);
	}

}