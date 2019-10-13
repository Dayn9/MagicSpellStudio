using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron : Pickup
{
    int red = 0;
    int blue = 0;

    public int Red { get { return red; } }
    public int Blue { get { return blue; } }

    public Material cauldronMat;

    [SerializeField] private Slider ratioSlider;
    private float targetRatio;

    private void Start()
    {
        cauldron = this;
        ratioSlider.value = 0.5f;
        targetRatio = 0.5f;
        cauldronMat.SetColor("_Color", Color.Lerp(Color.blue, Color.red, 0.5f));
    }

    private void Update()
    {
        ratioSlider.value = Mathf.Lerp(ratioSlider.value, targetRatio, 0.3f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Pickup") && collision.transform.parent == null)
        {
            Player pt = collision.gameObject.GetComponent<Pickup>().PlayerType;

            switch (pt)
            {
                case Player.P1:
                    red++;
                    break;
                case Player.P2:
                    blue++;
                    break;
            }

            //Debug.Log(red + " " + blue);
            targetRatio = (red * 1.0f) / (red + blue);

            cauldronMat.SetColor("_Color", Color.Lerp(Color.blue, Color.red, (red * 1.0f) / (red + blue)));

            Destroy(collision.gameObject);
        }
    }
}
