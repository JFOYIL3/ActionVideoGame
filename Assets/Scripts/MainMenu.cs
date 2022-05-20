using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] Animator transition;

    [SerializeField] AudioManager audioManager;

    void Start(){
        audioManager.Play("Main Menu Music");
    }


    public void PlayGame(){
        StartCoroutine(LoadLevel());
    }

    public void QuitGame(){
        Application.Quit();
    }


    IEnumerator LoadLevel(){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level_1");
    }
}
