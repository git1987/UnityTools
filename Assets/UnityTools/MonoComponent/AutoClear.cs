using UnityEngine;
using UnityTools;
using UnityTools.Single;
using UnityTools.MonoComponent;

/// <summary>
/// 自动清除：删除/放回对象池
/// </summary>
public class AutoClear : MonoBehaviour
{
    /// <summary>
    /// 是否自动清除:在prefab中设置好值： isEffect==true || lifeTIme>0
    /// </summary>
    /// <param name="autoClear"></param>
    /// <returns></returns>
    public static bool IsAutoClear(GameObject autoClear, float time)
    {
        AutoClear ac = autoClear.GetComponent<AutoClear>();
        ac.lifeTime = time;
        if (ac != null) return ac.autoClear;
        return false;
    }
    [SerializeField]
    private bool isEffect;
    /// 是否自动清除
    public bool autoClear
    {
        get { return lifeTime > 0; }
    }
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private GameObject deathObj;
    //特效播放完毕的回调
    private EventAction finish;
    private void Awake()
    {
        if (isEffect)
        {
            ParticleSystem[] pas = this.transform.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < pas.Length; i++)
            {
                ParticleSystem p = pas[i];
                lifeTime = Mathf.Max(lifeTime, p.main.duration);
            }
            Animator animator              = this.transform.GetComponent<Animator>();
            if (animator != null) lifeTime = Mathf.Max(lifeTime, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
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

    //通过对象池使用，每次Get的时候调用
    private void OnEnable()
    {
        if (lifeTime > 0)
        {
            Schedule.GetInstance(this.gameObject).Once(() => this.Disable(), lifeTime);
        }
    }
    private void OnDisable()
    {
        if (deathObj != null && Pool.instance)
        {
            GameObject effect = Pool.instance.Init(deathObj).GetObj(deathObj.name);
            effect.transform.position = this.transform.position;
            effect.transform.rotation = this.transform.rotation;
        }
    }
    private void Disable()
    {
        if (this.finish != null)
        {
            this.finish();
            this.finish = null;
        }
        Pool.Recover(this.gameObject);
    }
}