using Lesson_5;
using Lesson_Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_Common
{
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!enabled) return false;
            if (!CatchComponent(callingController))
            {
                return false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                CatchComponentToAttack(callingController);
            }

            return true;
        }

        public bool CatchComponent(PlayerController callingController)
        {
            var fighter5 = callingController.GetComponent<Lesson_5.Fighter>();
            if (fighter5)
                return fighter5.CanAttack(gameObject);
            var fighter6 = callingController.GetComponent<Lesson_6.Fighter>();
            if (fighter6)
                return fighter6.CanAttack(gameObject);
            var fighter7 = callingController.GetComponent<Lesson_7.Fighter>();
            if (fighter7)
                return fighter7.CanAttack(gameObject);
            return false;
        }
        public void CatchComponentToAttack(PlayerController callingController)
        {
            var fighter5 = callingController.GetComponent<Lesson_5.Fighter>();
            if (fighter5)
            {
                fighter5.Attack(gameObject);
                return;
            }

            var fighter6 = callingController.GetComponent<Lesson_6.Fighter>();
            if (fighter6)
            {
                fighter6.Attack(gameObject);
                return;
            }
            var fighter7 = callingController.GetComponent<Lesson_7.Fighter>();
            if (fighter7)
            {
                fighter7.Attack(gameObject);
                return;
            }
        }


        public bool HandleRaycast(Lesson_11.PlayerController callingController)
        {
            if (!enabled) return false;
            if (!CatchComponent(callingController))
            {
                return false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                CatchComponentToAttack(callingController);
            }

            return true;
        }

        public bool CatchComponent(Lesson_11.PlayerController callingController)
        {
            var fighter11 = callingController.GetComponent<Lesson_11.Fighter>();
            if (fighter11)
                return fighter11.CanAttack(gameObject);
            var fighter12 = callingController.GetComponent<Lesson_12.Fighter>();
            if (fighter12)
                return fighter12.CanAttack(gameObject);
            return false;
        }

        public void CatchComponentToAttack(Lesson_11.PlayerController callingController)
        {
            var fighter11 = callingController.GetComponent<Lesson_11.Fighter>();
            if (fighter11)
            {
                fighter11.Attack(gameObject);
                return;
            }

            var fighter12 = callingController.GetComponent<Lesson_12.Fighter>();
            if (fighter12)
            {
                fighter12.Attack(gameObject);
                return;
            }
        }
    }
}
