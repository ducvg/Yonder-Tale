using PrimeTween;
using TMPro;
using UnityEngine;

public class LoginCanvas : BaseCanvas
{
    [SerializeField] private TMP_InputField accountInput, passwordInput;
    [SerializeField] private float afterLoginDelay = 0.5f;

    [Header("Debug")]
    [SerializeField] private bool isProcessing = false;

    private LoadingCanvas loadingCanvas; //cache for faster processing

    public void Login()
    {
        if (isProcessing || isTweening) return; //calling api or tweening
        isProcessing = true;

        //temp model data to send to server
        var loginData = new
        {
            account = accountInput.text,
            password = passwordInput.text
        };

        // Hide the login panel
        UIManager.Instance.Close<LoginCanvas>();
        // show the loading panel
        loadingCanvas = UIManager.Instance.Open<LoadingCanvas>();
        loadingCanvas.Loading(); //loading state

        // Call the API
        Debug.Log("jsonData");
        StartCoroutine(ApiCall.PostRequest("api/Authorization/Login", loginData,
            (response) =>
            {
                isProcessing = false;

                //notify success in loading panel
                loadingCanvas.LoadSuccess(response["token"].ToString()); 
                
            },
            (errorResponse) =>
            {
                isProcessing = false;
                Debug.Log("Login failed: " + errorResponse.code);
                Debug.Log("Message: " + errorResponse.message);
        
                //notify failure in loading panel
                loadingCanvas.LoadFail(errorResponse);

            }
        ));


    }
    
}
