using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardManager;
    public int savedPlayerFoodPoints = 100;
    [HideInInspector]
    public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private int level = 1;
    private List<Ennemy> ennemies;
    private bool ennemiesMoving;
    private bool doingSetup;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        ennemies = new List<Ennemy>();
        boardManager = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        ennemies.Clear();
        boardManager.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved";
        levelImage.SetActive(true);
        this.enabled = false;
    }

    void Update()
    {
        if (playersTurn || ennemiesMoving || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        level++;
        InitGame();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }



    public void AddEnnemyToList(Ennemy script)
    {
        ennemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        ennemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (ennemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i = 0; i < ennemies.Count; i++)
        {
            ennemies[i].MoveEnnemy();
            yield return new WaitForSeconds(ennemies[i].moveTime);
        }
        playersTurn = true;
        ennemiesMoving = false;
    }
}
