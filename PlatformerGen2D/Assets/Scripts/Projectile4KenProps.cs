using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Projectile4KenProps : MonoBehaviour
{
    public int damage = 15;
    public float speed = 1f;
    public GameObject player;
    public Text hpText;
    public Text gameOver;
    public GameObject restartGame;
    Rigidbody2D projectile;
    bool var;
    // Start is called before the first frame update
    void Start()
    {
        projectile = GetComponent<Rigidbody2D>();
        StartCoroutine(addVelocity());
    }

    // Update is called once per frame
    void Update()
    {
        var = player.GetComponent<Animator>().GetBool("grounded");
        if(player.GetComponent<Animator>().GetInteger("healthPoints")<=0)
        {
            StartCoroutine(waitJump());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == player.name)
        {
            Animator playerAnimator = player.GetComponent<Animator>();
            playerAnimator.SetBool("hurt", true);
            playerAnimator.SetInteger("healthPoints", playerAnimator.GetInteger("healthPoints") - damage);
            hpText.GetComponent<Text>().text = "HP: " + playerAnimator.GetInteger("healthPoints");
            if (playerAnimator.GetInteger("healthPoints") <= 0)
            {
                hpText.GetComponent<Text>().text = "HP: 0";
                gameOver.enabled = true;
                restartGame.SetActive(true);
                player.GetComponent<PlayerPlatformerController>().enabled = false;
                Time.timeScale = 0.5f;
            }
            Destroy(gameObject);
        }
    }

    public IEnumerator addVelocity()
    {
        //if(transform.forward == Vector3.forward)
        //{
        //    transform.Translate(Vector3.Lerp(transform.position, new Vector3(transform.position.x - 1, transform.position.y + 0.05f, 0), 0.01f));
        //}
        //else
        //{
        //    transform.Translate(Vector3.Lerp(transform.position, new Vector3(transform.position.x + 1, transform.position.y - 0.05f, 0), 0.01f));
        //}
        yield return new WaitForSeconds(0.05f);
        projectile.velocity = (player.transform.position - transform.position).normalized * speed;
        Destroy(gameObject, 5);
    }

    public IEnumerator waitJump()
    {
        yield return new WaitUntil(() => var == true);
        player.GetComponent<PlayerPlatformerController>().enabled = false;
        player.GetComponent<Animator>().enabled = false;
    }
}
