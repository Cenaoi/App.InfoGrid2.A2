using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Reflection;
using System.Collections;
using EC5.SystemBoard.Web;
using EC5.Utility;

namespace EasyClick.BizWeb
{


    public static class XHtmlHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="serverControls">服务器控件</param>
        /// <returns></returns>
        public static Control[] GetControls(string path)
        {
            return GetControls(path, null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="serverControls">服务器控件</param>
        /// <returns></returns>
        public static Control[] GetControls(string path, List<Control> serverControls)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(path);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new Control[0];
            }

            List<Control> list = new List<Control>();

            XmlElement root = doc.DocumentElement;

            if (root.Name != "ASP_Template")
            {
                throw new Exception("非 \"ASP Template\" 模板格式.");
            }

            XmlNode body = root["body"];

            if (body == null)
            {
                body = root;
            }

            RecursionControls(body, list, null, serverControls);


            return list.ToArray();
        }

        /// <summary>
        /// 处理没有前缀的节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="list"></param>
        /// <param name="parentControl"></param>
        /// <param name="xnode"></param>
        private static void ProNoPrefix(XmlNode parent, List<Control> list, Control parentControl, XmlNode xnode, List<Control> serverControls)
        {
            if (xnode.NodeType == XmlNodeType.Text)
            {
                if (parentControl != null)
                {
                    parentControl.Controls.Add(new LiteralControl(xnode.Value));
                }
                else
                {
                    list.Add(new LiteralControl(xnode.Value));
                }

                //parentControl.Controls.Add(new LiteralControl(xnode.Value));

                return;
            }

            XHtmlControl html = new XHtmlControl(xnode.LocalName);

            foreach (XmlAttribute attr in xnode.Attributes)
            {
                html.Attributes.Add(attr.Name, attr.Value);
            }

            if (parentControl != null)
            {
                parentControl.Controls.Add(html);
            }
            else
            {
                list.Add(html);
            }

            if ("script".Equals(xnode.LocalName, StringComparison.CurrentCultureIgnoreCase))
            {
                html.InnerHtml = xnode.InnerXml;

                return;
            }

            RecursionControls(xnode, list, html,serverControls);
        }

        /// <summary>
        /// 处理有前缀的节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="list"></param>
        /// <param name="parentControl"></param>
        /// <param name="xnode"></param>
        private static void ProPrefix(XmlNode parent, List<Control> list, Control parentControl, XmlNode xnode, List<Control> serverControls)
        {
            Type conT = UIControlManager.GetItem(xnode.Prefix, xnode.LocalName);

            if (conT == null)
            {
                return;
            }

            Control con = (Control)Activator.CreateInstance(conT);

            IAttributeAccessor attrs = con as IAttributeAccessor;

            bool isServer = ProControlAttr(conT, con, xnode);

            //if (isServer)
            //{
            //    //处理服务器控件
            //    ProServerControl(conT, con, xnode);

            //    if (serverControls != null)
            //    {
            //        serverControls.Add(con);
            //    }
            //}


            if (parentControl != null)
            {
                parentControl.Controls.Add(con);
            }
            else
            {
                list.Add(con);
            }
        }

        /// <summary>
        /// 递归处理控件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="list"></param>
        /// <param name="parentControl"></param>
        public static void RecursionControls(XmlNode parent,List<Control> list,  Control parentControl, List<Control> serverControls)
        {
            if ( !parent.HasChildNodes || parent.ChildNodes.Count == 0)
            {
                return;
            }

            if (parent.NodeType == XmlNodeType.Text )
            {
                return;
            }

            foreach (XmlNode xnode in parent.ChildNodes)
            {
                if (string.IsNullOrEmpty(xnode.Prefix))
                {
                    //处理没有前缀的节点
                    ProNoPrefix(parent, list, parentControl, xnode, serverControls);    
                }
                else
                {
                    //处理有前缀的节点
                    ProPrefix(parent, list, parentControl, xnode,serverControls);      
                }
            }


        }

        /// <summary>
        /// 处理控件有 ParseChildrenAttribute 属性
        /// </summary>
        /// <param name="parseChildren"></param>
        /// <param name="conT"></param>
        /// <param name="con"></param>
        /// <param name="xnode"></param>
        private static void ProParseChildren(ParseChildrenAttribute parseChildren, Type conT, object con, XmlNode xnode)
        {
            string propName = parseChildren.DefaultProperty;

            PropertyInfo propDP = conT.GetProperty(propName);

            if (propDP == null)
            {
                return;
            }

            if (propDP.PropertyType.GetInterface("IList", false) == null)
            {
                return;
            }

            IList propList = (IList)propDP.GetValue(con, null);

            foreach (XmlNode item in xnode.ChildNodes)
            {
                Type objT = UIControlManager.GetItem(item.Prefix, item.LocalName);
                object obj = Activator.CreateInstance(objT);

                ProControlAttr(objT, obj, item);

                propList.Add(obj);
            }
        }


