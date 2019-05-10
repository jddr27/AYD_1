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
    public class ReporteComentarios
    {
        #region declaration
        int total_column = 5;
        Document document;
        Font fontStyle;
        PdfPTable pdftable = new PdfPTable(5);
        PdfPCell pdfCell;
        MemoryStream memoryStream = new MemoryStream();

        public byte[] PrepareReport() {

            #region
            document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            document.SetPageSize(PageSize.A4);
            document.SetMargins(20f, 20f, 20f, 20f);
            pdftable.WidthPercentage = 100;
            pdftable.HorizontalAlignment = Element.ALIGN_LEFT;
            fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            pdftable.SetWidths(new float[] {100f,100f,200f,150f,100f});
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

            fontStyle = FontFactory.GetFont("Tahoma",10f, 1);
            pdfCell = new PdfPCell(new Phrase("Usuario", fontStyle));            
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;            
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;            
            pdftable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("Producto", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdftable.AddCell(pdfCell);


            pdfCell = new PdfPCell(new Phrase("Comentario", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdftable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("Fecha", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdftable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("Valoracion", fontStyle));
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdftable.AddCell(pdfCell);
            pdftable.CompleteRow();
            #endregion

            #region table body
            fontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            foreach (var obj in this.RecorreComentarios()) {

                pdfCell = new PdfPCell(new Phrase(obj.usuario.nombres.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdftable.AddCell(pdfCell);


                pdfCell = new PdfPCell(new Phrase(obj.producto.nombre.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdftable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(obj.texto_comentario.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdftable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(obj.fecha_comentario.ToString(), fontStyle));
                pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell.BackgroundColor = BaseColor.WHITE;
                pdftable.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(obj.valoracion_comentario.ToString(), fontStyle));
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
            pdfCell = new PdfPCell(new Phrase("UNIVERSIDAD DE SAN CARLOS", fontStyle));
            pdfCell.Colspan = total_column;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.Border = 0;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfCell.ExtraParagraphSpace = 0;
            pdftable.AddCell(pdfCell);
            pdftable.CompleteRow();

            fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            pdfCell = new PdfPCell(new Phrase("PROYECTO AYD1-TIENDA EN LINEA", fontStyle));
            pdfCell.Colspan = total_column;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.Border = 0;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfCell.ExtraParagraphSpace = 0;
            pdftable.AddCell(pdfCell);
            pdftable.CompleteRow();


            fontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            pdfCell = new PdfPCell(new Phrase("Lista de Comentarios", fontStyle));
            pdfCell.Colspan = total_column;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.Border = 0;
            pdfCell.BackgroundColor = BaseColor.WHITE;
            pdfCell.ExtraParagraphSpace = 0;
            pdftable.AddCell(pdfCell);
            pdftable.CompleteRow();
        }

        public LinkedList<Comentario> RecorreComentarios() {
            LinkedList<Comentario> Comentarios = new LinkedList<Comentario>();
            foreach (var obj in Comentario.ObtenerReseña())
            {
                Comentarios.AddLast(obj);
            }
            foreach (Comentario obj in Comentarios)
            {
                var usu = new Usuario(obj.usuario_comentario);
                obj.usuario = usu;
            }

            foreach (Comentario obj in Comentarios)
            {
                var producto = new Producto(obj.producto_comentario);
                obj.producto = producto;
            }
            return Comentarios;
        }


        

            
        #endregion

    }
}