using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public List<GameObject> enemyList;
    public List<string> colorPattron;
    float timer;
    public Texture[] hearthImages;
    public int loseLevel;
    public int level;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 4 && SceneManager.GetActiveScene().buildIndex != 5)
        {
            enemyList.Clear();
            LevelStatus(false);
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemyList.Add(g);
            }
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LevelStatus(bool isEnd)
    {
        switch (isEnd)
        {
            case false:
                GameObject.Find("PANEL_InGame").GetComponent<Image>().CrossFadeAlpha(0, 2f, false);
                break;
            case true:
                GameObject.Find("PANEL_InGame").GetComponent<Image>().CrossFadeAlpha(1, 2f, false);
                break;
        }
    }

    public void ChangeLevel(int level)
    {
        if (enemyList.Count == 0)
        {
            if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 4 && SceneManager.GetActiveScene().buildIndex != 5)
                LevelStatus(true);
            StartCoroutine(FinalRutine(level));
        }
    }

    private IEnumerator FinalRutine(int level)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 4 && SceneManager.GetActiveScene().buildIndex != 5)
        {
            float duration = 3f;
            float normalizedTime = 0;
            while (normalizedTime <= 1f)
            {
                if (level != 3)
                    GameObject.FindGameObjectWithTag("Player").transform.Translate(new Vector2(0.1f, 0));
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }
        SceneManager.LoadScene(level + 1);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public bool CheckButtons(string color)
    {
        for (int i = 0; i <= colorPattron.Count; i++)
        {
            if (colorPattron.Count == 0)
            {
                colorPattron.Add(color);
                break;
            }
            if (color != colorPattron[colorPattron.Count - 1])
            {
                colorPattron.Add(color);
                break;
            }
            else
            {
                colorPattron.Clear();
                return true;
            }
        }
        return false;
    }

    public void HearthCheck()
    {
        RawImage h1 = GameObject.Find("1_Hearth").GetComponent<RawImage>();
        RawImage h2 = GameObject.Find("2_Hearth").GetComponent<RawImage>();
        RawImage h3 = GameObject.Find("3_Hearth").GetComponent<RawImage>();
        switch (GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>().health)
        {
            case 120:
                h1.texture = hearthImages[0];
                h2.texture = hearthImages[0];
                h3.texture = hearthImages[0];
                break;
            case float n when n > 80 && n < 120:
                h1.texture = hearthImages[0];
                h2.texture = hearthImages[0];
                h3.texture = hearthImages[1];
                break;
            case 80:
                h1.texture = hearthImages[0];
                h2.texture = hearthImages[0];
                h3.texture = hearthImages[2];
                break;
            case float n when n > 40 && n < 80:
                h1.texture = hearthImages[0];
                h2.texture = hearthImages[1];
                h3.texture = hearthImages[2];
                break;
            case 40:
                h1.texture = hearthImages[0];
                h2.texture = hearthImages[2];
                h3.texture = hearthImages[2];
                break;
            case float n when n > 0 && n < 40:
                h1.texture = hearthImages[1];
                h2.texture = hearthImages[2];
                h3.texture = hearthImages[2];
                break;
            case 0:
                h1.texture = hearthImages[2];
                h2.texture = hearthImages[2];
                h3.texture = hearthImages[2];
                break;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoseGame()
    {
        SceneManager.LoadScene(5);
    }

    public void RestartGame(int level)
    {
        SceneManager.LoadScene(level);
    }
}
