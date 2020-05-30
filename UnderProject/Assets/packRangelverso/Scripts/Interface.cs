using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interface : MonoBehaviour
{
	public TextMeshProUGUI fps;
	public GameObject floor;
	public TextMeshProUGUI floorText;
	public GameObject HowToPlay;
	public TextMeshProUGUI SetaPraCima;

	//References
	private Building building;
	private PlayerController player;
	private CameraControl cam;


	void Awake()
	{
		building = GameObject.FindGameObjectWithTag("Building").GetComponent<Building>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
	}

	void Update()
	{
		//Writting on screen.
		fps.text = ((int)(1/Time.deltaTime)).ToString() + " fps";
		
		if(building.spawned)
		{
			floorText.text = (building.existingRooms - building.currentRoom).ToString();
		}

		if((cam.idle) || building.suicided)
		{
			floor.SetActive(false);
			HowToPlay.SetActive(false);
			SetaPraCima.gameObject.SetActive(false);
		}
		else if(!floor.activeSelf)
		{
			floor.SetActive(true);
			HowToPlay.SetActive(true);
		}

		if(building.player.nextToDoor)
		{
			SetaPraCima.gameObject.SetActive(true);
		}
		else
		{
			SetaPraCima.gameObject.SetActive(false);
		}
	}
}
