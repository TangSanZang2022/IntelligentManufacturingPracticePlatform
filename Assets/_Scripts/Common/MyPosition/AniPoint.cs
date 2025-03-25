using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace DFDJ
{
    public class AniPoint : BasePos
    {
        public override void Init()
        {
            base.Init();
        }

        public override void MoveToPoint(Transform trans)
        {
            // base.MoveToPoint(trans);
            moveObjTransform = trans;
            trans.DOLocalMove(transform.localPosition, moveTime);
            trans.DOLocalRotate(transform.localEulerAngles, moveTime);

        }

        public override void CamArrived()
        {
            base.CamArrived();
        }

        public override void Leave()
        {
            base.Leave();
        }
    }
}