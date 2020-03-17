using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatHelper : MonoBehaviour
{
	//Supreme spawn power.
	public GameObject[] stuffStuff;
	public AudioSource[] soundStuff;

	//Supreme generic power.
	private Camera cam;
	private Room room;
	private Building building;
	private Interface ui;

	public int cheatDelay;									//TEMPO DE ESPERA ENTRE LETRAS
	public string cheatVariable;							//VARIAVEL QUE GUARDA A PALAVRA CHAVE
	private Dictionary<string, int> currentCheatLetter;		//POSIÇAO DAS LETRA ATUAIS RELACIONADAS A PALAVRAS

	void Awake()
	{
		currentCheatLetter = new Dictionary<string, int>();
		ui = GameObject.Find("Canvas").GetComponent<Interface>();
		/*cam = GetComponent<CameraControl>();
		room = ;
		door = ;
		building = ;
		interface = ;*/
	}

	void Update()
	{
		//Cheat section to warm our hearts. (WHEEEEEEEE)
		if(Cheat("lyzandra"))
		{
			Caldeira();
		}

		if(Cheat("motherlode"))
		{
			Money();
		}

		if(Cheat("devfps"))
		{
			ShowFps();
		}

		if(Cheat("wswsadadk "))
		{
			Debug.Log("Konami CODE");
		}
	}

	//Cheat Engine. (PROBABLY NEEDS SOME OPTIMIZATION (IT DOES)) -- WHENEVER I TYPE A CHEAT, IT WORKS BUT DROPS AROUND 20 FPS (MY PC)
	bool Cheat(string cheat)
	{
		if(Input.anyKeyDown)													//If you pressed something...
		{
			if(!currentCheatLetter.ContainsKey(cheat))							//..it adds all cheats to inspection.
			{
				currentCheatLetter.Add(cheat, 0);								//(if it doesn't exist already)
			}
			if(currentCheatLetter[cheat] != -1)									//If the cheat wasn't eliminated from inspection...
			{
				foreach(char letter in Input.inputString)						//...if you pressed a char... [?]
				{
					if(letter == cheat[currentCheatLetter[cheat]])				//...AND THAT CHAR IS PART OF A CHEAT. [V]
					{
						if(cheatVariable.Length == currentCheatLetter[cheat])	//If your attempt is a char of distance from the cheat word...
						{
							cheatVariable += letter;							//(that means your char wasn't computed yet, so it computes)
						}
						currentCheatLetter[cheat]++;							//And it goes to the next cheat char.
						if(currentCheatLetter[cheat] >= 3)						//But if you got 3 correct ones:
						{
							Debug.Log("Cheat Found! [" + currentCheatLetter[cheat].ToString() + "/" + cheat.Length.ToString() + "]");
						}
						cheatDelay = 100;										//Now you have some time to press the next correct char.
					}
					else														//...AND THAT CHAR IN'T PART OF A CHEAT. [X]
					{
						currentCheatLetter[cheat] = -1;							//The cheat is eliminated from inspection.
					}

					break;				//Gambiarra (sem isso, se vc apertar dois char juntos, dá um erro esquisito)
				
				}
			}
		}
		if(cheat == cheatVariable)												//If you found the cheat...
		{
			cheatDelay = 0;														//...the time is resetted...
			return true;														//...and the cheat is TRUE!
		}
		if(cheatDelay == 0)														//If time's up...
		{
			cheatVariable = "";													//...your cheat attempt is resetted...
			currentCheatLetter = new Dictionary<string, int>();					//...and all cheats are removed from inspection.
		}
		else
		{
			cheatDelay--;														//Times is decreasing.
		}

		return false;															//While it's not true, it's false.
	}

	//The Cheats:

		//Lyzandra's Birthday.
		void Caldeira()
		{
			Debug.Log("ESSE COMANDO VAI FAZER UMA SURPRESA PRA LYZ");
		}

		//Accurate aproximation of Dolar <-> Real conversion.
		void Money()
		{
			/*Instantiate(stuffStuff[])*/
		}

		void ShowFps()
		{
			if(ui.fps.gameObject.activeSelf)
			{
				ui.fps.gameObject.SetActive(false);
			}
			else if(!ui.fps.gameObject.activeSelf)
			{
				ui.fps.gameObject.SetActive(true);
			}
		}
}
