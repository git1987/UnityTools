using System.Collections.Generic;
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
            modelList.Add(model);
        }
        /// <summary>
        /// 清空所有的Model单例
        /// </summary>
        public static void ClearModel()
        {
            modelList.ForAction(model => model.Disable());
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