using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// ID 控制属性
    /// </summary>
    public interface IClientIDMode
    {
        ClientIDMode ClientIDMode { get; set; }

        string GetClientID();

        string ClientID { get; }
    }
}
