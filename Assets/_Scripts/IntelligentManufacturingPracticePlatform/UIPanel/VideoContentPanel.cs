using RenderHeads.Media.AVProVideo.Demos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFramework;
using UnityEngine.EventSystems;
/// <summary>
/// 智能制造实践平台
/// </summary>
namespace IntelligentManufacturingPracticePlatform
{
    /// <summary>
    /// 视频展示界面
    /// </summary>
    public class VideoContentPanel : BaseContentPanel
    {
        private VCR vcr;
        /// <summary>
        /// 视频播放VCR
        /// </summary>
        private VCR vCR
        {
            get
            {
                if (vcr==null)
                {
                    vcr = transform.FindChildForName<VCR>("VCR");
                }
                return vcr;
            }
        }

        private Button playButton;
        /// <summary>
        /// 播放按钮
        /// </summary>
        private Button PlayButton
        {
            get
            {
                if (playButton==null)
                {
                    playButton = transform.FindChildForName<Button>("PlayButton");
                }
                return playButton;
            }
        }
        private Button pauseButton;
        /// <summary>
        /// 暂停按钮
        /// </summary>
        private Button PauseButton
        {
            get
            {
                if (pauseButton == null)
                {
                    pauseButton = transform.FindChildForName<Button>("PauseButton");
                }
                return pauseButton;
            }
        }
        private Button closeVideoButton;
        /// <summary>
        /// 关闭视频按钮
        /// </summary>
        private Button CloseVideoButton
        {
            get
            {
                if (closeVideoButton == null)
                {
                    closeVideoButton = transform.FindChildForName<Button>("CloseVideoButton");
                }
                return closeVideoButton;
            }
        }

        private GameObject panel;
        /// <summary>
        /// 视频播放面板
        /// </summary>
        private GameObject Panel
        {
            get
            {
                if (panel==null)
                {
                    panel = transform.FindChildForName("Panel").gameObject;
                }
                return panel;
            }
        }

        private Transform startPauseImage;
        /// <summary>
        /// 开始暂停图片
        /// </summary>
        private Transform StartPauseImage
        {
            get
            {
                if (startPauseImage==null)
                {
                    startPauseImage = transform.FindChildForName("StartPauseImage");
                }
                return startPauseImage;
            }
        }
        protected override void Start()
        {
            base.Start();
            Panel.SetActive(true);
            PauseButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(false);
        }


        protected override void AllAllListeners()
        {
            base.AllAllListeners();
           // PlayButton.onClick.AddListener(() => On_PlayButton_Click());
           // PauseButton.onClick.AddListener(() => On_PauseButton_Click());
            CloseVideoButton.onClick.AddListener(() => On_CloseVideoButton_Click());
            UIEventListener.GetUIEventListener(StartPauseImage).pointEnterHandler += On_StartPauseImage_pointEnter;
            UIEventListener.GetUIEventListener(StartPauseImage).pointClickHandler += On_StartPauseImageClick;
            UIEventListener.GetUIEventListener(StartPauseImage).pointExitHandler += On_StartPauseImage_pointExit;


        }
        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="eventData"></param>
        private void On_StartPauseImage_pointEnter(PointerEventData eventData)
        {
            if (vcr.PlayingPlayer.Control.IsPlaying())//正在播放
            {
                PauseButton.gameObject.SetActive(true);
            }
            
        }
        /// <summary>
        /// 点击开始暂停按钮
        /// </summary>
        /// <param name="eventData"></param>
        private void On_StartPauseImageClick(PointerEventData eventData)
        {
            if (vcr.PlayingPlayer.Control.IsPlaying())//正在播放
            {
                vcr.OnPauseButton();
                On_PauseButton_Click();
            }
            else
            {
                vcr.OnPlayButton();
                On_PlayButton_Click();
            }
        }
        /// <summary>
        /// 鼠标移除
        /// </summary>
        /// <param name="eventData"></param>
        private void On_StartPauseImage_pointExit(PointerEventData eventData)
        {
            if (vcr.PlayingPlayer.Control.IsPlaying())//正在播放
            {
                PauseButton.gameObject.SetActive(false);
            }
        }

       

       

        /// <summary>
        /// 
        /// </summary>
        private void On_PlayButton_Click()
        {
            PlayButton.gameObject.SetActive(false);
            PauseButton.gameObject.SetActive(true);
        }
        /// <summary>
        /// 
        /// </summary>
        private void On_PauseButton_Click()
        {
            PlayButton.gameObject.SetActive(true);
            PauseButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// 关闭视频按钮点击
        /// </summary>
        private void On_CloseVideoButton_Click()
        {
            vCR.OnPauseButton();
            Panel.SetActive(false);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Panel.SetActive(true);
            PauseButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(false);
            vCR._videoSeekSlider.value = 0;
        }

        public override void OnPause()
        {
            base.OnPause();
        }
        public override void OnResume()
        {
            base.OnResume();
        }
        public override void OnExit()
        {
            base.OnExit();
        }

        public override void UpdatePanelData(object panelData)
        {
            base.UpdatePanelData(panelData);
            Panel.SetActive(true);
            PauseButton.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(false);
            vCR._videoFiles = new string[] { stepData.VideoPath };
         ;
            //vCR._mediaPlayer.m_VideoPath = vCR._videoFiles[0];
            //vCR._mediaPlayerB.m_VideoPath = vCR._videoFiles[0];
           
            StartCoroutine(IWaitToPlayAudio());
        }

        IEnumerator IWaitToPlayAudio()
        {
            yield return 0;
               //yield return new WaitForSeconds(1f);
               //vCR._mediaPlayer.Play();
               //vCR._mediaPlayerB.Play();
               //vCR.OnPlayButton();
               vCR.OnOpenVideoFile();
            //vCR.OnPlayButton();
        }
    }
}
