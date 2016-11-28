﻿using EstimatingLibrary;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary
{
    public static class PDFConverter
    {
        public static TECDrawing convertPDFToDrawing(string pdfPath)
        {
            string name = Path.GetFileNameWithoutExtension(pdfPath);

            TECDrawing drawing = new TECDrawing(name);

            MagickImageCollection pdf = new MagickImageCollection();
            pdf.Read(pdfPath);

            string directory = Path.GetTempPath();

            int pageNum = 1;

            foreach(MagickImage page in pdf)
            {
                string pngPath = name + "_" + pageNum + ".png";

                pngPath = Path.Combine(directory, pngPath);

                page.Quality = 100;
                page.Write(pngPath);

                drawing.Pages.Add(new TECPage(pngPath, pageNum));

                Console.WriteLine("Loaded page " + pageNum + " of PDF.");

                pageNum++;
            }

            return drawing;
        }
    }
}