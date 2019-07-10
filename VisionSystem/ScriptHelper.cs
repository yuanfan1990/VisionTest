using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using CSScriptLibrary;

namespace VisionSystem
{
    public class ScriptHelper
    {
        private string path;

        public int Index { get; set; }
        public dynamic Script { get; set; }

        public ScriptHelper()
        {
            CSScript.EvaluatorConfig.Engine = EvaluatorEngine.CodeDom;
        }

        public void Init()
        {
            int n;
            string scriptname;
            n = XmlHelper.Read(Global.ToolBlockConfigFilePath, string.Format("/ToolBlockManager/ToolBlock[@id='{0}']/scriptname", this.Index), out scriptname);

            this.path = Global.ProjectPath + scriptname;

            Compile();
        }

        public void Compile()
        {
            try
            {
                this.Script = CSScript.Evaluator.LoadFile(this.path);
            }
            catch (csscript.CompilerException c)
            {
                throw c;
            }
        }
    }
}
