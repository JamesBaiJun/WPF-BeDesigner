using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unvell.ReoScript;

namespace BeDesigner
{
    internal class Config
    {
        private static ScriptRunningMachine srm { get; } = new ScriptRunningMachine();

        static Config()
        {
            srm.WorkMode |=
                  // Enable DirectAccess 
                  MachineWorkMode.AllowDirectAccess
                // Ignore exceptions in CLR calling (by default)
                | MachineWorkMode.IgnoreCLRExceptions
                // Enable CLR Event Binding
                | MachineWorkMode.AllowCLREventBind;

            RegisterFunction();
        }

        /// <summary>
        /// 运行脚本
        /// </summary>
        /// <param name="script"></param>
        public static void RunJsScipt(string script)
        {
            try
            {
                srm.Run(script);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "脚本错误");
            }
        }

        /// <summary>
        /// 注册对象到js
        /// </summary>
        public static void SetVariable(string name, object obj)
        {
            srm.SetGlobalVariable(name, obj);
        }

        /// <summary>
        /// 注册方法到Js
        /// </summary>
        private static void RegisterFunction()
        {
            srm["ShowMessage"] = new NativeFunctionObject("ShowMessage", (ctx, owner, args) =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in args)
                {
                    sb.Append(item.ToString());
                }

                MessageBox.Show($"{sb}", "提示");
                return null;
            });

            srm["SetICDValue"] = new NativeFunctionObject("SetICDValue", (ctx, owner, args) =>
            {
                MessageBox.Show($"发送ICD数据", "提示");
                return null;
            });
        }
    }
}
