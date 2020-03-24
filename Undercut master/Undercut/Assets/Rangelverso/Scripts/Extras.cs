using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extras : MonoBehaviour
{
	public DateTime now;

	//Background Colors
	public Color morningColor;
	public Color noonColor;
	public Color afternoonColor;
	public Color nightColor;

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
	}

	void Update()
	{
		now = DateTime.Now;

		if(!auto)
		{
			PeriodCheck(now);
		}
	}

	void PeriodCheck(DateTime now)
	{
		if(periodChecked)
		{
			periodChecked = !periodChecked;
			lastHour = now.Hour;
		}

		if(now.Hour != lastHour)
		{
			GetPeriodControl(true);

			//Period Changing (Day & Night Cycle)
			if((now.Hour > 5) && (now.Hour <= 10))
			{
				PeriodTransition(morningColor,"Morning");
			}
			else if((now.Hour > 10) && (now.Hour <= 14))
			{
				PeriodTransition(noonColor,"Noon");
			}
			else if((now.Hour > 14) && (now.Hour <= 17))
			{
				PeriodTransition(afternoonColor,"Afternoon");
			}
			else if((now.Hour > 17) || (now.Hour <= 5))
			{
				PeriodTransition(nightColor,"Night");
			}

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