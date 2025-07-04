using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordField : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private Image toggleButtonImage;
    [SerializeField] private Sprite showIcon, HideIcon;

    public void ToggleVisibility()
    {
        if(inputField.contentType == TMP_InputField.ContentType.Password)
        {
            inputField.contentType = TMP_InputField.ContentType.Standard;
            toggleButtonImage.sprite = showIcon;
        }
        else
        {
            inputField.contentType = TMP_InputField.ContentType.Password;
            toggleButtonImage.sprite = HideIcon;
        }

        inputField.ForceLabelUpdate(); //force update the label to reflect the change
    }
}
