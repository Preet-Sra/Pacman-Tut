using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    CommonMovement movement;
    bool invincible;
    Ghost[] AllGhost;
    public float invincibleTime;
    Coroutine invincibleCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<CommonMovement>();
        AllGhost = FindObjectsOfType<Ghost>();
    }

    private void Update()
    {
       

       
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            movement.SetDirection(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            movement.SetDirection(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            movement.SetDirection(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            movement.SetDirection(Vector2.down);

        float movementangle = Mathf.Atan2(movement.direction.y ,movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(movementangle *Mathf.Rad2Deg,Vector3.forward);
        
    }


    

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
        
            collision.gameObject.SetActive(false);
            //increase ui
            if (GameObject.FindGameObjectWithTag("Food") == null && GameObject.FindGameObjectWithTag("SuperFood")==null)
                Time.timeScale = 0;

        }

        if (collision.CompareTag("SuperFood"))
        {
            
            collision.gameObject.SetActive(false);
            invincible = true;
            for(int i = 0; i < AllGhost.Length; i++)
            {
                AllGhost[i].ChangeIntoScaredMode();
            }
            if (invincibleCoroutine != null)
            {
                StopCoroutine(invincibleCoroutine);
            }

           invincibleCoroutine=  StartCoroutine(ChangeIntoNormalMode());
            if (GameObject.FindGameObjectWithTag("Food") == null && GameObject.FindGameObjectWithTag("SuperFood")==null)
                Time.timeScale = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            if (!invincible)
                gameObject.SetActive(false);
            else
                collision.gameObject.GetComponent<Ghost>().TransferToHome();
        }
    }

    IEnumerator ChangeIntoNormalMode()
    {
       yield  return new WaitForSeconds(invincibleTime);
        for (int i = 0; i < AllGhost.Length; i++)
        {
            AllGhost[i].TurnedOffScaredMode();
        }
        yield return new WaitForSeconds(3f);
        invincible = false;
    }

    public bool IsInvincible()
    {
        return invincible;
    }
}
