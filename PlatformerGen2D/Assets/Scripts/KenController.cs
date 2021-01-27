using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class KenController : MonoBehaviour
{
    const int STATE_IDLE = 0;
    const int STATE_WALK = 1;
    const int STATE_CROUCH = 2;
    const int STATE_JUMP = 3;
    const int STATE_HADOOKEN = 4;
    public float walkSpeed = 1f;
    Animator animator;
    public GameObject player;
    public GameObject tilemap;
    public GameObject projectile;
    public Text scoreText;
    public int score = 500;
    GameObject projectileClone;
    string _currentDirection = "left";
    int isLeft = 1;
    int justOnce = 1;
    public float offsetX = 0f;
    public float offsetY = 0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time;
        if (animator.GetInteger("state") != STATE_CROUCH 
            && t > 1.2f
            && !((t % 10 > 3f && t % 10 < 4f)
            || (t % 10 > 6f && t % 10 < 7f)
            || (t % 10 > 9f && t % 10 < 10f)))
        {
            animator.SetInteger("state", STATE_WALK);
            transform.Translate(Vector3.left * walkSpeed * Time.deltaTime);
        }
        if (animator.GetInteger("state") != STATE_CROUCH 
            && animator.GetInteger("state") != STATE_HADOOKEN 
            && ((t % 10 > 3f && t % 10 < 4f)
            || (t % 10 > 6f && t % 10 < 7f)
            || (t % 10 > 9f && t % 10 < 10f)))
        {
            animator.SetInteger("state", STATE_HADOOKEN);
            projectileClone = Instantiate(projectile,new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z), Quaternion.identity);
            projectileClone.SetActive(true);
        }
    }

    void changeDirection(string direction)
    {

        if (_currentDirection != direction)
        {
            if (direction == "right")
            {
                transform.Rotate(0, 180, 0);
                _currentDirection = "right";
            }
            else if (direction == "left")
            {
                transform.Rotate(0, -180, 0);
                _currentDirection = "left";
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D hit) 
    {
        if(hit.gameObject.name == player.name && justOnce == 1)
        {
            scoreText.GetComponent<Text>().text = (int.Parse(scoreText.text) + score).ToString();
            animator.SetInteger("state", STATE_CROUCH);
            justOnce = 0;
            this.GetComponents<CircleCollider2D>()[0].enabled = false;
            this.GetComponents<CircleCollider2D>()[1].enabled = false;
            StartCoroutine(waitasec());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == tilemap.name && isLeft == 1)
        {
            changeDirection("right");
            transform.Translate(Vector3.left * walkSpeed * Time.deltaTime);
            offsetX *= -1;
            isLeft = 0;
        }
        else if (collision.name == tilemap.name && isLeft == 0)
        {
            changeDirection("left");
            transform.Translate(Vector3.left * walkSpeed * Time.deltaTime);
            offsetX *= -1;
            isLeft = 1;
        }
    }

    public IEnumerator waitasec()
    {
        yield return new WaitForSeconds(1.0f);
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.GetComponent<CapsuleCollider2D>().enabled = false;
    }

}
