using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MainContorl : MonoBehaviour 
{
	void Awake ()
	{
		if (Application.isPlaying)
			Debug.Log ("Application.isPlaying");
		else
			Debug.Log ("[Not] Application.isPlaying");
		Debug.Log ("[MainControl] Awake");
	}
}
