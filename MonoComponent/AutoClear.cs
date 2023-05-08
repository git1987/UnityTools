using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityTools.Single;

namespace UnityTools.MonoComponent
{
    /// <summary>
    /// 自动清理组件
    /// </summary>
    public class AutoClear : MonoBehaviour
    {
        /// 是否自动清除
        public bool autoClear
        {
            get { return lifeTime > 0; }
        }
        /// <summary>
        /// 自动清理的生命周期
        /// </summary>
        public float lifeTime;
        /// <summary>
        /// 死亡创建的GameObject
        /// </summary>
        public GameObject dieCreateObj;
        /// <summary>
        /// 特效完毕之后的回调
        /// </summary>
        private EventAction finish;
        /// <summary>
        /// 设置特效播放完毕的回调
        /// </summary>
        /// <param name="_finish"></param>
        public void SetFinishAction(EventAction _finish)
        {
            if (autoClear)
                this.finish = _finish;
            else
                UnityTools.Debuger.LogError("不是自动清除的特效", this.gameObject);
        }

        public void OnEnable()
        {
            if (lifeTime > 0)
            {
                Schedule.GetInstance(this.gameObject).Once(() => Pool.Recover(this.gameObject), lifeTime);
            }
        }
        public void OnDisable()
        {
            if (dieCreateObj != null)
            {
                GameObject effect = Pool.GetInstance().Init(dieCreateObj).GetObj(dieCreateObj.name);
                effect.transform.position = this.transform.position;
                effect.transform.rotation = this.transform.rotation;
            }
        }
    }
}
