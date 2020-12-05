using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game instance;
    private static Game Instance
    {
        get {
            if (instance == null)
                instance = FindObjectOfType<Game>();

            if (instance == null)
                new GameObject("GAME").AddComponent<Game>();

            return instance;
        }
    }

    public static PlayerInteraction PlayerInteraction {
        get {
            return PlayerInteraction.GetInstance();
        }
    }

    public static CityConnectionHandler CityConnectionHandler
    {
        get {
            return CityConnectionHandler.GetInstance(usePrefab: true);
        }
    }

    public static LineHandler LineHandler
    {
        get {
            return LineHandler.GetInstance(usePrefab: true);
        }
    }

    public static TrainHandler TrainHandler {
        get {
            return TrainHandler.GetInstance(usePrefab: true);
        }
    }

    public static GoodsDataHandler GoodsHandler {
        get {
            return GoodsDataHandler.GetInstance();
        }
    }

    public static Game GetInstance () {
        return Instance;
    }
}
