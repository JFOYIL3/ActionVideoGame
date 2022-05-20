using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private Transform ballPosition;
    private Rigidbody2D _ball;
    public GameObject fire_ball;

    

    void Start()
    {
        _ball = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ballMovement();
    }

    void ballMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(fire_ball, ballPosition.position, ballPosition.rotation);
        }
    }
}