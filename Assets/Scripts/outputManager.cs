using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class outputManager : MonoBehaviour
{
    [SerializeField] private GameObject parObject, moveObject, scoreObject;
    private TMP_Text parText, moveText, scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        parText = parObject.GetComponent<TMP_Text>();
        moveText = moveObject.GetComponent<TMP_Text>();
        scoreText = scoreObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPar(int par_)
    {
        if(parText != null)parText.text = "Par: " + par_.ToString();
    }
    public void setMove(int move_)
    {
        if(moveText != null)moveText.text = "Move: " + move_.ToString();
    }
    public void setScore(int score_)
    {
        if(scoreText != null)scoreText.text = "Mcore: " + score_.ToString();
    }


}
