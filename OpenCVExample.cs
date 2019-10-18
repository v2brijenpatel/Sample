using System;
using System.Collections.Generic;

using System.Text;
using Newtonsoft.Json;
using OpenCvSharp;
using OpenCVSharpExample.Test;

namespace OpenCVSharpExample
{
    public class OpenCVExample
    {

        private string def = @"{

	Threshold:{
			min:50,
			max:255,
			type:0
		}

}";

        public void OpenCVInstance()
        {
           var x= JsonConvert.DeserializeObject<EngineDefaultConstruct>(def);
            Mat mat = Cv2.ImRead(@"D:\Data\Projects\Samples\OpenCVSharpExample\OpenCVSharpExample\0_BL_3.pdf.jpg", ImreadModes.Grayscale);
            Mat mat1 = Cv2.ImRead(@"D:\Data\Projects\Samples\OpenCVSharpExample\OpenCVSharpExample\0_BL_3.pdf.jpg", ImreadModes.Color);

            ExtractTextRect(mat, x);

            mat.Dispose();
            mat1.Dispose();


          




        }

        

        private static void ImageLog(Mat mat)
        {
            Cv2.ImShow("First", mat);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }



        public void ExtractTextRect(Mat img, EngineDefaultConstruct construct)
        {
   
            var dif = new Mat();
            var edges=img.Canny(100,200);//.AbsDiff(new Gray(0.0)).Convert<Gray, byte>().ThresholdBinary(new Gray(50), new Gray(255));
                                         // __logger.Log(canny.Bitmap, path, true);
            Cv2.Absdiff(img, edges, dif);
            Cv2.Threshold(edges, edges, construct.Threshold.min, construct.Threshold.Max, (ThresholdTypes)construct.Threshold.Type);
            Mat SE = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(12,2), new OpenCvSharp.Point(-1, -1));
            edges = edges.MorphologyEx(MorphTypes.Dilate, SE, new OpenCvSharp.Point(-1, -1), 1, BorderTypes.Reflect,Scalar.White);
            OpenCvSharp.Point[][] contours;
            OpenCvSharp.HierarchyIndex[] h;
            Mat m = new Mat();

            Cv2.FindContours(edges, out contours,out h, RetrievalModes.CComp,ContourApproximationModes.ApproxSimple);

           

            for (int i = 0; i < contours.Length; i++)
            {
                Rect brect = Cv2.BoundingRect(contours[i]);

                double ar = brect.Width / brect.Height;
                if (brect.Width > 50 && brect.Width > 25 && brect.Height > 10 && brect.Height < 100)
                {
                    Cv2.Rectangle(img, brect, Scalar.Green, 3);                    
                }
            }

            img.SaveImage("d:\\abc.jpg");
        }
    }
}
