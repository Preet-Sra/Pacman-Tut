using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 direction;
    public Vector2 nextDirection;
    public Vector2 initialDirection;
    public Vector2 boxSixe;
    public float distance;
    public LayerMask obstacleLayer;
    public float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = initialDirection;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (direction * speed * Time.fixedDeltaTime));
        PathAvaialble(direction);
    }

    bool PathAvaialble(Vector2 _direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSixe, 0, _direction, distance, obstacleLayer);
        if (hit.collider)
        {

            DrawBox((Vector2)transform.position, boxSixe, Color.red);
        }
        else
        {
            DrawBox((Vector2)transform.position, boxSixe, Color.green);

        }
        return hit.collider;
    }


    public void SetDirection(Vector2 _direction)
    {
        if (!PathAvaialble(_direction))
        {
            direction = _direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = _direction;
        }
    }

    private void Update()
    {
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    void DrawBox(Vector2 pos, Vector2 size, Color col)
    {

        Vector2 topleft = pos + new Vector2(-size.x / 2, size.y / 2);
        Vector2 topRight = pos + new Vector2(size.x / 2, size.y / 2);
        Vector2 BottomRight = pos + new Vector2(size.x / 2, -size.y / 2);
        Vector2 BottomLeft = pos + new Vector2(-size.x / 2, -size.y / 2);

        Debug.DrawLine(topleft, topRight, col);
        Debug.DrawLine(topRight, BottomRight, col);
        Debug.DrawLine(BottomRight, BottomLeft, col);
        Debug.DrawLine(BottomLeft, topleft, col);

    }

}
