using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interface : MonoBehaviour
{
	// public TextMeshProUGUI fps;
	// public TextMeshProUGUI timer;
	public GameObject floor;
	public TextMeshProUGUI floorText;
	// public GameObject HowToPlay;
	// public TextMeshProUGUI SetaPraCima;
	public TextMeshProUGUI undercut;
	public GameObject life;

	public bool on;
	public bool inMenu;

	//References
	private Building building;
	private PlayerController player;
	private CameraControl cam;


	void Awake()
	{
		building = GameObject.FindGameObjectWithTag("Building").GetComponent<Building>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();

		inMenu = true;
	}

	void Update()
	{
		if(cam.firstTime)
		{
			if(inMenu)
			{
				building.closeRooms = true;
				cam.holdFar();
				undercut.gameObject.SetActive(true);
			}

			if(Input.GetKeyDown("u"))
			{
				undercut.gameObject.SetActive(false);
				inMenu = false;
			}
		}

		//Writting on screen.
		// if(on)
		// {
		// 	fps.text = ((int)(1/Time.deltaTime)).ToString() + " fps";
		// 	timer.text = ((int)building.bombTimer).ToString();;
		// }
		
		if(building.spawned)
		{
			floorText.text = (building.existingRooms - building.currentRoom).ToString();
		}

		if((cam.idle) || building.suicided)
		{
			life.SetActive(false);
			floor.SetActive(false);
			// HowToPlay.SetActive(false);
			// SetaPraCima.gameObject.SetActive(false);
		}
		else if(!floor.activeSelf && !inMenu)
		{
			life.SetActive(true);
			floor.SetActive(true);
			// HowToPlay.SetActive(true);
		}

		// if(building.player.nextToDoor)
		// {
		// 	SetaPraCima.gameObject.SetActive(true);
		// }
		// else
		// {
		// 	SetaPraCima.gameObject.SetActive(false);
		// }
	}
}
