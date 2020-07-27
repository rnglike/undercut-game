using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressurable : MonoBehaviour
{
	public string type = "wall";

	public List<GameObject> otherButtons;
	public List<GameObject> doors;

	private List<Transform> wallPos;
	private Transform buttonPos;

	private List<Vector3> oldWallPos;
	private Vector3 oldButtonPos;

	private ZaWarudo zawarudo;

	private float time;
	public Building building;
	public float setTime;
	public float velocity = .5f;

	public bool presuring;
	public bool once;
	public bool active;
	public bool press;

	void Start()
	{
		buttonPos = transform.GetChild(0);
		oldButtonPos = buttonPos.position;

		wallPos = new List<Transform>();
		oldWallPos = new List<Vector3>();
		
		zawarudo = GetComponent<ZaWarudo>();
	}

	void Update()
	{
		if(zawarudo.getIsTimeStopped())
		{
			velocity = 0;
		}
		else
		{
			velocity = .5f;
		}

		if(active)
		{
			if(presuring == false) presuring = true;
		}

		// if(!GetComponent<ZaWarudo>().getIsTimeStopped())
		// {
			// if(presuring == false)
			// {
			// 	if(time <= 0)
			// 	{
			// 		buttonPos.position = oldButtonPos;

			// 		if(type == "wall")
			// 		{
			// 			BackToNormal();
			// 		}
			// 	}
				
			// 	time -= Time.deltaTime;
			// }
			// else
			// {
			// 	buttonPos.position = new Vector3(oldButtonPos.x,(oldButtonPos.y - .5f),oldButtonPos.z);

			// 	if(type == "wall")
			// 	{
			// 		OpenWall();
			// 	}

			// 	time = setTime;
			// }
		// }
	}

	void FixedUpdate()
	{
		if(!once)
		{
			foreach(GameObject door in doors)
			{
				if((door != null))
				{
					//Position Reference
					wallPos.Add(door.GetComponent<Transform>());

					//Position Backup
					oldWallPos.Add(door.GetComponent<Transform>().position);
				}			
			}

			once = true;
		}

		if(presuring == false)
		{
			if(time <= 0)
			{
				buttonPos.position = oldButtonPos;

				if(type == "wall")
				{
					BackToNormal();
				}
			}
			
			time -= Time.deltaTime;
		}
		else
		{
			buttonPos.position = new Vector3(oldButtonPos.x,(oldButtonPos.y - .5f),oldButtonPos.z);

			if(type == "wall")
			{
				OpenWall();
			}

			time = setTime;
		}

		if(press)
		{
			if(!GetComponent<ZaWarudo>().getIsTimeStopped()) press = false;
			presuring = true;
		}
		else 
		{
			presuring = false;
		}
	}

	void OpenWall()
	{
		foreach(Transform wallPo in wallPos)
		{
			if(wallPo.position.y != (oldWallPos[wallPos.IndexOf(wallPo)].y + 5))
			{	
				LeanTween.moveY(wallPo.gameObject,(oldWallPos[wallPos.IndexOf(wallPo)].y - 4),velocity).setEaseOutBounce();
			}
		}
	}
	
	void BackToNormal()
	{
		foreach(Transform wallPo in wallPos)
		{
			LeanTween.moveY(wallPo.gameObject,oldWallPos[wallPos.IndexOf(wallPo)].y,velocity).setEaseOutBounce();	
		}
	}

	void OnTriggerStay2D(Collider2D any)
	{
		press = true;
	}
}