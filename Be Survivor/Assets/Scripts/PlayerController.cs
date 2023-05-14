using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, jumpSpeed, menzil;
    [SerializeField] private int heal;
    [SerializeField] private LayerMask gameLayer;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool canJump=true, damaged;
    private int projectile = 10;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = rb.GetComponent<SpriteRenderer>();
        anim = rb.GetComponent<Animator>();
    }
    private void Update()
    {
        if(GameManager.instance.gameActive)
        {
            if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool("run", true);
                sprite.flipX = false;
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool("run", true);
                sprite.flipX = true;
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            else
            {
                anim.SetBool("run", false);
            }

            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                anim.SetTrigger("jump");
                canJump = false;
                rb.AddForce(Vector2.up * jumpSpeed); //Yukarý doðru güç uygular
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("projectile")) //Mermi yakaladýðý zaman çalýþýr
        {
            AudioManager.instance.PlayAudio(AudioManager.AudioCallers.Projectile);
            projectile += 10;
            UIManager.instance.ProjectileText(projectile);
            Destroy(collision.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground")) //Zeminde olup olmadýðýný kontrol etme 
        {
            canJump = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision) //Düþmana temas ettiðimizde canýmýz 0 ve üstüyse hasar alýrýz, deðilse ölürüz
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            if (heal <= 0)
                Die();
            else if(!damaged)
            {
                damaged = true;
                anim.SetTrigger("hurt");
                --heal;
                Invoke(nameof(CancelDamage), 1f);
                StartCoroutine(UIManager.instance.Damaged());
            }
        }
    }
    private void Attack()
    {
        if(projectile>0)
        {
            UIManager.instance.ProjectileText(--projectile);
            AudioManager.instance.PlayAudio(AudioManager.AudioCallers.Shot);
            anim.Play("Shot");
            Vector2 shootPoint = transform.position;
            RaycastHit2D hit;
            if (sprite.flipX) //Karakter sola bakýyorsa
            {
                hit = Physics2D.Raycast(shootPoint, Vector2.left, menzil, gameLayer); // Sol tarafa doðru menzilli bir ray gönderir
                Debug.DrawRay(shootPoint, Vector2.left * menzil, Color.red, 0.1f); // Debug için ray'ý gösterir
            }
            else
            {
                hit = Physics2D.Raycast(shootPoint, Vector2.right, menzil, gameLayer);
                Debug.DrawRay(shootPoint, Vector2.right * menzil, Color.red, 0.1f);
            }
            if (hit.collider != null && hit.collider.CompareTag("enemy"))
            {
                ParticleManager.instance.PlayParticle(ParticleManager.ParticleCallers.Blood, hit.transform.position);
                hit.collider.GetComponent<Zombie>().Damage(); //Ray düþmana çarptýysa hasar verir
            }
            else if (hit.collider != null && hit.collider.CompareTag("ground"))
            {
                ParticleManager.instance.PlayParticle(ParticleManager.ParticleCallers.Impact, hit.transform.position);
            }
        }
    }
    private void Die()
    {
        anim.SetTrigger("die");
        GameManager.instance.GameOver();
    }
    private void CancelDamage()
    {
        damaged = false;
    }

}

