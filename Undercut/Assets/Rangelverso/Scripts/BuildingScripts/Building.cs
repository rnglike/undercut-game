using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int roomsToSpawn;                                   //Amount of rooms that the building will support.
    public GameObject[] roomsTier0;                            //Array of special (and non-threatfull) rooms.
    public GameObject[] roomsTier1;                            //Array of easy rooms.
    public bool buildOnStart;

    //Hidden, but public, variables here.
        [HideInInspector] public int roomsSpawning;             //The being instantiated rooms counter.
        [HideInInspector] public int existingRooms;             //The instatiated rooms counter.
        [HideInInspector] public int currentRoom = 0;           //
        [HideInInspector] public GameObject playerSpawn;		//
        [HideInInspector] public GameObject[] rooms;            //Array of all doors in the building (sorry1...).
        [HideInInspector] public GameObject[] doors;            //Array of all doors in the building (sorry2...).
        [HideInInspector] public GameObject[] floorLimits;      //Array of all borders between floors. (sorry3...).
        [HideInInspector] public bool spawned;                  //Is it spawned?

    public Room topRoom;                                       //Reference to the SpawPoint script attached to SpawnPoint.
    public PlayerController player;                            //Reference to the PlayerController script attached to player.


    void Awake()
    {
        topRoom = transform.Find("TopRoom").GetComponent<Room>();
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawns");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if(buildOnStart)
        {
            SpawnRooms();
        }
    }

    void Update()
    {
        FloorTracking();
        SuicideCheck();

        if(Input.GetKeyDown("o"))
        {
        	SpawnRooms();
        }
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
            topRoom.Invoke("SpawnRooms", 0.1f);
        }
    }

    //Here's where they all get destroyed (but roof). Also where those hidden variables are resetted.
    public void ResetAll()
    {
        spawned = false;

        existingRooms = 0;

        player.WarpTo(playerSpawn.transform.GetChild(1));

        for (int room = 1; room < transform.childCount; room++)
        {
            Destroy(transform.GetChild(room).gameObject);
        }
    }

    //It's just how doors work if they weren't optimized at all. (OPTIMIZE ME PLS (or don't (pls do)))
    public void DoorWarpTo(Transform thing, Transform door)
    {
        if(door.name == "DoorDown")          //Going down
        {
            if(currentRoom == 0)
            {
                thing.gameObject.GetComponent<PlayerController>().inside = true;
            }
            if(currentRoom < existingRooms)
            {
                thing.position = doors[currentRoom + 1].transform.GetChild(0).transform.position;
            }
        }
        else if(door.name == "DoorUp")    //Going up
        {
            if(currentRoom == 1)
            {
                thing.gameObject.GetComponent<PlayerController>().inside = false;
            }
            if(currentRoom > 0)
            {
                thing.position = doors[currentRoom - 1].transform.GetChild(1).transform.position;
            }
        }
        else
        {
            Debug.LogError("The second parameter in DoorWarp(1,2) should be a door.", door);
        }
    }

    //Which floor is the player in? (+ suicide feature)
	private void FloorTracking()
	{
		if(spawned)
		{
            float playerPos = player.transform.position.y;

			if(currentRoom > 0)
			{
                float floorUp = floorLimits[currentRoom - 1].transform.position.y;

				if(playerPos > floorUp)
				{
					currentRoom--;
				}
			}
			if(currentRoom < floorLimits.Length)
			{
                float floorDown = floorLimits[currentRoom].transform.position.y;

				if(playerPos < floorDown)
				{
					currentRoom++;
				}
			}
		}
	}

	//Checks if player is falling off the building.
    private void SuicideCheck()
    {
        if((!player.inside) && (currentRoom > 0))
        {
            Debug.Log("Dead");
            player.WarpTo(playerSpawn.transform.GetChild(0));
        }
    }

    public bool ArrayDataFilled()
    {
        if(!spawned)
        {
            rooms = GameObject.FindGameObjectsWithTag("Room");
            doors = GameObject.FindGameObjectsWithTag("Doors");
            floorLimits = GameObject.FindGameObjectsWithTag("FloorLimit");

            return true;
        }
        else
        {
            return false;
        }
    }
}