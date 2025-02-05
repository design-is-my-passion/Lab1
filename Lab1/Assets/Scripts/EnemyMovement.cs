using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private float originalX;
    public float maxOffset = 5.0f;
    // private float enemyPatroltime = 2.0f;
    private float moveRight = -2.0f;
    private Vector2 velocity;
    private Vector3 initalPosition;

    private Rigidbody2D enemyBody;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        initalPosition = enemyBody.transform.position;
        // get the starting position
        originalX = initalPosition.x;
        ComputeVelocity();
    }
    void ComputeVelocity()
    {
        // velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
        velocity = new Vector2((moveRight), 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {// move goomba
            Movegoomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with goomba!");
        }
    }

    public void ResetPos()
    {
        enemyBody.transform.position = initalPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow - new Color(0, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(maxOffset*2 + 1, 1, 1));
    }
}