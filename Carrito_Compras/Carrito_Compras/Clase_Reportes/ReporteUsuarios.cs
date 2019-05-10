using iTextSharp.text;
using Carrito_Compras.Models;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Carrito_Compras.Clase_Reportes
{
    public class ReporteUsuarios
    {
        #region declaration
        int total_column = 5;
        Document document;
        Font fontStyle;
        PdfPTable pdftable = new PdfPTable(5);
        PdfPCell pdfCell;
        MemoryStream memoryStream = new MemoryStream();

        public byte[] PrepareReport()
        {

            #region
            document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            document.SetPageSize(PageSize.A4);
            document.SetMargins(20f, 20f, 20f, 20f);
            pdftable.WidthPercentage = 100;
            pdftable.HorizontalAlignment = Element.ALIGN_LEFT;
            fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            pdftable.SetWidths(new float[] { 100f, 100f, 200f, 150f, 100f });
            #endregion

            this.ReportHeader();
            this.ReportBody();
            pdftable.HeaderRows = 2;
            document.Add(pdftable);
            document.Close();
            return memoryStream.ToArray();


        }

        private void ReportBody()
        {
            #region TableHeader

            fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            pdfCell = new PdfPCell(new Phrase("Nombres", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdftable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("Apellidos", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdftable.AddCell(pdfCell);


            pdfCell = new PdfPCell(new Phrase("E-mail", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdftable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("Dirección", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdftable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("Rol", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdftable.AddCell(pdfCell);
            pdftable.CompleteRow();
            #endregion

            #region table body
            fontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            foreach (var obj in this.RecorrerUsuarios())
            {

                pdfCell = new PdfPCell(new Phrase(obj.nombres.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdftable.AddCell(pdfCell);


                pdfCell = new PdfPCell(new Phrase(obj.apellidos.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdftable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(obj.correo.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdftable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(obj.direccion.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdftable.AddCell(pdfCell);

                string rol="";
                if (obj.rol == 1)
                {
                    rol = "Administrador";
                }
                else if (obj.rol == 2)
                {
                    rol = "Empleado";
                }
                else if (obj.rol == 3)
                {
                    rol = "Empleado";
                }
            

                pdfCell = new PdfPCell(new Phrase(rol, fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdftable.AddCell(pdfCell);
            }

            #endregion

        }

        private void ReportHeader()
        {
            fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            pdfCell = new PdfPCell(new Phrase("UNIVERSIDAD DE SAN CARLOS DE GUATEMALA", fontStyle));
            pdfCell.Colspan = total_column;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.Border = 0;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfCell.ExtraParagraphSpace = 0;
            pdftable.AddCell(pdfCell);
            pdftable.CompleteRow();

            fontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            pdfCell = new PdfPCell(new Phrase("PROYECTO AYD1-TIENDA EN LINEA", fontStyle));
            pdfCell.Colspan = total_column;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.Border = 0;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfCell.ExtraParagraphSpace = 0;
            pdftable.AddCell(pdfCell);
            pdftable.CompleteRow();

            fontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            pdfCell = new PdfPCell(new Phrase("Reporte de Usuarios", fontStyle));
            pdfCell.Colspan = total_column;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.Border = 0;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfCell.ExtraParagraphSpace = 0;
            pdftable.AddCell(pdfCell);
            pdftable.CompleteRow();

            
        }

        public LinkedList<Usuario> RecorrerUsuarios()
        {
            LinkedList<Usuario> usuarios = new LinkedList<Usuario>();
            Usuario l = new Usuario();
            foreach (var obj in l.ObtenerUsuario())
            {
                //Agregamos a la lista
                usuarios.AddLast(obj);

            }
            
            return usuarios;
        }





        #endregion

    }
}
