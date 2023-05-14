using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private float speed, jumpSpeed;
    [SerializeField] private int heal;
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Rigidbody2D rb;
    private bool isDead, isJump;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(!isDead && GameManager.instance.gameActive)
        {
            Vector3 yon = playerTransform.position - transform.position; // Playerin pozisyonundan kendi poziyonunu ��kar�r ve b�ylece bir y�n vekt�r� olu�turur
            yon.y = 0;
            if (yon.x < 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;

            if (yon.magnitude > 0.3f)
            {
                anim.SetBool("run", true);
                yon.Normalize();
                transform.position += yon * speed * Time.deltaTime;
            }

            if(!isJump)
            {
                RaycastHit2D hit;
                if (spriteRenderer.flipX) //Karakter sola bak�yorsa
                {
                    Vector2 startingPoint = transform.position + Vector3.down * 0.3f + Vector3.left * 0.5f; // Ba�lang�� noktas�n� karakterin merkez noktas�n�n biraz a�a��s� ve ilerisi olarak ayarlad�k
                    hit = Physics2D.Raycast(startingPoint, Vector2.left, 0.5f); // Sol tarafa do�ru k�sa bir ray g�nderir
                }
                else
                {
                    Vector2 startingPoint = transform.position + Vector3.down * 0.3f + Vector3.right * 0.5f; 
                    hit = Physics2D.Raycast(startingPoint, Vector2.right, 0.5f);
                }
                if (hit.collider != null && hit.collider.CompareTag("ground")) // Ray bir grounda �arpt�ysa �al���r
                {
                    Jump();
                }
            }     
        }
        else if(!GameManager.instance.gameActive)
        {
            anim.SetBool("run", false);
            anim.SetBool("attack", false);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("player")) //Playere de�di�inde atak ba�lat
        {
            anim.SetBool("attack",true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player")) //Player temas� kesildi�inde atak durdur
        {
            anim.SetBool("attack", false);
        }
    }
    public void Damage() //Can� azalt, 0 a d��t�yse �l�r
    {
        if(--heal<=0)
            StartCoroutine(Dead());
    }
    public IEnumerator Dead()
    {
        GameManager.instance.Score();
        anim.SetTrigger("die");
        isDead = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<CapsuleCollider2D>().enabled = false;
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        Destroy(gameObject, 3f);
    }
    public void Jump()
    {
        isJump = true;
        anim.SetTrigger("jump");
        rb.AddForce(Vector2.up * jumpSpeed); //Yukar� z�plar
        Invoke(nameof(JumpCancel),1f); //S�rekli z�plamamas� i�in z�plad�ktan sonra tekrar 1 saniye sonra z�playabilir
    }
    private void JumpCancel()
    {
        isJump = false;
    }
}
