using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : Singleton<PlayerInteraction>
{
    Camera main;
    IClickable before;
    public Vector3 hitPosition;

    protected override void Start()
    {
        base.Start();
        main = Camera.main;
    }

    private void Update()
    {
        //did the hover change hover
        IClickable current = RaycastForClickable();

        if (current != before)
        {
            if (before != null)
                before.EndHover();

            if (current != null)
                current.StartHover();
        }

        if (current != null)
        {
            //update hover
            current.UpdateHover();

            //check for interaction with player
            ITrackpointCreator tpc = current as ITrackpointCreator;

            if (Input.GetMouseButtonDown(0))
                Game.CityConnectionHandler.TryStartConnectionAt(tpc);
            if (Input.GetMouseButtonUp(0))
                Game.CityConnectionHandler.TryEndConnectionAt(tpc);
        } else {
            if (Input.GetMouseButtonUp(0))
                Game.CityConnectionHandler.AbortPlacement();
        }

        before = current;
    }

    private IClickable RaycastForClickable() {
        RaycastHit hit;
        Ray ray = main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            hitPosition = hit.point;

            IClickable clickable = hit.transform.GetComponent<IClickable>();
            if (clickable != null)
                return clickable;
        }

        return null;
    }
}
