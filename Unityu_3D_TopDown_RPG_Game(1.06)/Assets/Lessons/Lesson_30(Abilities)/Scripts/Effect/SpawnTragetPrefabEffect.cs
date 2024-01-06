using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lesson_30
{
    [CreateAssetMenu(fileName = "Spawn Traget Prefab Effect", menuName = "Lesson/Effects/SpawnTragetPrefabEffect", order = 0)]
    public class SpawnTragetPrefabEffect : EffectStrategy
    {
        [SerializeField] Transform prefabToSpawn;
        [SerializeField] float destroyDelay = -1;
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data , Action finished)
        {
            Transform instance = Instantiate(prefabToSpawn);
            instance.position = data.GetTragetedPoint();
            if (destroyDelay > 0) 
            {
                yield return new WaitForSeconds(destroyDelay);
                Destroy(instance.gameObject);
            }
            finished?.Invoke();
        }
    }
}