        private static void ProPersistenceMode(PersistenceModeAttribute persistenceMode,PropertyInfo propInfo, Type conT, object con, XmlNode xnode)
        {
            if (persistenceMode.Mode == PersistenceMode.Attribute)
            {
                string value = xnode.InnerXml;

                propInfo.SetValue(con, value, null);
            }
            else if (persistenceMode.Mode == PersistenceMode.InnerProperty || persistenceMode.Mode == PersistenceMode.InnerDefaultProperty)
            {
                if ( propInfo.PropertyType.IsValueType )
                {
                    string value = xnode.InnerXml;

                    if (propInfo.PropertyType != typeof(string))
                    {
                        object valueObj = Convert.ChangeType(value, propInfo.PropertyType);
                        propInfo.SetValue(con, valueObj, null);
                    }
                    else
                    {
                        propInfo.SetValue(con, value, null);
                    }
                }
                else if(propInfo.PropertyType.IsClass)
                {
                    object propObj = propInfo.GetValue(con, null);  //获取属性

                    if (propObj is IList || propObj is System.Web.UI.ControlCollection)
                    {
                        Type propObjT = propObj.GetType();

                        foreach (XmlNode item in xnode.ChildNodes)
                        {
                            Type itemT = UIControlManager.GetItem(item.Prefix, item.LocalName);


                            MethodInfo addMethodInfo = propObjT.GetMethod("Add",new Type[]{ itemT});


                            ParameterInfo[] pInfos = addMethodInfo.GetParameters();

                            ParameterInfo pi = pInfos[0];


                           
                            if(!IsInherit(itemT, pi.ParameterType))
                            {
                                continue;
                            }

                            object itemObj = Activator.CreateInstance(itemT);

                            ProControlAttr(itemT, itemObj, item);

                            addMethodInfo.Invoke(propObj, new object[] { itemObj });
                        }

                    }
                    else if (propObj is Control)
                    {
                        ProControlAttr(propInfo.PropertyType, propObj, xnode);
                    }
                }
            }
        }

        private static bool IsInherit(Type a, Type inheritType)
        {
            bool isInherit = false;

            while (true)
            {
                if (a == inheritType)
                {
                    isInherit = true;
                    break;
                }

                if (a.BaseType == typeof(object))
                {
                    break;
                }

                a = a.BaseType;
            }

            return isInherit;
        }

        /// <summary>
        /// 处理服务器控件
        /// </summary>
        /// <param name="conT"></param>
        /// <param name="con"></param>
        /// <param name="xnode"></param>
        private static void ProServerControl(Type conT, object con, XmlNode xnode)
        {
            ParseChildrenAttribute pc = AttrUtil.GetAttr<ParseChildrenAttribute>(conT);

            if (pc != null && pc.ChildrenAsProperties && !string.IsNullOrEmpty(pc.DefaultProperty))
            {
                ProParseChildren(pc, conT, con, xnode);

                //PropertyInfo defaultProp = conT.GetProperty(pc.DefaultProperty);

                //if (defaultProp != null)
                //{
                //    PersistenceModeAttribute pm = AttrUtil.GetAttr<PersistenceModeAttribute>(defaultProp);

                //    if (pm.Mode == PersistenceMode.InnerDefaultProperty)
                //    {
                //        return;
                //    }
                //}
            }


            //处理控件复杂属性
            foreach (XmlNode item in xnode.ChildNodes)
            {
                string propName = item.Name;

                PropertyInfo propInfo = conT.GetProperty(propName);

                if (propInfo == null)
                {
                    continue;
                }

                PersistenceModeAttribute pm = AttrUtil.GetAttr<PersistenceModeAttribute>(propInfo);

                if (pm != null)
                {
                    ProPersistenceMode(pm, propInfo, conT, con, item);
                }

            }


        }

        /// <summary>
        /// 处理控件属性
        /// </summary>
        /// <param name="objT"></param>
        /// <param name="obj"></param>
        /// <param name="xNode"></param>
        /// <returns></returns>
        private static bool ProControlAttr(Type objT, object obj,XmlNode xNode)
        {
            bool isServer = false;

            IAttributeAccessor objAttr = obj as IAttributeAccessor;

            ParseChildrenAttribute parseChildren = AttrUtil.GetAttr<ParseChildrenAttribute>(objT);

            foreach (XmlAttribute attr in xNode.Attributes)
            {
                if ("runat".Equals(attr.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    isServer = true;
                    continue;
                }

                PropertyInfo objProp = objT.GetProperty(attr.Name);

                if (objProp == null)
                {
                    objAttr.SetAttribute(attr.Name, attr.Value);
                }
                else
                {
                    Type propType = objProp.PropertyType;

                    if (propType.IsEnum)
                    {
                        object targetV = Enum.Parse(propType, attr.Value);

                        objProp.SetValue(obj, targetV, null);
                    }
                    else
                    {
                        object targetV = Convert.ChangeType(attr.Value, objProp.PropertyType);
                        objProp.SetValue(obj, targetV, null);
                    }
                }
            }

            PersistenceModeAttribute pm = null;
            PropertyInfo defaultPi = null;

            if (parseChildren != null && parseChildren.ChildrenAsProperties && !string.IsNullOrEmpty(parseChildren.DefaultProperty))
            {
                string propName = parseChildren.DefaultProperty;

                defaultPi = objT.GetProperty(propName);

                if (defaultPi != null)
                {
                    pm = AttrUtil.GetAttr<PersistenceModeAttribute>(defaultPi);
                }
            }

            //内嵌唯一属性
            if (pm != null && pm.Mode == PersistenceMode.InnerDefaultProperty && defaultPi != null)
            {
                ProPersistenceMode(pm, defaultPi, objT, obj, xNode);
            }
            else
            {
                foreach (XmlNode item in xNode.ChildNodes)
                {
                    PropertyInfo pi = objT.GetProperty(item.Name);

                    if (pi == null) { continue; }

                    ProPersistenceMode(new PersistenceModeAttribute(PersistenceMode.InnerProperty), pi, objT, obj, item);
                }
            }

            return isServer;
        }
    }
}
