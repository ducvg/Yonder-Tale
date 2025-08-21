using UnityEngine;

public class StartLogin : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.Open<LoginCanvas>();
    }
}
