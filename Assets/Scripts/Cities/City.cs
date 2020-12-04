using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, IClickable
{
    public Color Color;





    public string GetName()
    {
        return name;
    }

    public void StartDrag()
    {
        Debug.Log("start drag");
        Game.CityConnectionHandler.TryStartConnectionAt(this);
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

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = new Material(meshRenderer.material);
            meshRenderer.material.color = Color;
        }
    }
}
