using System;
using UnityEngine;

public interface ITrackpointCreator
{
    void Pass(OldTrain train);

    bool Produces();

    Trackpoint[] CreateTrackpoints();

    string GetName();
    Color GetColor();
    Goods GetGoods();
    Vector3 GetLocation();
}

