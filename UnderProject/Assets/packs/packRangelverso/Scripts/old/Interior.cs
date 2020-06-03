using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interior : MonoBehaviour
{
	public bool mounting;
	public bool createPath;
	public bool startDraw;

	private Transform doors;
	private Transform interior;
	private Transform lastLevel;

	public GameObject[] parts;
	private Transform allLevels;

	[HideInInspector] public List<Transform> path;
	private int firstPath;
	private int secondPath;

	public int currentLevel;
	public int currentPlace;

	public int eprMax;
	private int epr = 0;

	private string lastLevelName;

	private Building building;


	void Awake()
	{
		//building = GameObject.FindGameObjectWithTag("Building").GetComponent<Building>();

		interior = transform.Find("Interior");
		allLevels = transform.Find("Levels");
		doors = transform.Find("Doors");
		mounting = true;

		lastLevelName = "7";
	}

	void Update()
	{
		if(mounting)
		{
			mounting = false;

			Level("delete");
			CreatePath();
			Level("create");

			Gatherdoors();
		}
	}
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;

		if(startDraw)
		{
			foreach(Transform point in path)
			{
				if(path.IndexOf(point) < (path.Count - 1))
				{
					Gizmos.DrawLine(point.position,path[path.IndexOf(point) + 1].position);
				}
			}
		}
	}

	void CreatePath()
	{
		startDraw = false;
		Path("erase");

		path = new List<Transform>();

		foreach(Transform level in allLevels)
		{
			if(level.GetSiblingIndex() == 0){firstPath = Random.Range(0,level.childCount);}

			path.Add(level.GetChild(firstPath));

			do{secondPath = Random.Range(0,level.childCount);}
			while(secondPath == firstPath);

			path.Add(level.GetChild(secondPath));

			firstPath = secondPath;
		}

		Path("write");
		startDraw = true;
	}

	void Path(string function)
	{
		foreach(Transform point in path)
		{
			int currentPoint = path.IndexOf(point);

			if(currentPoint == 0)
			{
				if(function == "write")
				{
					point.tag = "entrance";
				}
				else if(function == "erase")
				{
					point.tag = "Untagged";
				}
			}
			else if(currentPoint == (path.Count - 1))
			{
				if(function == "write")
				{
					point.tag = "exit";
				}
				else if(function == "erase")
				{
					point.tag = "Untagged";
				}
			}
			else if((currentPoint > 0) && (path[currentPoint - 1].tag == "closed"))
			{
				if(function == "write")
				{
					point.tag = "open";
				}
				else if(function == "erase")
				{
					point.tag = "Untagged";
				}
			}
			else
			{
				if(function == "write")
				{
					point.tag = "closed";
				}
				else if(function == "erase")
				{
					point.tag = "Untagged";
				}
			}

			point.name = "path";
		}
	}

	void Level(string function)
	{
		if(function == "create")
		{
			foreach(Transform level in allLevels)
			{
				currentLevel = level.GetSiblingIndex();

				foreach(Transform place in level)
				{
					currentPlace = place.GetSiblingIndex();
					Instantiate(DecidePart(place,level),place.position,Quaternion.identity,interior);
				}
			}
		}
		else if(function == "delete")
		{
			foreach(Transform part in interior)
			{
				Destroy(part.gameObject);
			}
		}
	}

	GameObject DecidePart(Transform place,Transform level,string exclude = "none")
	{
		GameObject choosenPart = parts[Random.Range(0,parts.Length)];

		if(place.tag == "Untagged")				//Trying to sort cool levels
		{
			if((LastPlace(level,"tag") == "exit") || (NextPlace(level,"tag") == "exit"))
			{
				return parts[5];
			}
			else if((choosenPart.name == "Aberto") || (choosenPart.name == "Aberto Small"))
			{
				if(LevelUpper(allLevels,"tag") != "entrance")
				{
					place.name = "Aberto";
					return choosenPart;
				}
			}
			else if((choosenPart.name == "Fechado") || (choosenPart.name == "Fechado Small") || (choosenPart.name == "Fechado Small 1"))
			{
				place.name = "Fechado";
				return choosenPart;
			}
			else if(choosenPart.name == "Wall Small")
			{
				if(LastPlace(level) == "Aberto")
				{
					place.name = "Wall";
					return choosenPart;
				}
			}
			//Enemies -------------------------------------
				else
				{
					if(level.name != lastLevelName)
					{
						epr = 0;
						lastLevelName = level.name;
					}

					if((epr < eprMax) && (level.name != "7"))
					{
						if(choosenPart.name == "SpawnBomb")
						{
							place.name = "Enemy";
							epr += 1;
							return choosenPart;
						}
						else if(choosenPart.name == "SpawnImao")
						{
							place.name = "Enemy";
							epr += 1;
							return choosenPart;
						}
						else if(choosenPart.name == "SpawnBloq")
						{
							place.name = "Enemy";
							epr += 1;
							return choosenPart;
						}
						else if(choosenPart.name == "SpawnBand")
						{
							place.name = "Enemy";
							epr += 1;
							return choosenPart;
						}
					}
				}
		}
		else if(place.tag != "Untagged")		//Trying to maintain clear path
		{
			if(place.tag == "entrance")
			{
				place.name = "Fechado";
				return parts[2];
			}
			if(place.tag == "closed")
			{
				place.name = "Fechado";
				return parts[1];
			}
			if(place.tag == "open")
			{
				place.name = "Aberto";
				return parts[0];
			}
			if((place.tag == "exit" ))
			{
				place.name = "Fechado";
				if(gameObject.name == "BottomRoom(Clone)")
				{
					return parts[1];
				}
				else
				{
					return parts[3];
				}
			}
		}

		return DecidePart(place,level,exclude);
	}

	void Gatherdoors()
	{
		interior.Find("DoorUp Small(Clone)").Find("DoorUp").parent = doors;
		if(gameObject.name != "BottomRoom(Clone)")
		{
			interior.Find("DoorDown Small(Clone)").Find("DoorDown").parent = doors;
		}
	}










	string LevelUpper(Transform something,string type = "name")
	{
		if(currentLevel > 0)
		{
			if(type == "name")
			{
				return something.GetChild(currentLevel - 1).GetChild(currentPlace).name;
			}
			else if(type == "tag")
			{
				return something.GetChild(currentLevel - 1).GetChild(currentPlace).tag;
			}
		}

		return "No LevelBelow";
	}

	Transform LevelUpperTransform(Transform something)
	{
		if(currentLevel > 0)
		{
			return something.GetChild(currentLevel - 1);
		}
		else
		{
			return null;
		}
	}
	
	string NextPlace(Transform something,string type = "name")
	{
		if(something != null)
		{
			if(currentPlace < 5)
			{
				if(type == "name")
				{
					return something.GetChild(currentPlace + 1).name;
				}
				else if(type == "tag")
				{
					return something.GetChild(currentPlace + 1).tag;
				}
			}	
		}

		return "No NextPlace";
	}

	Transform NextPlaceTransform(Transform something)
	{
		if(currentLevel > 0)
		{
			return something.GetChild(currentPlace + 1);
		}
		else
		{
			return null;
		}
	}

	string LastPlace(Transform something,string type = "name")
	{
		if(something != null)
		{
			if(currentPlace > 0)
			{
				if(type == "name")
				{
					return something.GetChild(currentPlace - 1).name;
				}
				else if(type == "tag")
				{
					return something.GetChild(currentPlace - 1).tag;
				}
			}
		}

		return "No LastPlace";
	}
}