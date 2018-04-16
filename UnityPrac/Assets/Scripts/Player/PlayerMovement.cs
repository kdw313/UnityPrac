using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class PlayerMovement:MonoBehaviour {

	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;

	public VirtualJoystick joystic;
	private Vector3 moveDirection = Vector3.zero;
	Rigidbody playerRigidbody;

	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	float camRayLength = 100f;          // The length of the ray from the camera into the scene.

	Animator anim;                      // Reference to the animator component.


	void Awake()
	{
		playerRigidbody = GetComponent <Rigidbody> ();
	}

	void FixedUpdate()
	{
		float h = joystic.InputDirection.x;
		float v = joystic.InputDirection.z;

		Move (h, v);

		Turning (h, v);

		Animating (h, v);
	}

	void Move (float h, float v)
	{
		moveDirection.Set (h, 0f, v);

		moveDirection = moveDirection.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + moveDirection);
	}


	void Turning(float h, float v)
	{
		if (h != 0f || v != 0f) {

			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotation = Quaternion.LookRotation (joystic.InputDirection);

			// Set the player's rotation to this new rotation.
			playerRigidbody.MoveRotation (newRotation);
		}
	}

	void Animating(float h, float v)
	{

		bool walking = h != 0f || v != 0f;

//		anim.SetBool ("IsWalking", walking);

	}
}
