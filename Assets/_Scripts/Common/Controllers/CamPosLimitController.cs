using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 相机位置限制控制器
    /// </summary>
    public class CamPosLimitController : MonoBehaviour
    {
        /// <summary>
        /// 包裹可以移动区域的碰撞
        /// </summary>
        [SerializeField]
        private WorldCenter worldCenter;

      
        private Transform cam;

        private void Awake()
        {
            if (cam==null)
            {
                cam = GameObject.FindObjectOfType<MoveController>().transform;
            }
            if (worldCenter==null)
            {
                worldCenter = GameObject.FindObjectOfType<WorldCenter>();
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}