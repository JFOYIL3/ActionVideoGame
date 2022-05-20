using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipHit : MonoBehaviour
{
    [SerializeField] public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Flips Sprite depending on desired AIPath
        if(playerTransform.localScale.x > 0f){
            transform.localScale = new Vector3(1f,1f,1f);
        }else if(playerTransform.localScale.x < 0f){
            transform.localScale = new Vector3(-1f,1f,1f);
        }
    }
}
