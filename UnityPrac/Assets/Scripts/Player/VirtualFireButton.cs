using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class VirtualFireButton : MonoBehaviour {

	public Transform transPlayerCharacter;
//	public Vector3 InputDirection { set; get; }

	int numOfBullet;

	private void Start()
	{
		numOfBullet = 10;
		transPlayerCharacter = GetComponent<Transform> ();
//		InputDirection = Vector3.zero;
	}

	void OnGUI()
	{
		if (GUI.Button ( new Rect(300,150,100,100), "Fire")) 
		{
			print ("Button clicked");
		}
	}


	void OnClick()
	{
		if(numOfBullet > 0)
			Debug.Log ("Fire" + numOfBullet--);
		
	}

	void OnRelease()
	{
		numOfBullet = 10;
		Debug.Log ("Fire finished");
//		InputDirection = Vector3.zero;

	}
}
