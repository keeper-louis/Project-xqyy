using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Orm.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XQYY.K3.BILLEDIT.PlugIn
{
    [Description("自定义费用分配标准值维护，选择订单携带主产品批号")]
    public class ZDYFYFPBillEdit : AbstractBillPlugIn
    {
        public override void OnBillInitialize(Kingdee.BOS.Core.Bill.PlugIn.Args.BillInitializeEventArgs e)
        {
            base.OnBillInitialize(e);
        }
        public override void DataChanged(Kingdee.BOS.Core.DynamicForm.PlugIn.Args.DataChangedEventArgs e)
        {
            
            String key = e.Field.Key.ToUpperInvariant();
            if (key == "FPROORDERID")
            {
                this.GetPH(e);
            }
            else
            {
                base.DataChanged(e);
            }
        }

        private void GetPH(Kingdee.BOS.Core.DynamicForm.PlugIn.Args.DataChangedEventArgs e)
        {
            DynamicObject scOrder = base.Model.GetValue("FPROORDERID", e.Row) as DynamicObject;
            if (scOrder!=null&&scOrder["Number"]!=null)
            {
                string orderNumber = scOrder["Number"].ToString();
                string strSql = string.Format(@"select my.F_KD_SCPH from T_PRD_MO m inner join T_PRD_MOENTRY my on m.FID = my.FID where m.FBILLNO = '{0}'",orderNumber);
                string queryReslut = DBUtils.ExecuteScalar<string>(base.Context,strSql,"no",null);
                if (!queryReslut.Equals("no"))
                {
                    base.Model.SetValue("F_PBCJ_ph", queryReslut, e.Row);
                }
            }
        }
    }
}
