using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	private Camera cam;

	public float vel;
	public float maxIdleTime;
	public float idleTime;
	private Vector3 oldPlayerPosition;


	public enum modos
	{
		Auto,
		DevTeclado,
		DevMouse
	};
	public modos escolherModos;


	private Vector3 pos;
	private Transform player;
	private Transform idleTarget;
	private Transform currentRoom;
	[HideInInspector] public bool idle;

	//Camera Position Aux Variables
	public float top;
	public float bottom;
	private bool noTeto = false;
	private bool noChao = false;


	//Zoom Variables
	public float maxZoom;
	public float minZoom;
	public float zoomRate;
	private float defaultOrtographicSize;
	private float privateZoomRate;

	//References
	private Building building;


	public bool firstTime;


	void Awake()
	{
		building = GameObject.FindGameObjectWithTag("Building").GetComponent<Building>();
		cam = GetComponent<Camera>();
	}

	void Start()
	{
		pos = transform.position;

		player = building.player.transform;

		top = building.topRoom.transform.position.y;

		defaultOrtographicSize = cam.orthographicSize;

		oldPlayerPosition = building.player.transform.position;

		idleTarget = building.player.transform.Find("IdleTarget");
	}

	void Update()
	{
		if(privateZoomRate != zoomRate)
		{
			privateZoomRate = zoomRate;
		}

		//Regulates Zoom Rate based on the distance of the object in evidence. (farther == faster)
		DinamicZoomRate();

		//Setting and Checking bounds. (happens only once per building spawn)
		if(BoundSet())
		{
			BoundCheck();
		}

		//Calculates time since input from player. If it reachs a value, camera focus on Cleiton.
		PlayerIdleZoom();

		if(building.spawned)
		{
			currentRoom = building.transform.GetChild(building.currentRoom);

			//Camera Movement (pos variable manipulation)
			if(escolherModos == modos.Auto)
			{
				if(!idle)
				{
					if(currentRoom.GetComponent<Room>().type == "long")
					{
						if
						(
							player.position.x >= (currentRoom.position.x + 10) ||
							player.position.x <= (currentRoom.position.x - 10)
						)
						{
							FocusOn(currentRoom,defaultOrtographicSize,"m","xlock",10);
						}
						else
						{
							FocusOn(player,defaultOrtographicSize,"m","x");
						}

						FocusOn(currentRoom,defaultOrtographicSize,"zm","y");
					}
					else if(currentRoom.GetComponent<Room>().type == "big")
					{
						//Something
					}
					else
					{
						FocusOn(currentRoom,defaultOrtographicSize);
					}
				}
			}
			else if(escolherModos == modos.DevTeclado)
			{
				if(Input.GetKey("u") && (!noTeto))
				{
					pos.y += vel;
				}
				else if(Input.GetKey("j") && (!noChao))
				{
					pos.y -= vel;
				}
			}
			else if(escolherModos == modos.DevMouse)
			{
				if
				(
					(!noTeto && Input.mouseScrollDelta.y > 0f) ||
					(!noChao && Input.mouseScrollDelta.y < 0f)
				)
				{
					pos.y += Input.mouseScrollDelta.y * vel;	
				}
			}
		}

		transform.position = pos;

	}

	//For devMouse e devKeyboard camera
	bool BoundSet()
	{
		if(building.spawned)
		{
			if(bottom == top)
			{
				bottom = building.rooms[building.roomsToSpawn].transform.position.y;
			}
		}
		else
		{
			bottom = top;
			return false;
		}

		return true;
	}

	void BoundCheck()
	{
		if(transform.position.y >= top)
		{
			if(!noTeto)
			{
				pos.y = top;
				noTeto = true;
			}
		}
		else
		{
			noTeto = false;
		}

		if(transform.position.y <= bottom)
		{
			if(!noChao)
			{
				pos.y = bottom;
				noChao = true;
			}
		}
		else
		{
			noChao = false;
		}
	}

	private void PlayerIdleZoom()	//Could make good use of a bool function isMoving() inside PlayerController.cs
	{
		if(firstTime)	//For MVP
		{
			maxIdleTime = 0;
			if(Input.anyKeyDown)
			{
				firstTime = false;
				maxIdleTime = 30;
			}
		}

		if((oldPlayerPosition - building.player.transform.position) == new Vector3(0,0,0))	//if(!player.isMoving())
		{
			if(idleTime < maxIdleTime)
			{
				if(cam.orthographicSize != defaultOrtographicSize)
				{
					idle = false;
				}
				idleTime += Time.deltaTime;
			}
			else
			{
				idle = true;
				FocusOn(idleTarget,maxZoom);
			}
		}
		else
		{
			idle = false;
			idleTime = 0;
		}

		oldPlayerPosition = building.player.transform.position;
	}

	public void FocusOn(Transform position = null, float certainDistance = float.NaN, string type = "zm", string axis = "xy", float correction = 0)
	{
		if(((type == "z") || (type == "zm")) && certainDistance != float.NaN)
		{
			zoomTo(certainDistance);
		}

		if(((type == "m") || (type == "zm")) && position != null)
		{
			moveTo(position,axis,correction);
		}
	}

	private void DinamicZoomRate()
	{
		float newRate;

		if(cam.orthographicSize > defaultOrtographicSize)
		{
			newRate = privateZoomRate * Mathf.Abs(cam.orthographicSize/defaultOrtographicSize);
		}
		else
		{
			newRate = zoomRate;
		}

		privateZoomRate = newRate;
	}

	private void zoomTo(float certainDistance)	//Optimize-me
	{
		if(cam.orthographicSize < certainDistance)
		{
			cam.orthographicSize += (privateZoomRate*2);
			if(cam.orthographicSize > certainDistance)
			{
				cam.orthographicSize = certainDistance;
			}
		}
		else if(cam.orthographicSize > certainDistance)
		{
			cam.orthographicSize -= privateZoomRate;
			if(cam.orthographicSize < certainDistance)
			{
				cam.orthographicSize = certainDistance;
			}
		}
	}

	private void moveTo(Transform thing,string axis,float correction)	//Optimize-me
	{
		if(pos != thing.transform.position)
		{
			if((axis == "x") || (axis == "y") || (axis == "xy"))
			{
				if((axis == "x") || (axis == "xy"))
				{
					if(pos.x > thing.transform.position.x + correction)
					{
						pos.x -= vel;
						if(pos.x < thing.transform.position.x)
						{
							pos.x = thing.transform.position.x;
						}
					}
					else if(pos.x < thing.transform.position.x - correction)
					{
						pos.x += vel;
						if(pos.x > thing.transform.position.x)
						{
							pos.x = thing.transform.position.x;
						}
					}
				}

				if((axis == "y") || (axis == "xy"))
				{
					if(pos.y > thing.transform.position.y)
					{
						pos.y -= vel;
						if(pos.y < thing.transform.position.y)
						{
							pos.y = thing.transform.position.y;
						}
					}
					else if(pos.y < thing.transform.position.y)
					{
						pos.y += vel;
						if(pos.y > thing.transform.position.y)
						{
							pos.y = thing.transform.position.y;
						}
					}
				}
			}

			if((axis == "xlock") || (axis == "ylock") || (axis == "xylock"))
			{
				if((axis == "xlock") || (axis == "xylock"))
				{
					if(correction != 0)
					{
						if(pos.x > (thing.transform.position.x + correction))
						{
							pos.x = (thing.transform.position.x + correction);
						}
						else if(pos.x < (thing.transform.position.x - correction))
						{
							pos.x = (thing.transform.position.x - correction);
						}
					}
					else 
					{
						pos.x = thing.transform.position.x;
					}
				}

				if((axis == "ylock") || (axis == "xylock"))
				{
					pos.y = thing.transform.position.y;
				}
			}
		}
	}
}