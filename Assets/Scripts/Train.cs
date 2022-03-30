using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] Tile startTile;
    [SerializeField] Wagon engine;
    [SerializeField] Wagon[] wagons;

    [SerializeField] List<SectionSwitch> sectionBuffer = new List<SectionSwitch>();
    RailSection start;

    private const int BUFFER_MAX_LENGTH = 2;

    private void Start()
    {
        start = startTile.Sections[0];

        engine.Init(start, this);

        foreach (Wagon wagon in wagons)
            wagon.Init(start, this);
    }

    internal SectionSwitch GetNextSection(Wagon wagon, SectionSwitch lastSectionSwitch)
    {
        if (wagon == engine)
        {
            SectionSwitch next = TileTravelHelper.GetNextSection(wagon.Section, wagon.Direction);

            if (sectionBuffer.Count > BUFFER_MAX_LENGTH)
                sectionBuffer.RemoveAt(0);

            if (sectionBuffer.Count > 0)
                sectionBuffer.Last().NextSwitch = next;

            sectionBuffer.Add(next);
            return next;
        }

        if (lastSectionSwitch != null)
            return lastSectionSwitch.NextSwitch;

        return sectionBuffer.First(); ;
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
