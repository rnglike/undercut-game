using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public int roomTypes; 						//Tipos diferentes de sala para CADA SpawnPoint.

	private int rand; 							//Variável que escolherá uma sala aleatória.

	//Referência
	private RoomController building;		    //Referência ao script que garda variáveis dos andares.


	void Awake()
	{
		building = GameObject.FindGameObjectWithTag("Building").GetComponent<RoomController>();
		transform.parent.transform.parent = building.transform;
	}

	void Start()
	{
		Invoke("SpawnRooms", 0.1f);
	}

	public void SpawnRooms()
	{
		//Spawns a random room or the final room.
		if(building.roomsSpawning > building.existingRooms)
		{
            if ((building.existingRooms + 1) == building.roomsToSpawn)
			{
				Instantiate(building.roomsTier0[0],transform.position,Quaternion.identity);
				GameObject.FindGameObjectWithTag("Sala da Bomba").transform.parent = building.transform;

                //Getting some arrays.
				building.floorLimits = GameObject.FindGameObjectsWithTag("FloorLimit");
				building.doors = GameObject.FindGameObjectsWithTag("Door");
				
				building.spawned = true;
			}
			else
			{
				if(roomTypes == 0)
				{
					rand = Random.Range(0,building.roomsTier1.Length);
					Instantiate(building.roomsTier1[rand],transform.position,Quaternion.identity);
				}
			}
			building.existingRooms++;
		}
	}
}