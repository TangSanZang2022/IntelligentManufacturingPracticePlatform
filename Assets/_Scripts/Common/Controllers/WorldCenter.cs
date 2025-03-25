using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Common
{
    /// <summary>
    /// 世界的中心
    /// </summary>
    public class WorldCenter : MonoBehaviour
    {
        /// <summary>
        /// 当超出边界的时候，相机向内移动多远
        /// </summary>
        public float moveDis=5f;

        public int worldCenterLayerIndex = 9;

        public bool getReady;
        private Camera myCamera;

        private void Awake()
        {
            if (myCamera == null)
            {
                myCamera = GameObject.FindObjectOfType<MoveController>().GetComponent<Camera>();
                Debug.Log(111);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (!getReady)
            {
                ReturnToCenter();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if ( other.tag == myCamera.tag)
            {
                getReady = true;
                myCamera.GetComponent<MoveController>().enabled = true; 
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag== myCamera.tag)
            {
                getReady = false;
                myCamera.GetComponent<MoveController>().enabled = false;
              
            }
        }
        private void Update()
        {
            if (myCamera.GetComponent<MoveController>().enabled == false)
            {
                ReturnToCenter();
            }
        }
        private void ReturnToCenter()
        {
            
            RaycastHit hit;
            if (Physics.Linecast(myCamera.transform.position, transform.position, out hit, 1 << worldCenterLayerIndex))
            {
                Vector3 dir = (transform.position-hit.point ).normalized;
               // myCamera.transform.position= dir * moveDis;
                myCamera.transform.position= Vector3.Lerp(myCamera.transform.position, myCamera.transform.position + dir * moveDis,0.5f);
            }
            else
            {
                Debug.Log(transform.name + "相机边界检测失败");
            }
        }
    }
}