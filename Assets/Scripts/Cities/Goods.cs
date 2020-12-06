using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Goods
{
    public GoodsData Data {
        get {
            return Game.GoodsHandler.GetDataFor(name);
        }
    }

    public bool IsNull {
        get {
            Debug.Log("'" + name + "' is " + (name != ""?"not ":"" ) + "null." );
            return (name == "");
        }
    }

    public string name;

    public Goods (string name) {
        this.name = name;
    }
}
