using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ScaleFade : MonoBehaviour
{
    public float fadeTime = 3f;
    public float scaleValue = 1.2f;
    private void Start()
    {
        this.GetComponent<RectTransform>().DOScale(new Vector3(scaleValue, scaleValue, scaleValue), fadeTime);
    }
}
