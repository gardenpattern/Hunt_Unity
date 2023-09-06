/using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] anicon;
    public Rigidbody2D target;

    bool isLive;

    Animator ani;
    Rigidbody2D rigid;
    Collider2D col;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive)
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        col.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        ani.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        ani.runtimeAnimatorController = anicon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {//bullet 태그내에 있는 얘들을 파악해서 데미지 받기
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            ani.SetTrigger("Hit");
            Audiomanager.instance.PlaySfx(Audiomanager.Sfx.Hit);
        }
        else
        {
            isLive = false;
            col.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            ani.SetBool("Dead", true);
            GameManager.Instance.kill++;
            GameManager.Instance.GetExp();
            if(GameManager.Instance.isLive)
                Audiomanager.instance.PlaySfx(Audiomanager.Sfx.Dead);
        }


    } 

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirvec = transform.position - playerPos;
        rigid.AddForce(dirvec.normalized * 3, ForceMode2D.Impulse);
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
