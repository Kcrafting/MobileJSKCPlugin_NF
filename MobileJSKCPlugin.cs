using Kingdee.K3.MobileBOS.PlugInModel;
using Kingdee.K3.MobileBOS.PlugInModel.Interface;
using Kingdee.K3.MobileBOS.PlugInModel.PlugIn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileJSKCPlugin_NF
{
    public class MobileJSKCPlugin : IBillPlugIn
    {
        public override void Changed(object sender, ChangeEventArgs e)
        {
            base.Changed(sender, e);
            try
            {
                if (e.Row != 0 && e.Key != "" && (e.Key == "FItemID" || e.Key == "FSCStockID"))
                {
                    var ds = m_BillInterface.GetDataRow(1, e.Row);
                    var fi = m_BillInterface.GetFieldValue("FItemID", e.Row);
                    var fs = m_BillInterface.GetFieldValue("FSCStockID", e.Row);
                    if(fi != null && fs != null)
                    {
                        var tfi = fi.ToString();
                        var tfs = fs.ToString();
                        if (!String.IsNullOrEmpty(tfi) && !String.IsNullOrEmpty(tfs))
                        {
                            var s = "select FQty from ICInventory where FItemID = " + fi + "  and FStockID = " + fs;
                            var ret = m_BillInterface.GetData(s);
                            if (ret.Tables[0].Rows.Count > 0)
                            {
                                var vv = ret.Tables[0].Rows[0]["FQty"].ToString();
                                
                                if (!String.IsNullOrEmpty(vv))
                                {
                                    var id = decimal.Parse(vv);
                                    m_BillInterface.SetFieldValue("FEntrySelfB0457", id.ToString("F3"), e.Row);
                                }
                            }
                            else
                            {
                                m_BillInterface.SetFieldValue("FEntrySelfB0457", "0", e.Row);
                            }
                            
                        }
                    }
                }
            }
            catch(Exception exp)
            {
                m_BillInterface.Alert(exp.Message);
            }
        }
    }
}
