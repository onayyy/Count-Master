using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public Image _goodGamesTextImage;
    public GameObject cursor;
    public float AlphaValue;
    public float AlphaValueIn;
    public float duration;
    private void Start()
    {
        StartCoroutine(FadeTextToFullAlpha(AlphaValueIn, this.GetComponent<Image>()));
    }

    public IEnumerator FadeTextToFullAlpha(float t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            //_goodGamesTextImage.fillAmount += Time.deltaTime / 2.4f;
            //cursor.gameObject.GetComponent<RectTransform>().localPosition += Time.deltaTime / 2.4f;
            yield return null;
        }
        yield return new WaitForSeconds(duration);
        StartCoroutine(FadeTextToZeroAlpha(AlphaValue, GetComponent<Image>()));
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
    
}
