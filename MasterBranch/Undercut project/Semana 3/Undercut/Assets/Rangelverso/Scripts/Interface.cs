using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interface : MonoBehaviour
{
	public TextMeshProUGUI fpsText;
	public TextMeshProUGUI floorText;

	//References
	private RoomController building;
	private PlayerController player;


	void Awake()
	{
		building = GameObject.FindGameObjectWithTag("Building").GetComponent<RoomController>();
	}

	void Update()
	{
		//Writting on screen.
		fpsText.text = ((int)(1/Time.deltaTime)).ToString() + " fps";
		
		if(building.spawned)
		{
			floorText.text = (building.existingRooms - building.currentRoom).ToString();
		}
	}
}
