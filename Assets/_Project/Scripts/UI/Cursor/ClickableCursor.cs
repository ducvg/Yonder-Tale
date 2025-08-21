using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Texture2D hoverTexture, clickTexture;
    [SerializeField] private bool normalAfterClick = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        Cursor.SetCursor(clickTexture, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(normalAfterClick)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(hoverTexture, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}