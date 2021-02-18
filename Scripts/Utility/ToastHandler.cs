﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToastHandler : MonoBehaviour
{
    public Image image;
    public Text text;
    
    public void InitToast(string str, System.Action callback)
    {
        text.text = str;
        int StrLenth = str.Length;
        int ToastWidth = 1;
        int ToastHeight = 1;

        if (StrLenth > 10)
        {
            ToastHeight = (StrLenth / 10) + 1;
            ToastWidth = 10;

        }
        else
        {
            ToastWidth = StrLenth;
        }

        image.rectTransform.sizeDelta = new Vector2(50 * ToastWidth + 100, 50 * ToastHeight + 100);
        FadeOut(callback);
    }

    public void FadeOut(System.Action callback)
    {
        image.DOFade(0, 2).OnComplete(() =>
        {
            callback.Invoke();
            Destroy(gameObject);
        });

        text.DOFade(0, 2);
    }

   
    public void Move(float speed, int targetPos)
    {
        transform.DOLocalMoveY(targetPos * image.rectTransform.sizeDelta.y, speed);
    }
}