using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    public enum Opening
    {
        LEFT_TO_RIGHT,
        RIGHT_TO_LEFT,
        FRONT_TO_BACK,
        BACK_TO_FRONT,
        TOP_TO_BOTTOM,
        BOTTOM_TO_TOP
    }

    public enum ClosingMode
    {
        ON_PLAYER_TOUCH,
        AUTO
    }

    public Opening slide;
    public ClosingMode closingMode;

    [Range(0.1f, 5f)]
    public float time = 1f;
    [Range(1f, 500f)]
    public float distance = 10f;
    [Range(0.1f, 5f)]
    public float movementDelay = 0f;

    public bool shouldMoveThePlayer = true;

    private Vector3 direction;
    private float stepIncrement;

    // Start is called before the first frame update
    void Start()
    {
        direction = GetDirection();
        stepIncrement = GetStepIncrement();
        if (closingMode == ClosingMode.AUTO)
        {
            StartCoroutine(AutoMovement());
        }
    }

    private Vector3 GetDirection()
    {
        switch (slide)
        {
            case Opening.RIGHT_TO_LEFT:
                return Vector3.left;
            case Opening.LEFT_TO_RIGHT:
                return Vector3.right;
            case Opening.FRONT_TO_BACK:
                return Vector3.down;
            case Opening.BACK_TO_FRONT:
                return Vector3.up;
            case Opening.BOTTOM_TO_TOP:
                return Vector3.forward;

            case Opening.TOP_TO_BOTTOM:
            default:
                return Vector3.back;

        }
    }

    private float GetStepIncrement()
    {
        return distance / (time / Time.fixedDeltaTime);
    }

    //Quick and dirty
    private IEnumerator AutoMovement()
    {
        while (true)
        {
            float increment = 0;
            yield return new WaitForSeconds(movementDelay);

            while (increment < distance)
            {
                increment += stepIncrement;
                transform.Translate(direction * stepIncrement);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            yield return new WaitForSeconds(movementDelay);

            increment = 0;
            while (increment < distance)
            {
                increment += stepIncrement;
                transform.Translate(-direction * stepIncrement);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(movementDelay);
        float increment = 0;
        while (increment < distance)
        {
            increment += stepIncrement;
            transform.Translate(direction * stepIncrement);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (shouldMoveThePlayer)
            {
                other.gameObject.transform.parent = transform;
            }
            if (closingMode == ClosingMode.ON_PLAYER_TOUCH)
            {
                StartCoroutine(Move());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(shouldMoveThePlayer)
            {
                other.gameObject.transform.parent = null;
            }
        }
        
    }
}
