using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    // Change to private this entire "block", after test
    public static int hairCode;
    public int weaponCode;
    public int howManyHairs = 10;
    public int howManyWeapons = 10;

    // Item drop game object
    public GameObject dropper;

    //rb
    private Rigidbody2D rb;

    //Não jogue o item fora
    public bool dropDelay;

    /* Tabela
        0 - Default
        1 - zawarudo
        2 - double jump
        3 - quebra parede
        4 - wall jump
        5 - Super Saibaman
        6 - Helicop
    */ 
    

    //Novas
    private PlayerController playerController = null; //GetComponent<PlayerController>();

    private void Awake() {
        playerController = GetComponent<PlayerController>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Drop the items
        if (Input.GetKey("n") && weaponCode > 0)
        {
            dropper.GetComponent<ItemDrop>().Dropper(weaponCode, false);
            SetWeaponCode(0);
        }
        if (Input.GetKey(KeyCode.DownArrow) && hairCode > 0 && !dropDelay)
        {
            dropper.GetComponent<ItemDrop>().Dropper(hairCode, true);
            SetHairCode(0);
        }
    }


    // Rules to set the hair
    public void SetHairCode(int newHair)
    {
        if (newHair >= 0 && newHair < howManyHairs)
        {
            hairCode = newHair;
            newHairSet(hairCode);
            StartCoroutine(Wait());
        }
    }

    public int GetHairCode()
    {
        return hairCode;
    }


    // Rules to set the weapon
    public void SetWeaponCode(int newWeapon)
    {
        if (newWeapon >= 0 && newWeapon < howManyWeapons)
        {
            weaponCode = newWeapon;
        }
    }

    public int GetWeaponCode()
    {
        return weaponCode;
    }

    private void newHairSet(int code)
    {
        // Default for all states, don't forget to update this.
        playerController.defaultJumps = 1;
        playerController.checkWidth = 0.7f;
        rb.gravityScale = 5.0f;

        // The World
        if (code == 1)
        {
            // The World is in each dynamic object
        }
        // Double jump, or more
        else if (code == 2)
        {
            playerController.defaultJumps = 2;
            //rb.gravityScale = 4.5f;
        }
        // Topete
        else if (code == 3)
        {
            // The code for this power is basicly in the breakable walls
        }
        // Wall jump
        else if (code == 4)
        {
            //playerController.checkWidth = 0.9f; // old school
        }
        else if (code ==  5)
        {
            // Super Saibaman apenas abilita o ataque desgraçado
        }
        else if (code == 6)
        {
            // Helicopter do nothing
        }
    }

    private IEnumerator Wait()
    {
        dropDelay = true;
        yield return new WaitForSeconds(0.5f);
        dropDelay = false;
    }
}