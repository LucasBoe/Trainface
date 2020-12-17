using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : RailObject
{
    [SerializeField] float trainSpeed = 10;

    Line line;
    private Goods goods;
    public Goods Goods {
        get {
            return goods;
        }
    }
    Color color;
    Trackpoint lastPassed;

    private float position = 0; //0 - 1
    public float Position
    {
        get
        {
            return position;
        }
    }

    Coroutine driving;





    public void Init(Line line, Goods goods, Color color)
    {
        this.line = line;
        this.goods = goods;
        this.color = color;

        Rail(line);
        StartDriving();
    }

    protected override void TryPass(Trackpoint passed) {
        if (lastPassed != passed)
        {
            lastPassed = passed;
            passed.Pass(this);
        }
    }
}
