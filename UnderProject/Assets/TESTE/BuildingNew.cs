using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNew : MonoBehaviour
{
    public Room baseLevel;

    public GameObject level;
    public GameObject levelEnd;

    public int levelAmount;
    public int levelToMake;
    public int floorMade;

    public bool IsDone;
    public bool makeLevels;

    void Start()
    {
        baseLevel = transform.GetChild(0).GetComponent<Room>();

        levelToMake = levelAmount;
    }

    void Update()
    {
        if(makeLevels)
        {
            makeLevels = false;
            baseLevel.MakeLevels();
        }

        IsDone = CheckIsDone();
    }

    public bool CheckIsDone()
    {
        if(floorMade < levelToMake)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void IncrementFloorMade(){floorMade++;}
}