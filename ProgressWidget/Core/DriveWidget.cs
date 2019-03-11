
/**********************************************************
 * 
 * 命名空间：
 *          ProgressWidget.Core
 * 
 * 描述：
 *          N/A
 *          
 * 功能及上下游：
 *          后期可添加Start后的锁机制
 *          
 * 人员：
 *          大鱼
 *          
 * 创建时间：
 *          2018/8/8 23:45:29
 * 
 ***********************************************************/

using System;
using System.Collections.Generic;
using System.Threading;

namespace ProgressWidget.Core
{
    internal class DriveWidget
    {
        private int _total = 0;
        private double _limit = 0;
        private int _current = 0;
        private IDictionary<Delegate, int> _taskList = new Dictionary<Delegate, int>();
        private bool _stop = false;

        #region 核心开放接口

        internal int ProgressUnit { get; set; } = 150;

        internal event ProgressChangedHandler OnProgressChanged;
        internal event ProgressChangedHandler OnProgressCompleted;

        internal DriveWidget() { }

        internal void Add(Delegate handler, int weight)
        {
            _total += weight;
            if (!_taskList.ContainsKey(handler))
            {
                _taskList.Add(handler, weight);
            }
        }

        internal void Start()
        {
            AddOccupyTask();
            StartTaskManageThread();
            StartProgressThread();
        }

        #endregion

        #region 内部实现逻辑

        private void AddOccupyTask()
        {
            Add(new TaskHandler((s) => { }), 1);
        }

        private void StartTaskManageThread()
        {
            new Thread(new ThreadStart(() =>
            {
                foreach (var task in _taskList)
                {
                    if (task.Key is TaskHandler)
                    {
                        // 无子进度任务
                        _limit += task.Value;
                        (task.Key as TaskHandler)(this);
                    }
                    else if (task.Key is MultipleTasksHandler)
                    {
                        // 有子进度任务
                        var handler = task.Key as MultipleTasksHandler;
                        handler(this, SubTaskProgressChanged, task.Value);
                    }
                }
            })).Start();
        }

        private void StartProgressThread()
        {
            new Thread(new ThreadStart(() =>
            {
                while (!_stop)
                {
                    if (_current < _limit)
                    {
                        _current++;
                        if (null != OnProgressChanged) { OnProgressChanged(this, (100.0 * _current / _total)); }
                    }
                    else if (_current == _total)
                    {
                        if (null != OnProgressCompleted) { OnProgressCompleted(this, 100); }
                        _stop = true;
                    }

                    Thread.Sleep(ProgressUnit);
                }
            })).Start();
        }

        private void SubTaskProgressChanged(double progress)
        {
            _limit += progress;
            //Console.WriteLine(String.Format("子进度：{0}", progress));
        }

        #endregion
    }
}
