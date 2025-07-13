using System;
using System.Text;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : BaseCanvas
{
    [SerializeField] private float spinningDuration = 1f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Vector3 shakeStrength = new Vector3(1.5f, 1.5f, 1.5f);

    [SerializeField] private Sprite tickSprite, errorSprite;

    [SerializeField] private Image loadingIcon;
    [SerializeField] private TextMeshProUGUI notificationText;

    private Tween spinningTween;
    private string initText;

    protected override void Awake()
    {
        base.Awake();
        initText = notificationText.text;
    }

    public void Loading()
    {
        notificationText.text = initText;

        spinningTween = Tween.EulerAngles(loadingIcon.transform,
            startValue: Vector3.zero,
            endValue: new Vector3(0, 0, 360),
            duration: spinningDuration,
            ease: Ease.Linear,
            cycles: -1);
    }

    public void LoadSuccess(string message)
    {
        //skip tween immediately to keep rotation
        spinningTween.Complete();

        //tween loading icon to tick
        Tween.Custom(startValue: 1, endValue: 0, duration: fadeDuration,
            onValueChange: value => loadingIcon.fillAmount = value)
        .OnComplete(target: this, target => target.loadingIcon.sprite = tickSprite)
        .Chain(
            Tween.Custom(startValue: 0, endValue: 1, duration: fadeDuration, 
            onValueChange: value => loadingIcon.fillAmount = value)
        );

        ChangeText(message);
    }

    public void LoadFail(ErrorResponse error)
    {
        //skip tween immediately to keep rotation
        spinningTween.Complete();

        //tween loading icon to error
        Tween.ShakeScale(loadingIcon.transform, strength: shakeStrength, duration: fadeDuration);
        loadingIcon.sprite = errorSprite;
        StringBuilder sb = new StringBuilder();
        sb.Append(error.code); sb.Append(": "); sb.Append(error.message);
        ChangeText(sb.ToString());
    }

    //fade out -> change text -> fade in
    private void ChangeText(string message)
    {
        Tween.Alpha(notificationText, startValue: 1f, endValue: 0f, duration: fadeDuration)
        .OnComplete(target: this, target => target.notificationText.text = message)
        .Chain(Tween.Alpha(notificationText, startValue: 0f, endValue: 1f, duration: fadeDuration));
    }

}
