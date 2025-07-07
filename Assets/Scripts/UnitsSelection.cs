using UnityEngine;
using UnityEngine.InputSystem;

public class UnitsSelection : MonoBehaviour
{
    private DefaultControls _defaultControls;
    private bool _isDraggingMouseBox = false;
    private Vector3 _dragStartPosition;
    public static bool DragJustReleased = false;
    public bool wasHeld = false;

    private void Awake()
    {
        _defaultControls = new DefaultControls();
    }

    private void OnEnable()
    {
        _defaultControls.UI.MouseHold.performed += OnHold;
        _defaultControls.UI.MouseHold.canceled += OnRelease;
        _defaultControls.UI.MouseHold.Enable();
    }

    private void OnDisable()
    {
        _defaultControls.UI.MouseHold.Disable();
    }

    void OnHold(InputAction.CallbackContext context)
    {
        _isDraggingMouseBox = true;
        _dragStartPosition = Mouse.current.position.ReadValue();
    }

    void OnRelease(InputAction.CallbackContext context)
    {
        if (!_isDraggingMouseBox)
            return; // Ignore if not button not held long enough to trigger dragging
        _isDraggingMouseBox = false;
        DragJustReleased = true;
        // Runs the selection logic once when the mouse button is released
        _SelectUnitsInDraggingBox();
    }
    // Update is called once per frame
    void Update()
    {
        if (_isDraggingMouseBox)
        {
            // Update the dragging box selection
            _SelectUnitsInDraggingBox();
        }

        if (DragJustReleased)
            DragJustReleased = false;
    }

    // Selects units in a rectangle defined by the start position and the current mouse position
    private void _SelectUnitsInDraggingBox()
    {
        Bounds selectionBounds = Utils.GetViewportBounds(
            Camera.main,
            _dragStartPosition,
            Mouse.current.position.ReadValue()
        );
        GameObject[] selectableUnits = GameObject.FindGameObjectsWithTag("Unit");
        bool inBounds;
        foreach (GameObject unit in selectableUnits)
        {
            inBounds = selectionBounds.Contains(
                Camera.main.WorldToViewportPoint(unit.transform.position)
            );
            if (inBounds)
            {
                unit.GetComponent<UnitManager>().Select();
            }
            else
                unit.GetComponent<UnitManager>().Deselect();
        }
    }

    void OnGUI()
    {
        if (_isDraggingMouseBox)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(_dragStartPosition, Mouse.current.position.ReadValue());
            Utils.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
            Utils.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
        }
    }

}