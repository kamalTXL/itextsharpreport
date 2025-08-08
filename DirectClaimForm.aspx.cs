using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMC.CMC.BAL;
using FMC.CMC.Entity;
using System.Globalization;
using FMC.CMC.CommonHelper;
using System.IO;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.Diagnostics;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Ionic.Zip;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Linq;



namespace CMC_UI.Claim
{
    public partial class DirectClaimForm : FMCBaseClass
    {
        //DataTable dt_InsuredPerson_Copy = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            

            bool isPreLoad = true;
            if (!IsPostBack)
            {
                if (Session["ActionUserID"] == null)
                {
                    Response.Redirect("default.aspx");
                }

                Load_defaultFormData();
                try
                {
                    string ClaimId = Request.QueryString["CNO"].ToString();
                    ClaimId = ClaimId.base64Decode();
                    if (Convert.ToInt32(ClaimId.ToString()) > 0)
                    {
                        ViewState.Add("ClaimId", ClaimId.ToString());
                        loadDatafromClaimId(ClaimId);
                        txtSearch.Enabled = true;
                        isPreLoad = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "hideheaderRow", "$('.Headerrow').hide();", true);
                    }
                }
                catch { ScriptManager.RegisterStartupScript(this, typeof(Page), "hideheaderRow", "$('.Headerrow').hide();", true); }
            }
            else
            {
                if (ViewState["memberId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "hideheaderRowLoad", "$('.Headerrow').hide();", true);
                }
                else
                {
                    if (ViewState["memberId"].ToString() == string.Empty)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "hideheaderRowLoad", "$('.Headerrow').hide();", true);
                    }
                }
            }
            string claimTypesSelected = string.Empty;
            int len = claimTypesSelected.Length > 19 ? 19 : claimTypesSelected.Length;
            if (!IsPostBack)
            {
                if (isPreLoad)
                {
                    //PNL_ACT.Visible = false;
                    btnSave.Visible = false;
                    pnlPatientMobPurpose.Visible = false;
                }
            }
            int activeUserdId = 0;
            try
            {
                int.TryParse(ViewState["ActiveSession"].ToString(), out activeUserdId);
            }
            catch
            {
                activeUserdId = 0;
            }
            if (activeUserdId <= 0)
            {
                ViewState["ActiveSession"] = Session["ActionUserID"].ToString();
            }
            else
            {
                if (ViewState["ActiveSession"].ToString() != Session["ActionUserID"].ToString())
                {
                    FMCMsgBox.ShowMessage(Page, "Found another login in the same browser please logut another session");
                    return;
                }
            }
        }

        private void loadDatafromClaimId(string ClaimId)
        {
            DataTable dt_claimDetails = new ClsClaimBAL().GetClaimDetailsByClaimId(ClaimId);
            if (dt_claimDetails != null)
            {
                if (dt_claimDetails.Rows.Count > 0)
                {
                    int RefId = 0;
                    try
                    {
                        int.TryParse(Request.QueryString["refNo"].ToString(), out RefId);
                    }
                    catch { RefId = 0; }
                    string memberId = dt_claimDetails.Rows[0]["AuthorizationMemberID"].ToString();
                    if (RefId > 0)
                    {
                        Load_defaultFormData();
                        SearchPolicyDetails(memberId);//show member Details
                    }
                    else
                    {
                        SearchPolicyDetails(memberId);//show member Details
                        //showClaimDetails(dt_claimDetails);
                        //LoadDiagnoSis(ClaimId);
                        //LoadEncounter(ClaimId);
                        //LoadActivityDetails(ClaimId);
                        //LoadPrescriptionDetails(ClaimId);
                        txtSearch.Visible = false;
                        btnsearch.Visible = false;
                    }
                }
            }
        }

        //private void LoadPrescriptionDetails(string ClaimId)
        //{
        //    DataTable dt_Prescription = new ClsClaimBAL().GetPrescriptionDetailsbyPriorAuhId(ClaimId);
        //    if (dt_Prescription.Rows.Count > 0)
        //    {
        //        //BindPrescription();
        //        ViewState.Add("DtPrescription", dt_Prescription);
        //    }
        //    else
        //    {
        //    }
        //}

        //private void LoadActivityDetails(string ClaimId)
        //{
        //    int providerID = int.Parse(Session["EmpId"].ToString());
        //    Label lbl_active = dv_member.FindControl("LBL_CARDNO") as Label;
        //    clsClaimsDataEntryENT objClaobjPriorAuthENT = new clsClaimsDataEntryENT();
        //    objClaobjPriorAuthENT.DtActivities = new ClsClaimBAL().GetActivityDetailsbyPriorAuhIdWithDeductableTypes(ClaimId, providerID, lbl_active.Text.ToString());
        //    objClaobjPriorAuthENT.DtActivities.Columns.Remove("ApprovedGross");
        //    if (objClaobjPriorAuthENT.DtActivities.Rows.Count > 0)
        //    {
        //        objClaobjPriorAuthENT.DtActivities.AddEmptyRow();
        //        ViewState.Add("DtActivities", objClaobjPriorAuthENT.DtActivities);
        //        //showFooterTot(objClaobjPriorAuthENT.DtActivities);
        //    }
        //}

        //private void LoadEncounter(string ClaimId)
        //{
        //    DataTable dt_encounterGrid = new ClsClaimBAL().GetEncounterDetailsbyPriorAuhId(ClaimId);
        //    if (dt_encounterGrid.Rows.Count > 0)
        //    {
        //        if (dt_encounterGrid.Rows[0]["EncounterType"].ToString().ContainsAny("3", "4", "5", "6"))
        //        {
        //            ddlIp_Op.SelectedValue = "I";
        //        }
        //        else
        //        {
        //            ddlIp_Op.SelectedValue = "O";
        //        }
        //    }
        //}

        //private void LoadDiagnoSis(string ClaimId)
        //{
        //    DataTable DtDiagnosisDetails = new ClsClaimBAL().GetDignoSisDetailsbyPriorAuhId(ClaimId);
        //    if (DtDiagnosisDetails.Rows.Count > 0)
        //    {
        //        clsClaimsDataEntryENT objClaobjPriorAuthENT = new clsClaimsDataEntryENT();
        //        foreach (DataRow dtr in DtDiagnosisDetails.Rows)
        //        {
        //            int DiagnosisTypeID = (int)Enum.Parse(typeof(Enumeration.DiagnosisTypes), dtr["DiagnosisType"].ToString());
        //            objClaobjPriorAuthENT.DtDiagnosisDetails.Rows.Add(DiagnosisTypeID, dtr["DiagnosisType"].ToString(), dtr["DiagnosisCodeID"].ToString(), dtr["DiagnosisCode"].ToString(), dtr["DiagnosisDescription"].ToString());
        //        }
        //        objClaobjPriorAuthENT.DtDiagnosisDetails.AddEmptyRow();
        //        ViewState.Add("DtDiagnosisDetails", objClaobjPriorAuthENT.DtDiagnosisDetails);
        //    }

        //}

        //private void showClaimDetails(DataTable dt_claimDetails)
        //{
        //    string claimTypesSelected = string.Empty;
        //}

        private void Load_defaultFormData()
        {
            ViewState.Add("ClaimId", "0");
            BindEncounterData();
            BindDiagnoSis();
            BindActivity();
            CreateDrugTable();
            ClearClaim();
            bool AllowE_prescription = false;
            bool.TryParse(Session["is_Eprescription"].ToString(), out AllowE_prescription);
            if (!AllowE_prescription)
            {
            }
            else
            {
            }
            txtSearch.Text = string.Empty;
            txtSearch.Enabled = true;
        }

        private void BindActivity()
        {
            DataTable dt_ActivityType = new ClsClaimBAL().GetActivityTypes();
            ViewState.Add("dt_ActivityType", dt_ActivityType);
            int providerID = int.Parse(Session["EmpId"].ToString());
            DataTable dt_CliniansList = new ClsClaimBAL().GetClinicianbyProvideId(providerID);
            ViewState.Add("dt_CliniansList", dt_CliniansList);
            clsClaimsDataEntryENT objClaobjPriorAuthENT = new clsClaimsDataEntryENT();
            objClaobjPriorAuthENT.DtActivities.AddEmptyRow();
            ViewState.Add("DtActivities", objClaobjPriorAuthENT.DtActivities);
            if (objClaobjPriorAuthENT.DtActivities.Columns.IndexOf("Status") < 0)
            {
                objClaobjPriorAuthENT.DtActivities.Columns.Add("Status", typeof(string));
            }
            if (objClaobjPriorAuthENT.DtActivities.Columns.IndexOf("DenialDet") < 0)
            {
                objClaobjPriorAuthENT.DtActivities.Columns.Add("DenialDet", typeof(string));
            }

            //showStatusColum(false);
        }

        private int GetColumnIndexByName(GridView grid, string name)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (grid.Columns[i].HeaderText.ToLower().Trim() == name.ToLower().Trim())
                {
                    return i;
                }
            }

            return -1;
        }

        private void BindDiagnoSis()
        {
            clsClaimsDataEntryENT objClaobjPriorAuthENT = new clsClaimsDataEntryENT();
            objClaobjPriorAuthENT.DtDiagnosisDetails.AddEmptyRow();
            ViewState.Add("DtDiagnosisDetails", objClaobjPriorAuthENT.DtDiagnosisDetails);
        }

        private void BindEncounterData()
        {
            DataTable dt_encounterGrid = BindEncounter();
            dt_encounterGrid.Rows.Add(string.Empty, string.Empty, string.Empty, DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm"), DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm"), string.Empty, string.Empty);

        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            //btnHistory.Visible = false;

            string strCardId = txtSearch.Text.ToString();
            if (strCardId.Trim() != string.Empty)
            {
                SearchPolicyDetails(strCardId);//seacrh insured person details by card Number 
                txtSearch.Enabled = true;
                //if (ViewState("IsAllowDirectClaimFormPrint")==0)
                //if (int.TryParse(ViewState["IsAllowDirectClaimFormPrint"].ToString()) == 0)
                if (ViewState["IsAllowDirectClaimFormPrint"].ToString().Trim() == "0")
                {
                    btnSave.Visible = false;
                    pnlPatientMobPurpose.Visible = false;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "hideheaderRow", "$('.Headerrow').hide();", true);
            }
        }

        [Ext.Net.DirectMethod]
        public void SubmitToEligibilityConfirm(string remarks,string providerRemarks)
        {
            try
            {
                int ProviderID = Convert.ToInt32(Session["EmpId"].ToString());
                int memberId = int.Parse( ViewState["memberPKId"].ToString());
                int iActionUserID = Convert.ToInt32(Session["ActionUserID"].ToString());
                string Type = "RTS";
                int id = 0;
                try
                {
                   id= int.Parse(ViewState["ELGID"].ToString());
                }
                catch { id = 0; }
                new ClsClaimBAL().UpdateEligibilityData(ProviderID, memberId, iActionUserID, remarks, Type, providerRemarks, id);
            }
            catch { }
        }

        private void SearchPolicyDetails(string strCardId)
        {
            lblspanvipmember.InnerText = "";

            DataTable dtMemberRule = new ClsClaimBAL().SearchRule(strCardId);
            int ClaimId = 0;
            Boolean eligibilityLogged = false;
            int.TryParse(ViewState["ClaimId"].ToString(), out ClaimId);
            DataTable dt_InsuredPerson = new ClsClaimBAL().SearchPolicyDetails(strCardId);
            if (dtMemberRule.Rows.Count > 0 && ClaimId <= 0)
            {
                string WarningMessage = "";
                try
                {
                    WarningMessage = dtMemberRule.Rows[0]["WarningMsg"].ToString();
                }
                catch { WarningMessage = ""; }
                if (dtMemberRule.Rows[0]["WarningMsg"].ToString().Length > 0)
                {
                    FMCMsgBox.SwalMessageConfirmWithRemarks(Page, dtMemberRule.Rows[0]["WarningMsg"].ToString());
                    try
                    {
                        int ProviderID = Convert.ToInt32(Session["EmpId"].ToString());
                        int memberId = int.Parse(dt_InsuredPerson.Rows[0]["Member_ID"].ToString());
                        int iActionUserID = Convert.ToInt32(Session["ActionUserID"].ToString());
                        string Type = "RTS";

                        DataTable dtRetuID = new ClsClaimBAL().SaveEligibilityData(ProviderID, memberId, iActionUserID, dtMemberRule.Rows[0]["WarningMsg"].ToString(), Type, "");
                        if(dtRetuID.Rows.Count>0)
                        {
                            ViewState.Add("ELGID", dtRetuID.Rows[0][0].ToString());
                        }
                        eligibilityLogged = true;
                    }
                    catch { }
                }               
                if (dtMemberRule.Rows[0]["ErrorMessageShow"].ToString().Length > 0)
                {
                    FMCMsgBox.ShowMessage(Page, dtMemberRule.Rows[0]["ErrorMessageShow"].ToString());
                    //BindDeductable();
                    //HideClaimGrids();
                    return;
                }
            }
            
            ViewState.Add("dt_InsuredPerson_Copy", dt_InsuredPerson); 
            if (dt_InsuredPerson.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "hideheaderRowLoad", "", true);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "hideheaderRow", "$('.Headerrow').show();", true);
                dv_member.DataSource = dt_InsuredPerson;
                dv_member.DataBind();
                dv_member.Visible = true;
                txtSearch.Text = dt_InsuredPerson.Rows[0]["MemberCardNo"].ToString();
                //label to show member status     
                Label lbl_active = dv_member.FindControl("lbl_active") as Label;
                //show Policy Suspension details from Suspension_cancel Table if policy Suspended between Period
                int policyno = Convert.ToInt32(dt_InsuredPerson.Rows[0]["Policy_ID"]);
                ViewState.Add("Policy_ID", policyno);
                ViewState.Add("memberId", strCardId);
                DataTable dt_PolicySuspensionDetails = new DataTable();
                dt_PolicySuspensionDetails = new ClsClaimBAL().GetSuspensionDetailsByPolicyId(policyno);
                if (dt_PolicySuspensionDetails.Rows.Count > 0)
                {
                    lbl_active.Text = "Policy Suspended with effect from " + Convert.ToDateTime(dt_PolicySuspensionDetails.Rows[0]["START_DATE"].ToString()).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(dt_PolicySuspensionDetails.Rows[0]["end_DATE"].ToString()).ToString("dd/MM/yyyy") + "";
                    lbl_active.ForeColor = System.Drawing.Color.Red;
                    BindDeductable();
                    HideClaimGrids();
                    ViewState.Add("IsAllowDirectClaimFormPrint",0);
                }
                else
                {
                    if (Convert.ToDateTime(System.DateTime.Now) <= Convert.ToDateTime(dt_InsuredPerson.Rows[0]["POLICY_CANCEL_DATE"].ToString()) && Convert.ToDateTime(System.DateTime.Now) >= Convert.ToDateTime(dt_InsuredPerson.Rows[0]["POLICY_CREATE_DATE"].ToString()))
                    {
                        lbl_active.ForeColor = System.Drawing.Color.Green;
                        lbl_active.Text = "Member is eligible in your facility for medical services".ToUpper();
                        ShowClaimGrids();
                        ViewState.Add("IsAllowDirectClaimFormPrint", 1);
                    }
                    else if (dt_InsuredPerson.Rows[0]["STATUS"].ToString() == "MEMBER IS ACTIVE")
                    {
                        lbl_active.ForeColor = System.Drawing.Color.Green;
                        ShowClaimGrids();
                        ViewState.Add("IsAllowDirectClaimFormPrint", 1);
                    }
                    else
                    {
                        lbl_active.Text = "MEMBER IS INACTIVE";
                        lbl_active.ForeColor = System.Drawing.Color.Red;
                        HideClaimGrids();
                        ViewState.Add("IsAllowDirectClaimFormPrint", 0);
                    }
                }
                //show policy limts     
                int memberId = int.Parse(dt_InsuredPerson.Rows[0]["Member_ID"].ToString());
                ViewState.Add("memberPKId", memberId);
                if (memberId > 0)
                {
                    DataTable dtMemberVIPType = new ClsClaimBAL().GetMemberVIPStatus(memberId);
                    if (dtMemberVIPType.Rows.Count >= 1)
                    {
                        lblspanvipmember.InnerText = dtMemberVIPType.Rows[0]["MemberVipStatus"].ToString();
                        if (lblspanvipmember.InnerText=="VIP MEMBER")
                        {
                            dv_member.RowStyle.BackColor = System.Drawing.Color.Goldenrod;
                            dv_member.AlternatingRowStyle.BackColor = System.Drawing.Color.Goldenrod;
                            dv_member.Font.Bold = true;
                        }
                        else
                        {
                            dv_member.RowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#EFF3FB");
                            dv_member.AlternatingRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#EFF3FB");
                        }
                       
                        
                    }
                  

                    int ProviderID = Convert.ToInt32(Session["EmpId"].ToString());
                    int policyID = Convert.ToInt32(ViewState["Policy_ID"].ToString());
                    DataTable dtNetworkLink = new ClsClaimBAL().GetValidNetWorkLink(policyID, ProviderID);
                    int RefId = 0;
                    try
                    {
                        int.TryParse(Request.QueryString["refNo"].ToString(), out RefId);
                    }
                    catch { RefId = 0; }
                    if (RefId <= 0)
                    {
                        DataTable dt_referalClaim = new ClsClaimBAL().getReferalCalimDetailsbyPriorID(ClaimId); // generate Ref#
                        if (dt_referalClaim.Rows.Count > 0)
                        {
                            try
                            {
                                RefId = int.Parse(dt_referalClaim.Rows[0][0].ToString());
                            }
                            catch
                            {
                                RefId = 0;
                            }
                        }
                    }
                    if (RefId <= 0)
                    {
                        if (dtNetworkLink != null)
                        {
                            if (dtNetworkLink.Rows.Count > 0)
                            {
                                if (Convert.ToInt32(dtNetworkLink.Rows[0]["ValidOP"]) <= 0 && ddlIp_Op.SelectedValue.ToString() == "O")
                                {
                                    lbl_active.Text = "MEMBER IS NOT UNDER THESE NETWORK";
                                    lbl_active.ForeColor = System.Drawing.Color.Red;
                                    HideClaimGrids();
                                    ViewState.Add("IsAllowDirectClaimFormPrint", 0);
                                    return;
                                }
                                else if (Convert.ToInt32(dtNetworkLink.Rows[0]["ValidIP"]) <= 0 && ddlIp_Op.SelectedValue.ToString() == "I")
                                {
                                    lbl_active.Text = "MEMBER IS NOT UNDER THESE NETWORK";
                                    lbl_active.ForeColor = System.Drawing.Color.Red;
                                    HideClaimGrids();
                                    ViewState.Add("IsAllowDirectClaimFormPrint", 0);
                                    return;
                                }
                            }
                            else
                            {
                                lbl_active.Text = "MEMBER IS NOT UNDER THESE NETWORK";
                                lbl_active.ForeColor = System.Drawing.Color.Red;
                                HideClaimGrids();
                                ViewState.Add("IsAllowDirectClaimFormPrint", 0);
                                return;
                            }
                        }
                        else
                        {
                            lbl_active.Text = "MEMBER IS NOT UNDER THESE NETWORK";
                            lbl_active.ForeColor = System.Drawing.Color.Red;
                            HideClaimGrids();
                            ViewState.Add("IsAllowDirectClaimFormPrint", 0);
                            return;
                        }
                    }
                    DataTable dt_policyDeductable = new ClsClaimBAL().GetPolicyDeducatiableByPolicyID(ProviderID, policyno);
                    gdvPolicyDeductibles.DataSource = dt_policyDeductable;
                    gdvPolicyDeductibles.DataBind();

                    //show Benefit and Coverage
                    DataTable dt_Benefit_Coverage = new ClsClaimBAL().GetPolicyBenifitsByPolicyID(policyno, memberId);
                    gdvBenefit_Coverage.DataSource = dt_Benefit_Coverage;
                    gdvBenefit_Coverage.DataBind();

                    ViewState.Add("DtDCR_Deductible", dt_policyDeductable);
                    ViewState.Add("DtDCR_Benefit", dt_Benefit_Coverage);
                    
                    //EligibilityCheckLog
                    if (eligibilityLogged==false)
                    {
                        try
                        {
                            ProviderID = Convert.ToInt32(Session["EmpId"].ToString());
                            memberId = int.Parse(dt_InsuredPerson.Rows[0]["Member_ID"].ToString());
                            int iActionUserID = Convert.ToInt32(Session["ActionUserID"].ToString());
                            string Type = "RTS";
                            DataTable dtRetuID = new ClsClaimBAL().SaveEligibilityData(ProviderID, memberId, iActionUserID, "Eligibility Checking", Type, "");
                            if (dtRetuID.Rows.Count > 0)
                            {
                                ViewState.Add("ELGID", dtRetuID.Rows[0][0].ToString());
                            }
                        }
                        catch { }
                    }

                    DataTable dt_RTSDirectClaimRefNo = new ClsClaimBAL().GetGenerated_RTSDirectClaimRefNo(ProviderID, Convert.ToInt32(Session["ActionUserID"].ToString()), memberId, lbl_active.Text.ToString().Trim(), txtPatientMobNo.Text.ToString().Trim(), string.Empty, txtVisitOtherPurposeRemark.Text.ToString().Trim());
                    ViewState.Add("DtRTSDirectClaimRefNo", dt_RTSDirectClaimRefNo);
                    //int providerID = int.Parse(Session["EmpId"].ToString());
                    //DataTable dtPTH = new ClsReportBAL().getPatientHistoryByCardno(strCardId.ToString(), providerID);
                    //if (dtPTH.Rows.Count > 0)
                    //{
                    //    string url1 = "/ReportsPage/rptPatientHistory.aspx?CRDNO=" + strCardId.ToString() + "";
                    //}
                    //else
                    //{
                    //}
                }
            }
            else
            {
                ViewState.Add("memberId", string.Empty);
                txtSearch.Text = string.Empty;
                Load_defaultFormData();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "hideheaderRow", "$('.Headerrow').hide();", true);
                txtSearch.Focus();
                ViewState.Add("IsAllowDirectClaimFormPrint", 0);
            }
        }

        private void ShowClaimGrids()
        {
            //PNL_ACT.Visible = true;
            bool AllowE_prescription = false;
            bool.TryParse(Session["is_Eprescription"].ToString(), out AllowE_prescription);
            if (!AllowE_prescription)
            {

            }
            else
            {

            }
            btnSave.Visible = true;
            pnlPatientMobPurpose.Visible = true;
        }

        private void HideClaimGrids()
        {
            //PNL_ACT.Visible = false;
            btnSave.Visible = false;
            pnlPatientMobPurpose.Visible = false;
        }
        //Show Patient Share for the policy/Plan he hold
        private void BindDeductable()
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                if ((txtPatientMobNo.Text.ToString().Trim().Length <= 0) || (txtPatientMobNo.Text.ToString().Trim() == string.Empty))
                {
                    FMCMsgBox.ShowMessage(Page, "Enter Patient Mobile Number");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PopupMessage", "Ext.net.Mask.hide();", true);
                    return;
                }
                //CheckBoxList list = new CheckBoxList();
                if (chlVisitPurpose.SelectedIndex == -1)
                {
                    FMCMsgBox.ShowMessage(Page, "Select Visit Purpose");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PopupMessage", "Ext.net.Mask.hide();", true);
                    return;
                }

                string sVisitPurpose = "";
                for (int i = 0; i < chlVisitPurpose.Items.Count ; i++)
                {
                    if (chlVisitPurpose.Items[i].Selected)
                    {
                        if (sVisitPurpose.Trim().Length >= 1)
                        {
                            sVisitPurpose = sVisitPurpose + ", " + chlVisitPurpose.Items[i].Text.Trim();
                        }
                        else { sVisitPurpose = chlVisitPurpose.Items[i].Text.Trim(); }

                        if (chlVisitPurpose.Items[i].Value.Trim() == "5") //0-Select, 1- doctor consultation , 2- physiotherapy session , 3- other multi- session treatment like injections, nebulization ...), 4- lab or radiology investigations , 5- others (please specify the reason/s) 
                        {
                            if ((txtVisitOtherPurposeRemark.Text.ToString().Trim().Length <= 0) || (txtVisitOtherPurposeRemark.Text.ToString().Trim() == string.Empty))
                            {
                                FMCMsgBox.ShowMessage(Page, "Specify the reason/s in Remark");
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PopupMessage", "Ext.net.Mask.hide();", true);
                                return;
                            }
                            //else
                            //{
                            //    sMsg = sMsg + "( " + txtVisitOtherPurposeRemark.Text.ToString().Trim() + " )";
                            //}
                        }
                    }

                }

                
                //if (ddlVisitPurpose.SelectedValue == "0") //0-Select, 1- doctor consultation , 2- physiotherapy session , 3- other multi- session treatment like injections, nebulization ...), 4- lab or radiology investigations , 5- others (please specify the reason/s) 
                //{
                //    FMCMsgBox.ShowMessage(Page, "Select Patient Visit Purpose");
                //    return;
                //}
                //if (ddlVisitPurpose.SelectedValue == "5") //0-Select, 1- doctor consultation , 2- physiotherapy session , 3- other multi- session treatment like injections, nebulization ...), 4- lab or radiology investigations , 5- others (please specify the reason/s) 
                //{
                //    if ((txtVisitOtherPurposeRemark.Text.ToString().Trim().Length <= 0) || (txtVisitOtherPurposeRemark.Text.ToString().Trim() == string.Empty))
                //    {
                //        FMCMsgBox.ShowMessage(Page, "Enter Patient Visit Purpose Remark");
                //        return;
                //    }
                //}




                DataTable dt1 = new DataTable();
                dt1 = ViewState["dt_InsuredPerson_Copy"] as DataTable;
                int iProvider_ID = Convert.ToInt32(Session["EmpId"].ToString());
                string sProviderName = Session["LogInProviderName"].ToString();
                int iActionUserID = Convert.ToInt32(Session["ActionUserID"].ToString());
                int iMember_ID = int.Parse(dt1.Rows[0]["Member_ID"].ToString());
                Label lbl_active = dv_member.FindControl("lbl_active") as Label;
                DataTable dtDCR_Deductible = ViewState["DtDCR_Deductible"] as DataTable;
                DataTable dtDCR_Benefit = ViewState["DtDCR_Benefit"] as DataTable;

                DataTable dt_RTSDirectClaimRefNo = new ClsClaimBAL().GetGenerated_RTSDirectClaimRefNo(iProvider_ID, iActionUserID, iMember_ID, lbl_active.Text.ToString().Trim(), txtPatientMobNo.Text.ToString().Trim(), sVisitPurpose.ToString().Trim(), txtVisitOtherPurposeRemark.Text.ToString().Trim());
                if (dt_RTSDirectClaimRefNo.Rows.Count > 0 && dtDCR_Deductible.Rows.Count > 0 && dtDCR_Benefit.Rows.Count > 0)
                {
                    var report = new LocalReport();
                    var vardtRTSDirectClaimRefNo = new ReportDataSource("dset_DirectClaimForm_Member", dt_RTSDirectClaimRefNo);
                    var vardtDCR_Deductible = new ReportDataSource("dset_DtDCR_Deductible", dtDCR_Deductible);
                    var varDtDCR_Benefit = new ReportDataSource("dset_DtDCR_Benefit", dtDCR_Benefit);

                    report.ReportPath = "Reports/rpt_PrintDirectClaimForm.rdlc";
                    report.DataSources.Add(vardtRTSDirectClaimRefNo);
                    report.DataSources.Add(vardtDCR_Deductible);
                    report.DataSources.Add(varDtDCR_Benefit);
                    // printPdf(report); 
                    Utility.printPdf(report, frmPrint, "ReportPage/"
                        );
                    string openCommand = "window.open('../PrintPreview.aspx?fpath=" + frmPrint.Src.ToString() + "','popup','Height: 450px; Width: 600px; edge: Raised; center: Yes; resizable: yes; status: Yes; scroll:Yes;modal:yes');Ext.net.Mask.hide();";

                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PopupMessage", openCommand, true);
                }                
            }
            catch (Exception ex)
            {
                Logging.LogException(ex);
                FMCMsgBox.ShowMessage(Page, "Error Occured While Printing try again later..");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PopupMessage", "Ext.net.Mask.hide();", true);
                return;
            }
            
        }

        

        //public static void ExportDataTableToPDF(DataTable dt, string filePath, string reportTitle, HttpResponse response)
        //{
        //    Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);

        //    try
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
                    
        //            PdfWriter.GetInstance(document, ms);
        //            document.Open();

        //            iTextSharp.text.Font font = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        //            PdfPTable pdfTable = new PdfPTable(dt.Columns.Count);
        //            pdfTable.WidthPercentage = 100;

        //            // Add column headers
        //            foreach (DataColumn column in dt.Columns)
        //            {
        //                PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, font));
        //                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                pdfTable.AddCell(cell);
        //            }

        //            // Add data rows
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                foreach (var item in row.ItemArray)
        //                {
        //                    pdfTable.AddCell(new Phrase(item.ToString(), font));
        //                }
        //            }

        //            document.Add(pdfTable);
        //            document.Close();
                    
        //            byte[] bytes = ms.ToArray();
        //            string base64String = Convert.ToBase64String(bytes);

        //            // Send PDF to browser
        //            response.Clear();
        //            response.ContentType = "application/pdf";
        //            response.AddHeader("Content-Disposition", "attachment; filename=MyDataTable.pdf");
        //            response.OutputStream.Write(bytes, 0, bytes.Length);
        //            response.Flush();
        //            HttpContext.Current.ApplicationInstance.CompleteRequest();
        //            //response.End();
        //        }
        //        //// 2. Create a PDF writer to write to the file
        //        //PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

        //        //// 3. Open the document to enable writing
        //        //document.Open();

        //        //// 4. Add title to the PDF
        //        //var titleFont = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD);
        //        //Paragraph title = new Paragraph(reportTitle + "\n\n", titleFont);
        //        //title.Alignment = Element.ALIGN_CENTER;
        //        //document.Add(title);

        //        //// 5. Create a PdfPTable with the number of columns in DataTable
        //        //PdfPTable pdfTable = new PdfPTable(dt.Columns.Count);
        //        //pdfTable.WidthPercentage = 100;

        //        //// 6. Add headers
        //        //foreach (DataColumn column in dt.Columns)
        //        //{
        //        //    PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
        //        //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        //    pdfTable.AddCell(cell);
        //        //}

        //        //// 7. Add rows
        //        //foreach (DataRow row in dt.Rows)
        //        //{
        //        //    foreach (var item in row.ItemArray)
        //        //    {
        //        //        pdfTable.AddCell(new Phrase(item.ToString(), FontFactory.GetFont("Arial", 10)));
        //        //    }
        //        //}

        //        //// 8. Add table to document
        //        //document.Add(pdfTable);
        //        //response.Clear();
        //        //response.ContentType = "application/pdf";
        //        //response.AddHeader("Content-Disposition", "attachment; filename=report1.pdf");
        //        //response.OutputStream.Write(ms.ToArray(), 0, ms.ToArray().Length);
        //        //response.Flush();
        //        //response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error generating PDF: " + ex.Message);
        //    }
        //    finally
        //    {
        //        // 9. Close the document
        //        document.Close();
        //    }
        //}

        private void ExportReportToPDF(DataTable dt, string reportPath)
        {
            // Create a LocalReport object
            LocalReport localReport = new LocalReport();

            // Load the RDLC file
            localReport.ReportPath = reportPath;// Server.MapPath("~/Reports/Report1.rdlc");

            // Set the data source
            //DataTable dt = GetReportData(); // Your method to fetch data
            DataTable dtDCR_Deductible = ViewState["DtDCR_Deductible"] as DataTable;
            DataTable dtDCR_Benefit = ViewState["DtDCR_Benefit"] as DataTable;

           // ReportDataSource rds = new ReportDataSource("DataSet1", dt); // "DataSet1" must match RDLC data source name
            var vardtRTSDirectClaimRefNo = new ReportDataSource("dset_DirectClaimForm_Member", dt);
            var vardtDCR_Deductible = new ReportDataSource("dset_DtDCR_Deductible", dtDCR_Deductible);
            var varDtDCR_Benefit = new ReportDataSource("dset_DtDCR_Benefit", dtDCR_Benefit);
            localReport.DataSources.Clear();
            localReport.DataSources.Add(vardtRTSDirectClaimRefNo);
            localReport.DataSources.Add(vardtDCR_Deductible);
            localReport.DataSources.Add(varDtDCR_Benefit);

            localReport.Refresh();

            // Render the report
            string mimeType, encoding, fileNameExtension;
            string[] streams;
            Warning[] warnings;

            byte[] bytes = localReport.Render(
                "PDF", null, out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);

            // Send the file to the browser
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("Content-Disposition", "attachment; filename=Report.pdf");
            Response.BinaryWrite(bytes);
            Response.End();

            // Render the report

            //Utility.printPdf(localReport, frmPrint, "ReportPage/"
            //            );
            //string openCommand = "window.open('../PrintPreview.aspx?fpath=" + frmPrint.Src.ToString() + "','popup','Height: 450px; Width: 600px; edge: Raised; center: Yes; resizable: yes; status: Yes; scroll:Yes;modal:yes');Ext.net.Mask.hide();";

            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PopupMessage", openCommand, true);

           
        }

        private void ClearClaim()
        {
            txtSearch.Text = string.Empty;
            dv_member.DataSource = null;
            dv_member.DataBind();
            dv_member.Visible = false;
        }

        private void clearPage()
        {
            Response.Redirect("/Claim/ClncEncounter.aspx");
            return;
        }

        private DataTable BindEncounter()
        {
            DataTable dt_encounterGrid = new DataTable();
            dt_encounterGrid.Columns.Add("EncounterType", typeof(string));
            dt_encounterGrid.Columns.Add("starttype", typeof(string));
            dt_encounterGrid.Columns.Add("endtype", typeof(string));
            dt_encounterGrid.Columns.Add("St_date", typeof(string));
            dt_encounterGrid.Columns.Add("st_time", typeof(string));
            dt_encounterGrid.Columns.Add("end_date", typeof(string));
            dt_encounterGrid.Columns.Add("end_time", typeof(string));
            dt_encounterGrid.Columns.Add("tr_Source", typeof(string));
            dt_encounterGrid.Columns.Add("tr_Destination", typeof(string));
            return dt_encounterGrid;
        }
        private DataTable CreatePriorAuthorization_Encounter()
        {
            DataTable dt_PriorAuthorization_Encounter = new DataTable();
            dt_PriorAuthorization_Encounter.Columns.Add("EncounterType", typeof(int));
            dt_PriorAuthorization_Encounter.Columns.Add("EncounterStartType", typeof(int));
            dt_PriorAuthorization_Encounter.Columns.Add("EncounterEndType", typeof(int));
            dt_PriorAuthorization_Encounter.Columns.Add("EncounterStartDate", typeof(DateTime));
            dt_PriorAuthorization_Encounter.Columns.Add("EncounterEndDate", typeof(DateTime));
            dt_PriorAuthorization_Encounter.Columns.Add("TransferSource", typeof(string));
            dt_PriorAuthorization_Encounter.Columns.Add("TransferDestination", typeof(string));
            return dt_PriorAuthorization_Encounter;
        }

        protected void gdvDiagnosis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlDiagnosisyType = e.Row.FindControl("ddlDiagnosisType") as DropDownList;
                if (ddlDiagnosisyType != null)
                {
                    ddlDiagnosisyType.DataSource = EnumHelper.GetAll<Enumeration.DiagnosisTypes>();
                    ddlDiagnosisyType.DataTextField = "Value";
                    ddlDiagnosisyType.DataValueField = "key";
                    ddlDiagnosisyType.DataBind();
                    ddlDiagnosisyType.Attributes.Add("onchange", "return CheckDuplicatePrimaryDiag(this);");
                }
                Label lblActivityTypeId = e.Row.FindControl("lblApplicable_DiagnosisTypeID") as Label;
                if (lblActivityTypeId != null)
                {
                    try
                    {
                        ddlDiagnosisyType.SelectedValue = lblActivityTypeId.Text.ToString().Trim();
                    }
                    catch (Exception ex)
                    { Logging.LogException(ex); }
                    if (e.Row.RowIndex > 0 && lblActivityTypeId.Text.ToString().Trim() == "")
                    {
                        ddlDiagnosisyType.SelectedIndex = 1;
                    }
                }
                try
                {
                }
                catch { }
            }
        }

        

        protected void gdvEncounter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList CMB_ENCOUNTERTYPE = e.Row.FindControl("CMB_ENCOUNTERTYPE") as DropDownList;
                if (CMB_ENCOUNTERTYPE != null)
                {
                    DataSet dsEncounter = new DataSet();
                    Label lblApplicable_EncounterType = e.Row.FindControl("lblApplicable_EncounterType") as Label;
                    Label lblApplicable_starttype = e.Row.FindControl("lblApplicable_starttype") as Label;
                    Label lblApplicable_endtype = e.Row.FindControl("lblApplicable_endtype") as Label;
                    dsEncounter = new ClsClaimBAL().GetEncounterMasterDetails();
                    DataRow drEncounterType = dsEncounter.Tables["MS_Encounter"].NewRow();
                    drEncounterType["EncounterName"] = EnumHelper.GetDescription(Enumeration.DefaultSelectCaption.Select);
                    dsEncounter.Tables["MS_Encounter"].Rows.InsertAt(drEncounterType, 0);
                    CMB_ENCOUNTERTYPE.DataValueField = "EncounterType";
                    CMB_ENCOUNTERTYPE.DataTextField = "EncounterName";
                    if (ddlIp_Op.SelectedValue.ToString() == "I")
                    {
                        CMB_ENCOUNTERTYPE.DataSource = dsEncounter.Tables["MS_Encounter"].AsEnumerable().Where(x => chekOPIPTypes(x, "IP")).CopyToDataTable();
                    }
                    else
                    {
                        CMB_ENCOUNTERTYPE.DataSource = dsEncounter.Tables["MS_Encounter"].AsEnumerable().Where(x => chekOPIPTypes(x, "OP")).CopyToDataTable();
                    }
                    CMB_ENCOUNTERTYPE.DataBind();
                    if (lblApplicable_EncounterType != null)
                    {
                        if (lblApplicable_EncounterType.Text.ToString() == string.Empty)
                        {
                            CMB_ENCOUNTERTYPE.SelectedIndex = 1;
                        }
                        else
                        {
                            CMB_ENCOUNTERTYPE.SelectedValue = lblApplicable_EncounterType.Text.ToString();
                        }
                    }
                    else
                    {
                        CMB_ENCOUNTERTYPE.SelectedIndex = 1;
                    }
                    DropDownList cmb_starttype = e.Row.FindControl("cmb_starttype") as DropDownList;
                    //bind Encounter StartType            
                    if (cmb_starttype != null)
                    {
                        DataRow drEncounterStartType = dsEncounter.Tables["MS_EncounterStartType"].NewRow();
                        drEncounterStartType["EncounterStartTypeName"] = EnumHelper.GetDescription(Enumeration.DefaultSelectCaption.Select);
                        dsEncounter.Tables["MS_EncounterStartType"].Rows.InsertAt(drEncounterStartType, 0);
                        cmb_starttype.DataValueField = "EncounterStartType";
                        cmb_starttype.DataTextField = "EncounterStartTypeName";
                        cmb_starttype.DataSource = dsEncounter.Tables["MS_EncounterStartType"];
                        cmb_starttype.DataBind();
                        if (lblApplicable_starttype != null)
                        {
                            if (lblApplicable_starttype.Text.ToString() == string.Empty)
                            {
                                cmb_starttype.SelectedIndex = 1;
                            }
                            else
                            {
                                cmb_starttype.SelectedValue = lblApplicable_starttype.Text.ToString();
                            }
                        }
                    }
                    //bind Encounter EndType   
                    DropDownList cmb_endtype = e.Row.FindControl("cmb_endtype") as DropDownList;
                    if (cmb_starttype != null)
                    {
                        DataRow drEncounterEndType = dsEncounter.Tables["MS_EncounterEndType"].NewRow();
                        drEncounterEndType["EncounterEndTypeName"] = EnumHelper.GetDescription(Enumeration.DefaultSelectCaption.Select);
                        dsEncounter.Tables["MS_EncounterEndType"].Rows.InsertAt(drEncounterEndType, 0);
                        cmb_endtype.DataValueField = "EncounterEndType";
                        cmb_endtype.DataTextField = "EncounterEndTypeName";
                        cmb_endtype.DataSource = dsEncounter.Tables["MS_EncounterEndType"];
                        cmb_endtype.DataBind();
                        if (lblApplicable_endtype != null)
                        {
                            if (lblApplicable_endtype.Text.ToString() == string.Empty)
                            {
                                cmb_endtype.SelectedIndex = 1;
                            }
                            else
                            {
                                cmb_endtype.SelectedValue = lblApplicable_endtype.Text.ToString();
                            }
                        }
                    }
                }
            }
        }

        private bool chekOPIPTypes(DataRow x, string typ)
        {
            string strVal = string.Empty;
            try
            {
                strVal = Convert.ToInt32(x.Field<string>("EncounterType")).ToString();
            }
            catch
            {
                try
                {
                    strVal = Convert.ToInt32(x.Field<int>("EncounterType")).ToString();
                }
                catch
                {
                    strVal = Convert.ToInt32(x.Field<byte>("EncounterType")).ToString();
                }
            }
            if (typ == "OP")
            {
                return !strVal.ContainsAny("3", "4", "5", "6");
            }
            else
            {
                return strVal.ContainsAny("3", "4", "5", "6");
            }
        }

        protected void CMB_ENCOUNTERTYPE_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void cmb_starttype_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList cmb_starttype = sender as DropDownList;
            if (cmb_starttype != null)
            {
                //*DRS
                //if (cmb_starttype.SelectedValue.ToString() == "3")
                //{
                //    gdvEncounter.Columns[8].Visible = true;
                //}
                //else
                //{
                //    gdvEncounter.Columns[8].Visible = false;
                //}
                //gdvEncounter.Rows[0].FindControl("cmb_endtype").Focus();
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Focus",
                //"GotoFocus('" +
                //gdvEncounter.Rows[0].FindControl("cmb_endtype").ClientID +
                //"')", true);
            }
        }

        protected void cmb_endtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList cmb_endtype = sender as DropDownList;
            if (cmb_endtype != null)
            {
                //*DRS
                //if (cmb_endtype.SelectedValue.ToString() == "4")
                //{
                //    gdvEncounter.Columns[9].Visible = true;
                //}
                //else
                //{
                //    gdvEncounter.Columns[9].Visible = false;
                //}
                //gdvEncounter.Rows[0].FindControl("txt_endencounter").Focus();
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Focus",
                //"GotoFocus('" +
                //gdvEncounter.Rows[0].FindControl("txt_endencounter").ClientID +
                //"')", true);
            }
        }

        protected void DRP_QUANTITY_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowIndex = ((sender as DropDownList).Parent.Parent as GridViewRow).RowIndex;
            //addActivitytoGrid(rowIndex);
        }

       

       

        protected void BTN_ADDRUG_Click(object sender, EventArgs e)
        {
            
        }

        private bool checkCodeExist(DataRow x, string CODE)
        {
            string drugCode = x[0].ToString();
            return drugCode.ToString() == CODE.ToString();
        }

       

        private void CreateDrugTable()
        {
            DataTable dt_Prescription = new DataTable();
            dt_Prescription.Columns.Add("Code", typeof(string));
            dt_Prescription.Columns.Add("TYPE", typeof(string));
            dt_Prescription.Columns.Add("GENERIC_NAME", typeof(string));
            dt_Prescription.Columns.Add("NAME", typeof(string));
            dt_Prescription.Columns.Add("UNIT", typeof(string));//unit per freequency
            dt_Prescription.Columns.Add("FREQUENCY", typeof(string));//FrequencyValue
            dt_Prescription.Columns.Add("FREQUENCY_TYPE", typeof(string));  //FrequencyType          
            dt_Prescription.Columns.Add("DURATION", typeof(string));//Duration
            dt_Prescription.Columns.Add("QUANTITY", typeof(string));//Quatity
            dt_Prescription.Columns.Add("REFILLS", typeof(string));//refills
            dt_Prescription.Columns.Add("ROA_CODE", typeof(string));//ROA_Id
            dt_Prescription.Columns.Add("ROA", typeof(string));
            dt_Prescription.Columns.Add("INSTRUCTIONS", typeof(string));//Instructions
            dt_Prescription.Columns.Add("DosageForm_Id", typeof(string));//UseDosageForm_Id            
            dt_Prescription.Columns.Add("Status", typeof(string));
            dt_Prescription.Columns.Add("DenialDet", typeof(string));
            ViewState.Add("DtPrescription", dt_Prescription);
        }

       

       

        protected void btnDelPrescription_Click(object sender, ImageClickEventArgs e)
        {
            var dtGridData = ViewState["DtPrescription"] as DataTable;
            var ImgbtnDelete = sender as ImageButton;
            var gdvTempRow = ImgbtnDelete.Parent.Parent as GridViewRow;
            int Rowindex = gdvTempRow.RowIndex;
            if (dtGridData.Rows[Rowindex]["NAME"].ToString() != string.Empty)
            {
                dtGridData.Rows.RemoveAt(Rowindex);
                ViewState.Add("DtPrescription", dtGridData);
                //*DRS
                //gdvPrescription.DataSource = dtGridData;
                //gdvPrescription.DataBind();
            }
        }

        protected void gdvPrescription_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblApplicable_txtActivityStatus = e.Row.FindControl("txtActivityStatus") as Label;
                if (lblApplicable_txtActivityStatus != null)
                {
                    if (lblApplicable_txtActivityStatus.Text.ToString().Trim() == "Approved")
                    {
                        lblApplicable_txtActivityStatus.ForeColor = System.Drawing.Color.Green;
                        lblApplicable_txtActivityStatus.Font.Bold = true;
                    }
                    else if (lblApplicable_txtActivityStatus.Text.ToString().Trim() == "Rejected")
                    {
                        lblApplicable_txtActivityStatus.ForeColor = System.Drawing.Color.Red;
                        lblApplicable_txtActivityStatus.Font.Bold = true;
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Load_defaultFormData();
            ScriptManager.RegisterStartupScript(this, typeof(Page), "hideheaderRow", "$('.Headerrow').hide();", true);
            try
            {
                string ClaimId = Request.QueryString["CNO"].ToString();
                ClaimId = ClaimId.base64Decode();
                if (Convert.ToInt32(ClaimId.ToString()) > 0)
                {
                    Response.Redirect("~/Claim/ClncEncounter.aspx");
                }
            }
            catch
            {
                HideClaimGrids();
            }
        }

        protected void ddlIp_Op_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEncounterData();
            //var dtGridData = ViewState["DtActivities"] as DataTable;
            //gdvActivity.DataSource = dtGridData;
            //gdvActivity.DataBind();
        }

        protected void btn_upload_Click(object sender, EventArgs e)
        {
           
        }

        private void BindAttachmentList(int claimId)
        {
            int dayofActivityLimit = 0;//used for checking upto how many days user can remove the attached documents
            int.TryParse(ConfigurationManager.AppSettings["AttachmentDayLimit"].ToString(), out dayofActivityLimit);
            DataTable dt_AttachmentList = new ClsClaimBAL().getAttachmentFilesByAuhID(claimId, dayofActivityLimit);
            if (dt_AttachmentList.Rows.Count > 0)
            {
                //gv_attachment.DataSource = dt_AttachmentList;
                //gv_attachment.DataBind();
            }
        }

        private string GeneratePdfBase64()
        {
            DataTable dt1 = new DataTable();
            dt1 = ViewState["dt_InsuredPerson_Copy"] as DataTable;
            int iProvider_ID = Convert.ToInt32(Session["EmpId"].ToString());
            string sProviderName = Session["LogInProviderName"].ToString();
            int iActionUserID = Convert.ToInt32(Session["ActionUserID"].ToString());
            int iMember_ID = int.Parse(dt1.Rows[0]["Member_ID"].ToString());
            Label lbl_active = dv_member.FindControl("lbl_active") as Label;
            DataTable dtDCR_Deductible = ViewState["DtDCR_Deductible"] as DataTable;
            DataTable dtDCR_Benefit = ViewState["DtDCR_Benefit"] as DataTable;
            DataTable dt_RTSDirectClaimRefNo = ViewState["DtRTSDirectClaimRefNo"] as DataTable;
            string sVisitPurpose = string.Empty;
            sVisitPurpose = "claimsFormReport download";
            //DataTable dt_RTSDirectClaimRefNo = new ClsClaimBAL().GetGenerated_RTSDirectClaimRefNo(iProvider_ID, iActionUserID, iMember_ID, lbl_active.Text.ToString().Trim(), txtPatientMobNo.Text.ToString().Trim(), sVisitPurpose.ToString().Trim(), txtVisitOtherPurposeRemark.Text.ToString().Trim());

            string reportPath = Server.MapPath("~/Reports/rpt_DirectClaimReImburseFormDownload.rdlc");
            //rpt_DirectClaimReImburseForm
            // Create a LocalReport object
            LocalReport localReport = new LocalReport();

            // Load the RDLC file
            localReport.ReportPath = reportPath;


            var vardtRTSDirectClaimRefNo = new ReportDataSource("dset_DirectClaimForm_Member", dt_RTSDirectClaimRefNo);
            var vardtDCR_Deductible = new ReportDataSource("dset_DtDCR_Deductible", dtDCR_Deductible);
            var varDtDCR_Benefit = new ReportDataSource("dset_DtDCR_Benefit", dtDCR_Benefit);
            localReport.DataSources.Clear();
            localReport.DataSources.Add(vardtRTSDirectClaimRefNo);
            localReport.DataSources.Add(vardtDCR_Deductible);
            localReport.DataSources.Add(varDtDCR_Benefit);
            localReport.Refresh();

            // Render the report
            string mimeType, encoding, fileNameExtension;
            string[] streams;
            Warning[] warnings;

            byte[] pdfBytes = localReport.Render(
                "PDF", null, out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);

            return Convert.ToBase64String(pdfBytes);
        }

        protected void chkClaimLink_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (chkClaimLink.Checked)
                {
                    // Generate base64 PDF
                    string base64 = GeneratePdfBase64();
                    string fileName = "ClaimForm.pdf";

                    // Build the data URI link
                    string dataUri = "data:application/pdf;base64," + base64;
                    string anchorHtml = "<a id='downloadClaimLink' href='" + dataUri + "'download='" + fileName + "' style='display:inline;'>Download Claim Form</a>";

                    litDownloadClaimLink.Text = anchorHtml;
                    litDownloadClaimLink.Visible = true;
                }
                else
                {
                    litDownloadClaimLink.Visible = false;
                    litDownloadClaimLink.Text = "";
                }
                
            }
            catch (Exception ex)
            {
                Logging.LogException(ex);
                FMCMsgBox.ShowMessage(Page, "Error Occured While downloading report try again later..");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PopupMessage", "Ext.net.Mask.hide();", true);
                return;
            }
        }

        protected void chkClaimReImburseLink_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (chkClaimReImburseLink.Checked)
                {
                    var formData = GetFormDataFromRequest();
                    string base64 = GenerateMedicalClaimForm(formData);

                    //// Generate base64 PDF RDLC report
                    //string base64 = GeneratePdfBase64();
                    string fileName = "rpt_DirectClaimFormDownload.pdf";

                    //// Build the data URI link
                    string dataUri = "data:application/pdf;base64," + base64;
                    string anchorHtml = "<a id='downloadClaimReImburseLink' href='" + dataUri + "'download='" + fileName + "' style='display:inline;'>Download Reimbursement Claim Form</a>";

                    litDownloadClaimReImburseLink.Text = anchorHtml;
                    litDownloadClaimReImburseLink.Visible = true;
                }
                else
                {
                    litDownloadClaimReImburseLink.Visible = false;
                    litDownloadClaimReImburseLink.Text = "";
                }
                
            }
            catch (Exception ex)
            {
                Logging.LogException(ex);
                FMCMsgBox.ShowMessage(Page, "Error Occured While downloading report try again later..");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PopupMessage", "Ext.net.Mask.hide();", true);
                return;
            }
        }



       ///========================================================================================================
       ///PDF itextsharp report
       ///========================================================================================================

        private MedicalClaimFormData GetFormDataFromRequest()
        {
            // Create form data from query parameters or form submission
            var formData = new MedicalClaimFormData();

            // Try to get data from request parameters
            formData.Date = Request.QueryString["date"] ?? DateTime.Now.ToString("dd/MM/yyyy");
            formData.ClinicName = Request.QueryString["clinicName"] ?? "Emirates Medical Center";
            formData.CardHolderName = Request.QueryString["cardHolderName"] ?? "AHMED AL MAHMOUD";

            // Parse age safely
            formData.Age = 35; // Default value
            formData.Emirates = "Dubai"; // Default value
            formData.Sex = Request.QueryString["sex"] ?? "M";
            formData.TelNo = Request.QueryString["telNo"] ?? "04-3456789";
            formData.MobileNo = Request.QueryString["mobileNo"] ?? "050-1234567";
            formData.InsCardNo = Request.QueryString["insCardNo"] ?? "EMC123456789";
            formData.ValidUpTo = Request.QueryString["validUpTo"] ?? "31/12/2025";
            formData.CompanyName = Request.QueryString["companyName"] ?? "ABC TRADING LLC";
            formData.EmployeeNo = Request.QueryString["employeeNo"] ?? "EMP001";
            formData.Nationality = Request.QueryString["nationality"] ?? "UAE";
            formData.SignatureDate = Request.QueryString["signatureDate"] ?? DateTime.Now.ToString("dd/MM/yyyy");

            // Clinical details
            formData.Temperature = Request.QueryString["temperature"] ?? "37.2";
            formData.BloodPressure = Request.QueryString["bloodPressure"] ?? "120/80";
            formData.Pulse = Request.QueryString["pulse"] ?? "72";
            formData.SignsSymptoms = Request.QueryString["signsSymptoms"] ?? "Fever, headache, body ache";
            formData.OnsetDate = Request.QueryString["onsetDate"] ?? "15/01/2025";
            formData.Diagnosis = Request.QueryString["diagnosis"] ?? "Viral fever with associated symptoms";
            formData.DoctorNameAndSignature = Request.QueryString["doctorName"] ?? "Dr. Sarah Ahmed";
            formData.DiagnosticProcedures = Request.QueryString["diagnosticProcedures"] ?? "";

            // Parse service types from comma-separated string
            string serviceTypes = Request.QueryString["serviceTypes"] ?? "CLINIC";
            formData.ServiceTypes = serviceTypes.Split(',').Where(s => !string.IsNullOrEmpty(s)).ToList();

            // Parse visit types from comma-separated string
            string visitTypes = Request.QueryString["visitTypes"] ?? "New visit";
            formData.VisitTypes = visitTypes.Split(',').Where(v => !string.IsNullOrEmpty(v)).ToList();

            // Parse pharmaceuticals from request (simple format)
            var lstPharmaceuticalItem = new List<PharmaceuticalItem>();
            for (int i = 1; i <= 1; i++)
            {
                string tradeName = "TradeName";
                string dose = "pharmaDose";
                string duration = "20";
                string quantity = "10";
                decimal price = 100;

                if (!string.IsNullOrEmpty(tradeName))
                {
                    
                    var pharmaItem = new PharmaceuticalItem
                    {
                        TradeName = tradeName,
                        Dose = dose ?? "",
                        TotalDuration = duration ?? "",
                        Quantity = quantity ?? "",
                        Price = price
                    };
                    lstPharmaceuticalItem.Add(pharmaItem);
                    
                }
                else if (i == 1) // Add default pharmaceutical for demo
                {
                    formData.Pharmaceuticals.Add(new PharmaceuticalItem
                    {
                        TradeName = "Paracetamol 500mg",
                        Dose = "500mg",
                        TotalDuration = "5 days",
                        Quantity = "15 tablets",
                        Price = 25.50m
                    });
                }
            }
            formData.Pharmaceuticals = new List<PharmaceuticalItem>();
            formData.Pharmaceuticals.AddRange(lstPharmaceuticalItem);

            // Parse management plan from request
            List<string> listManagementPlan = new List<string>();
            listManagementPlan.Add("Complete blood count test");
            listManagementPlan.Add("Paracetamol 500mg TID");
            listManagementPlan.Add("Rest and adequate fluid intake");
            listManagementPlan.Add("Test again");
            formData.ManagementPlan = listManagementPlan;
            //for (int i = 1; i <= 4; i++)
            //{
            //    string planItem = "plan{i}";
            //    if (!string.IsNullOrEmpty(planItem))
            //    {
            //        formData.ManagementPlan.Add(planItem);
            //    }
            //    else if (i <= 3) // Add default plans for demo
            //    {
            //        switch (i)
            //        {
            //            case 1: formData.ManagementPlan.Add("Complete blood count test"); break;
            //            case 2: formData.ManagementPlan.Add("Paracetamol 500mg TID"); break;
            //            case 3: formData.ManagementPlan.Add("Rest and adequate fluid intake"); break;
            //        }
            //    }
            //}

            return formData;
        }

        private string GenerateMedicalClaimForm(MedicalClaimFormData formData)
        {
            Document document = new Document(PageSize.A4, 20, 20, 20, 20);
            MemoryStream stream = new MemoryStream();

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                // Create precise fonts matching the original document
                BaseFont helvetica = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                BaseFont helveticaBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                BaseFont timesRoman = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                BaseFont timesRomanBold = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                Font titleFont = new Font(timesRoman, 14, Font.BOLD, BaseColor.BLACK);
                Font headerFont = new Font(helveticaBold, 11, Font.BOLD, BaseColor.BLACK);
                Font cheaderFont = new Font(timesRomanBold, 26, Font.BOLD, BaseColor.RED);
                Font annexFont = new Font(helveticaBold, 12, Font.BOLD, BaseColor.BLACK);
                Font labelFont = new Font(helvetica, 11, Font.NORMAL, BaseColor.BLACK);
                Font boldLabelFont = new Font(helveticaBold, 9, Font.BOLD, BaseColor.BLACK);
                Font smallFont = new Font(helvetica, 10, Font.NORMAL, BaseColor.BLACK);
                Font tinyFont = new Font(helvetica, 7, Font.NORMAL, BaseColor.BLACK);

                // ANNEXURE V section
                Chunk underlinedChunk = new Chunk("ANNEXURE V", annexFont);
                underlinedChunk.SetUnderline(0.5f, -1.5f);
                Paragraph annexure = new Paragraph(underlinedChunk);
                annexure.Alignment = Element.ALIGN_CENTER;
                annexure.SpacingAfter = 10f;
                document.Add(annexure);

                // Add company logo (placeholder)
                //AddCompanyLogo(document, writer);

                // Company header with exact formatting
                AddCompanyHeader(document, cheaderFont, tinyFont);

                // Draw horizontal line using PdfContentByte
                float y = writer.GetVerticalPosition(false) - 8f;
                PdfContentByte cb = writer.DirectContent;
                cb.SetLineWidth(1f); 
                cb.MoveTo(document.LeftMargin, y); // Start point
                cb.LineTo(document.PageSize.Width - document.RightMargin, y); // End point
                cb.Stroke();


                // Title section
                Chunk underlinedTitle = new Chunk("Medical Expenses Claim form", titleFont);
                underlinedTitle.SetUnderline(0.5f, -1.5f);
                Paragraph title = new Paragraph(underlinedTitle);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingBefore = 5f;
                title.SpacingAfter = 10f;
                document.Add(title);

                // Date row
                PdfPTable dateRow = CreateDateRow(formData, tinyFont);
                document.Add(dateRow);
                document.Add(new Paragraph(" ", new Font(helvetica, 3)));

                // Clinic name row
                PdfPTable clinicRow = CreateClinicRow(formData, tinyFont);
                document.Add(clinicRow);
                document.Add(new Paragraph(" ", new Font(helvetica, 3)));

                // Patient details section
                PdfPTable patientSection = CreatePatientSection(formData, tinyFont);
                document.Add(patientSection);
                document.Add(new Paragraph(" ", new Font(helvetica, 3)));

                // Contact details section
                PdfPTable contactSection = CreateContactSection(formData, tinyFont);
                document.Add(contactSection);
                document.Add(new Paragraph(" ", new Font(helvetica, 3)));

                // Insurance details section
                PdfPTable insuranceSection = CreateInsuranceSection(formData, tinyFont);
                document.Add(insuranceSection);
                document.Add(new Paragraph(" ", new Font(helvetica, 3)));

                // Company details section
                PdfPTable companySection = CreateCompanySection(formData, tinyFont);
                document.Add(companySection);
                document.Add(new Paragraph(" ", new Font(helvetica, 8)));

                AddClinicalDetailsPage(document, formData, headerFont, labelFont, labelFont);

                // Authorization text - exact formatting
                Paragraph authText = new Paragraph();
                authText.Add(new Chunk("I hereby authorize the physician, Hospital or pharmacy to file a claim for medical services on my behalf and I confirm that the above-", tinyFont));
                authText.Add(new Chunk("\nmentioned examination/Investigation/therapy is given to me by the doctor. I hereby authorize any Clinic, Physician, Pharmacy or any", tinyFont));
                authText.Add(new Chunk("\nother person who has provided medical services to me to furnish any and all information with regard to any medical history, medical condition,", tinyFont));
                authText.Add(new Chunk("\nor medical services and copies of all medical and Clinic records.", tinyFont));
                authText.SpacingAfter = 10f;
                document.Add(authText);

                // Signature section
                PdfPTable signatureSection = CreateSignatureSection(formData, tinyFont);
                document.Add(signatureSection);
                document.Add(new Paragraph(" ", new Font(helvetica, 8)));

                // Service type checkboxes with exact positioning
                PdfPTable serviceTypeSection = CreateServiceTypeSection(formData, labelFont);
                document.Add(serviceTypeSection);

                // "Kindly tick" instruction
                Paragraph kindlyTick = new Paragraph("Kindly tick whichever is applicable", smallFont);
                kindlyTick.Alignment = Element.ALIGN_CENTER;
                kindlyTick.SpacingAfter = 10f;
                document.Add(kindlyTick);

                // Pharmaceuticals section with exact table structure
                AddPharmaceuticalsSection(document, formData, headerFont, labelFont, smallFont, tinyFont);

                // Add spacing before footer
                document.Add(new Paragraph(" ", new Font(helvetica, 10)));

               

                //// Start new page for clinical details
                //document.NewPage();
                

                document.Close();

                // Send PDF to browser
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment;filename=Medical_Expenses_Claim_Form.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.BinaryWrite(stream.ToArray());
                return Convert.ToBase64String(stream.ToArray());
                //Response.End();
            }
            catch (Exception ex)
            {
                Response.Write("Error generating PDF: " + ex.Message);
            }
            finally
            {
                if (document.IsOpen())
                    document.Close();
                stream.Close();
            }
            return string.Empty;
        }

        private void AddCompanyLogo(Document document, PdfWriter writer)
        {
            try
            {
                //// Create a simple placeholder logo using shapes
                //PdfContentByte cb = writer.DirectContent;

                //// Draw placeholder logo box
                //cb.Rectangle(document.PageSize.Width - 100, document.PageSize.Height - 80, 80, 60);
                //cb.SetColorStroke(BaseColor.BLACK);
                //cb.SetLineWidth(2f);
                //cb.Stroke();

                //// Add logo text
                //cb.BeginText();
                //cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                //cb.SetTextMatrix(document.PageSize.Width - 95, document.PageSize.Height - 50);
                //cb.ShowText("FMC");
                //cb.SetTextMatrix(document.PageSize.Width - 95, document.PageSize.Height - 65);
                //cb.ShowText("LOGO");
                //cb.EndText();


                //header
                string imagePath = Server.MapPath("~/Images/logo.JPG");
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);

                // Optional: Resize image
                img.ScaleToFit(120f, 120f);

                // Optional: Set alignment
                img.Alignment = Element.ALIGN_LEFT;

                // Add image to PDF
                document.Add(img);

                // Optional: Add some text below image
                //document.Add(new Paragraph("Company Logo Above"));


            }
            catch (Exception)
            {
                // Logo creation failed, continue without logo
            }
        }

        private PdfPTable CreateDateRow(MedicalClaimFormData formData, Font font)
        {
            PdfPTable table = new PdfPTable(new float[] { 15f, 15f, 15f, 15f, 15f, 125f });
            table.WidthPercentage = 100;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.DefaultCell.PaddingBottom = 2f;

            table.AddCell(new PdfPCell(new Phrase("Date :", font)) { Border = Rectangle.NO_BORDER });

            if (!string.IsNullOrEmpty(formData.Date))
            {
                var dateParts = formData.Date.Split('/');
                table.AddCell(new PdfPCell(CreateCellSF(dateParts.Length > 0 ? dateParts[0] : "___", font)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
                table.AddCell(new PdfPCell(CreateCellSF("/", font)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
                table.AddCell(new PdfPCell(CreateCellSF(dateParts.Length > 1 ? dateParts[1] : "___", font)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
                table.AddCell(new PdfPCell(CreateCellSF("/", font)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
                table.AddCell(new PdfPCell(CreateCellSF((dateParts.Length > 2 ? dateParts[2] : "____") + ".", font)) { Border = Rectangle.NO_BORDER });
            }
            else
            {
                table.AddCell(new PdfPCell(CreateCellSF("__", font)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(CreateCellSF("/", font)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(CreateCellSF("___", font)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(CreateCellSF("/", font)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(CreateCellSF("____.", font)) { Border = Rectangle.NO_BORDER });
            }

            return table;
        }

        private PdfPTable CreateClinicRow(MedicalClaimFormData formData, Font font)
        {
            PdfPTable table = new PdfPTable(new float[] { 100f, 150f, 100f });
            table.WidthPercentage = 100;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            string clinicName = !string.IsNullOrEmpty(formData.ClinicName) ? formData.ClinicName : "_________________________________";
            string emirates = !string.IsNullOrEmpty(formData.Emirates) ? "Dubai" : "_________________________________";
           

            table.AddCell(new PdfPCell(new Phrase("Clinic Name", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(CreateCellSF(clinicName + ".", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("Emirates", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(CreateCellSF(emirates + ".", font)) { Border = Rectangle.NO_BORDER });

            return table;
        }

        private PdfPCell CreateCell(string str, Font font)
        {
            Chunk chunk = new Chunk(str);
            chunk.SetUnderline(0.5f, -1.5f);
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            BaseFont timesRoman = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font titleFont = new Font(timesRoman, 10, Font.BOLD, BaseColor.BLACK);
            phrase.Font = titleFont;
            PdfPCell cell = new PdfPCell(phrase);
            return cell;
        }

        private PdfPCell CreateCellSF(string str, Font font)
        {
            Chunk chunk = new Chunk(str);
            chunk.SetUnderline(0.5f, -1.5f);
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            BaseFont timesRoman = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font titleFont = new Font(timesRoman, 7, Font.BOLD, BaseColor.BLACK);
            phrase.Font = titleFont;
            PdfPCell cell = new PdfPCell(phrase);
            return cell;
        }

        private Chunk CreateChunk(string str)
        {
            Chunk chunk = new Chunk(str);
            chunk.SetUnderline(0.5f, -1.5f);
            return chunk;
        }

        private PdfPTable CreatePatientSection(MedicalClaimFormData formData, Font font)
        {
            PdfPTable table = new PdfPTable(new float[] { 120f, 250f, 35f, 50f, 40f, 15f, 15f, 15f, 15f, 35f });
            table.WidthPercentage = 100;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.DefaultCell.PaddingBottom = 2f;

            table.AddCell(new PdfPCell(new Phrase("Card Holder's Name", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(CreateCellSF(!string.IsNullOrEmpty(formData.CardHolderName) ? formData.CardHolderName : "_____________________________", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("Age", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(CreateCellSF(formData.Age.HasValue ? formData.Age.Value.ToString() : "______", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("Sex :", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("M", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase(formData.Sex == "M" ? "☑" : "☐", font)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(" F", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase(formData.Sex == "F" ? "☑" : "☐", font)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase("", font)) { Border = Rectangle.NO_BORDER }); // Empty cell for spacing

            return table;
        }

        private PdfPTable CreateContactSection(MedicalClaimFormData formData, Font font)
        {
            PdfPTable table = new PdfPTable(new float[] { 120f, 200f, 80f, 200f });
            table.WidthPercentage = 100;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.DefaultCell.PaddingBottom = 2f;

            table.AddCell(new PdfPCell(new Phrase("Card Holder's Tel No", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(CreateCellSF(!string.IsNullOrEmpty(formData.TelNo) ? formData.TelNo : "______________________", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("Mobile No", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(CreateCellSF((!string.IsNullOrEmpty(formData.MobileNo) ? formData.MobileNo : "_____________________") + ".", font)) { Border = Rectangle.NO_BORDER });

            return table;
        }

        private PdfPTable CreateInsuranceSection(MedicalClaimFormData formData, Font font)
        {
            PdfPTable table = new PdfPTable(new float[] { 80f, 200f, 80f, 240f });
            table.WidthPercentage = 100;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.DefaultCell.PaddingBottom = 2f;

            table.AddCell(new PdfPCell(new Phrase("Ins. Card No", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(CreateCellSF(!string.IsNullOrEmpty(formData.InsCardNo) ? formData.InsCardNo : "________________________", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("Valid up to", font)) { Border = Rectangle.NO_BORDER });

            if (!string.IsNullOrEmpty(formData.ValidUpTo))
            {
                var validParts = formData.ValidUpTo.Split('/');
                string validText = (validParts.Length > 0 ? validParts[0] : "____") + " / " +
                                 (validParts.Length > 1 ? validParts[1] : "____") + " / " +
                                 (validParts.Length > 2 ? validParts[2] : "__________") + ".";
                table.AddCell(new PdfPCell(CreateCellSF(validText, font)) { Border = Rectangle.NO_BORDER });
            }
            else
            {
                table.AddCell(new PdfPCell(CreateCellSF("____ / ____ / __________.", font)) { Border = Rectangle.NO_BORDER });
            }

            return table;
        }

        private PdfPTable CreateCompanySection(MedicalClaimFormData formData, Font font)
        {
            PdfPTable table = new PdfPTable(new float[] { 100f, 180f, 80f, 80f, 80f, 80f });
            table.WidthPercentage = 100;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.DefaultCell.PaddingBottom = 2f;

            table.AddCell(new PdfPCell(new Phrase("Company Name", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(CreateCellSF(!string.IsNullOrEmpty(formData.CompanyName) ? formData.CompanyName : "______________________", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("Employee No", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(CreateCellSF(!string.IsNullOrEmpty(formData.EmployeeNo) ? formData.EmployeeNo : "__________", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("Nationality", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(CreateCellSF((!string.IsNullOrEmpty(formData.Nationality) ? formData.Nationality : "__________") + ".", font)) { Border = Rectangle.NO_BORDER });

            return table;
        }

        private PdfPTable CreateSignatureSection(MedicalClaimFormData formData, Font font)
        {
            PdfPTable table = new PdfPTable(new float[] { 40f, 30f, 20f, 30f, 20f, 150f, 210f });
            table.WidthPercentage = 100;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.DefaultCell.PaddingBottom = 2f;

            table.AddCell(new PdfPCell(new Phrase("Date", font)) { Border = Rectangle.NO_BORDER });

            if (!string.IsNullOrEmpty(formData.SignatureDate))
            {
                var dateParts = formData.SignatureDate.Split('/');
                table.AddCell(new PdfPCell(new Phrase(dateParts.Length > 0 ? dateParts[0] : "____", font)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(new Phrase("/", font)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(new Phrase(dateParts.Length > 1 ? dateParts[1] : "____", font)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(new Phrase("/", font)) { Border = Rectangle.NO_BORDER });
            }
            else
            {
                table.AddCell(new PdfPCell(new Phrase("____", font)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(new Phrase("/", font)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(new Phrase("____", font)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(new Phrase("/", font)) { Border = Rectangle.NO_BORDER });
            }

            table.AddCell(new PdfPCell(new Phrase("____ Signature of the Patient", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("_____________________________.", font)) { Border = Rectangle.NO_BORDER });

            return table;
        }

        private PdfPTable CreateServiceTypeSection(MedicalClaimFormData formData, Font font)
        {
            PdfPTable table = new PdfPTable(new float[] { 30f, 100f, 30f, 100f, 30f, 150f, 30f, 130f });
            table.WidthPercentage = 100;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.DefaultCell.PaddingBottom = 5f;
            table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            string clinicCheck = formData.ServiceTypes.Contains("CLINIC") ? "☑" : "☐";
            string pharmacyCheck = formData.ServiceTypes.Contains("PHARMACY") ? "☑" : "☐";
            string diagnosticCheck = formData.ServiceTypes.Contains("DIAGNOSTIC CENTRE") ? "☑" : "☐";
            string hospitalCheck = formData.ServiceTypes.Contains("HOSPITAL OR OTHER") ? "☑" : "☐";

            table.AddCell(new PdfPCell(new Phrase(clinicCheck, font)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("CLINIC", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase(pharmacyCheck, font)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("PHARMACY", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase(diagnosticCheck, font)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("DIAGNOSTIC CENTRE", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase(hospitalCheck, font)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("HOSPITAL", font)) { Border = Rectangle.NO_BORDER });

            // Second row for "OR OTHER"
            for (int i = 0; i < 4; i++)
            {
                table.AddCell(new PdfPCell(new Phrase("", font)) { Border = Rectangle.NO_BORDER });
            }
            table.AddCell(new PdfPCell(new Phrase("", font)) { Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase("OR OTHER", font)) { Border = Rectangle.NO_BORDER });

            return table;
        }

        private void AddPharmaceuticalsSection(Document document, MedicalClaimFormData formData, Font headerFont, Font labelFont, Font smallFont, Font tinyFont)
        {
            // Section header
            Paragraph pharmaHeader = new Paragraph("Pharmaceuticals (to be filled by treating doctor only)", headerFont);
            pharmaHeader.SpacingAfter = 6f;
            document.Add(pharmaHeader);

            // Main table structure
            PdfPTable mainPharmaTable = new PdfPTable(new float[] { 200f, 120f, 120f, 80f, 80f });
            mainPharmaTable.WidthPercentage = 100;

            // Headers with gray background
            PdfPCell tradeNameHeader = new PdfPCell(new Phrase("Trade Name", headerFont));
            tradeNameHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            tradeNameHeader.BackgroundColor = new BaseColor(220, 220, 220);
            tradeNameHeader.Padding = 5f;
            mainPharmaTable.AddCell(tradeNameHeader);

            PdfPCell doseHeader = new PdfPCell(new Phrase("Dose", headerFont));
            doseHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            doseHeader.BackgroundColor = new BaseColor(220, 220, 220);
            doseHeader.Padding = 5f;
            mainPharmaTable.AddCell(doseHeader);

            PdfPCell durationHeader = new PdfPCell(new Phrase("Total Duration", headerFont));
            durationHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            durationHeader.BackgroundColor = new BaseColor(220, 220, 220);
            durationHeader.Padding = 5f;
            mainPharmaTable.AddCell(durationHeader);

            // Second header row for pharmacy section
            PdfPCell pharmacySection = new PdfPCell(new Phrase("(To be filled by the pharmacy)", tinyFont));
            pharmacySection.HorizontalAlignment = Element.ALIGN_CENTER;
            pharmacySection.BackgroundColor = new BaseColor(220, 220, 220);
            pharmacySection.Padding = 3f;
            pharmacySection.Colspan = 3;
            mainPharmaTable.AddCell(pharmacySection);

            PdfPCell quantityHeader = new PdfPCell(new Phrase("Quantity", headerFont));
            quantityHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            quantityHeader.BackgroundColor = new BaseColor(220, 220, 220);
            quantityHeader.Padding = 5f;
            mainPharmaTable.AddCell(quantityHeader);

            PdfPCell priceHeader = new PdfPCell(new Phrase("Price", headerFont));
            priceHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            priceHeader.BackgroundColor = new BaseColor(220, 220, 220);
            priceHeader.Padding = 5f;
            mainPharmaTable.AddCell(priceHeader);

            // Data rows
            for (int i = 0; i < 1; i++)
            {
                PharmaceuticalItem item = i < formData.Pharmaceuticals.Count ? formData.Pharmaceuticals[i] : null;

                mainPharmaTable.AddCell(new PdfPCell(CreateCell(i + 1 + (item.TradeName ?? ""), labelFont)) { MinimumHeight = 25f, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3f });
                mainPharmaTable.AddCell(new PdfPCell(CreateCell(item.Quantity ?? "", labelFont)) { MinimumHeight = 25f, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3f });
                mainPharmaTable.AddCell(new PdfPCell(CreateCell((bool)item.Price.HasValue ? item.Price.Value.ToString("0.00") : "", labelFont)) { MinimumHeight = 25f, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3f });
            }

            // Total row
            decimal totalPrice = formData.Pharmaceuticals.Where(p => p.Price.HasValue).Sum(p => p.Price.Value);
            PdfPCell exclusionsCell = new PdfPCell(new Phrase("Please apply general exclusions", smallFont));
            exclusionsCell.Colspan = 3;
            exclusionsCell.HorizontalAlignment = Element.ALIGN_LEFT;
            exclusionsCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            exclusionsCell.Padding = 5f;
            mainPharmaTable.AddCell(exclusionsCell);

            PdfPCell totalLabelCell = new PdfPCell(new Phrase("Total", headerFont));
            totalLabelCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalLabelCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            totalLabelCell.Padding = 5f;
            mainPharmaTable.AddCell(totalLabelCell);

            PdfPCell totalValueCell = new PdfPCell(new Phrase(totalPrice > 0 ? totalPrice.ToString("0.00") : "", labelFont));
            totalValueCell.HorizontalAlignment = Element.ALIGN_CENTER;
            totalValueCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            totalValueCell.Padding = 5f;
            mainPharmaTable.AddCell(totalValueCell);

            document.Add(mainPharmaTable);
        }

        private void AddCompanyHeader(Document document, Font cheaderFont, Font smallFont)
        {
            string imagePath = Server.MapPath("~/Images/logo.JPG");
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
            img.ScaleAbsolute(80f, 80f);
            img.Alignment = Element.ALIGN_LEFT;

            // Company footer in bordered box
            PdfPTable footerTable = new PdfPTable(3);
            footerTable.WidthPercentage = 100;
            
            //image cell
            PdfPCell imageCell = new PdfPCell(img)
            {
                Border = Rectangle.NO_BORDER,
                VerticalAlignment = Element.ALIGN_LEFT
            };

            PdfPCell footerCell = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                VerticalAlignment = Element.ALIGN_TOP
            };

            // Company name
            Paragraph companyName = new Paragraph("F M C NETWORK UAE", cheaderFont);
            companyName.Alignment = Element.ALIGN_CENTER;
            companyName.SpacingAfter = 5f;

            // Contact details
            Paragraph contactDetails = new Paragraph();
            contactDetails.Add(new Chunk("P. O. BOX: 50430, DUBAI, Tel – 04 3871900, Fax – 04 3977842", smallFont));
            contactDetails.Add(new Chunk("\nEmail – approval@fmchealthcare.ae Helpline Number: 600-565691", smallFont));
            contactDetails.Alignment = Element.ALIGN_CENTER;

            ////image of client
            string clientImagePath = Server.MapPath("~/Images/Take.png");
            iTextSharp.text.Image clientImg = iTextSharp.text.Image.GetInstance(clientImagePath);

            clientImg.ScaleAbsolute(80f, 80f);
            clientImg.Alignment = Element.ALIGN_TOP;
            PdfPCell clientImgCell = new PdfPCell(clientImg)
            {
                Border = Rectangle.NO_BORDER,
                VerticalAlignment = Element.ALIGN_TOP
            };

            footerCell.AddElement(companyName);
            footerCell.AddElement(contactDetails);
            footerCell.Border = Rectangle.NO_BORDER;

            footerTable.AddCell(imageCell);
            footerTable.AddCell(footerCell);
            footerTable.AddCell(clientImgCell);
            footerTable.SetWidths(new float[] { 20f, 60f, 20f });
            document.Add(footerTable);
        }

        private void AddClinicalDetailsPage(Document document, MedicalClaimFormData formData, Font headerFont, Font labelFont, Font smallFont)
        {
            // Clinical Details header with vitals
            PdfPTable clinicalHeader = new PdfPTable(new float[] { 60f, 30f, 15f, 15f, 40f, 30f, 40f, 55f });
            clinicalHeader.WidthPercentage = 100;
            //clinicalHeader.DefaultCell.Border = Rectangle.NO_BORDER;
            clinicalHeader.DefaultCell.PaddingBottom = 3f;

            clinicalHeader.AddCell(new PdfPCell(new Phrase("Clinical Details: Temp", labelFont)) { Border = Rectangle.NO_BORDER });
            clinicalHeader.AddCell(new PdfPCell(CreateCell(!string.IsNullOrEmpty(formData.Temperature) ? formData.Temperature : "_______", smallFont)) { Border = Rectangle.NO_BORDER });
            clinicalHeader.AddCell(new PdfPCell(new Phrase("°C", labelFont)) { Border = Rectangle.NO_BORDER });
            clinicalHeader.AddCell(new PdfPCell(new Phrase("B.P.", labelFont)) { Border = Rectangle.NO_BORDER });
            clinicalHeader.AddCell(new PdfPCell(CreateCell(!string.IsNullOrEmpty(formData.BloodPressure) ? formData.BloodPressure : "_______", smallFont)) { Border = Rectangle.NO_BORDER });
            clinicalHeader.AddCell(new PdfPCell(new Phrase("mmHg", labelFont)) { Border = Rectangle.NO_BORDER });
            clinicalHeader.AddCell(new PdfPCell(new Phrase("Pulse.", labelFont)) { Border = Rectangle.NO_BORDER });
            clinicalHeader.AddCell(new PdfPCell(CreateCell((!string.IsNullOrEmpty(formData.Pulse) ? formData.Pulse : "______") + " / min", smallFont)) { Border = Rectangle.NO_BORDER });


            //document.Add(clinicalHeader);
            //document.Add(new Paragraph(" ", new Font(Font.FontFamily.HELVETICA, 6)));
            PdfPCell clinicalTable  = new PdfPCell();
            clinicalTable.AddElement(clinicalHeader);
            // Signs & Symptoms
            string signsText = !string.IsNullOrEmpty(formData.SignsSymptoms) ?
                formData.SignsSymptoms :
                "________________________________________________________________";
            Paragraph signsSymptoms = new Paragraph("Sign & Symptoms " + CreateChunk(signsText) + ".", smallFont);
            signsSymptoms.SpacingAfter = 8f;
            clinicalTable.AddElement(signsSymptoms);

            // Date of onset
            string onsetText = !string.IsNullOrEmpty(formData.OnsetDate) ?
                formData.OnsetDate :
                "____________________________";
            Paragraph onsetDate = new Paragraph("Date of onset of illness:" + CreateChunk(onsetText), smallFont);
            onsetDate.SpacingAfter = 8f;

            clinicalTable.AddElement(onsetDate);

            // Visit type checkboxes
            PdfPTable visitTypeTable = new PdfPTable(new float[] { 30f, 100f, 30f, 120f, 30f, 100f, 30f, 160f });
            visitTypeTable.WidthPercentage = 100;
           // visitTypeTable.DefaultCell.Border = Rectangle.NO_BORDER;
            visitTypeTable.DefaultCell.PaddingBottom = 3f;

            string emergencyCheck = formData.VisitTypes.Contains("Emergency") ? "☑" : "☐";
            string workCheck = formData.VisitTypes.Contains("Work related") ? "☑" : "☐";
            string newCheck = formData.VisitTypes.Contains("New visit") ? "☑" : "☐";
            string followupCheck = formData.VisitTypes.Contains("Follow up visit") ? "☑" : "☐";

            visitTypeTable.AddCell(new PdfPCell(new Phrase(emergencyCheck, labelFont)) { Border = Rectangle.BOX });
            visitTypeTable.AddCell(new PdfPCell(new Phrase("Emergency", labelFont)) { Border = Rectangle.NO_BORDER });
            visitTypeTable.AddCell(new PdfPCell(new Phrase(workCheck, labelFont)) { Border = Rectangle.BOX });
            visitTypeTable.AddCell(new PdfPCell(new Phrase("Work related", labelFont)) { Border = Rectangle.NO_BORDER });
            visitTypeTable.AddCell(new PdfPCell(new Phrase(newCheck, labelFont)) { Border = Rectangle.BOX });
            visitTypeTable.AddCell(new PdfPCell(new Phrase("New visit", labelFont)) { Border = Rectangle.NO_BORDER });
            visitTypeTable.AddCell(new PdfPCell(new Phrase(followupCheck, labelFont)) { Border = Rectangle.BOX });
            visitTypeTable.AddCell(new PdfPCell(new Phrase("Follow up visit", labelFont)) { Border = Rectangle.NO_BORDER });

            clinicalTable.AddElement(visitTypeTable);
            clinicalTable.AddElement(new Paragraph(" ", new Font(Font.FontFamily.HELVETICA, 8)));

            // Diagnosis section
            string diagnosisText = !string.IsNullOrEmpty(formData.Diagnosis) ?
                formData.Diagnosis :
                "________________________________________________________________";
            Paragraph diagnosis1 = new Paragraph("Diagnosis " + CreateChunk(diagnosisText) + ".", labelFont);
            clinicalTable.AddElement(diagnosis1);

            // Additional diagnosis lines
            Paragraph diagnosis2 = new Paragraph("………………………..…………………………………………………………………………………", labelFont);
            clinicalTable.AddElement(diagnosis2);

            Paragraph diagnosis3 = new Paragraph("…………", labelFont);
            clinicalTable.AddElement(diagnosis3);

            Paragraph diagnosis4 = new Paragraph("………………………………………………………………………………………….", labelFont);
            diagnosis4.SpacingAfter = 10f;
            clinicalTable.AddElement(diagnosis4);


            clinicalTable.Border = Rectangle.BOX;
            clinicalTable.Padding = 10f; // Optional padding inside border

            // Create outer table to hold the single cell
            PdfPTable ClinicalDetails = new PdfPTable(1);
            ClinicalDetails.WidthPercentage = 100;
            ClinicalDetails.AddCell(clinicalTable);

            // Add to document
            document.Add(ClinicalDetails);


            // Management plan
            PdfPCell managementTable = new PdfPCell();
            Paragraph managementTitle = new Paragraph("Management plan (Services inside the clinic including injections and investigations)", labelFont);
            managementTitle.SpacingAfter = 8f;
            managementTable.AddElement(managementTitle);
            //document.Add(managementTitle);

            for (int i = 0; i < 1; i++)
            {
                string planText = i < formData.ManagementPlan.Count && !string.IsNullOrEmpty(formData.ManagementPlan[i]) ?
                    formData.ManagementPlan[i] :
                    "________________________________________________________________";
                Paragraph managementItem = new Paragraph(i + 1 + " " + planText, labelFont);
                managementTable.AddElement(managementItem);
            }
            document.Add(new Paragraph(" ", new Font(Font.FontFamily.HELVETICA, 10)));

            // Doctor's signature
            string doctorText = !string.IsNullOrEmpty(formData.DoctorNameAndSignature) ?
                formData.DoctorNameAndSignature :
                "________________________________________________________________";
            Paragraph doctorSig = new Paragraph("Doctor's Name and signature with seal: " + CreateChunk(doctorText) + ".", labelFont);
            doctorSig.SpacingAfter = 12f;
            managementTable.AddElement(doctorSig);
            managementTable.Border = Rectangle.BOX;
            managementTable.Padding = 10f; // Optional padding inside border
            // Create outer table to hold the single cell
            PdfPTable ManagementDetails = new PdfPTable(1);
            ManagementDetails.WidthPercentage = 100;
            ManagementDetails.AddCell(managementTable);

            // Add to document
            document.Add(ManagementDetails);

            // Diagnostic Procedures
            string diagnosticText = !string.IsNullOrEmpty(formData.DiagnosticProcedures) ?
                formData.DiagnosticProcedures :
                "";
            Paragraph diagnosticProc = new Paragraph("Diagnostic Procedures referred outside: " + CreateChunk(diagnosticText), labelFont);
            PdfPCell diagnosticTable = new PdfPCell();
            diagnosticTable.AddElement(diagnosticProc);
            diagnosticTable.Border = Rectangle.BOX;
            diagnosticTable.Padding = 10f; // Optional padding inside border
            PdfPTable diagnosticDetails = new PdfPTable(1);
            diagnosticDetails.WidthPercentage = 100;
            diagnosticDetails.AddCell(diagnosticTable);
            document.Add(diagnosticDetails);
            
        }
    

        // Data model classes - no database dependencies
        public class MedicalClaimFormData
    {
        public string Date { get; set; }
        public string ClinicName { get; set; } //= "Emirates";
        public string Emirates { get; set; }
        public string CardHolderName { get; set; }
        public int? Age { get; set; }
        public string Sex { get; set; } // "M" or "F"
        public string TelNo { get; set; }
        public string MobileNo { get; set; }
        public string InsCardNo { get; set; }
        public string ValidUpTo { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeNo { get; set; }
        public string Nationality { get; set; }
        public string SignatureDate { get; set; }
        public List<string> ServiceTypes { get; set; } //= new List<string>(); // CLINIC, PHARMACY, DIAGNOSTIC CENTRE, HOSPITAL OR OTHER
        public List<PharmaceuticalItem> Pharmaceuticals { get; set; } //= new List<PharmaceuticalItem>();
        public string Temperature { get; set; }
        public string BloodPressure { get; set; }
        public string Pulse { get; set; }
        public string SignsSymptoms { get; set; }
        public string OnsetDate { get; set; }
        public List<string> VisitTypes { get; set; }// = new List<string>(); // Emergency, Work related, New visit, Follow up visit
        public string Diagnosis { get; set; }
        public List<string> ManagementPlan { get; set; } //= new List<string>();
        public string DoctorNameAndSignature { get; set; }
        public string DiagnosticProcedures { get; set; }
    }



    public class PharmaceuticalItem
    {
        public string TradeName { get; set; }
        public string Dose { get; set; }
        public string TotalDuration { get; set; }
        public string Quantity { get; set; }
        public decimal? Price { get; set; }
    }

        // Utility class for form data validation and formatting
        public static class FormValidator
    {
        public static bool ValidateFormData(MedicalClaimFormData formData, out List<string> errors)
        {
            errors = new List<string>();

            // Basic validations
            if (string.IsNullOrEmpty(formData.CardHolderName))
                errors.Add("Card Holder Name is required");

            if (!formData.Age.HasValue || formData.Age < 0 || formData.Age > 120)
                errors.Add("Valid age is required (0-120)");

            if (string.IsNullOrEmpty(formData.Sex) || (formData.Sex != "M" && formData.Sex != "F"))
                errors.Add("Sex must be M or F");

            //if (!string.IsNullOrEmpty(formData.Date))
            //{
            //    if (!DateTime.TryParseExact(formData.Date, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ))
            //        errors.Add("Date must be in dd/MM/yyyy format");
            //}

            // Validate pharmaceutical prices
            foreach (var pharma in formData.Pharmaceuticals)
            {
                if (pharma.Price.HasValue && pharma.Price < 0)
                    errors.Add("Price for " + pharma.TradeName + " cannot be negative");
            }

            return errors.Count == 0;
        }

        public static void FormatPhoneNumbers(MedicalClaimFormData formData)
        {
            // Format phone numbers to standard UAE format
            if (!string.IsNullOrEmpty(formData.TelNo))
                formData.TelNo = FormatUAEPhoneNumber(formData.TelNo);

            if (!string.IsNullOrEmpty(formData.MobileNo))
                formData.MobileNo = FormatUAEMobileNumber(formData.MobileNo);
        }

        private static string FormatUAEPhoneNumber(string phone)
        {
            // Remove all non-digit characters
            string digits = new string(phone.Where(char.IsDigit).ToArray());

            // Format as UAE landline (04-XXXXXXX)
            //if (digits.Length == 8 && digits.StartsWith("04"))
            //    return digits.Substring(0, 2)-digits.Substring(2);
            //else
                //if (digits.Length == 7)
                //return 04-digits;

            return phone; // Return original if format doesn't match
        }

        private static string FormatUAEMobileNumber(string mobile)
        {
            // Remove all non-digit characters
            string digits = new string(mobile.Where(char.IsDigit).ToArray());

            // Format as UAE mobile (050-XXXXXXX)
            //if (digits.Length == 9 && (digits.StartsWith("050") || digits.StartsWith("055") ||
            //                          digits.StartsWith("056") || digits.StartsWith("052")))
            //    return $"{digits.Substring(0, 3)}-{digits.Substring(3)}";

            return mobile; // Return original if format doesn't match
        }
    }
    }

}


 