using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interface : MonoBehaviour
{
	public GameObject cam;
	public GameObject UI;

	public bool firstTime;

	void Start()
	{
		if(firstTime)
		{
			firstTime = false;
			cam.GetComponent<CameraZoom>().aproximated = false;
		}
	}
}
