using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardManager;
    public int savedPlayerFoodPoints = 100;
    [HideInInspector]
    public bool playersTurn = true;

    private int level = 3;
    private List<Ennemy> ennemies;
    private bool ennemiesMoving;

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
        ennemies.Clear();
        boardManager.SetupScene(level);
    }

    public void GameOver()
    {
        this.enabled = false;
    }

    void Update()
    {
        if(playersTurn || ennemiesMoving)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
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
