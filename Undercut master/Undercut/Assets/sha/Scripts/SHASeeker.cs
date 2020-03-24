using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHASeeker : MonoBehaviour
{
    // Go horse
    // Believe me, it's better don't understand this

    private int playerRoom;
    public static Vector3 lastDoor;
    public Transform player;

    private int old = 0;
    private bool key = false;

    void Update()
    {
        get_player_room();
        if(old != playerRoom && key == false)
        {
            key = true;
            manager();
        }
    }

    private void get_player_room()
    {
        playerRoom = PlayerInfos.room;
    }

    private void seekSoulsSeekTheKing()
    {
        transform.position = lastDoor;
        AstarPath.active.Scan();
        key = false;
        old = playerRoom;
    }

    private void manager()
    {
        Invoke("seekSoulsSeekTheKing", 3);
    }
}
