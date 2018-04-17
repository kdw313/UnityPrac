using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class VirtualButtons: MonoBehaviour {

	public Transform transPlayerCharacter;
	public Button fireButton;
	public Button[] skills;
	public float timer;

	int numOfBullet;
	float fireDelay;

	bool fireButtonClicked;


	void Start()
	{ 
		//TODO: needs to be reference from character class in the future
		numOfBullet = 10;
		fireDelay = 0.5f;
	}

	void Update()
	{
		timer += Time.deltaTime;

		if (timer > fireDelay)
		{
			if (fireButtonClicked)
			{
				print ("Fired");
				timer = 0.0f;
			}
		}

	}

	public void FireOnDown()
	{
		fireButtonClicked = true;
	}

	public void FireOnUp()
	{
		fireButtonClicked = false;
	}

}
