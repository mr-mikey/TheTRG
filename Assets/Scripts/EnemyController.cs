using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    private bool isWalking = false;
    private bool isFacingRight = true;
    public float XMin = 1.5f;
    public float XMax = 1.5f;
    public float moveSpeed = 2f;
    public Animator animator;
    private Rigidbody2D rigidBody;
    private bool isMovingRight = true;
    private float startPositionX;
  
    private bool isDead;
    private float killOffset = 1f;

    void Awake()
    {
        startPositionX = this.transform.position.x;
        if (isDead != true)
        {
            this.transform.position = new Vector2(Random.Range(startPositionX - XMin, startPositionX + XMax), this.transform.position.y);
        }
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void MoveRight()
    {
        if (rigidBody.velocity.x < moveSpeed)
        {
            rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.right * 0.6f, ForceMode2D.Impulse);
        }
    }
    void MoveLeft()
    {
        if (rigidBody.velocity.x > -moveSpeed)
        {
            rigidBody.velocity = new Vector2(-moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.left * 0.6f, ForceMode2D.Impulse);
        }
    }
    void Flip()
    {
        
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        isFacingRight = !isFacingRight;

    }
    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
        GameManager.instance.zombieDied();


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.transform.position.y > this.transform.position.y + killOffset)
            {
                isDead = true;

                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
                
            }
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isDead)
        {
            rigidBody.velocity = Vector2.zero;
        }
        if (isMovingRight )
        {
            if (isDead != true)
            {
                if (this.transform.localPosition.x < startPositionX + XMax)
                {
                    MoveRight();
                }
                else
                {
                    isMovingRight = false;
                    MoveLeft();
                    Flip();
                }
            }
        }
        else
        {
            if (isDead != true)
            {
                if (this.transform.localPosition.x > startPositionX - XMin)
                {
                    MoveLeft();
                }
                else
                {
                    isMovingRight = true;
                    MoveRight();
                    Flip();
                }
            }
    }
        
        
    }
}
