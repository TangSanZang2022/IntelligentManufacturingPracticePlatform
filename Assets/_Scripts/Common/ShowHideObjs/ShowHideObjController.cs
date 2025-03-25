using ShowHideObj;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 显示隐藏物体控制器
/// </summary>
public class ShowHideObjController : BaseController
{
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="gameFacade"></param>
    public ShowHideObjController(GameFacade gameFacade) : base(gameFacade)
    { }
    /// <summary>
    /// 存放所有需要显示隐藏的物体
    /// </summary>
    private Dictionary<string, BaseShowHideObj> allShowHideObjDict = new Dictionary<string, BaseShowHideObj>();
    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        SetAllShowHideObjDict();
    }


    /// <summary>
    /// 更新
    /// </summary>
    public override void OnUpdate()
    {
        
    }

    /// <summary>
    /// 将场景中所有需要显示隐藏的物体放入字典中
    /// </summary>
    private void SetAllShowHideObjDict()
    {
        allShowHideObjDict.Clear();
        BaseShowHideObj[]showHideObjs = GameObject.FindObjectsOfType<BaseShowHideObj>();
        foreach (BaseShowHideObj bs in showHideObjs)
        {
            allShowHideObjDict.Add(bs.GetID(), bs);
          
        }
        foreach (BaseShowHideObj bs in allShowHideObjDict.Values)
        {
            bs.Oninit();
        }
    }
    /// <summary>
    /// 重置所有物体为初始状态
    /// </summary>
    public void RestAllObjState()
    {
        foreach (var item in new List<string>(allShowHideObjDict.Keys))
        {
            allShowHideObjDict[item].ResetToNormalState();
        }
       
    }
    /// <summary>
    /// 根据ID来找到显示隐藏的物体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BaseShowHideObj GetShowHideObjForID(string id)
    {
        BaseShowHideObj baseShowHideObj;
        if (!allShowHideObjDict.TryGetValue(id,out baseShowHideObj))
        {
            Debug.LogError(string.Format("找不到对应ID为{0}的物体", id));
        }
        return baseShowHideObj;
    }
    /// <summary>
    /// 根据ID显示物体
    /// </summary>
    /// <param name="id"></param>
    public GameObject ShowObjForID(string id)
    {
        GetShowHideObjForID(id).Show();
        return GetShowHideObjForID(id).gameObject;
    }
    /// <summary>
    /// 根据ID隐藏物体
    /// </summary>
    /// <param name="id"></param>
    public void HideObjForID(string id)
    {
        GetShowHideObjForID(id).Hide();
    }

    /// <summary>
    /// 设置显示隐藏物体
    /// </summary>
    /// <param name="showHideObjIDList"></param>
    public void SetShowHideObj(string showHideObjIDList)
    {
        Debug.Log(showHideObjIDList);
        string[] idArray = showHideObjIDList.Split("|".ToCharArray());

        for (int i = 0; i < idArray.Length; i++)
        {
            int index = i;
            if (index == 0 && idArray[index] != "null")//要显示的所有物体ID
            {
                string[] showIdArray = idArray[index].Split("&".ToCharArray());
                for (int j = 0; j < showIdArray.Length; j++)
                {
                    int showIndex = j;
                    if (!string.IsNullOrEmpty(showIdArray[showIndex]))
                    {
                        ShowObjForID(showIdArray[showIndex]);
                    }
                }
            }
            else//要隐藏的所有物体ID
            {
                if (idArray[index] != "null")//要隐藏的所有物体ID
                {
                    string[] hideIdArray = idArray[index].Split("&".ToCharArray());
                    for (int j = 0; j < hideIdArray.Length; j++)
                    {
                        int hideIndex = j;
                        if (!string.IsNullOrEmpty(hideIdArray[hideIndex]))
                        {
                           HideObjForID(hideIdArray[hideIndex]);
                        }
                    }
                }
            }
        }


    }
    /// <summary>
    /// 只显示一个物体
    /// </summary>
    /// <param name="id"></param>
    public void ShowOnlyObj(string id)
    {
        foreach (var item in new List<string>(allShowHideObjDict.Keys))
        {
            if (item!=id)
            {
                allShowHideObjDict[item].Hide();
            }
            
        }
        allShowHideObjDict[id].Show();
    }
    /// <summary>
    /// 只隐藏一个物体
    /// </summary>
    /// <param name="id"></param>
    public void HideOnlyObj(string id)
    {
        foreach (var item in new List<string>(allShowHideObjDict.Keys))
        {
            if (item != id)
            {
                allShowHideObjDict[item].Show();
            }
                
        }
        allShowHideObjDict[id].Hide();
    }
    /// <summary>
    /// 销毁
    /// </summary>
    public override void OnDestory()
    {
        
    }
}
