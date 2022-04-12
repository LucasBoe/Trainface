using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class GoodsTransporter : MonoBehaviour
{
    [SerializeField, Expandable] GoodsData goods;
    [Header("Wagons")]
    [SerializeField, Foldout("Wagons")] GameObject[] wagonGameObjects;
    [SerializeField, Foldout("Wagons")] MeshRenderer[] wagonMeshRenderers;
    [SerializeField, Foldout("Wagons")] int goodsMaterialIndex;
    Material[] materials;

    private void Awake()
    {
        materials = new Material[wagonMeshRenderers.Length];
        for (int i = 0; i < wagonMeshRenderers.Length; i++)
        {
            MeshRenderer renderer = wagonMeshRenderers[i];
            Material material = renderer.materials[goodsMaterialIndex];
            materials[i] = material;
            Material[] temp = renderer.materials;
            temp[goodsMaterialIndex] = material;
            renderer.materials = temp;
        }

        UpdateWagonMats();
    }

    private void UpdateWagonMats()
    {
        bool hasGoods = goods != null;

        foreach (GameObject go in wagonGameObjects) go.SetActive(hasGoods);

        if (!hasGoods) return;

        Color color = goods.color;

        foreach (Material material in materials)
            material.color = color;
        
    }
}
