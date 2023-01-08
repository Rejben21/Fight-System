using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Rigidbody2D rgBody;
    public Animator anim;

    public float moveSpeed = 5, jumpforce = 10;
    public float attackForce;

    private bool isGrounded;
    public Transform groundPoint;
    public LayerMask whatIsGround;

    public bool hasAttacked = false;
    public bool canMove = false, canAttack = false, canBlock = false;
    private bool canSpell;
    public GameObject spell;
    private bool isMoving, isBlocking;
    private bool canShoot = true;
    private bool witch;
    public Transform shootPoint;
    public GameObject holyBullet;

    private void Awake()
    {
        instance = this;

        rgBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapPoint(groundPoint.position, whatIsGround);

        if(canMove)
        {
            Move();
        }

        if(canAttack)
        {
            Attack();
        }

        if(canBlock)
        {
            Block();
        }
        Roll();

        SwitchSide();

        anim.SetFloat("moveSpeed", Mathf.Abs(rgBody.velocity.x));
        anim.SetFloat("ySpeed", rgBody.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isBlocking", isBlocking);
    }

    private void Move()
    {
        rgBody.velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), rgBody.velocity.y);

        if(rgBody.velocity.x !=0)
        {
            isMoving = true;
            canAttack = false;
        }
        else if(rgBody.velocity.x == 0)
        {
            isMoving = false;
            canAttack = true;
        }

        if (Input.GetKeyDown(KeyCode.U) && rgBody.velocity.y == 0 && isGrounded)
        {
            rgBody.velocity = new Vector2(rgBody.velocity.x, jumpforce);
        }
    }

    private void SwitchSide()
    {
        if (rgBody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (rgBody.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void Attack()
    {
        if(Input.GetKeyDown(KeyCode.H) && !hasAttacked && isGrounded && !isBlocking)
        {
            hasAttacked = true;
        }
    }

    public void Block()
    {
        if(Input.GetKey(KeyCode.K) && canBlock)
        {
            if(Input.GetKeyDown(KeyCode.U) && canSpell && isBlocking && !canMove && isGrounded)
            {
                Instantiate(spell, transform.position, Quaternion.identity);
                anim.Play("skill1");
                rgBody.velocity = new Vector2(0, rgBody.velocity.y);
            }
            else if(Input.GetKeyDown(KeyCode.H) && canSpell && isBlocking && !canMove && isGrounded)
            {
                anim.Play("skill2");
            }
            else if (Input.GetKeyDown(KeyCode.J) && canSpell && isBlocking && !canMove && isGrounded && canShoot)
            {
                if(witch)
                {
                    anim.Play("shoot1");
                    witch = false;
                }
                else
                {
                    anim.Play("shoot2");
                    witch = true;
                }
            }
            canSpell = true;
            isBlocking = true;
            canAttack = false;
        }
        else
        {
            canSpell = false;
            canAttack = true;
            isBlocking = false;
        }
    }

    public void CanMove()
    {
        canMove = true;
    }

    public void CantMove()
    {
        canMove = false;
    }

    public void CanBlock()
    {
        canBlock = true;
    }

    public void CantBlock()
    {
        canBlock = false;
    }

    public void AddForce()
    {
        if(transform.localScale == new Vector3(1, 1, 1))
        {
            rgBody.AddForce(new Vector2(attackForce, rgBody.velocity.y));
        }
        else if (transform.localScale == new Vector3(-1, 1, 1))
        {
            rgBody.AddForce(new Vector2(-attackForce, rgBody.velocity.y));
        }
    }

    public void StopMoving()
    {
        rgBody.velocity = new Vector2(0, rgBody.velocity.y);
    }

    public void Shoot()
    {
        Instantiate(holyBullet, shootPoint.position, Quaternion.identity);
    }

    public void CanShoot()
    {
        if(canShoot)
        {
            canShoot = false;
        }
        else if(!canShoot)
        {
            canShoot = true;
        }
    }

    public void Roll()
    {
        if(Input.GetKeyDown(KeyCode.J) && !isBlocking && isGrounded)
        {
            anim.Play("PlayerRoll");
        }
    }
}
