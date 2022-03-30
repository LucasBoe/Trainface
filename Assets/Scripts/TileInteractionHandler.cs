using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteractionHandler : MonoBehaviour
{
    [SerializeField] TileData[] shop;
    [SerializeField] TileCursor cursor;
    Plane plane;

    [SerializeField] TileInteractionMode mode = new HoverInteractionMode();
    [SerializeField] TileData emptyTile;

    public static Camera Main;
    
    public static TileData EmptyTile;

    public Vector3 hitPosition;

    public static System.Action<TileInteractionMode> ChangedTileMode;

    private void Awake()
    {
        Main = Camera.main;
        EmptyTile = emptyTile;
        plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        Ray ray = Main.ScreenPointToRay(Input.mousePosition);
        float distance;
        plane.Raycast(ray, out distance);



        Vector3 free = ray.GetPoint(distance);
        Vector3 onGrid = new Vector3(Mathf.Round(free.x), Mathf.Round(free.y), Mathf.Round(free.z));
        cursor.UpdateCursorPosition(onGrid);

        TileInteractionMode modeChange = mode?.Update();
        if (modeChange != null) SetMode(modeChange);
    }

    private void OnGUI()
    {
        foreach (TileData tile in shop)
        {
            if (GUILayout.Button(tile.name))
                SetMode(new InHandInteractionMode(tile, 0));
        }
    }

    private void SetMode(TileInteractionMode newMode)
    {
        mode?.Exit();
        mode = newMode;
        newMode?.Enter();

        ChangedTileMode?.Invoke(newMode);
    }
}

public abstract class TileInteractionMode
{
    public abstract void Enter();

    public abstract TileInteractionMode Update();

    public abstract void Exit();
}

public class HoverInteractionMode : TileInteractionMode
{
    [SerializeField] protected Tile hovered;

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    private void SetHovered(Tile newHover)
    {
        if (hovered != null)
            hovered.Leave();

        hovered = newHover;

        if (newHover != null)
            hovered.Enter();
    }

    public override TileInteractionMode Update()
    {
        Ray ray = TileInteractionHandler.Main.ScreenPointToRay(Input.mousePosition);
        Tile newHover = RaycastForTile(ray);
        if (newHover != hovered)
            SetHovered(newHover);

        if (Input.GetMouseButtonUp(0) && hovered != null)
            return ClickOnTile(hovered);

        return null;
    }

    private Tile RaycastForTile(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Tile tile = hit.transform.GetComponent<Tile>();
            if (tile != null)
                return tile;
        }

        return null;
    }

    protected virtual TileInteractionMode ClickOnTile(Tile tile)
    {
        InHandInteractionMode mode = new InHandInteractionMode(tile.Data, tile.Offset);
        tile.PlaceTile(TileInteractionHandler.EmptyTile);
        return mode;
    }
}

public class InHandInteractionMode : HoverInteractionMode
{
    public TileData TileInHand;
    public int DirectionOffset;

    public static System.Action<TileData, int> ChangedPreviewTile;

    public InHandInteractionMode(TileData tile, int offset)
    {
        TileInHand = tile;
        DirectionOffset = offset;
    }

    public override void Enter()
    {
        ChangedPreviewTile?.Invoke(TileInHand, DirectionOffset);
    }

    public override void Exit()
    {
        ChangedPreviewTile?.Invoke(null, 0);
    }

    public override TileInteractionMode Update()
    {
        if (Input.GetMouseButtonUp(1))
            return new HoverInteractionMode();

        if (Input.GetKeyUp(KeyCode.Comma))
            OffsetDirection();

        return base.Update();
    }

    protected override TileInteractionMode ClickOnTile(Tile tile)
    {
        InHandInteractionMode mode = new InHandInteractionMode(tile.Data, tile.Offset);
        tile.PlaceTile(TileInHand, DirectionOffset);
        return mode;
    }

    private void OffsetDirection()
    {
        DirectionOffset++;

        if (DirectionOffset > 3)
            DirectionOffset = 0;

        ChangedPreviewTile?.Invoke(TileInHand, DirectionOffset);
    }
}
