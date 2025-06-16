using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //Since BuildingManager inherits from UnitManager, selectionCircle needs to be attached to the BuildingManager script in the inspector
    public GameObject selectionCircle;

    private void OnMouseDown()
    {
        if (IsActive())
            Select(true);
    }

    protected virtual bool IsActive()
    {
        return true; // Default implementation, can be overridden
    }
    public void Select() { Select(false); }
    public void Select(bool clearSelection)
    {
        if (Globals.SELECTED_UNITS.Contains(this)) return;
        if (clearSelection)
        {
            List<UnitManager> selectedUnits = new List<UnitManager>(Globals.SELECTED_UNITS);
            foreach (UnitManager um in selectedUnits)
                um.Deselect();
        }
        Globals.SELECTED_UNITS.Add(this);
        selectionCircle.SetActive(true);
    }

    public void Deselect()
    {
        if (!Globals.SELECTED_UNITS.Contains(this)) return;
        Globals.SELECTED_UNITS.Remove(this);
        selectionCircle.SetActive(false);
    }
}