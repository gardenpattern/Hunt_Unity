using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{//
    public int id;
    public int prefebid;
    public float damage;
    public float cutDamage;
    public int count;
    public float speed;
    float timer;

    Player player;
    
    public GameObject attackEaffectPrefab;
    public float attackInterval = 0.5f;
    private float lastAttackTime;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void Start()
    {
        Init();
    }
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (Input.GetMouseButton(1))
        {
            Attack();
        }

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    //fire();
                }
                break;
        }
    }

    public void cut(float cutDamage)
    {
        this.cutDamage = cutDamage;
       
    }
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                break;
            case 3:
                speed = 1f;
                break;
            default:
                speed = 0.3f;
                break;
        }
    }
    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefebid).transform;
            }

            bullet.parent = transform;

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 reVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(reVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity per
        }
    }
    private void Attack()
    {
        if (Time.time - lastAttackTime < attackInterval)
            return;

        lastAttackTime = Time.time;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(attackEaffectPrefab, mousePosition, Quaternion.identity);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mousePosition, 3f);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                collider.SendMessage("OntakeDamage", cutDamage, SendMessageOptions.DontRequireReceiver);
            }
        }
        Audiomanager.instance.PlaySfx(Audiomanager.Sfx.Melee);
    }
}
