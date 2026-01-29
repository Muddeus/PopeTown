using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    [SerializeField] private InputActionReference pointerPositionAction;
    private RectTransform cursorRectTransform;
    private Canvas canvas;
    private RectTransform canvasRectTransform;
    private Camera cam;

    private void Awake()
    {
        cursorRectTransform = GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
            cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        }
    }

    private void OnEnable()
    {
        pointerPositionAction.action.performed += OnPointerPositionChanged;
    }

    private void OnDisable()
    {
        pointerPositionAction.action.performed -= OnPointerPositionChanged;
    }

    private void OnPointerPositionChanged(InputAction.CallbackContext context)
    {
        if (cursorRectTransform == null || canvasRectTransform == null) return;

        var mousePosition = context.ReadValue<Vector2>();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, cam, out var localPoint))
        {
            cursorRectTransform.anchoredPosition = localPoint;
        }
    }
}
