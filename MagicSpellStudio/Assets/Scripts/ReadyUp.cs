using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReadyUp : MonoBehaviour
{
    private KeyCode readyUpP1 = KeyCode.LeftShift;
    private KeyCode readyUpP2 = KeyCode.RightShift;
    private bool readyP1 = false;
    private bool readyP2 = false;
    public Animator readyModalAnimator;
    public GameObject readyP1Image;
    public GameObject readyP2Image;
    public TextMeshProUGUI countdownText;
    private int secondsRemaining = 3;
    private WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1);

    private void Start()
    {
        StartCoroutine(CheckForReady());
    }

    private IEnumerator CheckForReady()
    {
        while(!(readyP1 && readyP2))
        {
            if (Input.GetKeyDown(readyUpP1))
            {
                readyP1 = true;
                readyP1Image.SetActive(true);
            }
            if (Input.GetKeyDown(readyUpP2))
            {
                readyP2 = true;
                readyP2Image.SetActive(true);
            }
            yield return null;
        }
        readyModalAnimator.SetTrigger("Ready");
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSecondsRealtime(2.1f);
        while(secondsRemaining > 0)
        {
            countdownText.text = secondsRemaining.ToString();
            yield return wait;
            secondsRemaining--;
        }
        countdownText.enabled = false;
        Time.timeScale = 1;
    }
}
