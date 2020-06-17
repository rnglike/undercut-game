using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct ColorToPrefab
    {
        public Color color;
        public GameObject prefab;
    }

    public Room room;
    public GameObject ground;
    public GameObject doors;

    //Assets to Generate Level
    public Texture2D map;

    [SerializeField]
    public ColorToPrefab[] colorToPrefabs;

    void Awake()
    {
        room = GetComponent<Room>();
    }

    void Start()
    {
        map = room.ChooseMap(map);

        GenerateLevel();
    }

    void GenerateLevel()
    {
        for(int x = 0; x < map.width - 3; x++)
        {
            for(int y = 0; y < map.height; y++)
            {
                GenerateTile(x,y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x,y);

        foreach(ColorToPrefab colorToPrefab in colorToPrefabs)
        {
            if(colorToPrefab.color.Equals(pixelColor))
            {
                Vector2 position;
                position.x = (x - 12);
                position.y = (y - 16 + transform.position.y);

                GameObject prefab = Instantiate(colorToPrefab.prefab,position,Quaternion.identity,transform);

                if(prefab.transform.tag == "groundTile")
                {
                    prefab.transform.parent = ground.transform;
                }
                else if(prefab.transform.tag == "doorTile")
                {
                    prefab.transform.parent = doors.transform;
                }
            }
        }
    }
}
