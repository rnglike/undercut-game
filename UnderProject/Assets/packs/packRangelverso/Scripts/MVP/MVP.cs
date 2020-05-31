using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MVP : MonoBehaviour
{
	private CameraControl cam;

	void Awake()
	{
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
	}

	void OnTriggerStay2D(Collider2D any)
	{
		if(Input.GetButtonDown("Interact"))
		{
			SceneManager.LoadScene(2);
		}
	}
}
