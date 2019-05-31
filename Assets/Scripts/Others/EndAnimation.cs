using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Others
{
    public class EndAnimation : MonoBehaviour
    {
        public int layerIndex = 0;
        public string clipName = "";

        private System.Action EndAction;
        private Animator animator;

        private bool done = false;

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
                AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(layerIndex);

                if (info.IsName(clipName) && info.normalizedTime >= 1 && !done)
                {
                    if (EndAction == null)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        EndAction();
                    }
                    done = true;
                }
                else if (done && !info.IsName(clipName))
                {
                    done = false;
                }
            }
        }

        public void SetEndAction(System.Action endAction)
        {
            EndAction = endAction;
        }
    }
}
