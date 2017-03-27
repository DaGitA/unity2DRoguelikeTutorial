using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MovingObject
{

    [SerializeField]
    private int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;

    protected override void Start()
    {
        GameManager.instance.AddEnnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (areInSameColumn())
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<Player>(xDir, yDir);
    }

    private bool areInSameColumn()
    {
        return Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon;
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("attack");
        hitPlayer.LoseFood(playerDamage);
    }
}
