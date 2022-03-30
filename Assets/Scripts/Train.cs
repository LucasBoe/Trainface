using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] Tile startTile;
    [SerializeField, Range(0, 1)] float tilePosition;
    [SerializeField] TrainDirection direction;
    RailSection section;

    private void Start()
    {
        section = startTile.Sections[0];
    }


    private void Update()
    {
        tilePosition += Time.deltaTime;
        TravelResult travel = TileTravelHelper.Travel(section, direction, tilePosition);

        transform.position = travel.Position;
        transform.rotation = travel.Rotation;

        if (travel.NewSection != null)
            section = travel.NewSection;

        if (travel.UpdateTilePosition)
            tilePosition = travel.NewTilePosition;

        if (travel.ChangeDirection)
            direction = direction == TrainDirection.Forward ? TrainDirection.Backward : TrainDirection.Forward;
    }
}

public enum TrainDirection
{
    Forward,
    Backward,
}

public static class TrainDirectionUtil
{
    public static float Invert(this TrainDirection direction, float forward)
    {
        if (direction == TrainDirection.Forward)
            return forward;

        return 1f - forward;
    }
}
