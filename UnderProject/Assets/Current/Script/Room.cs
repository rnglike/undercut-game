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
			GameObject aux;

			if(!building.CheckIsDone("forLastRoom"))
			{
				aux = Instantiate(building.level,place.position,Quaternion.identity,transform.parent);
			}
			else
			{
				aux = Instantiate(building.levelEnd,place.position,Quaternion.identity,transform.parent);
			}

			building.allLevels.Add(aux);
			
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