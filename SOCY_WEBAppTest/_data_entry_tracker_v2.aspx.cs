using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOCY_WEBAppTest.AppCode;

namespace SOCY_WEBAppTest
{
    public partial class _data_entry_tracker_v2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void save_tracker()
        {
            #region set variables
            DataEntryTracker.dst_id = "3";
            DataEntryTracker.usr_id_create = "1";
            DataEntryTracker.tracker_begin_date = DateTime.Today;
            DataEntryTracker.tracker_end_date = DateTime.Today;
            DataEntryTracker.t_ipt_total_received = txt_ipt.Text != string.Empty ? Convert.ToInt32(txt_ipt.Text) : 1;
            DataEntryTracker.t_ipt_total_entered = 0;
            DataEntryTracker.t_hat_total_received = txt_hat.Text != string.Empty ? Convert.ToInt32(txt_hat.Text) : 1;
            DataEntryTracker.t_hat_total_entered = 12;
            DataEntryTracker.t_hip_total_received = txt_hip.Text != string.Empty ? Convert.ToInt32(txt_hip.Text) : 1;
            DataEntryTracker.t_hip_total_entered = 0;
            DataEntryTracker.t_hv_total_received = txt_home_visit.Text != string.Empty ? Convert.ToInt32(txt_home_visit.Text) : 1;
            DataEntryTracker.t_hv_total_entered = 34;
            DataEntryTracker.t_rat_total_received = txt_rat.Text != string.Empty ? Convert.ToInt32(txt_rat.Text) : 1;
            DataEntryTracker.t_rat_total_entered = 7;
            DataEntryTracker.t_linkage_total_received = txt_linkages.Text != string.Empty ? Convert.ToInt32(txt_linkages.Text) : 1;
            DataEntryTracker.t_linkage_total_entered = 9;
            DataEntryTracker.t_refferal_total_received = txt_referal.Text != string.Empty ? Convert.ToInt32(txt_referal.Text) : 1;
            DataEntryTracker.t_refferal_total_entered = 4;
            DataEntryTracker.t_edu_subsidy_total_received = txt_edu_subsidy.Text != string.Empty ? Convert.ToInt32(txt_edu_subsidy.Text) : 1;
            DataEntryTracker.t_edu_subsidy_total_entered = 8;
            DataEntryTracker.t_cTraining_SILC_total_received = txt_comm_SILC.Text != string.Empty ? Convert.ToInt32(txt_comm_SILC.Text) : 1;
            DataEntryTracker.t_cTraining_SILC_total_entered = 0;
            DataEntryTracker.t_cTraining_youth_total_received = txt_comm_youth.Text != string.Empty ? Convert.ToInt32(txt_comm_youth.Text) : 1;
            DataEntryTracker.t_cTraining_youth_total_entered = 4;
            DataEntryTracker.t_youth_savings_total_received = txt_youth_saving.Text != string.Empty ? Convert.ToInt32(txt_youth_saving.Text) : 1;
            DataEntryTracker.t_youth_savings_total_entered = 0;
            DataEntryTracker.t_appre_progress_total_received = txt_app_progress.Text != string.Empty ? Convert.ToInt32(txt_app_progress.Text) : 1;
            DataEntryTracker.t_appre_progress_total_entered = 20;
            DataEntryTracker.t_training_inventory_total_received = txt_training_inventory.Text != string.Empty ? Convert.ToInt32(txt_training_inventory.Text) : 1;
            DataEntryTracker.t_training_inventory_total_entered = 5;
            DataEntryTracker.t_appre_skill_aqui_total_received = txt_skill_aquisition.Text != string.Empty ? Convert.ToInt32(txt_skill_aquisition.Text) : 1;
            DataEntryTracker.t_appre_skill_aqui_total_entered = 6;
            DataEntryTracker.t_training_comple_total_received = txt_training_completion.Text != string.Empty ? Convert.ToInt32(txt_training_completion.Text) : 1;
            DataEntryTracker.t_training_comple_total_entered = 4;
            DataEntryTracker.t_youth_ass_toolkit_total_received = txt_training_completion.Text != string.Empty ? Convert.ToInt32(txt_training_completion.Text) : 1;
            DataEntryTracker.t_youth_ass_toolkit_total_entered = 4;
            DataEntryTracker.t_youth_tracer_total_received = txt_youth_tracer.Text != string.Empty ? Convert.ToInt32(txt_youth_tracer.Text) : 1;
            DataEntryTracker.t_youth_tracer_total_entered = 0;
            DataEntryTracker.t_dovcc_total_received = txt_dovcc.Text != string.Empty ? Convert.ToInt32(txt_dovcc.Text) : 1;
            DataEntryTracker.t_dovcc_total_entered = 0;
            DataEntryTracker.t_sovcc_total_received = txt_sovcc.Text != string.Empty ? Convert.ToInt32(txt_sovcc.Text) : 1;
            DataEntryTracker.t_sovcc_total_entered = 0;
            DataEntryTracker.t_cbsd_resource_alloc_total_received = txt_cbsd_resource.Text != string.Empty ? Convert.ToInt32(txt_cbsd_resource.Text) : 1;
            DataEntryTracker.t_cbsd_resource_alloc_total_entered = 2;
            DataEntryTracker.t_cbsd_staff_appra_total_received = txt_staff_appraisal.Text != string.Empty ? Convert.ToInt32(txt_cbsd_resource.Text) : 1;
            DataEntryTracker.t_cbsd_staff_appra_total_entered = 0;
            DataEntryTracker.t_reintergration_total_received = txt_reintergration.Text != string.Empty ? Convert.ToInt32(txt_reintergration.Text) : 1;
            DataEntryTracker.t_reintergration_total_entered = 0;
            DataEntryTracker.t_dreams_registration_total_received = txt_dreams_registration.Text != string.Empty ? Convert.ToInt32(txt_dreams_registration.Text) : 1;
            DataEntryTracker.t_dreams_registration_total_entered = 0;
            DataEntryTracker.t_hct_total_received = txt_hct_registration.Text != string.Empty ? Convert.ToInt32(txt_hct_registration.Text) : 2;
            DataEntryTracker.t_hct_total_entered = 0;
            DataEntryTracker.t_dreams_partner_total_received = txt_dreams_partner.Text != string.Empty ? Convert.ToInt32(txt_dreams_partner.Text) : 65;
            DataEntryTracker.t_dreams_partner_total_entered = 0;
            DataEntryTracker.t_sunovuyo_total_received = txt_sinovuyo.Text != string.Empty ? Convert.ToInt32(txt_sinovuyo.Text) : 23;
            DataEntryTracker.t_sunovuyo_total_entered = 0;
            DataEntryTracker.t_dreams_stepping_stones_total_received = txt_stepping_stones.Text != string.Empty ? Convert.ToInt32(txt_stepping_stones.Text) : 1;
            DataEntryTracker.t_dreams_stepping_stones_total_entered = 0;
            DataEntryTracker.t_comments = "sample comment";

            #endregion set variables

            DataEntryTracker.save_data_entry_tracker();
            //if (x > 0)
            //{
            //    string script = "alert(\"Success!\");";
            //    ScriptManager.RegisterStartupScript(this, GetType(),
            //                          "ServerControlScript", script, true);
            //}
            //else
            //{
            //    string script = "alert(\"fail!\");";
            //    ScriptManager.RegisterStartupScript(this, GetType(),
            //                          "ServerControlScript", script, true);
            //}
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            save_tracker();
        }
    }
}