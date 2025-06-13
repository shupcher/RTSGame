using UnityEngine;
using UnityEngine.InputSystem;

public class UnitManager : MonoBehaviour
{
//Since BuildingManager inherits from UnitManager, selectionCircle needs to be attached to the BuildingManager script in the inspector.
    public GameObject selectionCircle;
    public InputActionAsset actions;
    private InputAction _selectUnitsAction;

    private void Start()
    {
      _selectUnitsAction = actions.FindActionMap("UI").FindAction("Click");
    }
    public void Select()
    {
        if (Globals.SELECTED_UNITS.Contains(this)) return;
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
