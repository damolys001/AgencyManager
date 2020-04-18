using System;
using System.Collections.Generic;
using System.Configuration;
using DataFactory.Models;
using Oracle.ManagedDataAccess.Client;

namespace DataFactory.Services
{
    public class TurnQuestHelper
    {
        public List<Production> GetNonLifeProduction(string startDate, string endDate)
        {
            var productionReport = new List<Production>();
            //var query = string.Format("SELECT  (select pol_UW_YEAR FROM GIN_POLICIES WHERE ROWNUM =1 AND POL_DRCR_NO =PR_DRCR_NO )POL_UW_YEAR, RSKRG_CODE,clnt_code, pr_pol_policy_no policy_no,pr_pol_ren_endos_no endorsement_no,gggap_code product_grp , upper(gggap_desc) product_class   , PR_DRCR_NO dr_cr_no,     DECODE(SIGN(rskrg_endos_diff_amt), 1, 'DR', 'CR')  dr_cr, TO_CHAR(pr_transaction_date, 'DD-Mon-YYYY') trans_date,rskrg_pol_policy_no pr_policy_no,     TO_CHAR(RSKRG_RISK_COVER_FROM, 'DD-Mon-YYYY') cover_from, TO_CHAR(RSKRG_RISK_COVER_TO, 'DD-Mon-YYYY') cover_to,AGN_SHT_DESC,AGN_NAME agent,AGN_CODE ,       CLNT_NAME|| ' ' || CLNT_OTHER_NAMES client ,  PR_SUM_INSURED,          SUM(ROUND(NVL(rskrg_sum_insured, 0) * DECODE(-2000,-2000,pr_cur_rate,1),0))SUM_INSURED,     SUM(ROUND(NVL(rskrg_endos_diff_amt, 0) * DECODE(-2000, -2000, pr_cur_rate, 1),0))PREMIUM,     SUM(ROUND(NVL(rskrg_comm_endos_diff_amt, 0) * DECODE(-2000, -2000, pr_cur_rate, 1),0))COMMISSION,     SUM(ROUND(NVL(pr_wtht, 0) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) wht_tax,     SUM(ROUND(NVL(pr_vat_amt, 0) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) VAT,     SUM(ROUND(NVL(pr_coin_fee_amt, 0) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) ORC,     SUM(ROUND((NVL(-rskrg_mand_prem, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) MANDATORY,     SUM(ROUND((NVL(-rskrg_quota_prem, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) quota,     SUM(ROUND((NVL(-rskrg_fstsup_prem, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) ST_SURPLUS,     SUM(ROUND((NVL(-rskrg_secsup_prem, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) ND_SURPLUS,     SUM(ROUND((NVL(-rskrg_facre_prem, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) FACULT_RI,         SUM(ROUND((NVL(rskrg_comp_net_ret, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) OWN,     coverage_code coverage_area_code, coverage_area,sbu, location_name location , spoke,SCL_DESC sub_class ,RSKRG_IPU_PROPERTY_ID risk_id,IPU_ITEM_DESC risk_desc,         (select    TO_CHAR(POL_RENEWAL_DT, 'DD-Mon-YYYY') from gin_policies where POL_REN_ENDOS_NO =PR_POL_REN_ENDOS_NO and rownum = 1 ) POL_RENEWAL_DT, CASE(select POL_UW_YEAR  from gin_policies where pr_pol_ren_endos_no = POL_REN_ENDOS_NO and rownum = 1) - (select POL_UW_YEAR from gin_policies where pr_pol_policy_no = pol_policy_no and rownum = 1) when 0 then 'NB'  else 'RN' END as \"type\"             FROM GIN_POLICY_REGISTER,GIN_GIS_GL_ACCTS_GROUPS,  GIN_CLASSES, GIN_SUB_CLASSES,  TQC_CLIENTS,GIN_POLICY_RISK_REG,GIN_POLICY_SBU_DTLS,tqc_agencies,gin_insured_property_unds,         (select a.osd_name coverage_area, b.osd_name spoke, c.osd_name sbu, c.osd_code sbu_code, a.osd_id coverage_code             from tqc_org_division_levels_type,             tqc_org_division_levels,             tqc_org_subdivisions a,             tqc_org_subdivisions b,             tqc_org_subdivisions c             where odl_dlt_code = dlt_code             and a.osd_odl_code = odl_code             and dlt_act_code = 100             and odl_ranking = 4             and b.osd_code = a.osd_parent_osd_code             and c.osd_code = b.osd_parent_osd_code),             (select  a.osd_name location_name, a.osd_id location_code             from tqc_org_division_levels_type,             tqc_org_division_levels,             tqc_org_subdivisions a,             tqc_org_subdivisions b             where odl_dlt_code = dlt_code             and a.osd_odl_code = odl_code             and dlt_act_code = 101             and odl_ranking = 2             and b.osd_code = a.osd_parent_osd_code)     WHERE rskrg_scl_code = scl_code     AND scl_cla_code = cla_code     AND rskrg_pr_code = pr_code     AND rskrg_ipu_code = ipu_code     AND PR_PRP_CODE = CLNT_CODE     AND scl_gggap_code = gggap_code     and AGN_CODE = PR_AGNT_AGENT_CODE     AND rskrg_tran_date BETWEEN '{0}' AND '{1}'     AND PR_POL_BATCH_NO = PDL_POL_BATCH_NO(+)     AND pdl_unit_code = coverage_code(+)     AND pdl_location_code = location_code(+) AND spoke='Agency' having SUM(ROUND((NVL(rskrg_endos_diff_amt,0))*DECODE(-2000, -2000, pr_cur_rate, 1),0)) <> 0     GROUP BY RSKRG_CODE,COVERAGE_CODE,AGN_SHT_DESC,gggap_code , rownum,clnt_code,PR_DRCR_NO ,PR_POL_POLICY_NO,pr_pol_ren_endos_no,AGN_CODE,     DECODE(SIGN(rskrg_endos_diff_amt), 1, 'DR', 'CR') , rskrg_pol_batch_no  ,rskrg_agnt_sht_desc, pr_vat_amt,PR_SUM_INSURED,RSKRG_SUM_INSURED,rskrg_pol_policy_no , gggap_desc  ,     clnt_SHT_DESC ,TO_CHAR(pr_transaction_date, 'DD-Mon-YYYY'),TO_CHAR(RSKRG_RISK_COVER_FROM, 'DD-Mon-YYYY'),TO_CHAR(RSKRG_RISK_COVER_TO, 'DD-Mon-YYYY'),coverage_area,sbu,location_name,spoke,AGN_NAME, CLNT_NAME,CLNT_OTHER_NAMES,SCL_DESC,RSKRG_IPU_PROPERTY_ID,IPU_ITEM_DESC      ORDER BY gggap_code,TO_CHAR(pr_transaction_date, 'DD-Mon-YYYY')", startDate, endDate);
            //filter only new Business
            var query = string.Format("select AGN_SHT_DESC, AGENT,PREMIUM,SPOKE,\"type\" from (SELECT  (select pol_UW_YEAR FROM GIN_POLICIES WHERE ROWNUM =1 AND POL_DRCR_NO =PR_DRCR_NO )POL_UW_YEAR, RSKRG_CODE,clnt_code, pr_pol_policy_no policy_no,pr_pol_ren_endos_no endorsement_no,gggap_code product_grp , upper(gggap_desc) product_class   , PR_DRCR_NO dr_cr_no,     DECODE(SIGN(rskrg_endos_diff_amt), 1, 'DR', 'CR')  dr_cr, TO_CHAR(pr_transaction_date, 'DD-Mon-YYYY') trans_date,rskrg_pol_policy_no pr_policy_no,     TO_CHAR(RSKRG_RISK_COVER_FROM, 'DD-Mon-YYYY') cover_from, TO_CHAR(RSKRG_RISK_COVER_TO, 'DD-Mon-YYYY') cover_to,AGN_SHT_DESC,AGN_NAME agent,AGN_CODE ,       CLNT_NAME|| ' ' || CLNT_OTHER_NAMES client ,  PR_SUM_INSURED,          SUM(ROUND(NVL(rskrg_sum_insured, 0) * DECODE(-2000,-2000,pr_cur_rate,1),0))SUM_INSURED,     SUM(ROUND(NVL(rskrg_endos_diff_amt, 0) * DECODE(-2000, -2000, pr_cur_rate, 1),0))PREMIUM,     SUM(ROUND(NVL(rskrg_comm_endos_diff_amt, 0) * DECODE(-2000, -2000, pr_cur_rate, 1),0))COMMISSION,     SUM(ROUND(NVL(pr_wtht, 0) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) wht_tax,     SUM(ROUND(NVL(pr_vat_amt, 0) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) VAT,     SUM(ROUND(NVL(pr_coin_fee_amt, 0) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) ORC,     SUM(ROUND((NVL(-rskrg_mand_prem, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) MANDATORY,     SUM(ROUND((NVL(-rskrg_quota_prem, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) quota,     SUM(ROUND((NVL(-rskrg_fstsup_prem, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) ST_SURPLUS,     SUM(ROUND((NVL(-rskrg_secsup_prem, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) ND_SURPLUS,     SUM(ROUND((NVL(-rskrg_facre_prem, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) FACULT_RI,         SUM(ROUND((NVL(rskrg_comp_net_ret, 0)) * DECODE(-2000, -2000, pr_cur_rate, 1),0)) OWN,     coverage_code coverage_area_code, coverage_area,sbu, location_name location , spoke,SCL_DESC sub_class ,RSKRG_IPU_PROPERTY_ID risk_id,IPU_ITEM_DESC risk_desc,         (select    TO_CHAR(POL_RENEWAL_DT, 'DD-Mon-YYYY') from gin_policies where POL_REN_ENDOS_NO =PR_POL_REN_ENDOS_NO and rownum = 1 ) POL_RENEWAL_DT, CASE(select POL_UW_YEAR  from gin_policies where pr_pol_ren_endos_no = POL_REN_ENDOS_NO and rownum = 1) - (select POL_UW_YEAR from gin_policies where pr_pol_policy_no = pol_policy_no and rownum = 1) when 0 then 'NB'  else 'RN' END as \"type\"             FROM GIN_POLICY_REGISTER,GIN_GIS_GL_ACCTS_GROUPS,  GIN_CLASSES, GIN_SUB_CLASSES,  TQC_CLIENTS,GIN_POLICY_RISK_REG,GIN_POLICY_SBU_DTLS,tqc_agencies,gin_insured_property_unds,         (select a.osd_name coverage_area, b.osd_name spoke, c.osd_name sbu, c.osd_code sbu_code, a.osd_id coverage_code             from tqc_org_division_levels_type,             tqc_org_division_levels,             tqc_org_subdivisions a,             tqc_org_subdivisions b,             tqc_org_subdivisions c             where odl_dlt_code = dlt_code             and a.osd_odl_code = odl_code             and dlt_act_code = 100             and odl_ranking = 4             and b.osd_code = a.osd_parent_osd_code             and c.osd_code = b.osd_parent_osd_code),             (select  a.osd_name location_name, a.osd_id location_code             from tqc_org_division_levels_type,             tqc_org_division_levels,             tqc_org_subdivisions a,             tqc_org_subdivisions b             where odl_dlt_code = dlt_code             and a.osd_odl_code = odl_code             and dlt_act_code = 101             and odl_ranking = 2             and b.osd_code = a.osd_parent_osd_code)     WHERE rskrg_scl_code = scl_code     AND scl_cla_code = cla_code     AND rskrg_pr_code = pr_code     AND rskrg_ipu_code = ipu_code     AND PR_PRP_CODE = CLNT_CODE     AND scl_gggap_code = gggap_code     and AGN_CODE = PR_AGNT_AGENT_CODE     AND rskrg_tran_date BETWEEN '{0}' AND '{1}'     AND PR_POL_BATCH_NO = PDL_POL_BATCH_NO(+)     AND pdl_unit_code = coverage_code(+)     AND pdl_location_code = location_code(+) AND spoke='Agency' having SUM(ROUND((NVL(rskrg_endos_diff_amt,0))*DECODE(-2000, -2000, pr_cur_rate, 1),0)) <> 0     GROUP BY RSKRG_CODE,COVERAGE_CODE,AGN_SHT_DESC,gggap_code , rownum,clnt_code,PR_DRCR_NO ,PR_POL_POLICY_NO,pr_pol_ren_endos_no,AGN_CODE,     DECODE(SIGN(rskrg_endos_diff_amt), 1, 'DR', 'CR') , rskrg_pol_batch_no  ,rskrg_agnt_sht_desc, pr_vat_amt,PR_SUM_INSURED,RSKRG_SUM_INSURED,rskrg_pol_policy_no , gggap_desc  ,     clnt_SHT_DESC ,TO_CHAR(pr_transaction_date, 'DD-Mon-YYYY'),TO_CHAR(RSKRG_RISK_COVER_FROM, 'DD-Mon-YYYY'),TO_CHAR(RSKRG_RISK_COVER_TO, 'DD-Mon-YYYY'),coverage_area,sbu,location_name,spoke,AGN_NAME, CLNT_NAME,CLNT_OTHER_NAMES,SCL_DESC,RSKRG_IPU_PROPERTY_ID,IPU_ITEM_DESC      ORDER BY gggap_code,TO_CHAR(pr_transaction_date, 'DD-Mon-YYYY') ) where \"type\"='NB'", startDate, endDate);
            var gisUrl = ConfigurationManager.AppSettings["gisUrl"].ToString();
            var con = new OracleConnection { ConnectionString = gisUrl };
            try
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = query;
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var production = new Production
                        {
                            AgentCode = reader[0].ToString(),
                            AgentName = reader[1].ToString(),
                            Premium = decimal.Parse(reader[2].ToString()),
                            TransactionType = reader[4].ToString(),
                            Spoke = reader[3].ToString(),
                        };
                        productionReport.Add(production);
                    }
                }
                con.Close();
                return productionReport;
            }
            catch (Exception ex)
            {
                con.Close();
                JUtility.ErrorLog.LogApplicationError(ex);
                return productionReport;
            }
        }
        public List<Production> GetIndividualLifeProduction(string startDate, string endDate)
        {
            var productionReport = new List<Production>();
            #region Query
            var query = string.Format(@"Select Spoke,AGN_NAME,AGN_SHT_DESC,ENDR_TYPE,PREMIUM from (Select ggg.*, (select Pol_prp_code from lms_policies where pol_policy_no =ggg.pol_policy_no ) prp_code, (select prp_clnt_code from lms_proposers where prp_code = (select Pol_prp_code from lms_policies where pol_policy_no =ggg.pol_policy_no ))clnt_code  from (SELECT itb_ref3, brn_sht_desc, brn_name, pol_policy_no, client,
         opr_receipt_no, to_char(opr_date,'DD-Mon-RRRR')opr_date, oprsource, dr_acc, cr_acc, paymthd, sbu,
         coverage_area, spoke, locations, credit, debit, prod_desc,
         endr_tot_premium, agn_name, agn_sht_desc, pol_proposal_no,
         ENDR_AUTHORIZATION_DATE, itb_date, receipt_date, endr_type, pol_cla_code,nvl(POL_PENS_INSTLMT_PREM,POL_INSTLMT_PREM)Premium,pol_term
    FROM tqc_branches,
         (SELECT DISTINCT itb_ref3, pol_bra_code, pol_policy_no,
                          prp_surname || ' ' || prp_other_names client,
                          opr_receipt_no, opr_date,
                          REPLACE (opr_source, '_', ' ') oprsource,
                          DECODE (NVL (endr_pay_method, 'C'),
                                  'K', 'CHECK-OFF',
                                  'OTHER'
                                 ) paymthd,
                          itb_acc_number dr_acc, itb_acc_number cr_acc, sbu,
                          coverage_area, spoke, locations,
                          DECODE (itb_drcr,
                                  'C', NVL (itb_amount, 0),
                                  0
                                 ) credit,
                          DECODE (itb_drcr,
                                  'D', NVL (itb_amount, 0),
                                  0
                                 ) debit, prod_desc,
                            DECODE (itb_drcr,
                                    'C', NVL (itb_amount, 0),
                                    0
                                   )
                          + DECODE (itb_drcr, 'D', NVL (itb_amount, 0) * -1,
                                    0) endr_tot_premium,
                          agn_name, agn_sht_desc, pol_proposal_no,
                          TO_CHAR (ENDR_AUTHORIZATION_DATE,
                                   'DD-Mon-RRRR'
                                  ) ENDR_AUTHORIZATION_DATE,
                          TO_CHAR (itb_date, 'DD-Mon-RRRR') itb_date,
                          TO_CHAR (opr_date, 'DD-Mon-RRRR') receipt_date,
                          (SELECT 'NEW BUSINESS'
                             FROM DUAL
                            WHERE opr_date - pol_effective_date <=
                                                                365
                           UNION
                           SELECT 'RENEWAL'
                             FROM DUAL
                            WHERE opr_date - pol_effective_date >
                                                                365)
                                                                    endr_type,
                          pol_cla_code,POL_INSTLMT_PREM,POL_PENS_INSTLMT_PREM,pol_term
                     FROM lms_interface_table a,
                          lms_ord_prem_receipts,
                          lms_policies,
                          lms_policy_endorsements,
                          lms_products,
                          lms_proposers,
                          lms_agencies,
                          tqc_branches,
                          (SELECT a.osd_name coverage_area, b.osd_name spoke,
                                  c.osd_name sbu, a.osd_id coverage_area_code,
                                  b.osd_id spoke_code, c.osd_id sbu_code
                             FROM tqc_org_division_levels_type,
                                  tqc_org_division_levels,
                                  tqc_org_subdivisions a,
                                  tqc_org_subdivisions b,
                                  tqc_org_subdivisions c
                            WHERE odl_dlt_code = dlt_code
                              AND a.osd_odl_code = odl_code
                              AND dlt_act_code = 100
                              AND odl_ranking = 4
                              AND b.osd_code = a.osd_parent_osd_code
                              AND c.osd_code = b.osd_parent_osd_code),
                          (SELECT a.osd_name locations,
                                  b.osd_name ORGANIZATION,
                                  a.osd_id location_code
                             FROM tqc_org_division_levels_type,
                                  tqc_org_division_levels,
                                  tqc_org_subdivisions a,
                                  tqc_org_subdivisions b
                            WHERE odl_dlt_code = dlt_code
                              AND a.osd_odl_code = odl_code
                              AND dlt_act_code = 101
                              AND odl_ranking = 2
                              AND b.osd_code = a.osd_parent_osd_code)
                    WHERE itb_transaction_type <> 'UPDATE_VGL'
                      AND itb_opr_code = opr_code
                      AND opr_pol_code = pol_code
                      AND pol_current_endr_code = endr_code
                      AND pol_client_prp_code = prp_code
                      AND pol_prod_code = prod_code
                      AND a.itb_covrg_area_code = coverage_area_code(+)
                      AND a.itb_sbu_code = sbu_code(+)
                      AND a.itb_spoke_code = spoke_code(+)
                      AND a.itb_loc_code = location_code(+)
                      AND itb_ort_code IS NULL
                      AND pol_cla_code = 20021
                      AND agn_code = brn_agn_code(+)
                      AND endr_agen_code = agn_code
--AND POL_POLICY_NO LIKE 'UACP/614/000001/HQ'
                  --    AND sbu_code =
                  --           DECODE (NVL (:v_sbu_code, -2000),
                  --                   -2000, sbu_code,
                   --                  :v_sbu_code
                    --                )
                      AND TRUNC (itb_date) BETWEEN NVL (To_date('{0}','DD-Mon-YYYY'),
                                                        TRUNC (itb_date)
                                                       )
                                               AND NVL (To_date('{1}','DD-Mon-YYYY'),
                                                        TRUNC (itb_date)
                                                       )
                      AND NOT itb_transaction_code IN (
                             SELECT itb_transaction_code
                               FROM lms_interface_table
                              WHERE itb_transaction_code =
                                                        a.itb_transaction_code
                                AND itb_transaction_type = 'UPDATE_VGL'
                                AND itb_narrative LIKE 'LMS RCPT%'
--GROUP BY ITB_TRANSACTION_CODE
                          )
          UNION ALL
          SELECT NULL itb_ref3, pol_bra_code, pol_policy_no,
                 prp_surname || ' ' || prp_other_names client, opr_receipt_no,
                 opr_date, REPLACE (opr_source, '_', ' ') oprsource,
                 DECODE (NVL (endr_pay_method, 'C'),
                         'K', 'CHECK-OFF',
                         'OTHER'
                        ) paymthd,
                 NULL dr_acc,
                 (SELECT tgl_crt_acc_number
                    FROM fms_trngl
                   WHERE tgl_narrative LIKE '%O/S PREM RECOVERY%'
                     AND ROWNUM = 1) cr_acc,
                 sbu, coverage_area, spoke, locations,
                 DECODE (opr_drcr, 'C', NVL (opr_amt, 0), 0) credit,
                 DECODE (opr_drcr, 'D', NVL (opr_amt, 0), 0) debit, prod_desc,
                   DECODE (opr_drcr,
                           'C', NVL (opr_amt, 0),
                           0
                          )
                 + DECODE (opr_drcr, 'D', NVL (opr_amt, 0) * -1, 0)
                                                             endr_tot_premium,
                 agn_name, agn_sht_desc, pol_proposal_no,
                 TO_CHAR (ENDR_AUTHORIZATION_DATE, 'DD-Mon-RRRR') ENDR_AUTHORIZATION_DATE,
                 TO_CHAR (opr_date, 'DD-Mon-RRRR') opr_date,
                 TO_CHAR (opr_date, 'DD-Mon-RRRR') receipt_date,
                 (SELECT 'NEW BUSINESS'
                    FROM DUAL
                   WHERE opr_date - pol_effective_date <= 365
                  UNION
                  SELECT 'RENEWAL'
                    FROM DUAL
                   WHERE opr_date - pol_effective_date > 365) endr_type,
                 pol_cla_code,POL_INSTLMT_PREM,POL_PENS_INSTLMT_PREM,pol_term
            FROM lms_ord_prem_receipts,
                 lms_policies,
                 lms_policy_endorsements a,
                 lms_products,
                 lms_proposers,
                 lms_agencies,
                 tqc_branches,
                 (SELECT a.osd_name coverage_area, b.osd_name spoke,
                         c.osd_name sbu, a.osd_id coverage_area_code,
                         b.osd_id spoke_code, c.osd_id sbu_code
                    FROM tqc_org_division_levels_type,
                         tqc_org_division_levels,
                         tqc_org_subdivisions a,
                         tqc_org_subdivisions b,
                         tqc_org_subdivisions c
                   WHERE odl_dlt_code = dlt_code
                     AND a.osd_odl_code = odl_code
                     AND dlt_act_code = 100
                     AND odl_ranking = 4
                     AND b.osd_code = a.osd_parent_osd_code
                     AND c.osd_code = b.osd_parent_osd_code),
                 (SELECT a.osd_name locations, b.osd_name ORGANIZATION,
                         a.osd_id location_code
                    FROM tqc_org_division_levels_type,
                         tqc_org_division_levels,
                         tqc_org_subdivisions a,
                         tqc_org_subdivisions b
                   WHERE odl_dlt_code = dlt_code
                     AND a.osd_odl_code = odl_code
                     AND dlt_act_code = 101
                     AND odl_ranking = 2
                     AND b.osd_code = a.osd_parent_osd_code)
           WHERE opr_pol_code = pol_code
             AND pol_current_endr_code = endr_code
             AND pol_client_prp_code = prp_code
             AND pol_prod_code = prod_code
             AND a.endr_coverage_area_code = coverage_area_code(+)
             AND a.endr_sbu_code = sbu_code(+)
             AND a.endr_spoke_code = spoke_code(+)
             AND a.endr_location_code = location_code(+)
--AND ITB_ORT_CODE IS NULL
             AND pol_cla_code = 20021
             AND agn_code = brn_agn_code(+)
             AND endr_agen_code = agn_code
             --AND sbu_code =
                 --   DECODE (NVL (:v_sbu_code, -2000),
                       --     -2000, sbu_code,
                       --     :v_sbu_code
                       --    )
             AND TRUNC (opr_date) BETWEEN NVL (To_date('{0}','DD-Mon-YYYY'), TRUNC (opr_date))
                                      AND NVL (To_date('{1}','DD-Mon-YYYY'), TRUNC (opr_date))
             AND opr_source LIKE '%MATURITY PROCESSING%'
          UNION ALL
          SELECT NULL itb_ref3, pol_bra_code, pol_policy_no,
                 prp_surname || ' ' || prp_other_names client, opr_receipt_no,
                 opr_date, REPLACE (opr_source, '_', ' ') oprsource,
                 DECODE (NVL (endr_pay_method, 'C'),
                         'K', 'CHECK-OFF',
                         'OTHER'
                        ) paymthd,
                 NULL dr_acc,
                 (SELECT tgl_crt_acc_number
                    FROM fms_trngl
                   WHERE tgl_narrative LIKE '%O/S PREM RECOVERY%'
                     AND ROWNUM = 1) cr_acc,
                 sbu, coverage_area, spoke, locations,
                 DECODE (opr_drcr, 'C', NVL (opr_amt, 0), 0) credit,
                 DECODE (opr_drcr, 'D', NVL (opr_amt, 0), 0) debit, prod_desc,
                   DECODE (opr_drcr,
                           'C', NVL (opr_amt, 0),
                           0
                          )
                 + DECODE (opr_drcr, 'D', NVL (opr_amt, 0) * -1, 0)
                                                             endr_tot_premium,
                 agn_name, agn_sht_desc, pol_proposal_no,
                 TO_CHAR (ENDR_AUTHORIZATION_DATE, 'DD-Mon-RRRR') ENDR_AUTHORIZATION_DATE,
                 TO_CHAR (opr_date, 'DD-Mon-RRRR') opr_date,
                 TO_CHAR (opr_date, 'DD-Mon-RRRR') receipt_date,
                 (SELECT 'NEW BUSINESS'
                    FROM DUAL
                   WHERE opr_date - pol_effective_date <= 365
                  UNION
                  SELECT 'RENEWAL'
                    FROM DUAL
                   WHERE opr_date - pol_effective_date > 365) endr_type,
                 pol_cla_code,POL_INSTLMT_PREM,POL_PENS_INSTLMT_PREM,pol_term
            FROM lms_ord_prem_receipts,
                 lms_policies,
                 lms_policy_endorsements a,
                 lms_products,
                 lms_proposers,
                 lms_agencies,
                 tqc_branches,
                 (SELECT a.osd_name coverage_area, b.osd_name spoke,
                         c.osd_name sbu, a.osd_id coverage_area_code,
                         b.osd_id spoke_code, c.osd_id sbu_code
                    FROM tqc_org_division_levels_type,
                         tqc_org_division_levels,
                         tqc_org_subdivisions a,
                         tqc_org_subdivisions b,
                         tqc_org_subdivisions c
                   WHERE odl_dlt_code = dlt_code
                     AND a.osd_odl_code = odl_code
                     AND dlt_act_code = 100
                     AND odl_ranking = 4
                     AND b.osd_code = a.osd_parent_osd_code
                     AND c.osd_code = b.osd_parent_osd_code),
                 (SELECT a.osd_name locations, b.osd_name ORGANIZATION,
                         a.osd_id location_code
                    FROM tqc_org_division_levels_type,
                         tqc_org_division_levels,
                         tqc_org_subdivisions a,
                         tqc_org_subdivisions b
                   WHERE odl_dlt_code = dlt_code
                     AND a.osd_odl_code = odl_code
                     AND dlt_act_code = 101
                     AND odl_ranking = 2
                     AND b.osd_code = a.osd_parent_osd_code)
           WHERE opr_pol_code = pol_code
             AND pol_current_endr_code = endr_code
             AND pol_client_prp_code = prp_code
             AND pol_prod_code = prod_code
             AND a.endr_coverage_area_code = coverage_area_code(+)
             AND a.endr_sbu_code = sbu_code(+)
             AND a.endr_spoke_code = spoke_code(+)
             AND a.endr_location_code = location_code(+)
             AND pol_cla_code = 20021
             AND agn_code = brn_agn_code(+)
             AND endr_agen_code = agn_code
             --AND sbu_code =
             --       DECODE (NVL (:v_sbu_code, -2000),
              --              -2000, sbu_code,
              --              :v_sbu_code
              --             )
             AND TRUNC (opr_date) BETWEEN NVL (To_date('{0}','DD-Mon-YYYY'), TRUNC (opr_date))
                                      AND NVL (To_date('{1}','DD-Mon-YYYY'), TRUNC (opr_date))
             AND opr_source LIKE '%CLAIM PROCESSING%'
          UNION ALL
          SELECT DISTINCT itb_ref3, pol_bra_code, pol_policy_no,
                          prp_surname || ' ' || prp_other_names client,
                          opr_receipt_no, opr_date,
                          REPLACE (opr_source, '_', ' ') oprsource,
                          DECODE (NVL (endr_pay_method, 'C'),
                                  'K', 'CHECK-OFF',
                                  'OTHER'
                                 ) paymthd,
                          itb_acc_number dr_acc, itb_acc_number cr_acc, sbu,
                          coverage_area, spoke, locations,
                          DECODE (itb_drcr,
                                  'C', NVL (itb_amount, 0),
                                  0
                                 ) credit,
                          DECODE (itb_drcr,
                                  'D', NVL (itb_amount, 0),
                                  0
                                 ) debit, prod_desc,
                            DECODE (itb_drcr,
                                    'C', NVL (itb_amount, 0),
                                    0
                                   )
                          + DECODE (itb_drcr, 'D', NVL (itb_amount, 0) * -1,
                                    0) endr_tot_premium,
                          agn_name, agn_sht_desc, pol_proposal_no,
                          TO_CHAR (ENDR_AUTHORIZATION_DATE,
                                   'DD-Mon-RRRR'
                                  ) ENDR_AUTHORIZATION_DATE,
                          TO_CHAR (itb_date, 'DD-Mon-RRRR') itb_date,
                          TO_CHAR (opr_date, 'DD-Mon-RRRR') receipt_date,
                          (SELECT 'NEW BUSINESS'
                             FROM DUAL
                            WHERE opr_date - pol_effective_date <=
                                                                365
                           UNION
                           SELECT 'RENEWAL'
                             FROM DUAL
                            WHERE opr_date - pol_effective_date >
                                                                365)
                                                                    endr_type,
                          pol_cla_code,POL_INSTLMT_PREM,POL_PENS_INSTLMT_PREM,pol_term
                     FROM lms_interface_table a,
                          lms_ord_prem_receipts,
                          lms_policies,
                          lms_policy_endorsements,
                          lms_products,
                          lms_proposers,
                          lms_agencies,
                          tqc_branches,
                          (SELECT a.osd_name coverage_area, b.osd_name spoke,
                                  c.osd_name sbu, a.osd_id coverage_area_code,
                                  b.osd_id spoke_code, c.osd_id sbu_code
                             FROM tqc_org_division_levels_type,
                                  tqc_org_division_levels,
                                  tqc_org_subdivisions a,
                                  tqc_org_subdivisions b,
                                  tqc_org_subdivisions c
                            WHERE odl_dlt_code = dlt_code
                              AND a.osd_odl_code = odl_code
                              AND dlt_act_code = 100
                              AND odl_ranking = 4
                              AND b.osd_code = a.osd_parent_osd_code
                              AND c.osd_code = b.osd_parent_osd_code),
                          (SELECT a.osd_name locations,
                                  b.osd_name ORGANIZATION,
                                  a.osd_id location_code
                             FROM tqc_org_division_levels_type,
                                  tqc_org_division_levels,
                                  tqc_org_subdivisions a,
                                  tqc_org_subdivisions b
                            WHERE odl_dlt_code = dlt_code
                              AND a.osd_odl_code = odl_code
                              AND dlt_act_code = 101
                              AND odl_ranking = 2
                              AND b.osd_code = a.osd_parent_osd_code)
                    WHERE itb_transaction_type <> 'UPDATE_VGL'
                      AND itb_opr_code = opr_code
                      AND opr_pol_code = pol_code
                      AND pol_current_endr_code = endr_code
                      AND pol_client_prp_code = prp_code
                      AND pol_prod_code = prod_code
                      AND a.itb_covrg_area_code = coverage_area_code(+)
                      AND a.itb_sbu_code = sbu_code(+)
                      AND a.itb_spoke_code = spoke_code(+)
                      AND a.itb_loc_code = location_code(+)
                      AND itb_ort_code IS NULL
                      AND pol_cla_code = 20021
                      AND agn_code = brn_agn_code(+)
                      AND endr_agen_code = agn_code
                 --     AND sbu_code =
                      --       DECODE (NVL (:v_sbu_code, -2000),
                     --                -2000, sbu_code,
                        --             :v_sbu_code
                        --            )
                      AND TRUNC (itb_date) BETWEEN NVL (To_date('{0}','DD-Mon-YYYY'),
                                                        TRUNC (itb_date)
                                                       )
                                               AND NVL (To_date('{1}','DD-Mon-YYYY'),
                                                        TRUNC (itb_date)
                                                       )
                      AND NOT itb_transaction_code IN (
                             SELECT itb_transaction_code
                               FROM lms_interface_table
                              WHERE itb_transaction_code =
                                                        a.itb_transaction_code
                                AND itb_transaction_type = 'UPDATE_VGL'
                                AND itb_narrative LIKE 'LMS RCPT%')
          UNION ALL
          SELECT DISTINCT itb_ref3, pol_bra_code, pol_policy_no,
                          prp_surname || ' ' || prp_other_names client,
                          opr_receipt_no, opr_date,
                          REPLACE (opr_source, '_', ' ') oprsource,
                          DECODE (NVL (endr_pay_method, 'C'),
                                  'K', 'CHECK-OFF',
                                  'OTHER'
                                 ) paymthd,
                          itb_acc_number dr_acc, itb_acc_number cr_acc, sbu,
                          coverage_area, spoke, locations,
                          DECODE (itb_drcr,
                                  'C', NVL (itb_amount, 0),
                                  0
                                 ) credit,
                          DECODE (itb_drcr,
                                  'D', NVL (itb_amount, 0),
                                  0
                                 ) debit, prod_desc,
                            DECODE (itb_drcr,
                                    'C', NVL (itb_amount, 0),
                                    0
                                   )
                          + DECODE (itb_drcr, 'D', NVL (itb_amount, 0) * -1,
                                    0) endr_tot_premium,
                          agn_name, agn_sht_desc, pol_proposal_no,
                          TO_CHAR (ENDR_AUTHORIZATION_DATE,
                                   'DD-Mon-RRRR'
                                  ) ENDR_AUTHORIZATION_DATE,
                          TO_CHAR (itb_date, 'DD-Mon-RRRR') itb_date,
                          TO_CHAR (opr_date, 'DD-Mon-RRRR') receipt_date,
                          (SELECT 'NEW BUSINESS'
                             FROM DUAL
                            WHERE opr_date - pol_effective_date <=
                                                                365
                           UNION
                           SELECT 'RENEWAL'
                             FROM DUAL
                            WHERE opr_date - pol_effective_date >
                                                                365)
                                                                    endr_type,
                          pol_cla_code,POL_INSTLMT_PREM,POL_PENS_INSTLMT_PREM,pol_term
                     FROM lms_interface_table a,
                          lms_ord_prem_receipts,
                          lms_policies,
                          lms_policy_endorsements,
                          lms_products,
                          lms_proposers,
                          lms_agencies,
                          tqc_branches,
                          (SELECT a.osd_name coverage_area, b.osd_name spoke,
                                  c.osd_name sbu, a.osd_id coverage_area_code,
                                  b.osd_id spoke_code, c.osd_id sbu_code
                             FROM tqc_org_division_levels_type,
                                  tqc_org_division_levels,
                                  tqc_org_subdivisions a,
                                  tqc_org_subdivisions b,
                                  tqc_org_subdivisions c
                            WHERE odl_dlt_code = dlt_code
                              AND a.osd_odl_code = odl_code
                              AND dlt_act_code = 100
                              AND odl_ranking = 4
                              AND b.osd_code = a.osd_parent_osd_code
                              AND c.osd_code = b.osd_parent_osd_code),
                          (SELECT a.osd_name locations,
                                  b.osd_name ORGANIZATION,
                                  a.osd_id location_code
                             FROM tqc_org_division_levels_type,
                                  tqc_org_division_levels,
                                  tqc_org_subdivisions a,
                                  tqc_org_subdivisions b
                            WHERE odl_dlt_code = dlt_code
                              AND a.osd_odl_code = odl_code
                              AND dlt_act_code = 101
                              AND odl_ranking = 2
                              AND b.osd_code = a.osd_parent_osd_code)
                    WHERE itb_transaction_type <> 'UPDATE_VGL'
                      AND itb_opr_code = opr_code
                      AND opr_pol_code = pol_code
                      AND pol_current_endr_code = endr_code
                      AND pol_client_prp_code = prp_code
                      AND pol_prod_code = prod_code
                      AND a.itb_covrg_area_code = coverage_area_code(+)
                      AND a.itb_sbu_code = sbu_code(+)
                      AND a.itb_spoke_code = spoke_code(+)
                      AND a.itb_loc_code = location_code(+)
                      AND itb_ort_code IS NULL
                      AND agn_code = brn_agn_code(+)
                      AND endr_agen_code = agn_code
                      AND pol_cla_code = 20021
                    --  AND sbu_code =
                       --      DECODE (NVL (:v_sbu_code, -2000),
                       --              -2000, sbu_code,
                       --              :v_sbu_code
                       --             )
                      AND TRUNC (itb_date) BETWEEN NVL (To_date('{0}','DD-Mon-YYYY'),
                                                        TRUNC (itb_date)
                                                       )
                                               AND NVL (To_date('{1}','DD-Mon-YYYY'),
                                                        TRUNC (itb_date)
                                                       )
                      AND itb_transaction_code IN (
                             SELECT itb_transaction_code
                               FROM lms_interface_table
                              WHERE itb_transaction_code =
                                                        a.itb_transaction_code
                                AND itb_transaction_type = 'UPDATE_VGL'
                                AND itb_narrative LIKE 'LMS RCPT%')
          UNION ALL
          SELECT DISTINCT itb_ref3, pol_bra_code, pol_policy_no,
                          prp_surname || ' ' || prp_other_names client,
                          opr_receipt_no, opr_date,
                          REPLACE (opr_source, '_', ' ') oprsource,
                          DECODE (NVL (endr_pay_method, 'C'),
                                  'K', 'CHECK-OFF',
                                  'OTHER'
                                 ) paymthd,
                          itb_acc_number dr_acc, itb_acc_number cr_acc, sbu,
                          coverage_area, spoke, locations,
                          DECODE (itb_drcr,
                                  'C', NVL (itb_amount, 0),
                                  0
                                 ) credit,
                          DECODE (itb_drcr,
                                  'D', NVL (itb_amount, 0),
                                  0
                                 ) debit, prod_desc,
                            DECODE (itb_drcr,
                                    'C', NVL (itb_amount, 0),
                                    0
                                   )
                          + DECODE (itb_drcr, 'D', NVL (itb_amount, 0) * -1,
                                    0) endr_tot_premium,
                          agn_name, agn_sht_desc, pol_proposal_no,
                          TO_CHAR (ENDR_AUTHORIZATION_DATE,
                                   'DD-Mon-RRRR'
                                  ) ENDR_AUTHORIZATION_DATE,
                          TO_CHAR (itb_date, 'DD-Mon-RRRR') itb_date,
                          TO_CHAR (opr_date, 'DD-Mon-RRRR') receipt_date,
                          (SELECT 'NEW BUSINESS'
                             FROM DUAL
                            WHERE opr_date - pol_effective_date <=
                                                                365
                           UNION
                           SELECT 'RENEWAL'
                             FROM DUAL
                            WHERE opr_date - pol_effective_date >
                                                                365)
                                                                    endr_type,
                          pol_cla_code,POL_INSTLMT_PREM,POL_PENS_INSTLMT_PREM,pol_term
                     FROM lms_interface_table a,
                          lms_ord_prem_receipts,
                          lms_policies,
                          lms_policy_endorsements,
                          lms_products,
                          lms_proposers,
                          lms_agencies,
                          tqc_branches,
                          (SELECT a.osd_name coverage_area, b.osd_name spoke,
                                  c.osd_name sbu, a.osd_id coverage_area_code,
                                  b.osd_id spoke_code, c.osd_id sbu_code
                             FROM tqc_org_division_levels_type,
                                  tqc_org_division_levels,
                                  tqc_org_subdivisions a,
                                  tqc_org_subdivisions b,
                                  tqc_org_subdivisions c
                            WHERE odl_dlt_code = dlt_code
                              AND a.osd_odl_code = odl_code
                              AND dlt_act_code = 100
                              AND odl_ranking = 4
                              AND b.osd_code = a.osd_parent_osd_code
                              AND c.osd_code = b.osd_parent_osd_code),
                          (SELECT a.osd_name locations,
                                  b.osd_name ORGANIZATION,
                                  a.osd_id location_code
                             FROM tqc_org_division_levels_type,
                                  tqc_org_division_levels,
                                  tqc_org_subdivisions a,
                                  tqc_org_subdivisions b
                            WHERE odl_dlt_code = dlt_code
                              AND a.osd_odl_code = odl_code
                              AND dlt_act_code = 101
                              AND odl_ranking = 2
                              AND b.osd_code = a.osd_parent_osd_code)
                    WHERE itb_transaction_type <> 'UPDATE_VGL'
                      AND itb_opr_code = opr_code
                      AND opr_pol_code = pol_code
                      AND pol_current_endr_code = endr_code
                      AND pol_client_prp_code = prp_code
                      AND pol_prod_code = prod_code
                      AND a.itb_covrg_area_code = coverage_area_code(+)
                      AND a.itb_sbu_code = sbu_code(+)
                      AND a.itb_spoke_code = spoke_code(+)
                      AND a.itb_loc_code = location_code(+)
                      AND itb_ort_code IS NULL
                      AND agn_code = brn_agn_code(+)
                      AND endr_agen_code = agn_code
                      AND pol_cla_code = 20021
--AND POL_POLICY_NO LIKE 'UACP/614/000001/HQ'
             --         AND sbu_code =
              --               DECODE (NVL (:v_sbu_code, -2000),
              --                       -2000, sbu_code,
              --                       :v_sbu_code
              --                      )
                      AND TRUNC (itb_date) BETWEEN NVL (To_date('{0}','DD-Mon-YYYY'),
                                                        TRUNC (itb_date)
                                                       )
                                               AND NVL (To_date('{1}','DD-Mon-YYYY'),
                                                        TRUNC (itb_date)
                                                       )
                      AND itb_transaction_code IN (
                             SELECT itb_transaction_code
                               FROM lms_interface_table
                              WHERE itb_transaction_code =
                                                        a.itb_transaction_code
                                AND itb_transaction_type = 'UPDATE_VGL'
                                AND itb_narrative LIKE 'LMS RCPT%'
--GROUP BY ITB_TRANSACTION_CODE
                          )
          UNION ALL
          SELECT   itb_ref3, pol_bra_code, pol_policy_no,
                   prp_surname || ' ' || prp_other_names client,
                   opr_receipt_no, opr_date,
                   REPLACE (opr_source, '_', ' ') oprsource,
                   DECODE (NVL (endr_pay_method, 'C'),
                           'K', 'CHECK-OFF',
                           'OTHER'
                          ) paymthd,
                   itb_acc_number dr_acc, itb_acc_number cr_acc, sbu,
                   coverage_area, spoke, locations,
                   DECODE (itb_drcr, 'C', NVL (itb_amount, 0), 0) credit,
                   DECODE (itb_drcr, 'D', NVL (itb_amount, 0), 0) debit,
                   prod_desc,
                     DECODE (itb_drcr,
                             'C', NVL (itb_amount, 0),
                             0
                            )
                   + DECODE (itb_drcr, 'D', NVL (itb_amount, 0) * -1, 0)
                                                             endr_tot_premium,
                   agn_name, agn_sht_desc, pol_proposal_no,
                   TO_CHAR (ENDR_AUTHORIZATION_DATE, 'DD-Mon-RRRR') ENDR_AUTHORIZATION_DATE,
                   TO_CHAR (itb_date, 'DD-Mon-RRRR') itb_date,
                   TO_CHAR (opr_date, 'DD-Mon-RRRR') receipt_date,
                   (SELECT 'NEW BUSINESS'
                      FROM DUAL
                     WHERE opr_date - pol_effective_date <= 365
                    UNION
                    SELECT 'RENEWAL'
                      FROM DUAL
                     WHERE opr_date - pol_effective_date > 365) endr_type,
                   pol_cla_code,POL_INSTLMT_PREM,POL_PENS_INSTLMT_PREM,pol_term
              FROM lms_interface_table a,
                   lms_ord_prem_receipts,
                   lms_policies,
                   lms_policy_endorsements,
                   lms_products,
                   lms_proposers,
                   lms_ord_recpt_transfer,
                   lms_agencies,
                   tqc_branches,
                   (SELECT a.osd_name coverage_area, b.osd_name spoke,
                           c.osd_name sbu, a.osd_id coverage_area_code,
                           b.osd_id spoke_code, c.osd_id sbu_code
                      FROM tqc_org_division_levels_type,
                           tqc_org_division_levels,
                           tqc_org_subdivisions a,
                           tqc_org_subdivisions b,
                           tqc_org_subdivisions c
                     WHERE odl_dlt_code = dlt_code
                       AND a.osd_odl_code = odl_code
                       AND dlt_act_code = 100
                       AND odl_ranking = 4
                       AND b.osd_code = a.osd_parent_osd_code
                       AND c.osd_code = b.osd_parent_osd_code),
                   (SELECT a.osd_name locations, b.osd_name ORGANIZATION,
                           a.osd_id location_code
                      FROM tqc_org_division_levels_type,
                           tqc_org_division_levels,
                           tqc_org_subdivisions a,
                           tqc_org_subdivisions b
                     WHERE odl_dlt_code = dlt_code
                       AND a.osd_odl_code = odl_code
                       AND dlt_act_code = 101
                       AND odl_ranking = 2
                       AND b.osd_code = a.osd_parent_osd_code)
             WHERE NOT itb_transaction_type = 'UPDATE_VGL'
               AND opr_pol_code = pol_code
               AND itb_pol_code = pol_code
               AND pol_current_endr_code = endr_code
               AND pol_client_prp_code = prp_code
               AND a.itb_covrg_area_code = coverage_area_code(+)
               AND a.itb_sbu_code = sbu_code(+)
               AND a.itb_spoke_code = spoke_code(+)
               AND a.itb_loc_code = location_code(+)
               AND pol_prod_code = prod_code
               AND opr_source = 'TRANSFER'
               AND itb_vchr_no = 'PR_PL'
               AND itb_transaction_code IN (
                      SELECT itb_transaction_code
                        FROM lms_interface_table
                       WHERE itb_transaction_code = a.itb_transaction_code
                         AND itb_transaction_type = 'UPDATE_VGL'
                         AND TRUNC (itb_date) BETWEEN NVL (To_date('{0}','DD-Mon-YYYY'),
                                                           TRUNC (itb_date)
                                                          )
                                                  AND NVL (To_date('{1}','DD-Mon-YYYY'),
                                                           TRUNC (itb_date)
                                                          ))
               AND ort_opr_code_to = opr_code
               AND itb_ort_code = ort_code
               AND itb_drcr = 'C'
               AND itb_ppr_code IS NULL
               AND itb_narrative NOT LIKE 'PHCF%'
               AND itb_narrative NOT LIKE 'INV. FEE. FROM%'
               AND ort_transfer_from = 'PR_PREM;'
               AND ort_transfer_to = 'PL_PREM;'
               AND agn_code = brn_agn_code(+)
               AND pol_cla_code = 20021
               AND endr_agen_code = agn_code
--AND POL_POLICY_NO LIKE 'UACP/614/000001/HQ'
             --  AND sbu_code =
             --         DECODE (NVL (:v_sbu_code, -2000),
            --                 -2000, sbu_code,
            --                  :v_sbu_code
            --                 )
          GROUP BY itb_ref3,
                   pol_bra_code,
                   pol_policy_no,
                   prp_surname || ' ' || prp_other_names,
                   opr_receipt_no,
                   opr_date,
                   opr_source,
                   NVL (TRUNC (opr_production_date), opr_date),
                   DECODE (NVL (endr_pay_method, 'C'),
                           'K', 'CHECK-OFF',
                           'OTHER'
                          ),
                   sbu,
                   coverage_area,
                   ENDR_AUTHORIZATION_DATE,
                   spoke,
                   locations,
                   itb_drcr,
                   endr_pay_method,
                   itb_amount,
                   prod_desc,
                   endr_tot_premium,
                   agn_name,
                   agn_sht_desc,
                   pol_proposal_no,
                   itb_date,
                   opr_date,
                   endr_type,
                   pol_cla_code,
                   itb_acc_number,
                   opr_date,
                   pol_effective_date,POL_INSTLMT_PREM,POL_PENS_INSTLMT_PREM,pol_term
          UNION ALL
          SELECT   itb_ref3, pol_bra_code, pol_policy_no,
                   prp_surname || ' ' || prp_other_names client,
                   opr_receipt_no, opr_date,
                   REPLACE (opr_source, '_', ' ') oprsource,
                   DECODE (NVL (endr_pay_method, 'C'),
                           'K', 'CHECK-OFF',
                           'OTHER'
                          ) paymthd,
                   itb_acc_number dr_acc, itb_acc_number cr_acc, sbu,
                   coverage_area, spoke, locations,
                   DECODE (itb_drcr, 'C', NVL (itb_amount, 0), 0) credit,
                   DECODE (itb_drcr, 'D', NVL (itb_amount, 0), 0) debit,
                   prod_desc, endr_tot_premium, agn_name, agn_sht_desc,
                   pol_proposal_no,
                   TO_CHAR (ENDR_AUTHORIZATION_DATE, 'DD-Mon-RRRR') ENDR_AUTHORIZATION_DATE,
                   TO_CHAR (itb_date, 'DD-Mon-RRRR') itb_date,
                   TO_CHAR (opr_date, 'DD-Mon-RRRR') receipt_date,
                   (SELECT 'NEW BUSINESS'
                      FROM DUAL
                     WHERE opr_date - pol_effective_date <= 365
                    UNION
                    SELECT 'RENEWAL'
                      FROM DUAL
                     WHERE opr_date - pol_effective_date > 365) endr_type,
                   pol_cla_code,POL_INSTLMT_PREM,POL_PENS_INSTLMT_PREM,pol_term
              FROM lms_interface_table a,
                   lms_ord_prem_receipts,
                   lms_policies,
                   lms_policy_endorsements,
                   lms_products,
                   lms_proposers,
                   lms_ord_recpt_transfer,
                   lms_agencies,
                   tqc_branches,
                   (SELECT a.osd_name coverage_area, b.osd_name spoke,
                           c.osd_name sbu, a.osd_id coverage_area_code,
                           b.osd_id spoke_code, c.osd_id sbu_code
                      FROM tqc_org_division_levels_type,
                           tqc_org_division_levels,
                           tqc_org_subdivisions a,
                           tqc_org_subdivisions b,
                           tqc_org_subdivisions c
                     WHERE odl_dlt_code = dlt_code
                       AND a.osd_odl_code = odl_code
                       AND dlt_act_code = 100
                       AND odl_ranking = 4
                       AND b.osd_code = a.osd_parent_osd_code
                       AND c.osd_code = b.osd_parent_osd_code),
                   (SELECT a.osd_name locations, b.osd_name ORGANIZATION,
                           a.osd_id location_code
                      FROM tqc_org_division_levels_type,
                           tqc_org_division_levels,
                           tqc_org_subdivisions a,
                           tqc_org_subdivisions b
                     WHERE odl_dlt_code = dlt_code
                       AND a.osd_odl_code = odl_code
                       AND dlt_act_code = 101
                       AND odl_ranking = 2
                       AND b.osd_code = a.osd_parent_osd_code)
             WHERE NOT itb_transaction_type = 'UPDATE_VGL'
               AND opr_pol_code = pol_code
               AND pol_current_endr_code = endr_code
               AND pol_client_prp_code = prp_code
               AND pol_prod_code = prod_code
               AND itb_pol_code = pol_code
               AND itb_opr_code = opr_code
               AND itb_vchr_no = 'PL_PR'
               AND itb_drcr = 'D'
               AND itb_ppr_code IS NULL
               AND a.itb_covrg_area_code = coverage_area_code(+)
               AND a.itb_sbu_code = sbu_code(+)
               AND a.itb_spoke_code = spoke_code(+)
               AND a.itb_loc_code = location_code(+)
               AND itb_narrative NOT LIKE 'PHCF%'
               AND itb_narrative NOT LIKE 'INV. FEE. FROM%'
               AND (   lms_reports_pkg.valid_new
                                            (pol_effective_date,
                                             NVL (TRUNC (opr_production_date),
                                                  opr_date
                                                 ),
                                             To_date('{0}','DD-Mon-YYYY'),
                                             To_date('{1}','DD-Mon-YYYY')
                                            ) = 1
                    OR lms_reports_pkg.valid_renew
                                            (pol_effective_date,
                                             NVL (TRUNC (opr_production_date),
                                                  opr_date
                                                 ),
                                             To_date('{0}','DD-Mon-YYYY'),
                                             To_date('{1}','DD-Mon-YYYY')
                                            ) = 1
                   )
               AND TRUNC (itb_date) BETWEEN NVL (To_date('{0}','DD-Mon-YYYY'),
                                                 TRUNC (itb_date)
                                                )
                                        AND NVL (To_date('{1}','DD-Mon-YYYY'), TRUNC (itb_date))
               AND opr_code = ort_opr_code_from
               AND itb_ort_code = ort_code
               AND agn_code = brn_agn_code(+)
               AND endr_agen_code = agn_code
               AND pol_cla_code = 20021
--AND POL_POLICY_NO LIKE 'UACP/614/000001/HQ'
        --       AND sbu_code =
         --             DECODE (NVL (:v_sbu_code, -2000),
          --                    -2000, sbu_code,
          --                    :v_sbu_code
          --                   )
          GROUP BY opr_date,
                   pol_effective_date,
                   itb_ref3,
                   pol_bra_code,
                   pol_policy_no,
                   prp_surname || ' ' || prp_other_names,
                   opr_receipt_no,
                   opr_date,
                   opr_source,
                   NVL (TRUNC (opr_production_date), opr_date),
                   DECODE (NVL (endr_pay_method, 'C'),
                           'K', 'CHECK-OFF',
                           'OTHER'
                          ),
                   sbu,
                   coverage_area,
                   itb_date,
                   spoke,
                   ENDR_AUTHORIZATION_DATE,
                   locations,
                   itb_drcr,
                   itb_acc_number,
                   itb_acc_number2,
                   itb_amount,
                   prod_desc,
                   endr_tot_premium,
                   pol_proposal_no,
                   itb_acc_number,
                   agn_name,
                   agn_sht_desc,
                   opr_date,
                   endr_type,
                   pol_cla_code,POL_INSTLMT_PREM,POL_PENS_INSTLMT_PREM,pol_term)
   WHERE pol_bra_code = brn_code
--AND (DR_ACC =DR_ACC OR CR_ACC =CR_ACC)
--AND POL_POLICY_NO LIKE 'E/ES/2016/110611'
         AND pol_cla_code = 20021
         AND spoke='Agency'
         AND ENDR_TYPE='NEW BUSINESS'
-- AND NVL(GROSS_NEW,0) <> 0
GROUP BY itb_ref3,
         brn_sht_desc,
         brn_name,
         pol_policy_no,
         client,
         opr_receipt_no,
         opr_date,
         oprsource,
         dr_acc,
         cr_acc,
         itb_date,
         paymthd,
         sbu,
         coverage_area,
         spoke,
         locations,
         credit,
         debit,
         prod_desc,
         endr_tot_premium,
         agn_name,
         agn_sht_desc,
         pol_proposal_no,
         ENDR_AUTHORIZATION_DATE,
         receipt_date,
         endr_type,
         pol_cla_code,
         POL_INSTLMT_PREM,POL_PENS_INSTLMT_PREM,pol_term
ORDER BY pol_policy_no DESC) ggg)", startDate, endDate);
            #endregion
            var lmsUrl = ConfigurationManager.AppSettings["lmsUrl"].ToString();
            var con = new OracleConnection { ConnectionString = lmsUrl };
            try
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = query;
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //Spoke,AGN_NAME,AGN_SHT_DESC,ENDR_TYPE,PREMIUM from
                        var production = new Production
                        {
                            AgentCode = reader[2].ToString(),
                            AgentName = reader[1].ToString(),
                            Premium = decimal.Parse(reader[4].ToString()),
                            TransactionType = reader[3].ToString(),
                            Spoke = reader[0].ToString(),
                        };
                        productionReport.Add(production);
                    }
                }
                con.Close();
                return productionReport;
            }
            catch (Exception ex)
            {
                con.Close();
                JUtility.ErrorLog.LogApplicationError(ex);
                return productionReport;
            }
        }
        public List<Production> GetGroupLifeProduction(string startDate, string endDate)
        {
            var productionReport = new List<Production>();
            #region Query
            var query = string.Format(@"Select AGN_SHT_DESC,AGN_NAME, ENDR_TOT_PREMIUM,SPOKE,TRANS_TYPE from (SELECT prp_clnt_code, prp_code,POL_POLICY_NO,AGN_SHT_DESC,POL_PROPOSAL_NO,ENDR_CODE,PROD_DESC,AGN_NAME,AGN_ACT_CODE,AGN_CODE,PRP_SURNAME||' '||PRP_OTHER_NAMES CLIENT_NAME,
        NVL(ENDR_TOT_SA,0) ENDR_TOT_SA, ENDR_NO,
        --NVL(ENDR_TOT_PREMIUM,0) ENDR_TOT_PREMIUM,
        DECODE(SIGN(ENDR_ADD_REF_PREM),1,nvl(ENDR_ADD_REF_PREM,0),0) + DECODE(SIGN(ENDR_ADD_REF_PREM),-1,nvl(ENDR_ADD_REF_PREM,0),0) ENDR_TOT_PREMIUM, ENDR_COMM_AMT,
        DECODE(SIGN(ENDR_ADD_REF_PREM),-1,nvl(ENDR_ADD_REF_PREM,0),0)CREDIT,DECODE(SIGN(ENDR_ADD_REF_PREM),1,nvl(ENDR_ADD_REF_PREM,0),0)DEBIT,
        BRN_NAME,TO_CHAR(POL_AUTHORIZATION_DATE,'DD-Mon-RRRR')AUTHO_DATE,DECODE(ENDR_TYPE,'NB','NEW BUSINESS','CN','CANCELLATION','PU','PAID UP','CE','CHANGE OF EFFECTIVE DATE','CD',
        'CHANGE OF DEPENDANTS','CA','CHANGE OF ANB','CH','CHANGE OF OPTION','CS','CHANGE OF SUM ASSURED','CG','CHANGE OF CATEGORY/EARNINGS','CP','CHANGE OF PREMIUM','CM','CHANGE OF TERM','PR',
        'NON PREMIUM CHANGE','CR','CHANGE OF RIDERS','RN','RENEWAL','RD','REDATING','EN','ENDORSEMENT','CO','CONTRA','LP','LAPSATION','RE','REINSTATEMENT','RL','LOADING', 
        'CT','CHANGE OF DEPENDENT DETAILS','AP','ADDITIONAL PREMIUM','RP','REFUND OF PREMIUM', 'EC','EXTENSION OF COVER')ENDR_TYPE, 
        COVERAGE_AREA,COVERAGE_AREA_CODE,SPOKE_CODE,SPOKE,SBU_CODE,SBU,LOCATION_CODE,LOCATIONS,TO_CHAR(ENDR_AUTHORIZATION_DATE,'DD-Mon-RRRR')ITB_VGL_POST_DATE,ENDR_DRCR_NO DEBIT_NO,NVL(POL_OS_PREM_BAL_AMT,0)OS_PREM_BAL,
        TO_CHAR(ENDR_COVER_FROM_DATE,'DD-Mon-RRRR')ENDR_COVER_FROM_DATE, TO_CHAR((NVL(NVL(ENDR_COVER_TO_DATE, ENDR_RENEWAL_DATE-1),ENDR_COVER_FROM_DATE+99)),'DD-Mon-RRRR')ENDR_COVER_TO_DATE,
        TO_CHAR(nvl(ENDR_RENEWAL_DATE,ENDR_COVER_FROM_DATE + 100),'DD-Mon-RRRR')ENDR_RENEWAL_DATE, NVL(GRCT_AMT,0) GRCT_AMT,
         CASE(select ENDR_UW_YEAR  from LMS_POLICY_ENDORSEMENTS where ENDR_POL_POLICY_NO = POL_POLICY_NO and rownum = 1) - (select ENDR_UW_YEAR from LMS_POLICY_ENDORSEMENTS d where ENDR_POL_POLICY_NO = POL_POLICY_NO and d.ENDR_CODE = h.ENDR_CODE   and rownum = 1) when 0 then 'NEW BUSINESS'
 else 'RENEWAL' END as Trans_type
       FROM LMS_POLICIES,LMS_PRODUCTS,LMS_AGENCIES,LMS_PROPOSERS,LMS_POLICY_ENDORSEMENTS h,TQC_BRANCHES,LMS_CLASSES,
        ( SELECT A.OSD_NAME COVERAGE_AREA,
                        B.OSD_NAME SPOKE,
                        C.OSD_NAME SBU,A.OSD_ID  COVERAGE_AREA_CODE, B.OSD_ID   SPOKE_CODE, C.OSD_ID   SBU_CODE
            FROM TQC_ORG_DIVISION_LEVELS_TYPE,
                     TQC_ORG_DIVISION_LEVELS,
                     TQC_ORG_SUBDIVISIONS A,
                     TQC_ORG_SUBDIVISIONS B,
                     TQC_ORG_SUBDIVISIONS C
            WHERE ODL_DLT_CODE = DLT_CODE
                AND  A.OSD_ODL_CODE = ODL_CODE
                AND DLT_ACT_CODE = 100
                AND ODL_RANKING = 4
                AND B.OSD_CODE = A.OSD_PARENT_OSD_CODE
                AND C.OSD_CODE = B.OSD_PARENT_OSD_CODE
               
        ),
        (SELECT A.OSD_NAME LOCATIONS,
                        B.OSD_NAME ORGANIZATION, A.OSD_ID  LOCATION_CODE
            FROM TQC_ORG_DIVISION_LEVELS_TYPE,
                     TQC_ORG_DIVISION_LEVELS,
                     TQC_ORG_SUBDIVISIONS A,
                     TQC_ORG_SUBDIVISIONS B
            WHERE ODL_DLT_CODE = DLT_CODE
                AND  A.OSD_ODL_CODE = ODL_CODE
                AND DLT_ACT_CODE = 101
                AND ODL_RANKING = 2
                AND B.OSD_CODE = A.OSD_PARENT_OSD_CODE
               
        ),
        (SELECT GRCT_ENDR_CODE, SUM(NVL(GRCT_AMT,0))GRCT_AMT
         FROM LMS_GRP_PREM_RECEIPTS
         GROUP BY GRCT_ENDR_CODE
        )
        WHERE POL_PROD_CODE=PROD_CODE
        and ENDR_AGEN_CODE=AGN_CODE
        AND AGN_BRN_CODE=BRN_CODE(+)
        AND ENDR_POL_CODE=POL_CODE
        AND POL_PRP_CODE=PRP_CODE
        AND POL_CLIENT_PRP_CODE=PRP_CODE
        AND GRCT_ENDR_CODE(+) = ENDR_CODE
        --AND POL_CURRENT_ENDR_CODE = ENDR_CODE
        AND ENDR_COVERAGE_AREA_CODE=COVERAGE_AREA_CODE(+)
        AND ENDR_SPOKE_CODE=SPOKE_CODE(+)
        AND ENDR_SBU_CODE=SBU_CODE(+)
        AND ENDR_LOCATION_CODE=LOCATION_CODE(+)
        AND CLA_TYPE='G'
        AND CLA_CODE=POL_CLA_CODE
        AND DECODE(NVL(-2000,-2000),-2000, -2000, SBU_CODE)=DECODE(NVL(-2000,-2000),-2000,-2000,-2000)
        AND spoke='Agency'
        AND ENDR_AUTHORIZATION_DATE  BETWEEN '{0}'  AND '{1}'
        ORDER BY ENDR_AUTHORIZATION_DATE, POL_POLICY_NO, ENDR_NO) where Trans_type='NEW BUSINESS'", startDate, endDate);
            #endregion
            var lmsUrl = ConfigurationManager.AppSettings["lmsUrl"].ToString();
            var con = new OracleConnection { ConnectionString = lmsUrl };
            string clientShortCode = string.Empty;
            try
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = query;
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //AGN_SHT_DESC,AGN_NAME, ENDR_TOT_PREMIUM,SPOKE,TRANS_TYPE
                        var production = new Production
                        {
                            AgentCode = reader[0].ToString(),
                            AgentName = reader[1].ToString(),
                            Premium = decimal.Parse(reader[2].ToString()),
                            TransactionType = reader[4].ToString(),
                            Spoke = reader[3].ToString()

                        };
                        productionReport.Add(production);
                    }
                }
                con.Close();
                productionReport =
                    productionReport.FindAll(m => m.Spoke.Equals("Agency", StringComparison.InvariantCultureIgnoreCase));
                return productionReport;
            }
            catch (Exception ex)
            {
                con.Close();
                JUtility.ErrorLog.LogApplicationError(ex);
                return productionReport;
            }
        }
        public List<Production> GetLifeProductions(string startDate, string endDate)
        {
            var individual = GetIndividualLifeProduction(startDate, endDate);
            var group = GetGroupLifeProduction(startDate, endDate);
            var lifeProduction= new List<Production>();
            lifeProduction.AddRange(individual);
            lifeProduction.AddRange(group);
            return lifeProduction;
        }
    }
}
