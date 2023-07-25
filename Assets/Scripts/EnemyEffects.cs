using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnemyManager;

public class EnemyEffects : MonoBehaviour
{
    [SerializeField] 
    private float maxValue = 50;

    [SerializeField]
    private List<EffectStruct> effects = new List<EffectStruct>();
    private Dictionary<EnemyType, EffectStruct> effectStructs = new Dictionary<EnemyType, EffectStruct>();

    [Serializable]
    public struct EffectStruct
    {
        public EnemyType enemyType;
        public float currentValue;
        public Slider positiveSlider;
        public float defaultEffect;
        public float maxEffect;
        public float effectChange;
        public EnemyType effectedEnemyType;
        public EnemyType secondaryEnemyEffect;
    }

    private void Awake()
    {
        foreach (EffectStruct effect in effects)
        {
            effect.positiveSlider.maxValue = maxValue;
            effectStructs.Add(effect.enemyType, effect);
        }
    }

    public void AddEffect(EnemyType enemyType)
    {
        EditValues(enemyType, 1);
        EditValues(effectStructs[enemyType].effectedEnemyType, -1);
        EditValues(effectStructs[enemyType].secondaryEnemyEffect, -.5f);
        UpdateEffects(enemyType);
        UpdateEffects(effectStructs[enemyType].effectedEnemyType);
        UpdateEffects(effectStructs[enemyType].secondaryEnemyEffect);
    }

    private void EditValues(EnemyType enemyType, float modifier)
    {
        EffectStruct enemyEffectStruct = effectStructs[enemyType];
        enemyEffectStruct.currentValue = Mathf.Clamp(enemyEffectStruct.currentValue + enemyEffectStruct.effectChange * modifier, 0, maxValue);
        enemyEffectStruct.positiveSlider.value = Mathf.Abs(enemyEffectStruct.currentValue);
        effectStructs[enemyType] = enemyEffectStruct;
    }

    private void UpdateEffects(EnemyType enemyType)
    {
        EffectStruct enemyEffectStruct = effectStructs[enemyType];
        float effectValue = 0;
        if (enemyEffectStruct.maxEffect > enemyEffectStruct.defaultEffect)
        {
            effectValue = enemyEffectStruct.defaultEffect + (enemyEffectStruct.currentValue / maxValue) * (enemyEffectStruct.maxEffect - enemyEffectStruct.defaultEffect);
        }
        else
        {
            effectValue = enemyEffectStruct.defaultEffect - (enemyEffectStruct.currentValue / maxValue) * (enemyEffectStruct.defaultEffect - enemyEffectStruct.maxEffect);
        }
        switch (enemyType)
        {
            case EnemyType.Happiness:
                PlayerCharacter.Instance.SetOrthographicSize(effectValue);
                break;
            case EnemyType.Sadness:
                PlayerCharacter.Instance.SetSpeedMod(effectValue);
                break;
            case EnemyType.Fear:
                EnemyManager.Instance.SetFearful(effectValue);
                break;
            default:
                PlayerCharacter.Instance.SetAccuracy(effectValue);
                break;
        }
    }
}
