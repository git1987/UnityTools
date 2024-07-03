using System.Collections.Generic;
using UnityEngine;
using UnityTools.Extend;
namespace UnityTools.UI
{
    /// <summary>
    /// 数据层：MVC中的M
    /// </summary>
    public abstract class BaseModel
    {
        private static List<BaseModel> modelList = new();
        protected static void CreateModel(BaseModel model)
        {
            if (modelList.Contains(model))
            {
                Debug.LogError($"{model}已经存在");
                model.Disable();
            }
            else
            {
                modelList.Add(model);
            }
        }
        /// <summary>
        /// 清空所有的Model单例
        /// </summary>
        public static void ClearModel()
        {
            modelList.ForAction(model => model.Disable());
            modelList.Clear();
        }
        public static void RemoveModel<M>() where M : BaseModel
        {
            for (int i = 0; i < modelList.Count; i++)
            {
                BaseModel model = modelList[i];
                if (model is M)
                {
                    model.Disable();
                    modelList.RemoveAt(i);
                    return;
                }
            }
            Debug.LogError($"不存在{typeof(M)}");
        }
        protected BaseModel()
        {
            CreateModel(this);
        }
        /// <summary>
        /// 移除：清空单例
        /// </summary>
        protected abstract void Disable();
    }
}