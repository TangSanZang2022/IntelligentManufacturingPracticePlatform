using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTreeView;
using System;

public class MyTreeViewTest : MonoBehaviour
{
    public TreeViewConfig treeViewDataConfig;

    public TreeViewControl TreeView;
    // Start is called before the first frame update
    void Start()
    {
        TreeView.Data = treeViewDataConfig.datas;
        //重新生成树形菜单
        TreeView.GenerateTreeView();
        //刷新树形菜单
        TreeView.RefreshTreeView();
        //注册子元素的鼠标点击事件
        TreeView.ClickItemEvent += CallBack;
        TreeView.CloseAllItem();
    }

    private void CallBack(GameObject item)
    {
        Debug.Log(item.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
