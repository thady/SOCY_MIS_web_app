using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

using SOCY_WEBAppTest.AppCode;
using System.Reflection;

namespace SOCY_WEBAppTest
{
    public partial class _data_entry_tracker_v2 : System.Web.UI.Page
    {
        #region static variables
        public static int pret_linkage_total_entered = 0;
        public static int pret_t_youth_savings_total_entered = 0;
        public static int pret_t_appre_progress_total_entered = 0;
        public static int pret_t_training_inventory_total_entered = 0;
        public static int pret_t_youth_ass_toolkit_total_entered = 0;
        public static int pret_t_reintergration_total_entered = 0;
        public static int pret_t_dreams_registration_total_entered = 0;
        public static int pret_t_hct_total_entered = 0;
        public static int pret_t_dreams_partner_total_entered = 0;
        public static int pret_t_sunovuyo_total_entered = 0;
        public static int pret_t_dreams_stepping_stones_total_entered = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session[utilSOCYWeb.cSVUserID] != null)
                {
                    LoadDistricts();

                    if (Request.QueryString["Token_id"] != null)
                    {
                        ReturnTrackerDetails(Request.QueryString["Token_id"].ToString());
                    }
                }
                else
                {
                    Response.Redirect("_data_entry_tracker_v2_view.aspx");
                }
                
            }
        }

        protected void save_tracker()
        {
            string tracker_id = string.Empty;
            if (Request.QueryString["Token_id"] != null) { tracker_id = Request.QueryString["Token_id"].ToString(); }
            else
            {
                tracker_id = string.Empty;
            }


            #region set variables
            #region Globals
            DataEntryTracker.dst_id = cbo_district.SelectedValue.ToString();
            DataEntryTracker.usr_id_create = Session[utilSOCYWeb.cSVUserID].ToString();
            DataEntryTracker.tracker_date = DateTime.Today;
            DataEntryTracker.t_comments = txt_comment.Text;
            #endregion Globals

            #region total_received
            DataEntryTracker.t_ipt_total_received = txt_ipt.Text != string.Empty ? Convert.ToInt32(txt_ipt.Text) :0;
            DataEntryTracker.t_hat_total_received = txt_hat.Text != string.Empty ? Convert.ToInt32(txt_hat.Text) : 0;
            DataEntryTracker.t_hip_total_received = txt_hip.Text != string.Empty ? Convert.ToInt32(txt_hip.Text) : 0;
            DataEntryTracker.t_hv_total_received = txt_home_visit.Text != string.Empty ? Convert.ToInt32(txt_home_visit.Text) : 0;
            DataEntryTracker.t_rat_total_received = txt_rat.Text != string.Empty ? Convert.ToInt32(txt_rat.Text) : 0;
            DataEntryTracker.t_linkage_total_received = txt_linkages.Text != string.Empty ? Convert.ToInt32(txt_linkages.Text) : 0;
            DataEntryTracker.t_refferal_total_received = txt_referal.Text != string.Empty ? Convert.ToInt32(txt_referal.Text) : 0;
            DataEntryTracker.t_edu_subsidy_total_received = txt_edu_subsidy.Text != string.Empty ? Convert.ToInt32(txt_edu_subsidy.Text) :0;
            DataEntryTracker.t_cTraining_SILC_total_received = txt_comm_SILC.Text != string.Empty ? Convert.ToInt32(txt_comm_SILC.Text) :0;
            DataEntryTracker.t_cTraining_youth_total_received = txt_comm_youth.Text != string.Empty ? Convert.ToInt32(txt_comm_youth.Text) : 0;
            DataEntryTracker.t_youth_savings_total_received = txt_youth_saving.Text != string.Empty ? Convert.ToInt32(txt_youth_saving.Text) : 0;
            DataEntryTracker.t_appre_progress_total_received = txt_app_progress.Text != string.Empty ?Convert.ToInt32(txt_app_progress.Text) : 0;
            DataEntryTracker.t_training_inventory_total_received = txt_training_inventory.Text != string.Empty ? Convert.ToInt32(txt_training_inventory.Text) : 0;
            DataEntryTracker.t_appre_skill_aqui_total_received = txt_skill_aquisition.Text != string.Empty ?Convert.ToInt32(txt_skill_aquisition.Text) : 0;
            DataEntryTracker.t_training_comple_total_received = txt_training_completion.Text != string.Empty ? Convert.ToInt32(txt_training_completion.Text) : 0;
            DataEntryTracker.t_youth_ass_toolkit_total_received = txt_ass_awarding_tool_kit.Text != string.Empty ? Convert.ToInt32(txt_ass_awarding_tool_kit.Text) : 0;
            DataEntryTracker.t_youth_tracer_total_received = txt_youth_tracer.Text != string.Empty ? Convert.ToInt32(txt_youth_tracer.Text) : 0;
            DataEntryTracker.t_dovcc_total_received = txt_dovcc.Text != string.Empty ? Convert.ToInt32(txt_dovcc.Text) : 0;
            DataEntryTracker.t_sovcc_total_received = txt_sovcc.Text != string.Empty ? Convert.ToInt32(txt_sovcc.Text) : 0;
            DataEntryTracker.t_cbsd_resource_alloc_total_received = txt_cbsd_resource.Text != string.Empty ? Convert.ToInt32(txt_cbsd_resource.Text) : 0;
            DataEntryTracker.t_cbsd_staff_appra_total_received = txt_staff_appraisal.Text != string.Empty ? Convert.ToInt32(txt_staff_appraisal.Text) : 0;
            DataEntryTracker.t_reintergration_total_received = txt_reintergration.Text != string.Empty ? Convert.ToInt32(txt_reintergration.Text) : 0;
            DataEntryTracker.t_dreams_registration_total_received = txt_dreams_registration.Text != string.Empty ? Convert.ToInt32(txt_dreams_registration.Text) : 0;
            DataEntryTracker.t_hct_total_received = txt_hct_registration.Text != string.Empty ? Convert.ToInt32(txt_hct_registration.Text) : 0;
            DataEntryTracker.t_dreams_partner_total_received = txt_dreams_partner.Text != string.Empty ? Convert.ToInt32(txt_dreams_partner.Text) : 0;
            DataEntryTracker.t_sunovuyo_total_received = txt_sinovuyo.Text != string.Empty ? Convert.ToInt32(txt_sinovuyo.Text) : 0;
            DataEntryTracker.t_dreams_stepping_stones_total_received = txt_stepping_stones.Text != string.Empty ? Convert.ToInt32(txt_stepping_stones.Text) : 0;
            DataEntryTracker.t_dreams_screening_total_received = txt_dreams_screening.Text != string.Empty ? Convert.ToInt32(txt_dreams_screening.Text) : 0;
            DataEntryTracker.t_dreams_sasa_total_received = txt_sasa_session.Text != string.Empty ? Convert.ToInt32(txt_sasa_session.Text) : 0;
            DataEntryTracker.t_viral_load_total_received = txt_viral_load.Text != string.Empty ? Convert.ToInt32(txt_viral_load.Text) : 0;
            #endregion total_received

            #region total_entered
            DataEntryTracker.t_ipt_total_entered = txt_ipt_entered.Text != string.Empty ? Convert.ToInt32(txt_ipt_entered.Text) : 0; 
            DataEntryTracker.t_hat_total_entered = txt_hat_entered.Text != string.Empty ? Convert.ToInt32(txt_hat_entered.Text) : 0; 
            DataEntryTracker.t_hip_total_entered = txt_hip_entered.Text != string.Empty ? Convert.ToInt32(txt_hip_entered.Text) : 0; 
            DataEntryTracker.t_hv_total_entered = txt_home_visit_entered.Text != string.Empty ? Convert.ToInt32(txt_home_visit_entered.Text) : 0; 
            DataEntryTracker.t_rat_total_entered = txt_rat_entered.Text != string.Empty ? Convert.ToInt32(txt_rat_entered.Text) : 0; 
            DataEntryTracker.t_linkage_total_entered = txt_linkages_entered.Text != string.Empty ? Convert.ToInt32(txt_linkages_entered.Text) : 0; 
            DataEntryTracker.t_refferal_total_entered = txt_referal_entered.Text != string.Empty ? Convert.ToInt32(txt_referal_entered.Text) : 0; 
            DataEntryTracker.t_edu_subsidy_total_entered = txt_edu_subsidy_entered.Text != string.Empty ? Convert.ToInt32(txt_edu_subsidy_entered.Text) : 0; 
            DataEntryTracker.t_cTraining_SILC_total_entered = txt_comm_SILC_entered.Text != string.Empty ? Convert.ToInt32(txt_comm_SILC_entered.Text) : 0; 
            DataEntryTracker.t_cTraining_youth_total_entered = txt_comm_youth_entered.Text != string.Empty ? Convert.ToInt32(txt_comm_youth_entered.Text) : 0; 
            DataEntryTracker.t_youth_savings_total_entered = txt_youth_saving_entered.Text != string.Empty ? Convert.ToInt32(txt_youth_saving_entered.Text) : 0; 
            DataEntryTracker.t_appre_progress_total_entered = txt_app_progress_entered.Text != string.Empty ? Convert.ToInt32(txt_app_progress_entered.Text) : 0;
            DataEntryTracker.t_training_inventory_total_entered = txt_training_inventory_entered.Text != string.Empty ? Convert.ToInt32(txt_training_inventory_entered.Text) : 0;
            DataEntryTracker.t_appre_skill_aqui_total_entered = txt_skill_aquisition_entered.Text != string.Empty ? Convert.ToInt32(txt_skill_aquisition_entered.Text) : 0;
            DataEntryTracker.t_training_comple_total_entered = txt_training_completion.Text != string.Empty ? Convert.ToInt32(txt_training_completion.Text) : 0;
            DataEntryTracker.t_youth_ass_toolkit_total_entered = txt_ass_awarding_tool_kit_entered.Text != string.Empty ? Convert.ToInt32(txt_ass_awarding_tool_kit_entered.Text) : 0;
            DataEntryTracker.t_youth_tracer_total_entered = txt_youth_tracer_entered.Text != string.Empty ? Convert.ToInt32(txt_youth_tracer_entered.Text) : 0;
            DataEntryTracker.t_dovcc_total_entered = txt_dovcc_entered.Text != string.Empty ? Convert.ToInt32(txt_dovcc_entered.Text) : 0;
            DataEntryTracker.t_sovcc_total_entered = txt_sovcc_entered.Text != string.Empty ? Convert.ToInt32(txt_sovcc_entered.Text) : 0;
            DataEntryTracker.t_cbsd_resource_alloc_total_entered = txt_cbsd_resource_entered.Text != string.Empty ? Convert.ToInt32(txt_cbsd_resource_entered.Text) : 0;
            DataEntryTracker.t_cbsd_staff_appra_total_entered = txt_staff_appraisal_entered.Text != string.Empty ? Convert.ToInt32(txt_staff_appraisal_entered.Text) : 0;
            DataEntryTracker.t_dreams_registration_total_entered = txt_dreams_registration_entered.Text != string.Empty ? Convert.ToInt32(txt_dreams_registration_entered.Text) : 0;
            DataEntryTracker.t_hct_total_entered = txt_hct_registration_entered.Text != string.Empty ? Convert.ToInt32(txt_hct_registration_entered.Text) : 0;
            DataEntryTracker.t_dreams_partner_total_entered = txt_dreams_partner_entered.Text != string.Empty ? Convert.ToInt32(txt_dreams_partner_entered.Text) : 0;
            DataEntryTracker.t_sunovuyo_total_entered = txt_sinovuyo_entered.Text != string.Empty ? Convert.ToInt32(txt_sinovuyo_entered.Text) : 0;
            DataEntryTracker.t_dreams_stepping_stones_total_entered = txt_stepping_stones_entered.Text != string.Empty ? Convert.ToInt32(txt_stepping_stones_entered.Text) : 0;

            DataEntryTracker.t_dreams_screening_total_entered = txt_dreams_screening_entered.Text != string.Empty ? Convert.ToInt32(txt_dreams_screening_entered.Text) : 0;
            DataEntryTracker.t_dreams_sasa_total_entered = txt_sasa_session_entered.Text != string.Empty ? Convert.ToInt32(txt_sasa_session_entered.Text) : 0;
            DataEntryTracker.t_viral_load_total_entered = txt_viral_load_entered.Text != string.Empty ? Convert.ToInt32(txt_viral_load_entered.Text) : 0;
            #endregion total_entered

            #region percent_entered
            DataEntryTracker.t_ipt_percent_entered = DataEntryTracker.t_ipt_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_ipt_total_entered / (decimal)DataEntryTracker.t_ipt_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_hat_percent_entered = DataEntryTracker.t_hat_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_hat_total_entered / DataEntryTracker.t_hat_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_hip_percent_entered = DataEntryTracker.t_hip_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_hip_total_entered / DataEntryTracker.t_hip_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_hv_percent_entered = DataEntryTracker.t_hv_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_hv_total_entered / DataEntryTracker.t_hv_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_rat_percent_entered = DataEntryTracker.t_rat_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_rat_total_entered / DataEntryTracker.t_rat_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_linkage_percent_entered = DataEntryTracker.t_linkage_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_linkage_total_entered / DataEntryTracker.t_linkage_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_refferal_percent_entered = DataEntryTracker.t_refferal_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_refferal_total_entered / DataEntryTracker.t_refferal_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_edu_subsidy_percent_entered = DataEntryTracker.t_edu_subsidy_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_edu_subsidy_total_entered / DataEntryTracker.t_edu_subsidy_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_cTraining_SILC_percent_entered = DataEntryTracker.t_cTraining_SILC_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_cTraining_SILC_total_entered / DataEntryTracker.t_cTraining_SILC_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_cTraining_youth_percent_entered = DataEntryTracker.t_cTraining_youth_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_cTraining_youth_total_entered / DataEntryTracker.t_cTraining_youth_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_youth_savings_percent_entered = DataEntryTracker.t_youth_savings_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_youth_savings_total_entered / DataEntryTracker.t_youth_savings_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_appre_progress_percent_entered = DataEntryTracker.t_appre_progress_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_appre_progress_total_entered / DataEntryTracker.t_appre_progress_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_training_inventory_percent_entered = DataEntryTracker.t_training_inventory_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_training_inventory_total_entered / DataEntryTracker.t_training_inventory_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_appre_skill_aqui_percent_entered = DataEntryTracker.t_appre_skill_aqui_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_appre_skill_aqui_total_entered / DataEntryTracker.t_appre_skill_aqui_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_training_comple_percent_entered = DataEntryTracker.t_training_comple_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_training_comple_total_entered / DataEntryTracker.t_training_comple_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_youth_ass_toolkit_percent_entered = DataEntryTracker.t_youth_ass_toolkit_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_youth_ass_toolkit_total_entered / DataEntryTracker.t_youth_ass_toolkit_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_youth_tracer_percent_entered = DataEntryTracker.t_youth_tracer_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_youth_tracer_total_entered / DataEntryTracker.t_youth_tracer_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_dovcc_percent_entered = DataEntryTracker.t_dovcc_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_dovcc_total_entered / DataEntryTracker.t_dovcc_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_sovcc_percent_entered = DataEntryTracker.t_sovcc_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_sovcc_total_entered / DataEntryTracker.t_sovcc_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_cbsd_resource_alloc_percent_entered = DataEntryTracker.t_cbsd_resource_alloc_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_cbsd_resource_alloc_total_entered / DataEntryTracker.t_cbsd_resource_alloc_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_cbsd_staff_appra_percent_entered = DataEntryTracker.t_cbsd_staff_appra_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_cbsd_staff_appra_total_entered / DataEntryTracker.t_cbsd_staff_appra_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_reintergration_percent_entered = DataEntryTracker.t_reintergration_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_reintergration_total_entered / DataEntryTracker.t_reintergration_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_dreams_registration_percent_entered = DataEntryTracker.t_dreams_registration_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_dreams_registration_total_entered / DataEntryTracker.t_dreams_registration_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_hct_percent_entered = DataEntryTracker.t_hct_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_hct_total_entered / DataEntryTracker.t_hct_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_dreams_partner_percent_entered = DataEntryTracker.t_dreams_partner_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_dreams_partner_total_entered / DataEntryTracker.t_dreams_partner_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_sunovuyo_percent_entered = DataEntryTracker.t_sunovuyo_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_sunovuyo_total_entered / DataEntryTracker.t_sunovuyo_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_dreams_stepping_stones_percent_entered = DataEntryTracker.t_dreams_stepping_stones_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_dreams_stepping_stones_total_entered / DataEntryTracker.t_dreams_stepping_stones_total_received) * 100), 2).ToString() : string.Empty;

            DataEntryTracker.t_dreams_screening_percent_entered = DataEntryTracker.t_dreams_screening_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_dreams_screening_total_entered / DataEntryTracker.t_dreams_screening_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_dreams_sasa_percent_entered = DataEntryTracker.t_dreams_sasa_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_dreams_sasa_total_entered / DataEntryTracker.t_dreams_sasa_total_received) * 100), 2).ToString() : string.Empty;
            DataEntryTracker.t_viral_load_percent_entered = DataEntryTracker.t_viral_load_total_received != 0 ? Math.Round((((decimal)DataEntryTracker.t_viral_load_total_entered / DataEntryTracker.t_viral_load_total_received) * 100), 2).ToString() : string.Empty;
            #endregion percent_entered

            #endregion set variables
            if (Request.QueryString["Token_id"] == null)
            {
                int count = DataEntryTracker.Check_if_tracker_exists(cbo_district.SelectedValue.ToString(), Convert.ToDateTime(txt_date.Text));
                if (count > 0)
                {
                    string script = "alert(\"Tracker for this date already uploaded!\");";
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "ServerControlScript", script, true);
                }
                else
                {
                    DataEntryTracker.save_data_entry_tracker(tracker_id);
                }
            }
            else
            {
                string tracker_inserted_id = DataEntryTracker.save_data_entry_tracker(tracker_id);
            }  
        }


        protected void btnsave_Click(object sender, EventArgs e)
        {
            string tracker_id = string.Empty;

            if (Request.QueryString["Token_id"] != null) { tracker_id = Request.QueryString["Token_id"].ToString(); }
            else
            {
                tracker_id = string.Empty;
            }

            if (ValidateInput() == false)
            {
                string script = "alert(\"Fill in all required values!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
            }
            else
            {
                DataEntryTracker.Return_previous_tracker_total_tools_received(cbo_district.SelectedValue.ToString(), tracker_id);
                DataEntryTracker.Return_Tools_entered(cbo_district.SelectedValue.ToString());
                save_tracker();

                #region Update manual Tables
                DataEntryTracker.Update_manual_tools(cbo_district.SelectedValue.ToString(), "t_linkage_total_entered", DataEntryTracker.t_linkage_total_entered);
                DataEntryTracker.Update_manual_tools(cbo_district.SelectedValue.ToString(), "t_youth_savings_total_entered", DataEntryTracker.t_youth_savings_total_entered);
                DataEntryTracker.Update_manual_tools(cbo_district.SelectedValue.ToString(), "t_appre_progress_total_entered", DataEntryTracker.t_appre_progress_total_entered);
                DataEntryTracker.Update_manual_tools(cbo_district.SelectedValue.ToString(), "t_training_inventory_total_entered", DataEntryTracker.t_training_inventory_total_entered);
                DataEntryTracker.Update_manual_tools(cbo_district.SelectedValue.ToString(), "t_youth_ass_toolkit_total_entered", DataEntryTracker.t_youth_ass_toolkit_total_entered);
                DataEntryTracker.Update_manual_tools(cbo_district.SelectedValue.ToString(), "t_reintergration_total_entered", DataEntryTracker.t_reintergration_total_entered);
                DataEntryTracker.Update_manual_tools(cbo_district.SelectedValue.ToString(), "t_dreams_registration_total_entered", DataEntryTracker.t_dreams_registration_total_entered);
                DataEntryTracker.Update_manual_tools(cbo_district.SelectedValue.ToString(), "t_hct_total_entered", DataEntryTracker.t_hct_total_entered);
                DataEntryTracker.Update_manual_tools(cbo_district.SelectedValue.ToString(), "t_dreams_partner_total_entered", DataEntryTracker.t_dreams_partner_total_entered);
                DataEntryTracker.Update_manual_tools(cbo_district.SelectedValue.ToString(), "t_sunovuyo_total_entered", DataEntryTracker.t_sunovuyo_total_entered);
                DataEntryTracker.Update_manual_tools(cbo_district.SelectedValue.ToString(), "t_dreams_stepping_stones_total_entered", DataEntryTracker.t_dreams_stepping_stones_total_entered);
                #endregion Update manual Tables
            }

        }

        protected void LoadDistricts()
        {
            DataTable dt = DataEntryTracker.ReturnDistrictList();
            if (dt.Rows.Count > 0)
            {
                DataRow dtEmptyRow = dt.NewRow();
                dtEmptyRow["dst_id"] = "-1";
                dtEmptyRow["dst_name"] = "Select District";
                dt.Rows.InsertAt(dtEmptyRow, 0);

                cbo_district.DataSource = dt;
                cbo_district.DataValueField = "dst_id";
                cbo_district.DataTextField = "dst_name";
                cbo_district.DataBind();
            }
        }

        protected bool ValidateInput()
        {
            bool isValidInput = false;
            if (cbo_district.SelectedValue.ToString() == "-1" || txt_date.Text == string.Empty)
            {
                isValidInput = false;
            }
            else
            {
                isValidInput = true;
            }

            return isValidInput;
        }

        protected void ReturnTrackerDetails(string tracker_id)
        {
            DataTable dt = DataEntryTracker.LoadTrackerDetails(tracker_id);
            if (dt.Rows.Count > 0)
            {
                DataRow dtRow = dt.Rows[0];
                cbo_district.SelectedValue = dtRow["dst_id"].ToString();
                txt_date.Text = Convert.ToDateTime(dtRow["tracker_date"]).ToString();
                txt_ipt.Text = Convert.ToInt32(dtRow["t_ipt_total_received"].ToString()).ToString();
                txt_ipt_entered.Text = Convert.ToInt32(dtRow["t_ipt_total_entered"].ToString()).ToString();

                txt_hat.Text = Convert.ToInt32(dtRow["t_hat_total_received"].ToString()).ToString();
                txt_hat_entered.Text = Convert.ToInt32(dtRow["t_hat_total_entered"].ToString()).ToString();

                txt_hip.Text = Convert.ToInt32(dtRow["t_hip_total_received"].ToString()).ToString();
                txt_hip_entered.Text = Convert.ToInt32(dtRow["t_hip_total_entered"].ToString()).ToString();

                txt_home_visit.Text = Convert.ToInt32(dtRow["t_hv_total_received"].ToString()).ToString();
                txt_home_visit_entered.Text = Convert.ToInt32(dtRow["t_hv_total_entered"].ToString()).ToString();

                txt_rat.Text = Convert.ToInt32(dtRow["t_rat_total_received"].ToString()).ToString();
                txt_rat_entered.Text = Convert.ToInt32(dtRow["t_rat_total_entered"].ToString()).ToString();

                txt_linkages.Text = Convert.ToInt32(dtRow["t_linkage_total_received"].ToString()).ToString();
                txt_linkages_entered.Text = Convert.ToInt32(dtRow["t_linkage_total_entered"].ToString()).ToString();

                txt_referal.Text = Convert.ToInt32(dtRow["t_refferal_total_received"].ToString()).ToString();
                txt_referal_entered.Text = Convert.ToInt32(dtRow["t_refferal_total_entered"].ToString()).ToString();

                txt_edu_subsidy.Text = Convert.ToInt32(dtRow["t_edu_subsidy_total_received"].ToString()).ToString();
                txt_edu_subsidy_entered.Text = Convert.ToInt32(dtRow["t_edu_subsidy_total_entered"].ToString()).ToString();

                txt_comm_SILC.Text = Convert.ToInt32(dtRow["t_cTraining_SILC_total_received"].ToString()).ToString();
                txt_comm_SILC_entered.Text = Convert.ToInt32(dtRow["t_cTraining_SILC_total_entered"].ToString()).ToString();

                txt_comm_youth.Text = Convert.ToInt32(dtRow["t_cTraining_youth_total_received"].ToString()).ToString();
                txt_comm_youth_entered.Text = Convert.ToInt32(dtRow["t_cTraining_youth_total_entered"].ToString()).ToString();

                txt_youth_saving.Text = Convert.ToInt32(dtRow["t_youth_savings_total_received"].ToString()).ToString();
                txt_youth_saving_entered.Text = Convert.ToInt32(dtRow["t_youth_savings_total_entered"].ToString()).ToString();

                txt_app_progress.Text = Convert.ToInt32(dtRow["t_appre_progress_total_received"].ToString()).ToString();
                txt_app_progress_entered.Text = Convert.ToInt32(dtRow["t_appre_progress_total_entered"].ToString()).ToString();

                txt_training_inventory.Text = Convert.ToInt32(dtRow["t_training_inventory_total_received"].ToString()).ToString();
                txt_training_inventory_entered.Text = Convert.ToInt32(dtRow["t_training_inventory_total_entered"].ToString()).ToString();

                txt_skill_aquisition.Text = Convert.ToInt32(dtRow["t_appre_skill_aqui_total_received"].ToString()).ToString();
                txt_skill_aquisition_entered.Text = Convert.ToInt32(dtRow["t_appre_skill_aqui_total_entered"].ToString()).ToString();

                txt_training_completion.Text = Convert.ToInt32(dtRow["t_training_comple_total_received"].ToString()).ToString();
                txt_training_completion_entered.Text = Convert.ToInt32(dtRow["t_training_comple_total_entered"].ToString()).ToString();

                txt_ass_awarding_tool_kit.Text = Convert.ToInt32(dtRow["t_youth_ass_toolkit_total_received"].ToString()).ToString();
                txt_ass_awarding_tool_kit_entered.Text = Convert.ToInt32(dtRow["t_youth_ass_toolkit_total_entered"].ToString()).ToString();

                txt_youth_tracer.Text = Convert.ToInt32(dtRow["t_youth_tracer_total_received"].ToString()).ToString();
                txt_youth_tracer_entered.Text = Convert.ToInt32(dtRow["t_youth_tracer_total_entered"].ToString()).ToString();

                txt_dovcc.Text = Convert.ToInt32(dtRow["t_dovcc_total_received"].ToString()).ToString();
                txt_dovcc_entered.Text = Convert.ToInt32(dtRow["t_dovcc_total_entered"].ToString()).ToString();

                txt_sovcc.Text = Convert.ToInt32(dtRow["t_sovcc_total_received"].ToString()).ToString();
                txt_sovcc_entered.Text = Convert.ToInt32(dtRow["t_sovcc_total_entered"].ToString()).ToString();

                txt_cbsd_resource.Text = Convert.ToInt32(dtRow["t_cbsd_resource_alloc_total_received"].ToString()).ToString();
                txt_cbsd_resource_entered.Text = Convert.ToInt32(dtRow["t_cbsd_resource_alloc_total_entered"].ToString()).ToString();

                txt_staff_appraisal.Text = Convert.ToInt32(dtRow["t_cbsd_staff_appra_total_received"].ToString()).ToString();
                txt_staff_appraisal_entered.Text = Convert.ToInt32(dtRow["t_cbsd_staff_appra_total_entered"].ToString()).ToString();

                txt_reintergration.Text = Convert.ToInt32(dtRow["t_reintergration_total_received"].ToString()).ToString();
                txt_reintergration_entered.Text = Convert.ToInt32(dtRow["t_reintergration_total_entered"].ToString()).ToString();

                txt_dreams_registration.Text = Convert.ToInt32(dtRow["t_dreams_registration_total_received"].ToString()).ToString();
                txt_dreams_registration_entered.Text = Convert.ToInt32(dtRow["t_dreams_registration_total_entered"].ToString()).ToString();

                txt_hct_registration.Text = Convert.ToInt32(dtRow["t_hct_total_received"].ToString()).ToString();
                txt_hct_registration_entered.Text = Convert.ToInt32(dtRow["t_hct_total_entered"].ToString()).ToString();

                txt_dreams_partner.Text = Convert.ToInt32(dtRow["t_dreams_partner_total_received"].ToString()).ToString();
                txt_dreams_partner_entered.Text = Convert.ToInt32(dtRow["t_dreams_partner_total_entered"].ToString()).ToString();

                txt_sinovuyo.Text = Convert.ToInt32(dtRow["t_sunovuyo_total_received"].ToString()).ToString();
                txt_sinovuyo_entered.Text = Convert.ToInt32(dtRow["t_sunovuyo_total_entered"].ToString()).ToString();

                txt_stepping_stones.Text = Convert.ToInt32(dtRow["t_dreams_stepping_stones_total_received"].ToString()).ToString();
                txt_stepping_stones_entered.Text = Convert.ToInt32(dtRow["t_dreams_stepping_stones_total_entered"].ToString()).ToString();

                txt_dreams_screening.Text = Convert.ToInt32(dtRow["t_dreams_screening_total_received"].ToString()).ToString();
                txt_dreams_screening_entered.Text = Convert.ToInt32(dtRow["t_dreams_screening_total_entered"].ToString()).ToString();

                txt_sasa_session.Text = Convert.ToInt32(dtRow["t_dreams_sasa_total_received"].ToString()).ToString();
                txt_sasa_session_entered.Text = Convert.ToInt32(dtRow["t_dreams_sasa_total_entered"].ToString()).ToString();

                txt_viral_load.Text = Convert.ToInt32(dtRow["t_viral_load_total_received"].ToString()).ToString();
                txt_viral_load_entered.Text = Convert.ToInt32(dtRow["t_viral_load_total_entered"].ToString()).ToString();
            }
        }

        #region Clear
        protected void Clear()
        {
            cbo_district.SelectedValue = "-1";
            txt_date.Text = string.Empty;
            txt_ipt.Text = "0";
            txt_hat.Text = "0";
            txt_hip.Text = "0";
            txt_home_visit.Text = "0";
            txt_rat.Text = "0";
            txt_linkages.Text = "0";
            txt_referal.Text = "0";
            txt_edu_subsidy.Text = "0";
            txt_comm_SILC.Text = "0";
            txt_comm_youth.Text = "0";
            txt_youth_saving.Text = "0";
            txt_app_progress.Text = "0";
            txt_training_inventory.Text = "0";
            txt_skill_aquisition.Text = "0";
            txt_training_completion.Text = "0";
            txt_ass_awarding_tool_kit.Text = "0";
            txt_youth_tracer.Text = "0";
            txt_dovcc.Text = "0";
            txt_sovcc.Text = "0";
            txt_cbsd_resource.Text = "0";
            txt_staff_appraisal.Text = "0";
            txt_reintergration.Text = "0";
            txt_dreams_registration.Text = "0";
            txt_hct_registration.Text = "0";
            txt_dreams_partner.Text = "0";
            txt_sinovuyo.Text = "0";
            txt_stepping_stones.Text = "0";
            txt_comment.Text = "0";
            txt_dreams_partner_entered.Text = "0";
            txt_linkages_entered.Text = "0";
            txt_youth_saving_entered.Text = "0";
            txt_app_progress_entered.Text = "0";
            txt_training_inventory_entered.Text = "0";
            txt_ass_awarding_tool_kit_entered.Text = "0";
            txt_reintergration_entered.Text = "0";
            txt_dreams_registration_entered.Text = "0";
            txt_hct_registration_entered.Text = "0";
            txt_dreams_partner_entered.Text = "0";
            txt_sinovuyo_entered.Text = "0";
            txt_stepping_stones_entered.Text = "0";

            //remove query string
            PropertyInfo isreadonly =
          typeof(System.Collections.Specialized.NameValueCollection).GetProperty(
          "IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove("Token_id");
            Response.Redirect(Request.RawUrl);
        }
        #endregion Clear

        protected void btnreset_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}