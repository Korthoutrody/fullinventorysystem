using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	public GameObject dropPoint;
	public Camera cam;
	public int dropDistance;

	public GameObject use;
	public GameObject holder;
	public GameObject weapon;
	public GameObject tool;
	public GameObject head;
	
	void Update()
	{

		if (Input.GetKey(KeyCode.Z))
		{
			dropPoint = GameObject.Find("Bip01");
			cam = GameObject.Find("PlayerView").GetComponent<Camera>();

			GameObject go = Instantiate(use, dropPoint.transform.position + cam.transform.forward * dropDistance, Quaternion.identity);
			go.GetComponent<Rigidbody>().velocity = 5 * cam.transform.forward;
			go.GetComponent<ItemEntity>().amount = 3;

			go.name = "TestUse";
		}

		if (Input.GetKey(KeyCode.X))
		{
			dropPoint = GameObject.Find("Bip01");
			cam = GameObject.Find("PlayerView").GetComponent<Camera>();

			GameObject go = Instantiate(holder, dropPoint.transform.position + cam.transform.forward * dropDistance, Quaternion.identity);
			go.GetComponent<Rigidbody>().velocity = 5 * cam.transform.forward;
			go.name = "TestHolder";

		}

		if (Input.GetKey(KeyCode.C))
		{
			dropPoint = GameObject.Find("Bip01");
			cam = GameObject.Find("PlayerView").GetComponent<Camera>();

			GameObject go = Instantiate(weapon, dropPoint.transform.position + cam.transform.forward * dropDistance, Quaternion.identity);
			go.GetComponent<Rigidbody>().velocity = 5 * cam.transform.forward;
			go.name = "TestWeapon";

		}

		if (Input.GetKey(KeyCode.V))
		{
			dropPoint = GameObject.Find("Bip01");
			cam = GameObject.Find("PlayerView").GetComponent<Camera>();

			GameObject go = Instantiate(tool, dropPoint.transform.position + cam.transform.forward * dropDistance, Quaternion.identity);
			go.GetComponent<Rigidbody>().velocity = 5 * cam.transform.forward;
			go.name = "TestTool";
		}

		if (Input.GetKey(KeyCode.B))
		{
			dropPoint = GameObject.Find("Bip01");
			cam = GameObject.Find("PlayerView").GetComponent<Camera>();

			GameObject go = Instantiate(head, dropPoint.transform.position + cam.transform.forward * dropDistance, Quaternion.identity);
			go.GetComponent<Rigidbody>().velocity = 5 * cam.transform.forward;
			go.name = "TestEquipHead";
		}
	}
}
