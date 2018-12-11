using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	//Sticking horizontal and vertical movement under the same definition
	public enum RotationAxis
	{
		MouseX = 1,
		MouseY = 2
	}

	//Default Setting for the inspector 
	public RotationAxis axes = RotationAxis.MouseX;

	//Sensitivity of the camera's movement
	public float sensHorizontal = 10.0f;
	public float sensVertical = 10.0f;

	//Realistic view boundaries
	public float minimumVert = -45.0f;
	public float maximumVert = 45.0f;

	//Default rotation
	public float _rotationX = 0;

	void Update()
	{
		//Move the camera with the mouse and the set sensitivity 
		if (axes == RotationAxis.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensHorizontal, 0);
		}
		else if (axes == RotationAxis.MouseY)
		{
			_rotationX -= Input.GetAxis("Mouse Y") * sensVertical;
			_rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
		}

		// ??
		float rotationY = transform.localEulerAngles.y;
		transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);

	}
}

