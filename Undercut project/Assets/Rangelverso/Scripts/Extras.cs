using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extras : MonoBehaviour
{
	public DateTime now;
	public float fake;

	//Background Colors
	public Color morningColor;
	public Color noonColor;
	public Color afternoonColor;
	public Color nightColor;

	private int currentPeriod;
	private int lastPeriod;
	private GameObject[] allPeriods;

	public float transitionRate;	//Public -- Waiting for Game Designer
	private float lastHour;
	private float RGBdone;
	private bool auto;
	private bool periodChecked;

	//Important Variables
	private Camera cam;

	void Awake()
	{
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		allPeriods = GameObject.FindGameObjectsWithTag("Period");
		lastHour = float.NaN;
	}

	void Update()
	{
		Application.targetFrameRate = 60;

		now = DateTime.Now;

		if(!auto)
		{
			PeriodCheck(fake);
		}
	}

	void PeriodCheck(float fake)
	{
		if(periodChecked)
		{
			periodChecked = !periodChecked;
			lastHour = fake;
		}

		if(fake != lastHour)
		{
			GetPeriodControl(true);

			//Period Changing (Day & Night Cycle)
			if((fake > 5) && (fake <= 10))
			{
				PeriodTransition(morningColor,"Morning");
				currentPeriod = 0;
			}
			else if((fake > 10) && (fake <= 14))
			{
				PeriodTransition(noonColor,"Noon");
				currentPeriod = 1;
			}
			else if((fake > 14) && (fake <= 17))
			{
				PeriodTransition(afternoonColor,"Afternoon");
				currentPeriod = 2;
			}
			else if((fake > 17) || (fake <= 5))
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


			GetPeriodControl(false);
		}
	}

	public void GetPeriodControl(bool state){auto = state;}

	//Change the period (background color) to any given Color. (can be called from other scripts)
	public void PeriodTransition(Color desiredColor, string periodName = "Not Available")
	{
		if(auto)
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
				if(RGBdone == 3)
				{
					RGBdone = 0;
					periodChecked = true;
					Debug.Log("Current period: " + periodName);
				}
			}
			else
			{
				periodChecked = true;
			}
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
					RGBdone++;
				}
			}
			else if(RGB1 < RGB2)
			{
				RGB1 += rate;
				if(RGB1 >= RGB2)
				{
					RGB1 = RGB2;
					RGBdone++;
				}
			}
		}

		return RGB1;
	}
}