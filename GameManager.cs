using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    [Header("#Gmae Control")]
    public bool isLive;
    public float gametime;
    public float maxgametime = 20 * 60f;
    [Header("#Player Info")]
    public int level;
    public float Health;
    public float MaxHealth = 100;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 280, 400, 600 };
    [Header("#Game Object")]
    public Player player;
    public PoolManager pool;
    public GameObject uiResult;
    public GameObject HealthBar;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
    }

    public void GameStart()
    {
        isLive = true;

        Health = MaxHealth;

       Audiomanager.instance.PlayBgm(true);
       Audiomanager.instance.PlaySfx(Audiomanager.Sfx.select);
        Resuem();
    }

    public void GmaeOver()
    { 
        StartCoroutine(GameOverRoutine());
        Audiomanager.instance.PlayBgm(false);
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.SetActive(true);
        HealthBar.SetActive(false);
        Stop();

        Audiomanager.instance.PlaySfx(Audiomanager.Sfx.Lose);
        Audiomanager.instance.PlayBgm(false);
    }
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (!isLive)
            return;

        gametime += Time.deltaTime;
        if (gametime > maxgametime)
        {
            gametime = maxgametime;
        }
    }

    public void GetExp()
    {
        exp++;

        if (exp >= nextExp[level])
        {
            level++;
            exp = 0;
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resuem()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
