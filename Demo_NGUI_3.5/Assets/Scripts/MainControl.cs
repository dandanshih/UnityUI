using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class MainControl : MonoBehaviour 
{
	void Awake ()
	{
		Dictionary<string, string> dictResult = new Dictionary<string, string> ();
		dictResult["Test"] = "1";
		string strResult = JsonConvert.SerializeObject (dictResult);
		Debug.Log (strResult);
	}
}
