using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVPOutro : MonoBehaviour
{
	void Start()
	{
		Invoke("QuitGame",2f);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
