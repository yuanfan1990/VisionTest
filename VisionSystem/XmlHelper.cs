using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace VisionSystem
{
    public static class XmlHelper
    {
        /// <summary>
        /// 创建XML文件
        /// </summary>
        /// <param name="filepath">xml文件路径</param>
        /// <param name="rootnode_name">根节点名称</param>
        /// <returns>=0创建成功 =1文件已存在 =-1报错</returns>
        public static int FileCreate(string filepath, string rootnode_name)
        {
            try
            {
                if (!File.Exists(filepath))
                {
                    XmlDocument xml = new XmlDocument();
                    XmlDeclaration xmldel;
                    XmlNode root;

                    xmldel = xml.CreateXmlDeclaration("1.0", "utf-8", null);
                    xml.AppendChild(xmldel);
                    root = xml.CreateElement(rootnode_name);
                    xml.AppendChild(root);
                    xml.Save(filepath);
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 读取节点内容
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="xpath">xpath表达式</param>
        /// <param name="txt">读取到的内容 如果节点不存在或者报错，返回""</param>
        /// <returns>=0读取成功 =1节点不存在 =-1读取报错</returns>
        public static int Read(string filepath, string xpath, out string txt)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(filepath);
                XmlNode root = xml.DocumentElement;

                XmlNode node = root.SelectSingleNode(xpath);
                if (node != null)
                {
                    txt = node.InnerText;
                    return 0;
                }
                else
                {
                    txt = "";
                    return 1;
                }
            }
            catch
            {
                txt = "";
                return -1;
            }
        }

        /// <summary>
        /// 读取节点属性值
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="xpath">xpath表达式</param>
        /// <param name="attributename">节点属性名称</param>
        /// <param name="attributevalue">节点属性值</param>
        /// <returns>=0读取成功 =1节点不存在 =2属性不存在 =-1读取报错</returns>
        public static int Read(string filepath, string xpath, string attributename, out string attributevalue)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(filepath);
                XmlNode root = xml.DocumentElement;

                XmlNode node = root.SelectSingleNode(xpath);
                if (node == null)
                {
                    attributevalue = "";
                    return 1;
                }


                XmlAttribute attr = node.Attributes[attributename];
                if (attr == null)
                {
                    attributevalue = "";
                    return 2;
                }

                attributevalue = attr.Value;
                return 0;
            }
            catch
            {
                attributevalue = "";
                return -1;
            }
        }

        /// <summary>
        /// 读取节点个数
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="xpath">xpath表达式</param>
        /// <param name="num">读取的节点个数</param>
        /// <returns>=0读取成功 =1节点不存在 =-1读取报错</returns>
        public static int Read(string filepath, string xpath, out int num)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(filepath);
                XmlNode root = xml.DocumentElement;

                XmlNodeList nodes = root.SelectNodes(xpath);
                if (nodes != null)
                {
                    num = nodes.Count;
                    return num;
                }
                else
                {
                    num = 0;
                    return 1;
                }
            }
            catch
            {
                num = 0;
                return -1;
            }
        }

        /// <summary>
        /// 读取多个节点内容
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="xpath">xpath表达式</param>
        /// <param name="list">返回的节点内容</param>
        /// <returns>=0读取成功 =1节点不存在 =-1报错</returns>
        public static int Read(string filepath, string xpath, out List<List<string>> list)
        {
            List<List<string>> list2 = new List<List<string>>();

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(filepath);
                XmlNode root = xml.DocumentElement;

                XmlNodeList nodes = root.SelectNodes(xpath);
                if (nodes == null)
                {
                    list = list2;
                    return 1;
                }

                for (int i = 0; i < nodes.Count; i++)
                {
                    List<string> list1 = new List<string>();

                    XmlAttributeCollection attrs = nodes[i].Attributes;
                    for (int j = 0; j < attrs.Count; j++)
                    {
                        list1.Add(attrs[j].Value);
                    }

                    XmlNodeList child = nodes[i].ChildNodes;
                    for (int k = 0; k < child.Count; k++)
                    {            
                        string txt = child[k].InnerText;
                        if (!string.IsNullOrEmpty(txt))
                        {
                            list1.Add(txt);
                        }

                        XmlAttributeCollection aa = child[k].Attributes;
                        if (aa == null)
                        {
                            continue;
                        }

                        for (int j = 0; j < aa.Count; j++)
                        {
                            list1.Add(aa[j].Value);
                        }
                    }

                    list2.Add(list1);
                }

                list = list2;
                return 0;
            }
            catch
            {
                list = list2;
                return -1;
            }
        }

        /// <summary>
        /// 修改节点
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="xpath">xpath表达式</param>
        /// <param name="txt">待写入的内容</param>
        /// <returns>=0写入成功 =1节点不存在 =-1写入报错</returns>
        public static int Write(string filepath, string xpath, string txt)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(filepath);
                XmlNode root = xml.DocumentElement;

                XmlNode node = root.SelectSingleNode(xpath);
                if (node == null)
                {
                    return 1;
                }
                else
                {
                    node.InnerText = txt;
                }
                xml.Save(filepath);
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 修改节点属性
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="xpath"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Write(string filepath, string xpath, string key, string value)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(filepath);
                XmlNode root = xml.DocumentElement;


                XmlNode node = root.SelectSingleNode(xpath);
                if (node == null)
                {
                    return 1;
                }

                XmlElement e = (XmlElement)node;
                e.SetAttribute(key, value);

                xml.Save(filepath);
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 增加新节点
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="xpath">xpath表达式</param>
        /// <param name="nodename">新增的节点名称</param>
        /// <param name="innertxt">新增的节点内容</param>
        /// <param name="dic">新增节点的属性，=null表示没有属性</param>
        /// <returns>=0增加成功 =1节点已存在，增加失败 =2表示xpath表达式错误 -1表示增加节点报错</returns>
        public static int AddNode(string filepath, string xpath, string nodename, string innertxt, Dictionary<string, string> dic)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(filepath);
                XmlNode root = xml.DocumentElement;

                XmlNode node = root.SelectSingleNode(xpath);
                if (node == null)
                {
                    return 2;
                }

                XmlNodeList nodes = root.SelectNodes(xpath + "/" + nodename);
                if (nodes != null)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        if (dic == null)
                        {
                            XmlAttributeCollection xas = nodes[i].Attributes;
                            if (xas.Count == 0)
                            {
                                return 1;
                            }
                        }
                        else
                        {
                            XmlAttributeCollection xas = nodes[i].Attributes;
                            if (xas.Count != 0)
                            {
                                if (dic.Count == xas.Count)
                                {
                                    int j = 0;
                                    foreach (var item in dic)
                                    {
                                        if (xas[j].Name == item.Key && xas[j].Value == item.Value)
                                        {
                                            j++;
                                        }
                                    }
                                    if (j == dic.Count)
                                    {
                                        return 1;
                                    }
                                }
                            }
                        }
                    }
                }

                XmlElement e = xml.CreateElement(nodename);
                e.InnerText = innertxt;

                if (dic != null)
                {
                    foreach (var item in dic)
                    {
                        e.SetAttribute(item.Key, item.Value);
                    }
                }
                node.AppendChild(e);

                xml.Save(filepath);
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="xpath">xpath表达式(上层节点)</param>
        /// <param name="childname">待删除的节点名称</param>
        /// <returns>=0删除成功 =1上层节点不存在 =2上层节点不存在子节点 =-1报错</returns>
        public static int DeleteNode(string filepath, string xpath, string childname)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(filepath);
                XmlNode root = xml.DocumentElement;

                XmlNode node = root.SelectSingleNode(xpath);
                if (node == null)
                {
                    return 1;
                }

                if (!node.HasChildNodes)
                {
                    return 2;
                }

                XmlNodeList nodes = node.ChildNodes;
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Name == childname)
                    {
                        node.RemoveChild(nodes[i]);
                        i--;
                    }
                }

                xml.Save(filepath);
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public static void DeleteNode(string filepath, string xpath, string childname, string attributename, string attributevalue)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filepath);
            XmlNode root = xml.DocumentElement;

            XmlNode node = root.SelectSingleNode(xpath);
            if (node == null)
            {
                return;
            }

            if (!node.HasChildNodes)
            {
                return;
            }

            XmlNodeList nodes = node.ChildNodes;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name != childname)
                {
                    continue;
                }
                if (nodes[i].Attributes.Count == 0)
                {
                    continue;
                }

                XmlAttribute xa = nodes[i].Attributes[attributename];
                if (xa == null)
                {
                    continue;
                }
                if (xa.Value.ToString() == attributevalue)
                {
                    node.RemoveChild(nodes[i]);
                    i--;
                }
            }

            xml.Save(filepath);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="xpath">xpath表达式</param>
        /// <param name="childname">要被删除的节点名称</param>
        /// <param name="dic">要被删除的节点的属性集合</param>
        /// <returns>=0删除成功 =1上层节点不存在 =2上层节点不存在子节点 =-1报错</returns>
        public static int DeleteNode(string filepath, string xpath, string childname, Dictionary<string, string> dic)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(filepath);
                XmlNode root = xml.DocumentElement;

                XmlNode node = root.SelectSingleNode(xpath);
                if (node == null)
                {
                    return 1;
                }

                if (!node.HasChildNodes)
                {
                    return 2;
                }

                XmlNodeList nodes = node.ChildNodes;

                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Name != childname)
                    {
                        continue;
                    }
                    if (nodes[i].Attributes.Count == 0)
                    {
                        if (dic != null)
                        {
                            continue;
                        }
                        node.RemoveChild(nodes[i]);
                        i--;
                    }

                    if (dic == null)
                    {
                        continue;
                    }

                    XmlAttributeCollection xas = nodes[i].Attributes;
                    if (xas.Count != dic.Count)
                    {
                        continue;
                    }

                    int j = 0;
                    foreach (var item in dic)
                    {
                        if (item.Key == xas[j].Name && item.Value == xas[j].Value)
                        {
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (j == dic.Count)
                    {
                        node.RemoveChild(nodes[i]);
                        i--;
                    }
                }

                xml.Save(filepath);
                return 0;
            }
            catch
            {
                return -1;
            }
        }
    }
}


