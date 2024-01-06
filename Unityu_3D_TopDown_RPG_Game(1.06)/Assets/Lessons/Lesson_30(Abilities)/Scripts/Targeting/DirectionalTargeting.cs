using Lesson_11;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    [CreateAssetMenu(fileName = "DirectionalTargeting", menuName = "Lesson/Abilities/Targeting/DirectionalTargeting", order = 0)]
    public class DirectionalTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float groundOffset = 1;
        public override void StartTargeting(AbilityData data, Action finished)
        {
            RaycastHit raycastHit;
            Ray ray = PlayerController.GetMouseRay();
            if (Physics.Raycast(ray, out raycastHit,1000, layerMask))
            {
                data.SetTragetedPoint(raycastHit.point + ray.direction * groundOffset / ray.direction.y);
            }
            finished?.Invoke();
        }
    }
}