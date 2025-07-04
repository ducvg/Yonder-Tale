using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class LoginBtn : MonoBehaviour
{
    [SerializeField] private float tweenTime = 1f;

    [SerializeField] private float LoginDelay = 1.5f;
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
            child.DOFade(0, tweenTime)
                .SetEase(Ease.InOutQuad);
        }
        loginPanel.rectTransform
            .DOAnchorPos(loginPanel.hidePosition, tweenTime)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                loginPanel.rectTransform.gameObject.SetActive(false);
            });

        //Show the loading panel
        loadingPanel.rectTransform.gameObject.SetActive(true);
        foreach (var child in loadingPanelGraphics)
        {
            child.DOFade(1, tweenTime)
                .SetEase(Ease.InOutQuad);
        }
        loadingPanel.rectTransform.DOAnchorPos(loadingPanel.showPosition, 0.5f)
            .SetEase(Ease.InOutQuad);

        // Call the API
        StartCoroutine(ApiCall.PostRequest("api/Authorization/Login", loginData,
            (response) =>
            {
                Debug.Log("Login successful: " + response);

                //notify with login panel
                loadingAnimator.SetTrigger(AnimatorHash.isDone);

                //local coroutine to handle post-login actions
                IEnumerator OnLoginSuccess()
                {
                    yield return new WaitForSeconds(LoginDelay);



                    // change scene or something idk

                    isLoggingIn = false;
                }

                // Start the coroutine to handle post-login actions
                StartCoroutine(OnLoginSuccess());

            },
            (error) =>
            {
                IEnumerator OnLoginFailed()
                {
                    yield return new WaitForSeconds(LoginDelay);


                    // change scene or something idk
                    isLoggingIn = false;
                }

                StartCoroutine(OnLoginFailed());
            }));
    }
    
}
