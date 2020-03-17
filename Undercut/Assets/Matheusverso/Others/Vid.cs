using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vid : MonoBehaviour
{ 

    // This script is only for the meme

    bool isPlaying = false;
    float duration;
    public GameObject vp;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && isPlaying == false) {
            vp.SetActive(true);
            isPlaying = true;
            duration = 5.0f;
        }
        if (duration <= 0 && isPlaying == true) {
            isPlaying = false;
            vp.SetActive(false);
        }
        duration -= Time.deltaTime;
    }
}
