namespace PokeRPG.Battle.UI
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;

    public class TextBox : MonoBehaviour
    {
        private Animator animator;

        private readonly int hashTextBoxUp = Animator.StringToHash("Up");
        private readonly int hashTextBoxDown = Animator.StringToHash("Down");

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void BoxUp()
        {
            animator.SetTrigger(hashTextBoxUp);
        }

        public void BoxDown()
        {
            animator.SetTrigger(hashTextBoxDown);
        }
    }
}
