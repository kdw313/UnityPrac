using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	public float moveSpeed = 5.0f;
	public float drag = 0.5f;
	public float terminalRotationSpeed = 25.0f;
	public Vector3 MoveVector{ set; get; }
	public VirtualJoystic joystick;

	private Rigidbody thisRigidbody;


	private void Move()
	{
		thisRigidbody.AddForce((MoveVector * moveSpeed));
	}
		


	private Vector3 PoolInput()
	{
		Vector3 dir = Vector3.zero;

		dir.x = Input.GetAxis ("Horizontal");
		dir.z = Input.GetAxis ("Vertical");

		if (dir.magnitude > 1)
			dir.Normalize ();

		return dir;
	}
}
