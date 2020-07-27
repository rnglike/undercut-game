using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
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
    public GameObject bombs;

    public GameObject light;

    public Sprite[] groundVariants;

    private Dictionary<float,List<GameObject>> doorsDic;
    private Dictionary<float,List<GameObject>> buttonDic;

    private Dictionary<float,List<GameObject>> buttonSDic;
    private Dictionary<float,List<GameObject>> bombDic;

    public BuildingNew building;

    /*
        GROUND VARIANTS IDEAL ORDER:

        [0] BASE
        [1] MIDDLE
        [2] HORIZONTAL
        [3] VERTICAL
        [4] TOP_RIGHT_CORNER
        [5] TOP_LEFT_CORNER
        [6] BOTTOM_RIGHT_CORNER
        [7] BOTTOM_LEFT_CORNER
    */

    //Assets to Generate Level
    public Texture2D map;

    [SerializeField]
    public ColorToPrefab[] colorToPrefabs;

    void Awake()
    {
        room = GetComponent<Room>();
        doorsDic = new Dictionary<float,List<GameObject>>();
        buttonDic = new Dictionary<float,List<GameObject>>();

        buttonSDic = new Dictionary<float,List<GameObject>>();
        bombDic = new Dictionary<float,List<GameObject>>();
    }

    void Start()
    {
        map = room.ChooseMap(map);

        ground = transform.Find("Ground").gameObject;
        doors = transform.Find("Doors").gameObject;

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

        //Setting position of the tile.
        Vector2 position;
        position.x = (x - 12);
        position.y = (y - 16 + transform.position.y);

        if(pixelColor.a != 0)
        {
            Color pixelColor0 = new Color(pixelColor.r,pixelColor.g,pixelColor.b,1);

            foreach(ColorToPrefab colorToPrefab in colorToPrefabs)
            {
                if(colorToPrefab.color.Equals(pixelColor0))
                {
                    //Actually instantiating the tile.
                    GameObject prefabAux = Instantiate(colorToPrefab.prefab,position,Quaternion.identity,transform);

                    //Organizing the transforms.
                    if(prefabAux.transform.tag == "groundTile")
                    {
                        prefabAux.transform.parent = ground.transform;
                        prefabAux.GetComponent<SpriteRenderer>().sprite = SortGroundOut(x,y);
                    }
                    else if(prefabAux.transform.tag == "doorTile")
                    {
                        prefabAux.transform.parent = doors.transform;
                    }
                    else if((prefabAux.transform.tag == "activeDoor") || (prefabAux.transform.tag == "button"))
                    {
                        if(!doorsDic.ContainsKey(pixelColor.a))
                        {
                            List<GameObject> auxList = new List<GameObject>();  //?
                            List<GameObject> auxList2 = new List<GameObject>(); //?
                            doorsDic.Add(pixelColor.a,auxList);
                            buttonDic.Add(pixelColor.a,auxList2);
                        }

                        if(prefabAux.transform.tag == "activeDoor")
                        {
                            doorsDic[pixelColor.a].Add(prefabAux);
                        }
                        else if(prefabAux.transform.tag == "button")
                        {
                            buttonDic[pixelColor.a].Add(prefabAux);
                        }
                    }
                    else if((prefabAux.transform.tag == "buttonSpecial") || (prefabAux.transform.tag == "Bomb"))
                    {
                        if(prefabAux.transform.tag == "Bomb")
                        {
                            prefabAux.transform.parent = bombs.transform;
                        }
                        
                        if(!buttonSDic.ContainsKey(pixelColor.a))
                        {
                            List<GameObject> auxList = new List<GameObject>();  //?
                            List<GameObject> auxList2 = new List<GameObject>(); //?
                            buttonSDic.Add(pixelColor.a,auxList);
                            bombDic.Add(pixelColor.a,auxList2);
                        }

                        if(prefabAux.transform.tag == "buttonSpecial")
                        {
                            buttonSDic[pixelColor.a].Add(prefabAux);
                        }
                        else if(prefabAux.transform.tag == "Bomb")
                        {
                            bombDic[pixelColor.a].Add(prefabAux);
                        }
                    }

                    if(LastPixel(x,y))
                    {
                        foreach(KeyValuePair<float,List<GameObject>> buttonList in buttonDic)
                        {
                            foreach(GameObject button in buttonList.Value)
                            {
                                button.transform.GetChild(0).GetComponent<Pressurable>().doors = new List<GameObject>();
                                button.transform.GetChild(0).GetComponent<Pressurable>().doors.AddRange(doorsDic[buttonList.Key]);
                            }
                        }

                        foreach(KeyValuePair<float,List<GameObject>> bombList in bombDic)
                        {
                            foreach(GameObject bomb in bombList.Value)
                            {
                                bomb.transform.GetComponent<BombTrigger>().buttons = new List<GameObject>();
                                bomb.transform.GetComponent<BombTrigger>().buttons.AddRange(buttonSDic[bombList.Key]);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Vector2 p = new Vector2(x,y);

            if(CanLight(x,y))
            {
                Instantiate(light,position,Quaternion.identity,transform);
            }
        }
    }

    bool LastPixel(int x,int y)
    {
        if((x == (map.width - 4)) && (y == (map.height - 1)))
        {
            Debug.Log("hsgdh");
            return true;
        }

        return false;
    }

    Sprite SortGroundOut(int x, int y)
    {
        Sprite temp_theTile = groundVariants[0];
        Vector2 p = new Vector2(x,y);

        //Sort based on neighbours
        
        //0z
        if(
                ThereAreTiles(
                                1,1,1,
                                1,p,1,
                                1,1,1
                            )
            )
        {
            temp_theTile = groundVariants[1];
        }

        //----------------------------------------

            //1z
            else
            if(
                    ThereAreTiles(
                                    1,1,1,
                                    1,p,1,
                                    0,1,1
                                )
                )
            {
                temp_theTile = groundVariants[29];
            }
            //1z
            else
            if(
                    ThereAreTiles(
                                    1,1,1,
                                    1,p,1,
                                    1,1,0
                                )
                )
            {
                temp_theTile = groundVariants[30];
            }
            //1z
            else
            if(
                    ThereAreTiles(
                                    0,1,1,
                                    1,p,1,
                                    1,1,1
                                )
                )
            {
                temp_theTile = groundVariants[31];
            }
            //1z
            else
            if(
                    ThereAreTiles(
                                    1,1,0,
                                    1,p,1,
                                    1,1,1
                                )
                )
            {
                temp_theTile = groundVariants[32];
            }

        //----------------------------------------

            //3z
            else
            if(
                    ThereAreTiles(
                                    1,1,1,
                                    1,p,1,
                                    0,0,0
                                )
                )
            {
                temp_theTile = groundVariants[3];
            }
            //3z
            else
            if(
                    ThereAreTiles(
                                    0,0,0,
                                    1,p,1,
                                    1,1,1
                                )
                )
            {
                temp_theTile = groundVariants[6];
            }
            //3z
            else
            if(
                    ThereAreTiles(
                                    1,1,0,
                                    1,p,0,
                                    1,1,0
                                )
                )
            {
                temp_theTile = groundVariants[12];
            }
            //3z
            else
            if(
                    ThereAreTiles(
                                    0,1,1,
                                    0,p,1,
                                    0,1,1
                                )
                )
            {
                temp_theTile = groundVariants[15];
            }
            //3z
            else
            if(
                    ThereAreTiles(
                                    0,1,1,
                                    1,p,1,
                                    0,1,0
                                )
                )
            {
                temp_theTile = groundVariants[35];
            }
            //3z
            else
            if(
                    ThereAreTiles(
                                    1,1,0,
                                    1,p,1,
                                    0,1,0
                                )
                )
            {
                temp_theTile = groundVariants[36];
            }
            //3z
            else
            if(
                    ThereAreTiles(
                                    0,1,0,
                                    1,p,1,
                                    0,1,1
                                )
                )
            {
                temp_theTile = groundVariants[33];
            }
            //3z
            else
            if(
                    ThereAreTiles(
                                    0,1,0,
                                    1,p,1,
                                    1,1,0
                                )
                )
            {
                temp_theTile = groundVariants[34];
            }

        //----------------------------------------

            //4z
            else
            if(
                    ThereAreTiles(
                                    0,1,0,
                                    1,p,1,
                                    0,1,0
                                )
                )
            {
                temp_theTile = groundVariants[2];
            }

        //----------------------------------------

            //5z
            else
            if(
                    ThereAreTiles(
                                    0,1,0,
                                    1,p,1,
                                    0,0,0
                                )
                )
            {
                temp_theTile = groundVariants[5];
            }
            //5z
            else
            if(
                    ThereAreTiles(
                                    0,1,0,
                                    0,p,1,
                                    0,1,0
                                )
                )
            {
                temp_theTile = groundVariants[17];
            }
            //5z
            else
            if(
                    ThereAreTiles(
                                    0,0,0,
                                    1,p,1,
                                    0,1,0
                                )
                )
            {
                temp_theTile = groundVariants[8];
            }
            //5z
            else
            if(
                    ThereAreTiles(
                                    0,1,0,
                                    1,p,0,
                                    0,1,0
                                )
                )
            {
                temp_theTile = groundVariants[14];
            }
            //5z
            else
            if(
                    ThereAreTiles(

                                    0,0,0,
                                    1,p,0,
                                    1,1,0
                                )
                )
            {
                temp_theTile = groundVariants[21];
            }
            //5z
            else
            if(
                    ThereAreTiles(
                                    0,0,0,
                                    0,p,1,
                                    0,1,1
                                )
                )
            {
                temp_theTile = groundVariants[23];
            }
            //5z
            else
            if(
                    ThereAreTiles(
                                    1,1,0,
                                    1,p,0,
                                    0,0,0
                                )
                )
            {
                temp_theTile = groundVariants[25];
            }
            //5z
            else
            if(
                    ThereAreTiles(
                                    0,1,1,
                                    0,p,1,
                                    0,0,0
                                )
                )
            {
                temp_theTile = groundVariants[27];
            }

        //----------------------------------------

            //6z
            else
            if(
                    ThereAreTiles(
                                    0,0,0,
                                    1,p,1,
                                    0,0,0
                                )
                )
            {
                temp_theTile = groundVariants[9];
            }       
            //6z
            else
            if(
                    ThereAreTiles(
                                    0,1,0,
                                    0,p,0,
                                    0,1,0
                                )
                )
            {
                temp_theTile = groundVariants[18];
            }
            //6z
            else
            if(
                    ThereAreTiles(

                                    0,0,0,
                                    1,p,0,
                                    0,1,0
                                )
                )
            {
                temp_theTile = groundVariants[22];
            }
            //6z
            else
            if(
                    ThereAreTiles(
                                    0,0,0,
                                    0,p,1,
                                    0,1,0
                                )
                )
            {
                temp_theTile = groundVariants[24];
            }
            //6z
            else
            if(
                    ThereAreTiles(
                                    0,1,0,
                                    1,p,0,
                                    0,0,0
                                )
                )
            {
                temp_theTile = groundVariants[26];
            }
            //6z
            else
            if(
                    ThereAreTiles(
                                    0,1,0,
                                    0,p,1,
                                    0,0,0
                                )
                )
            {
                temp_theTile = groundVariants[28];
            }

        //----------------------------------------

            //5z
            else
            if(
                    ThereAreTiles(
                                    1,1,1,
                                    0,p,0,
                                    0,0,0
                                )
                )
            {
                temp_theTile = groundVariants[4];
            }
            //5z
            else
            if(
                    ThereAreTiles(
                                    0,0,1,
                                    0,p,1,
                                    0,0,1
                                )
                )
            {
                temp_theTile = groundVariants[16];
            }
            //5z
            else
            if(
                    ThereAreTiles(
                                    0,0,0,
                                    0,p,0,
                                    1,1,1
                                )
                )
            {
                temp_theTile = groundVariants[7];
            }
            //5z
            else
            if(
                    ThereAreTiles(
                                    1,0,0,
                                    1,p,0,
                                    1,0,0
                                )
                )
            {
                temp_theTile = groundVariants[13];
            }
            
        //----------------------------------------
    
            //7z
            else
            if(
                    ThereAreTiles(
                                    0,1,0,
                                    0,p,0,
                                    0,0,0
                                )
                )
            {
                temp_theTile = groundVariants[20];
            }
            //7z
            else
            if(
                    ThereAreTiles(
                                    0,0,0,
                                    1,p,0,
                                    0,0,0
                                )
                )
            {
                temp_theTile = groundVariants[11];
            }
            //7z
            else
            if(
                    ThereAreTiles(
                                    0,0,0,
                                    0,p,0,
                                    0,1,0
                                )
                )
            {
                temp_theTile = groundVariants[19];
            }
            //7z
            else
            if(
                    ThereAreTiles(
                                    0,0,0,
                                    0,p,1,
                                    0,0,0
                                )
                )
            {
                temp_theTile = groundVariants[10];
            }
        


        return temp_theTile;
    }

    bool ThereAreTiles(
                                    /*1*/     /*2*/   /*3*/
                            /*a*/   int a1,   int a2,  int a3,
                            /*b*/   int b1,Vector2 pos,int b3,
                            /*c*/   int c1,   int c2,  int c3
                            )
    {
        int[] aux_a = {a1,a2,a3};
        int[] aux_b = {b1,0 ,b3};
        int[] aux_c = {c1,c2,c3};

        int[][] aux = {aux_a,aux_b,aux_c};

        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if(aux[i][j] == 1)
                {
                    int temp_i = i;
                    int temp_j = j;
                
                    if(i < 1)
                    {
                        i = 1;
                    }
                    else if(i == 1)
                    {
                        i = 0;
                    }
                    else
                    {
                        i = -1;
                    }

                    if(j < 1)
                    {
                        j = -1;
                    }
                    else if(j == 1)
                    {
                        j = 0;
                    }
                    else
                    {
                        j = 1;
                    }

                    if(((pos.x + j) < map.width) && ((pos.y + i) < map.height))
                    {
                        if(map.GetPixel(((int)pos.x + j),((int)pos.y + i)) != colorToPrefabs[0].color)
                        {
                            return false;
                        }
                    }

                    i = temp_i;
                    j = temp_j;
                }
                // else if(aux[i][j] == 0)
                // {
                //     int temp_i = i;
                //     int temp_j = j;
                
                //     if(i < 1)
                //     {
                //         i = -1;
                //     }
                //     else if(j == 1)
                //     {
                //         j = 0;
                //     }
                //     else
                //     {
                //         i = 1;
                //     }

                //     if(j < 1)
                //     {
                //         j = -1;
                //     }
                //     else if(j == 1)
                //     {
                //         j = 0;
                //     }
                //     else
                //     {
                //         j = 1;
                //     }

                //     if(((pos.x + j) < map.width) && ((pos.y + i) < map.height))
                //     {
                //         if(map.GetPixel(((int)pos.x + j),((int)pos.y + i)) == colorToPrefabs[0].color)
                //         {
                //             return false;
                //         }
                //     }

                //     i = temp_i;
                //     j = temp_j;
                // }
            }
        }

        return true;
    }

    bool CanLight(int x, int y)
    {
        for(int i = 1; i <= 3; i++)
        {
            if((x % 3) == 0)
            {
                if((y + 1) < map.height - 1)
                {
                    if((map.GetPixel(x,(y + 1)) != colorToPrefabs[0].color))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}