using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace BillAppSapB1
{
    class Log
    {
        private string m_exePath = string.Empty;
        public void LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }
        public void LogWrite(string logMessage)
        {
            m_exePath = Application.StartupPath.ToString() + "\\log\\log.txt";
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath))
                {
                    _Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void _Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }



    }
}
