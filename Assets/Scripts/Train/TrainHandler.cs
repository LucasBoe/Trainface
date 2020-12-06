using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHandler : Singleton<TrainHandler>
{
    [SerializeField] Train trainPrefab;

    public Train SpawnTrain(Line line)
    {
        Train newTrain = Instantiate(trainPrefab);
        newTrain.Init(line, line.GetCities()[0].Goods,line.Color);
        return newTrain;
    }
}
