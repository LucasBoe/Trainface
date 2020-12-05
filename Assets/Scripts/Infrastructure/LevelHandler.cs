﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : Singleton<LevelHandler>
{
    [SerializeField] private List<City> unfullfilled = new List<City>();
    [SerializeField] private List<City> fullfilled = new List<City>();

    internal void Fullfilled(City city)
    {
        if (!fullfilled.Contains(city) && unfullfilled.Contains(city))
        {
            Debug.Log("Fullfilled needs of " + city.name);
            fullfilled.Add(city);
            unfullfilled.Remove(city);
        }

        CheckIfAllFullfilled();
    }

    private void CheckIfAllFullfilled()
    {
        if (unfullfilled.Count == 0)
            Debug.LogWarning("level finished (" + fullfilled.Count + "/" + fullfilled.Count + ")");
    }

    internal void Unfullfilled(City city)
    {
        if (!(fullfilled.Contains(city) || unfullfilled.Contains(city)))
            unfullfilled.Add(city);
    }

    internal void ResetProgression()
    {
        unfullfilled.AddRange(fullfilled);
        fullfilled.Clear();
    }
}
