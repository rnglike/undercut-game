using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsPlayer : MonoBehaviour
{
    Transform player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.position = transform.position;
    }
}
