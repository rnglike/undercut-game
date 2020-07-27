using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Extras : MonoBehaviour
{
	public DateTime now;

	//Background Colors
	public Color morningColor;
	public Color noonColor;
	public Color afternoonColor;
	public Color nightColor;

	//Period Variables
	private int currentPeriod;
	private int lastPeriod;
	private GameObject[] allPeriods;

	public float transitionRate;							//Public -- Waiting for Game Designer

	//Supreme generic cheat power.
	private Camera cam;
	private Room room;
	private Building building;
	private Interface ui;
	private brain brain;
	private GameObject player;

	//Cheat Variables
	public string cheatVariable;							//VARIAVEL QUE GUARDA A PALAVRA CHAVE
	private float cheatDelay;								//TEMPO DE ESPERA ENTRE LETRAS
	private Dictionary<string, int> currentCheatLetter;		//POSIÇAO DAS LETRA ATUAIS RELACIONADAS A PALAVRAS

	//Important variables.
	private bool auto = true;
	public bool periodChecked;
	public float lastHour;


	void Awake()
	{
		//PERIOD PART ---

		// cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		// allPeriods = GameObject.FindGameObjectsWithTag("Period");
		// lastHour = float.NaN;

		// //CHEAT PART ---

		// currentCheatLetter = new Dictionary<string, int>();
		// ui = GetComponent<Interface>();

		player = GameObject.FindWithTag("Player");
	}

	void Update()
	{
		// Application.targetFrameRate = 60;

		// //PERIOD PART ---

		// now = DateTime.Now;

		// //CHEAT PART ---

		// // if(Cheat("devfps"))
		// // {
		// // 	ShowFps();
		// // }

		if(Cheat("nicole"))
		{
			GetComponent<brain>().ui.screens[0].transform.Find("Byte").Find("info").GetComponent<TextMeshProUGUI>().text = "<3";
		}

		if(Cheat("ganha"))
		{
			GetComponent<brain>().cheatWon = true;
		}

		if(Cheat("cleiton"))
		{
			player.GetComponent<PlayerItems>().cheat = !player.GetComponent<PlayerItems>().cheat;
		}
	}

	void FixedUpdate()
	{
		if(auto)
		{
			PeriodCheck(now);
		}
	}




	//FUNCTIONS

		//PERIOD PART ---

		void PeriodCheck(DateTime now)
		{
			if(periodChecked)
			{
				periodChecked = false;
				lastHour = now.Hour;
			}

			if(now.Hour != lastHour)
			{
				GetPeriodControl(false);

				//Period Changing (Day & Night Cycle)
				if((now.Hour > 5) && (now.Hour <= 10))
				{
					PeriodTransition(morningColor,"Morning");
					currentPeriod = 0;
				}
				else if((now.Hour > 10) && (now.Hour <= 14))
				{
					PeriodTransition(noonColor,"Noon");
					currentPeriod = 1;
				}
				else if((now.Hour > 14) && (now.Hour <= 17))
				{
					PeriodTransition(afternoonColor,"Afternoon");
					currentPeriod = 2;
				}
				else if((now.Hour > 17) || (now.Hour <= 5))
				{
					PeriodTransition(nightColor,"Night");
					currentPeriod = 3;
				}

				// if(currentPeriod < allPeriods.Length)
				// {
				// 	if(allPeriods[lastPeriod].transform.position.y <= cam.transform.position.y)
				// 	{
				// 		LeanTween.moveY(allPeriods[lastPeriod],cam.transform.position.y + 18,2).setEaseInOutBack();
				// 	}
				// 	if(allPeriods[lastPeriod].transform.position.y >= cam.transform.position.y)
				// 	{
				// 		LeanTween.moveY(allPeriods[currentPeriod],cam.transform.position.y - 18,2).setEaseInOutBack();
				// 	}
				// 	lastPeriod = currentPeriod;
				// }

				//For Time/Date Events (Special Dates, Times, etc...)


				GetPeriodControl(true);
			}
		}

		public void GetPeriodControl(bool state){auto = state;}

		//Change the period (background color) to any given Color. (can be called from other scripts)
		public void PeriodTransition(Color desiredColor, string periodName = "Not Available")
		{
			Debug.Log("checking...");

			if(cam.backgroundColor != desiredColor)
			{
				cam.backgroundColor = new Color
				(
					RGBcheck(cam.backgroundColor.r,desiredColor.r,transitionRate),
					RGBcheck(cam.backgroundColor.g,desiredColor.g,transitionRate),
					RGBcheck(cam.backgroundColor.b,desiredColor.b,transitionRate),
					cam.backgroundColor.a
				);
			}
			else
			{
				periodChecked = true;
				Debug.Log("Current period: " + periodName);
			}
		}

		//Change Red or Green or Blue value to other Red or Green or Blue value in a given rate and returns it.
		public float RGBcheck(float RGB1, float RGB2, float rate)
		{
			if(RGB1 != RGB2)
			{
				if(RGB1 > RGB2)
				{
					RGB1 -= rate;
					if(RGB1 <= RGB2)
					{
						RGB1 = RGB2;
					}
				}
				else if(RGB1 < RGB2)
				{
					RGB1 += rate;
					if(RGB1 >= RGB2)
					{
						RGB1 = RGB2;
					}
				}
			}

			return RGB1;
		}


		//CHEAT PART ---

		//Cheat Engine. (PROBABLY NEEDS SOME OPTIMIZATION (IT DOES)) -- WHENEVER I TYPE A CHEAT, IT WORKS BUT DROPS AROUND 20 FPS (MY PC)
		bool Cheat(string cheat)
		{
			if(Input.inputString.Length != 0)								//If a charater is pressed...
			{
				char letter = Input.inputString[0];								//...it stores that character...

				if(!currentCheatLetter.ContainsKey(cheat))						//..and adds all cheats to inspection.
				{
					currentCheatLetter.Add(cheat, 0);								//(if it doesn't exist already)
				}
				if(currentCheatLetter[cheat] != -1)								//If the cheat wasn't already eliminated from inspection...
				{
					if(letter == cheat[currentCheatLetter[cheat]])					//...AND THAT CHAR IS PART OF A CHEAT. [V]
					{
						cheatVariable += letter;										//It computes the char.
						currentCheatLetter[cheat]++;									//And it goes to the next cheat char.
						if(currentCheatLetter[cheat] >= 3)								//And if you got 3 correct ones:
						{
							Debug.Log("Cheat Found! [" + currentCheatLetter[cheat].ToString() + "/" + cheat.Length.ToString() + "]");
						}
						cheatDelay = 4 * (Time.deltaTime * 10);								//Now you have some time to press the next correct char.
					}
					else															//...AND THAT CHAR ISN'T PART OF A CHEAT. [X]
					{
						currentCheatLetter[cheat] = -1;									//The cheat is eliminated from inspection.
					}
				}
			}

			if(cheatDelay <= 0)													//If time's up...
			{
				cheatVariable = "";													//...your cheat attempt is resetted...
				currentCheatLetter = new Dictionary<string, int>();					//...and all cheats are removed from inspection.
			}
			else if(cheat == cheatVariable)											//If you found the cheat...
			{
				cheatDelay = 0;														//...the time is resetted...
				return true;														//...and the cheat is TRUE!
			}
			else
			{
				cheatDelay -= Time.deltaTime;										//Times is decreasing.
			}

			return false;														//While it's not true, it's false.
		}

		//Cheats

		// void ShowFps()
		// {
		// 	if(ui.fps.gameObject.activeSelf)
		// 	{
		// 		ui.fps.gameObject.SetActive(false);
		// 	}
		// 	else if(!ui.fps.gameObject.activeSelf)
		// 	{
		// 		ui.fps.gameObject.SetActive(true);
		// 	}
		// }

}