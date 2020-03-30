using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float sms;
    public bool suicided;

    public int roomsToSpawn;                                   //Amount of rooms that the building will support.
    public GameObject[] roomsTier0;                            //Array of special (and non-threatfull) rooms.
    public GameObject[] roomsTier1;                            //Array of easy rooms.
    public bool buildOnStart;

    //Hidden, but public, variables here.
        [HideInInspector] public int roomsSpawning;             //The being instantiated rooms counter.
        [HideInInspector] public int existingRooms;             //The instatiated rooms counter.
        public int currentRoom = 0;           //
        [HideInInspector] public GameObject playerSpawn;		//
        [HideInInspector] public GameObject[] rooms;            //Array of all doors in the building (sorry1...).
        [HideInInspector] public GameObject[] doors;            //Array of all doors in the building (sorry2...).
        [HideInInspector] public GameObject[] floorLimits;      //Array of all borders between floors. (sorry3...).
        [HideInInspector] public bool spawned;                  //Is it spawned?
        public bool closeRooms;
        public int virtualCurrentRoom;
        public bool changingFloors;

    public Room topRoom;                                       //Reference to the SpawPoint script attached to SpawnPoint.
    public PlayerController player;                            //Reference to the PlayerController script attached to player.


    void Awake()
    {
        topRoom = GameObject.Find("TopRoom").GetComponent<Room>();
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawns");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if(buildOnStart)
        {
            SpawnRooms();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown("o"))
        {
        	SpawnRooms();
        }
    }

    void FixedUpdate()
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
            topRoom.Invoke("SpawnRooms", 0.1f);
        }
    }

    //Here's where they all get destroyed (but roof). Also where those hidden variables are resetted.
    public void ResetAll()
    {
        spawned = false;

        existingRooms = 0;

        player.WarpTo(playerSpawn.transform.GetChild(1),"no");
        player.inside = false;

        for (int room = 1; room < transform.childCount; room++)
        {
            Destroy(transform.GetChild(room).gameObject);
        }
    }

    //It's just how doors work if they weren't optimized at all. (OPTIMIZE ME PLS (or don't (pls do)))
    public void DoorWarpTo(Transform thing, Transform door)
    {
    	if(!changingFloors)
    	{
	        if(door.name == "DoorDown")          //Going down
	        {
	        	if(virtualCurrentRoom == 0)
	            {
                    if(virtualCurrentRoom == currentRoom)
                    {
	                   thing.gameObject.GetComponent<PlayerController>().inside = true;
                    }
	            }
	            if(virtualCurrentRoom < existingRooms)
	            {
	                thing.position = doors[virtualCurrentRoom + 1].transform.GetChild(0).transform.position;
	            }
	        }
	        else if(door.name == "DoorUp")    //Going up
	        {
	            if(virtualCurrentRoom > 0)
	            {
                    if(virtualCurrentRoom == currentRoom)
                    {
                        thing.position = doors[virtualCurrentRoom - 1].transform.GetChild(1).transform.position;
                    }
	            }
	            if(virtualCurrentRoom == 1)
	            {
	                thing.gameObject.GetComponent<PlayerController>().inside = false;
	            }
	        }
	        else
	        {
	            Debug.LogError("The second parameter in DoorWarp(1,2) should be a door.", door);
	        }
	    }
    }

    //Which floor is the player in? (+ suicide feature)
	private void FloorTracking()
	{
		if(spawned)
		{
            float playerPos = player.transform.position.y;

			if(virtualCurrentRoom > 0)
			{
                float floorUp = floorLimits[virtualCurrentRoom - 1].transform.position.y;

				if(playerPos > floorUp)
				{
					virtualCurrentRoom--;
				}
			}

			if(virtualCurrentRoom < floorLimits.Length)
			{
                float floorDown = floorLimits[virtualCurrentRoom].transform.position.y;

				if(playerPos < floorDown)
				{
                    if(virtualCurrentRoom < (floorLimits.Length - 1))
                    {
					   virtualCurrentRoom++;
                    }
                    else
                    {
                        SpawnRooms();
                    }
				}
			}
		}
        
        if(!spawned)
        {
            virtualCurrentRoom = 0;
            currentRoom = virtualCurrentRoom;
        }

        if((!player.inside) && (virtualCurrentRoom > 0))
        {
            player.GetGravityControl(true);
            player.SetSpeedY(sms);
            closeRooms = true;
            Debug.Log("dead");

            suicided = true;
        }
        else
        {
            suicided = false;
            closeRooms = false;
            player.GetGravityControl(false);
        }

        if(!suicided)
        {
            if(currentRoom != virtualCurrentRoom)
            {
                if(!changingFloors)
                {
                    player.LockThePlayer();
                }

                changingFloors = true;
            }
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