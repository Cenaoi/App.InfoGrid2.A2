using HWQ.Entity.LightModels;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EC5.Action3.SEngine
{
    public class Class1
    {

        public string Game(string ppp)
        {
            string code = @"
                using System;
                using System.Text;
                using System.IO;
                using System.Collections.Generic;
                using System.Xml;

                namespace EC5_CSharpScript
                {
                  public class GameX2
                  {
                    private StringBuilder Response  = new StringBuilder();
                    

                    public GameX2()
                    {

                    }

                    public string Exe()
                    {

                        return DateTime.Now.ToString();
                    
                    }

                  }
                }
              ";



            code = File.ReadAllText(@"C:\GameX2.cs", Encoding.UTF8);




            CSharpScript cs = new CSharpScript(code);
            cs.Compiler();

            object result = cs.ExecuteMethod("EC5_CSharpScript.GameX2", "Exe", new object[] { ppp });


            return (string)result;
        }



        public object Game2(string ppp)
        {            
            string code = File.ReadAllText(@"C:\GameX2.cs", Encoding.UTF8);

            
            CSharpScript cs = new CSharpScript(code);
            cs.Compiler();

            object result = cs.ExecuteMethod("EC5_CSharpScript.GameX2", "Exe", new object[] { ppp });


            return (string)result;

        }

    }







}
