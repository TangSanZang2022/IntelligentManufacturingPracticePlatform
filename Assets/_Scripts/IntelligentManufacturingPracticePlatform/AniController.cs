using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 智能制造实践平台
/// </summary>
namespace IntelligentManufacturingPracticePlatform
{
    public class AniController : BaseController
    {
       public AniController(GameFacade gameFacade) : base(gameFacade) { }


        private Dictionary<string, AniObj> aniObjDict = new Dictionary<string, AniObj>();
        /// <summary>
        /// 设置字典
        /// </summary>
        private void Set_aniObjDict()
        {
            AniObj[] aniObjs = GameObject.FindObjectsOfType<AniObj>();
            foreach (var item in aniObjs)
            {
                aniObjDict.Add(item.ID, item);
            }
        }
        public override void OnInit()
        {
            base.OnInit();
            Set_aniObjDict();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnDestory()
        {
            base.OnDestory();
        }
        /// <summary>
        /// 根据动画配置播放动画，格式为"AniObjName1:stateName1&AniObjName2:stateName2……"
        /// </summary>
        /// <param name="aniConfigItem"></param>
        public void PlayAniForAniConfig(string aniConfig)
        {
            string[] aniConfigItemArray = aniConfig.Split('&');
            for (int i = 0; i < aniConfigItemArray.Length; i++)
            {
                int index = i;
                if (!string.IsNullOrEmpty(aniConfigItemArray[index]))
                {
                    string[] AniObjIDAndNameArray = aniConfigItemArray[index].Split(':');
                    AniObj aniObj = aniObjDict[AniObjIDAndNameArray[0]];
                    aniObj.PlayAni(AniObjIDAndNameArray[1]);
                }
            }
        }
        /// <summary>
        /// 根据动画配置准备动画，格式为"AniObjName1:stateName1&AniObjName2:stateName2……"
        /// </summary>
        /// <param name="aniConfigItem"></param>
        public void ReadyAniForAniConfig(string aniConfig)
        {
            string[] aniConfigItemArray = aniConfig.Split('&');
            for (int i = 0; i < aniConfigItemArray.Length; i++)
            {
                int index = i;
                if (!string.IsNullOrEmpty(aniConfigItemArray[index]))
                {
                    string[] AniObjIDAndNameArray = aniConfigItemArray[index].Split(':');
                    AniObj aniObj = aniObjDict[AniObjIDAndNameArray[0]];
                    aniObj.AniGetReady(AniObjIDAndNameArray[1]);
                }
            }
        }/// <summary>
         /// 根据动画配置准备动画，格式为"AniObjName1:stateName1&AniObjName2:stateName2……"
         /// </summary>
         /// <param name="aniConfigItem"></param>
        public void SetAniToIdel(string aniConfig)
        {
            string[] aniConfigItemArray = aniConfig.Split('&');
            for (int i = 0; i < aniConfigItemArray.Length; i++)
            {
                int index = i;
                if (!string.IsNullOrEmpty(aniConfigItemArray[index]))
                {
                    string[] AniObjIDAndNameArray = aniConfigItemArray[index].Split(':');
                    AniObj aniObj = aniObjDict[AniObjIDAndNameArray[0]];
                    aniObj.SetIdel();
                }
            }
        }
    }
}