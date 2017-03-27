using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public Text foodText;

    [SerializeField]
    private int wallDamage = 1;
    [SerializeField]
    private int pointsPerFood = 10;
    [SerializeField]
    private int pointsPerSoda = 20;
    [SerializeField]
    private float restartLevelDelay = 1f;

    private Animator animator;
    private int food;

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("chop");
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoseFood(int loss)
    {
        foodText.text = "-" + loss + " food";
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    protected override void Start()
    {
        foodText.text = "food: " + food;
        animator = GetComponent<Animator>();
        food = GameManager.instance.savedPlayerFoodPoints;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playersTurn)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        vertical = removeDiagonalMove(horizontal, vertical);

        if (moveAttempted(horizontal, vertical))
        {
            AttemptMove<Wall>(horizontal, vertical);
        }

    }

    private bool moveAttempted(int horizontal, int vertical)
    {
        return horizontal != 0 || vertical != 0;
    }

    private static int removeDiagonalMove(int horizontal, int vertical)
    {
        if (horizontal != 0)
        {
            vertical = 0;
        }

        return vertical;
    }

    private void OnDisable()
    {
        GameManager.instance.savedPlayerFoodPoints = food;
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "food: " + food;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            foodText.text = "+" + pointsPerFood + " food";
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            foodText.text = "+" + pointsPerSoda + " food";
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }
}
