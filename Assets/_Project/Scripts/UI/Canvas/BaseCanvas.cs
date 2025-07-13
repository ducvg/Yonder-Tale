using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BaseCanvas : MonoBehaviour
{
    [SerializeField] protected TweenSettings<Vector3> tweenSetting;

    private RectTransform rectTransform;
    private Graphic[] graphics;
    protected bool isTweening = false; //track if canvas is fully open or closed

    protected virtual void Awake()
    {
        rectTransform = transform as RectTransform;
        graphics = GetComponentsInChildren<Graphic>(includeInactive: true);
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
        foreach (Graphic graphic in graphics)
        {
            Tween.Alpha(graphic, endValue: 1, tweenSetting.settings.duration);
        }
        Tween.UIAnchoredPosition3D(rectTransform, tweenSetting.WithDirection(toEndValue: false))
        .OnComplete(this, target => target.OnOpenComplete());
    }

    protected virtual void OnOpenComplete()
    {
        isTweening = false;
    }

    public virtual void Close(float delay)
    {
        if (isTweening) return;
        isTweening = true;

        Tween.Delay(target: this, duration: delay,
            onComplete: target => target.Close()
        );
    }

    public virtual void Close()
    {
        if (isTweening) return;
        isTweening = true;

        foreach (Graphic graphic in graphics)
        {
            Tween.Alpha(graphic, endValue: 0f, tweenSetting.settings.duration);
        }
        Tween.UIAnchoredPosition3D(rectTransform, tweenSetting.WithDirection(toEndValue: true))
        .OnComplete(this, target => target.OnCloseComplete());
    }

    protected virtual void OnCloseComplete()
    {
        isTweening = false;
        gameObject.SetActive(false);
    }

    [ContextMenu("Set as Show")]
    public void EditorSetAsShow()
    {
        var tmp = GetComponentsInChildren<Graphic>(includeInactive: true);
        foreach (var graphic in tmp)
        {
            graphic.color = graphic.color.WithAlpha(1f);
        }
        (gameObject.transform as RectTransform).anchoredPosition3D = tweenSetting.startValue;
    }

    [ContextMenu("Set as Hidden")]
    public void EditorSetAsHidden()
    {
        var tmp = GetComponentsInChildren<Graphic>(includeInactive: true);
        foreach (var graphic in tmp)
        {
            graphic.color = graphic.color.WithAlpha(0f);
        }
        (gameObject.transform as RectTransform).anchoredPosition3D = tweenSetting.endValue;
    }
}
