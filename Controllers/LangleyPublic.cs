using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace WsSensitivity.Controllers
{
    public class LangleyPublic
    {
        public struct Langley_list
        {
            public int number;
            public int Id;
            public double PrecisionInstruments;
            public double StimulusQuantityCeiling;
            public double StimulusQuantityFloor;
            public string Power;
            public string DistributionState;
            public int Correction;
            public int FlipTheResponse;
            public string ExperimentalDate;
            public int count;
            public string projectname;
        }
        public struct Langley
        {
            public int ldt_Id;
            public int ldt_Number;
            public double ldt_StimulusQuantity;
            public int ldt_Response;
            public double ldt_Mean;
            public double ldt_StandardDeviation;
            public double ldt_MeanVariance;
            public double ldt_StandardDeviationVariance;
            public double ldt_Covmusigma;
            public double number;
            //public string LangleyName;
        }
        public struct XAndVArray
        {
            public double[] xArray;
            public int[] vArray;
        }

        public static List<string> aArray = new List<string>();
        public static List<string> bArray = new List<string>();
        public static List<string> cArray = new List<string>();
        public static SideReturnData sideReturnData = new SideReturnData();
        public static string incredibleIntervalType = "";
        public static string incredibleLevelName = "";

        public static LangleyAlgorithm SelectState(LangleyExperimentTable let)
        {
            LangleyDistributionSelection distributionSelection = null;
            LangleyMethodStandardSelection langleyMethod = null;
            if (let.let_DistributionState == 0)
                distributionSelection = new Normal();
            else if (let.let_DistributionState == 1)
                distributionSelection = new Logistic();

            if (let.let_StandardState == 0)
                langleyMethod = new Standard();
            else if (let.let_StandardState == 1)
                langleyMethod = new Ln();
            else if (let.let_StandardState == 2)
                langleyMethod = new Log();
            else if (let.let_StandardState == 3)
                langleyMethod = new Pow(double.Parse(let.let_Power));

            return new LangleyAlgorithm(distributionSelection, langleyMethod); ;

        }
        //public static string GetLangleyName(int id) {
        //    LangleyExperimentTable langlryExpTable = dbDrive.GetLangleyExperimentTable(id);
        //}

        public static LangleyDataTable LangleyDataTables(int langlryExpTableId, IDbDrive dbDrive, double sq = 0, int resp = 0, double mean = 0, double sd = 0, double mv = 0, double sdv = 0, double covmusigma = 0, string note = null)
        {
            LangleyDataTable ldt = new LangleyDataTable();
            ldt.ldt_ExperimentTableId = langlryExpTableId;
            ldt.ldt_StimulusQuantity = double.Parse(sq.ToString("f6"));
            ldt.ldt_Number = dbDrive.GetAllLangleyDataTable(langlryExpTableId).Count + 1;
            ldt.ldt_Response = resp;
            ldt.ldt_Mean = mean;
            ldt.ldt_StandardDeviation = sd;
            ldt.ldt_MeanVariance = mv;
            ldt.ldt_StandardDeviationVariance = sdv;
            ldt.ldt_Covmusigma = covmusigma;
            ldt.ldt_Note1 = note;
            return ldt;
        }

        public static string Correction(int let_Correction)
        {
            if (let_Correction == 0)
                return "标准差修正";
            else
                return "标准差不修正";
        }

        public static string DistributionState(LangleyExperimentTable let)
        {
            LangleyAlgorithm lr = SelectState(let);
            return lr.Discription();
        }

        public static XAndVArray XAndVArrays(List<LangleyDataTable> langleyDataTables)
        {
            XAndVArray xOrVArray = new XAndVArray();
            xOrVArray.xArray = new double[langleyDataTables.Count];
            xOrVArray.vArray = new int[langleyDataTables.Count];
            for (int i = 0; i < langleyDataTables.Count; i++)
            {
                xOrVArray.xArray[i] = langleyDataTables[i].ldt_StimulusQuantity;
                xOrVArray.vArray[i] = langleyDataTables[i].ldt_Response;
            }
            return xOrVArray;
        }

        //修改数据值
        public static LangleyDataTable UpdateLangleyDataTable(LangleyExperimentTable langlryExpTable, LangleyAlgorithm langleyAlgorithm, double[] xArray, int[] vArray, LangleyDataTable ldt)
        {
            ldt.ldt_Response = vArray[vArray.Length - 1];
            ldt.ldt_StimulusQuantity = xArray[xArray.Length - 1];
            vArray = IsFlipTheResponse(langlryExpTable,vArray);
            var pointCalculateValue = langleyAlgorithm.GetResult(xArray, vArray);
            ldt.ldt_Mean = double.Parse(pointCalculateValue.μ0_final.ToString("f13"));
            if (langlryExpTable.let_Correction == 0)
                pointCalculateValue.σ0_final = langleyAlgorithm.CorrectionAlgorithm(pointCalculateValue.σ0_final, xArray.Length);
            ldt.ldt_StandardDeviation = pointCalculateValue.σ0_final;
            if (double.IsNaN(pointCalculateValue.varmu))
                ldt.ldt_MeanVariance = 0;
            else
                ldt.ldt_MeanVariance = pointCalculateValue.varmu;
            if (double.IsNaN(pointCalculateValue.varsigma))
                ldt.ldt_StandardDeviationVariance = 0;
            else
                ldt.ldt_StandardDeviationVariance = pointCalculateValue.varsigma;
            if (double.IsNaN(pointCalculateValue.covmusigma))
                ldt.ldt_Covmusigma = 0;
            else
                ldt.ldt_Covmusigma = pointCalculateValue.covmusigma;
            return ldt;
        }

        public static List<Langley> Langleys(LangleyExperimentTable langlryExpTable, List<LangleyDataTable> ldts, int first, int count)
        {
            List<Langley> langleys = new List<Langley>();
            for (int i = ldts.Count - 1; i >= 0; i--)
            {
                Langley langley = new Langley();
                langley.ldt_Id = ldts[i].ldt_Id;
                langley.ldt_Number = ldts[i].ldt_Number;
                langley.ldt_StimulusQuantity = ldts[i].ldt_StimulusQuantity;
                langley.ldt_Response = ldts[i].ldt_Response;
                langley.ldt_Mean = ldts[i].ldt_Mean;
                langley.ldt_MeanVariance = ldts[i].ldt_MeanVariance;
                langley.ldt_StandardDeviation = ldts[i].ldt_StandardDeviation;
                langley.ldt_StandardDeviationVariance = ldts[i].ldt_StandardDeviationVariance;
                langley.ldt_Covmusigma = ldts[i].ldt_Covmusigma;
                langley.number = count;
                //langley.LangleyName = DistributionState(langlryExpTable) + "/" + Correction(langlryExpTable.let_Correction);
                langleys.Add(langley);
            }
            return langleys;
        }
        public static List<Langley_list> Langley_lists(IDbDrive dbDrive, List<LangleyExperimentTable> lets)
        {
            List<Langley_list> langletlists = new List<Langley_list>();
            for (int i = 0; i < lets.Count; i++)
            {
                Langley_list langley_List = new Langley_list();
                langley_List.number = i + 1;
                langley_List.Id = lets[i].let_Id;
                langley_List.PrecisionInstruments = lets[i].let_PrecisionInstruments;
                langley_List.StimulusQuantityFloor = lets[i].let_StimulusQuantityFloor;
                langley_List.StimulusQuantityCeiling = lets[i].let_StimulusQuantityCeiling;
                langley_List.Power = lets[i].let_Power;
                langley_List.DistributionState = DistributionState(lets[i]);
                langley_List.Correction = lets[i].let_Correction;
                langley_List.count = dbDrive.GetAllLangleyDataTable(lets[i].let_Id).Count - 1;
                langley_List.FlipTheResponse = lets[i].let_FlipTheResponse;
                langley_List.ExperimentalDate = lets[i].let_ExperimentalDate.ToString();
                langley_List.projectname = lets[i].let_ProductName;
                langletlists.Add(langley_List);
            }
            langletlists.Reverse();
            return langletlists;
        }
        public static List<Langley_list> Langley_lists(IDbDrive dbDrive, List<LangleyExperimentTable> lets, int first)
        {
            List<Langley_list> langletlists = new List<Langley_list>();
            for (int i = lets.Count - 1; i >= 0; i--)
            {
                Langley_list langley_List = new Langley_list();
                langley_List.number = i + 1 + first;
                langley_List.Id = lets[i].let_Id;
                langley_List.PrecisionInstruments = lets[i].let_PrecisionInstruments;
                langley_List.StimulusQuantityFloor = lets[i].let_StimulusQuantityFloor;
                langley_List.StimulusQuantityCeiling = lets[i].let_StimulusQuantityCeiling;
                langley_List.Power = lets[i].let_Power;
                langley_List.DistributionState = DistributionState(lets[i]);
                langley_List.Correction = lets[i].let_Correction;
                langley_List.count = dbDrive.GetAllLangleyDataTable(lets[i].let_Id).Count - 1;
                langley_List.FlipTheResponse = lets[i].let_FlipTheResponse;
                langley_List.ExperimentalDate = lets[i].let_ExperimentalDate.ToString();
                langley_List.projectname = lets[i].let_ProductName;
                langletlists.Add(langley_List);
            }
            return langletlists;
        }

        public static int[] IsFlipTheResponse(LangleyExperimentTable let,int[] vArray)
        {
            if (let.let_FlipTheResponse == 1)
            {
                for (int i = 0; i < vArray.Length; i++)
                {
                    if (vArray[i] == 0)
                        vArray[i] = 1;
                    else
                        vArray[i] = 0;
                }
            }
            return vArray;
        }
    }
}