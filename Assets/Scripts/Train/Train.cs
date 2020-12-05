using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] float trainSpeed = 10;

    Line line;
    Goods goods;
    Color color;
    City lastPassed;

    float position = 0; //0 - 1
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

        StartDriving();
    }

    public void StartDriving() {

        if (driving == null)
            driving = StartCoroutine(DrivingRoutine());
    }

    public void StopDriving () {
        Destroy(gameObject);
    }

    IEnumerator DrivingRoutine() {
        while (line != null) {
            position = (Mathf.Sin(Time.time * trainSpeed / line.Length) + 1) / 2;
            yield return null;
        }
    }

    private void TryPass(City passed) {
        if (lastPassed != passed)
        {
            Debug.LogWarning("passed " + passed.name + " at " + position);
            lastPassed = passed;
            passed.Pass(this);
        }
    }

    internal void CheckPassTEMP(City passing)
    {
        TryPass(passing);
    }
}
