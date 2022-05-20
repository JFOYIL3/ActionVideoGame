using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{


    [SerializeField] Text pauseText;

    public static bool isPaused = false;

    

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape)){
           if (isPaused){
               Resume();
           }
           else{
               Pause();
           }
       } 
    }

    void Resume(){
        pauseText.gameObject.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause(){
        pauseText.gameObject.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}
