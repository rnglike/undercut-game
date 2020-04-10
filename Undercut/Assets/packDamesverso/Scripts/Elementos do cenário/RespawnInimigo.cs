using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnInimigo : MonoBehaviour
{
    public Transform Spawnpoint;
    public GameObject Enemy;
    public GameObject RespawnInimigoTrigger;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("FOGUEIRA");
            //Enemy.GetComponent<EnemyHealth>().limpaCorpos();
            Instantiate(Enemy, Spawnpoint.position, Spawnpoint.rotation);
            RespawnInimigoTrigger.SetActive(false);


        }
    }
    /*void limpaCorpos()
    {
        enemy1 = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
        destroy(gameObject.enemy1);
    }*/

}
