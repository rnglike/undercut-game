using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interface : MonoBehaviour
{
	[Serializable]
	public enum Modos
	{
		TS,
		MM,
		HUD
	}

	[SerializeField]
	public Modos modos;

	public GameObject[] screens;

	TextMeshProUGUI[] life;
	TextMeshProUGUI[] levelCurrent; 
	TextMeshProUGUI[] levelAmount;

	//References
		public BuildingNew building;
		public Playerlifes playerLifes;
		public brain brain;

	void Start()
	{
		life = new TextMeshProUGUI[2];
		levelCurrent = new TextMeshProUGUI[2];
		levelAmount = new TextMeshProUGUI[1];

		Transform lifeIndicator = screens[2].transform.Find("LifeIndicator");
		Transform andares = screens[2].transform.Find("Andares");

		life[0] = lifeIndicator.Find("Lifes").Find("LifesSombra").GetComponent<TextMeshProUGUI>();
		levelCurrent[0] = andares.Find("AndarAtual").Find("AndarHighlight").GetComponent<TextMeshProUGUI>();
		levelAmount[0] = andares.Find("AndaresTotais").GetComponent<TextMeshProUGUI>();

		life[1] = lifeIndicator.Find("Lifes").Find("LifeTxt").GetComponent<TextMeshProUGUI>();
		levelCurrent[1] = andares.Find("AndarAtual").Find("AndarTxt").GetComponent<TextMeshProUGUI>();
	}

	void Update()
	{
		if(brain.buildingInPos)	
		{
			if(modos == Modos.TS)
			{
				foreach(GameObject screen in screens)
				{
					if(screen != screens[0])
					{
						if(screen.activeSelf)
						{
							screen.active = false;
						}
					}
				}

				screens[0].active = true;
			}
			else if(modos == Modos.MM)
			{
				foreach(GameObject screen in screens)
				{
					if(screen != screens[1])
					{
						if(screen.activeSelf)
						{
							screen.active = false;
						}
					}
				}

				screens[1].active = true;
			}
			else if(modos == Modos.HUD)
			{
				foreach(GameObject screen in screens)
				{
					if(screen != screens[2])
					{
						if(screen.activeSelf)
						{
							screen.active = false;
						}
					}
				}

				screens[2].active = true;
			}
		}
		else
		{
			foreach(GameObject screen in screens)
			{
				if(screen.activeSelf)
				{
					screen.active = false;
				}
			}
		}

		//HUD
		foreach(TextMeshProUGUI TMP in life)
		{
			TMP.text = playerLifes.lifes.ToString();
		}

		foreach(TextMeshProUGUI TMP in levelCurrent)
		{
			string prefix = "";
			
			if(building.currentRoom < 10)
			{
				prefix = "0";
			}

			TMP.text = (prefix + (building.levelToMake - building.currentRoom).ToString());
		}

		foreach(TextMeshProUGUI TMP in levelAmount)
		{
			string prefix = "/";

			if(building.levelToMake < 10)
			{
				prefix = "/0";
			}

			TMP.text = (prefix + building.levelToMake.ToString());
		}
	}

	public void Put(string mode)
	{
		if(mode == "Title Screen")
		{
			modos = Modos.TS;
		}
		if(mode == "Main Menu")
		{
			modos = Modos.MM;
		}
		if(mode == "HUD")
		{
			modos = Modos.HUD;
		}
	}

	public bool IsActive(string mode)
	{
		if(mode == "Title Screen")
		{
			if(modos == Modos.TS)
			{
				return true;
			}
		}
		if(mode == "Main Menu")
		{
			if(modos == Modos.MM)
			{
				return true;
			}
		}
		if(mode == "HUD")
		{
			if(modos == Modos.HUD)
			{
				return true;
			}
		}

		return false;
	}
}