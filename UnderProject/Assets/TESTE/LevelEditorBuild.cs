using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelEditorBuild : MonoBehaviour
{
    public bool made;

    public BuildingNew building;

    void Update()
    {
        if(!made)
        {
            building.maps[0] = LoadLevel();

            if(building.maps[0] != null)
            {
                made = true;
                building.makeLevels = true;
            }
        }
    }

    Texture2D LoadLevel()
    {
        Texture2D level = null;
        byte[] imageBytes;
        string imagePath = Application.dataPath + "/PutOneLevelHere/level.png";

        if(File.Exists(imagePath))
        {
            imageBytes = File.ReadAllBytes(imagePath);
            level = new Texture2D(10,10);
            level.LoadImage(imageBytes);
        }

        return level;
    }
}
