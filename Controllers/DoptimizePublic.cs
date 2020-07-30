using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;
using WsSensitivity.Models.LangleyAndDOptimize;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace WsSensitivity.Controllers
{
    class DoptimizeExperimentModel
    { 
        public DoptimizeExperimentTable det { get; set; }
        public string doptimizeNameSeting { get; set; }
    }
    public class DoptimizePublic
    {
        public static DoptimizationAlgorithm SelectState(DoptimizeExperimentTable det)
        {
            IDoptimizationDistributionSelection distributionSelection = null;
            LangleyMethodStandardSelection langleyMethod = null;
            if (det.let_DistributionState == 0)
                distributionSelection = new Dop_Noraml();
            else if (det.let_DistributionState == 1)
                distributionSelection = new Dop_Logistic();

            if (det.let_StandardState == 0)
                langleyMethod = new Standard();
            else if (det.let_StandardState == 1)
                langleyMethod = new Ln();
            else if (det.let_StandardState == 2)
                langleyMethod = new Log();
            else if (det.let_StandardState == 3)
                langleyMethod = new Pow(double.Parse(det.let_Power));

            return new DoptimizationAlgorithm(distributionSelection, langleyMethod); 

        }
        public static DoptimizeDataTable DoptimizeDataTable(int det_Id, IDbDrive dbDrive, double sq , OutputParameters outputParameters)
        {
            DoptimizeDataTable doptimizeDataTable = new DoptimizeDataTable();
            doptimizeDataTable.ddt_ExperimentTableId = det_Id;
            doptimizeDataTable.ddt_StimulusQuantity = sq;
            doptimizeDataTable.ddt_Number = 1;
            doptimizeDataTable.ddt_Response = 0;
            doptimizeDataTable.ddt_Mean = outputParameters.μ0_final;
            doptimizeDataTable.ddt_StandardDeviation = outputParameters.σ0_final;
            doptimizeDataTable.ddt_SigmaGuess = outputParameters.sigmaguess;
            doptimizeDataTable.ddt_MeanVariance = double.IsNaN(doptimizeDataTable.ddt_MeanVariance) ? outputParameters.varsigma : 0;
            doptimizeDataTable.ddt_StandardDeviationVariance = double.IsNaN(doptimizeDataTable.ddt_StandardDeviationVariance) ? outputParameters.varmu : 0;
            doptimizeDataTable.ddt_Covmusigma = double.IsNaN(doptimizeDataTable.ddt_Covmusigma) ? outputParameters.covmusigma : 0;
            doptimizeDataTable.ddt_Note1 = "";
            return doptimizeDataTable;
        }
        public static string DistributionState(DoptimizeExperimentTable det)
        {
            LangleyAlgorithm lr = SelectState(det);
            return lr.Discription();
        }
    }
}