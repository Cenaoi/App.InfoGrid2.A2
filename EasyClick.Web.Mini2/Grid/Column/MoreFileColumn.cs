using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 多文件上传的视图
    /// </summary>
    public enum MoreFileView
    {
        /// <summary>
        /// 列表模式
        /// </summary>
        List,
        /// <summary>
        /// 大图标模式
        /// </summary>
        Large
    }

    /// <summary>
    /// 多文件列
    /// </summary>
    public class MoreFileColumn:BoundField
    {

        /// <summary>
        /// 多文件列(构造函数)
        /// </summary>
        public MoreFileColumn()
        {
            this.MiType = "morefilecolumn";
            this.DefaultEditorType = "more_file";
        }

        /// <summary>
        /// 多文件列(构造函数)
        /// </summary>
        /// <param name="dataField"></param>
        public MoreFileColumn(string dataField)
        {
            this.MiType = "morefilecolumn";
            this.DefaultEditorType = "more_file";

            this.DataField = dataField;
        }

        /// <summary>
        /// 多文件列(构造函数)
        /// </summary>
        /// <param name="dataField"></param>
        /// <param name="headerText"></param>
        public MoreFileColumn(string dataField, string headerText)
        {
            this.MiType = "morefilecolumn";
            this.DefaultEditorType = "more_file";

            this.DataField = dataField;
            this.HeaderText = headerText;
        }

        /// <summary>
        /// 显示模式
        /// </summary>
        [DefaultValue(MoreFileView.Large)]
        public MoreFileView View { get; set; } = MoreFileView.Large;

        /// <summary>
        /// 缩略图宽度
        /// </summary>
        public int ThumbWidth { get; set; } = 60;

        /// <summary>
        /// 缩略图高度
        /// </summary>
        public int ThumbHeight { get; set; } = 60;


        /// <summary>
        /// 用户自定义样式
        /// </summary>
        public string Class { get; set; }

        private void FullScript(ScriptTextWriter st)
        {
            st.RetractBengin("{");
            {
                st.WriteParam("miType", this.MiType);

                st.WriteParam("dataIndex", this.DataField);
                st.WriteParam("width", this.Width);
                st.WriteParam("headerText", this.HeaderText);


                st.WriteParam("id", this.ID);
                st.WriteParam("clientId", this.ClientID);

                st.WriteParam("view", this.View, MoreFileView.Large, TextTransform.Lower);
                
                
                st.WriteParam("userCls", this.Class);

                st.WriteParam("editorMode", this.EditorMode, EditorMode.Auto,  TextTransform.Lower);

                st.RenderBengin("editor");
                {
                    st.WriteParam("xtype", this.DefaultEditorType);                   
                }
                st.RenderEnd();

            }
            st.RetractEnd("}");
            
        }



        public override string CreateHtmlTemplate(Mini.MiniDataControlCellType cellType, Mini.MiniDataControlRowState rowState)
        {
            StringBuilder sb = new StringBuilder();

            ScriptTextWriter stw = new ScriptTextWriter(sb, QuotationMarkConvertor.SingleQuotes);

            FullScript(stw);

            return sb.ToString();
        }
    }
}
