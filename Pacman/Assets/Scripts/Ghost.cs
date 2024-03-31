
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum GhostBehviours { Patrol,Chase,Scared,Home}
public class Ghost : MonoBehaviour
{
    CommonMovement movement;
    public GhostBehviours currentBehaviours = GhostBehviours.Patrol;
    private Transform target;
    public GameObject ScaredBody, Eyes, NormalBody;
    SpriteRenderer ScaredBodySprite;
    public Sprite BlueSprite, WhiteSprite;
    Coroutine changingColor;
    [SerializeField] Transform CenterHome, OutsideHome, LeftHome, RightHomem,InsideHome;
    Collider2D col;
    Pacman pacman;
    public float waitInHouseTime;
    ChangeBehvior changeBehvior;

    private void Start()
    {
        changeBehvior = GetComponent<ChangeBehvior>();
        pacman = GameObject.FindGameObjectWithTag("Player").GetComponent<Pacman>();
        col = GetComponent<Collider2D>();
        movement = GetComponent<CommonMovement>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        ScaredBody.SetActive(false);
        ScaredBodySprite = ScaredBody.GetComponent<SpriteRenderer>();

        if (currentBehaviours == GhostBehviours.Home)
        {
            col.enabled = false;
            movement.direction = Vector2.zero;
            movement.initialDirection = Vector2.zero;
            Invoke("StartExitingFromHome", waitInHouseTime);
            changeBehvior.enabled = false;
        }
        else
        {
            changeBehvior.ResetState();
        }
    }


    void StartExitingFromHome()
    {
        StartCoroutine(ExitHome());
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

            #region Scared

            if (currentBehaviours == GhostBehviours.Scared)
            {
                Vector2 _direction = Vector2.zero;
                float maxDistance = float.MinValue;

                foreach (Vector2 availDirection in node.availableDirecction)
                {
                    Vector3 newPosition = transform.position + new Vector3(availDirection.x, availDirection.y, 0);

                    float distance = (target.position - newPosition).sqrMagnitude;

                    if (distance > maxDistance  && availDirection!=-movement.direction )
                    {
                        _direction = availDirection;
                        maxDistance = distance;
                    }
                }
                movement.SetDirection(_direction);
            }

            #endregion

        }
    }

    public void ChangeIntoScaredMode()
    {
        currentBehaviours = GhostBehviours.Scared;
        Eyes.SetActive(false);
        NormalBody.SetActive(false);
        ScaredBody.SetActive(true);
    }

    public void TurnedOffScaredMode()
    {
        currentBehaviours = GhostBehviours.Patrol;
 
        if (changingColor != null)
        {
            StopCoroutine(changingColor);
        }
        changingColor = StartCoroutine(ChangingColorSequnece());
    }

    IEnumerator ChangingColorSequnece()
    {
        ScaredBodySprite.sprite = WhiteSprite;
        float interval = 0.1f;
        float iteration = Mathf.CeilToInt(3f/(2*interval));

        for(int i = 0; i < iteration; i++)
        {
            yield return new WaitForSeconds(interval);
            Debug.Log(i%2==0);
            ScaredBodySprite.sprite = (i % 2 == 0) ? BlueSprite : WhiteSprite;
        }

        ScaredBody.SetActive(false);
        NormalBody.SetActive(true);
        Eyes.SetActive(true);
    }

    public void TransferToHome()
    {
        currentBehaviours = GhostBehviours.Home;
        transform.position = new Vector3(InsideHome.position.x, InsideHome.position.y, transform.position.z);
        StartCoroutine(ExitHome());
    }

    IEnumerator ExitHome()
    {
        if (changingColor != null)
        {
            StopCoroutine(changingColor);
        }
        col.enabled = false;
        movement.direction = Vector2.zero;
        movement.nextDirection = Vector2.zero;

        while (pacman.IsInvincible())
        {
            yield return null;
        }

        float duration = 0.5f;
        float elapased = 0;
        while (elapased < duration)
        {
            transform.position = Vector3.Lerp(InsideHome.position,CenterHome.position,elapased/duration);
            elapased += Time.deltaTime;
            yield return null;
        }
        elapased = 0;
        while (elapased < duration)
        {
            transform.position = Vector3.Lerp(CenterHome.position, OutsideHome.position, elapased / duration);
            elapased += Time.deltaTime;
            yield return null;
        }

        currentBehaviours = GhostBehviours.Patrol;
        int randomnum = Random.Range(0, 2);
        if (randomnum == 0)
            movement.direction = Vector2.left;
        else
            movement.direction = Vector2.right;
        col.enabled = true;
        changeBehvior.enabled = true;
        changeBehvior.ResetState();
    }
}
