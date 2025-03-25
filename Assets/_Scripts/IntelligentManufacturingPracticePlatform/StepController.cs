using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 智能制造实践平台
/// </summary>
namespace IntelligentManufacturingPracticePlatform
{
    /// <summary>
    /// 步骤控制器
    /// </summary>
    public class StepController : BaseController
    {

        private int currentStepIndex;
        /// <summary>
        /// 当前步骤
        /// </summary>
        private int CurrentStepIndex
        {
            get
            {
                return currentStepIndex;
            }

            set
            {
                StepData stepData_Last = GetStepData(currentStepIndex);
                StepData stepData = GetStepData(value);
                if (stepData_Last != null && stepData != null && GetStepData(currentStepIndex).type == GetStepData(value).type)//类型相同时，就不用再次弹出界面
                {
                    BasePanel basePanel = facade.GetTopPanel();
                    if (basePanel.panelType == GetStepData(value).type.ToEnum<UIPanelType>() && !basePanel.gameObject.activeSelf)
                    {
                        basePanel.OnEnter();
                    }
                    else if (basePanel.panelType != GetStepData(value).type.ToEnum<UIPanelType>())
                    {
                        basePanel = facade.PushPanel(GetStepData(value).type.ToEnum<UIPanelType>());
                    }
                    basePanel.UpdatePanelData(stepData);
                }
                else//面板类型不同时需要更换面板
                {
                    if (value >= 0)
                    {
                        if (facade.GetTopPanel().panelType != UIPanelType.MainPanel)
                        {
                            facade.PopPanel();
                        }
                        UIPanelType uIPanelType = stepData.type.ToEnum<UIPanelType>();
                        BasePanel basePanel = facade.PushPanel(uIPanelType);
                        basePanel.UpdatePanelData(stepData);
                    }
                }
                if (value >= 0)
                {
                    MainPanel mainPanel = GameFacade.Instance.GetUIPanel(UIPanelType.MainPanel) as MainPanel;
                    if (mainPanel != null)
                    {
                        mainPanel.ExpandChild(GetStepData(value).id);
                    }
                }
                currentStepIndex = value;

                if (GetStepData(currentStepIndex)!=null)
                {
                    GameFacade.Instance.SetTarnsToPos(GetStepData(currentStepIndex).Get_posID(), Camera.main.transform);
                }
                else
                {
                    GameFacade.Instance.SetTarnsToPos("StartePos", Camera.main.transform);
                }
                
            }
        }
        /// <summary>
        /// 存放所有步骤的列表
        /// </summary>
        public List<StepData> allStepList = new List<StepData>();
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="gameFacade"></param>
        public StepController(GameFacade gameFacade) : base(gameFacade)
        {

        }

        public override void OnInit()
        {
            base.OnInit();
            CurrentStepIndex = -1;



            allStepList = XmlController.ReadJsonForLitJson<List<StepData>>(Resources.Load<TextAsset>("StepDataConfig").text);
            Debug.Log(allStepList.Count);
        }
        /// <summary>
        /// 获取当前步骤数据
        /// </summary>
        public StepData GetStepData(int stepIndex)
        {

            if (stepIndex >= 0 && stepIndex < allStepList.Count)
            {
                return allStepList[stepIndex];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取当前步骤数据
        /// </summary>
        public StepData GetCurrentStepData()
        {

            if (CurrentStepIndex >= 0 && CurrentStepIndex < allStepList.Count)
            {
                return allStepList[CurrentStepIndex];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 下一步
        /// </summary>
        public void GotoNextStep()
        {
            if (CurrentStepIndex < allStepList.Count - 1)
            {
                CurrentStepIndex++;
            }
            else
            {
                Debug.Log("已经到达最后一步");
                if (facade.GetTopPanel().panelType != UIPanelType.MainPanel)
                {
                    facade.PopPanel();
                }
                MainPanel mainPanel = facade.GetTopPanel() as MainPanel;
                mainPanel.End();
            }
        }
        /// <summary>
        /// 上一步
        /// </summary>
        public void GotoLastStep()
        {
            if (CurrentStepIndex > 0)
            {
                CurrentStepIndex--;
            }
            else
            {
                Debug.Log("已经到达第一步");
            }
        }
        /// <summary>
        /// 设置当前步骤
        /// </summary>
        /// <param name="stepIndex"></param>
        public void Set_CurrentStepIndex(int stepIndex)
        {
            CurrentStepIndex = stepIndex;
        }
        /// <summary>
        /// 根据步骤ID来设置步骤
        /// </summary>
        /// <param name="StepID"></param>
        public void SetStepForStepID(string StepID)
        {
            int index = -1;
            foreach (var item in allStepList)
            {
                index++;
                if (item.id == StepID)
                {
                    CurrentStepIndex = index;
                    Debug.Log(string.Format("当前步骤ID为{0}，步骤序号为{1}", StepID, CurrentStepIndex));
                    return;
                }
            }
            Debug.LogWarning(string.Format("设置当前步骤ID为{0}失败", StepID));
        }
    }
    [Serializable]
    public class StepData
    {
        /// <summary>
        /// 标题名称
        /// </summary>
        public string titleName;
        /// <summary>
        /// 面板类型
        /// </summary>
        public string type;
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// ID 
        /// </summary>
        public string id;
        /// <summary>
        /// 描述文字
        /// </summary>
        public string describe;
        /// <summary>
        /// 语音路径
        /// </summary>
        public string soundPath;
        /// <summary>
        /// 图片路径
        /// </summary>
        public List<string> imagePathArray;
        /// <summary>
        /// 播放的视频路径
        /// </summary>
        public string VideoPath;

        /// <summary>
        /// 相机位置ID
        /// </summary>
        public string posID;
        /// <summary>
        /// 动画准备配置，用于初始化本步骤的显示隐藏物体
        /// </summary>
        public string aniReadyConfig;
        /// <summary>
        /// 动画配置
        /// </summary>
        public string aniConfig;
        /// <summary>
        /// 获取位置ID
        /// </summary>
        /// <returns></returns>
        public  string Get_posID()
        {
            if (string.IsNullOrEmpty(posID))
            {
                return "DefaultPos";
            }
            return posID;
        }
    }
}
