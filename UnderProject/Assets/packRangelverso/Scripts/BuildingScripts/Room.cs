using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public int index;
	public string type; 						//Tipos diferentes de sala para CADA SpawnPoint.
	public string spawns = "any";
	public GameObject outsideTile;

	private int rand; 						//Variável que escolherá uma sala aleatória.

	private Transform spawnPoint;

	//Referência
	private Building building;		    	//Referência ao script que garda variáveis dos andares.

	void Awake()
	{
		building = GameObject.FindGameObjectWithTag("Building").GetComponent<Building>();
		transform.parent = building.transform;
		spawnPoint = transform.Find("SpawnPoint");
	}

	void Start()
	{
		if((gameObject.name != "TopRoom") && (gameObject.name != "BottomRoom"))
		{
			Invoke("SpawnRooms", 0.1f);
		}
	}

	void Update()
	{
		if(building.spawned && gameObject.name != "TopRoom")
		{
			if(building.closeRooms)
			{
				outsideTile.SetActive(true);
			}
			else if(!building.closeRooms)
			{
				outsideTile.SetActive(false);
			}
		}
	}

	public void SpawnRooms()
	{
		//Spawns a random room or the final room.
		if(building.roomsSpawning > building.existingRooms)
		{
            if ((building.existingRooms + 1) == building.roomsToSpawn)
			{
				Instantiate(building.roomsTier0[0],spawnPoint.position,Quaternion.identity,building.transform);
				
				if(building.ArrayDataFilled())
				{
					building.spawned = true;
				}
			}
			else
			{
				if(spawns == "any")
				{
					rand = Random.Range(0,building.roomsTier1.Length);
					Instantiate(building.roomsTier1[index],spawnPoint.position,Quaternion.identity);
				}
			}
			building.existingRooms++;
		}
	}
}