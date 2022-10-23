using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHST.Models;
using NHST.Bussiness;
using System.Data;
using WebUI.Business;
using MB.Extensions;

namespace NHST.Controllers
{
    public class HistoryIntroduceController
    {
        public static string Insert(int UID, string Username, string Amount, string TradeContent, string AmountLeft, int Type, DateTime CreatedDate, string CreatedBy)
        {
            using (var dbe = new NHSTEntities())
            {               
                tbl_HistoryIntroduce a = new tbl_HistoryIntroduce();
                a.UID = UID;
                a.Username = Username;
                a.Amount = Amount;
                a.TradeContent = TradeContent;
                a.AmountLeft = AmountLeft;
                a.Type = Type;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                dbe.tbl_HistoryIntroduce.Add(a);
                int kq = dbe.SaveChanges();              
                string k = a.ID.ToString();
                return k;
            }
        }
        public static List<tbl_HistoryIntroduce> GetByUID(int UID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_HistoryIntroduce> aus = new List<tbl_HistoryIntroduce>();
                aus = dbe.tbl_HistoryIntroduce.Where(a => a.UID == UID).OrderByDescending(a => a.ID).ToList();
                return aus;
            }
        }
        public static int GetTotalFromDateTodate(string fd, string td, int UID)
        {
            var sql = @"select Total=Count(*) ";
            sql += "from tbl_HistoryIntroduce ";
            sql += "where UID = " + UID + "";           
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            int a = 0;
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }
        public static List<tbl_HistoryIntroduce> GetFromDateTodate(string fd, string td, int pageSize, int pageIndex, int UID)
        {
            var sql = @"select * ";
            sql += "from tbl_HistoryIntroduce ";
            sql += "where UID = " + UID + "";
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            sql += " order by ID DESC OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<tbl_HistoryIntroduce> a = new List<tbl_HistoryIntroduce>();
            while (reader.Read())
            {
                var entity = new tbl_HistoryIntroduce();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["UID"] != DBNull.Value)
                    entity.UID = reader["UID"].ToString().ToInt(0);
                if (reader["Type"] != DBNull.Value)
                    entity.Type = reader["Type"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["Amount"] != DBNull.Value)
                    entity.Amount = reader["Amount"].ToString();
                if (reader["TradeContent"] != DBNull.Value)
                    entity.TradeContent = reader["TradeContent"].ToString();
                if (reader["AmountLeft"] != DBNull.Value)
                    entity.AmountLeft = reader["AmountLeft"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                a.Add(entity);
            }
            reader.Close();
            return a;
        }
    }
}