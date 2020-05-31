using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MVPIntro : MonoBehaviour
{
	void Start()
	{
		Invoke("GameScene",2f);
	}

	void GameScene()
	{
		SceneManager.LoadSceneAsync(1);
	}
}
