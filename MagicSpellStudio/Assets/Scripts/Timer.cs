using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private int secondsRemaining = 30;
    public TextMeshPro timerText;
    public GameObject endGameModal;
    public Cauldron cauldron;
    public TextMeshProUGUI endGameText;
    public Material cauldronMat;
    public Image endGameImage;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DecreaseTimer", 1, 1);
    }

    private void DecreaseTimer()
    {
        secondsRemaining--;
        timerText.text = secondsRemaining.ToString("D2");
        if(secondsRemaining <= 0)
        {
            Time.timeScale = 0;
            endGameModal.SetActive(true);
            if(cauldron.Red > cauldron.Blue)
            {
                endGameText.text = "Red wins!";
            }
            else if(cauldron.Blue > cauldron.Red)
            {
                endGameText.text = "Blue wins!";
            }
            else
            {
                endGameText.text = "It's a tie!";
            }
            endGameImage.color = cauldronMat.GetColor("_Color") * 0.85f;
            CancelInvoke();
        }
    }
}
