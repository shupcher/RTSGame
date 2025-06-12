using UnityEngine;
using UnityEngine.InputSystem;

public class UnitsSelection : MonoBehaviour
{
    public InputActionAsset actions;
    private InputAction _selectUnitsAction;
    private bool _isDraggingMouseBox = false;
    private Vector3 _dragStartPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _selectUnitsAction = actions.FindActionMap("UI").FindAction("Click");
    }

    // Update is called once per frame
    void Update()
    {
     if (_selectUnitsAction.WasPerformedThisFrame())
        {
            _isDraggingMouseBox = true;
            _dragStartPosition = Input.mousePosition;
        }

        if (_selectUnitsAction.WasReleasedThisFrame())
            _isDraggingMouseBox = false;
    }

    void OnGUI()
    {
        if (_isDraggingMouseBox)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(_dragStartPosition, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
            Utils.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
        }
    }

}