using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    // Box values
    public int item;
    public float ActionRange;
    public bool isHair; // Missing something to choose automatically


    // Variables to get the distance between box and player
    private GameObject onlyPlayer;
    private Vector3 playerPosition;
    private float distanceUntilPlayer;


    void Start()
    {
        // Getting the player object
        onlyPlayer = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        // Calculating the distance
        playerPosition = onlyPlayer.transform.position;
        Vector2 distanceVector = transform.localPosition - playerPosition;
        distanceUntilPlayer = distanceVector.magnitude;

        // Calling the method GiveItem
        if (distanceUntilPlayer <= ActionRange && Input.GetKeyDown("m"))
        {
            Debug.Log("Getting item");
            GiveItem(item);
        }
    }


    // Tell to the player change his weapon / hair
    private void GiveItem(int itemCode)
    {
        // Change player state
        if (isHair == true)
        {
            onlyPlayer.GetComponent<PlayerItems>().SetHairCode(itemCode);
        } else
        {
            onlyPlayer.GetComponent<PlayerItems>().SetWeaponCode(itemCode);
        }

        Destroy(gameObject);
    }


    // Draw the action range of the box
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.localPosition, ActionRange - 0.3f);
    }
}
