using Lesson_11;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Lesson_30
{
    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Lesson/Abilities/Targeting/Delayed Click Targeting", order = 0)]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] private Texture2D cursorTexture;
        [SerializeField] private Vector2 cursorHotspot;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float areaAffectRadius;
        [SerializeField] Transform targetingPrefab;

        Transform targetingPrefabInstance = null;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            PlayerController playerController = data.GetUser().GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(data, playerController, finished));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finished)
        {
            playerController.enabled = false;
            if (targetingPrefabInstance == null)
                targetingPrefabInstance = Instantiate(targetingPrefab);
            else
                targetingPrefabInstance.gameObject.SetActive(true);

            targetingPrefabInstance.localScale = new Vector3(areaAffectRadius * 2,1, areaAffectRadius * 2);

            while (!data.IsCancled())
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
                RaycastHit raycastHit;
                if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask))
                {
                    targetingPrefabInstance.position = raycastHit.point;
                    if (Input.GetMouseButtonDown(0))
                    {
                        //마우스를 뗄 때 까지 기다린다
                        yield return new WaitWhile(() => Input.GetMouseButton(0));
                        data.SetTragetedPoint(raycastHit.point);
                        data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                        break;
                    }
                }

                yield return null;
            }
            targetingPrefabInstance.gameObject.SetActive(false);
            playerController.enabled = true;
            finished?.Invoke();
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
            foreach (var hits in raycastHits)
            {
                yield return hits.collider.gameObject;
            }
        }

    }
}