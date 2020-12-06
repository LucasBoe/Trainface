using System;
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

    public static LevelHandler LevelHandler {
        get {
            return LevelHandler.GetInstance();
        }
    }

    public static Game GetInstance () {
        return Instance;
    }

    private static GameSettings gameSettings;
    public static GameSettings Settings {
        get {
            if (gameSettings == null)
                gameSettings = LoadGameSettings();

            return gameSettings;
        }
    }

    private static GameSettings LoadGameSettings()
    {
        return Resources.LoadAll<GameSettings>("Settings")[0];
    }
}
