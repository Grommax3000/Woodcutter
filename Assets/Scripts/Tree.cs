using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public Transform treeTransform;

    public int health = 3;

    public void GetHit()
    {
        health--;
        if(health <= 0)
        {
            GameManager.instance.trees.Remove(this);
            Destroy(gameObject);
        }
    }
}
