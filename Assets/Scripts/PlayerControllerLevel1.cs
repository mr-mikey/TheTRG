using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLevel1 : MonoBehaviour {
    public Transform CameraMain;
    public Transform Player;
    private bool isWalking = false;
    public Animator animator;
    public LayerMask groundLayer;
    public float moveSpeed = 0.1f;
    public float jumpForce = 6f;
    private Rigidbody2D rigidBody;
    private float killOffset = 1f;
    private Vector2 startPosition;
    public AudioClip coinSound;
    public AudioClip zombieDeath;
    public AudioClip fall;
    public AudioSource source;
    public int keys = 0;
    public int maxKeys = 3;
    public bool finish = false;
    private bool isFacingRight = true;
    // Use this for initialization
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
        source = GetComponent<AudioSource>();
    }
   bool IsGrounded()
    {
        //return Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, groundLayer.value);
        return Physics2D.BoxCast(transform.position, new Vector2(0.86f, 0.001f), 0, Vector2.down, 1.5f, groundLayer.value);
        
    }
    void Jump()
    {
        
        if (IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
        }
    }

    void Start () {
		
	}
	void Flip ()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        isFacingRight = !isFacingRight;
    }
    void onTriggerStay2d(Collider2D other)
    {
        if (other.CompareTag("Moving Platform"))
        {
            rigidBody.isKinematic = true;
            transform.parent = other.transform.parent;
        }
    }
    void Unlock()
    {
        rigidBody.isKinematic = false;
        transform.parent = null;

    }
     void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Moving Platform"))
        {
            Unlock();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.instance.addCoins();
            source.PlayOneShot(coinSound, AudioListener.volume);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Key"))
        {
            keys += 1;
            GameManager.instance.addKeys();
            Destroy(other.gameObject);
            
        }
        if (other.CompareTag("Life"))
        {
            GameManager.instance.Hearts(true);
            Destroy(other.gameObject);

        }
        if (other.CompareTag("Respawn"))
        {
            source.PlayOneShot(fall, AudioListener.volume);
            animator.SetBool("isDead", true);
            
            GameManager.instance.Hearts(false);
            if (GameManager.instance.currentGameState == GameState.GS_GAME)
            transform.position = new Vector3(-7.79f, -1.56f, 0);

        }
        if (other.CompareTag("Finish") && (!finish))
        {
            if (keys == maxKeys)
            {
               // GameManager.instance.SumScore();
                GameManager.instance.LevelCompleted();
                finish = true;
            }
            else
                Debug.Log("You've not enough keys");
        }
        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.transform.position.y + killOffset < this.transform.position.y)
            {
                source.PlayOneShot(zombieDeath, AudioListener.volume);
            }
            else
            {
                animator.SetBool("isDead", true);
                GameManager.instance.Hearts(false);
                if (GameManager.instance.currentGameState == GameState.GS_GAME)
                    this.transform.position = startPosition;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {


            isWalking = false;
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                isWalking = true;
                if (isFacingRight == false)
                    Flip();


            }


            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                isWalking = true;
                if (isFacingRight == true)
                    Flip();


            }

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {

                Jump();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (IsGrounded())
                {

                    moveSpeed = 6;
                    jumpForce = 9.5f;
                    //Debug.Log("Sprint started");

                }

            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {

                moveSpeed = 4;
                //Debug.Log("Sprint stopped");

            }
            if ((Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D)) && transform.parent!=null)
            {
                Unlock();
            }
            animator.SetBool("isGrounded", IsGrounded());
            animator.SetBool("isWalking", isWalking);
        }
    }
}

