using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class VirtualButtons: MonoBehaviour {

	public Transform transPlayerCharacter;
	public Image crossHead;
	public Button fireButton;
	public Button[] skills;

	//TODO: debuging purpose, needed to be protected.
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

		crossHead.color = new Color32 (255, 0, 0, 100);
	}

	public void FireOnUp()
	{
		fireButtonClicked = false;

		crossHead.color = new Color32 (255, 255, 255, 100);

	}

}
