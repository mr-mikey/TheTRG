using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLevel2 : MonoBehaviour {
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
    public AudioClip fall;
    public AudioClip coinSound;
    public AudioClip zombieDeath;
    public AudioSource source;
    private bool StartRun = false;
    public float distance = 0.001f;
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
                transform.position = startPosition;

        }
        if (other.CompareTag("Finish"))
        {
           
                //GameManager.instance.SumScore();
                GameManager.instance.LevelCompleted();
                
           
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
               
            }
        }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            distance = this.transform.position.x - startPosition.x;
            GameManager.instance.Distance.text = distance.ToString();
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartRun = true;
            }
            if( rigidBody.velocity.x < moveSpeed && StartRun)
            {
                rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
          
                    isWalking = true;
                
            }
            animator.SetBool("isGrounded", IsGrounded());
            animator.SetBool("isWalking", isWalking);
        }
        else
        {
            if (rigidBody.velocity.x > 0.01f)
                rigidBody.velocity = new Vector2(
                0.95f * rigidBody.velocity.x, rigidBody.velocity.y);
            else
            if (rigidBody.velocity.x < 0.01f)
                rigidBody.velocity = new Vector2(
                0.95f * rigidBody.velocity.x, rigidBody.velocity.y);
            else
                rigidBody.velocity = new Vector2(0f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {

            Jump();
        }
    }
}
