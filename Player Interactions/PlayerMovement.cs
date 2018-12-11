using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public bool playerIsMoving;
	public float gravity = 8.0f;
	public float walkingSpeed = 5;
	Vector3 priorFrameTransform;
	CharacterController _charCont;

	void Start()

	{
		_charCont = this.gameObject.GetComponent<CharacterController>();
	}

	void Update()
	{
		float deltaX = Input.GetAxis("Horizontal") * walkingSpeed;
		float deltaZ = Input.GetAxis("Vertical") * walkingSpeed;
		Vector3 movement = new Vector3(deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude(movement, walkingSpeed);

		//Stick to the ground while walking
		movement.y = gravity;

		//Ensuring FPS does not change the speed and the movement itself
		movement *= Time.deltaTime;
		movement = transform.TransformDirection(movement);
		_charCont.Move(movement);

		//Track when the player stops and starts moving
		if (Vector3.Distance(transform.position, priorFrameTransform) > 0.01f)
		{
			playerIsMoving = true;

		}
		else
		{
			if (playerIsMoving)
				playerIsMoving = false;
		}
		priorFrameTransform = transform.position;
	}
	}

