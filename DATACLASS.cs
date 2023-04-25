using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillAppSapB1
{
    class DATACLASS
    {
        CONNECTTION CONN = new CONNECTTION();
        Log Log = new Log();

        public int indexConfig { get; set; }

        public void CREATEFMSMASTERRUNNING()
        {
            try
            {
                string sql1 = "";
                bool result = CONN.ConnectSQL("Bill", indexConfig);
                if (result == true)
                {
                    DataTable dtchkTemp = new DataTable();
                    string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSMASTERRUNING'";
                    SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
                    cmdquery.Fill(dtchkTemp);
                    if (dtchkTemp.Rows.Count == 0)
                    {
                        sql1 = "CREATE TABLE [dbo].[FMSMASTERRUNING]( " + Environment.NewLine;
                        sql1 += "DOCTYPE [NVARCHAR](20) NULL" + Environment.NewLine;
                        sql1 += ",LENGTH [INT] NULL" + Environment.NewLine;
                        sql1 += ",PREFIX [NVARCHAR](50) NULL" + Environment.NewLine;
                        sql1 += ",RUNNO [NVARCHAR](50) NULL" + Environment.NewLine;
                        sql1 += ",COMP [NVARCHAR](50) NULL" + Environment.NewLine;
                        sql1 += ",RCPTYPE [char](6) NULL" + Environment.NewLine;
                        sql1 += ")";
                        SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                        cmd1.ExecuteNonQuery();
                    }
                    CONN.connection.Close();
                }
                else
                {
                    CONN.connection.Close();
                    Log.LogWrite("Error (DATACLASS/CREATEFMSMASTERRUNNING): ConnectSQL:" + result.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.LogWrite("Error (DATACLASS/CREATEFMSMASTERRUNNING): " + ex.Message);
            }
        }

        public void CREATEFMSCUSTOMER()
        {
            string sql1 = "";
            bool result = CONN.ConnectSQL("Bill", indexConfig);
            if (result == true)
            {
                DataTable dtchkTemp = new DataTable();
                string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSOCRDCUSTOMER'";
                SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
                cmdquery.Fill(dtchkTemp);
                if (dtchkTemp.Rows.Count == 0)
                {
                    sql1 = "CREATE TABLE [dbo].[FMSOCRDCUSTOMER]( " + Environment.NewLine;
                    sql1 += "CardCode [nvarchar](255) Not NULL" + Environment.NewLine;
                    sql1 += ",CardName [nvarchar](255) NULL" + Environment.NewLine;
                    sql1 += ",CardType [nvarchar](18) NULL" + Environment.NewLine;
                    sql1 += ",GroupCode [INT] NULL" + Environment.NewLine;
                    sql1 += ",CmpPrivate [nvarchar](18) NULL" + Environment.NewLine;
                    sql1 += ",Address [nvarchar](255) NULL" + Environment.NewLine;
                    sql1 += ",MailAddress [nvarchar](255) NULL"+ Environment.NewLine;
                    sql1 += ",MailZipCode [nvarchar](255) NULL"+ Environment.NewLine;
                    sql1 += ",Phone1 [nvarchar](50) NULL"+ Environment.NewLine;
                    sql1 += ",Phone2 [nvarchar](50) NULL"+ Environment.NewLine;
                    sql1 += ",Fax [nvarchar](100) NULL"+ Environment.NewLine;
                    sql1 += ",City [nvarchar](100) NULL"+ Environment.NewLine;
                    sql1 += ",Country [nvarchar](100) NULL"+ Environment.NewLine;
                    sql1 += ",StreetNo [nvarchar](255) NULL" + Environment.NewLine;
                    sql1 += ")";
                    SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                    cmd1.ExecuteNonQuery();
                }
                CONN.connection.Close();
            }
        }

        public bool CREATEFMSCUSMASTER()
        {
            //CUSMASTER
            try
            {
                string sql1 = "";
                bool result = CONN.ConnectSQL("Bill", indexConfig);
                if (result == true)
                {
                    DataTable dtchkTemp = new DataTable();
                    string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSOADMMASTER'";
                    SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
                    cmdquery.Fill(dtchkTemp);
                    if (dtchkTemp.Rows.Count == 0)
                    {
                        sql1 = "CREATE TABLE [dbo].[FMSOADMMASTER]( " + Environment.NewLine;
                        sql1 += "CONAME [nvarchar](60) Not NULL" + Environment.NewLine;
                        sql1 += ",ADDR [nvarchar](60) NULL" + Environment.NewLine;
                        sql1 += ",STREET [nvarchar](30) NULL" + Environment.NewLine;
                        sql1 += ",COUNTRY [nvarchar](30) NULL" + Environment.NewLine;
                        sql1 += ",PHONE1 [nvarchar](30) NULL" + Environment.NewLine;
                        sql1 += ",PHONE2 [nvarchar](30) NULL" + Environment.NewLine;
                        sql1 += ",FAX [nvarchar](30) NULL" + Environment.NewLine;
                        sql1 += ",ZIPCODE [nvarchar](30) NULL" + Environment.NewLine;
                        sql1 += ",EMAIL [nvarchar](50) NULL" + Environment.NewLine;
                        sql1 += ",MANAGER [nvarchar](50) NULL" + Environment.NewLine;
                        sql1 += ")";
                        SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                        cmd1.ExecuteNonQuery();
                    }
                    CONN.connection.Close();
                    return true;
                }
                else
                {
                    Log.LogWrite("Error (DATACLASS/CREATEFMSCSMASTER): ConnectSQL: " + result.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.LogWrite("Error (DATACLASS/CREATEFMSUSERLOGIN): " + ex.Message);
                return false;
            }
            
        }

        public bool CREATEFMSUSERLOGIN(int indexConfig)
        {
            try
            {
                string sql1 = "";
                bool result = CONN.ConnectSQL("Bill", indexConfig);
                if (result == true)
                {
                    DataTable dtchkTemp = new DataTable();
                    string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSUSERLOGIN'";
                    SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
                    cmdquery.Fill(dtchkTemp);
                    if (dtchkTemp.Rows.Count == 0)
                    {
                        sql1 = "CREATE TABLE [dbo].[FMSUSERLOGIN]( " + Environment.NewLine;
                        sql1 += "ID [NUMERIC](18) NOT NULL UNIQUE" + Environment.NewLine;
                        sql1 += ",USERNAME [varchar](255) NOT NULL UNIQUE" + Environment.NewLine;
                        sql1 += ",PASSWORD [varchar](255) NOT NULL " + Environment.NewLine;
                        sql1 += ",ROLE [varchar](50) NOT NULL" + Environment.NewLine;
                        sql1 += ")";

                        SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                        cmd1.ExecuteNonQuery();
                    }
                    CONN.connection.Close();
                    return true;
                }
                else
                {
                    Log.LogWrite("Error (DATACLASS/CREATEFMSUSERLOGIN): ConnectSQL: " + result.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.LogWrite("Error (DATACLASS/CREATEFMSUSERLOGIN): " + ex.Message);
                return false;
            }
        }
        public void CREATEFMSBILLHEAD()
        {
                string STREXIT = "";
                string sql1 = "";
                bool result = CONN.ConnectSQL("Bill", indexConfig);
                if (result == true)
                {
                    DataTable dtchkTemp = new DataTable();
                    string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSBILLHEAD'";
                    SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
                    cmdquery.Fill(dtchkTemp);
                    if (dtchkTemp.Rows.Count == 0)
                    {
                        sql1 = "CREATE TABLE [dbo].[FMSBILLHEAD]( " + Environment.NewLine;
                        sql1 += "BILLSEQ [NUMERIC](18) NOT NULL" + Environment.NewLine;
                        sql1 += ",BILLNO [NVARCHAR](18) NOT NULL" + Environment.NewLine;
                        sql1 += ",IDCUST [NVARCHAR](18) NULL" + Environment.NewLine;
                        sql1 += ",INVDATE [date] NULL" + Environment.NewLine;
                        sql1 += ",DUEDATE [date] NULL" + Environment.NewLine;
                        sql1 += ",DOCDATE [date] NULL" + Environment.NewLine;
                        sql1 += ",NETAMT [DECIMAL](18,2) NULL" + Environment.NewLine;
                        sql1 += ",AMT [DECIMAL](18,2) NULL" + Environment.NewLine;
                        sql1 += ",COMMENTS [NVARCHAR](100)" + Environment.NewLine;
                        sql1 += ",STA_0 [NVARCHAR](18) NULL" + Environment.NewLine;
                        
                    sql1 += ")";
                        SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                        cmd1.ExecuteNonQuery();
                    }
                    CONN.connection.Close();
                }
        }

        public void CREATEFMSBILLDETAIL()
        {
                string STREXIT = "";
                string sql1 = "";
                bool result = CONN.ConnectSQL("Bill", indexConfig);
                DataTable dtchkTemp = new DataTable();
                string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSBILLDETAIL'";
                SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
                cmdquery.Fill(dtchkTemp);
                if (dtchkTemp.Rows.Count == 0)
                {
                    sql1 = "CREATE TABLE [dbo].[FMSBILLDETAIL]( " + Environment.NewLine;
                    sql1 += "BILLSEQ [NUMERIC](18) NOT NULL" + Environment.NewLine;
                    sql1 += ",BILLNO [NVARCHAR](18) NOT NULL" + Environment.NewLine;
                    sql1 += ",INVNO [NVARCHAR](50) NULL" + Environment.NewLine;
                    sql1 += ",DOCNUM [NVARCHAR](50) NULL" + Environment.NewLine;
                    sql1 += ",DOCTYPE [NVARCHAR](18) NULL" + Environment.NewLine;
                    sql1 += ",IDCUST [NVARCHAR](18) NULL" + Environment.NewLine;
                    sql1 += ",INVDATE [date] NULL" + Environment.NewLine;
                    sql1 += ",DUEDATE [date] NULL" + Environment.NewLine;
                    sql1 += ",DOCDATE [date] NULL" + Environment.NewLine;
                    sql1 += ",LINEAMOUNT [DECIMAL](18,2) NULL" + Environment.NewLine;
                    sql1 += ",AMTOUTSTAND [DECIMAL](18,2) NULL" + Environment.NewLine;
                    sql1 += ",NETAMT [DECIMAL](18,2) NULL" + Environment.NewLine;
                    sql1 += ",CHECK_0 [NVARCHAR](18) NULL" + Environment.NewLine;
                    sql1 += ",LINECOMMENT [NVARCHAR](100) NULL" + Environment.NewLine;
                    sql1 += ",STA_0 [NVARCHAR](18) NULL" + Environment.NewLine;
                    sql1 += ",VATSUM [DECIMAL](18,2) NULL" + Environment.NewLine;
                    sql1 += ",WTBASE [DECIMAL](18,2) NULL" + Environment.NewLine;
                    sql1 += ",WTSUM [DECIMAL](18,2) NULL" + Environment.NewLine;
                    sql1 += ")";
                    SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                    cmd1.ExecuteNonQuery();
                }
                CONN.connection.Close();
        }

        public void CreateTableInit()
        {   // Create table
            CREATEFMSMASTERRUNNING();
            CREATEFMSCUSMASTER();
            CREATEFMSCUSTOMER();
            CREATEFMSBILLHEAD();
            CREATEFMSBILLDETAIL();
            // Insert
            INSERTFMSCUSMMASTER();
            INSERTFMSCUSTOMER();
        }

        public void INSERTBILLDETAIL(DataTable dtBillDetail)
        {
            string STREXIT = "";
            string sql1 = "";
            bool result = CONN.ConnectSQL("Bill", indexConfig);
            DataTable dtchkTemp = new DataTable();
            string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSBILLDETAIL'";
            SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
            cmdquery.Fill(dtchkTemp);
            if (dtchkTemp.Rows.Count > 0)
            {
                for (int i=0;i<= dtBillDetail.Rows.Count-1;i++)
                {
                    sql1 = "INSERT INTO [dbo].[FMSBILLDETAIL] (" + Environment.NewLine;
                    sql1 += "BILLSEQ" + Environment.NewLine;
                    sql1 += ",BILLNO" + Environment.NewLine;
                    sql1 += ",INVNO" + Environment.NewLine;
                    sql1 += ",DOCNUM" + Environment.NewLine;
                    sql1 += ",DOCTYPE" + Environment.NewLine;
                    sql1 += ",IDCUST" + Environment.NewLine;
                    sql1 += ",INVDATE" + Environment.NewLine;
                    sql1 += ",DUEDATE" + Environment.NewLine;
                    sql1 += ",DOCDATE" + Environment.NewLine;
                    sql1 += ",LINEAMOUNT" + Environment.NewLine;
                    sql1 += ",AMTOUTSTAND" + Environment.NewLine;
                    sql1 += ",NETAMT" + Environment.NewLine;
                    sql1 += ",CHECK_0" + Environment.NewLine;
                    sql1 += ",LINECOMMENT" + Environment.NewLine;
                    sql1 += ",STA_0" + Environment.NewLine;
                    sql1 += ",VATSUM" + Environment.NewLine;
                    sql1 += ",WTBASE" + Environment.NewLine;
                    sql1 += ",WTSUM )" + Environment.NewLine;
                    sql1 += "VALUES ( " + Environment.NewLine;
                    sql1 += "'" + Convert.ToInt32(dtBillDetail.Rows[i]["BILLSEQ"]) + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillDetail.Rows[i]["BILLNO"].ToString() + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillDetail.Rows[i]["INVNO"].ToString() + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillDetail.Rows[i]["DOCNUM"].ToString() + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillDetail.Rows[i]["DOCTYPE"].ToString() + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillDetail.Rows[i]["IDCUST"].ToString() + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillDetail.Rows[i]["INVDATE"] + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillDetail.Rows[i]["DUEDATE"] + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillDetail.Rows[i]["DOCDATE"] + "'" + Environment.NewLine;
                    sql1 += ",'"+ Convert.ToDecimal(dtBillDetail.Rows[i]["LINEAMOUNT"]) + "'" + Environment.NewLine;
                    sql1 += ",'"+ Convert.ToDecimal(dtBillDetail.Rows[i]["AMTOUTSTAND"]) + "'" + Environment.NewLine;
                    sql1 += ",'"+ Convert.ToDecimal(dtBillDetail.Rows[i]["NETAMT"]) + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillDetail.Rows[i]["CHECK_0"].ToString() + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillDetail.Rows[i]["LINECOMMENT"].ToString() + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillDetail.Rows[i]["STA_0"].ToString() + "'" + Environment.NewLine;
                    sql1 += ",'" + Convert.ToDecimal(dtBillDetail.Rows[i]["VATSUM"]) + "'" + Environment.NewLine;
                    sql1 += ",'" + Convert.ToDecimal(dtBillDetail.Rows[i]["WTBASE"]) + "'" + Environment.NewLine;
                    sql1 += ",'" + Convert.ToDecimal(dtBillDetail.Rows[i]["WTSUM"]) + "'" + Environment.NewLine;
                    sql1 += ")";
                    SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                    cmd1.ExecuteNonQuery();
                }
                CONN.connection.Close();
            }
        }

        public void INSERTBILLHEAD(DataTable dtBillHead)
        {
            string STREXIT = "";
            string sql1 = "";
            bool result = CONN.ConnectSQL("Bill", indexConfig);
            DataTable dtchkTemp = new DataTable();
            string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSBILLHEAD'";
            SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
            cmdquery.Fill(dtchkTemp);
            if (dtchkTemp.Rows.Count > 0)
            {
                for (int i=0;i<= dtBillHead.Rows.Count -1;i++)
                {
                    sql1 = "INSERT INTO [dbo].[FMSBILLHEAD] (" + Environment.NewLine;
                    sql1 += "BILLSEQ" + Environment.NewLine;
                    sql1 += ",BILLNO" + Environment.NewLine;
                    sql1 += ",IDCUST" + Environment.NewLine;
                    sql1 += ",INVDATE" + Environment.NewLine;
                    sql1 += ",DUEDATE" + Environment.NewLine;
                    sql1 += ",DOCDATE" + Environment.NewLine;
                    sql1 += ",NETAMT" + Environment.NewLine;
                    sql1 += ",AMT" + Environment.NewLine;
                    sql1 += ",COMMENTS" + Environment.NewLine;
                    sql1 += ",STA_0)" + Environment.NewLine;
                    sql1 += "VALUES (" + Environment.NewLine;
                    sql1 += "'" + Convert.ToInt32(dtBillHead.Rows[i]["BILLSEQ"])  + "'" + Environment.NewLine;
                    sql1 += ",'" + dtBillHead.Rows[i]["BILLNO"].ToString() + "'" + Environment.NewLine;
                    sql1 += ",'" + dtBillHead.Rows[i]["IDCUST"].ToString() + "'" + Environment.NewLine;
                    sql1 += ",'" + dtBillHead.Rows[i]["INVDATE"] + "'" + Environment.NewLine;
                    sql1 += ",'" + dtBillHead.Rows[i]["DUEDATE"] + "'" + Environment.NewLine;
                    sql1 += ",'" + dtBillHead.Rows[i]["DOCDATE"] + "'" + Environment.NewLine;
                    sql1 += ",'" + Convert.ToDecimal(dtBillHead.Rows[i]["NETAMT"]) + "'" + Environment.NewLine;
                    sql1 += ",'" + Convert.ToDecimal(dtBillHead.Rows[i]["AMT"]) + "'" + Environment.NewLine;
                    sql1 += ",'" + dtBillHead.Rows[i]["COMMENT"].ToString() + "'" + Environment.NewLine;
                    sql1 += ",'"+ dtBillHead.Rows[i]["STA_0"].ToString() + "' )" + Environment.NewLine;
                    SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                    cmd1.ExecuteNonQuery();
                }
                CONN.connection.Close();
            }
        }

        public void INSERTFMSCUSTOMER()
        {
            string STREXIT = "";
            string sql1 = "";
            bool result = CONN.ConnectSQL("Bill", indexConfig);
            DataTable dtchkTemp = new DataTable();
            string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSOCRDCUSTOMER'";
            SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
            cmdquery.Fill(dtchkTemp);
            if (dtchkTemp.Rows.Count > 0)
            {
                sql1 = "DELETE FROM FMSOCRDCUSTOMER " + Environment.NewLine;
                sql1 += "INSERT INTO [OCRBILL].[dbo].[FMSOCRDCUSTOMER] (" + Environment.NewLine;
                sql1 += "CardCode" +
                        ",CardName" +
                        ",CardType" +
                        ",GroupCode" +
                        ",CmpPrivate" +
                        ",Address" +
                        ",MailAddress" +
                        ",MailZipCode" +
                        ",Phone1" +
                        ",Phone2" +
                        ",Fax" +
                        ",City" +
                        ",Country" +
                        ",StreetNo ) " + Environment.NewLine;
                sql1 += "SELECT " + Environment.NewLine;
                sql1 += "CardCode" +
                        ",CardName" +
                        ",CardType" + 
                        ",GroupCode" +
                        ",CmpPrivate" +
                        ",Address" +
                        ",MailAddres" +
                        ",MailZipCod" +
                        ",Phone1" +
                        ",Phone2" +
                        ",Fax" +
                        ",City" +
                        ",Country" +
                        ",StreetNo " + Environment.NewLine;
                sql1 += "FROM [OCR].[dbo].OCRD";
                SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                cmd1.ExecuteNonQuery();
            }
            CONN.connection.Close();
        }

        public void INSERTFMSCUSMMASTER()
        {
            string STREXIT = "";
            string sql1 = "";
            bool result = CONN.ConnectSQL("Bill", indexConfig);
            DataTable dtchkTemp = new DataTable();
            string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSOADMMASTER'";
            SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
            cmdquery.Fill(dtchkTemp);
            if (dtchkTemp.Rows.Count > 0)
            {
                sql1 = "DELETE FROM FMSOADMMASTER " + Environment.NewLine;
                sql1 += "INSERT INTO [OCRBILL].[dbo].[FMSOADMMASTER] (CONAME,ADDR,COUNTRY,PHONE1,PHONE2,FAX,EMAIL,MANAGER)" + Environment.NewLine;
                sql1 += "SELECT CompnyName,CompnyAddr,Country,Phone1,Phone2,Fax,E_mail,Manager" + Environment.NewLine;
                sql1 += "FROM [OCR].[dbo].OADM";
                SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                cmd1.ExecuteNonQuery();
            }
            CONN.connection.Close();
        }

        public bool INSERTFMSMASTERRUNNO(DataTable dt)
        {
            try
            {
                string STREXIT = "";
                string sql1 = "";
                bool result = CONN.ConnectSQL("Bill", indexConfig);
                DataTable dtchkTemp = new DataTable();
                string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSMASTERRUNING'";
                SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
                cmdquery.Fill(dtchkTemp);
                if (dtchkTemp.Rows.Count > 0)
                {
                    sql1 = "DELETE FROM [FMSMASTERRUNING]" + Environment.NewLine;
                    for (int i = 0; i <= dt.Rows.Count-1; i++)
                    {
                        sql1 += "INSERT INTO [dbo].[FMSMASTERRUNING](" + Environment.NewLine;
                        sql1 += "DOCTYPE" + Environment.NewLine;
                        sql1 += ",LENGTH" + Environment.NewLine;
                        sql1 += ",PREFIX" + Environment.NewLine;
                        sql1 += ",RUNNO" + Environment.NewLine;
                        sql1 += ",COMP" + Environment.NewLine;
                        sql1 += ",RCPTYPE " + Environment.NewLine;
                        sql1 += ")";
                        sql1 += "VALUES(" + Environment.NewLine;
                        sql1 += "'" + dt.Rows[i]["DOCTYPE"].ToString().Trim() + "'" + Environment.NewLine;
                        sql1 += ",'" + dt.Rows[i]["LENGTH"].ToString().Trim() + "'" + Environment.NewLine;
                        sql1 += ",'" + dt.Rows[i]["PREFIX"].ToString().Trim() + "'" + Environment.NewLine;
                        sql1 += ",'" + dt.Rows[i]["RUNNO"].ToString().Trim() + "'" + Environment.NewLine;
                        sql1 += ",'" + dt.Rows[i]["COMP"].ToString().Trim() + "'" + Environment.NewLine;
                        sql1 += ",'" + dt.Rows[i]["RCPTYPE"].ToString().Trim() + "'" + Environment.NewLine;
                        sql1 += ")";
                    }
                    SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                    cmd1.ExecuteNonQuery();
                }
                CONN.connection.Close();
                Log.LogWrite("Complte (DATACLASS/INSERTFMSMASTERRUNNO): " + "Insert Complete");
                return true;
            }
            catch (Exception ex)
            {
                Log.LogWrite("Error (DATACLASS/INSERTFMSMASTERRUNNO): " + ex.Message);
                return false;
            }
            
        }

        public void UPDATEFMSRUNNO(string DOCTYPE,string BILLNO) //Billing Note,Receipt
        {   //split 'BTN','0000000000'
            string[] runing = BILLNO.Split('-');
            string STREXIT = "";
            string sql1 = "";
            bool result = CONN.ConnectSQL("Bill", indexConfig);
            DataTable dtchkTemp = new DataTable();
            string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSMASTERRUNING'";
            SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
            cmdquery.Fill(dtchkTemp);
            if (dtchkTemp.Rows.Count > 0)
            {
                sql1 = "UPDATE FMSMASTERRUNING " + Environment.NewLine;
                sql1 += "SET RUNNO = '" + runing[1] + "'" + Environment.NewLine;
                sql1 += "WHERE DOCTYPE = '" + DOCTYPE + "'";
                SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                cmd1.ExecuteNonQuery();

            }
            CONN.connection.Close();
        }

        public void DELETEFMSBILL(string BILLNO,string IDCUST)
        {
            string STREXIT = "";
            string sql1 = "";
            bool result = CONN.ConnectSQL("Bill", indexConfig);
            DataTable dtchkTemp = new DataTable();
            string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSBILLDETAIL'";
            SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
            cmdquery.Fill(dtchkTemp);
            if (dtchkTemp.Rows.Count > 0)
            {
                sql1 = "DELETE FROM FMSBILLHEAD WHERE BILLNO = '" + BILLNO + "'" + "AND IDCUST = '" + IDCUST + "'" + Environment.NewLine;
                sql1 += "DELETE FROM FMSBILLDETAIL WHERE BILLNO = '" + BILLNO + "'" + "AND IDCUST = '" + IDCUST + "'";
                SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                cmd1.ExecuteNonQuery();
            }
            CONN.connection.Close();
        }
    }
}
