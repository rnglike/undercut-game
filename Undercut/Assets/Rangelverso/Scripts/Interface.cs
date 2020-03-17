using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interface : MonoBehaviour
{
	public TextMeshProUGUI fps;
	public TextMeshProUGUI floorText;

	//References
	private Building building;
	private PlayerController player;
	private CameraControl cam;


	void Awake()
	{
		building = GameObject.FindGameObjectWithTag("Building").GetComponent<Building>();
	}

	void Update()
	{
		//Writting on screen.
		fps.text = ((int)(1/Time.deltaTime)).ToString() + " fps";
		
		if(building.spawned)
		{
			floorText.text = (building.existingRooms - building.currentRoom).ToString();
		}
	}
}
