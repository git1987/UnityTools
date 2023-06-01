# Unity中一些常用的类和方法
# 在Unity编辑器内调试和修改（Unity版本2021.2.18f1）
## UnityTools.Tools(工具类)
Debuger(Dll的形式)：自定义日志类，`disable`为日志禁用状态，通过`Enable()`和`UnEnable()`来打开和关闭日志  
EventManager：事件管理器  
Tools：静态方法类  

## UnityTools.Single(MonoBehaviour类型单例类)
SingleMono：单例类基类  
Game：常用的一些功能  
&#8195;&#8195;[Delayed:延时调用方法]  
Pool：GameObject对象池  

## UnityTools.Extend（类扩展）
UnityEngineExtend：UnityEngine程序集的扩展方法  

## UnityTools.UI（UI框架）
UIManager：UI总控制器  
UICtrl：场景UI控制器基类（每个场景只有一个）  
BasePanel：面板基类  
MaskGraphic：不渲染的Graphic，raycastTarget有效  
VirtualRocker：虚拟摇杆  
GraphicPointer：鼠标手势事件监听，鼠标左、右、中键点击事件

## UnityTools.MonoComponent（继承MonoBehaviour的组件）
AutoClear:自动清除的组件（特效类型），最终调用Pool.Recover(this.gameObject)  
Schedule:计时任务类组件（Update方法内更新时间，使用Time.deltaTime更新时间）单次延时回调，多次重复回调
