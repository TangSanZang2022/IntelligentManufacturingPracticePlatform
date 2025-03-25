using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
/// <summary>
/// 智能制造实践平台
/// </summary>
namespace IntelligentManufacturingPracticePlatform
{

    /// <summary>
    /// 展示内容基类
    /// </summary>
    public class BaseContentPanel : BasePanel
    {

        protected StepData stepData;
        private Text titleText;
        /// <summary>
        /// 标题Text
        /// </summary>
        protected Text TitleText
        {
            get
            {
                if (titleText == null)
                {
                    titleText = transform.FindChildForName<Text>("TitleText");
                }
                return titleText;
            }
        }
        private Text contentText;
        /// <summary>
        /// 内容Text
        /// </summary>
        protected Text ContentText
        {
            get
            {
                if (contentText == null)
                {
                    contentText = transform.FindChildForName<Text>("ContentText");
                }
                return contentText;
            }
        }

        private Button lastItemButton;
        /// <summary>
        /// 上一步按钮
        /// </summary>
        protected Button LastItemButton
        {
            get
            {
                if (lastItemButton == null)
                {
                    lastItemButton = transform.FindChildForName<Button>("LastItemButton");
                }
                return lastItemButton;
            }
        }
        private Button nextItemButton;
        /// <summary>
        /// 下一步按钮
        /// </summary>
        protected Button NextItemButton
        {
            get
            {
                if (nextItemButton == null)
                {
                    nextItemButton = transform.FindChildForName<Button>("NextItemButton");
                }
                return nextItemButton;
            }
        }
        private Button backButton;
        /// <summary>
        /// 返回按钮
        /// </summary>
        protected Button BackButton
        {
            get
            {
                if (backButton == null)
                {
                    backButton = transform.FindChildForName<Button>("BackButton");
                }
                return backButton;
            }
        }
        private Button playAniButton;
        /// <summary>
        /// 播放动画按钮
        /// </summary>
        protected Button PlayAniButton
        {
            get
            {
                if (playAniButton == null)
                {
                    playAniButton = transform.FindChildForName<Button>("PlayAniButton");
                }
                return playAniButton;
            }
        }
        private VerticalLayoutGroup content;
        /// <summary>
        /// 竖向排版组件
        /// </summary>
        protected VerticalLayoutGroup Content
        {
            get
            {
                if (content == null)
                {
                    content = transform.FindChildForName<VerticalLayoutGroup>("Content");
                }
                return content;
            }
        }
        private Scrollbar scrollbarVertical;
        /// <summary>
        /// 滑块
        /// </summary>
        protected Scrollbar ScrollbarVertical
        {
            get
            {
                if (scrollbarVertical == null)
                {
                    scrollbarVertical = transform.FindChildForName<Scrollbar>("Scrollbar Vertical");
                }
                return scrollbarVertical;
            }
        }


        private AudioSource _audio;
        /// <summary>
        /// 语音播放器
        /// </summary>
        private AudioSource Audio
        {
            get
            {
                if (_audio == null)
                {
                    _audio = GetComponent<AudioSource>();
                }
                return _audio;
            }
        }
        protected virtual void Start()
        {
            AllAllListeners();
        }
        protected virtual void AllAllListeners()
        {
            BackButton.onClick.AddListener(() => On_BackButtonClick());
            NextItemButton.onClick.AddListener(() => On_NextItemButtonClick());
            LastItemButton.onClick.AddListener(() => On_LastItemButtonClick());
            PlayAniButton.onClick.AddListener(() => On_PlayAniButtonClick());
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        protected virtual void On_PlayAniButtonClick()
        {
            PlayAniButton.gameObject.SetActive(false);
            Debug.Log("播放动画"+stepData.aniConfig);
            GameFacade.Instance.PlayAniForAniConfig(stepData.aniConfig);
        }

        /// <summary>
        /// 返回按钮点击事件
        /// </summary>
        private void On_BackButtonClick()
        {
            PlayIdelAni();
            GameFacade.Instance.RestAllObjState();
           
            GameFacade.Instance.PopPanel();
            GameFacade.Instance.Set_CurrentStepIndex(-1);
        }
        /// <summary>
        /// 下一步按钮点击事件
        /// </summary>
        public void On_NextItemButtonClick()
        {
            PlayIdelAni();
            GameFacade.Instance.RestAllObjState();
            MainPanel mainPanel= GameFacade.Instance.GetUIPanel(UIPanelType.MainPanel) as MainPanel;
            mainPanel. currentTreeViewItem = null;
            GameFacade.Instance.GotoNextStep();
           
        }
        /// <summary>
        /// 上一步按钮点击事件
        /// </summary>
        public void On_LastItemButtonClick()
        {
            PlayIdelAni();
            GameFacade.Instance.RestAllObjState();
            MainPanel mainPanel = GameFacade.Instance.GetUIPanel(UIPanelType.MainPanel) as MainPanel;
            mainPanel.currentTreeViewItem = null;
            GameFacade.Instance.GotoLastStep();
          
        }
        public override void OnEnter()
        {
            base.OnEnter();
            gameObject.SetActive(true);
        }

        public override void OnPause()
        {
            base.OnPause();
            //gameObject.SetActive(false);
            Audio.Pause();
        }

        public override void OnResume()
        {
            base.OnResume();
            gameObject.SetActive(true);
            Audio.Play();
        }

        public override void OnExit()
        {
            base.OnExit();
            gameObject.SetActive(false);
        }

        public override void UpdatePanelData(object panelData)
        {
            base.UpdatePanelData(panelData);
            stepData = panelData as StepData;
            TitleText.text = stepData.titleName;
            ContentText.text = XmlController.ReadStringFromStreamingAssets(stepData.describe).Replace(" ", "\u3000");
            Audio.Stop();
            StartCoroutine(IWaitContentFix(() => Content.childForceExpandHeight = false, delegate
            {
                Content.childForceExpandHeight = true;
                ScrollbarVertical.value = 1;
                string path = string.Empty;
                if (!string.IsNullOrEmpty(stepData.soundPath))
                {
                    path = Application.streamingAssetsPath + "/" + stepData.soundPath;
                }
                StartCoroutine(IPlayAudio(path, PlayAudio));

            }));


            if (string.IsNullOrEmpty(stepData.aniConfig))
            {
                PlayAniButton.gameObject.SetActive(false);
            }
            else
            {
                PlayAniButton.gameObject.SetActive(true);
                GameFacade.Instance.SetShowHideObj(stepData.aniReadyConfig);
                GameFacade.Instance.ReadyAniForAniConfig(stepData.aniConfig);
            }


        }
        /// <summary>
        /// 等待自适应文本框长度
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        IEnumerator IWaitContentFix(Action first, Action second)
        {
            first();
            yield return 0;
            second();
        }
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IEnumerator IPlayAudio(string path, Action<AudioClip> action)
        {
            if (!string.IsNullOrEmpty(path))
            {
                UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(path, 0);
                yield return unityWebRequest.SendWebRequest();
                if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
                {
                    Debug.Log(path + "路径加载音频失败");
                    yield break;
                }
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
                action(audioClip);
            }
        }
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="clip"></param>
        private void PlayAudio(AudioClip clip)
        {
            Audio.clip = clip;
            Audio.Play();
        }

        private void PlayIdelAni()
        {
            if (!string.IsNullOrEmpty(stepData.aniConfig))
            {
                GameFacade.Instance.SetAniToIdel(stepData.aniConfig);
            }
          
        }
    }
}