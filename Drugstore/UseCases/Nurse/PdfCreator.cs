using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Drugstore.Models.Shared;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Drugstore.UseCases.Nurse
{
    public class PdfCreator
    {
        private const int FONT_SIZE = 12;
        private const float STEP = FONT_SIZE + 0.2f * FONT_SIZE;
        private readonly XFont FONT = new XFont("Arial", FONT_SIZE, XFontStyle.Regular);
        private float column;
        private PdfDocument document;
        private XGraphics graphics;
        private float line;
        private PdfPage page;

        private float pageHeight;

        public Stream PreparePdf(IList<PrescriptionViewModel> prescriptions)
        {
            document = new PdfDocument();
            var patient = prescriptions.First().PatientName;
            document.Info.Title = "Historia leczenia " + patient;
            document.Info.Author = "Szpital Szybka Pigula";
            page = document.AddPage();
            graphics = XGraphics.FromPdfPage(page);
            column = (float) page.Width * 0.08f;
            line = (float) page.Height * 0.08f;
            pageHeight = (float) page.Height;

            //Wstep
            graphics.DrawString("Recepty " + patient + " na dzien: " + DateTime.Now.ToString("yyyy-MM-dd"),
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            IncreaseLine(1);
            WriteLine();

            //Rozwiniecie
            foreach (var prescription in prescriptions)
            {
                WritePrescriptionHeader(prescription);
                WriteDescrption();
                foreach (var medicine in prescription.AssignedMedicines)
                {
                    WriteMedicines(medicine);
                }

                WriteLine();
            }

            var saveStream = new MemoryStream();
            document.Save(saveStream);
            return saveStream;
        }

        private void WriteLine()
        {
            graphics.DrawString(
                "_________________________________________________________________________",
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            IncreaseLine(2);
        }

        private void WriteDescrption()
        {
            var column = this.column;
            graphics.DrawString(
                "Nazwa",
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            column += 2 * this.column;
            graphics.DrawString(
                "Cena za sztuke",
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            column += 2 * this.column;
            graphics.DrawString("Refundacja [%]",
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            column += 2 * this.column;
            graphics.DrawString("Kategoria",
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            column += 2 * this.column;
            graphics.DrawString("Ilosc",
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            IncreaseLine(1);
        }

        private void WriteMedicines(MedicineViewModel medicine)
        {
            var column = this.column;
            graphics.DrawString(
                medicine.Name,
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            column += 2 * this.column;
            graphics.DrawString(
                medicine.PricePerOne.ToString(),
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            column += 2 * this.column;
            graphics.DrawString(medicine.Refundation.ToString(),
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            column += 2 * this.column;
            graphics.DrawString(
                medicine.MedicineCategory.ToString(),
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            column += 2 * this.column;
            graphics.DrawString(medicine.Quantity.ToString(),
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            IncreaseLine(1);
        }

        private void WritePrescriptionHeader(PrescriptionViewModel prescription)
        {
            graphics.DrawString("Lekarz wystawiajacy: " + prescription.DoctorName,
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            IncreaseLine(1);

            graphics.DrawString("Data wystawienia: " + prescription.CreationTime.ToString("yyyy-MM-dd"),
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            IncreaseLine(1);

            graphics.DrawString("Calkowity koszt:" + prescription.TotalCost + " zl",
                FONT,
                XBrushes.Black,
                new XPoint(column, line),
                XStringFormats.BaseLineLeft);
            IncreaseLine(1);
        }

        private void IncreaseLine(int multipiler)
        {
            line += multipiler * STEP;
            if (line >= pageHeight*0.85f)
            {
                page = document.AddPage();
                graphics = XGraphics.FromPdfPage(page);
                column = (float) page.Width * 0.08f;
                line = (float) page.Height * 0.08f;
            }
        }
    }
}