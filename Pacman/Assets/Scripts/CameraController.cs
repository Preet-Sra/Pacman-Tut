using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject map;
    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D boxcollider = map.GetComponent<BoxCollider2D>();

        float mapWidth = boxcollider.size.x;
        float mapHeight = boxcollider.size.y;

        float screenAspect = Screen.width / Screen.height;
        float targetAspect = mapWidth / mapHeight;


        if (screenAspect >= targetAspect)
        {
            Camera.main.orthographicSize = (mapHeight+offset) / 2f;
        }
        else
        {
            float targetwidth = (mapHeight+offset) * screenAspect;
            Camera.main.orthographicSize = targetwidth / (2 * targetAspect);
        }

        float yoffset = Camera.main.orthographicSize - mapHeight / 2f;
        transform.position = new Vector3(map.transform.position.x, map.transform.position.y + yoffset, transform.position.z);
    }

    
}
