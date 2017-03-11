using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Account : MonoBehaviour {
	public static Account instance;
	private Dictionary<string, string> accountHeader;
	private 

	void Awake () {
		instance = this;
	}

	public Dictionary<string, string> getAccountHeader (){
		return accountHeader;
	}
		
	public void setCookie (string cookie){
		//print ("Cookie : " + cookie);
		accountHeader = new Dictionary<string, string> ();
		accountHeader.Add ("Set-Cookie", cookie);
	}
}
