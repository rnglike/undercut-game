using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrigger : MonoBehaviour
{
	private BuildingNew building;
	public bool activated;

	public List<GameObject> buttons;
	public bool buttonsActive;

	void Start()
	{
		building = GameObject.FindGameObjectWithTag("Building").GetComponent<BuildingNew>();
	}

	void Update()
	{
		if(buttonsActive)
		{
			foreach (GameObject button in buttons)
			{
				button.transform.GetChild(0).GetComponent<Pressurable>().active = true;
			}
		}

		if(buttons.Count == 0 || buttonsActive)
		{	
			if(!activated)
			{
				transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
				transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
				transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
			}
			else
			{
				transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
				transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
			}
		}
		else
		{
			CheckButtons();
			transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
		}
	}

    void OnTriggerStay2D(Collider2D any)
    {
		if(any.tag == "Player") any.GetComponent<PlayerController>().uuuuuuuuuuuuuuh = true;

    	if(!building.CheckBombs())
    	{
			if(buttons.Count == 0 || buttonsActive)
			{
				if(Input.GetButtonDown("Interact") && any.tag == "Player")
				{
					activated = true;
				}
			}	
	    }			
    }

	void CheckButtons()
	{
		foreach (GameObject button in buttons)
		{
			if(!button.transform.GetChild(0).GetComponent<Pressurable>().presuring)
			{
				return;
			}
		}

		buttonsActive = true;
	}
}
