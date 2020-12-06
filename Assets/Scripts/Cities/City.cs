using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, IClickable
{
    [SerializeField] MeshRenderer meshRenderer, groundBlend;
    [SerializeField] Goods goods;
    public Goods Goods {
        get => goods;
    }
    [SerializeField] bool isSource;
    private bool isFullfilled;
    public bool IsFullfilled {
        get => isFullfilled;
        set {
            isFullfilled = value;
                
            if (isFullfilled)
                Game.LevelHandler.Fullfilled(this);
            else
                Game.LevelHandler.Unfullfilled(this);

            UpdateFullfillmentVisuals(isFullfilled);
        }
    }

    private void UpdateFullfillmentVisuals(bool fullfillment)
    {
        groundBlend.enabled = (Game.Settings.displaySimplyfiedUI && !isSource && !fullfillment);
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0,360), 0);

        if (!goods.IsNull)
        {
            if (meshRenderer != null && Game.Settings.displaySimplyfiedUI)
            {
                Debug.Log("enable visuals for " + name);
                meshRenderer.enabled = true;
                meshRenderer.material = new Material(meshRenderer.material);
                meshRenderer.material.color = goods.Data.color;
            }

            if (!isSource)
            {
                IsFullfilled = false;
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
            IsFullfilled = true;
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
