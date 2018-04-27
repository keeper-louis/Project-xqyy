using Kingdee.BOS.App;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.K3.SCM.App.Stock.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louis.K3.SCM.Stock.App.ReportPlugIn
{
    [Description("二开物料收发明细表")]
    public class MaterialReport : StockDetailRpt
    {
        private string[] materialRptTableNames;

        public override void BuilderReportSqlAndTempTable(Kingdee.BOS.Core.Report.IRptParams filter, string tableName)
        {
            
            //创建临时表，用于存放自己的数据
            IDBService dbservice = ServiceHelper.GetService<IDBService>();
            materialRptTableNames = dbservice.CreateTemporaryTableName(this.Context, 1);
            string strTable = materialRptTableNames[0];
            //调用基类的方法，获取初步查询的结果到临时表
            base.BuilderReportSqlAndTempTable(filter, strTable);
            //对初步查询的结果进行处理，然后写回基类默认的存储查询结果临时表
            string strSql = string.Format(@"SELECT t1.*,
	CASE 
	WHEN T1.FFORMID = 'STK_MisDelivery' THEN MIS.FPICKERID
	WHEN T1.FFORMID = 'PRD_PickMtrl' THEN MTRL.FPICKERID
	WHEN T1.FFORMID = 'PRD_FeedMtrl' THEN FEED.FPICKERID
	ELSE NULL
	END FPICKERID, --领料人
    CASE
    WHEN T1.FFORMID = 'PRD_ReturnMtrl' THEN RET.FRETURNERID
    ELSE NULL
    END FRETURNERID, ----退料人
	CASE 
	WHEN T1.FFORMID = 'PRD_PickMtrl' THEN MTRL.F_KD_WLBM
	WHEN T1.FFORMID = 'PRD_FeedMtrl' THEN FEED.F_KD_BASE
	WHEN T1.FFORMID = 'PRD_ReturnMtrl' THEN RET.F_KD_WLBM
	ELSE NULL
	END F_Ls_Material, --主产品物料
	CASE 
	WHEN T1.FFORMID = 'PRD_PickMtrl' THEN MTRL.F_kd_CPPJ
	WHEN T1.FFORMID = 'PRD_FeedMtrl' THEN FEED.F_kd_Text
	WHEN T1.FFORMID = 'PRD_ReturnMtrl' THEN RET.F_kd_CPPH
	ELSE NULL
	END F_LS_CPPH --主产品批号
	into {0}
  FROM {1} T1
  LEFT JOIN T_STK_MISDELIVERY MIS
  ON T1.FBILLID = MIS.FID
  LEFT JOIN T_PRD_PICKMTRL MTRL
  ON MTRL.FID = T1.FBILLID
  LEFT JOIN T_PRD_FEEDMTRL FEED
  ON T1.FBILLID = FEED.FID
  LEFT JOIN T_PRD_RETURNMTRL RET
  ON T1.FBILLID = RET.FID", tableName,strTable);
            DBUtils.Execute(this.Context, strSql);

        }
        public override void CloseReport()
        {
            if (materialRptTableNames.Length<=0)
            {
                return;
            }
            IDBService dbservice = ServiceHelper.GetService<IDBService>();
            dbservice.DeleteTemporaryTableName(this.Context,materialRptTableNames);
            base.CloseReport();
        }
    }
}
