using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] Wagon engine;
    [SerializeField] Wagon[] wagons;

    [SerializeField] Tile startTile;
    RailSection startSection;
    SectionSwitch startSwitch;

    SectionSwitch latestSwitch;

    private void Start()
    {
        startSection = startTile.Sections[0];

        engine.Init(startSection, this);

        foreach (Wagon wagon in wagons)
            wagon.Init(startSection, this);
    }

    internal SectionSwitch GetNextSection(Wagon wagon, SectionSwitch lastSectionSwitch)
    {
        if (wagon == engine)
        {
            SectionSwitch next = TileTravelHelper.GetNextSection(wagon.Section, wagon.Direction);

            if (startSwitch == null) startSwitch = next;

            if (latestSwitch != null)
                latestSwitch.NextSwitch = next;

            latestSwitch = next;
            return next;
        }

        if (lastSectionSwitch != null)
            return lastSectionSwitch.NextSwitch;

        return startSwitch;
    }

    private void Update()
    {
        float speed = Time.deltaTime * 0.5f;
        engine.Move(speed);

        foreach (Wagon wagon in wagons)
            wagon.Move(speed);
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

    public static TrainDirection Inverse(this TrainDirection direction)
    {
        return direction == TrainDirection.Forward ? TrainDirection.Backward : TrainDirection.Forward;
    }
}
