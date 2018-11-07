using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.IO;

namespace SOCY_WEBAppTest
{
    public partial class detailed_modal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString["token_id"] == null)
                {
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    switch (Request.QueryString["modalType"].ToString())
                    {
                        case "hiv_art":
                            LoadModelData(Request.QueryString["token_id"].ToString());
                            lblPageHeader.Text = "HIV ART STATUS:" + Request.QueryString["token_id"].ToString();
                            break;
                        case "ovc_serve":
                            LoadOvcServed_by_subcounty(Request.QueryString["token_id"].ToString());
                            lblPageHeader.Text = "OVC_SERV:" + Request.QueryString["token_id"].ToString();
                            break;
                    }
                   
                }
                
            }
        }

        public void LoadModelData(string dst_name)
        {
           
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

            DataTable _dt = new DataTable();
            SqlDataAdapter Adapt = null;

            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                string strsQL = @"SELECT dst_name, UPPER(sct_name) sct_name,wrd_name  ,hh_code ,hhm_name,gnd_name,hst_name,ynna_name
                                FROM   (SELECT hhv.hhm_id,dst.dst_name, sct.sct_name,W.wrd_name,hh.hh_code,hhm.hhm_first_name + ' ' + hhm.hhm_last_name AS hhm_name,gnd.gnd_name,hst.hst_name,ynna.ynna_name,Row_number() OVER(PARTITION BY hhvm.hhm_id ORDER BY hhvm.hhm_id) rn FROM hh_household_home_visit_member hhvm 
                                INNER JOIN hh_household_home_visit hhv ON hhv.hhv_id = hhvm.hhv_id
                                INNER JOIN hh_household_member hhm ON hhvm.hhm_id = hhm.hhm_id
                                INNER JOIN hh_household hh ON hhv.hh_id = hh.hh_id
                                INNER JOIN lst_ward W ON hh.wrd_id = W.wrd_id
                                INNER JOIN lst_sub_county sct ON W.sct_id = sct.sct_id
                                INNER JOIN lst_district dst ON dst.dst_id = sct.dst_id
                                INNER JOIN lst_gender gnd ON hhm.gnd_id = gnd.gnd_id
                                INNER JOIN lst_hiv_status hst ON hhvm.hst_id = hst.hst_id
                                INNER JOIN lst_yes_no_not_applicable ynna ON hhvm.ynna_id_hhp_art = ynna.ynna_id
                                WHERE hhv.hhv_date between '2018-10-01' AND '2018-12-31' 
                                AND dst.dst_name = '{0}'
                                AND hhvm.hst_id = '1'
                                AND (hhvm.ynna_id_hhp_art = '0' OR hhvm.ynna_id_hhp_art = '-1')) t
                                WHERE  rn = 1";

                strsQL = string.Format(strsQL, dst_name);
                SqlCommand query = new SqlCommand(strsQL, conn);
                Adapt = new SqlDataAdapter(query);
                Adapt.Fill(_dt);
            }

            gdvModel.DataSource = _dt;
            gdvModel.DataBind();


        }

        public void LoadOvcServed_by_subcounty(string dst_name)
        {

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SOCY_LIVE"].ToString());

            DataTable _dt = new DataTable();
            SqlDataAdapter Adapt = null;

            using (conn)
            {
                // Establish the connection with the database
                conn.Open();

                string strsQL = @";With cte_hhm_served AS
                                    (
                                    SELECT UPPER(dst.dst_name) AS dst_name,UPPER(sct.sct_name) AS sct_name, COUNT( DISTINCT hhvm.hhm_id) AS total_ovc FROM hh_household_home_visit_member hhvm
                                    INNER join hh_household_member hhm ON hhvm.hhm_id = hhm.hhm_id
                                    INNER JOIN hh_household_home_visit hhv ON hhvm.hhv_id = hhv.hhv_id
                                    INNER JOIN hh_household hh ON hhm.hh_id = hh.hh_id
                                    INNER JOIN lst_ward W ON hh.wrd_id = W.wrd_id
                                    INNER JOIN lst_sub_county sct ON W.sct_id = sct.sct_id
                                    INNER JOIN lst_district dst ON sct.dst_id = dst.dst_id
                                    WHERE hhv.hhv_date between '2018-10-01' AND '2018-12-31'
                                    AND dst.dst_name = '{0}'
                                    GROUP BY  dst.dst_name,sct.sct_name
                                    )
                                    SELECT* FROM cte_hhm_served";

                strsQL = string.Format(strsQL, dst_name);
                SqlCommand query = new SqlCommand(strsQL, conn);
                Adapt = new SqlDataAdapter(query);
                Adapt.Fill(_dt);
            }

            gdvModel_ovc_serv.DataSource = _dt;
            gdvModel_ovc_serv.DataBind();


        }

        protected void DownLoadCsv()
        {
            switch (Request.QueryString["modalType"].ToString())
            {
                case "hiv_art":
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + Request.QueryString["token_id"].ToString() + "_ART.csv");
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    //gdvModel.AllowPaging = false;
                    //gdvModel.DataBind();

                    StringBuilder columnbind = new StringBuilder();
                    for (int k = 0; k < gdvModel.Columns.Count; k++)
                    {

                        columnbind.Append(gdvModel.Columns[k].HeaderText + ',');
                    }

                    columnbind.Append("\r\n");
                    for (int i = 0; i < gdvModel.Rows.Count; i++)
                    {
                        for (int k = 0; k < gdvModel.Columns.Count; k++)
                        {

                            columnbind.Append(gdvModel.Rows[i].Cells[k].Text + ',');
                        }

                        columnbind.Append("\r\n");
                    }
                    Response.Output.Write(columnbind.ToString());
                    Response.Flush();
                    Response.End();
                    break;
                case "ovc_serve":
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + Request.QueryString["token_id"].ToString() + "_OVC_serv.csv");
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    //gdvModel.AllowPaging = false;
                    //gdvModel.DataBind();

                    StringBuilder _columnbind = new StringBuilder();
                    for (int k = 0; k < gdvModel_ovc_serv.Columns.Count; k++)
                    {

                        _columnbind.Append(gdvModel_ovc_serv.Columns[k].HeaderText + ',');
                    }

                    _columnbind.Append("\r\n");
                    for (int i = 0; i < gdvModel_ovc_serv.Rows.Count; i++)
                    {
                        for (int k = 0; k < gdvModel_ovc_serv.Columns.Count; k++)
                        {

                            _columnbind.Append(gdvModel_ovc_serv.Rows[i].Cells[k].Text + ',');
                        }

                        _columnbind.Append("\r\n");
                    }
                    Response.Output.Write(_columnbind.ToString());
                    Response.Flush();
                    Response.End();
                    break;
            }
            
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            switch (Request.QueryString["modalType"].ToString()) {
                case "hiv_art":
                    if (gdvModel.Rows.Count > 0)
                    {
                        DownLoadCsv();
                    }
                    break;
                case "ovc_serve":
                    if (gdvModel_ovc_serv.Rows.Count > 0)
                    {
                        DownLoadCsv();
                    }
                    break;
            }
           
        }
    }
}