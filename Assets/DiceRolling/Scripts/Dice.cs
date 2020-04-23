using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public Rigidbody rigidBody;
    Vector3 originalPosition;
    public bool rolled = false;
    public bool landed = false;

    public GameManager gameManager;
    
    void Start()
    {
        originalPosition = transform.position;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!rolled && !landed)
                RollDie();
            else
                ResetDie();
        }
        if (Input.GetKeyUp(KeyCode.R))
            Debug.Log(getDieValue());


        if(rolled && !landed && rigidBody.IsSleeping())
        {
            landed = true;
            rigidBody.useGravity = false;
        }
    }

    public void RollDie()
    {
        rolled = true;
        rigidBody.useGravity = true;
        rigidBody.AddTorque(Random.Range(0, 700), Random.Range(0, 700), Random.Range(0, 700));
    }

    public void ResetDie()
    {
        rolled = false;
        landed = false;
        transform.position = originalPosition;
        rigidBody.useGravity = false;
    }

    public int getDieValue()
    {
        int value = -1;
        Vector3 dieRotation = transform.eulerAngles;

        dieRotation = new Vector3(Mathf.RoundToInt(dieRotation.x), Mathf.RoundToInt(dieRotation.y), Mathf.RoundToInt(dieRotation.z));

        if (dieRotation.x == 180 && dieRotation.z == 270 ||
            dieRotation.x == 0 && dieRotation.z == 90
            || dieRotation.x == 180 && dieRotation.z == -90)
        {
            value = 5;
        }
        else if (dieRotation.x == 270)
        {
            value = 2;
        }
        else if (dieRotation.x == 180 && dieRotation.z == 0 ||
          dieRotation.x == 0 && dieRotation.z == 180)
        {
            value = 1;
        }
        else if (dieRotation.x == 180 && dieRotation.z == 180 ||
          dieRotation.x == 0 && dieRotation.z == 0)
        {
            value = 3;
        }
        else if (dieRotation.x == 90)
        {
            value = 4;
        }
        else if (dieRotation.x == 0 && dieRotation.z == 270 ||
          dieRotation.x == 180 && dieRotation.z == 90
          || dieRotation.x == 0 && dieRotation.z == -90)
        {
            value = 6;
        }

        return value;
    }
}
