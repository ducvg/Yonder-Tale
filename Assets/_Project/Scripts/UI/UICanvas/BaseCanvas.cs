using System.Collections.Generic;
using UnityEngine;

public class BaseCanvas : MonoBehaviour
{
    [SerializeReference] protected List<Transition> transitions;
    [SerializeField] protected CanvasGroup canvasGroup;

    private RectTransform rectTransform;
    private bool isTweening = false; //track if canvas is fully open or closed

    protected virtual void Awake()
    {
        
        rectTransform = transform as RectTransform;
    }

    public virtual void Setup()
    {
        if (isTweening) return;
        isTweening = true;

        gameObject.SetActive(true);

        Open();
    }

    protected virtual void Open()
    {
        SetAsHidden();

        // Tween.Alpha(canvasGroup, alphaSettings)
        // .Chain(
        //     Tween.UIAnchoredPosition3D(rectTransform, moveSettings.WithDirection(toEndValue: false))
        // )
        // .OnComplete(this, target => target.OnOpenComplete());
    }

    protected virtual void OnOpenComplete()
    {
        isTweening = false;
    }

    public virtual void Close(float delay)
    {
        if (isTweening) return;
        isTweening = true;

        // Tween.Delay(target: this, duration: delay,
        //     onComplete: target => target.Close()
        // );
    }

    public virtual void Close()
    {
        if (isTweening) return;
        isTweening = true;

        SetAsShow();

        // Tween.Alpha(canvasGroup, alphaSettings.WithDirection(toEndValue: false))
        // .Chain(
        //     Tween.UIAnchoredPosition3D(rectTransform, moveSettings.WithDirection(toEndValue: true))
        // )
        // .OnComplete(this, target => target.OnCloseComplete());
    }

    protected virtual void OnCloseComplete()
    {
        isTweening = false;
        gameObject.SetActive(false);
    }

    [ContextMenu("Set as Show")]
    public void SetAsShow()
    {
        canvasGroup.alpha = 1f;
    }

    [ContextMenu("Set as Hidden")]
    public void SetAsHidden()
    {
        canvasGroup.alpha = 0f;
    }
}


