using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 智能制造实践平台
/// </summary>
namespace IntelligentManufacturingPracticePlatform
{
    /// <summary>
    /// 图片展示界面
    /// </summary>
    public class ImageContentPanel : BaseContentPanel
    {

        private int currentImageIndex;

        /// <summary>
        /// 当前图片
        /// </summary>
        private int CurrentImageIndex
        {
            get
            {
                return currentImageIndex;
            }
            set
            {
                if (value >= 0 && value < sprites.Count)
                {
                    ContentImage.sprite = sprites[value];
                }
                else
                {
                    value = currentImageIndex;
                }


                currentImageIndex = value;
                SetImageChangeBtnState();
            }
        }
        /// <summary>
        /// 所有图片
        /// </summary>
        private List<Sprite> sprites = new List<Sprite>();

        private Image contentImage;
        /// <summary>
        /// 展示图片Image
        /// </summary>
        private Image ContentImage
        {
            get
            {
                if (contentImage == null)
                {
                    contentImage = transform.FindChildForName<Image>("ContentImage");
                }
                return contentImage;
            }
        }

        private Button lastImageButton;
        /// <summary>
        /// 上一个照片
        /// </summary>
        private Button LastImageButton
        {
            get
            {
                if (lastImageButton == null)
                {
                    lastImageButton = transform.FindChildForName("LastImageButton").GetComponent<Button>();
                }
                return lastImageButton;
            }
        }
        private Button nextImageButton;
        /// <summary>
        /// 下一个照片
        /// </summary>
        private Button NextImageButton
        {
            get
            {
                if (nextImageButton == null)
                {
                    nextImageButton = transform.FindChildForName("NextImageButton").GetComponent<Button>();
                }
                return nextImageButton;
            }
        }
        protected override void AllAllListeners()
        {
            base.AllAllListeners();
            LastImageButton.onClick.AddListener(() => On_LastImageButton());
            NextImageButton.onClick.AddListener(() => On_NextImageButton());
        }
        /// <summary>
        /// 设置图片切换按钮状态
        /// </summary>
        private void SetImageChangeBtnState()
        {
            int totalCount = sprites.Count;
            if (totalCount > 1)
            {
                if (totalCount - CurrentImageIndex >1)
                {
                    NextImageButton.gameObject.SetActive(true);
                }
                else
                {
                    NextImageButton.gameObject.SetActive(false);
                }

                if (CurrentImageIndex>0)
                {
                    LastImageButton.gameObject.SetActive(true);
                }
                else
                {
                    LastImageButton.gameObject.SetActive(false);
                }
            }
            else
            {
                LastImageButton.gameObject.SetActive(false);
                NextImageButton.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 上一张图片
        /// </summary>
        private void On_LastImageButton()
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex--;
            }
        }
        /// <summary>
        /// 下一张图片
        /// </summary>
        private void On_NextImageButton()
        {
            if (CurrentImageIndex < sprites.Count)
            {
                CurrentImageIndex++;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
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


        protected override void On_PlayAniButtonClick()
        {
            base.On_PlayAniButtonClick();
            ContentImage.gameObject.SetActive(false);
        }
        public override void UpdatePanelData(object panelData)
        {
            ContentImage.gameObject.SetActive(true);
            base.UpdatePanelData(panelData);
            
            sprites.Clear();
            foreach (var item in stepData.imagePathArray)
            {
                sprites.Add(Resources.Load<Sprite>(item));
            }
            CurrentImageIndex = 0;
        }
    }
}