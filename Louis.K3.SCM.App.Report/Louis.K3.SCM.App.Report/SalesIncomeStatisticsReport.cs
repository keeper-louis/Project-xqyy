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
    [Description("销售收入统计表，服务器取数插件")]
    public class SalesIncomeStatisticsReport: SysReportBaseService
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
            this.ReportProperty.ReportName = new LocaleValue("销售收入统计表", base.Context.UserLocale.LCID);
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
                var DATAVALUE = header.AddChild("DATAVALUE", new LocaleValue("区域"));
                DATAVALUE.ColIndex = 0;
                var BILLTYPE = header.AddChild("BILLTYPE", new LocaleValue("单据类型"));
                BILLTYPE.ColIndex = 1;
                var MATNUMBER = header.AddChild("MATNUMBER", new LocaleValue("产品代码"));
                MATNUMBER.ColIndex = 2;
                var MATNAME = header.AddChild("MATNAME", new LocaleValue("产品名称"));
                MATNAME.ColIndex = 3;
                var MATSPECIFICATION = header.AddChild("MATSPECIFICATION", new LocaleValue("规格型号"));
                MATSPECIFICATION.ColIndex = 4;
                var UNNUMBER = header.AddChild("UNNUMBER", new LocaleValue("单位(基本)"));
                UNNUMBER.ColIndex = 5;
                var PRICEQTY = header.AddChild("PRICEQTY", new LocaleValue("数量"), SqlStorageType.SqlDecimal);
                PRICEQTY.ColIndex = 6;
                var PRICE = header.AddChild("PRICE", new LocaleValue("单价"), SqlStorageType.SqlDecimal);
                PRICE.ColIndex = 7;
                var NOTAXAMOUNTFOR = header.AddChild("NOTAXAMOUNTFOR", new LocaleValue("销售收入"), SqlStorageType.SqlDecimal);
                NOTAXAMOUNTFOR.ColIndex = 8;
                var TAXRATE = header.AddChild("TAXRATE", new LocaleValue("税率"));
                TAXRATE.ColIndex = 9;
                var TAXAMOUNTFOR = header.AddChild("TAXAMOUNTFOR", new LocaleValue("销售税额"), SqlStorageType.SqlDecimal);
                TAXAMOUNTFOR.ColIndex = 10;
                var ALLAMOUNTFOR = header.AddChild("ALLAMOUNTFOR", new LocaleValue("合计"), SqlStorageType.SqlDecimal);
                ALLAMOUNTFOR.ColIndex = 11;
                return header;
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
            string materialRange = "所有产品";
            string regionRange = "所有地区";
            var result = base.GetReportTitles(filter);
            DynamicObject dyFilter = filter.FilterParameter.CustomFilter;
            if (dyFilter != null)
            {
                if (result == null)
                {
                    result = new ReportTitles();
                }
                //起始日期：结束日期=起始日期
                if (dyFilter["FFormDate"] != null)
                {

                    result.AddTitle("FFormDate", dyFilter["FFormDate"].ToString());
                }
                //结束日期
                if (dyFilter["FToDate"] != null)
                {
                    result.AddTitle("FToDate", dyFilter["FToDate"].ToString());
                }

                //物料范围
                DynamicObjectCollection materialCol = dyFilter["FMaterialId"] as DynamicObjectCollection;
                if (materialCol!=null&&materialCol.Count>0)
                {
                    materialRange = Convert.ToString(materialCol[0]["number"]) + "-" + Convert.ToString(materialCol[materialCol.Count - 1]["number"]);
                }
                result.AddTitle("FMaterialId", materialRange);
                //地区范围
                DynamicObjectCollection regionCol = dyFilter["FRegionId"] as DynamicObjectCollection;
                if (regionCol != null && regionCol.Count > 0)
                {
                    regionRange = Convert.ToString(regionCol[0]["number"]) + "-" + Convert.ToString(regionCol[regionCol.Count - 1]["number"]);
                }
                result.AddTitle("FRegionId", regionRange);
            }
            return result;
        }
        #endregion
        #region 实现报表的主方法
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            try
            {
                DynamicObject dyFilter = filter.FilterParameter.CustomFilter;
                DynamicObjectCollection materialCol = dyFilter["FMaterialId"] as DynamicObjectCollection;//物料集合
                DynamicObjectCollection regionCol = dyFilter["FRegionId"] as DynamicObjectCollection;//地区集合
                DateTime FormDate = Convert.ToDateTime(dyFilter["FFormDate"]);//起始
                DateTime ToDate = Convert.ToDateTime(dyFilter["FToDate"]);//截止
                //this.CreateTempTable();//创建临时表，用于数据整理
                //if (Convert.ToInt32(dyFilter["F_Ls_Combo"]) == 3)
                //{
                //    this.insertDataMaterial(filter);//整理数据插入临时表
                //}
                //String table = tempTable;
                base.KSQL_SEQ = String.Format(base.KSQL_SEQ, table + ".FFINDATE");//排序
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
                DBUtils.Execute(base.Context, strSql);

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
            DynamicObjectCollection materialCol = dyFilter["FMaterialId"] as DynamicObjectCollection;//物料集合
            DynamicObjectCollection regionCol = dyFilter["FRegionId"] as DynamicObjectCollection;//地区集合
            DateTime FormDate = Convert.ToDateTime(dyFilter["FFormDate"]);//起始
            DateTime ToDate = Convert.ToDateTime(dyFilter["FToDate"]);//截止
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
            DBUtils.Execute(base.Context, strSql_qc);
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
            DBUtils.Execute(base.Context, strSql_fc);
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
  DATAVALUE			VARCHAR(200),
  MATNUMBER         VARCHAR(100),
  MATNAME           VARCHAR(100),
  MATSPECIFICATION  VARCHAR(100),
  UNNUMBER          VARCHAR(50),
  BILLTYPE			VARCHAR(100),
  TAXRATE           VARCHAR(50),
  PRICEQTY			DECIMAL(23,10),
  PRICE			    DECIMAL(23,10),
  NOTAXAMOUNTFOR	DECIMAL(23,10),
  TAXAMOUNTFOR		DECIMAL(23,10),
  ALLAMOUNTFOR		DECIMAL(23,10)
)", tempTable);
            DBUtils.Execute(this.Context, strSql);
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
            summaryList.Add(new SummaryField(string.Format("PRICEQTY"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summaryList.Add(new SummaryField(string.Format("NOTAXAMOUNTFOR"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summaryList.Add(new SummaryField(string.Format("TAXAMOUNTFOR"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            summaryList.Add(new SummaryField(string.Format("ALLAMOUNTFOR"), Kingdee.BOS.Core.Enums.BOSEnums.Enu_SummaryType.SUM));
            return summaryList;
        }
        #endregion

    }
}
