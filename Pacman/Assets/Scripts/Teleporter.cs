
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform TeleportPosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = new Vector3(TeleportPosition.position.x, collision.transform.position.y, collision.transform.position.z);
    }
}
