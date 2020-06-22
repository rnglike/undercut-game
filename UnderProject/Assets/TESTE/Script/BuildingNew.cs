using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNew : MonoBehaviour
{
    public Room baseLevel;

    public GameObject level;
    public GameObject levelEnd;
    
    public GameObject[] allBombs;
    public Transform[] allDoors;

    public int levelAmount;
    public int levelToMake;
    public int floorMade;

    public Texture2D[] maps;

    public bool IsDone;
    public bool IsReady;

    public bool makeLevels;
    public bool HaveDoors;
    
    public int currentRoom;
    public bool dooring;

    void Start()
    {
        baseLevel = transform.GetChild(0).GetComponent<Room>();

        levelToMake = levelAmount;
    }

    void Update()
    {
        IsDone = CheckIsDone();
        IsReady = CheckBombs();

        if(makeLevels)
        {
            makeLevels = false;
            baseLevel.MakeLevels();
        }

        if((floorMade == levelToMake) && !HaveDoors)
        {
            allDoors = GetAllDoors();
            allBombs = GetAllBombs();
            HaveDoors = true;
        }
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

    GameObject[] GetAllBombs()  //Gives an all bombs array.
    {
        GameObject[] bombArray = GameObject.FindGameObjectsWithTag("Bomb");

        return bombArray;
    }

    Transform[] GetAllDoors()   //Gives an all doors array.
    {
        Transform[] doorArray = new Transform[levelAmount + 1];

        foreach(Transform level in this.transform)
        {
            Transform thisDoor = level.Find("Doors");
            doorArray[level.GetSiblingIndex()] = thisDoor;
        }

        return doorArray;
    }

    public bool CheckIsDone()   //Gives a true when the building is all spawned.
    {
        if(levelToMake != 0)
        {
            if(floorMade < (levelToMake - 1))
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
            foreach(GameObject bomb in allBombs)
            {
                if(!bomb.GetComponent<BombTrigger>().activated)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    public void IncrementFloorMade(){floorMade++;}

}