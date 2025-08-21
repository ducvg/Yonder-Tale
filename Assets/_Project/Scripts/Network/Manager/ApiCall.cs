using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiCall
{
    const string BASE_URL = "https://meobeovl.runasp.net/";
    const string CONTENT_TYPE_JSON = "application/json";

    public static IEnumerator GetRequest(string uri, Action<JObject> onSuccess = null, Action<string> onError = null)
    {
        uri = BASE_URL + uri;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", CONTENT_TYPE_JSON);

            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Request succeeded 200
                JObject jsonResponse = JObject.Parse(webRequest.downloadHandler.text);

                onSuccess?.Invoke(jsonResponse);

            }
            else //fail to get request
            {
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.Append(webRequest.error);
                errorMessage.AppendLine(webRequest.downloadHandler.text);

                onError?.Invoke(errorMessage.ToString());

            }
        }
    }

    public static IEnumerator PostRequest(string uri, object data, Action<JObject> onSuccess = null, Action<string> onError = null)
    {
        uri = BASE_URL + uri;
        // Convert the data object to JSON
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, jsonData, CONTENT_TYPE_JSON))
        {
            Debug.Log(data);

            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Request succeeded 200
                JObject jsonResponse = JObject.Parse(webRequest.downloadHandler.text);

                onSuccess?.Invoke(jsonResponse);
            }
            else //fail to post request
            {
                StringBuilder errorMessage = new();
                errorMessage.Append(webRequest.error);
                errorMessage.AppendLine(webRequest.downloadHandler.text);

                onError?.Invoke(errorMessage.ToString());
            }
        }
    }
}
