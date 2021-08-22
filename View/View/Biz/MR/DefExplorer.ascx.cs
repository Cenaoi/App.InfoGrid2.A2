using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.Biz.MR
{
    public partial class DefExplorer : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            store1.PageLoading += Store1_PageLoading;
            store1.BatchDeleted += Store1_BatchDeleted;

            this.TreePanel1.Creating += TreePanel1_Creating;
            this.TreePanel1.Removeing += TreePanel1_Removeing;

            this.TreePanel1.Renaming += TreePanel1_Renaming;

            

            fileUpload1.Uploader += FileUpload1_Uploader;

            if (!IsPostBack)
            {
                InitData();
            }


        }

        private void TreePanel1_Renaming(object sender, TreeNodeRenameEventArgs e)
        {

            TreeNode node = e.Node;

            string dir_path = GetDirPath(node);

            DirectoryInfo dirInfo = new DirectoryInfo(dir_path);

            string new_dir_path = dirInfo.Parent.FullName + "\\" +e.Text;

            if (Directory.Exists(new_dir_path))
            {
                MessageBox.Alert("目录名称已存在！");
                return;
            }

            SModel sm = GetSmById();

            SModel sm_tag = new SModel();


            string root_path = sm.Get<string>("ROOT_PATH");

            string dd = new_dir_path.Substring(root_path.Length);

            sm_tag["dir"] = dd;

            node.Tag = sm_tag.ToJson();


            dirInfo.MoveTo(new_dir_path);


            node.RemoveChilds();
       
        }

        private void TreePanel1_Removeing(object sender, TreeNodeCancelEventArgs e)
        {

            TreeNode node = this.TreePanel1.NodeSelected;

            string dir_path = GetDirPath(node);

            Directory.Delete(dir_path, true);
                

        }

        private void TreePanel1_Creating(object sender, EventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            SModel sm = GetSmById();

            string dir_path = GetDirPath(node);

            DirectoryInfo dirInfo = new DirectoryInfo(dir_path);

            string new_dir_name = "新建文件夹_"+DateTime.Now.ToString("yyyyMMddHHmmss");
            
            DirectoryInfo dir = Directory.CreateDirectory(dir_path + "\\" + new_dir_name);

            var tNode1 = CreateNode(node, dir, sm.Get<string>("ROOT_PATH"));

            TreePanel1.Add(tNode1);


            node.ChildLoaded = true;


            TreePanel1.Refresh();



        }

        private void FileUpload1_Uploader(object sender, EventArgs e)
        {

            SModel sm_ws = GetSmById();

            string tagStr = WebUtil.Query("tag");

            string full_path = sm_ws.Get<string>("ROOT_PATH") + tagStr;


            string file_name = fileUpload1.FileName;


            string full_name = full_path + "\\" + file_name;



            if (File.Exists(full_name))
            {
                File.Delete(full_name);
            }


            fileUpload1.SaveAs(full_name);

            EasyClick.Web.Mini2.ScriptManager.Eval("widget1_I_store1.loadPage(0)");

        }

        private void Store1_PageLoading(object sender, CancelPageEventArags e)
        {
            e.Cancel = true;


            TreePanel1_Selected(null, null);

        }

        private void Store1_BatchDeleted(object sender, ObjectListEventArgs e)
        {
            

            var drc = table2.CheckedRows;

            SModel sm_ws = GetSmById();

            foreach(var dr in drc)
            {

               string file_path =  dr["FILE_PATH"].ToString();

               string full_path = sm_ws.Get<string>("ROOT_PATH") + file_path;

                File.Delete(full_path);


            }



            TreePanel1_Selected(null, null);

            Toast.Show("删除成功！");
        }


        /// <summary>
        ///初始化数据
        /// </summary>
        void InitData()
        {

            int id = WebUtil.QueryInt("id");


            SModel sm = GetSmById();

            if(sm == null)
            {
                return;
            }

            string root_path = sm.Get<string>("ROOT_PATH");


            if (string.IsNullOrWhiteSpace(root_path) || !Directory.Exists(root_path))
            {

                return;

            }


            DirectoryInfo dirInfo = new DirectoryInfo(root_path);

            root_path = dirInfo.FullName.Replace("\\", "\\\\");

            string rootId = "ROOT_" + id;

            SModel root_sm = new SModel();
            

            TreeNode tNode = new TreeNode(dirInfo.Name, rootId);
            tNode.ParentId = "0";
            tNode.NodeType = "default";
            tNode.StatusID = 0;
            tNode.Tag = root_sm.ToJson();
            tNode.Expand();

            TreePanel1.Add(tNode);


            foreach (var dir in dirInfo.GetDirectories())
            {
               var tNode1 =  CreateNode(tNode, dir, dirInfo.FullName);

                TreePanel1.Add(tNode1);

            }


            tNode.ChildLoaded = true;





        }

        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="p_node">上级节点</param>
        /// <param name="dir">当前目录对象</param>
        /// <param name="root_path">根目录</param>
        /// <returns></returns>
        TreeNode CreateNode(TreeNode p_node, DirectoryInfo dir,string root_path)
        {

            string dirName = dir.Name;

            string tag = p_node.Tag;


            SModel sm = SModel.ParseJson(tag);

            SModel sm_tag = new SModel();

          
            string dd = dir.FullName.Substring(root_path.Length);

            sm_tag["dir"] = dd;

            string nodeId = dd.Replace(@"\", @"_").Replace(" ", @"_").Replace("(","_").Replace(")","_");

            TreeNode tNode1 = new TreeNode(dir.Name, nodeId);
            tNode1.ParentId = p_node.Value;
            tNode1.NodeType = "default";
            tNode1.StatusID = 0;
            tNode1.Tag = sm_tag.ToJson();
            tNode1.Expand();

            return tNode1;


        }

        SModel GetSmById()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");

            SModel sm = decipher.GetSModel($"select * from MR_WEBSITE where ROW_SID >=0 and MR_WEBSITE_ID = {id}");

            return sm;

        }

        /// <summary>
        /// 节点点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreePanel1_Selected(object sender, EventArgs e)
        {


            TreeNode node = this.TreePanel1.NodeSelected;


            SModel sm_tag = SModel.ParseJson(node.Tag);

            SModel sm_ws = GetSmById();

            string dir_path = sm_ws.Get<string>("ROOT_PATH") + sm_tag.Get<string>("dir");

            DirectoryInfo root_dir = new DirectoryInfo(dir_path);

            LoadFile(root_dir, sm_ws.Get<string>("ROOT_PATH"));

            fileUpload1.TempValue = sm_tag.Get<string>("dir");


            this.fileUpload1.Tag = sm_tag.Get<string>("dir");

            if (node.ChildLoaded)
            {
                return;
            }

            foreach (var dir in root_dir.GetDirectories())
            {
                var tNode1 = CreateNode(node, dir, sm_ws.Get<string>("ROOT_PATH"));

                TreePanel1.Add(tNode1);

            }

            node.ChildLoaded = true;


            this.TreePanel1.Refresh();


        }

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="dir">当前目录对象</param>
       /// <param name="root_path">根目录</param>
        void LoadFile(DirectoryInfo dir,string root_path)
        {

            SModelList sms = GetFiles(dir,root_path);

            store1.RemoveAll();

            store1.BeginLoadData();
            {
                store1.AddRange(sms);
                store1.SetTotalCount(sms.Count);
            }
            store1.EndLoadData();

        }


        /// <summary>
        /// 返回文件列表
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="root_path">根目录</param>
        /// <returns></returns>
        SModelList GetFiles(DirectoryInfo dir,string root_path)
        {

            SModelList sms = new SModelList();

            var files = dir.GetFiles();

            foreach (FileInfo file in files)
            {

                SModel sm = new SModel();

                sm["FILE_NAME"] = file.Name;
                sm["FILE_SIZE"] = file.Length;
                sm["FILE_PATH"] = file.FullName.Substring(root_path.Length);

                sms.Add(sm);


            }

            return sms;


               
        }

        /// <summary>
        /// 根据节点获取绝对路径
        /// </summary>
        /// <param name="p_node">节点</param>
        /// <returns></returns>
        string GetDirPath(TreeNode p_node)
        {
            if(p_node == null)
            {
                return string.Empty;
            }

            SModel sm_ws = GetSmById();

            SModel sm_tag = SModel.ParseJson(p_node.Tag);


            string dir_path = sm_ws.Get<string>("ROOT_PATH") + sm_tag.Get<string>("dir");


            return dir_path;
        }


    }
}