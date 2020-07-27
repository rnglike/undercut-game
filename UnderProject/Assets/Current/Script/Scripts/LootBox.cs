using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LootBox : MonoBehaviour
{
    public float vel = 1f;
    public float limit;
    public Transform backPos;
    public bool active = true;

    // Box values
    public int item;
    public float ActionRange;
    public bool isHair; // Missing something to choose automatically
    public Sprite[] peruca;
    public Color[] color;
    public Light2D luz;
    public SpriteRenderer back;


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
        if(onlyPlayer.GetComponent<PlayerItems>().GetHairCode() == item) active = false;
        else active = true;

        GetComponent<SpriteRenderer>().enabled = active;

        // Calculating the distance
        playerPosition = onlyPlayer.transform.position;
        Vector2 distanceVector = transform.position - playerPosition;
        distanceUntilPlayer = distanceVector.magnitude;

        // Calling the method GiveItem
        if (distanceUntilPlayer <= ActionRange && active)
        {
            Debug.Log("Getting item");
            GiveItem(item);
        }

        GetComponent<SpriteRenderer>().sprite = peruca[item];
        luz.color = color[item];
        back.sprite = peruca[item];
    }

    void FixedUpdate()
    {
        float aux0 = Mathf.PingPong(Time.time,limit);
        Vector3 aux1 = new Vector3(backPos.position.x,backPos.position.y + aux0,backPos.position.z);
        transform.position = aux1;
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
    }


    // Draw the action range of the box
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ActionRange - 0.3f);
    }
}
