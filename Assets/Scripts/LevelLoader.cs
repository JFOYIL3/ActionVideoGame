using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] Animator transition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(1)){
        //    LoadLevel();
        //}
    }

    public void ReloadLevel(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }

    public void LoadMainMenu(){
        StartCoroutine(LoadLevel("Main_Menu"));
    }

    public void LoadNextLevel(){
        string path = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        StartCoroutine(LoadLevel(name.Substring(0, dot)));
        
    }

    
    IEnumerator LoadLevel(string name){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(name);
    }
}
