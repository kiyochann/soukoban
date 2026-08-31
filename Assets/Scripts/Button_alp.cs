using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_alp : MonoBehaviour
{
    private Image buttonImage;
    private void Start()
    {
        buttonImage = GetComponent<Image>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPointerEnter()
    {
        if (buttonImage != null) {
            //Debug.Log("aaa");
            Color color = buttonImage.color;
            color.a = 1;
            buttonImage.color = color;
        }
    }

    // ƒ}ƒEƒX‚ª—£‚ê‚½Žž
    public void OnPointerExit()
    {
        if (buttonImage != null)
        {
            //Debug.Log("bbb");
            Color color = buttonImage.color;
            color.a = 0;
            buttonImage.color = color;
        }
    }
}
