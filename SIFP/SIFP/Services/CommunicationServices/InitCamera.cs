using Serilog;
using Services.Interfaces;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Services
{
    public class InitCamera : IInitCamera
    {
        public ConfigCameraModel InitConfigCamera(SubWorkModeE subWorkMode)
        {
#if false //CreateJsonFile
            ConfigCameraModel configCamera = new ConfigCameraModel
            {
                DoReset = 0,
                StandByMode = StandByModeE.None,
                SysXtalClkKHz = 19800,
                UserCases = new List<UserCaseModel>
                {
                   new UserCaseModel
                   {
                       WorkMode=WorkModeE.SINGLE_FREQ,
                       SubWorkMode=SubWorkModeE._4PHASE,
                       SubFrameModes=new SubFrameModeE[]{0,0,0,0},
                       NumSubFramePerFrame=new uint[]{ 1,0,0,0},
                       SpecialFrameModes=new SpecialFrameModeE[]{0,0,0,0 },
                       DifferentialBG=0,
                       NumDepthSequencePerDepthMap=1,
                       MIPI_FS_FE_Pos=MIPI_FS_FE_PosE.SubFrame,
                       FrameSeqSchedule=new FrameSeqSchedule
                       {
                           Slot0FrameNum=0,
                           Slot1FrameNum=1,
                           Slot2FrameNum=2,
                           Slot3FrameNum=3,
                       },
                       IntegrationTimes=new IntegrationTime[]
                       {
                         new IntegrationTime{ Phase1_4Int=1000000,Phase5_8Int=0,SpecialPhaseInt=0,},
                         new IntegrationTime{ Phase1_4Int=0,Phase5_8Int=0,SpecialPhaseInt=0,},
                         new IntegrationTime{ Phase1_4Int=0,Phase5_8Int=0,SpecialPhaseInt=0,},
                         new IntegrationTime{ Phase1_4Int=0,Phase5_8Int=0,SpecialPhaseInt=0,},
                       },
                       PLLDLLDivs=new PLLDLLDiv[]
                       {
                         new PLLDLLDiv{ Phase1_4Div=5,Phase5_8Div=2,SpecialPhaseDiv=2,},
                         new PLLDLLDiv{ Phase1_4Div=2,Phase5_8Div=2,SpecialPhaseDiv=2,},
                         new PLLDLLDiv{ Phase1_4Div=2,Phase5_8Div=2,SpecialPhaseDiv=2,},
                         new PLLDLLDiv{ Phase1_4Div=2,Phase5_8Div=2,SpecialPhaseDiv=2,},
                       },
                   },
                   new UserCaseModel
                   {
                       WorkMode=WorkModeE.SINGLE_FREQ,
                       SubWorkMode=SubWorkModeE._4PHASE_GRAY,
                       SubFrameModes=new SubFrameModeE[]{SubFrameModeE.Mode_5Phase,0,0,0},
                       NumSubFramePerFrame=new uint[]{ 1,0,0,0},
                       SpecialFrameModes=new SpecialFrameModeE[]{SpecialFrameModeE.Gray,0,0,0 },
                       DifferentialBG=0,
                       NumDepthSequencePerDepthMap=1,
                       MIPI_FS_FE_Pos=MIPI_FS_FE_PosE.SubFrame,
                       FrameSeqSchedule=new FrameSeqSchedule
                       {
                           Slot0FrameNum=0,
                           Slot1FrameNum=1,
                           Slot2FrameNum=2,
                           Slot3FrameNum=3,
                       },
                       IntegrationTimes=new IntegrationTime[]
                       {
                         new IntegrationTime{ Phase1_4Int=1000000,Phase5_8Int=0,SpecialPhaseInt=1000000,},
                         new IntegrationTime{ Phase1_4Int=0,Phase5_8Int=0,SpecialPhaseInt=0,},
                         new IntegrationTime{ Phase1_4Int=0,Phase5_8Int=0,SpecialPhaseInt=0,},
                         new IntegrationTime{ Phase1_4Int=0,Phase5_8Int=0,SpecialPhaseInt=0,},
                       },
                       PLLDLLDivs=new PLLDLLDiv[]
                       {
                         new PLLDLLDiv{ Phase1_4Div=5,Phase5_8Div=2,SpecialPhaseDiv=2,},
                         new PLLDLLDiv{ Phase1_4Div=2,Phase5_8Div=2,SpecialPhaseDiv=2,},
                         new PLLDLLDiv{ Phase1_4Div=2,Phase5_8Div=2,SpecialPhaseDiv=2,},
                         new PLLDLLDiv{ Phase1_4Div=2,Phase5_8Div=2,SpecialPhaseDiv=2,},
                       },
                   },
                   new UserCaseModel
                   {
                       WorkMode=WorkModeE.DOUBLE_FREQ,
                       SubWorkMode=SubWorkModeE._4PHASE_GRAY_4PHASE_BG,
                       SubFrameModes=new SubFrameModeE[]{SubFrameModeE.Mode_5Phase, SubFrameModeE.Mode_5Phase,0,0},
                       NumSubFramePerFrame=new uint[]{ 1,1,0,0},
                       SpecialFrameModes=new SpecialFrameModeE[]{SpecialFrameModeE.Gray,SpecialFrameModeE.Bg,0,0 },
                       DifferentialBG=0,
                       NumDepthSequencePerDepthMap=1,
                       MIPI_FS_FE_Pos=MIPI_FS_FE_PosE.SubFrame,
                       FrameSeqSchedule=new FrameSeqSchedule
                       {
                           Slot0FrameNum=0,
                           Slot1FrameNum=1,
                           Slot2FrameNum=2,
                           Slot3FrameNum=3,
                       },
                       IntegrationTimes=new IntegrationTime[]
                       {
                         new IntegrationTime{ Phase1_4Int=1000000,Phase5_8Int=0,SpecialPhaseInt=1000000,},
                         new IntegrationTime{ Phase1_4Int=1000000,Phase5_8Int=0,SpecialPhaseInt=1000000,},
                         new IntegrationTime{ Phase1_4Int=0,Phase5_8Int=0,SpecialPhaseInt=0,},
                         new IntegrationTime{ Phase1_4Int=0,Phase5_8Int=0,SpecialPhaseInt=0,},
                       },
                       PLLDLLDivs=new PLLDLLDiv[]
                       {
                         new PLLDLLDiv{ Phase1_4Div=5,Phase5_8Div=2,SpecialPhaseDiv=2,},
                         new PLLDLLDiv{ Phase1_4Div=8,Phase5_8Div=2,SpecialPhaseDiv=2,},
                         new PLLDLLDiv{ Phase1_4Div=2,Phase5_8Div=2,SpecialPhaseDiv=2,},
                         new PLLDLLDiv{ Phase1_4Div=2,Phase5_8Div=2,SpecialPhaseDiv=2,},
                       },
                   },
                },
                MIPIFrameRate = 5,
                SequencerRepeatMode = SequencerRepeatModeE.Auto_Repeat,
                TriggerMode = TriggerModeE.Master_Mode,
                ROISetting = new ROISetting { XStart = 0, XSize = 640, XStep = 1, YStart = 0, YSize = 480, YStep = 1 },
                BinningMode = BinningModeE.None,
                MirrorMode = MirrorModeE.None,
                TSensorMode = TSensorModeE.EveryPhase,
                PerformClkChanges = 0,
                ClkDivOverride = new ClkDIvOverride { ClkDigSlowDiv = 0, PLLFBDiv = 0, PLLPreDiv = 0 },
            };
            var json = JsonSerializer.Serialize(configCamera);
            File.WriteAllText(@"Configs\ConfigCamera.json", json);

#endif
            try
            {
                var json = File.ReadAllText(@"Configs\ConfigCamera.json");

                var configs = JsonSerializer.Deserialize<ConfigCameraModel>(json);

                //计算每一个UserCase的最大深度帧帧率
                foreach (var usercase in configs.UserCases)
                {
                    Int32[] numPhasePerFrame = new Int32[4];
                    for (int i = 0; i < usercase.NumSubFramePerFrame.Length; i++)
                    {
                        switch (usercase.SubFrameModes[i])
                        {
                            case SubFrameModeE.Mode_4Phase:
                                numPhasePerFrame[i] = (int)(4 * usercase.NumSubFramePerFrame[i]);
                                break;
                            case SubFrameModeE.Mode_2Phase:
                                numPhasePerFrame[i] = (int)(2 * usercase.NumSubFramePerFrame[i]);
                                break;
                            case SubFrameModeE.Mode_8Phase:
                                numPhasePerFrame[i] = (int)(8 * usercase.NumSubFramePerFrame[i]);
                                break;
                            case SubFrameModeE.Mode_2Phase_2Phase:
                                numPhasePerFrame[i] = (int)(4 * usercase.NumSubFramePerFrame[i]);
                                break;
                            case SubFrameModeE.Mode_1SpecialPhase:
                                numPhasePerFrame[i] = (int)(1 * usercase.NumSubFramePerFrame[i]);
                                break;
                            case SubFrameModeE.Mode_5Phase:
                                numPhasePerFrame[i] = (int)(5 * usercase.NumSubFramePerFrame[i]);
                                break;
                            case SubFrameModeE.Mode_9Phase:
                                numPhasePerFrame[i] = (int)(9 * usercase.NumSubFramePerFrame[i]);
                                break;
                            case SubFrameModeE.Mode_3Phase:
                                numPhasePerFrame[i] = (int)(3 * usercase.NumSubFramePerFrame[i]);
                                break;
                            default:
                                break;
                        }
                    }
                    usercase.NumPhasePerFrameStruct = (uint)numPhasePerFrame.Sum() * usercase.NumDepthSequencePerDepthMap;

                    usercase.MaxFPS = Math.Min(30, 240 / usercase.NumPhasePerFrameStruct);
                }
                
                if (configs.UserCases.Exists(config => config.SubWorkMode == subWorkMode))
                    configs.CurrentUserCase = configs.UserCases.Find(config => config.SubWorkMode == subWorkMode);
                else
                    configs.CurrentUserCase = configs.UserCases.First();

                return configs;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            return null;
        }

        public List<ComboBoxViewMode<uint>> InitFrequencies()
        {
            /*DLL(output of DLL is fMod) frequency div
        clock path:
        |PLL| -> |PLL_DLL div| -> |div by 2 fixed| -> DLL(fMod)

        e.g. for PHASE1_4's modulation frequency:
        The final division ratio from PLL to output of DLL(fMod) is PHASE1_4_PLL_DLL_DIV * 2
        So, fMod = fPLL / (PHASE1_4_PLL_DLL_DIV * 2)

        IMPORTANT: allowed values for the 3 values in the struct are:
        2,3,4,5 and from 6 to 30(only even numbers)
        2的时候频率太高不需要
        */
            var frequencies = new List<ComboBoxViewMode<uint>>();
            string fre = "990";// AppConfigHelper.GetAppConfigValue("PllFrequency");

            double pllFreq = double.Parse(fre);//Pll频率 

            for (uint i = 3; i < 6; i++)//分频 2的时候频率太高不需要
            {
                frequencies.Add(new ComboBoxViewMode<UInt32> { Description = (pllFreq / i / 2.0).ToString("0.00"), SelectedModel = i, IsShow = Visibility.Visible });
            }
            for (uint i = 6; i <= 30; i++)//分频
            {
                if (i % 2 == 0)
                    frequencies.Add(new ComboBoxViewMode<UInt32> { Description = (pllFreq / i / 2.0).ToString("0.00"), SelectedModel = i, IsShow = Visibility.Visible });
            }

            return frequencies;
        }

        public Tuple<double, double> InitIntegrationTimesRange()
        {
            try
            {
                var json = File.ReadAllText(@"Configs\Camera.json");
                return JsonSerializer.Deserialize<Tuple<double, double>>(json);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            return null;
        }
    }
}
