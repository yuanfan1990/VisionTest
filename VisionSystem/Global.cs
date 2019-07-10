using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Threading;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ImageFile;

namespace VisionSystem
{
    public static class Global
    {
        public static bool OnOffLineState = false;

        public static string BaseFolder = AppDomain.CurrentDomain.BaseDirectory;
        public static string BaseProjectFolder = AppDomain.CurrentDomain.BaseDirectory + "Projects\\";
        public static string BaseTempleteFolder = AppDomain.CurrentDomain.BaseDirectory + "Templete\\";
        public static string BaseTestImageFolder = AppDomain.CurrentDomain.BaseDirectory + "TestImage\\";
        public static string ApplicationConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "Application.xml";

        public static string ProjectConfigFilePath;
        public static string CameraConfigFilePath;
        public static string ToolBlockConfigFilePath;
        public static string CommunicationConfigFilePath;
        public static string SaveImagePath;
        public static string ProjectPath;
        public static string ProjectName;

        public static ICogFrameGrabber[] FrameGrabbers;
        public static ICogAcqFifo[] AcqFifos;
        public static string[] CameraSerialNumbers;
        public static string[] CameraModels;
        public static string[] CameraNames;
        public static CogRecordDisplay[] RecordDisplays;
        public static int CameraNumber;
        public static int ToolBlockNumber;
        public static int[,] ImageSize;



        public static void ConnectCameras()
        {
            CogFrameGrabbers myFrameGrabbers = new CogFrameGrabbers();

            FrameGrabbers = new ICogFrameGrabber[Global.CameraNumber];
            CameraModels = new string[Global.CameraNumber];
            AcqFifos = new ICogAcqFifo[Global.CameraNumber];

            if (myFrameGrabbers.Count < 1)
            {
                return;
            }

            for (int i = 0; i < Global.CameraNumber; i++)
            {
                for (int j = 0; j < myFrameGrabbers.Count; j++)
                {
                    ICogFrameGrabber myFrameGrabber = myFrameGrabbers[j];
                    ICogAcqFifo myAcqFifo = myFrameGrabber.CreateAcqFifo("Generic GigEVision (Mono)", Cognex.VisionPro.CogAcqFifoPixelFormatConstants.Format8Grey, 0, false);

                    if (CameraSerialNumbers[i] == myAcqFifo.FrameGrabber.SerialNumber)
                    {
                        FrameGrabbers[i] = myFrameGrabber;
                        AcqFifos[i] = myAcqFifo;
                        CameraModels[i] = myAcqFifo.FrameGrabber.Name;
                        break;
                    }
                    else
                    {
                        if (j == myFrameGrabbers.Count - 1)
                        {
                            
                        }
                    }
                }
            }
        }

        public static void DisConnectCameras()
        {
            if (FrameGrabbers != null)
            {
                for (int i = 0; i < FrameGrabbers.Length; i++)
                {
                    if (FrameGrabbers[i] != null)
                    {
                        FrameGrabbers[i].Disconnect(false);
                    }
                }
            }
        }
    }
}
