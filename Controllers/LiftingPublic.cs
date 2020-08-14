using AlgorithmReconstruct;
using System.Collections.Generic;
using System.Data.Entity.Core;
using WsSensitivity.Models;

namespace WsSensitivity.Controllers
{
    public class UpDownMethodModel
    {
        public int ExperimentalId { get; set; }
        public int id { get; set; }//组id
        public string Groupingstate { get; set; }//分组状态
        public string ProductName { get; set; }//产品名称
        public string ExperimentalLabelName { get; set; }
        public bool IsLastGroup { get; set; }
        public int GroupNumber { get; set; }
    }

    public class GetAllUpDownMethodsModel
    {
        public int number { get; set; }
        public double stimulusQuantity { get; set; }
        public int response { get; set; }
        public double standardStimulus { get; set; }
    }
    public class SingleExperimentTable
    {
        public int i { get; set; }
        public int i2 { get; set; }
        public int vi { get; set; }
        public int mi { get; set; }
        public int ivi { get; set; }
        public int i2vi { get; set; }
        public int imi1 { get; set; }
        public int i2mi1 { get; set; }
    }

    public class SingleSetResultsModel
    {
        public Upanddown upanddown { get; set; }
        public double prec001 { get; set; }
        public double prec999 { get; set; }
        public double rpse001 { get; set; }
        public double rpse999 { get; set; }
    }

    public class ComprehensiveResultsModel
    { 
        public int setNumber { get; set; }
        public double stimulusQuantity { get; set; }
        public double stepLength { get; set; }
        public int sampleNumber { get; set; }
        public double average { get; set; }
        public double standardDeviation { get; set; }
        public double msde { get; set; }
        public double sdsde { get; set; }
        public int temptationNumber { get; set; }
        public double G { get; set; }
        public double H { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double M { get; set; }
        public double b { get; set; }
        public double p { get; set; }
        public double percentage01 { get; set; }
        public double percentage999 { get; set; }
    }

    public class ManyexperimentsModel
    { 
        public double previousSetNumber { get; set; }//上一组步长
        public int currentSetNumber { get; set; }//当前组次
        public string ProductName { get; set; }
        public string stimulusQuantity { get; set; }
        public string stepLength { get; set; }
        public string distribution { get; set; }
    }

    public class LiftingPublic
    {
        public static LiftingAlgorithm SelectState(UpDownExperiment updateException)
        {
            LiftingDistributionSelection lds = null;
            LiftingMethodStandardSelection lms = null;
            if (updateException.udt_Distribution == 0)
                lds = new TraditionalMethod();
            else if (updateException.udt_Distribution == 1)
                lds = new CombinationMethod();
            else if (updateException.udt_Distribution == 2)
                lds = new AmendmentMethod();
            else if (updateException.udt_Distribution == 3)
                lds = new LiftingLogistic();

            if (updateException.udt_Standardstate == 0)
                lms = new LiftingStandard();
            else if (updateException.udt_Standardstate == 1)
                lms = new LiftingLn();
            else if (updateException.udt_Standardstate == 2)
                lms = new LiftingLog();
            else if (updateException.udt_Standardstate == 3)
                lms = new LiftingPow(updateException.udt_Power);

            return new LiftingAlgorithm(lms, lds);
        }

        //翻转
        public static int Filp(int response, int filpValue) => filpValue == 0 ? response : response == 1 ? 0 : 1;

        //当前组次
        public static int CurrentSetNumber(List<UpDownGroup> upDownGroups,int udg_id)
        {
            int count = 1;
            foreach (var i in upDownGroups)
            {
                count = i.Id == udg_id ? count : count++;
            }
            return count;
        }
    }
}