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
			building.IncrementFloorMade();
			
			if(!building.IsDone)
			{
				Instantiate(building.level,place.position,Quaternion.identity,transform.parent);
			}
			else
			{
				Instantiate(building.levelEnd,place.position,Quaternion.identity,transform.parent);
			}

			made = true;
		}
	}
}