using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressurable : MonoBehaviour
{
	public string type = "wall";

	public GameObject door;

	private Transform wallPos;
	private Transform buttonPos;

	private Vector3 oldWallPos;
	private Vector3 oldButtonPos;

	private ZaWarudo zawarudo;

	private float time;
	public Building building;
	public float setTime;
	public float velocity = .5f;

	public bool presuring;
	public bool once;

	void Start()
	{
		buttonPos = transform.GetChild(0);
		oldButtonPos = buttonPos.position;

		// if(type == "door")
		// {
		// 	door = GameObject.FindGameObjectWithTag("Building").transform.Find("TopRoom").Find("DoorDeDeus").gameObject;
		// }
		
		zawarudo = GetComponent<ZaWarudo>();
	}

	void Update()
	{
		if((door != null) && !once)
		{
			//Position Reference
			wallPos = door.GetComponent<Transform>();

			//Position Backup
			oldWallPos = wallPos.position;

			once = true;
		}

		// if(zawarudo.getIsTimeStopped())
		// {
		// 	velocity = 0;
		// }
		// else
		// {
		// 	velocity = .5f;
		// }

		if(presuring == false)
		{
			buttonPos.position = oldButtonPos;

			if(type == "wall")
			{
				BackToNormal();
				time -= Time.deltaTime;
			}
		}
		else
		{
			time = setTime;

			if(type == "wall")
			{
				OpenWall();
			}

			if(type == "door")
			{
				door.SetActive(true);
			}
		}
	}

	void OpenWall()
	{
		if(wallPos.position.y != (oldWallPos.y + 5))
		{
			LeanTween.moveY(wallPos.gameObject,(oldWallPos.y - 4),velocity).setEaseOutBounce();
		}
	}

	void OnTriggerStay2D(Collider2D any)
	{
		buttonPos.position = new Vector3(oldButtonPos.x,(oldButtonPos.y - .5f),oldButtonPos.z);
		presuring = true;
	}

	void OnTriggerExit2D(Collider2D any)
	{
		if(type != "bomb")
		{
			presuring = false;
		}
	}

	void BackToNormal()
	{
		if((time <= 0) && (type == "wall"))
		{
			LeanTween.moveY(wallPos.gameObject,oldWallPos.y,.5f).setEaseOutBounce();
			buttonPos.position = oldButtonPos;
		}
	}
}