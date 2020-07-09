using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    class SysParam
    {

        //全局串口名称
        public static string Sys_PortName = "COM3";
        //全局串口比特率
        public static int Sys_BaudRate = 9600;
        //全局串口发送命令
        public static string Sys_Send = " ";
        //全局串口接收命令
        public static string Sys_Receive = " ";
        //全局串口打开关闭状态
        public static bool Sys_IsOpen = false;
        //全局发火装置连接正确
        public static bool Sys_发火装置连接正确 = false;

        //全局直流电源连接正确
        public static bool Sys_直流电源连接正确 = false;

        //全局直流电源名称
        public static string Sys_直流电源名称 = "";
        //全局电压输出
        public static bool Sys_电压输出 = false;
        //全局电流输出
        public static bool Sys_电流输出 = false;
        //全局发火模式
        public static int Sys_mode = 0;


        public static bool tech_show = false;
        public static double final_mu;
        public static double final_sigma;
        //  public static string userName = "";//当前用户名
        public static string curstate = "新实验";//新实验或者是查询

        /// <summary>
        /// 兰利法参数
        /// </summary>
        public static string LangLie_Distribute;
        public static double LangLie_Prec99;
        public static double LangLie_Prec01;
        public static string LangLie_Reverse;
        public static double LangLie_Precision;
        public static double LangLie_UpperLimit = 0;
        public static double LangLie_LowLimit = 0;
        public static int LangLie_TestTimes = 1;
        public static string CurUserName;
        public static int LangLie_MastId;
        public static int LangLie_DetailId;


        /// <summary>
        /// 升级法参数
        /// </summary>
        public static string store_upDown_product = "";//产品名称
        public static string store_upDown_x0 = "";//最大刺激量
        public static string store_upDown_step = "";//步长
        public static string store_upDown_Reverse = "";//最大刺激量
        public static string store_upDown_group = "";//是否分组
        public static string store_upDown_Distribute = "";//分布方法
        public static string store_upDown_tradition = "";// 是否是传统方法
        public static int upDown_TestTimes = 1;//测试剌数
        public static string updown_data = " ";//时间

        public static int upDown_MastId;
        public static int upDown_DetailId = 1;
        //2008.11.12
        //升降法中下一步参数选择不能保持设置，重新进入还是原来的参数
        public static string store_upDown_next_avg = "";//总体平均直
        public static string store_upDown_next_step = "";//步长
        public static string store_upDown_next_Count = "";//测量次数

        //升降法中用到的全局变量
        public static int Global_Frac_up_dwon = 8;

        // public static int MastId = -1;//id编号
        public static string ProductName = "新产品";
        public static int xCount_positive = 0; //正刺激量的个数
        public static int xCount_negative = 0; //负刺激量的个数
                                               // public static double x0 = 10; //初始值
                                               //   public static double y0 = 1.02; //初始值
                                               //  public static double step = 0.01; //步长
        public static double[] X = new double[60];//试探次数不超过60次
        public static int[] V = new int[60];//试探次数不超过60次
        public static int[] vi;//响应数
        public static int[] mi;//不响应数
        //public static int[] vi1;//中间结果
        public static int vi1_signal = 0;//表示vi1取的是vi还是mi
        public static int[] result_i;//
                                     //public static double[] result_x;//
        public static int Current_x_i_pos = 0;//当前正方向
        public static int Current_x_i_neg = 0;//当前负方向
        public static int max_i_pos = 0;//当前正方向
        public static int max_i_neg = 0;//当前负方向
        public static int Current_v_i = 0;//当前v的序号,用于数组下标
        public static int GroupId = 1;//当前组次
        public static Boolean contiue_add = false;//当前进行到第几次试验
        public static int N = 30;//要求的试探次数
        public static int test_n = 0;//试探次数()用公式计算)
        public static Boolean EstimateSigma = false;//是否需要估计参数
        public static Boolean notCal = false;//是否不计数
        public static double updown_Precision = 0;//精度
        public static double updown_Power = 0;//幂


        //用于计算最终的结果
        public static int[] nj = new int[30];
        public static double[] Gj = new double[30];
        public static double[] Hj = new double[30];
        public static double[] muj = new double[30];
        public static double[] sigmaj = new double[30];

        public static double mu_final = 0;//多组试验最终的平均值
        public static double sigma_final = 0;//多组试验最终的标准误差
        public static int n_final = 0;//多组试验最终的试探次数之和
        public static double sigmaMu_final = 0;//多组试验最终的平均值的误差
        public static double sigmaSigma_final = 0;//多组试验最终的标准误差的误差

        public static int testCount = 0;//试探次数
        public static Boolean tradition = true;//传统方法
        public static Boolean correct = false;//传统方法
        public static Boolean hasCal = false;//是否已经计算
        public static Boolean calsigma = false;//初始的标准误差

        public static double sigma = 0;//计算出来的标准误差
        public static double mu = 0;//计算出来的平均值
        public static double sigmaMu = 0;//计算出来的平均值的误差
        public static double sigmaSigma = 0;//计算出来的误差的误差
        public static double G;
        public static double H;
        public static double A;
        public static double B;
        public static double M;
        public static double b;
        public static double p;

        public static Boolean multiGroup = false;//多组试验
        public static Boolean loadParam = false;
        public static string Distribution = "正态分布";
        public static string Reverse = "否";//是否反转
        public static double P001 = 0;//p=0.001的估计值
        public static double x999 = 0;//X0.001的95%置信区间
        public static double prec999 = 0;
        public static double prec001 = 0;
        public static double sigmaprec999 = 0;//99.9%标准误差
        public static double sigmaprec001 = 0;//0.11%标准误差






        public static int maxGroupId = 1;//最大分组号码
        public static int sumcount = 0;//总样本量


        //Doptimize
        //记录最后一次的状态



        public static string Doptimize_Production = "";
        public static string Doptimize_Distribute = "";
        public static string Doptimize_Memo = "";
        public static double Doptimize_Precision = 0;
        public static double Doptimize_UpperLimit = 0;
        public static double Doptimize_LowLimit = 0;
        public static double Doptimize_normc = 0;
        public static double Doptimize_avgX = 0;
        public static double Doptimize_Prece01 = 0;
        public static double Doptimize_Prece99 = 0;
        public static int Doptimize_Frac = 0;
        public static int Doptimize_TestTimes = 0;
        public static int Doptimize_NM = 0;
        public static int Doptimize_MastId = 0;
        public static int Doptimize_DetailId = 0;
        public static double Doptimize_Power = 0;
        public static string Doptimize_reverse = "否";
    }
}