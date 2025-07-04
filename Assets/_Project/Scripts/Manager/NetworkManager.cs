using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiCall
{
    const string BASE_URL = "https://meobeovl.runasp.net/";

    public static IEnumerator GetRequest(string uri, Action<object> onSuccess = null, Action<string> onError = null)
    {
        uri = BASE_URL + uri;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Request succeeded 200
                JObject jsonResponse = JObject.Parse(webRequest.downloadHandler.text);

                onSuccess?.Invoke(webRequest.downloadHandler.text);

            }
            else //fail to get request
            {

                onError?.Invoke(webRequest.error);

            }
        }
    }

    public static IEnumerator PostRequest(string uri, object data, Action<JObject> onSuccess = null, Action<string> onError = null)
    {
        uri = BASE_URL + uri;
        // Convert the data object to JSON
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, jsonData, "application/json"))
        {

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
                onError?.Invoke(webRequest.error);
            }
        }
    }
}
