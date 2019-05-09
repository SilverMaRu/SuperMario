using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Others
{
    public class DestroyEndAnimation : MonoBehaviour
    {
        private Animator animator;
        //private AnimatorStateInfo stateInfo;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (animator != null)
            {
                //stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}