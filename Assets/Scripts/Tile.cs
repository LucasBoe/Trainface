using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Transform visuals;
    [SerializeField] TileData data;
    [SerializeField] int offset;
    public int Offset => offset;
    private static Vector3 localOffsetOnHover = new Vector3(0, -0.1f, 0);
    public TileData Data => data;

    [SerializeField] private RailSection[] sections;
    public RailSection[] Sections => sections;

    private void Awake()
    {
        Game.TileManager.RegisterTile(this);
    }

    private void Start()
    {
        InitTile();
    }

    internal void Enter()
    {
        LerpTo(localOffsetOnHover);
    }

    internal void Leave()
    {
        LerpTo(Vector3.zero);
    }

    private void LerpTo(Vector3 localTarget)
    {
        StopAllCoroutines();
        StartCoroutine(LerpVisualsRoutine(localTarget));
    }

    internal RailSection GetSection()
    {
        if (sections == null || sections.Length == 0) return null;
        return sections[0];
    }

    IEnumerator LerpVisualsRoutine(Vector3 localTarget)
    {
        float distance = float.MaxValue;
        while (distance > 0.0001f && visuals != null)
        {
            visuals.transform.localPosition = Vector3.Lerp(visuals.transform.localPosition, localTarget, Time.deltaTime * 10f);
            distance = Vector3.Distance(visuals.transform.localPosition, localTarget);
            yield return null;
        }

        if (visuals != null)
            visuals.transform.localPosition = localTarget;
    }

    private void InitTile()
    {
        visuals = Instantiate(data.visualizationPrefab, transform.position, offset.ToTileRotation(), transform).transform;
        UpdateConnections();
    }

    internal void PlaceTile(TileData newData, int offset = 0)
    {
        transform.DestroyAllChildren();

        data = newData;
        this.offset = offset;

        UpdateConnections();

        if (data != null)
            visuals = Instantiate(data.visualizationPrefab, transform.position + Vector3.up * 0.5f, offset.ToTileRotation(), transform).transform;

        LerpTo(localOffsetOnHover);
    }

    private void UpdateConnections()
    {
        RailTileData railTileData = data as RailTileData;
        if (railTileData == null)
        {
            this.sections = null;
            return;
        }

        List<RailSection> sections = new List<RailSection>();

        foreach (TileDataConnection connection in railTileData.Connections)
        {
            TileDirection fromDir = connection.From.Offset(offset);
            TileDirection toDir = connection.To.Offset(offset);

            Tile from = Game.TileManager.GetNeightbour(this, fromDir);
            Tile to = Game.TileManager.GetNeightbour(this, toDir);

            RailSection newSection = new RailSection(this, from, to, fromDir, toDir);

            sections.Add(newSection);
        }

        this.sections = sections.ToArray();
    }
}
