using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;

namespace MABEE_MVC.Models
{
    public class MabeeDB
    {
        Database DB = new DatabaseProviderFactory().Create("MabeeConnectionString");//連線資料庫


        public DateTime TWtime()
        { //以台北時間為基準
            DateTime today = DateTime.Now;
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time"); // 取得台北時區與格林威治標準時間差
            DateTime TWTime = TimeZoneInfo.ConvertTime(today, est); // 轉換為台北時間
            return TWTime.AddDays(0);
        }

        public DataTable dt_product_con(string series, string language, string link_name)
        {//撈出該產品
            using (DbCommand se = DB.GetSqlStringCommand(@"SELECT *   FROM   product  WHERE  link_name =@link_name and series=@series  and language=@language  and  show_on=1  and Recycle=1 "))
            {
                DB.AddInParameter(se, "@series", DbType.String, series);
                DB.AddInParameter(se, "@language", DbType.String, language);
                DB.AddInParameter(se, "@link_name", DbType.String, link_name);
                DataTable dt = DB.ExecuteDataSet(se).Tables[0];
                return dt;
            }
        }


        public void Update_user_on(int id, bool delete_on)
        {//刪除會員到垃圾桶
            using (DbConnection dbconn = DB.CreateConnection())
            {
                dbconn.Open();
                using (DbTransaction dbtrans = dbconn.BeginTransaction())
                using (DbCommand updbcmd = DB.GetSqlStringCommand(@"UPDATE admin_user SET delete_on=@delete_on  WHERE   id=@id"))
                   
                {
                    DB.AddInParameter(updbcmd, "@id", DbType.Int32, id);
                    DB.AddInParameter(updbcmd, "@delete_on", DbType.Boolean, delete_on);

                    try
                    {
                        DB.ExecuteNonQuery(updbcmd);
                        dbtrans.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbtrans.Rollback();
                        throw ex;
                    }
                }
            }
        }



    }
}