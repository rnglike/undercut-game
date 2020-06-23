using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interface : MonoBehaviour
{
	public TextMeshProUGUI[] life;
	public TextMeshProUGUI[] levelCurrent; 
	public TextMeshProUGUI[] levelAmount;

	public BuildingNew building;
	public Playerlifes playerLifes;

	void Update()
	{
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
}
