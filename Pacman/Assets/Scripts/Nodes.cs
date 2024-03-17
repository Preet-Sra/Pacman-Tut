using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public List<Vector2> availableDirecction = new List<Vector2>();


    private void Start()
    {
        availableDirecction.Clear();
        CheckAvailablePaths(Vector2.up);
        CheckAvailablePaths(Vector2.down);
        CheckAvailablePaths(Vector2.left);
        CheckAvailablePaths(Vector2.right);


    }

    void CheckAvailablePaths(Vector2 direction)
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f,0f, direction,1f, obstacleLayer);

        if (hit2D.collider == null)
        {
            availableDirecction.Add(direction);
        }
    }
   
}
