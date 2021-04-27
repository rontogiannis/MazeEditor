using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellEncoder : MonoBehaviour
{
    SpriteRenderer upWall;
    SpriteRenderer rightWall;

    void Awake()
    {
        upWall = transform.Find("Up").GetComponent<SpriteRenderer>();
        rightWall = transform.Find("Right").GetComponent<SpriteRenderer>();
    }

    public int GetValue()
    {
        int val = 0;

        if (upWall.enabled) val += 2;
        if (rightWall.enabled) val += 1;

        return val;
    }
}
