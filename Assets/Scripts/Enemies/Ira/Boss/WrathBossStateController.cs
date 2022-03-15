using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WrathBossStateController : MonoBehaviour
{
    public enum BossFase { NONE, FASE_1, FASE_2, FASE_3 };
    public enum BossState { NONE, BELOW_FLOOR, ATTACKING, DEAD };
    public enum BossAttacks { NONE, LAVA_CIRCLE, ENEMIE_CIRCLE, BRAKE_FLOOR, FIRE_COLUMN, ROCK_RAIN };

    private BossFase currentFase = BossFase.NONE;
    private BossState currentState = BossState.NONE;
    private BossAttacks currentAttacks = BossAttacks.NONE;

    [Header("Boss Stats")]
    [SerializeField]
    private float maxHP;
    private float currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    // ExternActions
    public void GetDamage(float _damageDealt) {

        currentHP -= _damageDealt;
    }

}
