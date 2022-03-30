using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHandler : Singleton<TrainHandler>
{
    [SerializeField] OldTrain trainPrefab;

    public OldTrain SpawnTrain(Line line)
    {
        OldTrain newTrain = Instantiate(trainPrefab);
        newTrain.Init(line, line.Goods,line.Color);
        return newTrain;
    }
}
