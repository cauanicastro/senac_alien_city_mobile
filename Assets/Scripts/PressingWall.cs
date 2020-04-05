using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressingWall : MonoBehaviour
{
    public float distance = 10f;
    public float time = 2f;
    public float delay = 2f;
    public bool isMoving;

    private float stepIncrement;
    private Vector3 direction;
    private BoxCollider boxCollider;

    private float playerY = 0;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (isMoving)
        {
            stepIncrement = distance / (time / Time.fixedDeltaTime);
            StartCoroutine(WallMovement());
        }
    }

    private void Move(Vector3 direction)
    {
        transform.Translate(direction * stepIncrement);
    }


    IEnumerator WallMovement()
    {
        float moved = 0f;
        direction = Vector3.right;
        while (true)
        {
            while (moved < distance)
            {
                moved += stepIncrement;
                Move(direction);
                yield return new WaitForFixedUpdate();
            }
            moved = 0f;
            direction = -direction;
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedGameObject = collision.gameObject;
        if (collidedGameObject.CompareTag("PressingWall")) return;

        bool isPlayer = collidedGameObject.CompareTag("Player");
        if (isPlayer)
        {
            playerY = collision.collider.bounds.center.y;
        }

        if (collidedGameObject.transform.parent && collidedGameObject.transform.parent.CompareTag("PressingWall"))
        {
            if (isPlayer)
            {
                collidedGameObject.transform.parent = null;
                GameManager.GetInstance().DealDamage(100);
            }
        }
        else
        {
            collidedGameObject.transform.parent = transform;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.transform.parent = null;
    }

    private void OnCollisionStay(Collision collision)
    {
        GameObject collidedGameObject = collision.gameObject;
        if (collidedGameObject.CompareTag("Player"))
        {
            if (collision.collider.bounds.center.y > playerY)
            {
                Vector3 playerPos = collidedGameObject.transform.position;
                collidedGameObject.transform.SetPositionAndRotation(new Vector3(playerPos.x, playerY, playerPos.z), Quaternion.identity);
            }
        }
    }


}

