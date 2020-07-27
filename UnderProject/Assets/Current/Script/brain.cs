using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class brain : MonoBehaviour
{
    public TextMeshProUGUI status;
    public Color thatKindaYellowColor;

    public GameObject winningstuff;
    public GameObject winningstuff2;
    public bool timerPaused;
    public float transVol;
    public AudioSource theme;
    public bool themeplaying;

    public float escapeTime;
    public bool escaping;
    public GameObject escape;
    public GameObject tutorial;
    public float winningaux;

    public TextMeshProUGUI thighlight;
    public TextMeshProUGUI ttext;

    public GameObject fachada;
    public float timer;
    public float bonusTime;
    public float inTimerBack;

    public bool menu;
    public bool ToMM;
    public bool buildingInPos;
    public bool lockPlayerOnce;
    public bool launch;
    public bool fade;
    public bool over;
    public bool once;
    public bool winningScene;
    public bool cheatWon;

    public float closedAmount;

    public Transform camera;
    public Transform defeatScreen;
    public Interface ui;
    public Vector3 playerBack;

    public PlayerController player;

    public GameObject inputTxt;
    public GameObject mainMenuTxt;
    public int controller;
    public GameObject[] selections;

    CameraZoom cameraZoom;
    public CameraOnPlayer cameraPlayer;
    public Sumario sumario;

    public BuildingNew building;

    void Start()
    {
        ui = camera.Find("Canvas").GetComponent<Interface>();

        cameraZoom = camera.GetComponent<CameraZoom>();
        cameraPlayer = camera.GetComponent<CameraOnPlayer>();

        playerBack = player.transform.position;

        closedAmount = 2f;
    }

    void Update()
    {
        if(IsFade("opened"))
        {
            if(!themeplaying)
            {
                themeplaying = true;
                theme.Play();
            }
        }
        else
        {
            themeplaying = false;
        }

        if(menu)
        {
            theme.volume = Mathf.MoveTowards(theme.volume,.25f,transVol);

            timer = 0;

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
                            lockPlayerOnce = false;
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

        if(menu || winningScene) fachada.SetActive(true);
        else fachada.SetActive(false);

        if(!winningScene)
        {
            if(over)
            {
                fade = true;                                            //Fade Out

                if(IsFade("closed"))                                    //If faded
                {
                    defeatScreen.gameObject.active = true;              //Show Defeat Screen
                    cameraZoom.Zoom(false);
                }

                if(!cameraPlayer.IsMoving())                            //If camera ended moving,
                {
                    InputActive(defeatScreen,true);                     //Show Input

                    if(Input.GetButtonDown("Select")) over = false;     //If Selected, switch to the next case
                }
            }
            else
            {
                InputActive(defeatScreen,false);                        //Hide Input

                defeatScreen.gameObject.active = false;                 //Hide Defeat Screen

                fade = false;                                           //Fade In
            }
        }

        if(building.IsReady || cheatWon)
        {
            if(once)
            {
                once = false;
                inTimerBack = timer;
                timer = 60;
            }

            if(building.won || timer <= 0)
            {
                if(building.won || cheatWon) player.GetComponent<Playerlifes>().lifes = 5;
                else player.GetComponent<Playerlifes>().PlayerTakeDamage(5);

                if(building.won)
                {
                    if(inTimerBack < sumario.inTimer || sumario.inTimer == 0)
                    {
                        sumario.inTimer = inTimerBack;
                    }
                    if(60 - timer < sumario.outTimer || sumario.outTimer == 0)
                    {
                        sumario.outTimer = 60 - timer;
                    }
                    sumario.kills = player.SumKills();
                    sumario.pKills += 1;
            
                    building.won = false;
                    cheatWon = false;

                    player.enabled = false;
                    player.GetComponent<PlayerItems>().SetHairCode(0);
                    player.GetComponent<Rigidbody2D>().simulated = false;

                    winningaux = (10 * player.transform.position).normalized.x;

                    player.transform.position = player.respawn.position;

                    winningScene = true;
                }

                building.MakeLevels();
                building.IsReady = false;

                timer = 0; 
                once = true;
            }
            else
            {
                status.color = Color.red;
                thighlight.color = Color.red;
                
                status.text = "Fuja!";

                if(!timerPaused || !winningScene) timer -= Time.deltaTime;
            }
        }
        else
        {
            if(!menu || winningScene)
            {
                if(building.won)
                {
                    building.won = false;

                    player.GetComponent<Playerlifes>().PlayerTakeDamage(5);

                    building.MakeLevels();

                    timer = 0;
                    once = true;
                }

                status.color = thatKindaYellowColor;
                thighlight.color = thatKindaYellowColor;

                status.text = "Plante as bombas no predio";

                if(!timerPaused || !winningScene) timer += Time.deltaTime;
            }
        }

        if(winningScene)
        {
            if(Input.GetButtonDown("Select"))
            {
                fade = true;
            }

            if(IsFade("closed"))
            {
                winningScene = false;
                menu = true;
                fade = false;
            }
        }

        if(!menu)
        {
            theme.volume = Mathf.MoveTowards(theme.volume,.75f,transVol);

            if(Input.GetKey("escape"))
            {
                escapeTime += Time.deltaTime;
                escape.GetComponent<TextMeshProUGUI>().text = "saindo em " + ((int)escapeTime).ToString() + "...";
                escape.SetActive(true);
                tutorial.SetActive(false);
            }
            else
            {
                escapeTime = 0;
                escape.SetActive(false);
                tutorial.SetActive(true);
            }

            if(escapeTime >= 3)
            {
                escaping = true;
            }
            if(escaping)
            {
                player.GetComponent<Playerlifes>().lifes = 5;

                player.enabled = false;
                player.GetComponent<Rigidbody2D>().simulated = false;

                player.transform.position = player.respawn.position;

                building.MakeLevels();
                building.IsReady = false;
                cheatWon = false;

                timer = 0; 
                once = true;

                fade = true;
                if(IsFade("closed"))
                {
                    winningScene = false;
                    menu = true;
                    fade = false;
                    escaping = false;
                }
            }
        }

        string auxTimer = ((int)timer/60).ToString() + ":" + ((int)timer%60);

        thighlight.text = auxTimer;
        ttext.text = auxTimer;
    }

    void FixedUpdate()
    {
        if(fade) closedAmount = Mathf.MoveTowards(closedAmount,2f,(4*Time.deltaTime));
        else closedAmount = Mathf.MoveTowards(closedAmount,0f,(4*Time.deltaTime));

        camera.Find("Canvas").Find(":o").GetComponent<Image>().material.SetFloat("Vector1_40C23361",closedAmount);

        if(menu)
        {
            cameraZoom.Zoom(false);
            cameraPlayer.enabled = false;
            camera.position = new Vector3(0,0,camera.position.z);

            Vector3 btp = building.transform.parent.position;
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
            building.transform.parent.position = new Vector3(temp_smoothNewPos,btp.y,btp.z);
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
                    // launch = true;

                    player.enabled = true;
                    player.GetComponent<Rigidbody2D>().simulated = true;
                }
            }

            ui.Put("HUD");
        }

        if((building.transform.parent.position.x == 25f) || (building.transform.parent.position.x == 0f))
        {
            buildingInPos = true;
        }
        else
        {
            buildingInPos = false;
        }

        if(winningScene)
        {
            cameraPlayer.enabled = false;

            winningstuff.SetActive(true);
            winningstuff2.SetActive(true);

            float aux1 = Mathf.MoveTowards(camera.transform.position.x,winningaux * 60,(10 * Time.deltaTime));
            Vector3 aux2 = new Vector3(aux1,camera.transform.position.y,camera.transform.position.z);
            camera.transform.position = aux2;
        }
        else
        {
            winningstuff.SetActive(false);
            winningstuff2.SetActive(false);
        }
    }

    void InputActive(Transform screen, bool boolean)
    {
        GameObject input = screen.Find("Input").gameObject;
        input.active = boolean;
    }

    public bool IsFade(string type = "none")
    {
        if(
            (closedAmount == 0f && type == "opened")        ||
            (closedAmount == 2f && type == "closed")        ||
            (((closedAmount == 0f) || (closedAmount == 2f)) && type == "none")
        ) return true;

        return false;
    }

    public void Over()
    {
        over = true;

        if(IsFade("closed"))
        {
            player.Reset();
            building.Reset();
        }
    }

    IEnumerator WaitForLaunch()
    {
        yield return new WaitForSeconds(2);

        launch = true;
    }
}