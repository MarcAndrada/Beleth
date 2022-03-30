using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasBossController : MonoBehaviour
{
    [Header("Bosses")]
    [SerializeField]
    WrathBossStateController BossStateController;
    [Header("ImageCanvas")]
    [SerializeField]
    Slider slider;

    float maxHp;

    private void Start()
    {
        maxHp = BossStateController.GetMaxLife();
    }

    private void Update()
    {
        slider.value = BossStateController.GetActualLife() / maxHp;
    }
}
