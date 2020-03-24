using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfos : MonoBehaviour
{
    public static int room = 0;

    public static void setRoom (int aux, Transform t)
    {
        room += aux;
        SHASeeker.lastDoor = t.position;
    }
}
