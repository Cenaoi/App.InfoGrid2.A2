<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.Page" %>
<%@ Import Namespace="System.IO" %>

<script runat="server">
    
    protected override void OnLoad(EventArgs e)
    {
        Test_App_Data();

        Test_App_Data_CreateFile();

        Test_File();
    }

    private void Test_App_Data()
    {
        string appData = Server.MapPath("~/App_Data");

        this.Response.Write("测试目录 --------------<br />");

        if (System.IO.Directory.Exists(appData))
        {
            Response.Write("App_Data 目录正常 <br/>");
        }
        else
        {
            Response.Write("[错误] App_Data 目录不存在< br />");
        }
    }


    private void Test_App_Data_CreateFile()
    {
        string appData = Server.MapPath("~/App_Data/Test.txt");

        this.Response.Write("测试 ~/App_Data/Test.txt --------------<br />");

        string tmpFile = appData;


        if (System.IO.File.Exists(tmpFile))
        {
            this.Response.Write("测试文件存在，试图删除。。。。");

            try
            {
                File.Delete(tmpFile);

                this.Response.Write("成功! <br/>");
            }
            catch (Exception ex)
            {
                this.Response.Write("删除失败! <br/>");
            }
        }
        else
        {
            this.Response.Write("创建文件....");

            try
            {
                File.AppendAllText(tmpFile, "你好啊 啊啊啊啊啊啊!");

                this.Response.Write("成功! <br/>");
            }
            catch (Exception ex)
            {
                this.Response.Write("删除失败! <br/>" + ex.Message);
            }
        }


    }

    private void Test_File()
    {
        string dir = Server.MapPath("~/Test_WebSite");
        string file = Server.MapPath("~/Test_WebSite/Test_File.txt");

        Response.Write("<br>目录、文件读取操作----------------<br />");

        if (Directory.Exists(dir))
        {
            Response.Write("<br />Test_WebSite 目录存在:");

            try
            {
                Directory.Delete(dir, true);

                Response.Write("删除目录成功！");
            }
            catch (Exception ex)
            {
                Response.Write("删除文件错误:" + ex.Message);
            }
        }

        try
        {
            Response.Write("<br />创建目录：");

            Directory.CreateDirectory(dir);

            Response.Write("成功!");



            Response.Write("<br />创建 Text_File.txt :");

            try
            {
                File.WriteAllText(file, "测试数据");
                Response.Write("成功!");


                try
                {
                    Response.Write("<br />修改 Text_File.txt:");
                    File.AppendAllText(file, "\n新行");

                    Response.Write("成功!");
                }
                catch
                {

                    Response.Write("失败！");
                }


                try
                {
                    Response.Write("<br />删除 Text_File.txt:");
                    File.Delete(file);

                    Response.Write("成功!");
                }
                catch
                {

                    Response.Write("失败！");
                }

            }
            catch (Exception ex)
            {
                Response.Write("失败!");
            }





            Response.Write("<br />删除目录:");

            Directory.Delete(dir, true);

            Response.Write("成功");
        }
        catch (Exception ex)
        {
            Response.Write("失败");
        }

        try
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

        }
        catch
        {
        }
    }


    </script>