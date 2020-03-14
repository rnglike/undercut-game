using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatHelper : MonoBehaviour
{
	//Supreme spawn power.
	public GameObject[] stuffStuff;
	public AudioSource[] soundStuff;

	//Supreme generic power.
	private CameraControl cam;
	private Room room;
	private Door door;
	private RoomController building;
	private Interface ui;

	public int cheatDelay;								//TEMPO DE ESPERA ENTRE LETRAS
	public string cheatVariable;						//VARIAVEL QUE GUARDA A PALAVRA CHAVE
	private Dictionary<string, int> currentCheatLetter;	//POSIÇAO DAS LETRA ATUAIS RELACIONADAS A PALAVRAS

	void Awake()
	{
		currentCheatLetter = new Dictionary<string, int>();
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

		if(Cheat("wswsadadk "))
		{
			Debug.Log("Konami CODE");
		}
	}

	//Cheat Engine. (PROBABLY NEEDS SOME OPTIMIZATION (IT DOES)) -- WHENEVER I TYPE A CHEAT, IT WORKS BUT DROPS AROUND 20 FPS (MY PC)
	bool Cheat(string cheat)
	{
		if(Input.anyKeyDown)
		{
			if(!currentCheatLetter.ContainsKey(cheat))
			{
				currentCheatLetter.Add(cheat, 0);
			}
			foreach(char letter in Input.inputString)
			{
				if(letter == cheat[currentCheatLetter[cheat]])
				{
					cheatVariable += letter;
					currentCheatLetter[cheat]++;
					if(currentCheatLetter[cheat] >= 3)
					{
						Debug.Log("Cheat Found! [" + currentCheatLetter[cheat].ToString() + "/" + cheat.Length.ToString() + "]");
					}
					cheatDelay = 100;
				}
			}
		}
		if(cheat == cheatVariable)
		{
			cheatDelay = 0;
			return true;
		}
		if(cheatDelay == 0)
		{
			cheatVariable = "";
			currentCheatLetter = new Dictionary<string, int>();
		}
		else
		{
			cheatDelay--;
		}

		return false;
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
}
