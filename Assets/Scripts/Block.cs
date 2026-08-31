using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Sprite nomalBlock;
    [SerializeField] private Sprite completedBlock;

    SpriteRenderer MainSpriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSprite(bool conpleted)
    {
        Debug.Log(conpleted);
        if (conpleted)
        {
            MainSpriteRenderer.sprite = completedBlock;
        }
        else
        {
            MainSpriteRenderer.sprite = nomalBlock;
        }
    }
}
