
using PrimeTween;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class LoginBtn : MonoBehaviour
{
    [SerializeField] private float tweenTime = 1f;

    [SerializeField] private float loginDelay = 1.5f;

    [SerializeField] private Graphic loadingGraphic;
    [SerializeField] private TMP_Text notificationText;

    [SerializeField] private TweenRect loginPanel, loadingPanel;

    [SerializeField] private Animator loadingAnimator;

    [SerializeField] private TMP_InputField accountInput;
    [SerializeField] private TMP_InputField passInput;

    private bool isLoggingIn = false;

    private Graphic[] loginPanelGraphics, loadingPanelGraphics;

    void Awake()
    {
        loginPanelGraphics = loginPanel.rectTransform.GetComponentsInChildren<Graphic>(true);
        loadingPanelGraphics = loadingPanel.rectTransform.GetComponentsInChildren<Graphic>(true);
    }

    public void Login()
    {
        if (isLoggingIn) return;
        isLoggingIn = true;

        //temp data to send to server
        var loginData = new
        {
            account = accountInput.text,
            password = passInput.text
        };

        // Hide the login panel
        foreach (var child in loginPanelGraphics)
        {
            Tween.Alpha(child, 0, tweenTime, Ease.InOutQuad);
        }
        Tween.UIAnchoredPosition
        (
            loginPanel.rectTransform,
            loginPanel.hidePosition,
            tweenTime,
            Ease.InOutQuad
        );

        //Show the loading panel
        loadingPanel.rectTransform.gameObject.SetActive(true);
        foreach (var child in loadingPanelGraphics)
        {
            Tween.Alpha(child, 1, tweenTime, Ease.InOutQuad);

        }
        Tween.UIAnchoredPosition(loadingPanel.rectTransform, loadingPanel.showPosition, tweenTime, Ease.InOutQuad);

        // Call the API
        StartCoroutine(ApiCall.PostRequest("api/Authorization/Login", loginData,
            (response) =>
            {
                Debug.Log("Login successful: " + response);

                //fade in and out the loading icon
                loadingAnimator.SetTrigger(AnimatorHash.isDone);
                float clipLength = loadingAnimator.GetCurrentAnimatorStateInfo(0).length;
                Debug.Log(clipLength);
                Tween.Alpha(loadingGraphic, 0, clipLength * 0.7f, Ease.InOutQuad);
                Tween.Alpha(loadingGraphic, 1, clipLength, Ease.InOutQuad, startDelay: clipLength);

                //notify with login panel
                notificationText.text = response["token"].ToString();

                Tween.Delay(loginDelay, () =>
                {

                    // change scene or something idk

                    isLoggingIn = false;
                });
            },
            (error) =>
            {
                Tween.Delay(loginDelay, () =>
                {
                    isLoggingIn = false;
                });
            }));
    }
    
}


// public void Login()
//     {
//         if (isLoggingIn) return;
//         isLoggingIn = true;

//         //temp data to send to server
//         var loginData = new
//         {
//             account = accountInput.text,
//             password = passInput.text
//         };

//         // Hide the login panel
//         foreach (var child in loginPanelGraphics)
//         {
//             child.DOFade(0, tweenTime)
//                 .SetEase(Ease.InOutQuad);
//         }
//         loginPanel.rectTransform
//             .DOAnchorPos(loginPanel.hidePosition, tweenTime)
//             .SetEase(Ease.InOutQuad)
//             .OnComplete(() =>
//             {
//                 loginPanel.rectTransform.gameObject.SetActive(false);

                
//             });

//         //Show the loading panel
//         loadingPanel.rectTransform.gameObject.SetActive(true);
//         foreach (var child in loadingPanelGraphics)
//         {
//             Debug.Log(child.gameObject.name);
//             child.DOFade(1, tweenTime)
//                 .SetEase(Ease.InOutQuad);
//         }
//         loadingPanel.rectTransform
//             .DOAnchorPos(loadingPanel.showPosition, 0.5f)
//             .SetEase(Ease.InOutQuad);

//         // Call the API
//         StartCoroutine(ApiCall.PostRequest("api/Authorization/Login", loginData,
//             (response) =>
//             {
//                 Debug.Log("Login successful: " + response);

//                 //notify with login panel
//                 loadingAnimator.SetTrigger(AnimatorHash.isDone);
//                 notificationText.text = response["token"].ToString();

//                 DOVirtual.DelayedCall(loginDelay, () =>
//                 {

//                     // change scene or something idk

//                     isLoggingIn = false;
//                 });
//             },
//             (error) =>
//             {
//                 DOVirtual.DelayedCall(loginDelay, () =>
//                 {
//                     isLoggingIn = false;
//                 });
//             }));
//     }