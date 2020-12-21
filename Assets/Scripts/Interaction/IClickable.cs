using UnityEngine;
using System.Collections;

public interface IClickable {

    void EndHover();
    void UpdateHover();
    void StartHover();

    string GetName();
}
