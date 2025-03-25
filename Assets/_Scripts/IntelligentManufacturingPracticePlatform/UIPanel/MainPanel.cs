using MyTreeView;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 智能制造实践平台
/// </summary>
namespace IntelligentManufacturingPracticePlatform
{
    /// <summary>
    /// 智能制造实践平台主界面
    /// </summary>
    public class MainPanel : BasePanel
    {
        /// <summary>
        /// 所有点击之后有响应的菜单
        /// </summary>
        private List<TreeViewItem> treeViewItemsList = new List<TreeViewItem>();
        [SerializeField]
        /// <summary>
        /// 背景图片
        /// </summary>
        private Sprite[] sprites;
        /// <summary>
        /// 当前所处在的节点
        /// </summary>
        public TreeViewItem currentTreeViewItem;
        private TreeViewControl treeView;
        /// <summary>
        /// 目录树形结构
        /// </summary>
        private TreeViewControl TreeView
        {
            get
            {
                if (treeView == null)
                {
                    treeView = transform.FindChildForName("TreeView").GetComponent<TreeViewControl>();
                }
                return treeView;
            }
        }

        private GameObject endImage;
        /// <summary>
        /// 结束图片
        /// </summary>
        private GameObject EndImage
        {
            get
            {
                if (endImage==null)
                {
                    endImage = transform.FindChildForName("EndImage").gameObject;
                }
                return endImage;
            }
        }
        private Toggle treeStateToggle;
        /// <summary>
        /// 结束图片
        /// </summary>
        private Toggle TreeStateToggle
        {
            get
            {
                if (treeStateToggle == null)
                {
                    treeStateToggle = transform.FindChildForName("TreeStateToggle").GetComponent<Toggle>();
                }
                return treeStateToggle;
            }
        }
        private Button exitButton;
        /// <summary>
        /// 退出按钮
        /// </summary>
        private Button ExitButton
        {
            get
            {
                if (exitButton == null)
                {
                    exitButton = transform.FindChildForName("ExitButton").GetComponent<Button>();
                }
                return exitButton;
            }
        }

        private Transform treeViewHidePoint;

        private Transform TreeViewHidePoint
        {
            get
            {
                if (treeViewHidePoint==null)
                {
                    treeViewHidePoint = transform.FindChildForName("TreeViewHidePoint");
                }
                return treeViewHidePoint;
            }
        }
        private Transform treeViewShowPoint;

        private Transform TreeViewShowPoint
        {
            get
            {
                if (treeViewShowPoint == null)
                {
                    treeViewShowPoint = transform.FindChildForName("TreeViewShowPoint");
                }
                return treeViewShowPoint;
            }
        }

        private GameObject maskImage;
        /// <summary>
        /// 遮罩图片，避免菜单收回之后还能被点击
        /// </summary>
        private GameObject MaskImage
        {
            get
            {
                if (maskImage==null)
                {
                    maskImage = transform.FindChildForName("MaskImage").gameObject;
                }
                return maskImage;
            }
        }
        void Start()
        {
            List<TreeViewData> treeViewDatas = XmlController.ReadJsonForLitJson<List<TreeViewData>>(Resources.Load<TextAsset>("TreeViewDataConfig").text);
            TreeView.Data = treeViewDatas;
            TreeView.GenerateTreeView_New();
            //刷新树形菜单
            TreeView.RefreshTreeView();
            //注册子元素的鼠标点击事件
            TreeView.ClickItemEvent += CallBack;
            Set_treeViewItemsList();
            TreeView.CloseAllItem();
            AddAllListener();
            MaskImage.SetActive(false);
        }

        private void AddAllListener()
        {
            TreeStateToggle.onValueChanged.AddListener((b) => On_TreeStateToggle_Change(b));
            ExitButton.onClick.AddListener(()=>On_ExitButton_Click());
        }
        /// <summary>
        /// 推出按钮点击
        /// </summary>
        private void On_ExitButton_Click()
        {
           
            GameFacade.Instance.PushPanel(UIPanelType.ExitPanel);
           
        }

