using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class brain : MonoBehaviour
{
    public bool menu;
    public bool ToMM;
    public bool buildingInPos;
    public bool lockPlayerOnce;
    public bool launch;
    public bool firstTime;

    public float closedAmount;

    public Transform camera;
    Interface ui;

    public PlayerController player;

    public GameObject inputTxt;
    public GameObject mainMenuTxt;
    public int controller;
    public GameObject[] selections;

    CameraZoom cameraZoom;
    public CameraOnPlayer cameraPlayer;

    public GameObject bulding;

    void Start()
    {
        ui = camera.Find("Canvas").GetComponent<Interface>();

        cameraZoom = camera.GetComponent<CameraZoom>();
        cameraPlayer = camera.GetComponent<CameraOnPlayer>();

        closedAmount = 2f;
    }

    void Update()
    {
        if(menu)
        {
            if(!ToMM)
            {
                if(!mainMenuTxt.activeSelf)
                {
                    if(Input.anyKeyDown)
                    {
                        inputTxt.active = false;
                        mainMenuTxt.active = true;
                    }
                }
                else
                {
                    if(Input.GetKeyDown("up"))
                    {
                        if(controller > 0)
                        {
                            controller -= 1;
                        }
                    }
                    else if(Input.GetKeyDown("down"))
                    {
                        if(controller < 1)
                        {
                            controller += 1;
                        }
                    }

                    if(Input.GetButtonDown("Select"))
                    {
                        if(controller == 0)
                        {
                            menu = false;
                        }
                        else if(controller == 1)
                        {
                            ToMM = true;
                        }
                    }

                    foreach(GameObject selection in selections)
                    {
                        if(selection != selections[controller])
                        {
                            selection.active = false;
                        }

                        selections[controller].active = true;
                    }
                }
            }

            if(ToMM)
            {
                if(Input.GetKeyDown("v"))
                {
                    ToMM = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(firstTime) closedAmount = Mathf.MoveTowards(closedAmount,2f,(4*Time.deltaTime));
        else closedAmount = Mathf.MoveTowards(closedAmount,0f,(4*Time.deltaTime));

        camera.Find("Canvas").Find(":o").GetComponent<Image>().material.SetFloat("Vector1_40C23361",closedAmount);

        if(menu)
        {
            cameraZoom.Zoom(false);
            cameraPlayer.enabled = false;
            camera.position = new Vector3(0,0,camera.position.z);

            Vector3 btp = bulding.transform.position;
            float temp_oldPos = btp.x;
            float temp_newPos;

            player.GetComponent<Rigidbody2D>().simulated = false;
            player.enabled = false;
            
            if(ToMM)
            {
                temp_newPos = 25f;
                ui.Put("Main Menu");
            }
            else
            {
                temp_newPos = 0f;
                ui.Put("Title Screen");
            }

            float temp_smoothNewPos = Mathf.MoveTowards(temp_oldPos,temp_newPos,(80 * Time.deltaTime));
            bulding.transform.position = new Vector3(temp_smoothNewPos,btp.y,btp.z);
        }
        else
        {
            cameraZoom.Zoom(true);
            cameraPlayer.enabled = true;

            if(!lockPlayerOnce)
            {   
                if(!launch)
                {
                    StartCoroutine("WaitForLaunch");
                }

                if(player.transform.position.y != -12)
                {
                    if(launch)
                    {
                        float aux1 = Mathf.MoveTowards(player.transform.position.y,-12,(10 * Time.deltaTime));
                        Vector3 aux2 = new Vector3(player.transform.position.x,aux1,player.transform.position.z);
                        player.transform.position = aux2;
                    }
                }
                else
                {
                    lockPlayerOnce = true;
                    launch = true;

                    player.enabled = true;
                    player.GetComponent<Rigidbody2D>().simulated = true;
                }
            }

            ui.Put("HUD");
        }

        if((bulding.transform.position.x == 25f) || (bulding.transform.position.x == 0f))
        {
            buildingInPos = true;
        }
        else
        {
            buildingInPos = false;
        }
    }

    IEnumerator WaitForLaunch()
    {
        yield return new WaitForSeconds(2);

        launch = true;
    }
}