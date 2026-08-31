using UnityEngine;

public class RestartLevelButton : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    private GameManager gameManagerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonClick()
    {
        gameManagerScript.isRestart = true;
    }
}
