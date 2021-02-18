using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{

    static private UIFade _singleton = null;

  
    public Image texture = null;

    Color color = new Color();

    private bool _isEndActive;

    protected void Awake()
    {
        if (_singleton != null && _singleton != this)
        {
            DestroyImmediate(this);
            return;
        }
        _singleton = this;
        SetActive(false);

        //
        
    }

    void OnDestroy()
    {
        if (_singleton == this)
        {
            _singleton = null;
        }
    }


    private void Update()
    {
        if( Input.GetKeyDown(KeyCode.P))
        {
            Debug.LogError("P");

            In(2.0f, true);
        }
        else
        {
            Debug.LogError("L");

            Out(2.0f, true);

        }
    }

    static public void SetActive(bool active)
    {
        _singleton.gameObject.SetActive(active);
        _singleton.texture.enabled = active;
    }

    static public void In(float duration, bool isEndActive)
    {
        _singleton._Play(duration, false, isEndActive);
    }

    static public void Out(float duration, bool isEndActive)
    {
        _singleton._Play(duration, true, isEndActive);
    }

    private void _Play(float duration, bool isOut, bool isEndActive)
    {
        SetActive(true);
        if (isOut)
        {


            var fadeImage = GetComponent<Image>();
            fadeImage.enabled = true;
            var c = fadeImage.color;
            c.a = 0.0f;

            //  texture.color.a = 0.0f;

            if (isEndActive)
            {
                //                DOTween.To(() => texture.alpha, x => texture.alpha = x, 1, duration).SetTarget(texture);
                // DOTween.To(() => texture.color, x => texture.alpha = x, 1, duration).SetTarget(texture);

                //var fadeImage = GetComponent<Image>();
                //fadeImage.enabled = true;
                //var c = fadeImage.color;
                //c.a = 1.0f; // 
                fadeImage.color = c;

                DOTween.ToAlpha(
                    () => fadeImage.color,
                    color => fadeImage.color = color,
                    0f, // 
                    1f // 
                );

            }
            else
            {
                //                DOTween.To(() => texture.alpha, x => texture.alpha = x, 1, duration).SetTarget(texture).OnComplete(CompleteTween);

                fadeImage.enabled = true;
                c = fadeImage.color;
                c.a = 1.0f; // 
                fadeImage.color = c;

                DOTween.ToAlpha(
                    () => fadeImage.color,
                    color => fadeImage.color = color,
                    0f, // 
                    1f // 
                ).OnComplete(CompleteTween);

            }
        }
        else
        {
            //texture.alpha = 1;

            //if (isEndActive)
            //    DOTween.To(() => texture.alpha, x => texture.alpha = x, 0, duration).SetTarget(texture);
            //else
            //    DOTween.To(() => texture.alpha, x => texture.alpha = x, 0, duration).SetTarget(texture).OnComplete(CompleteTween);

            var fadeImage = GetComponent<Image>();
            fadeImage.enabled = true;
            var c = fadeImage.color;
            c.a = 1.0f;

            if( isEndActive)
            {
                fadeImage.color = c;

                DOTween.ToAlpha(
                    () => fadeImage.color,
                    color => fadeImage.color = color,
                    1f, // 
                    0f // 
                );
            }
            else
            {
                DOTween.ToAlpha(
                    () => fadeImage.color,
                    color => fadeImage.color = color,
                    1f, // 
                    0f // 
                ).OnComplete(CompleteTween);
            }


        }
    }

    private void CompleteTween()
    {
        SetActive(false);
    }

    //--

    /*
     * 
     * // @param -
// getter   : 트위닝의 속성 값을 반환받는 파람 delegate형식. 람다식 사용 가능 () => myValue
// setter   : 트위닝의 속성 값을 설정하는 파람 delegate형식. 람다식 사용 가능 x => myValue = x
// to       : 도달할 값 설정
// duration : 트위닝의 지속 시간
DOTween.To(getter, setter, to, float duration);
// 사용 예시
// Tween a Vector3 called myVector to 3,4,8 in 1 second
DOTween.To(()=> myVector, x=> myVector = x, new Vector3(3,4,8), 1);
// Tween a float called myFloat to 52 in 1 second
DOTween.To(()=> myFloat, x=> myFloat = x, 52, 1);
     */

    void Alpha()
    {
        var fadeImage = GetComponent<Image>();
        fadeImage.enabled = true;
        var c = fadeImage.color;
        c.a = 1.0f; // 
        fadeImage.color = c;

        DOTween.ToAlpha(
            () => fadeImage.color,
            color => fadeImage.color = color,
            0f, // 
            1f // 
        );
    }
}
