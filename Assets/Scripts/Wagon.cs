using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float tilePosition;
    [SerializeField] public TrainDirection Direction;
    public RailSection Section;
    Train train;
    private SectionSwitch sectionSwitch;

    public void Move(float speed)
    {
        tilePosition += speed;

        if (tilePosition > 1f)
        {
            tilePosition -= 1f;
            sectionSwitch = train.GetNextSection(this, sectionSwitch);

            Section = sectionSwitch.NewSection;
            Direction = sectionSwitch.Direction;
        }

        PositionRotationPair newPosAndRot = TileTravelHelper.Travel(Section, Direction, tilePosition);

        transform.position = newPosAndRot.Position;
        transform.rotation = newPosAndRot.Rotation;
    }

    internal void Init(RailSection section, Train train)
    {
        this.Section = section;
        this.train = train;
    }
}
