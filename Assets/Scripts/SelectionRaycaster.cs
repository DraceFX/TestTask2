using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionRaycaster : MonoBehaviour
{
    [SerializeField] private float _holdTime = 2f;

    private float _holdTimer;
    private bool _isHolding;
    private bool _holdTriggered;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isHolding = true;
            _holdTriggered = false;
            _holdTimer = 0f;
        }

        if (Mouse.current.leftButton.isPressed && _isHolding)
        {
            _holdTimer += Time.deltaTime;

            if (_holdTimer >= _holdTime && !_holdTriggered)
            {
                HoldRaycast();
                _holdTriggered = true;
                _isHolding = false;
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (_isHolding)
            {
                ClickRaycast();
            }

            _isHolding = false;
        }
    }

    private void ClickRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out ISelectable selectable))
            {
                selectable.ClickObject();
            }
        }
    }

    private void HoldRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out ISelectable selectable))
            {
                selectable.HoldObject();
            }
        }
    }
}
