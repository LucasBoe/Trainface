using UnityEngine;
using System.Collections;

public interface IClickable {
    string GetName();

    void EndHover();
    void UpdateHover();
    void StartHover();

    void StartDrag();
    void EndDrag();
}
