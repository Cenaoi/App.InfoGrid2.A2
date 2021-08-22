using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace App.InfoGrid2.GBZZZD.Bll
{
    /// <summary>
    /// 打印帮助类
    /// </summary>
    public class PrintHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static PrintHelper _Instance = null;

        /// <summary>
        /// 实例
        /// </summary>
        public static PrintHelper Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new PrintHelper();
                }

                return _Instance;
            }
        }

        private Graphics _Graphics = null;

        /// <summary>
        /// 生成任务单打印文件
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool DrawTaskOrderPrintFile(string savePath, SModel data)
        {
            savePath = System.Web.HttpContext.Current.Server.MapPath(savePath);

            string templatePath = System.Web.HttpContext.Current.Server.MapPath("/App_Biz/PrintTemplate/template.xml");

            return DrawPrintFile(templatePath, savePath, data);
        }

        /// <summary>
        /// 生成任务完成标签打印文件
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool DrawTaskFinishTagPrintFile(string savePath, SModel data)
        {
            savePath = System.Web.HttpContext.Current.Server.MapPath(savePath);

            string templatePath = System.Web.HttpContext.Current.Server.MapPath("/App_Biz/PrintTemplate/template2.xml");

            return DrawPrintFile(templatePath, savePath, data);
        }

        /// <summary>
        /// 绘制打印文件
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool DrawPrintFile(string templatePath, string path, SModel data) 
        {
            XmlElement root = null;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(templatePath);

                root = doc.DocumentElement;
            }
            catch (Exception ex)
            {
                log.Error("读取打印模板文件出错了. 文件路径:" + templatePath, ex);

                return false;
            }

            XmlNode headNode = root.SelectSingleNode("head");

            SModel headItems = new SModel();

            foreach (var item in headNode.ChildNodes)
            {
                XmlNode itemNode = item as XmlNode;

                SModel nodeInfo = new SModel();
                nodeInfo["name"] = itemNode.Name;

                GetAttrInfo(itemNode, nodeInfo);

                headItems[itemNode.Name] = nodeInfo;
            }

            SModel options = headItems["options"];

            int width = options.GetInt("width");
            int height = options.GetInt("height");

            Bitmap bitmap = new Bitmap(width, height);

            try
            {
                using (this._Graphics = Graphics.FromImage(bitmap))
                {
                    //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    this._Graphics.PageUnit = GraphicsUnit.Millimeter;
                    //字体缩放的
                    //g.ScaleTransform(1, 1);
                    //画边框
                    //g.DrawLine(Pens.Red, 0, 1, width, 1);
                    //g.DrawLine(Pens.Red, 1, height - 1, width, height - 1);
                    //g.DrawLine(Pens.Red, 0, 0, 0, height - 1);
                    //g.DrawLine(Pens.Red, width - 1, 0, width - 1, height - 1);

                    //XmlNode head_node = root.SelectSingleNode("head");
                    XmlNode bodyNode = root.SelectSingleNode("body");

                    //List<string> fieldList = new List<string>();

                    SModel bodyInfo = new SModel();

                    //this._Graphics.TranslateTransform(10, 20);

                    DrawEmf(bodyNode, bodyInfo, data);
                }

                string saveDir = Path.GetDirectoryName(path);

                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                bitmap.Save(path);

                return true;
            }
            catch (Exception ex)
            {
                log.Error("生成打印文件出错了. 打印内容Json:" + data.ToJson(), ex);

                return false;
            }
        }

        /// <summary>
        /// 默认字体
        /// </summary>
        Font m_DefaultFont = new Font(new FontFamily("黑体"), 12);

        /// <summary>
        /// 绘制打印内容
        /// </summary>
        /// <param name="bodyNode"></param>
        /// <param name="nodeInfo"></param>
        /// <param name="data"></param>
        private void DrawEmf(XmlNode bodyNode, SModel nodeInfo, SModel data) 
        {
            SModelList childNodeList = new SModelList();

            foreach (var item in bodyNode.ChildNodes)
            {
                XmlNode xmlNode = item as XmlNode;

                if (xmlNode == null || xmlNode?.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }

                SModel itemNode = new SModel();
                itemNode["name"] = xmlNode.Name;

                GetAttrInfo(xmlNode, itemNode);

                childNodeList.Add(itemNode);

                DrawEmf(xmlNode, itemNode, data);

                if (xmlNode.Name == "rect")
                {
                    DrawRect(itemNode, data);
                }
                else if (xmlNode.Name == "table")
                {
                    DrawTable(itemNode, data);
                }
            }

            nodeInfo["childs"] = childNodeList;
            nodeInfo["child_count"] = childNodeList.Count;
        }

        /// <summary>
        /// 获取节点属性信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="item"></param>
        private void GetAttrInfo(XmlNode node, SModel item) 
        {
            foreach (var node_attr in node.Attributes)
            {
                XmlAttribute xml_attr = node_attr as XmlAttribute;

                if (xml_attr.Name == "points")
                {
                    SModel attr_sm = SModel.ParseJson(xml_attr.Value);

                    foreach (var attr_field in attr_sm.GetFields())
                    {
                        item[attr_field] = attr_sm[attr_field];
                    }

                    continue;
                }

                item[xml_attr.Name] = xml_attr.Value;

                //if (xml_attr.Name == "name" && !string.IsNullOrWhiteSpace(xml_attr.Value))
                //{
                //    fieldList.Add(xml_attr.Value);
                //}
            }
        }

        /// <summary>
        /// 获取文字对齐方式
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private StringFormat GetTextAlign(SModel item) 
        {
            string align = item.GetString("align");

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Center;
            //format.FormatFlags = StringFormatFlags.DirectionVertical;
            if (align == "center")
            {            
                format.Alignment = StringAlignment.Center;
            }
            else if (align == "right")
            {
                format.Alignment = StringAlignment.Far;
            }

            return format;
        }
      
        /// <summary>
        /// 获取字体
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Font GetFont(SModel item) 
        {
            Font font = m_DefaultFont;

            int font_size = item.GetInt("fontSize");

            if (font_size > 0)
            {
                font = new Font(new FontFamily("黑体"), font_size);
            }

            return font;
        }

        /// <summary>
        /// 画水平线
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        private void DrawHorizontalLine(Pen pen, int left, int top, int width) 
        {
            this._Graphics.DrawLine(pen, left, top, left + width, top);
        }

        /// <summary>
        /// 画垂直线
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="height"></param>
        private void DrawVerticalLine(Pen pen, int left, int top, int height)
        {
            this._Graphics.DrawLine(pen, left, top, left, top + height);
        }
        
        /// <summary>
        /// 画边框
        /// </summary>
        /// <param name="item"></param>
        private void DrawBorder(SModel item) 
        {
            DrawTopBorder(Pens.Black, item);
            DrawBottomBorder(Pens.Black, item);
            DrawRightBorder(Pens.Black, item);
            DrawLeftBorder(Pens.Black, item);
        }

        /// <summary>
        /// 画上边框
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="item"></param>
        private void DrawTopBorder(Pen pen, SModel item)
        {
            int x1 = item.GetInt("x1");
            int y1 = item.GetInt("y1");
            int width = item.GetInt("width");

            //this._Graphics.DrawLine(pen, x1, y1, x1 + width, y1);

            this.DrawHorizontalLine(pen, x1, y1, width);
        }

        /// <summary>
        /// 画右边框
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="item"></param>
        private void DrawRightBorder(Pen pen, SModel item)
        {
            int x1 = item.GetInt("x1");
            int y1 = item.GetInt("y1");
            int width = item.GetInt("width");
            int height = item.GetInt("height");

            //this._Graphics.DrawLine(pen, x1 + width, y1, x1 + width, y1 + height);

            this.DrawVerticalLine(pen, x1 + width, y1, height);
        }

        /// <summary>
        /// 画左边框
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="item"></param>
        private void DrawLeftBorder(Pen pen, SModel item) 
        {
            int x1 = item.GetInt("x1");
            int y1 = item.GetInt("y1");
            int width = item.GetInt("width");
            int height = item.GetInt("height");

            //this._Graphics.DrawLine(pen, x1, y1, x1, y1 + height);

            this.DrawVerticalLine(pen, x1, y1, height);
        }

        /// <summary>
        /// 画下边框
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="item"></param>
        private void DrawBottomBorder(Pen pen, SModel item) 
        {
            int x1 = item.GetInt("x1");
            int y1 = item.GetInt("y1");
            int width = item.GetInt("width");
            int height = item.GetInt("height");

            //this._Graphics.DrawLine(pen, x1, y1 + height, x1 + width, y1 + height);

            this.DrawHorizontalLine(pen, x1, y1 + height, width);
        }

        /// <summary>
        /// 绘制区域
        /// </summary>
        /// <param name="item"></param>
        /// <param name="formData"></param>
        private void DrawRect(SModel item, SModel formData)
        {
            StringFormat format = GetTextAlign(item);

            Font rectFont = GetFont(item);

            string label = item.GetString("label");

            string labelAlign = item.GetString("labelAlign");

            PrintRectangle rect = new PrintRectangle(item);

            bool hasBorder = item.GetBool("border");

            if (hasBorder) 
            {
                DrawBorder(item);
            }

            string text = label;

            string name = item.GetString("field");

            if (!string.IsNullOrWhiteSpace(name))
            {
                string value = "";

                if (formData.HasField(name))
                {
                    value = formData.GetString(name);
                }
                else 
                {
                    string[] fields = name.Split('+');

                    foreach (var f in fields)
                    {
                        if (formData.HasField(f)) 
                        {
                            value = formData.GetString(f);
                        }
                    }
                }

                text = label + value;

                if (labelAlign == "right")
                {
                    text = value + label;
                }
            }

            Brush brush = SystemBrushes.ControlText;

            if (hasBorder) 
            {
                //item["x1"] += 3;
                //item["y1"] += 2;
                rect = new PrintRectangle(item);
            }

            this._Graphics.DrawString(text, rectFont, brush, rect.Rectangle, format);
        }

        /// <summary>
        /// 绘制表格
        /// </summary>
        /// <param name="table"></param>
        /// <param name="data"></param>
        private void DrawTable(SModel table, SModel data)
        {
            string dataSource = table.GetString("dataSource");

            SModelList dataList = new SModelList();

            if (data.HasField(dataSource))
            {
                dataList = data[dataSource] as SModelList;
            }

            bool hasBorder = table.GetBool("border");

            if (hasBorder)
            {
                DrawBorder(table);
            }

            int rowCount = table.GetInt("rowCount");
            int rowHeight = table.GetInt("rowHeight");

            Font rectFont = GetFont(table);

            PrintRectangle tableRect = new PrintRectangle(table);

            SModelList childList = table["childs"] as SModelList;

            foreach (var child in childList)
            {
                string name = child.GetString("name");

                if (name == "cols")
                {
                    child["x1"] = tableRect.X1;
                    child["y1"] = tableRect.Y1;

                    PrintRectangle colsRect = new PrintRectangle(child);

                    SModelList cols = child["childs"] as SModelList;

                    int sx = colsRect.X1;
                    int sy = colsRect.Y1;

                    for (int i = 0; i < rowCount; i++)
                    {
                        DrawHorizontalLine(Pens.Black, colsRect.X1, i * rowHeight + colsRect.Y1 + rowHeight, colsRect.Rectangle.Width);
                    }

                    int colIndex = 0;

                    foreach (var col in cols)
                    {
                        col["x1"] = sx;
                        col["y1"] = sy;
                        int width = col.GetInt("width");
                        string label = col.GetString("label");

                        PrintRectangle colRect = new PrintRectangle(col);

                        StringFormat format = GetTextAlign(col);

                        this._Graphics.DrawString(label, rectFont, Brushes.Black, colRect.Rectangle, format);

                        sx += width;

                        if (colIndex < cols.Count - 1)
                        {
                            DrawVerticalLine(Pens.Black, sx, sy, colsRect.Rectangle.Height + 8);
                        }

                        colIndex++;
                    }

                    int rowIndex = 0;

                    foreach (var row in dataList)
                    {
                        foreach (var col in cols)
                        {
                            PrintRectangle colRect = new PrintRectangle(col);

                            StringFormat format = GetTextAlign(col);

                            string field = col.GetString("field");

                            Rectangle cellRect = new Rectangle(colRect.X1, rowIndex * rowHeight + colsRect.Y1 + rowHeight, colRect.Rectangle.Width, colRect.Rectangle.Height);

                            if (!string.IsNullOrWhiteSpace(field) && row.HasField(field))
                            {
                                string value = row.GetString(field);

                                this._Graphics.DrawString(value, rectFont, Brushes.Black, cellRect, format);
                            }
                        }

                        rowIndex++;
                    }

                    if (table.GetBool("showSummary"))
                    {
                        SModel col = cols.First();

                        PrintRectangle colRect = new PrintRectangle(col);

                        StringFormat format = GetTextAlign(col);

                        Rectangle cellRect = new Rectangle(colRect.X1, (rowCount - 1) * rowHeight + colsRect.Y1 + 1, colRect.Rectangle.Width, colRect.Rectangle.Height);

                        string stext = table.GetString("summaryText");

                        this._Graphics.DrawString(stext, rectFont, Brushes.Black, cellRect, format);

                        foreach (var colItem in cols)
                        {
                            if (!colItem.GetBool("summary"))
                            {
                                continue;
                            }

                            int total = 0;

                            string field = colItem.GetString("field");

                            foreach (var dataItem in dataList)
                            {
                                int fv = dataItem.GetInt(field);

                                total += fv;
                            }

                            PrintRectangle colItemRect = new PrintRectangle(colItem);

                            StringFormat itemFormat = GetTextAlign(colItem);

                            Rectangle cellItemRect = new Rectangle(colItemRect.X1, (rowCount - 1) * rowHeight + colItemRect.Y1, colItemRect.Rectangle.Width, colItemRect.Rectangle.Height);

                            this._Graphics.DrawString($"{total}", rectFont, Brushes.Black, cellItemRect, itemFormat);
                        }
                    }
                }
            }
        }


        public void Test(string path)
        {
            Graphics gs = Graphics.FromImage(new Bitmap(1, 1));

            gs.PageUnit = GraphicsUnit.Point;

            Metafile mf = new Metafile(path, gs.GetHdc());

            Graphics g = Graphics.FromImage(mf);

            g.PageUnit = GraphicsUnit.Point;

            //字体缩放的
            //g.ScaleTransform(0.85f, 0.85f);
        }

    }


    public class PrintRectangle
    {
        public int X1 { get; set; }

        public int Y1 { get; set; }

        public int X2 { get; set; }

        public int Y2 { get; set; }

        public Rectangle Rectangle { get; set; }

        public PrintRectangle(int x1, int y1, int x2, int y2, int width = 20, int height = 11)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;

            this.Rectangle = new Rectangle(x1, y1, width, height);
        }

        public PrintRectangle(SModel item)
        {
            int x1 = item.GetInt("x1");
            int y1 = item.GetInt("y1");
            int x2 = item.GetInt("x2");
            int y2 = item.GetInt("y2");

            int width = item.GetInt("width");
            int height = item.GetInt("height");

            if (height == 0)
            {
                height = 11;
            }

            int pl = item.GetInt("pl");
            int pt = item.GetInt("pt");
            int pr = item.GetInt("pr");
            int pb = item.GetInt("pb");

            x1 += pl;
            y1 += pt;
            width -= pr;
            height -= pb;

            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;

            this.Rectangle = new Rectangle(x1, y1, width, height);
        }
    }


}
