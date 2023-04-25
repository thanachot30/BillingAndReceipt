using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace BillAppSapB1
{
    class CONNECTTION
    {
        public SqlConnection connection;
        public SqlCommand Command;
        public SqlDataAdapter adapter;
        public SqlDataReader dataReader;
        public DataTable dtConfig = new DataTable();

        public Log Log = new Log();
        
        public Boolean ConnectSQL(string DB,int indexConfig)
        {
            try
            {
                dtConfig = ReadConfigDB();
                
                if (dtConfig.Rows.Count > 0)
                {
                    string BillDB = "data source = '" + dtConfig.Rows[indexConfig]["SERVER"]
                        + "';initial catalog='" + dtConfig.Rows[indexConfig]["DBBILL"]
                        + "';user id='" + dtConfig.Rows[indexConfig]["USER"]
                        + "';password='" + dtConfig.Rows[indexConfig]["PASSWORD"]
                        + "'";

                    string SAPDB = "data source = '" + dtConfig.Rows[indexConfig]["SERVERSource"]
                        + "';initial catalog='" + dtConfig.Rows[indexConfig]["DBSource"]
                        + "';user id='" + dtConfig.Rows[indexConfig]["USERSAP"]
                        + "';password='" + dtConfig.Rows[indexConfig]["PASSSAP"]
                        + "'";
                    if (DB == "Bill")
                    {
                        connection = new SqlConnection(BillDB);
                    }
                    else if (DB == "Source")
                    {
                        connection = new SqlConnection(SAPDB);
                    }
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.LogWriter(ex.Message);
                return false;
            }
        }

        public string TestConnectSQL(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    string BillDB = "data source = '" + dt.Rows[0]["SERVER"]
                        + "';initial catalog='" + dt.Rows[0]["DBBILL"]
                        + "';user id='" + dt.Rows[0]["USER"]
                        + "';password='" + dt.Rows[0]["PASSWORD"]
                        + "'";

                    string SAPDB = "data source = '" + dt.Rows[0]["SERVERSource"]
                        + "';initial catalog='" + dt.Rows[0]["DBSource"]
                        + "';user id='" + dt.Rows[0]["USERSAP"]
                        + "';password='" + dt.Rows[0]["PASSSAP"]
                        + "'";
                    try
                    {
                        connection = new SqlConnection(BillDB);
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    connection.Close();

                    try
                    {
                        connection = new SqlConnection(SAPDB);
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    connection.Close();
                    return "true";
                }
                else
                {
                    return "Empty Config DB";
                }
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public DataTable ReadConfigDB()
        {
            DataTable dt = new DataTable("dt");
            
            if (dt.Rows.Count == 0) {
                dt.Columns.Add("ID");
                dt.Columns.Add("CONAME");
                dt.Columns.Add("SERVER");
                dt.Columns.Add("USER");
                dt.Columns.Add("PASSWORD");
                dt.Columns.Add("DBBILL");
                dt.Columns.Add("SERVERSource");
                dt.Columns.Add("USERSAP");
                dt.Columns.Add("PASSSAP");
                dt.Columns.Add("DBSource");
            }
           
            XmlDocument xmlDoc = new XmlDocument(); //For loading xml file to read
            string locfile = Application.StartupPath.ToString() + "\\config\\DBCONFIG.xml";
            xmlDoc.Load(locfile);

            XmlNodeList ArticleNodeList = xmlDoc.GetElementsByTagName("row"); //'For getting the list of main/parent nodes
            foreach (XmlNode articlenode in ArticleNodeList)
            {
                dt.Rows.Add();
            }
            int j = 0;
            ArticleNodeList = xmlDoc.GetElementsByTagName("row");
            foreach (XmlNode articlenode in ArticleNodeList)
            {
                foreach (XmlNode basenode in articlenode)
                {
                    string result = "";
                    result = basenode.Name;
                    switch (result)
                    {
                        case "ID":
                            dt.Rows[j]["ID"] = basenode.InnerText;
                            break;
                        case "CONAME":
                            dt.Rows[j]["CONAME"] = basenode.InnerText;
                            break;
                        case "SERVER":
                            dt.Rows[j]["SERVER"] = basenode.InnerText;
                            break;
                        case "USER":
                            dt.Rows[j]["USER"] = basenode.InnerText;
                            break;
                        case "PASSWORD":
                            dt.Rows[j]["PASSWORD"] = basenode.InnerText;
                            break;
                        case "DBBILL":
                            dt.Rows[j]["DBBILL"] = basenode.InnerText;
                            break;
                        case "SERVERSource":
                            dt.Rows[j]["SERVERSource"] = basenode.InnerText;
                            break;
                        case "DBSource":
                            dt.Rows[j]["DBSource"] = basenode.InnerText;
                            break;
                        case "USERSAP":
                            dt.Rows[j]["USERSAP"] = basenode.InnerText;
                            break;
                        case "PASSSAP":
                            dt.Rows[j]["PASSSAP"] = basenode.InnerText;
                            break;
                    }
                }
                j += 1;
            }
            return dt;
        }
    }
}
