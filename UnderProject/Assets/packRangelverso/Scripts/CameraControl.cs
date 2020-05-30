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
		DevMouse,
		JustFollow
	};
	public modos escolherModos;


	private Vector3 pos;

	private Transform player;
	private Transform idleTarget;
	private Transform currentRoom;
	private Transform virtualCurrentRoom;


	public bool idle;

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

	private Vector3 evidencePoint;
	private Vector3 pointA;
	private Vector3 pointC;

	//References
	private Building building;


	public bool firstTime;


	void Awake()
	{
		if(escolherModos != modos.JustFollow)
		{
			building = GameObject.FindGameObjectWithTag("Building").GetComponent<Building>();
		}

		cam = GetComponent<Camera>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().transform;
	}

	void Start()
	{
		if(escolherModos != modos.JustFollow)
		{
			top = building.topRoom.transform.position.y;
		}

		pos = transform.position;
		defaultOrtographicSize = cam.orthographicSize;
		oldPlayerPosition = player.position;
		idleTarget = player.Find("IdleTarget");

		movedTo(idleTarget.position,"xylock");
		zoomTo(maxZoom,"cut");
	}

	void Update()
	{
		if(privateZoomRate != zoomRate)
		{
			privateZoomRate = zoomRate;
		}

		//Regulates Zoom Rate based on the distance of the object in evidence. (farther == faster)
		DinamicZoomRate();

		//Calculates time since input from player. If it reachs a value, camera focus on Cleiton.
		PlayerIdleZoom();

		currentRoom = building.transform.GetChild(building.currentRoom);
		virtualCurrentRoom = building.transform.GetChild(building.virtualCurrentRoom);

		if(building.suicided)
		{
			escolherModos = modos.JustFollow;
		}
		else
		{
			escolherModos = modos.Auto;
		}

		if(escolherModos == modos.JustFollow)
		{
			if(!idle)
			{
				if(player.position.y == pos.y)
				{
					movedTo(player.position,"ylock");
				}
				else
				{
					movedTo(player.position,"y");
				}
				zoomTo(defaultOrtographicSize);
			}
		}
		else
		{
			//Setting and Checking bounds. (happens only once per building spawn)
			if(BoundSet())
			{
				BoundCheck();
			}

			if(building.spawned)
			{
				//Camera Movement (pos variable manipulation)
				if(escolherModos == modos.Auto)
				{
					if(!idle)
					{
						SortCameraBehaviour();
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
		if((oldPlayerPosition - player.position) == new Vector3(0,0,0))	//if(!player.isMoving())
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
				FocusOn(idleTarget.position,maxZoom);
			}
		}
		else
		{
			idle = false;
			idleTime = 0;
		}

		oldPlayerPosition = player.position;
	}

	public void FocusOn(Vector3 position, float certainDistance = float.NaN)
	{
		movedTo(position);

		if(certainDistance != float.NaN)
		{
			zoomTo(certainDistance);
		}
	}

	private void zoomTo(float certainDistance,string mode = "soft")	//Optimize-me
	{
		if(mode == "soft")
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
		else if(mode == "cut")
		{
			cam.orthographicSize = certainDistance;
		}
	}

	private bool movedTo(Vector3 thing,string axis = "xy", float correction = 0)	//Optimize-me
	{
		if(pos != thing)
		{
			if((axis == "x") || (axis == "y") || (axis == "xy"))
			{
				if((axis == "x") || (axis == "xy"))
				{
					if(pos.x > thing.x + correction)
					{
						pos.x -= vel;
						if(pos.x < thing.x)
						{
							pos.x = thing.x;
						}
					}
					else if(pos.x < thing.x - correction)
					{
						pos.x += vel;
						if(pos.x > thing.x)
						{
							pos.x = thing.x;
						}
					}

					if((axis != "xy") && (pos.x == thing.x))
					{
						return true;
					}
				}

				if((axis == "y") || (axis == "xy"))
				{
					if(pos.y > thing.y)
					{
						pos.y -= vel;
						if(pos.y < thing.y)
						{
							pos.y = thing.y;
						}
					}
					else if(pos.y < thing.y)
					{
						pos.y += vel;
						if(pos.y > thing.y)
						{
							pos.y = thing.y;
						}
					}

					if((axis != "xy") && (pos.y == thing.y))
					{
						return true;
					}
				}
			}

			if((axis == "xlock") || (axis == "ylock") || (axis == "xylock"))
			{
				if((axis == "xlock") || (axis == "xylock"))
				{
					pos.x = thing.x;

					if(axis != "xylock")
					{
						return true;
					}
				}

				if((axis == "ylock") || (axis == "xylock"))
				{
					pos.y = thing.y;

					if(axis != "xylock")
					{
						return true;
					}
				}
			}
		}
		else
		{
			return true;
		}

		return false;
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

	private void SortCameraBehaviour()
	{
		
		if(virtualCurrentRoom.GetComponent<Room>().type == "long")
		{
			pointA = new Vector3((virtualCurrentRoom.position.x - 10),(virtualCurrentRoom.position.y),(virtualCurrentRoom.position.z));
			pointC = new Vector3((virtualCurrentRoom.position.x + 10),(virtualCurrentRoom.position.y),(virtualCurrentRoom.position.z));

			if(!building.changingFloors)
			{
				if((player.position.x >= pointA.x) && (player.position.x <= pointC.x))
				{
					if(pos != player.position)
					{
						movedTo(player.position,"x");
					}
					else
					{
						movedTo(player.position,"xlock");
					}
				}
				else
				{
					if(player.position.x < pointA.x)
					{
						movedTo(pointA,"xlock");
					}

					if(player.position.x > pointC.x)
					{
						movedTo(pointC,"xlock");
					}
				}
			}
		}
		else if(virtualCurrentRoom.GetComponent<Room>().type == "big")
		{
			pointA = new Vector3((virtualCurrentRoom.position.x),(virtualCurrentRoom.position.y - 12),(virtualCurrentRoom.position.z));
			pointC = new Vector3((virtualCurrentRoom.position.x),(virtualCurrentRoom.position.y + 12),(virtualCurrentRoom.position.z));

			if(!building.changingFloors)
			{
				if((player.position.y >= pointA.y) && (player.position.y <= pointC.y))
				{
					if(pos.y != player.position.y)
					{
						movedTo(player.position,"y");
					}
				}
				else
				{
					if(player.position.y < pointA.y)
					{
						movedTo(pointA,"y");
					}
					else if(player.position.y == pointA.y)
					{
						movedTo(pointA,"ylock");
					}

					if(player.position.y > pointC.y)
					{
						movedTo(pointC,"y");
					}
					else if(player.position.y == pointC.y)
					{
						movedTo(pointC,"ylock");
					}
				}
			}

			movedTo(virtualCurrentRoom.position,"xlock");
		}
		else
		{
			evidencePoint = virtualCurrentRoom.position;

			movedTo(evidencePoint,"xy");
		}

		if(building.changingFloors)
		{
			if(virtualCurrentRoom.GetComponent<Room>().type == "long")
			{

			}
			else if(virtualCurrentRoom.GetComponent<Room>().type == "big")
			{
				if(player.position.y < pointA.y)
				{
					evidencePoint = pointA;
				}

				if(player.position.y > pointC.y)
				{
					evidencePoint = pointC;
				}
			}
			else
			{
				evidencePoint = virtualCurrentRoom.position;
			}

			if(pos.y == evidencePoint.y)
			{
				building.currentRoom = building.virtualCurrentRoom;
				building.player.LockThePlayerAlternate();
				building.changingFloors = false;
			}
			else
			{
				if(evidencePoint != virtualCurrentRoom.position)
				{
					movedTo(evidencePoint,"y");
				}
			}
		}

		zoomTo(defaultOrtographicSize);
	}
}