using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FallingStalagmite : MonoBehaviour
{
   
    public float triggerDistance = 5f;
    private Rigidbody bodyController;
    private GameManager gm;
    private bool fell = false;
    // Start is called before the first frame update
    void Start()
    {
        bodyController = GetComponent<Rigidbody>();
        gm = GameManager.GetInstance();
    }

    void Update()
    {
        if (Vector3.Distance(gm.playerPos.position, transform.position) < triggerDistance)
        {
            Fall();
        }
    }

    private void Fall()
    {
        bodyController.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ceiling")) return;

        if (collision.gameObject.CompareTag("Player") && !fell)
        {
            GameManager.GetInstance().DealDamage(50);
        } 
        else if (!fell)
        {
            fell = true;
        }
    }
}
