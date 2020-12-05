using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, IClickable
{
    [SerializeField] MeshRenderer meshRenderer, groundBlend;
    [SerializeField] Goods goods;
    public Goods Goods {
        get {
            return goods;
        }
    }
    [SerializeField] bool isSource;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0,360), 0);

        if (meshRenderer != null && !goods.IsNull)
        {
            Debug.Log("enable visuals for " + name);
            meshRenderer.enabled = true;
            meshRenderer.material = new Material(meshRenderer.material);
            meshRenderer.material.color = goods.Data.color;

            if (!isSource)
            {
                groundBlend.enabled = true;
                Game.LevelHandler.Unfullfilled(this);
            }
        }
    }

    public virtual bool Produces (string goodsName = "") {
        return isSource && (goodsName == "" || goodsName == goods.name);
    }

    public virtual bool Needs(string goodsName = "")
    {
        return !isSource && (goodsName == "" || goodsName == goods.name);
    }

    /// <summary>
    /// Interaction
    /// </summary>

    public void StartDrag()
    {
        Debug.Log("start drag");
        Game.CityConnectionHandler.TryStartConnectionAt(this);
    }

    internal void Pass(Train train)
    {
        Debug.Log(train.name + " passed " + name + " with " + train.Goods.name + " and they " + (isSource?"produce ":"need ") + Goods.name);

        if (!isSource && train.Goods.name == Goods.name)
        {
            Debug.Log("so it's a match!");
            Game.LevelHandler.Fullfilled(this);
        }
    }

    public void EndDrag()
    {
        Debug.Log("end drag");
        Game.CityConnectionHandler.TryEndConnectionAt(this);
    }

    public void StartHover()
    {
        transform.localScale = Vector3.one * 1.1f;
    }

    public void UpdateHover()
    {
        //
    }

    public void EndHover()
    {
        transform.localScale = Vector3.one;
    }

    public string GetName()
    {
        return name;
    }
}
