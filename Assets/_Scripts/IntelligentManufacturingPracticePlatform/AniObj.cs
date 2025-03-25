using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 智能制造实践平台
/// </summary>
namespace IntelligentManufacturingPracticePlatform
{
    /// <summary>
    /// 动画物体
    /// </summary>
    public class AniObj : MonoBehaviour
    {
        public string ID;

        private Animator _animator;

        private Animator animator
        {
            get
            {
                if (_animator==null)
                {
                    _animator = GetComponent<Animator>();
                }
                return _animator;
            }
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="aniName"></param>
        public virtual void PlayAni(string aniName)
        {
            animator.Play(aniName);
            animator.speed = 1;
        }
        /// <summary>
        /// 准备动画
        /// </summary>
        /// <param name="aniName"></param>
        public virtual void AniGetReady(string aniName)
        {
            animator.Play(aniName);
            animator.speed = 0;
        }

        /// <summary>
        /// 动画待机
        /// </summary>
        /// <param name="aniName"></param>
        public virtual void SetIdel()
        {
            animator.Play("idel");
           
        }
    }
}