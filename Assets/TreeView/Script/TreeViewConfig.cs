using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyTreeView
{ /// <summary>
  /// 树结构配置列表
  /// </summary>
    [CreateAssetMenu(fileName = "TreeViewConfig", menuName = ("ScriptableObject/TreeView/TreeViewDataConfig"))]
    public class TreeViewConfig : ScriptableObject
    {
        public List<TreeViewData> datas = new List<TreeViewData>();
    }
}
