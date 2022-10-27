using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enablewin : MonoBehaviour
{
    private void OnEnable()
    {
        this.transform.DOShakePosition(30, 15, 35, 90);
    }
    private void OnDisable()
    {
        DOTween.KillAll();
    }
}
