using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Razor.Parser.SyntaxTree;
using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;
using WsSensitivity.Models.LangleyAndDOptimize;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace WsSensitivity.Controllers
{
    public class DoptimizeExperimentModel
    { 
        public DoptimizeExperimentTable det { get; set; }
        public string doptimizeNameSeting { get; set; }
        public double sq { get; set; }
    }
    public class DoptimizeExperimentRecoed
    { 
        public double stimulusQuantity { get; set; }
        public int response { get; set; }
        public int Filp(int response,int filpValue) => filpValue == 0 ? response : response == 1 ? 0 : 1;
    }
    public struct XarrayAndVarray
    {
        public double[] xArray;
        public int[] vArray;
    }
    public struct Doptimization
    {
        public int ddt_Id;
        public int ddt_Number;
        public double ddt_StimulusQuantity;
        public int ddt_Response;
        public double ddt_Mean;
        public double ddt_StandardDeviation;
        public double ddt_MeanVariance;
        public double ddt_StandardDeviationVariance;
        public double ddt_Covmusigma;
        public double ddt_SigmaGuess;
        public double number;
    }
    public class DoptimizePublic
    {
        public static DoptimizationAlgorithm SelectState(DoptimizeExperimentTable det)
        {
            IDoptimizationDistributionSelection distributionSelection = null;
            LangleyMethodStandardSelection langleyMethod = null;
            if (det.det_DistributionState == 0)
                distributionSelection = new Dop_Noraml();
            else if (det.det_DistributionState == 1)
                distributionSelection = new Dop_Logistic();

            if (det.det_StandardState == 0)
                langleyMethod = new Standard();
            else if (det.det_StandardState == 1)
                langleyMethod = new Ln();
            else if (det.det_StandardState == 2)
                langleyMethod = new Log();
            else if (det.det_StandardState == 3)
                langleyMethod = new Pow(double.Parse(det.det_Power));

            return new DoptimizationAlgorithm(distributionSelection, langleyMethod); 

        }
        public static DoptimizeDataTable DoptimizeDataTable(int det_Id, IDbDrive dbDrive, double sq , OutputParameters outputParameters)
        {
            DoptimizeDataTable doptimizeDataTable = new DoptimizeDataTable();
            doptimizeDataTable.ddt_ExperimentTableId = det_Id;
            doptimizeDataTable.ddt_StimulusQuantity = double.Parse(sq.ToString("f6"));
            doptimizeDataTable.ddt_Number = dbDrive.GetDoptimizeDataTables(det_Id).Count + 1;
            doptimizeDataTable.ddt_Response = 0;
            doptimizeDataTable.ddt_Mean = 0;
            doptimizeDataTable.ddt_StandardDeviation = 0;
            doptimizeDataTable.ddt_SigmaGuess = outputParameters.sigmaguess;
            doptimizeDataTable.ddt_StandardDeviationVariance = 0;
            doptimizeDataTable.ddt_MeanVariance = 0;
            doptimizeDataTable.ddt_Covmusigma = 0;
            doptimizeDataTable.ddt_Note1 = "";
            return doptimizeDataTable;
        }
        public static string DistributionState(DoptimizeExperimentTable det)
        {
            LangleyAlgorithm lr = SelectState(det);
            return lr.Discription();
        }
        public static List<DoptimizeExperimentRecoed> DoptimizeExperimentRecoedsList(List<DoptimizeDataTable> ddt_list, DoptimizeExperimentTable det)
        {
            List<DoptimizeExperimentRecoed> der_list = new List<DoptimizeExperimentRecoed>();
            for (int i = 0; i < ddt_list.Count; i++)
            {
                DoptimizeExperimentRecoed der = new DoptimizeExperimentRecoed();
                der.stimulusQuantity = ddt_list[i].ddt_StimulusQuantity;
                der.response = der.Filp(ddt_list[i].ddt_Response, det.det_FlipTheResponse);
                der_list.Add(der);
            }
            return der_list;
        }

        public static XarrayAndVarray ReturnXarrayAndVarray(List<DoptimizeExperimentRecoed> der_list)
        {
            XarrayAndVarray rxav = new XarrayAndVarray();
            rxav.xArray = new double[der_list.Count];
            rxav.vArray = new int[der_list.Count];
            for (int i = 0; i < der_list.Count; i++)
            {
                rxav.xArray[i] = der_list[i].stimulusQuantity;
                rxav.vArray[i] = der_list[i].response;
            }
            return rxav;
        }
        public static void UpdateDoptimizeDataTable(ref DoptimizeDataTable ddt,OutputParameters outputParameters,string response,string sq)
        {
            ddt.ddt_StimulusQuantity = double.Parse(sq);
            ddt.ddt_Response = int.Parse(response);
            ddt.ddt_Mean = double.Parse(outputParameters.μ0_final.ToString("f13"));
            ddt.ddt_StandardDeviation = double.Parse(outputParameters.σ0_final.ToString("f13"));
            ddt.ddt_MeanVariance = double.IsNaN(outputParameters.varmu) ? 0 : outputParameters.varmu;
            ddt.ddt_StandardDeviationVariance = double.IsNaN(outputParameters.varsigma) ? 0 : outputParameters.varsigma;
            ddt.ddt_Covmusigma = double.IsNaN(outputParameters.covmusigma) ? 0 : outputParameters.covmusigma;
            ddt.ddt_SigmaGuess = double.Parse(outputParameters.sigmaguess.ToString("f6"));
        }
        public static List<Doptimization> GetDoptimizes(List<DoptimizeDataTable> ddt_list,int count)
        {
            List<Doptimization> doptimizes = new List<Doptimization>();
            for (int i = ddt_list.Count - 1; i >= 0; i--)
            {
                Doptimization doptimize = new Doptimization();
                doptimize.ddt_Id = ddt_list[i].ddt_Id;
                doptimize.ddt_Number = ddt_list[i].ddt_Number;
                doptimize.ddt_StimulusQuantity = ddt_list[i].ddt_StimulusQuantity;
                doptimize.ddt_Response = ddt_list[i].ddt_Response;
                doptimize.ddt_Mean = ddt_list[i].ddt_Mean;
                doptimize.ddt_MeanVariance = ddt_list[i].ddt_MeanVariance;
                doptimize.ddt_StandardDeviation = ddt_list[i].ddt_StandardDeviation;
                doptimize.ddt_StandardDeviationVariance = ddt_list[i].ddt_StandardDeviationVariance;
                doptimize.ddt_Covmusigma = ddt_list[i].ddt_Covmusigma;
                doptimize.ddt_SigmaGuess = ddt_list[i].ddt_SigmaGuess;
                doptimize.number = count;
                doptimizes.Add(doptimize);
            }
            return doptimizes;
        }
        public static string[] GetIntervalEstimateValue(List<IntervalEstimation> ies)
        {
            string[] value = { "(" + ies[0].Confidence.Down.ToString("f6") + "," + ies[0].Confidence.Up.ToString("f6") + ")", "(" + ies[0].Mu.Down.ToString("f6") + "," + ies[0].Mu.Up.ToString("f6") + ")", "(" + ies[0].Sigma.Down.ToString("f6") + "," + ies[0].Sigma.Up.ToString("f6") + ")", "(" + ies[1].Confidence.Down.ToString("f6") + "," + ies[1].Confidence.Up.ToString("f6") + ")", "(" + ies[1].Mu.Down.ToString("f6") + "," + ies[1].Mu.Up.ToString("f6") + ")", "(" + ies[1].Sigma.Down.ToString("f6") + "," + ies[1].Sigma.Up.ToString("f6") + ")" };
            return value;
        }
    }
}