using Kingdee.BOS.Contracts.Report;
using Kingdee.BOS.Core.List;
using Kingdee.BOS.Core.Report.PlugIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louis.K3.SCM.App.Report
{
    [System.ComponentModel.Description("分期收款发出商品汇总表，金额字段显示千分位")]
    public class FQSKFCSPSysReport : AbstractSysReportPlugIn
    {
        public override void FormatCellValue(Kingdee.BOS.Core.Report.PlugIn.Args.FormatCellValueArgs args)
        {
            base.FormatCellValue(args);
            if (args.Header.Key.Equals("QCAMOUNT"))
            {
                args.FormateValue = Convert.ToDecimal(args.Value).ToString("#,##0.00"); 
            }
            if (args.Header.Key.Equals("OUTAMOUNT"))
            {
                args.FormateValue = Convert.ToDecimal(args.Value).ToString("#,##0.00");
            }
            if (args.Header.Key.Equals("GJAMOUNT"))
            {
                args.FormateValue = Convert.ToDecimal(args.Value).ToString("#,##0.00");
            }
            if (args.Header.Key.Equals("QMAMOUNT"))
            {
                args.FormateValue = Convert.ToDecimal(args.Value).ToString("#,##0.00");
            }


        }
    }
}
