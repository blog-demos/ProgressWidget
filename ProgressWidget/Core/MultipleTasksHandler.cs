
/**********************************************************
 * 
 * 命名空间：
 *          ProgressWidget.Core
 * 
 * 描述：
 *          使用嵌套委托实现子进度控制
 *          
 * 功能及上下游：
 *          N/A
 *          
 * 人员：
 *          大鱼
 *          
 * 创建时间：
 *          2018/8/8 23:48:46
 * 
 ***********************************************************/

namespace ProgressWidget.Core
{
    internal delegate void MultipleTasksHandler(object sender, SubProgressChangedHandler handler, int weight);
}
