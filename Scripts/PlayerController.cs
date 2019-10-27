using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool facingRight;
    private int count;
    private int count2;
    private int lives = 3;

    public float speed;

    public Text countText;
    public Text count2Text;
    public Text livesText;
    public Text winText;
    public Text loseText;
    public Text retryText;

    public AudioSource _as;
    public AudioClip pickupSound;
    public AudioClip enemySound;
    public AudioClip winSound;
    
    Animator anim;

    void Start()
    {
        _as = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();



        facingRight = true;

        count = 0;
        count2 = 0;
        winText.text = "";
        loseText.text = "";
        retryText.text = "";
        SetCountText();
    }

    void Update()
    {
    
     //Animation

        if (Input.GetKey(KeyCode.D))
        {
            anim.SetInteger("State", 2);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            anim.SetInteger("State", 2);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }

        if ((Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W)))
        {
            anim.SetInteger("State", 3);
        }

        if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W)))
        {
            anim.SetInteger("State", 3);
        }
        
        if (lives < 1)
        {
            anim.SetInteger("State", 4);
        }


    //Quit or restart scene

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }

    //Movement

        float hozMovement = Input.GetAxis("Horizontal");
        //float vertMovement = Input.GetAxis("Vertical");
        rb2d.AddForce(new Vector2(hozMovement * speed, 0f * speed));

        if (hozMovement < 0 && facingRight)
        {
            flip();
        }

        else if (hozMovement > 0 && !facingRight)
        {
            flip();
        }

    }

    void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    //Jumping

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {

            anim.SetInteger("State", 0);

            if (Input.GetKey(KeyCode.W))
            {
                rb2d.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                anim.SetInteger("State", 3);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                rb2d.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                anim.SetInteger("State", 3);
            }

        }

        if (collision.collider.tag != "Ground")
        {
            anim.SetInteger("State", 3);
        }

    }

    //Pickups, enemies and text

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickups"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            _as.PlayOneShot(pickupSound);
        }

        if (other.gameObject.CompareTag("Pickups2"))
        {
            other.gameObject.SetActive(false);
            count2 = count2 + 1;
            SetCountText();

            if (count2 < 7)
            {
                _as.PlayOneShot(pickupSound);
            }
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            lives = lives - 1;
            SetCountText();
            _as.PlayOneShot(enemySound);
        }

        if (other.gameObject.CompareTag("Enemy2"))
        {
            other.gameObject.SetActive(false);
            lives = lives - 1;
            SetCountText();
            _as.PlayOneShot(enemySound);
        }

        if (other.gameObject.CompareTag("Restart"))
        {
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }

        if (other.gameObject.CompareTag("Restart2"))
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }

    }


    void SetCountText()
    {
        countText.text = "Gold: " + count.ToString();
        if (count >= 5)
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }

        count2Text.text = "Gems: " + count2.ToString();
        if (count2 >= 7)
        {
            winText.text = "Great job! Made by Christopher Gascoyne";
            _as.PlayOneShot(winSound);
        }

        livesText.text = "Lives: " + lives.ToString();
        if (lives < 1)
        {
            loseText.text = "...Yikes";
        }
        
        if (lives < 1)
        {
            retryText.text = "Press R to try again";
        }
       

    }

}
