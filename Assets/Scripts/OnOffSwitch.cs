using UnityEngine;
using UnityEngine.UI;

public class OnOffSwitch : MonoBehaviour
{
    [SerializeField] public bool isOnSwitch;

    private Image buttonImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBuutonClick()
    {
        isOnSwitch = !isOnSwitch;

        if (isOnSwitch)
        {
            Color color = buttonImage.color;
            color.a = 1;
            buttonImage.color = color;
        }
        else
        {
            Color color = buttonImage.color;
            color.a = 0;
            buttonImage.color = color;
        }
    }
}
