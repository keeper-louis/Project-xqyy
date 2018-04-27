using Kingdee.BOS;
using Kingdee.BOS.App;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Contracts.Report;
using Kingdee.BOS.Core.Report;
using Kingdee.BOS.Orm.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louis.K3.SCM.App.Report
{
    [Description("分期收款发出商品汇总表")]
    public class FQSKFCSPReport:SysReportBaseService
    {
        #region 参数设置
        private String tempTable = String.Empty;
        #endregion
        #region 初始化报表参数
        public override void Initialize()
        {
            base.Initialize();
            this.ReportProperty.ReportType = ReportType.REPORTTYPE_MOVE;
            this.ReportProperty.IsGroupSummary = true;
            this.ReportProperty.ReportName = new LocaleValue("分期收款发出商品汇总表", base.Context.UserLocale.LCID);
        }
        #endregion
        #region 表列设置
        /// <summary>
        /// 动态构造列
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override ReportHeader GetReportHeaders(IRptParams filter)
        {
            ReportHeader header = new ReportHeader();
            DynamicObject dyFilter = filter.FilterParameter.CustomFilter;
            int comboValue = Convert.ToInt32(dyFilter["F_Ls_Combo"]);
            //物料
            if (comboValue == 1)
            {
                var FFINDATE = header.AddChild("FFINDATE", new LocaleValue("会计期间"));
                FFINDATE.ColIndex = 0;
                var FMATERIALNO = header.AddChild("FMATERIALNO", new LocaleValue("物料代码"));
                FMATERIALNO.ColIndex = 1;
                var FMATERIALNAME = header.AddChild("FMATERIALNAME", new LocaleValue("物料名称"));
                FMATERIALNAME.ColIndex = 2;
                var FSPE = header.AddChild("FSPE", new LocaleValue("规格型号"));
                FSPE.ColIndex = 3;
                var UNITID = header.AddChild("UNITID", new LocaleValue("计量单位"));
                UNITID.ColIndex = 4;
                var QCQTY = header.AddChild("QCQTY", new LocaleValue("期初&数量"), SqlStorageType.SqlDecimal);
                QCQTY.ColIndex = 5;
                var QCPRICE = header.AddChild("QCPRICE", new LocaleValue("期初&单价"), SqlStorageType.SqlDecimal);
                QCPRICE.ColIndex = 6;
                var QCAMOUNT = header.AddChild("QCAMOUNT", new LocaleValue("期初&金额"), SqlStorageType.SqlDecimal);
                QCAMOUNT.ColIndex = 7;
                var OUTQUT = header.AddChild("OUTQTY", new LocaleValue("发出&数量"), SqlStorageType.SqlDecimal);
                OUTQUT.ColIndex = 8;
                var OUTPRICE = header.AddChild("OUTPRICE", new LocaleValue("发出&单价"), SqlStorageType.SqlDecimal);
                OUTPRICE.ColIndex = 9;
                var OUTAMOUNT = header.AddChild("OUTAMOUNT", new LocaleValue("发出&金额"), SqlStorageType.SqlDecimal);
                OUTAMOUNT.ColIndex = 10;
                var GJQTY = header.AddChild("GJQTY", new LocaleValue("勾稽&数量"), SqlStorageType.SqlDecimal);
                GJQTY.ColIndex = 11;
                var GJPRICE = header.AddChild("GJPRICE", new LocaleValue("勾稽&单价"), SqlStorageType.SqlDecimal);
                GJPRICE.ColIndex = 12;
                var GJAMOUNT = header.AddChild("GJAMOUNT", new LocaleValue("勾稽&金额"), SqlStorageType.SqlDecimal);
                OUTAMOUNT.ColIndex = 13;
                var QMQTY = header.AddChild("QMQTY", new LocaleValue("期末&数量"), SqlStorageType.SqlDecimal);
                QMQTY.ColIndex = 12;
                var QMPRICE = header.AddChild("QMPRICE", new LocaleValue("期末&单价"), SqlStorageType.SqlDecimal);
                QMPRICE.ColIndex = 13;
                var QMAMOUNT = header.AddChild("QMAMOUNT", new LocaleValue("期末&金额"), SqlStorageType.SqlDecimal);
                QMAMOUNT.ColIndex = 14;
                return header;
            }
            else
            {
                var FFINDATE = header.AddChild("FFINDATE", new LocaleValue("会计期间"));
                FFINDATE.ColIndex = 0;
                var FCUSTNO = header.AddChild("FCUSTNO",new LocaleValue("客户编号"));
                FCUSTNO.ColIndex = 1;
                var FCUSTNAME = header.AddChild("FCUSTNAME", new LocaleValue("客户"));
                FCUSTNAME.ColIndex = 2;
                var FMATERIALNO = header.AddChild("FMATERIALNO", new LocaleValue("物料代码"));
                FMATERIALNO.ColIndex = 3;
                var FMATERIALNAME = header.AddChild("FMATERIALNAME", new LocaleValue("物料名称"));
                FMATERIALNAME.ColIndex = 4;
                var FSPE = header.AddChild("FSPE", new LocaleValue("规格型号"));
                FSPE.ColIndex = 5;
                var QCQTY = header.AddChild("QCQTY", new LocaleValue("期初&数量"), SqlStorageType.SqlDecimal);
                QCQTY.ColIndex = 6;
                var QCPRICE = header.AddChild("QCPRICE", new LocaleValue("期初&单价"), SqlStorageType.SqlDecimal);
                QCPRICE.ColIndex = 7;
                var QCAMOUNT = header.AddChild("QCAMOUNT", new LocaleValue("期初&金额"), SqlStorageType.SqlDecimal);
                QCAMOUNT.ColIndex = 8;
                var OUTQUT = header.AddChild("OUTQTY", new LocaleValue("发出&数量"), SqlStorageType.SqlDecimal);
                OUTQUT.ColIndex = 9;
                var OUTPRICE = header.AddChild("OUTPRICE", new LocaleValue("发出&单价"), SqlStorageType.SqlDecimal);
                OUTPRICE.ColIndex = 10;
                var OUTAMOUNT = header.AddChild("OUTAMOUNT", new LocaleValue("发出&金额"), SqlStorageType.SqlDecimal);
                OUTAMOUNT.ColIndex = 11;
                var GJQTY = header.AddChild("GJQTY", new LocaleValue("勾稽&数量"), SqlStorageType.SqlDecimal);
                GJQTY.ColIndex = 12;
                var GJPRICE = header.AddChild("GJPRICE", new LocaleValue("勾稽&单价"), SqlStorageType.SqlDecimal);
                GJPRICE.ColIndex = 13;
                var GJAMOUNT = header.AddChild("GJAMOUNT", new LocaleValue("勾稽&金额"), SqlStorageType.SqlDecimal);
                GJAMOUNT.ColIndex = 14;
                var QMQTY = header.AddChild("QMQTY", new LocaleValue("期末&数量"), SqlStorageType.SqlDecimal);
                QMQTY.ColIndex = 15;
                var QMPRICE = header.AddChild("QMPRICE", new LocaleValue("期末&单价"), SqlStorageType.SqlDecimal);
                QMPRICE.ColIndex = 16;
                var QMAMOUNT = header.AddChild("QMAMOUNT", new LocaleValue("期末&金额"), SqlStorageType.SqlDecimal);
                QMAMOUNT.ColIndex = 17;
                return header;
            }
        }
        #endregion
        #region 表头设置
        /// <summary>
        /// 设置报表头
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override ReportTitles GetReportTitles(IRptParams filter)
        {
            var result = base.GetReportTitles(filter);
            DynamicObject dyFilter = filter.FilterParameter.CustomFilter;
            if (dyFilter != null)
            {
                if (result == null)
                {
                    result = new ReportTitles();
                }
                DynamicObject startMaterial = dyFilter["F_Ls_StartMaterial"] as DynamicObject;
                DynamicObject endMaterial = dyFilter["F_Ls_EndMaterial"] as DynamicObject;
                String materialNum = "";//物料范围
                if (startMaterial != null)
                {
                    materialNum = startMaterial["number"].ToString();
                    if (endMaterial != null)
                    {
                        materialNum = materialNum + "-" + endMaterial["number"].ToString();
                    }
                }
                result.AddTitle("F_Ls_MaterialNum", materialNum);
                if (dyFilter["F_Ls_StartDate"] != null)
                {

                    result.AddTitle("F_Ls_StartDate", dyFilter["F_Ls_StartDate"].ToString());
                }
                if (dyFilter["F_Ls_EndDate"] != null)
                {
                    result.AddTitle("F_Ls_EndDate", dyFilter["F_Ls_EndDate"].ToString());
                }

                if (dyFilter["F_Ls_Combo"] != null)
                {
                    result.AddTitle("F_Ls_Combo", dyFilter["F_Ls_Combo"].ToString());
                }
            }
            return result;
        }
        #endregion
        #region 实现报表的主方法
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            try
            {
                this.CreateTempTable();//创建临时表，用于数据整理
                DynamicObject dyFilter = filter.FilterParameter.CustomFilter;
                if (Convert.ToInt32(dyFilter["F_Ls_Combo"]) == 3)
                {
                    this.insertDataMaterial(filter);//整理数据插入临时表
                }
                String table = tempTable;
                base.KSQL_SEQ = String.Format(base.KSQL_SEQ, table+".FFINDATE");//排序
                String strSql = String.Format(@"/*dialect*/SELECT FFINDATE,
       FCUSTNO,
       FCUSTNAME,
       FMATERIALNO,
       FMATERIALNAME,
       FSPE,
       UNITID,
       SUM(QCQTY) AS QCQTY,
       CASE
         WHEN SUM(QCQTY) <> 0 THEN
          SUM(QCAMOUNT) / SUM(QCQTY)
         ELSE
          0
       END QCPRICE,
       SUM(QCAMOUNT) AS QCAMOUNT,
       SUM(OUTQTY) AS OUTQTY,
       CASE
         WHEN SUM(OUTQTY) <> 0 THEN
          SUM(OUTAMOUNT) / SUM(OUTQTY)
         ELSE
          0
       END OUTPRICE,
       SUM(OUTAMOUNT) OUTAMOUNT,
       SUM(GJQTY) AS GJQTY,
       CASE
         WHEN SUM(GJQTY) <> 0 THEN
          SUM(GJAMOUNT) / SUM(GJQTY)
         ELSE
          0
       END GJPRICE,
       SUM(GJAMOUNT) AS GJAMOUNT,
       SUM(QCQTY)+SUM(OUTQTY)-SUM(GJQTY) AS QMQTY,
       CASE
         WHEN (SUM(QCQTY)+SUM(OUTQTY)-SUM(GJQTY)) <> 0 THEN
          (SUM(QCAMOUNT)+SUM(OUTAMOUNT)-SUM(GJAMOUNT)) / (SUM(QCQTY)+SUM(OUTQTY)-SUM(GJQTY)) ELSE 0
       END QMPRICE,
       SUM(QCAMOUNT)+SUM(OUTAMOUNT)-SUM(GJAMOUNT) AS QMAMOUNT,
       {0}
INTO {1}
  FROM {2}
 GROUP BY FFINDATE,
          FCUSTNO,
          FCUSTNAME,
          FMATERIALNO,
          FMATERIALNAME,
          FSPE,
          UNITID", KSQL_SEQ, tableName, tempTable);
                DBUtils.Execute(base.Context,strSql);
                
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                this.dropTempTable();
            }
        }
        #endregion
        #region 整理汇总依据为物料的数据插入临时表
        private void insertDataMaterial(IRptParams filter)
        {
            DynamicObject dyFilter = filter.FilterParameter.CustomFilter;
            DynamicObject startMaterial = dyFilter["F_Ls_StartMaterial"] as DynamicObject;
            DynamicObject endMaterial = dyFilter["F_Ls_EndMaterial"] as DynamicObject;
            DynamicObject startCustomer = dyFilter["F_Ls_StartCustomer"] as DynamicObject;
            DynamicObject endCustomer = dyFilter["F_Ls_EndCustomer"] as DynamicObject;
            DateTime startDate = Convert.ToDateTime(dyFilter["F_Ls_StartDate"]);
            String strSql_qc = String.Format(@"/*dialect*/INSERT INTO {0} SELECT QC.FFINDATE FFINDATE,
       QC.FCUSTNO FCUSTNO,
       QC.FCUSTNAME FCUSTNAME,
       QC.FMATERIALNO FMATERIALNO,
       QC.FMATERIALNAME FMATERIALNAME,
       QC.FSPE FSPE,
       QC.UNITID UNITID,
       SUM(QC.QCQTY) QCQTY,
       CASE
         WHEN SUM(QC.QCQTY) <> 0 THEN
          SUM(QC.QCAMOUNT) / SUM(QC.QCQTY)
         ELSE
          0
       END QCPRICE,
       SUM(QC.QCAMOUNT) QCAMOUNT,
       0 OUTQTY, --发出数量
       0 OUTPRICE, --发出单价
       0 OUTAMOUNT, --发出金额
       0 GJQTY, --期末数量
       0 GJPRICE, --期末单价
       0 GJAMOUNT --期末金额
  FROM (SELECT A.FFINDATE AS FFINDATE, --会计期间
               A.FCUSTNO AS FCUSTNO,--客户编号
               A.FCUSTNAME AS FCUSTNAME,--客户名称
               A.FMATERIALNO AS FMATERIALNO, --物料编码
               A.FMATERIALNAME AS FMATERIALNAME, --物料名称
               A.FSPE AS FSPE, --规格型号
               A.UNITID AS UNITID, --计量单位
               SUM(A.QCQTY) AS QCQTY, --期初数量
               SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
          FROM (SELECT distinct CONVERT(VARCHAR(7),
                               CONVERT(DATE,'{1}'),
                               20) AS FFINDATE, --会计期间
                       CUST.FNUMBER AS FCUSTNO,--客户编号
                       CUST_L.FNAME AS FCUSTNAME,--客户名称
                       MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
                       MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
                       MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
                       UNIT_L.FNAME AS UNITID, --计量单位
                       OUTENTRY.FREALQTY AS QCQTY, --期初数量
                       OUTSTOCK.FBILLNO BILLNO,--单据编号
                       OUTENTRY.FENTRYID ENTRYID,--分录ID
                       (OUTENTRY.FREALQTY * OUTENTRY_F.FSALCOSTPRICE) AS QCAMOUNT --期初余额
                  FROM T_SAL_OUTSTOCK OUTSTOCK
                 INNER JOIN T_SAL_OUTSTOCKENTRY OUTENTRY
                    ON OUTSTOCK.FID = OUTENTRY.FID
                 INNER JOIN T_SAL_OUTSTOCKENTRY_F OUTENTRY_F
                    ON OUTENTRY_F.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_SAL_OUTSTOCKENTRY_R OUTENTRY_R
                    ON OUTENTRY_R.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_BD_CUSTOMER CUST
                    ON CUST.FCUSTID  = OUTSTOCK.FCUSTOMERID
                 INNER JOIN T_BD_CUSTOMER_L CUST_L
                    ON CUST_L.FCUSTID = CUST.FCUSTID
                 INNER JOIN T_BD_MATERIAL MATERIAL
                    ON MATERIAL.FMASTERID = OUTENTRY.FMATERIALID
                 INNER JOIN T_BD_MATERIAL_L MATERIAL_L
                    ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_MATERIALBASE MATERIALBASE
                    ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_UNIT UNIT
                    ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
                 INNER JOIN T_BD_UNIT_L UNIT_L
                    ON UNIT_L.FUNITID = UNIT.FUNITID
                 WHERE OUTSTOCK.F_KD_XSFS = 103122
                   AND CONVERT(VARCHAR(7), OUTSTOCK.FDATE, 20) <
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20)
                   AND MATERIAL.FNUMBER >= '{2}'
                   AND MATERIAL.FNUMBER <= '{3}'
                   AND CUST.FNUMBER >= '{4}'
                   AND CUST.FNUMBER <= '{5}') A
         GROUP BY A.FFINDATE,
                  A.FMATERIALNO,
                  A.FMATERIALNAME,
                  A.FSPE,
                  A.UNITID,
                  A.FCUSTNO,
                  A.FCUSTNAME
        UNION
        --期初销售出库
        SELECT A.FFINDATE AS FFINDATE, --会计期间
               A.FCUSTNO AS FCUSTNO,--客户编号
               A.FCUSTNAME AS FCUSTNAME,--客户名称
               A.FMATERIALNO AS FMATERIALNO, --物料编码
               A.FMATERIALNAME AS FMATERIALNAME, --物料名称
               A.FSPE AS FSPE, --规格型号
               A.UNITID AS UNITID, --计量单位
               SUM(A.QCQTY) AS QCQTY, --期初数量
               SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
          FROM (SELECT distinct CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20) AS FFINDATE, --会计期间
                       CUST.FNUMBER AS FCUSTNO,--客户编号
                       CUST_L.FNAME AS FCUSTNAME,--客户名称
                       MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
                       MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
                       MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
                       UNIT_L.FNAME AS UNITID, --计量单位
                       OUTENTRY.FREALQTY AS QCQTY, --期初数量
                       OUTSTOCK.FBILLNO BILLNO,--单据编号
                       OUTENTRY.FENTRYID ENTRYID,--分录ID
                       (OUTENTRY.FREALQTY * OUTENTRY_F.FSALCOSTPRICE) AS QCAMOUNT --期初余额
                  FROM T_SAL_INITOUTSTOCK OUTSTOCK
                 INNER JOIN T_SAL_INITOUTSTOCKENTRY OUTENTRY
                    ON OUTSTOCK.FID = OUTENTRY.FID
                 INNER JOIN T_SAL_INITOUTSTOCKENTRY_F OUTENTRY_F
                    ON OUTENTRY_F.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_SAL_INITOUTSTOCKENTRY_R OUTENTRY_R
                    ON OUTENTRY_R.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_BD_CUSTOMER CUST
                    ON CUST.FCUSTID  = OUTSTOCK.FCUSTOMERID
                 INNER JOIN T_BD_CUSTOMER_L CUST_L
                    ON CUST_L.FCUSTID = CUST.FCUSTID
                 INNER JOIN T_BD_MATERIAL MATERIAL
                    ON MATERIAL.FMASTERID = OUTENTRY.FMATERIALID
                 INNER JOIN T_BD_MATERIAL_L MATERIAL_L
                    ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_MATERIALBASE MATERIALBASE
                    ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_UNIT UNIT
                    ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
                 INNER JOIN T_BD_UNIT_L UNIT_L
                    ON UNIT_L.FUNITID = UNIT.FUNITID
                 WHERE MATERIAL.FNUMBER >= '{2}'
                   AND MATERIAL.FNUMBER <= '{3}'
                   AND CUST.FNUMBER >= '{4}'
                   AND CUST.FNUMBER <= '{5}'
                   AND OUTSTOCK.FBILLTYPEID = '5518f5ceee8053') A
         GROUP BY A.FFINDATE,
                  A.FMATERIALNO,
                  A.FMATERIALNAME,
                  A.FSPE,
                  A.UNITID,
                  A.FCUSTNO,
                  A.FCUSTNAME
        --销售退货
        UNION
        SELECT A.FFINDATE AS FFINDATE, --会计期间
               A.FCUSTNO AS FCUSTNO,--客户编号
               A.FCUSTNAME AS FCUSTNAME,--客户名称
               A.FMATERIALNO AS FMATERIALNO, --物料编码
               A.FMATERIALNAME AS FMATERIALNAME, --物料名称
               A.FSPE AS FSPE, --规格型号
               A.UNITID AS UNITID, --计量单位
               -SUM(A.QCQTY) AS QCQTY, --期初数量
               -SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
          FROM (SELECT distinct CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20) AS FFINDATE, --会计期间
                       CUST.FNUMBER AS FCUSTNO,--客户编号
                       CUST_L.FNAME AS FCUSTNAME,--客户名称
                       MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
                       MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
                       MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
                       UNIT_L.FNAME AS UNITID, --计量单位
                       OUTENTRY.FREALQTY AS QCQTY, --期初数量
                       OUTSTOCK.FBILLNO BILLNO,--单据编号
                       OUTENTRY.FENTRYID ENTRYID,--分录ID
                       (OUTENTRY.FREALQTY * OUTENTRY_F.FSALCOSTPRICE) AS QCAMOUNT --期初余额
                  FROM T_SAL_RETURNSTOCK OUTSTOCK
                 INNER JOIN T_SAL_RETURNSTOCKENTRY OUTENTRY
                    ON OUTSTOCK.FID = OUTENTRY.FID
                 INNER JOIN T_SAL_RETURNSTOCKENTRY_F OUTENTRY_F
                    ON OUTENTRY_F.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_SAL_RETURNSTOCKENTRY_R OUTENTRY_R
                    ON OUTENTRY_R.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_BD_CUSTOMER CUST
                    ON CUST.FCUSTID  = OUTSTOCK.FRETCUSTID
                 INNER JOIN T_BD_CUSTOMER_L CUST_L
                    ON CUST_L.FCUSTID = CUST.FCUSTID
                 INNER JOIN T_BD_MATERIAL MATERIAL
                    ON MATERIAL.FMASTERID = OUTENTRY.FMATERIALID
                 INNER JOIN T_BD_MATERIAL_L MATERIAL_L
                    ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_MATERIALBASE MATERIALBASE
                    ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_UNIT UNIT
                    ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
                 INNER JOIN T_BD_UNIT_L UNIT_L
                    ON UNIT_L.FUNITID = UNIT.FUNITID
                 WHERE OUTSTOCK.F_KD_BASE = 103122
                   AND CONVERT(VARCHAR(7), OUTSTOCK.FDATE, 20) <
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20)
                   AND MATERIAL.FNUMBER >= '{2}'
                   AND MATERIAL.FNUMBER <= '{3}'
                   AND CUST.FNUMBER >= '{4}'
                   AND CUST.FNUMBER <= '{5}') A
         GROUP BY A.FFINDATE,
                  A.FMATERIALNO,
                  A.FMATERIALNAME,
                  A.FSPE,
                  A.UNITID,
                  A.FCUSTNO,
                  A.FCUSTNAME
        UNION
        --期初销售退货
        SELECT A.FFINDATE AS FFINDATE, --会计期间
               A.FCUSTNO AS FCUSTNO,--客户编号
               A.FCUSTNAME AS FCUSTNAME,--客户名称
               A.FMATERIALNO AS FMATERIALNO, --物料编码
               A.FMATERIALNAME AS FMATERIALNAME, --物料名称
               A.FSPE AS FSPE, --规格型号
               A.UNITID AS UNITID, --计量单位
               -SUM(A.QCQTY) AS QCQTY, --期初数量
               -SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
          FROM (SELECT distinct CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20) AS FFINDATE, --会计期间
                       CUST.FNUMBER AS FCUSTNO,--客户编号
                       CUST_L.FNAME AS FCUSTNAME,--客户名称
                       MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
                       MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
                       MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
                       UNIT_L.FNAME AS UNITID, --计量单位
                       OUTENTRY.FREALQTY AS QCQTY, --期初数量
                       OUTSTOCK.FBILLNO BILLNO,--单据编号
                       OUTENTRY.FENTRYID ENTRYID,--分录ID
                       (OUTENTRY.FREALQTY * OUTENTRY_F.FSALCOSTPRICE) AS QCAMOUNT --期初余额
                  FROM T_SAL_INITOUTSTOCK OUTSTOCK
                 INNER JOIN T_SAL_INITOUTSTOCKENTRY OUTENTRY
                    ON OUTSTOCK.FID = OUTENTRY.FID
                 INNER JOIN T_SAL_INITOUTSTOCKENTRY_F OUTENTRY_F
                    ON OUTENTRY_F.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_SAL_INITOUTSTOCKENTRY_R OUTENTRY_R
                    ON OUTENTRY_R.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_BD_CUSTOMER CUST
                    ON CUST.FCUSTID  = OUTSTOCK.FCUSTOMERID
                 INNER JOIN T_BD_CUSTOMER_L CUST_L
                    ON CUST_L.FCUSTID = CUST.FCUSTID
                 INNER JOIN T_BD_MATERIAL MATERIAL
                    ON MATERIAL.FMASTERID = OUTENTRY.FMATERIALID
                 INNER JOIN T_BD_MATERIAL_L MATERIAL_L
                    ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_MATERIALBASE MATERIALBASE
                    ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_UNIT UNIT
                    ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
                 INNER JOIN T_BD_UNIT_L UNIT_L
                    ON UNIT_L.FUNITID = UNIT.FUNITID
                 WHERE MATERIAL.FNUMBER >= '{2}'
                   AND MATERIAL.FNUMBER <= '{3}'
                   AND CUST.FNUMBER >= '{4}'
                   AND CUST.FNUMBER <= '{5}'
                   AND OUTSTOCK.FBILLTYPEID = '5518f60aee8191') A
         GROUP BY A.FFINDATE,
                  A.FMATERIALNO,
                  A.FMATERIALNAME,
                  A.FSPE,
                  A.UNITID,
                  A.FCUSTNO,
                  A.FCUSTNAME
       UNION
       SELECT A.FFINDATE AS FFINDATE, --会计期间
       A.FCUSTNO AS FCUSTNO, --客户编号
       A.FCUSTNAME AS FCUSTNAME, --客户名称
       A.FMATERIALNO AS FMATERIALNO, --物料编码
       A.FMATERIALNAME AS FMATERIALNAME, --物料名称
       A.FSPE AS FSPE, --规格型号
       A.UNITID AS UNITID, --计量单位
       -SUM(A.QCQTY) AS QCQTY, --期初数量
       -SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
  FROM (SELECT distinct CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20) AS FFINDATE, --会计期间
               CUST.FNUMBER AS FCUSTNO, --客户编号
               CUST_L.FNAME AS FCUSTNAME, --客户名称
               MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
               MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
               MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
               UNIT_L.FNAME AS UNITID, --计量单位
               AY.FPRICEQTY AS QCQTY, --期初数量
               AR.FBILLNO BILLNO,--单据编号
               AY.FENTRYID ENTRYID,--分录ID
               AY.FCOSTAMTSUM AS QCAMOUNT --期初余额
          FROM T_AR_RECEIVABLE AS AR
         INNER JOIN T_AR_RECEIVABLEENTRY AS AY
            ON AR.FID = AY.FID
         INNER JOIN T_BD_CUSTOMER CUST
            ON CUST.FCUSTID = AR.FCUSTOMERID
         INNER JOIN T_BD_CUSTOMER_L CUST_L
            ON CUST_L.FCUSTID = CUST.FCUSTID
         INNER JOIN T_BD_MATERIAL MATERIAL
            ON MATERIAL.FMASTERID = AY.FMATERIALID
         INNER JOIN T_BD_MATERIAL_L MATERIAL_L
            ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_MATERIALBASE MATERIALBASE
            ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_UNIT UNIT
            ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
         INNER JOIN T_BD_UNIT_L UNIT_L
            ON UNIT_L.FUNITID = UNIT.FUNITID
         WHERE MATERIAL.FNUMBER >= '{2}'
           AND MATERIAL.FNUMBER <= '{3}'
           AND CUST.FNUMBER >= '{4}'
           AND CUST.FNUMBER <= '{5}'
           AND CONVERT(VARCHAR(7), AR.FDATE, 20) <
               CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20)
           AND AY.FSOURCEBILLNO <> ''
           AND AY.FSOURCEBILLNO IN
               (SELECT INITSTOCK.FBILLNO
                  FROM T_SAL_INITOUTSTOCK INITSTOCK
                 WHERE CONVERT(VARCHAR(7), INITSTOCK.FDATE, 20) <
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{6}'),
                               20)
                   AND INITSTOCK.FBILLTYPEID = '5518f5ceee8053')) A
 GROUP BY A.FFINDATE,
          A.FMATERIALNO,
          A.FMATERIALNAME,
          A.FSPE,
          A.UNITID,
          A.FCUSTNO,
          A.FCUSTNAME
       UNION
       SELECT A.FFINDATE AS FFINDATE, --会计期间
       A.FCUSTNO AS FCUSTNO, --客户编号
       A.FCUSTNAME AS FCUSTNAME, --客户名称
       A.FMATERIALNO AS FMATERIALNO, --物料编码
       A.FMATERIALNAME AS FMATERIALNAME, --物料名称
       A.FSPE AS FSPE, --规格型号
       A.UNITID AS UNITID, --计量单位
       -SUM(A.QCQTY) AS QCQTY, --期初数量
       -SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
  FROM (SELECT distinct CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20) AS FFINDATE, --会计期间
               CUST.FNUMBER AS FCUSTNO, --客户编号
               CUST_L.FNAME AS FCUSTNAME, --客户名称
               MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
               MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
               MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
               UNIT_L.FNAME AS UNITID, --计量单位
               AY.FPRICEQTY AS QCQTY, --期初数量
               AR.FBILLNO BILLNO,--单据编号
               AY.FENTRYID ENTRYID,--分录ID
               AY.FCOSTAMTSUM AS QCAMOUNT --期初余额
          FROM T_AR_RECEIVABLE AS AR
         INNER JOIN T_AR_RECEIVABLEENTRY AS AY
            ON AR.FID = AY.FID
         INNER JOIN T_BD_CUSTOMER CUST
            ON CUST.FCUSTID = AR.FCUSTOMERID
         INNER JOIN T_BD_CUSTOMER_L CUST_L
            ON CUST_L.FCUSTID = CUST.FCUSTID
         INNER JOIN T_BD_MATERIAL MATERIAL
            ON MATERIAL.FMASTERID = AY.FMATERIALID
         INNER JOIN T_BD_MATERIAL_L MATERIAL_L
            ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_MATERIALBASE MATERIALBASE
            ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_UNIT UNIT
            ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
         INNER JOIN T_BD_UNIT_L UNIT_L
            ON UNIT_L.FUNITID = UNIT.FUNITID
         WHERE MATERIAL.FNUMBER >= '{2}'
           AND MATERIAL.FNUMBER <= '{3}'
           AND CUST.FNUMBER >= '{4}'
           AND CUST.FNUMBER <= '{5}'
           AND CONVERT(VARCHAR(7), AR.FDATE, 20) <
               CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20)
           AND AY.FSOURCEBILLNO <> ''
           AND AY.FSOURCEBILLNO IN
               (SELECT INITSTOCK.FBILLNO
                  FROM T_SAL_INITOUTSTOCK INITSTOCK
                 WHERE CONVERT(VARCHAR(7), INITSTOCK.FDATE, 20) <
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{6}'),
                               20)
                   AND INITSTOCK.FBILLTYPEID = '5518f60aee8191')) A
 GROUP BY A.FFINDATE,
          A.FMATERIALNO,
          A.FMATERIALNAME,
          A.FSPE,
          A.UNITID,
          A.FCUSTNO,
          A.FCUSTNAME
        UNION
        SELECT A.FFINDATE AS FFINDATE, --会计期间
       A.FCUSTNO AS FCUSTNO, --客户编号
       A.FCUSTNAME AS FCUSTNAME, --客户名称
       A.FMATERIALNO AS FMATERIALNO, --物料编码
       A.FMATERIALNAME AS FMATERIALNAME, --物料名称
       A.FSPE AS FSPE, --规格型号
       A.UNITID AS UNITID, --计量单位
       -SUM(A.QCQTY) AS QCQTY, --期初数量
       -SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
  FROM (SELECT distinct CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20) AS FFINDATE, --会计期间
               CUST.FNUMBER AS FCUSTNO, --客户编号
               CUST_L.FNAME AS FCUSTNAME, --客户名称
               MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
               MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
               MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
               UNIT_L.FNAME AS UNITID, --计量单位
               AY.FPRICEQTY AS QCQTY, --期初数量
               AR.FBILLNO BILLNO,--单据编号
               AY.FENTRYID ENTRYID,--分录ID
               AY.FCOSTAMTSUM AS QCAMOUNT --期初余额
          FROM T_AR_RECEIVABLE AS AR
         INNER JOIN T_AR_RECEIVABLEENTRY AS AY
            ON AR.FID = AY.FID
         INNER JOIN T_BD_CUSTOMER CUST
            ON CUST.FCUSTID = AR.FCUSTOMERID
         INNER JOIN T_BD_CUSTOMER_L CUST_L
            ON CUST_L.FCUSTID = CUST.FCUSTID
         INNER JOIN T_BD_MATERIAL MATERIAL
            ON MATERIAL.FMASTERID = AY.FMATERIALID
         INNER JOIN T_BD_MATERIAL_L MATERIAL_L
            ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_MATERIALBASE MATERIALBASE
            ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_UNIT UNIT
            ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
         INNER JOIN T_BD_UNIT_L UNIT_L
            ON UNIT_L.FUNITID = UNIT.FUNITID
         WHERE MATERIAL.FNUMBER >= '{2}'
           AND MATERIAL.FNUMBER <= '{3}'
           AND CUST.FNUMBER >= '{4}'
           AND CUST.FNUMBER <= '{5}'
           AND CONVERT(VARCHAR(7), AR.FDATE, 20) <
               CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20)
           AND AY.FSOURCEBILLNO <> ''
           AND AY.FSOURCEBILLNO IN
               (SELECT INITSTOCK.FBILLNO
                  FROM T_SAL_OUTSTOCK INITSTOCK
                 WHERE CONVERT(VARCHAR(7), INITSTOCK.FDATE, 20) <
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{6}'),
                               20)
                 AND INITSTOCK.F_KD_XSFS = 103122)) A
 GROUP BY A.FFINDATE,
          A.FMATERIALNO,
          A.FMATERIALNAME,
          A.FSPE,
          A.UNITID,
          A.FCUSTNO,
          A.FCUSTNAME
        UNION
        SELECT A.FFINDATE AS FFINDATE, --会计期间
       A.FCUSTNO AS FCUSTNO, --客户编号
       A.FCUSTNAME AS FCUSTNAME, --客户名称
       A.FMATERIALNO AS FMATERIALNO, --物料编码
       A.FMATERIALNAME AS FMATERIALNAME, --物料名称
       A.FSPE AS FSPE, --规格型号
       A.UNITID AS UNITID, --计量单位
       -SUM(A.QCQTY) AS QCQTY, --期初数量
       -SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
  FROM (SELECT distinct CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20) AS FFINDATE, --会计期间
               CUST.FNUMBER AS FCUSTNO, --客户编号
               CUST_L.FNAME AS FCUSTNAME, --客户名称
               MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
               MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
               MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
               UNIT_L.FNAME AS UNITID, --计量单位
               AY.FPRICEQTY AS QCQTY, --期初数量
               AR.FBILLNO BILLNO,--单据编号
               AY.FENTRYID ENTRYID,--分录ID
               AY.FCOSTAMTSUM AS QCAMOUNT --期初余额
          FROM T_AR_RECEIVABLE AS AR
         INNER JOIN T_AR_RECEIVABLEENTRY AS AY
            ON AR.FID = AY.FID
         INNER JOIN T_BD_CUSTOMER CUST
            ON CUST.FCUSTID = AR.FCUSTOMERID
         INNER JOIN T_BD_CUSTOMER_L CUST_L
            ON CUST_L.FCUSTID = CUST.FCUSTID
         INNER JOIN T_BD_MATERIAL MATERIAL
            ON MATERIAL.FMASTERID = AY.FMATERIALID
         INNER JOIN T_BD_MATERIAL_L MATERIAL_L
            ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_MATERIALBASE MATERIALBASE
            ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_UNIT UNIT
            ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
         INNER JOIN T_BD_UNIT_L UNIT_L
            ON UNIT_L.FUNITID = UNIT.FUNITID
         WHERE MATERIAL.FNUMBER >= '{2}'
           AND MATERIAL.FNUMBER <= '{3}'
           AND CUST.FNUMBER >= '{4}'
           AND CUST.FNUMBER <= '{5}'
           AND CONVERT(VARCHAR(7), AR.FDATE, 20) <
               CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20)
           AND AY.FSOURCEBILLNO <> ''
           AND AY.FSOURCEBILLNO IN
               (SELECT INITSTOCK.FBILLNO
                  FROM T_SAL_RETURNSTOCK INITSTOCK
                 WHERE CONVERT(VARCHAR(7), INITSTOCK.FDATE, 20) <
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{6}'),
                               20)
                AND INITSTOCK.F_KD_BASE = 103122)) A
 GROUP BY A.FFINDATE,
          A.FMATERIALNO,
          A.FMATERIALNAME,
          A.FSPE,
          A.UNITID,
          A.FCUSTNO,
          A.FCUSTNAME) QC
 GROUP BY QC.FFINDATE, QC.FMATERIALNO, QC.FMATERIALNAME, QC.FSPE, QC.UNITID,QC.FCUSTNO,QC.FCUSTNAME", tempTable, dyFilter["F_Ls_StartDate"].ToString(), startMaterial["number"].ToString(), endMaterial["number"].ToString(), startCustomer["number"].ToString(), endCustomer["number"].ToString(), Convert.ToDateTime(dyFilter["F_Ls_StartDate"]).AddMonths(-1).ToString());
            DBUtils.Execute(base.Context,strSql_qc);
            String strSql_fc = String.Format(@"/*dialect*/INSERT INTO {0} SELECT QC.FFINDATE FFINDATE,
       QC.FCUSTNO FCUSTNO,
       QC.FCUSTNAME FCUSTNAME,
       QC.FMATERIALNO FMATERIALNO,
       QC.FMATERIALNAME FMATERIALNAME,
       QC.FSPE FSPE,
       QC.UNITID UNITID,
       0 QCQTY,
       0 QCPRICE,
       0 QCAMOUNT,
       SUM(QC.QCQTY) OUTQTY, --发出数量
       CASE
         WHEN SUM(QC.QCQTY) <> 0 THEN
          SUM(QC.QCAMOUNT) / SUM(QC.QCQTY)
         ELSE
          0
       END OUTPRICE, --发出单价
       SUM(QC.QCAMOUNT) OUTAMOUNT, --发出金额
       0 GJQTY, --期末数量
       0 GJPRICE, --期末单价
       0 GJAMOUNT --期末金额
  FROM (SELECT A.FFINDATE AS FFINDATE, --会计期间
               A.FCUSTNO AS FCUSTNO,--客户编号
               A.FCUSTNAME AS FCUSTNAME,--客户名称
               A.FMATERIALNO AS FMATERIALNO, --物料编码
               A.FMATERIALNAME AS FMATERIALNAME, --物料名称
               A.FSPE AS FSPE, --规格型号
               A.UNITID AS UNITID, --计量单位
               SUM(A.QCQTY) AS QCQTY, --期初数量
               SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
          FROM (SELECT distinct CONVERT(VARCHAR(7),
                               CONVERT(DATE,'{1}'),
                               20) AS FFINDATE, --会计期间
                       CUST.FNUMBER AS FCUSTNO,--客户编号
                       CUST_L.FNAME AS FCUSTNAME,--客户名称
                       MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
                       MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
                       MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
                       UNIT_L.FNAME AS UNITID, --计量单位
                       OUTENTRY.FREALQTY AS QCQTY, --期初数量
                       OUTSTOCK.FBILLNO BILLNO,--单据编号
                       OUTENTRY.FENTRYID ENTRYID,--分录ID
                       (OUTENTRY.FREALQTY * OUTENTRY_F.FSALCOSTPRICE) AS QCAMOUNT --期初余额
                  FROM T_SAL_OUTSTOCK OUTSTOCK
                 INNER JOIN T_SAL_OUTSTOCKENTRY OUTENTRY
                    ON OUTSTOCK.FID = OUTENTRY.FID
                 INNER JOIN T_SAL_OUTSTOCKENTRY_F OUTENTRY_F
                    ON OUTENTRY_F.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_SAL_OUTSTOCKENTRY_R OUTENTRY_R
                    ON OUTENTRY_R.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_BD_CUSTOMER CUST
                    ON CUST.FCUSTID  = OUTSTOCK.FCUSTOMERID
                 INNER JOIN T_BD_CUSTOMER_L CUST_L
                    ON CUST_L.FCUSTID = CUST.FCUSTID
                 INNER JOIN T_BD_MATERIAL MATERIAL
                    ON MATERIAL.FMASTERID = OUTENTRY.FMATERIALID
                 INNER JOIN T_BD_MATERIAL_L MATERIAL_L
                    ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_MATERIALBASE MATERIALBASE
                    ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_UNIT UNIT
                    ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
                 INNER JOIN T_BD_UNIT_L UNIT_L
                    ON UNIT_L.FUNITID = UNIT.FUNITID
                 WHERE OUTSTOCK.F_KD_XSFS = 103122
                   AND CONVERT(VARCHAR(7), OUTSTOCK.FDATE, 20) =
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20)
                   AND MATERIAL.FNUMBER >= '{2}'
                   AND MATERIAL.FNUMBER <= '{3}'
                   AND CUST.FNUMBER >= '{4}'
                   AND CUST.FNUMBER <= '{5}') A
         GROUP BY A.FFINDATE,
                  A.FMATERIALNO,
                  A.FMATERIALNAME,
                  A.FSPE,
                  A.UNITID,
                  A.FCUSTNO,
                  A.FCUSTNAME
        --销售退货
        UNION
        SELECT A.FFINDATE AS FFINDATE, --会计期间
               A.FCUSTNO AS FCUSTNO,--客户编号
               A.FCUSTNAME AS FCUSTNAME,--客户名称
               A.FMATERIALNO AS FMATERIALNO, --物料编码
               A.FMATERIALNAME AS FMATERIALNAME, --物料名称
               A.FSPE AS FSPE, --规格型号
               A.UNITID AS UNITID, --计量单位
               -SUM(A.QCQTY) AS QCQTY, --期初数量
               -SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
          FROM (SELECT distinct CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20) AS FFINDATE, --会计期间
                       CUST.FNUMBER AS FCUSTNO,--客户编号
                       CUST_L.FNAME AS FCUSTNAME,--客户名称
                       MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
                       MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
                       MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
                       UNIT_L.FNAME AS UNITID, --计量单位
                       OUTENTRY.FREALQTY - OUTENTRY_R.FINVOICEDQTY AS QCQTY, --期初数量
                       OUTSTOCK.FBILLNO BILLNO,--单据编号
                       OUTENTRY.FENTRYID ENTRYID,--分录ID
                       (OUTENTRY.FREALQTY * OUTENTRY_F.FSALCOSTPRICE) AS QCAMOUNT --期初余额
                  FROM T_SAL_RETURNSTOCK OUTSTOCK
                 INNER JOIN T_SAL_RETURNSTOCKENTRY OUTENTRY
                    ON OUTSTOCK.FID = OUTENTRY.FID
                 INNER JOIN T_SAL_RETURNSTOCKENTRY_F OUTENTRY_F
                    ON OUTENTRY_F.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_SAL_RETURNSTOCKENTRY_R OUTENTRY_R
                    ON OUTENTRY_R.FENTRYID = OUTENTRY.FENTRYID
                 INNER JOIN T_BD_CUSTOMER CUST
                    ON CUST.FCUSTID  = OUTSTOCK.FRETCUSTID
                 INNER JOIN T_BD_CUSTOMER_L CUST_L
                    ON CUST_L.FCUSTID = CUST.FCUSTID
                 INNER JOIN T_BD_MATERIAL MATERIAL
                    ON MATERIAL.FMASTERID = OUTENTRY.FMATERIALID
                 INNER JOIN T_BD_MATERIAL_L MATERIAL_L
                    ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_MATERIALBASE MATERIALBASE
                    ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
                 INNER JOIN T_BD_UNIT UNIT
                    ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
                 INNER JOIN T_BD_UNIT_L UNIT_L
                    ON UNIT_L.FUNITID = UNIT.FUNITID
                 WHERE OUTSTOCK.F_KD_BASE = 103122
                   AND CONVERT(VARCHAR(7), OUTSTOCK.FDATE, 20) =
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20)
                   AND MATERIAL.FNUMBER >= '{2}'
                   AND MATERIAL.FNUMBER <= '{3}'
                   AND CUST.FNUMBER >= '{4}'
                   AND CUST.FNUMBER <= '{5}') A
         GROUP BY A.FFINDATE,
                  A.FMATERIALNO,
                  A.FMATERIALNAME,
                  A.FSPE,
                  A.UNITID,
                  A.FCUSTNO,
                  A.FCUSTNAME) QC
 GROUP BY QC.FFINDATE, QC.FMATERIALNO, QC.FMATERIALNAME, QC.FSPE, QC.UNITID,QC.FCUSTNO,QC.FCUSTNAME", tempTable, dyFilter["F_Ls_StartDate"].ToString(), startMaterial["number"].ToString(), endMaterial["number"].ToString(), startCustomer["number"].ToString(), endCustomer["number"].ToString());
            DBUtils.Execute(base.Context,strSql_fc);
            String strSql_gj = String.Format(@"/*dialect*/INSERT INTO {0} SELECT QC.FFINDATE FFINDATE,
       QC.FCUSTNO FCUSTNO,
       QC.FCUSTNAME FCUSTNAME,
       QC.FMATERIALNO FMATERIALNO,
       QC.FMATERIALNAME FMATERIALNAME,
       QC.FSPE FSPE,
       QC.UNITID UNITID,
       0 QCQTY,
       0 QCPRICE,
       0 QCAMOUNT,
       0 OUTQTY, --发出数量
       0 OUTPRICE, --发出单价
       0 OUTAMOUNT, --发出金额
       SUM(QC.QCQTY) GJQTY, --期末数量
       CASE
         WHEN SUM(QC.QCQTY) <> 0 THEN
          SUM(QC.QCAMOUNT) / SUM(QC.QCQTY)
         ELSE
          0
       END GJPRICE, --期末单价
       SUM(QC.QCAMOUNT) GJAMOUNT --期末金额
  FROM (
       SELECT A.FFINDATE AS FFINDATE, --会计期间
       A.FCUSTNO AS FCUSTNO, --客户编号
       A.FCUSTNAME AS FCUSTNAME, --客户名称
       A.FMATERIALNO AS FMATERIALNO, --物料编码
       A.FMATERIALNAME AS FMATERIALNAME, --物料名称
       A.FSPE AS FSPE, --规格型号
       A.UNITID AS UNITID, --计量单位
       SUM(A.QCQTY) AS QCQTY, --期初数量
       SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
  FROM (SELECT distinct CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20) AS FFINDATE, --会计期间
               CUST.FNUMBER AS FCUSTNO, --客户编号
               CUST_L.FNAME AS FCUSTNAME, --客户名称
               MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
               MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
               MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
               UNIT_L.FNAME AS UNITID, --计量单位
               AY.FPRICEQTY AS QCQTY, --期初数量
               AR.FBILLNO BILLNO,--单据编号
               AY.FENTRYID ENTRYID,--分录ID
               AY.FCOSTAMTSUM AS QCAMOUNT --期初余额
          FROM T_AR_RECEIVABLE AS AR
         INNER JOIN T_AR_RECEIVABLEENTRY AS AY
            ON AR.FID = AY.FID
         INNER JOIN T_BD_CUSTOMER CUST
            ON CUST.FCUSTID = AR.FCUSTOMERID
         INNER JOIN T_BD_CUSTOMER_L CUST_L
            ON CUST_L.FCUSTID = CUST.FCUSTID
         INNER JOIN T_BD_MATERIAL MATERIAL
            ON MATERIAL.FMASTERID = AY.FMATERIALID
         INNER JOIN T_BD_MATERIAL_L MATERIAL_L
            ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_MATERIALBASE MATERIALBASE
            ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_UNIT UNIT
            ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
         INNER JOIN T_BD_UNIT_L UNIT_L
            ON UNIT_L.FUNITID = UNIT.FUNITID
         WHERE MATERIAL.FNUMBER >= '{2}'
           AND MATERIAL.FNUMBER <= '{3}'
           AND CUST.FNUMBER >= '{4}'
           AND CUST.FNUMBER <= '{5}'
           AND CONVERT(VARCHAR(7), AR.FDATE, 20) =
               CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20)
           AND AY.FSOURCEBILLNO <> ''
           AND AY.FSOURCEBILLNO IN
               (SELECT INITSTOCK.FBILLNO
                  FROM T_SAL_INITOUTSTOCK INITSTOCK
                 WHERE CONVERT(VARCHAR(7), INITSTOCK.FDATE, 20) <
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20)
                   AND INITSTOCK.FBILLTYPEID = '5518f5ceee8053')) A
 GROUP BY A.FFINDATE,
          A.FMATERIALNO,
          A.FMATERIALNAME,
          A.FSPE,
          A.UNITID,
          A.FCUSTNO,
          A.FCUSTNAME
       UNION
       SELECT A.FFINDATE AS FFINDATE, --会计期间
       A.FCUSTNO AS FCUSTNO, --客户编号
       A.FCUSTNAME AS FCUSTNAME, --客户名称
       A.FMATERIALNO AS FMATERIALNO, --物料编码
       A.FMATERIALNAME AS FMATERIALNAME, --物料名称
       A.FSPE AS FSPE, --规格型号
       A.UNITID AS UNITID, --计量单位
       SUM(A.QCQTY) AS QCQTY, --期初数量
       SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
  FROM (SELECT distinct CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20) AS FFINDATE, --会计期间
               CUST.FNUMBER AS FCUSTNO, --客户编号
               CUST_L.FNAME AS FCUSTNAME, --客户名称
               MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
               MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
               MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
               UNIT_L.FNAME AS UNITID, --计量单位
               AY.FPRICEQTY AS QCQTY, --期初数量
               AR.FBILLNO BILLNO,--单据编号
               AY.FENTRYID ENTRYID,--分录ID
               AY.FCOSTAMTSUM AS QCAMOUNT --期初余额
          FROM T_AR_RECEIVABLE AS AR
         INNER JOIN T_AR_RECEIVABLEENTRY AS AY
            ON AR.FID = AY.FID
         INNER JOIN T_BD_CUSTOMER CUST
            ON CUST.FCUSTID = AR.FCUSTOMERID
         INNER JOIN T_BD_CUSTOMER_L CUST_L
            ON CUST_L.FCUSTID = CUST.FCUSTID
         INNER JOIN T_BD_MATERIAL MATERIAL
            ON MATERIAL.FMASTERID = AY.FMATERIALID
         INNER JOIN T_BD_MATERIAL_L MATERIAL_L
            ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_MATERIALBASE MATERIALBASE
            ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_UNIT UNIT
            ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
         INNER JOIN T_BD_UNIT_L UNIT_L
            ON UNIT_L.FUNITID = UNIT.FUNITID
         WHERE MATERIAL.FNUMBER >= '{2}'
           AND MATERIAL.FNUMBER <= '{3}'
           AND CUST.FNUMBER >= '{4}'
           AND CUST.FNUMBER <= '{5}'
           AND CONVERT(VARCHAR(7), AR.FDATE, 20) =
               CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20)
           AND AY.FSOURCEBILLNO <> ''
           AND AY.FSOURCEBILLNO IN
               (SELECT INITSTOCK.FBILLNO
                  FROM T_SAL_INITOUTSTOCK INITSTOCK
                 WHERE CONVERT(VARCHAR(7), INITSTOCK.FDATE, 20) <
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20)
                   AND INITSTOCK.FBILLTYPEID = '5518f60aee8191')) A
 GROUP BY A.FFINDATE,
          A.FMATERIALNO,
          A.FMATERIALNAME,
          A.FSPE,
          A.UNITID,
          A.FCUSTNO,
          A.FCUSTNAME
        UNION
        SELECT A.FFINDATE AS FFINDATE, --会计期间
       A.FCUSTNO AS FCUSTNO, --客户编号
       A.FCUSTNAME AS FCUSTNAME, --客户名称
       A.FMATERIALNO AS FMATERIALNO, --物料编码
       A.FMATERIALNAME AS FMATERIALNAME, --物料名称
       A.FSPE AS FSPE, --规格型号
       A.UNITID AS UNITID, --计量单位
       SUM(A.QCQTY) AS QCQTY, --期初数量
       SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
  FROM (SELECT distinct CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20) AS FFINDATE, --会计期间
               CUST.FNUMBER AS FCUSTNO, --客户编号
               CUST_L.FNAME AS FCUSTNAME, --客户名称
               MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
               MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
               MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
               UNIT_L.FNAME AS UNITID, --计量单位
               AY.FPRICEQTY AS QCQTY, --期初数量
               AR.FBILLNO BILLNO,--单据编号
               AY.FENTRYID ENTRYID,--分录ID
               AY.FCOSTAMTSUM AS QCAMOUNT --期初余额
          FROM T_AR_RECEIVABLE AS AR
         INNER JOIN T_AR_RECEIVABLEENTRY AS AY
            ON AR.FID = AY.FID
         INNER JOIN T_BD_CUSTOMER CUST
            ON CUST.FCUSTID = AR.FCUSTOMERID
         INNER JOIN T_BD_CUSTOMER_L CUST_L
            ON CUST_L.FCUSTID = CUST.FCUSTID
         INNER JOIN T_BD_MATERIAL MATERIAL
            ON MATERIAL.FMASTERID = AY.FMATERIALID
         INNER JOIN T_BD_MATERIAL_L MATERIAL_L
            ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_MATERIALBASE MATERIALBASE
            ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_UNIT UNIT
            ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
         INNER JOIN T_BD_UNIT_L UNIT_L
            ON UNIT_L.FUNITID = UNIT.FUNITID
         WHERE MATERIAL.FNUMBER >= '{2}'
           AND MATERIAL.FNUMBER <= '{3}'
           AND CUST.FNUMBER >= '{4}'
           AND CUST.FNUMBER <= '{5}'
           AND CONVERT(VARCHAR(7), AR.FDATE, 20) =
               CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20)
           AND AY.FSOURCEBILLNO <> ''
           AND AY.FSOURCEBILLNO IN
               (SELECT INITSTOCK.FBILLNO
                  FROM T_SAL_OUTSTOCK INITSTOCK
                 WHERE CONVERT(VARCHAR(7), INITSTOCK.FDATE, 20) <
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20)
                AND INITSTOCK.F_KD_XSFS = 103122)) A
 GROUP BY A.FFINDATE,
          A.FMATERIALNO,
          A.FMATERIALNAME,
          A.FSPE,
          A.UNITID,
          A.FCUSTNO,
          A.FCUSTNAME
        UNION
        SELECT A.FFINDATE AS FFINDATE, --会计期间
       A.FCUSTNO AS FCUSTNO, --客户编号
       A.FCUSTNAME AS FCUSTNAME, --客户名称
       A.FMATERIALNO AS FMATERIALNO, --物料编码
       A.FMATERIALNAME AS FMATERIALNAME, --物料名称
       A.FSPE AS FSPE, --规格型号
       A.UNITID AS UNITID, --计量单位
       SUM(A.QCQTY) AS QCQTY, --期初数量
       SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
  FROM (SELECT distinct CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20) AS FFINDATE, --会计期间
               CUST.FNUMBER AS FCUSTNO, --客户编号
               CUST_L.FNAME AS FCUSTNAME, --客户名称
               MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
               MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
               MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
               UNIT_L.FNAME AS UNITID, --计量单位
               AY.FPRICEQTY AS QCQTY, --期初数量
               AR.FBILLNO BILLNO,--单据编号
               AY.FENTRYID ENTRYID,--分录ID
               AY.FCOSTAMTSUM AS QCAMOUNT --期初余额
          FROM T_AR_RECEIVABLE AS AR
         INNER JOIN T_AR_RECEIVABLEENTRY AS AY
            ON AR.FID = AY.FID
         INNER JOIN T_BD_CUSTOMER CUST
            ON CUST.FCUSTID = AR.FCUSTOMERID
         INNER JOIN T_BD_CUSTOMER_L CUST_L
            ON CUST_L.FCUSTID = CUST.FCUSTID
         INNER JOIN T_BD_MATERIAL MATERIAL
            ON MATERIAL.FMASTERID = AY.FMATERIALID
         INNER JOIN T_BD_MATERIAL_L MATERIAL_L
            ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_MATERIALBASE MATERIALBASE
            ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
         INNER JOIN T_BD_UNIT UNIT
            ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
         INNER JOIN T_BD_UNIT_L UNIT_L
            ON UNIT_L.FUNITID = UNIT.FUNITID
         WHERE MATERIAL.FNUMBER >= '{2}'
           AND MATERIAL.FNUMBER <= '{3}'
           AND CUST.FNUMBER >= '{4}'
           AND CUST.FNUMBER <= '{5}'
           AND CONVERT(VARCHAR(7), AR.FDATE, 20) =
               CONVERT(VARCHAR(7), CONVERT(DATE, '{1}'), 20)
           AND AY.FSOURCEBILLNO <> ''
           AND AY.FSOURCEBILLNO IN
               (SELECT INITSTOCK.FBILLNO
                  FROM T_SAL_RETURNSTOCK INITSTOCK
                 WHERE CONVERT(VARCHAR(7), INITSTOCK.FDATE, 20) <
                       CONVERT(VARCHAR(7),
                               CONVERT(DATE, '{1}'),
                               20)
                AND INITSTOCK.F_KD_BASE = 103122)) A
 GROUP BY A.FFINDATE,
          A.FMATERIALNO,
          A.FMATERIALNAME,
          A.FSPE,
          A.UNITID,
          A.FCUSTNO,
          A.FCUSTNAME) QC
 GROUP BY QC.FFINDATE, QC.FMATERIALNO, QC.FMATERIALNAME, QC.FSPE, QC.UNITID,QC.FCUSTNO,QC.FCUSTNAME", tempTable, dyFilter["F_Ls_StartDate"].ToString(), startMaterial["number"].ToString(), endMaterial["number"].ToString(), startCustomer["number"].ToString(), endCustomer["number"].ToString());
