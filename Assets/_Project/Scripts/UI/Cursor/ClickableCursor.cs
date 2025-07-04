using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private bool normalAfterClick = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(normalAfterClick)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
//
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}