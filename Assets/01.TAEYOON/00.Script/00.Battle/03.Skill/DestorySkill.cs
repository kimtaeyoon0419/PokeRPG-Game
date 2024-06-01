namespace PokeRPG.Battle.Skill
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;

    public class DestorySkill : MonoBehaviour
    {
        public float delay;
        private WaitForSeconds waitForSeconds;

        private void Start()
        {
            waitForSeconds = new WaitForSeconds(delay);
            StartCoroutine(Co_delete());
        }

        IEnumerator Co_delete()
        {
            yield return waitForSeconds;
            Destroy(gameObject);
        }
    }
}