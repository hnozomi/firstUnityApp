using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] LayerMask blockLayer;
    public enum DIRECTION_TYPE
    {
    STOP,RIGHT,LEFT,}

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;
    new Rigidbody2D rigidbody2D;
    float speed;
    Animator animator;
    float JumpPower = 400;
    bool isDead = false;

    //SE
    [SerializeField] AudioClip getItemSE;
    [SerializeField] AudioClip jumpSE;
    [SerializeField] AudioClip stampSE;
    AudioSource audioSource;



    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }


    private void Update()
    {
        if (isDead) {
            return;
        }
        float x = Input.GetAxis("Horizontal"); // 方向キーの取得
        animator.SetFloat("speed", Mathf.Abs(x));


        if (x == 0)
        {
            // 止まっている
            direction = DIRECTION_TYPE.STOP;
        }
        else if (x > 0)
        {
            //右へ
            direction = DIRECTION_TYPE.RIGHT;
        }
        else if (x < 0)
        {
            // 左へ
            direction = DIRECTION_TYPE.LEFT;
         }

        if (IsGround())
        {
            if (Input.GetKeyDown("space"))
            {
                Jump();
            } else
            {
                animator.SetBool("isJumping", false);
            }
            
          
        }
    }


    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        switch (direction)
        {
            case DIRECTION_TYPE.STOP:

                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT:

                speed = 3;
                transform.localScale = new Vector3(1,1,1);
                break;
            case DIRECTION_TYPE.LEFT:

                speed = -3;
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }

        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
    }


    void Jump()
    {
        
        //上に力を加える
        rigidbody2D.AddForce(Vector2.up * JumpPower);
        audioSource.PlayOneShot(jumpSE);

        animator.SetBool("isJumping", true);
    }

    bool IsGround()
    {
        //始点と終点を作成
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;
        Debug.DrawLine(leftStartPoint, endPoint);
        Debug.DrawLine(rightStartPoint, endPoint);
        return Physics2D.Linecast(leftStartPoint, endPoint, blockLayer) || Physics2D.Linecast(rightStartPoint, endPoint, blockLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
        {
            return;
        }

        if (collision.gameObject.tag == "Trap")
        {
            PlayerDeath();
        }
        if (collision.gameObject.tag == "Finish")
        {
            Debug.Log("クリア!!");
            gameManager.GameClear();
        }
        if (collision.gameObject.tag == "Item")
        {
            audioSource.PlayOneShot(getItemSE);
            Debug.Log("ア!!");
            collision.gameObject.GetComponent<ItemManager>().GetItem();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("イ!!");
            EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();

            if (this.transform.position.y + 0.2f > enemy.transform.position.y)
            {
                //上から
                //敵を削除
                audioSource.PlayOneShot(stampSE);

                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                Jump();
                enemy.DestoryEnemy();
            }
            else
            {  //横から
                PlayerDeath();
            }
        }
    }

    void PlayerDeath()
    {
        isDead = true;
        rigidbody2D.velocity = new Vector2(0, 0);
        rigidbody2D.AddForce(Vector2.up * JumpPower);
        animator.Play("PlayerDeathAnimation");
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        Destroy(boxCollider2D);
        gameManager.GameOver();


            }
}
