using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public BuildingNew building;

	public Transform place;

	public bool made;
	public bool baseLevel;
	public bool lastLevel;

	void Awake()
	{
		building = GameObject.FindGameObjectWithTag("Building").GetComponent<BuildingNew>();
	}

	void Start()
	{
		if(!baseLevel && !lastLevel)
		{
			Invoke("MakeLevels",.1f);
		}
	}

	public void MakeLevels()
	{
		if(!made)
		{
			if(!building.CheckIsDone("forLastRoom"))
			{
				Instantiate(building.level,place.position,Quaternion.identity,transform.parent);
			}
			else
			{
				Instantiate(building.levelEnd,place.position,Quaternion.identity,transform.parent);
			}
			
			building.IncrementFloorMade();

			made = true;
		}
	}

	public Texture2D ChooseMap(Texture2D map)
	{
		Texture2D[] maps = building.maps;
		int rand = Random.Range(0,maps.Length);

		return maps[rand];
	}
}