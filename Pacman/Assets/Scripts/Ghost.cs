
using UnityEngine;


public enum GhostBehviours { Patrol,Chase,Scared,Death}
public class Ghost : MonoBehaviour
{
    CommonMovement movement;
    public GhostBehviours currentBehaviours = GhostBehviours.Patrol;
    private Transform target;

    private void Start()
    {
        movement = GetComponent<CommonMovement>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Nodes node = collision.GetComponent<Nodes>();

        if (node != null)
        {

            #region Patrolling
            if (currentBehaviours == GhostBehviours.Patrol)
            {
                int RandomIndex = Random.Range(0, node.availableDirecction.Count);

                if (node.availableDirecction[RandomIndex] == -movement.direction)
                {
                    RandomIndex++;

                    if (RandomIndex >= node.availableDirecction.Count)
                    {
                        RandomIndex = 0;
                    }
                }
                movement.SetDirection(node.availableDirecction[RandomIndex]);
            }
            #endregion


            #region Chase

            if (currentBehaviours == GhostBehviours.Chase)
            {
                Vector2 _direction = Vector2.zero;
                float minDistance=float.MaxValue;

                foreach(Vector2 availDirection in node.availableDirecction)
                {
                    Vector3 newPosition = transform.position + new Vector3(availDirection.x, availDirection.y, 0);

                    float distance = (target.position - newPosition).sqrMagnitude;

                    if (distance < minDistance)
                    {
                        _direction = availDirection;
                        minDistance = distance;
                    }
                }
                movement.SetDirection(_direction);
            }

            #endregion
        }
    }
}
