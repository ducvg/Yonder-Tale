using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BaseCanvas : MonoBehaviour
{
    [SerializeField] protected TweenSettings<Vector3> tweenSetting;

    private RectTransform rectTransform;
    private Graphic[] graphics;
    private bool isRunning = false; //track if canvas is fully open or closed

    void Awake()
    {
        rectTransform = transform as RectTransform;
        graphics = GetComponentsInChildren<Graphic>(includeInactive: true);
    }

    public virtual void Setup()
    {
        if (isRunning) return;
        isRunning = true;

        Open();
    }

    protected virtual void Open()
    {
        foreach (Graphic graphic in graphics)
        {
            Tween.Alpha(graphic, endValue: 1, tweenSetting.settings.duration);
        }
        Tween.UIAnchoredPosition3D(rectTransform, tweenSetting.WithDirection(toEndValue: false))
        .OnComplete(this, target => target.OnOpenComplete());
    }

    protected virtual void OnOpenComplete()
    {
        isRunning = false;
        gameObject.SetActive(true);
    }

    public virtual void Close(float delay)
    {
        Tween.Delay(target: this, duration: delay,
            onComplete: target => target.Close()
        );
    }

    public virtual void Close()
    {
        if(isRunning) return;
        isRunning = true;

        foreach (Graphic graphic in graphics)
        {
            Tween.Alpha(graphic, endValue: 0f, tweenSetting.settings.duration);
        }
        Tween.UIAnchoredPosition3D(rectTransform, tweenSetting.WithDirection(toEndValue: true))
        .OnComplete(this, target => target.CloseDirectly());
    }

    protected virtual void CloseDirectly()
    {
        isRunning = false;
        gameObject.SetActive(false);
    }

}