//            String strSql_qm = String.Format(@"/*dialect*/INSERT INTO {0} SELECT QC.FFINDATE FFINDATE,
//       QC.FCUSTNO FCUSTNO,
//       QC.FCUSTNAME FCUSTNAME,
//       QC.FMATERIALNO FMATERIALNO,
//       QC.FMATERIALNAME FMATERIALNAME,
//       QC.FSPE FSPE,
//       QC.UNITID UNITID,
//       0 QCQTY,
//       0 QCPRICE,
//       0 QCAMOUNT,
//       0 OUTQTY, --发出数量
//       0 OUTPRICE, --发出单价
//       0 OUTAMOUNT, --发出金额
//       SUM(QC.QCQTY) QMQTY, --期末数量
//       CASE
//         WHEN SUM(QC.QCAMOUNT) <> 0 THEN
//          SUM(QC.QCAMOUNT) / SUM(QC.QCQTY)
//         ELSE
//          0
//       END QMPRICE, --期末单价
//       SUM(QC.QCAMOUNT) QMAMOUNT --期末金额
//  FROM (SELECT A.FFINDATE AS FFINDATE, --会计期间
//               A.FCUSTNO AS FCUSTNO,--客户编号
//               A.FCUSTNAME AS FCUSTNAME,--客户名称
//               A.FMATERIALNO AS FMATERIALNO, --物料编码
//               A.FMATERIALNAME AS FMATERIALNAME, --物料名称
//               A.FSPE AS FSPE, --规格型号
//               A.UNITID AS UNITID, --计量单位
//               SUM(A.QCQTY) AS QCQTY, --期初数量
//               SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
//          FROM (SELECT CONVERT(VARCHAR(7),
//                               CONVERT(DATE,'{1}'),
//                               20) AS FFINDATE, --会计期间
//                       CUST.FNUMBER AS FCUSTNO,--客户编号
//                       CUST_L.FNAME AS FCUSTNAME,--客户名称
//                       MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
//                       MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
//                       MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
//                       UNIT_L.FNAME AS UNITID, --计量单位
//                       OUTENTRY.FREALQTY - OUTENTRY_R.FARJOINQTY AS QCQTY, --期初数量
//                       ((OUTENTRY.FREALQTY - OUTENTRY_R.FARJOINQTY) *
//                       OUTENTRY_F.FSALCOSTPRICE) AS QCAMOUNT --期初余额
//                  FROM T_SAL_OUTSTOCK OUTSTOCK
//                 INNER JOIN T_SAL_OUTSTOCKENTRY OUTENTRY
//                    ON OUTSTOCK.FID = OUTENTRY.FID
//                 INNER JOIN T_SAL_OUTSTOCKENTRY_F OUTENTRY_F
//                    ON OUTENTRY_F.FENTRYID = OUTENTRY.FENTRYID
//                 INNER JOIN T_SAL_OUTSTOCKENTRY_R OUTENTRY_R
//                    ON OUTENTRY_R.FENTRYID = OUTENTRY.FENTRYID
//                 INNER JOIN T_BD_CUSTOMER CUST
//                    ON CUST.FCUSTID  = OUTSTOCK.FCUSTOMERID
//                 INNER JOIN T_BD_CUSTOMER_L CUST_L
//                    ON CUST_L.FCUSTID = CUST.FCUSTID
//                 INNER JOIN T_BD_MATERIAL MATERIAL
//                    ON MATERIAL.FMASTERID = OUTENTRY.FMATERIALID
//                 INNER JOIN T_BD_MATERIAL_L MATERIAL_L
//                    ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
//                 INNER JOIN T_BD_MATERIALBASE MATERIALBASE
//                    ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
//                 INNER JOIN T_BD_UNIT UNIT
//                    ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
//                 INNER JOIN T_BD_UNIT_L UNIT_L
//                    ON UNIT_L.FUNITID = UNIT.FUNITID
//                 WHERE OUTSTOCK.F_KD_XSFS = 103122
//                   AND CONVERT(VARCHAR(7), OUTSTOCK.FDATE, 20) <=
//                       CONVERT(VARCHAR(7),
//                               CONVERT(DATE, '{1}'),
//                               20)
//                   AND MATERIAL.FNUMBER >= '{2}'
//                   AND MATERIAL.FNUMBER <= '{3}'
//                   AND CUST.FNUMBER >= '{4}'
//                   AND CUST.FNUMBER <= '{5}') A
//         GROUP BY A.FFINDATE,
//                  A.FMATERIALNO,
//                  A.FMATERIALNAME,
//                  A.FSPE,
//                  A.UNITID,
//                  A.FCUSTNO,
//                  A.FCUSTNAME
//        UNION
//        --期初销售出库
//        SELECT A.FFINDATE AS FFINDATE, --会计期间
//               A.FCUSTNO AS FCUSTNO,--客户编号
//               A.FCUSTNAME AS FCUSTNAME,--客户名称
//               A.FMATERIALNO AS FMATERIALNO, --物料编码
//               A.FMATERIALNAME AS FMATERIALNAME, --物料名称
//               A.FSPE AS FSPE, --规格型号
//               A.UNITID AS UNITID, --计量单位
//               SUM(A.QCQTY) AS QCQTY, --期初数量
//               SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
//          FROM (SELECT CONVERT(VARCHAR(7),
//                               CONVERT(DATE, '{1}'),
//                               20) AS FFINDATE, --会计期间
//                       CUST.FNUMBER AS FCUSTNO,--客户编号
//                       CUST_L.FNAME AS FCUSTNAME,--客户名称
//                       MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
//                       MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
//                       MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
//                       UNIT_L.FNAME AS UNITID, --计量单位
//                       OUTENTRY.FREALQTY AS QCQTY, --期初数量
//                       (OUTENTRY.FREALQTY * OUTENTRY_F.FSALCOSTPRICE) AS QCAMOUNT --期初余额
//                  FROM T_SAL_INITOUTSTOCK OUTSTOCK
//                 INNER JOIN T_SAL_INITOUTSTOCKENTRY OUTENTRY
//                    ON OUTSTOCK.FID = OUTENTRY.FID
//                 INNER JOIN T_SAL_INITOUTSTOCKENTRY_F OUTENTRY_F
//                    ON OUTENTRY_F.FENTRYID = OUTENTRY.FENTRYID
//                 INNER JOIN T_SAL_INITOUTSTOCKENTRY_R OUTENTRY_R
//                    ON OUTENTRY_R.FENTRYID = OUTENTRY.FENTRYID
//                 INNER JOIN T_BD_CUSTOMER CUST
//                    ON CUST.FCUSTID  = OUTSTOCK.FCUSTOMERID
//                 INNER JOIN T_BD_CUSTOMER_L CUST_L
//                    ON CUST_L.FCUSTID = CUST.FCUSTID
//                 INNER JOIN T_BD_MATERIAL MATERIAL
//                    ON MATERIAL.FMASTERID = OUTENTRY.FMATERIALID
//                 INNER JOIN T_BD_MATERIAL_L MATERIAL_L
//                    ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
//                 INNER JOIN T_BD_MATERIALBASE MATERIALBASE
//                    ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
//                 INNER JOIN T_BD_UNIT UNIT
//                    ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
//                 INNER JOIN T_BD_UNIT_L UNIT_L
//                    ON UNIT_L.FUNITID = UNIT.FUNITID
//                 WHERE MATERIAL.FNUMBER >= '{2}'
//                   AND MATERIAL.FNUMBER <= '{3}'
//                   AND CUST.FNUMBER >= '{4}'
//                   AND CUST.FNUMBER <= '{5}'
//                   AND OUTSTOCK.FBILLTYPEID = '5518f5ceee8053') A
//         GROUP BY A.FFINDATE,
//                  A.FMATERIALNO,
//                  A.FMATERIALNAME,
//                  A.FSPE,
//                  A.UNITID,
//                  A.FCUSTNO,
//                  A.FCUSTNAME
//        --销售退货
//        UNION
//        SELECT A.FFINDATE AS FFINDATE, --会计期间
//               A.FCUSTNO AS FCUSTNO,--客户编号
//               A.FCUSTNAME AS FCUSTNAME,--客户名称
//               A.FMATERIALNO AS FMATERIALNO, --物料编码
//               A.FMATERIALNAME AS FMATERIALNAME, --物料名称
//               A.FSPE AS FSPE, --规格型号
//               A.UNITID AS UNITID, --计量单位
//               -SUM(A.QCQTY) AS QCQTY, --期初数量
//               -SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
//          FROM (SELECT CONVERT(VARCHAR(7),
//                               CONVERT(DATE, '{1}'),
//                               20) AS FFINDATE, --会计期间
//                       CUST.FNUMBER AS FCUSTNO,--客户编号
//                       CUST_L.FNAME AS FCUSTNAME,--客户名称
//                       MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
//                       MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
//                       MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
//                       UNIT_L.FNAME AS UNITID, --计量单位
//                       OUTENTRY.FREALQTY - OUTENTRY_R.FINVOICEDQTY AS QCQTY, --期初数量
//                       ((OUTENTRY.FREALQTY - OUTENTRY_R.FINVOICEDQTY) *
//                       OUTENTRY_F.FSALCOSTPRICE) AS QCAMOUNT --期初余额
//                  FROM T_SAL_RETURNSTOCK OUTSTOCK
//                 INNER JOIN T_SAL_RETURNSTOCKENTRY OUTENTRY
//                    ON OUTSTOCK.FID = OUTENTRY.FID
//                 INNER JOIN T_SAL_RETURNSTOCKENTRY_F OUTENTRY_F
//                    ON OUTENTRY_F.FENTRYID = OUTENTRY.FENTRYID
//                 INNER JOIN T_SAL_RETURNSTOCKENTRY_R OUTENTRY_R
//                    ON OUTENTRY_R.FENTRYID = OUTENTRY.FENTRYID
//                 INNER JOIN T_BD_CUSTOMER CUST
//                    ON CUST.FCUSTID  = OUTSTOCK.FRETCUSTID
//                 INNER JOIN T_BD_CUSTOMER_L CUST_L
//                    ON CUST_L.FCUSTID = CUST.FCUSTID
//                 INNER JOIN T_BD_MATERIAL MATERIAL
//                    ON MATERIAL.FMASTERID = OUTENTRY.FMATERIALID
//                 INNER JOIN T_BD_MATERIAL_L MATERIAL_L
//                    ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
//                 INNER JOIN T_BD_MATERIALBASE MATERIALBASE
//                    ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
//                 INNER JOIN T_BD_UNIT UNIT
//                    ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
//                 INNER JOIN T_BD_UNIT_L UNIT_L
//                    ON UNIT_L.FUNITID = UNIT.FUNITID
//                 WHERE OUTSTOCK.F_KD_BASE = 103122
//                   AND CONVERT(VARCHAR(7), OUTSTOCK.FDATE, 20) <=
//                       CONVERT(VARCHAR(7),
//                               CONVERT(DATE, '{1}'),
//                               20)
//                   AND MATERIAL.FNUMBER >= '{2}'
//                   AND MATERIAL.FNUMBER <= '{3}'
//                   AND CUST.FNUMBER >= '{4}'
//                   AND CUST.FNUMBER <= '{5}') A
//         GROUP BY A.FFINDATE,
//                  A.FMATERIALNO,
//                  A.FMATERIALNAME,
//                  A.FSPE,
//                  A.UNITID,
//                  A.FCUSTNO,
//                  A.FCUSTNAME
//        UNION
//        --期初销售退货
//        SELECT A.FFINDATE AS FFINDATE, --会计期间
//               A.FCUSTNO AS FCUSTNO,--客户编号
//               A.FCUSTNAME AS FCUSTNAME,--客户名称
//               A.FMATERIALNO AS FMATERIALNO, --物料编码
//               A.FMATERIALNAME AS FMATERIALNAME, --物料名称
//               A.FSPE AS FSPE, --规格型号
//               A.UNITID AS UNITID, --计量单位
//               -SUM(A.QCQTY) AS QCQTY, --期初数量
//               -SUM(A.QCAMOUNT) AS QCAMOUNT --期初金额
//          FROM (SELECT CONVERT(VARCHAR(7),
//                               CONVERT(DATE, '{1}'),
//                               20) AS FFINDATE, --会计期间
//                       CUST.FNUMBER AS FCUSTNO,--客户编号
//                       CUST_L.FNAME AS FCUSTNAME,--客户名称
//                       MATERIAL.FNUMBER AS FMATERIALNO, --物料编码
//                       MATERIAL_L.FNAME AS FMATERIALNAME, --物料名称
//                       MATERIAL_L.FSPECIFICATION AS FSPE, --规格型号
//                       UNIT_L.FNAME AS UNITID, --计量单位
//                       OUTENTRY.FREALQTY AS QCQTY, --期初数量
//                       (OUTENTRY.FREALQTY * OUTENTRY_F.FSALCOSTPRICE) AS QCAMOUNT --期初余额
//                  FROM T_SAL_INITOUTSTOCK OUTSTOCK
//                 INNER JOIN T_SAL_INITOUTSTOCKENTRY OUTENTRY
//                    ON OUTSTOCK.FID = OUTENTRY.FID
//                 INNER JOIN T_SAL_INITOUTSTOCKENTRY_F OUTENTRY_F
//                    ON OUTENTRY_F.FENTRYID = OUTENTRY.FENTRYID
//                 INNER JOIN T_SAL_INITOUTSTOCKENTRY_R OUTENTRY_R
//                    ON OUTENTRY_R.FENTRYID = OUTENTRY.FENTRYID
//                 INNER JOIN T_BD_CUSTOMER CUST
//                    ON CUST.FCUSTID  = OUTSTOCK.FCUSTOMERID
//                 INNER JOIN T_BD_CUSTOMER_L CUST_L
//                    ON CUST_L.FCUSTID = CUST.FCUSTID
//                 INNER JOIN T_BD_MATERIAL MATERIAL
//                    ON MATERIAL.FMASTERID = OUTENTRY.FMATERIALID
//                 INNER JOIN T_BD_MATERIAL_L MATERIAL_L
//                    ON MATERIAL_L.FMATERIALID = MATERIAL.FMATERIALID
//                 INNER JOIN T_BD_MATERIALBASE MATERIALBASE
//                    ON MATERIALBASE.FMATERIALID = MATERIAL.FMATERIALID
//                 INNER JOIN T_BD_UNIT UNIT
//                    ON UNIT.FUNITID = MATERIALBASE.FBASEUNITID
//                 INNER JOIN T_BD_UNIT_L UNIT_L
//                    ON UNIT_L.FUNITID = UNIT.FUNITID
//                 WHERE MATERIAL.FNUMBER >= '{2}'
//                   AND MATERIAL.FNUMBER <= '{3}'
//                   AND CUST.FNUMBER >= '{4}'
//                   AND CUST.FNUMBER <= '{5}'
//                   AND OUTSTOCK.FBILLTYPEID = '5518f60aee8191') A
//         GROUP BY A.FFINDATE,
//                  A.FMATERIALNO,
//                  A.FMATERIALNAME,
//                  A.FSPE,
//                  A.UNITID,
//                  A.FCUSTNO,
//                  A.FCUSTNAME) QC
// GROUP BY QC.FFINDATE, QC.FMATERIALNO, QC.FMATERIALNAME, QC.FSPE, QC.UNITID,QC.FCUSTNO,QC.FCUSTNAME", tempTable, dyFilter["F_Ls_StartDate"].ToString(), startMaterial["number"].ToString(), endMaterial["number"].ToString(), startCustomer["number"].ToString(), endCustomer["number"].ToString());
            DBUtils.Execute(base.Context, strSql_gj);
        }
        #endregion
        #region 删除临时表
        private void dropTempTable()
        {
            string strSql = string.Format("TRUNCATE TABLE {0}", tempTable);
            DBUtils.Execute(this.Context, strSql);
            string strSql_1 = string.Format("DROP TABLE {0}", tempTable);
            DBUtils.Execute(this.Context, strSql_1);
        }
        #endregion
        #region 创建临时表
        private void CreateTempTable()
        {
            this.tempTable = addTempTable(base.Context);
            String strSql = String.Format(@"CREATE TABLE {0}
(
  FFINDATE			VARCHAR(50),
  FCUSTNO           VARCHAR(50),
  FCUSTNAME         VARCHAR(100),
  FMATERIALNO		VARCHAR(50),
  FMATERIALNAME     VARCHAR(255),
  FSPE				VARCHAR(255),
  UNITID			VARCHAR(10),
  QCQTY				DECIMAL(23,10),
  QCPRICE			DECIMAL(23,10),
  QCAMOUNT			DECIMAL(23,10),
  OUTQTY			DECIMAL(23,10),
  OUTPRICE			DECIMAL(23,10),
  OUTAMOUNT			DECIMAL(23,10),
  GJQTY				DECIMAL(23,10),
  GJPRICE			DECIMAL(23,10),
  GJAMOUNT			DECIMAL(23,10)
)", tempTable);
            DBUtils.Execute(this.Context,strSql);
        }
        #endregion
        #region 获取临时表名
        private string addTempTable(Kingdee.BOS.Context ctx)
        {
            return ServiceHelper.GetService<IDBService>().CreateTemporaryTableName(ctx);
        }
        #endregion
        #region 合计
        /// <summary>
        /// 设置汇总行，只有显示财务信息时才需要汇总
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override List<SummaryField> GetSummaryColumnInfo(IRptParams filter)
        {
            List<SummaryField> summaryList = new List<SummaryField>();
            summaryList.Add(new SummaryField(string.Format("OUTAMOUNT"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summaryList.Add(new SummaryField(string.Format("QCAMOUNT"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summaryList.Add(new SummaryField(string.Format("GJAMOUNT"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summaryList.Add(new SummaryField(string.Format("QMAMOUNT"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            summaryList.Add(new SummaryField(string.Format("QCQTY"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summaryList.Add(new SummaryField(string.Format("OUTQTY"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summaryList.Add(new SummaryField(string.Format("GJQTY"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summaryList.Add(new SummaryField(string.Format("QMQTY"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));

            
            return summaryList;
        }
        #endregion


    }
}
