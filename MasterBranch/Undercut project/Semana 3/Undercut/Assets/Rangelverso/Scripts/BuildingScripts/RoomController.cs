using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public int roomsToSpawn;                                    //Amount of rooms that the building will support.
    public GameObject[] roomsTier0;                             //Array of special (and non-threatfull) rooms.
    public GameObject[] roomsTier1;                             //Array of easy rooms.



    //Hidden, but public, variables here.
        [HideInInspector] public int roomsSpawning;             //
        [HideInInspector] public int existingRooms;             //The instatiated rooms counter.
        [HideInInspector] public bool spawned;                  //Is it spawned?
        [HideInInspector] public GameObject[] doors;            //Array of all doors in the building (sorry...).
        [HideInInspector] public GameObject[] floorLimits;      //Array of all borders between floors. (sorry2...).
        [HideInInspector] public int currentRoom = 0;           //

    private Vector3 playerPos;
    private Room spawnPoint;                                    //Reference to the RoomSpawner script attached to SpawnPoint.
    private PlayerController player;


    void Awake()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Spawn").GetComponent<Room>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        FloorTracking();
    }

    //Spawn (or respawn) the rooms in the building.
    public void SpawnRooms()
    {
        if (existingRooms > 0)      //If there was any, they are gone now.
        {
            ResetAll();
        }
        if (roomsToSpawn > 0)       //If they needed to exist, it'll happen.
        {
            roomsSpawning = roomsToSpawn;
            spawnPoint.Invoke("SpawnRooms", 0.1f);
        }
    }

    //Here's where they all get destroyed (but roof). Also where those hidden variables are resetted.
    public void ResetAll()
    {
        spawned = false;

        existingRooms = 0;

        player.Warps(0f, 0f, 0f);

        for (int room = 1; room < transform.childCount; room++)
        {
            Destroy(transform.GetChild(room).gameObject);
        }
    }

    //It's just how doors work if they weren't optimized at all. (OPTIMIZE ME PLS (or don't (pls do)))
    public void DoorWarp(Transform thing, bool downStairs)
    {
        if(downStairs)          //Going down
        {
            if(currentRoom == 0)
            {
                thing.gameObject.GetComponent<PlayerController>().inside = true;
            }
            thing.position = doors[currentRoom + 1].transform.GetChild(0).position;
            downStairs = !downStairs;
        }
        else if(!downStairs)    //Going up
        {
            if(currentRoom > 0)
            {
                thing.position = doors[currentRoom - 1].transform.GetChild(1).position;
                downStairs = !downStairs;
            }
            if(currentRoom == 1)
            {
                thing.gameObject.GetComponent<PlayerController>().inside = false;
            }
        }
    }

    //Which floor is the player in? (+ suicide feature)
	private void FloorTracking()
	{
		playerPos = player.transform.position;

		if(spawned)
		{
			if(currentRoom > 0)
			{
				if(playerPos.y > floorLimits[currentRoom - 1].transform.position.y)
				{
					currentRoom--;
				}
			}
			if(currentRoom < floorLimits.Length)
			{
				if(playerPos.y < floorLimits[currentRoom].transform.position.y)
				{
					currentRoom++;
				}
			}
		}
	}
}