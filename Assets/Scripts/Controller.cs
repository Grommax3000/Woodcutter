using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Controller : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public Transform homePoint;
    public Tree target;
    public GameObject logs;

    private Transform charTransform;

    enum State
    {
        idle = 1,
        move = 2,
        hack = 3,
        carry = 4
    }
    private State state = State.idle;

    private void Start()
    {
        charTransform = transform;
        FindTree();
    }

    private void Update()
    {
        switch (state)
        {
            case State.idle:
                break;

            case State.move:
                float distToTarget = Vector3.Distance(charTransform.position, target.treeTransform.position);
                if(distToTarget <= agent.stoppingDistance)
                {
                    animator.SetTrigger("Hack");
                    state = State.hack;
                }
                break;

            case State.hack:
                if (target.health <= 0)
                {
                    animator.SetTrigger("Carry");
                    state = State.carry;
                    logs.SetActive(true);
                    agent.SetDestination(homePoint.position);
                }
                break;

            case State.carry:
                float distToHome = Vector3.Distance(charTransform.position, homePoint.position);
                if (distToHome <= agent.stoppingDistance)
                {
                    logs.SetActive(false);
                    state = State.idle;
                    FindTree();
                }
                break;
        }
    }

    public void Hit()
    {
        target.GetHit();
    }

    public void FindTree()
    {
        if (target == null && state == State.idle)
        {
            target = GetNearestTree();
            if (target != null)
            {
                agent.SetDestination(target.treeTransform.position);
                animator.SetTrigger("Move");
                state = State.move;
            }
            else
            {
                animator.SetTrigger("Idle");
            }
        }
    }

    private Tree GetNearestTree()
    {
        float dist = 0f;
        Tree tmpTreee = null;
        NavMeshPath tmpPath = new NavMeshPath();
        bool haveValue = false;
        for (int i = 0; i < GameManager.instance.trees.Count; i++)
        {
            float lng = 0f;
            agent.CalculatePath(GameManager.instance.trees[i].treeTransform.position, tmpPath);
            if (tmpPath.status != NavMeshPathStatus.PathInvalid && tmpPath.status != NavMeshPathStatus.PathPartial)
            {
                for (int j = 1; j < tmpPath.corners.Length; j++)
                {
                    lng += Vector3.Distance(tmpPath.corners[j - 1], tmpPath.corners[j]);
                }
                if (haveValue == false)
                {
                    haveValue = true;
                    dist = lng;
                    tmpTreee = GameManager.instance.trees[i];
                }
                else
                {
                    if (lng < dist)
                    {
                        dist = lng;
                        tmpTreee = GameManager.instance.trees[i];
                    }
                }
            }
        }
        return tmpTreee;
    }
}
