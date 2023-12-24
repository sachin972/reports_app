using ceTe.DynamicPDF.Viewer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Some_Application
{
    /// <summary>
    /// Interaction logic for BuyersWindow.xaml
    /// </summary>
    public partial class BuyersWindow : Window
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private ceTe.DynamicPDF.Viewer.PdfViewer pdfViewer;

        public BuyersWindow(string pathToPdf)
        {
            InitializeComponent();
            this.pdfViewer = new PdfViewer();

            Console.WriteLine($@"{documentsPath}\{pathToPdf}.pdf");

            FileStream streamObj = new FileStream($@"{documentsPath}\reports\{pathToPdf}.pdf", FileMode.Open);
            PdfDocument pdf = new PdfDocument(streamObj);
            this.windowsFormsHost1.Child = this.pdfViewer;
            pdfViewer.Open(pdf);

        }

        
    }
}
