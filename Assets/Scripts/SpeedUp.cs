using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    private TweenAlpha _tweenAlpha = null;

    private void Awake()
    {
        _tweenAlpha = gameObject.GetComponent<TweenAlpha>();
    }

    public void Active()
    {
        gameObject.SetActive(true);

        _tweenAlpha.PlayForward();

        Invoke("InActive", 2f);
    }

    private void InActive()
    {
        gameObject.SetActive(false);
    }
}