        /// <summary>
        /// 显示隐藏树形结构Toggle
        /// </summary>
        /// <param name="b"></param>
        private void On_TreeStateToggle_Change(bool b)
        {
            if (b)
            {
                BasePanel basePanel = GameFacade.Instance.GetTopPanel();
                switch (basePanel.panelType)
                {
                    case UIPanelType.None:
                        break;
                    case UIPanelType.MainPanel:
                        EndImage.SetActive(false);
                        break;
                    case UIPanelType.TextContentPanel:
                        GameFacade.Instance.PopPanel();
                        break;
                    case UIPanelType.ImageContentPanel:
                        GameFacade.Instance.PopPanel();
                        break;
                    case UIPanelType.VideoContentPanel:
                        GameFacade.Instance.PopPanel();
                        break;
                    case UIPanelType.ExitPanel:

                        break;
                    default:
                        break;
                }
                ShowTree();
                //TreeView.gameObject.SetActive(true);
            }
            else
            {
                HideTree();
               // TreeView.gameObject.SetActive(false);
                if (currentTreeViewItem!=null)
                {
                    CallBack(currentTreeViewItem.gameObject);
                }
            }
        }
        /// <summary>
        /// 显示树形菜单
        /// </summary>
        private void ShowTree()
        {
            TreeView.transform.DOLocalMoveX(TreeViewShowPoint.localPosition.x,0.5f).OnComplete(() => OnShowTree());
        }
        /// <summary>
        /// 显示树形菜单之后回调
        /// </summary>
        private void OnShowTree()
        {
            //TreeStateToggle.transform.FindChildForName("Background").gameObject.SetActive(false);
            TreeView.GetComponent<Image>().sprite = sprites[0];
            MaskImage.SetActive(false);
        }
        /// <summary>
        /// 隐藏树形菜单
        /// </summary>
        private void HideTree()
        {
            TreeView.transform.DOLocalMoveX(TreeViewHidePoint.localPosition.x, 0.5f).OnComplete(() => OnHideTree());
        }
        /// <summary>
        /// 隐藏了树形菜单之后回调
        /// </summary>
        private void OnHideTree()
        {
            //TreeStateToggle.transform.FindChildForName("Background").gameObject.SetActive(true);
            TreeView.GetComponent<Image>().sprite = sprites[1];
            MaskImage.SetActive(true);
        }

        /// <summary>
        /// 展开子物体
        /// </summary>
        /// <param name="id"></param>
        public void ExpandChild(string id)
        {
            TreeViewItem treeViewItem = null;
            foreach (var item in treeViewItemsList)
            {
                if (item.Get_treeViewData().id == id)
                {
                    treeViewItem = item;
                    break;
                }
            }
            if (treeViewItem != null)
            {
                treeViewItem.ExpandAll(true);
                treeViewItem.SetAllParentSelected();
                currentTreeViewItem = treeViewItem;
            }

        }
        /// <summary>
        /// 点击目录之后回调
        /// </summary>
        /// <param name="item"></param>
        private void CallBack(GameObject item)
        {
          
            TreeViewItem treeViewItem = item.GetComponent<TreeViewItem>();
            if (treeViewItem.TreeViewToggle.isOn)
            {
                treeViewItem.TreeViewToggle.isOn = false;
                if (treeViewItem.GetChildrenNumber()!=0)
                {
                    treeView.ResetAllToggle();
                    treeView.CloseAllItem();
                    if (treeViewItem.GetParent()!=null)
                    {
                        treeViewItem.GetParent().ExpandAll();
                        treeViewItem.GetParent().TreeViewToggle.isOn = true;
                    }
                    //treeViewItem.SetAllParentSelected(true);
                }
                currentTreeViewItem = null;
            }
            else
            {
                currentTreeViewItem = null;
                if (item.GetComponent<TreeViewItem>().GetChildrenNumber() != 0)
                {

                    treeView.ResetAllToggle();
                    treeView.CloseAllItem();
                    item.GetComponent<TreeViewItem>().SetAllParentSelected(false);
                    item.GetComponent<TreeViewItem>().TreeViewToggle.isOn = true;


                    item.GetComponent<TreeViewItem>().ContextButtonClick();
                    ExpandParent(item.GetComponent<TreeViewItem>());
                }
                else
                {
                    GameFacade.Instance.SetStepForStepID(item.transform.FindChildForName("TreeViewText").GetComponent<Text>().text);
                }
            }
            // treeViewItem.TreeViewToggle.isOn = true;

        }

        /// <summary>
        /// 展开所有父物体
        /// </summary>
        /// <param name="treeViewItem"></param>
        private void ExpandParent(TreeViewItem treeViewItem)
        {
            if (treeViewItem != null && treeViewItem.GetParent() != null)
            {
                if (!treeViewItem.GetParent().IsExpanding)
                {
                    treeViewItem.GetParent().ContextButtonClick();
                }
                ExpandParent(treeViewItem.GetParent());
            }
        }
        /// <summary>
        /// 设置可点击菜单列表
        /// </summary>
        private void Set_treeViewItemsList()
        {
            TreeViewItem[] treeViewItems = transform.GetComponentsInChildren<TreeViewItem>();
            foreach (var item in treeViewItems)
            {
                if (item.GetChildrenNumber() == 0)
                {
                    treeViewItemsList.Add(item);
                }
            }
            Debug.Log("可点击的菜单长度为" + treeViewItemsList.Count);
        }
        /// <summary>
        /// 设置Item点击
        /// </summary>
        /// <param name="id"></param>
        public void Set_ItemClick(string id)
        {
            foreach (var item in treeViewItemsList)
            {
                if (item.transform.FindChildForName("TreeViewText").GetComponent<Text>().text == id)
                {
                    CallBack(item.gameObject);
                    return;
                }
            }

        }
        /// <summary>
        /// 结束
        /// </summary>
        public void End()
        {
            EndImage.SetActive(true);
            currentTreeViewItem = null;
            TreeStateToggle.isOn = false;

        }
        public override void OnEnter()
        {
            base.OnEnter();
            EndImage.SetActive(false);
        }

        public override void OnPause()
        {
            base.OnPause();
            TreeStateToggle.isOn = false;
        }

        public override void OnResume()
        {
            base.OnResume();
            TreeStateToggle.isOn = true;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}