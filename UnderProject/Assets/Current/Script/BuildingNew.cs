using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNew : MonoBehaviour
{
    public Room baseLevel;

    public GameObject level;
    public GameObject levelEnd;
    
    public List<GameObject> allLevels;
    public Transform[] allBombs;
    public Transform[] allDoors;

    public int levelToMake;
    public int floorMade;

    public Texture2D[] maps;

    public bool IsDone;
    public bool IsReady;

    public bool makeLevels;
    public bool HaveDoors;
    
    public int currentRoom;
    public bool dooring;

    public bool won;

    void Start()
    {
        baseLevel = transform.GetChild(0).GetComponent<Room>();
        allLevels = new List<GameObject>();

        MakeLevels();
    }

    void Update()
    {
        if(makeLevels)
        {
            makeLevels = false;
            MakeLevels();
        }

        if(floorMade == levelToMake)
        {
            if(!HaveDoors)
            {
                HaveDoors = true;
                allDoors = GetAllDoors();
                allBombs = GetAllBombs();                
            }
            
            IsDone = CheckIsDone();
            IsReady = CheckBombs();
        }
    }

    void FixedUpdate()
    {
        foreach(GameObject level in allLevels)
        {
            if((allLevels.IndexOf(level) == currentRoom - 1) || (allLevels.IndexOf(level) == currentRoom - 2) || (allLevels.IndexOf(level) == currentRoom))
            {
                level.SetActive(true);
            }
            else
            {
                level.SetActive(false);
            }
        }
    }

    public void Reset()
    {        
        currentRoom = 0;
        baseLevel.MakeLevels();
    }

    public void MakeLevels()
    {
        foreach(Transform level in this.transform)
        {
            if(level.tag != "Base")
            {
                Destroy(level.gameObject);
            }
        }

        allLevels = new List<GameObject>();
        
        floorMade = 0;
        currentRoom = 0;
        baseLevel.made = false;
        HaveDoors = false;
        baseLevel.MakeLevels();;
    }

    public void GoToNextDoor(Transform currentDoor,Transform thing)
    {
        if(!dooring)
        {
            dooring = true;

            Transform nextDoor = currentDoor;

            if(currentDoor.tag == "upDoor")
            {
                currentRoom--;
                nextDoor = allDoors[currentRoom].Find("DoorDown(Clone)");
            }

            if(currentDoor.tag == "downDoor")
            {
                currentRoom++;
                nextDoor = allDoors[currentRoom].Find("DoorUp(Clone)");
            }

            thing.position = nextDoor.transform.Find("Door").position;

            StartCoroutine("Gambiarra");
        }
    }

    IEnumerator Gambiarra() //Literally it won't be useful anywhere else.
    {
        yield return new WaitForSeconds(.5f);
        dooring = false;
    }

    Transform[] GetAllBombs()  //Gives an all bombs array.
    {
        List<Transform> bombArray = new List<Transform>();

        foreach(Transform level in this.transform)
        {
            if(level.tag != "Base")
            {
                Transform thisBombs = level.Find("Bombs");
                
                foreach(Transform thisBomb in thisBombs)
                {
                    bombArray.Add(thisBomb);
                }
            }
        }

        return bombArray.ToArray();
    }

    Transform[] GetAllDoors()   //Gives an all doors array.
    {
        Transform[] doorArray = new Transform[levelToMake + 1];

        foreach(Transform level in this.transform)
        {
            Transform thisDoor = level.Find("Doors");
            doorArray[level.GetSiblingIndex()] = thisDoor;
        }

        return doorArray;
    }

    public bool CheckIsDone(string mode = "default")   //Gives a true when the building is all spawned.
    {
        if(levelToMake != 0)
        {
            if(mode == "default" && floorMade < levelToMake)
            {
                return false;
            }
            else if(mode == "forLastRoom" && floorMade < (levelToMake - 1))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    public bool CheckBombs()    //Gives a true when all bombs are active.
    {
        if(allBombs.Length > 0)
        {
            foreach(Transform bomb in allBombs)
            {
                if(!bomb.gameObject.GetComponent<BombTrigger>().activated)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    void OnTriggerEnter2D(Collider2D any)
    {
        if(any.tag == "Player")
        {
            won = true;
        }
    }

    public void IncrementFloorMade(){floorMade++;}

}