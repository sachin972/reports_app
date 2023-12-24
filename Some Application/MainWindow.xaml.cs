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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Aspose.Pdf.Facades;
using ceTe.DynamicPDF.Viewer;
using WkHtmlToPdfDotNet;

namespace Some_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string dbPathText = string.Empty;
        static string dbPasswordText = string.Empty;
        static string connectionString = string.Empty;
        List<List<string>> buyersList;
        List<string> buyerDetails;
        List<string> columns;
        List<List<object>> items;
        List<string> divData;
        string currBuyer;
        int currIndex;
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private SynchronizedConverter converter = new SynchronizedConverter(new PdfTools());



        public MainWindow()
        {
            
            InitializeComponent();
            this.Title = "Reports Generator"; // Set the title of the window
            // ddList.Items.Add("Buyers Report");
            // ddList.Items.Add("Sellers Report");
            this.WindowStyle = WindowStyle.SingleBorderWindow; // Set the window style
            this.ResizeMode = ResizeMode.CanResize; // Allow resizing the window
            this.WindowState = WindowState.Normal;

            
            

            this.columns = new List<string>();
            this.columns.Add("M. NO.");
            this.columns.Add("Item");
            this.columns.Add("Size");
            this.columns.Add("Quantity");
            this.columns.Add("Full");
            this.columns.Add("Half");
            this.columns.Add("Weight");
            this.columns.Add("Rate");
            this.columns.Add("Amount");
            this.columns.Add("Gr. No.");
            this.columns.Add("P. Code");

            //this.pdfViewer = new PdfViewer();

            string directoryPath = $@"{documentsPath}\reports\";

            // Check if the directory exists
            if (!Directory.Exists(directoryPath))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath);
            }
        }


       

        

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            
            if (MyDatabasePath.Text != "")
            {
                //txtSearchPlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                //txtSearchPlaceholder.Visibility = Visibility.Visible;
            }
            dbPathText = MyDatabasePath.Text;
        }

        private void password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dbPassword.Text != "")
            {
                //txtSearchPlaceholder2.Visibility = Visibility.Hidden;
                dbPasswordText = dbPassword.Text;
            }
            else
            {
                //txtSearchPlaceholder2.Visibility = Visibility.Visible;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Set the file dialog to display a folder selection option
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = "Access Database (*.mdb)|*.mdb";
            Nullable<bool> result = openFileDialog.ShowDialog();
            // Get the path from the user
            if (result == true)
            {
                string selectedPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                dbPathText = selectedPath;
                MyDatabasePath.Text = dbPathText;
                
            }
        }


        private void dbButton_Click(object sender, RoutedEventArgs e)
        {
            this.buyerDetails = Database_Connection.getBuyerDetails(connectionString, this.currBuyer);
            this.items = Database_Connection.getItemDetails(connectionString, this.currIndex);
            int remBalance = Database_Connection.getRemainingBalance(connectionString, this.currBuyer);




            //var converter = new BasicConverter(new PdfTools());

            string headers = $@"<tr style=""border: 1px solid black; "" >";




            for (int i = 0; i < this.columns.Count; i++)
            {
                headers += $@"<th style=""padding-left: 10px; padding-right: 50px; border-bottom: 1px solid black; border-top: 1px solid black;"" > {this.columns[i]} </th>";
            }

            headers += "</tr>";


            string finalItems = "";
            int count = 0;
            int totQuantity = 0;
            int totalMoney = 0;
            int totFull = 0;
            int totalHalf = 0;

            for (int i = 0; i < this.items.Count; i++)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                finalItems += "<tr>";
                bool full = false, half = false;
                for (int j = 0; j < this.items[i].Count; j++)
                {

                    //Console.WriteLine(this.items[i][j]);
                    if (j == 46)
                    {
                        dict.Add("name", this.items[i][j].ToString());
                    }
                    if (j == 5)
                    {
                        dict.Add("size", this.items[i][j].ToString());
                    }
                    if (j == 6)
                    {
                        dict.Add("quantity", this.items[i][j].ToString());
                        totQuantity += int.Parse(this.items[i][j].ToString());
                        
                            if((string.IsNullOrEmpty((string)this.items[i][40]) ? "" : this.items[i][40].ToString()) == "H")
                            {
                                dict.Add("half", this.items[i][j].ToString());
                                half = true;
                                totalHalf += int.Parse(this.items[i][j].ToString());
                            }
                            else
                            {
                                dict.Add("full", this.items[i][j].ToString());
                                full = true;
                                totFull += int.Parse(this.items[i][j].ToString());
                        }
                        

                    }
                    if (j == 9)
                    {
                        dict.Add("weight", this.items[i][j].ToString());
                    }
                    if (j == 7)
                    {
                        dict.Add("rate", this.items[i][j].ToString());
                    }
                    if (j == 21)
                    {
                        dict.Add("amount", this.items[i][j].ToString());
                        totalMoney += int.Parse(this.items[i][j].ToString());
                    }
                    if (j == 66)
                    {
                        dict.Add("truck_no", string.IsNullOrEmpty((string)this.items[i][j]) ? "" : this.items[i][j].ToString());
                    }
                    if(j == 67)
                    {
                        dict.Add("party", string.IsNullOrEmpty((string)this.items[i][j]) ? "" : this.items[i][j].ToString());
                    }
                    //sorted.Add(dict);
                }
                if(!full)
                dict.Add("full", "");
                dict.Add("srno", (i + 1).ToString());
                if(!half)
                dict.Add("half", "");
               

                finalItems += $@"<td style=""padding-left: 30px; padding-right: 50px; "">{dict["srno"]}</td>";
                finalItems += $@"<td style=""padding-left: 30px; padding-right: 50px; "">{dict["name"]}</td>";
                finalItems += $@"<td style=""padding-left: 30px; padding-right: 50px; "">{dict["size"]}</td>";
                finalItems += $@"<td style=""padding-left: 30px; padding-right: 50px; "">{dict["quantity"]}</td>";
                finalItems += $@"<td style=""padding-left: 30px; padding-right: 50px; "">{dict["full"]}</td>";
                finalItems += $@"<td style=""padding-left: 30px; padding-right: 50px; "">{dict["half"]}</td>";
                finalItems += $@"<td style=""padding-left: 30px; padding-right: 50px; "">{dict["weight"]}</td>";
                finalItems += $@"<td style=""padding-left: 30px; padding-right: 50px; "">{dict["rate"]}</td>";
                finalItems += $@"<td style=""padding-left: 30px; padding-right: 50px; "">{dict["amount"]}</td>";
                finalItems += $@"<td style=""padding-left: 30px; padding-right: 50px; "">{dict["truck_no"]}</td>";
                finalItems += $@"<td style=""padding-left: 30px; padding-right: 50px; "">{dict["party"]}</td>";
                finalItems += "</tr>";
                count++;
            }
            Console.WriteLine(this.buyerDetails[3].Length);
            int commissionAmount = 0;
            int netAmount = totalMoney + commissionAmount;
            int totalAmount = totalMoney + commissionAmount + remBalance;



            string htmlContent = $@"
                <table style="" padding: 0px;"">
        <thead>
            <tr style="" width:100%;padding: 0px;margin: 0px"">
                <td colspan=""2"" style=""font-size: 30px; text-align:center; border-bottom: 1px solid black;"">
                    <div>{this.divData[0]}</div>
                </td>
                <td colspan=""6"" style=""font-size: 30px; text-align:center; border-bottom: 1px solid black;"">
                    <div>{this.divData[1]}</div>
                </td>
                <td colspan=""3"" style=""font-size: 30px; text-align:center; border-bottom: 1px solid black;"">
                    <div>{this.divData[2]}</div>
                </td>
            <tr style="" width:100%;padding: 0px;margin: 0px"">
                <td colspan=""8"" style=""font-size: 30px; text-align:center; "">
                    <div>{this.divData[4]}</div>
                </td>
                <td colspan=""3"" style=""font-size: 23px; text-align:center; "">
                    <div>{this.divData[3]}</div>
                </td>
            </tr>
            <tr style="" width:100%;padding: 0px;margin: 0px"">
                <td colspan=""7"" style=""font-size: 23px; padding-left:10px; "">
                    P.Name: <b>{this.buyerDetails[0]}</b>
                </td>
                <td colspan=""4"">
                    Date: {this.buyerDetails[3]}
                </td>
            </tr>
            <tr style="" width:100%;padding: 0px;margin: 0px"">
                <td colspan=""7"" style=""font-size: 23px; padding-left:10px "">
                    Address: {this.buyerDetails[1] + ", " + this.buyerDetails[2]}
                </td>
                <td colspan=""4"">
                    Invoice
                </td>
            </tr>
        </thead>" + headers +

        finalItems +
        $@"<tr>
            <td style=""padding-left: 30px; padding-right: 50px; "">{count + 1}</td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
        </tr>

        <tr style="" "">
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; "">{count + 2}</td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
        </tr>
        <tr>
            <td style=""padding-left: 10px; padding-right: 10px; "">T. Qtty: {totQuantity}</td>
            <td style=""padding-left: 30px; padding-right: 50px; padding-top: 20px border: 1px solid black;"" colspan=2> Bar Code Print</td>
            
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; "" colspan=3></td>
            <td style=""padding-left: 30px; padding-right: 50px; "" >GROSS</td>
            <td style=""padding-left: 30px; padding-right: 50px; "" >{totalMoney}</td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
        </tr>
<tr>
            <td style=""padding-left: 10px; padding-right: 50px; "">Full: {totFull}</td>
            <td style=""padding-left: 30px; padding-right: 50px; "" colspan=2></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            
            <td style=""padding-left: 30px; padding-right: 50px; "" colspan=3></td>
            <td style=""padding-left: 30px; padding-right: 50px; "">E. Name</td>
            <td style=""padding-left: 30px; padding-right: 50px; "">{commissionAmount}</td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
<td style=""padding-left: 30px; padding-right: 50px; ""></td>
        </tr>
<tr>
            <td style=""padding-left: 10px; padding-right: 50px; "">Half: {totalHalf}</td>
            
            <td style=""padding-left: 30px; padding-right: 50px; "" colspan=2></td>
<td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; "" colspan=3></td>
            <td style=""padding-left: 30px; padding-right: 50px; "">E. Name</td>
            <td style=""padding-left: 30px; padding-right: 50px; "">{commissionAmount}</td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
<td style=""padding-left: 30px; padding-right: 50px; ""></td>
        </tr>
<tr>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; "" colspan=2></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; "" colspan=3></td>
            <td style=""padding-left: 30px; padding-right: 50px; "">Today Bill</td>
            <td style=""padding-left: 30px; padding-right: 50px; "">{netAmount}</td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
<td style=""padding-left: 30px; padding-right: 50px; ""></td>
        </tr>
<tr>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; "" colspan=2></td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; "" colspan=3></td>
            <td style=""padding-left: 30px; padding-right: 50px; "">Old Balance</td>
            <td style=""padding-left: 30px; padding-right: 50px; "">{remBalance}</td>
            <td style=""padding-left: 30px; padding-right: 50px; ""></td>
<td style=""padding-left: 30px; padding-right: 50px; ""></td>
        </tr>
<tr>
            
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; "" colspan=2></td>
<td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; "" colspan=3></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; "">Net Amount</td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; "">{totalAmount}</td>
<td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
            <td style=""padding-left: 30px; padding-right: 50px; border-bottom: 1px solid black; ""></td>
        </tr>

    </table>
            ";
            string currentDate = DateTime.Now.ToString("MM/dd/yyyy_HH-mm-ss");
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = WkHtmlToPdfDotNet.Orientation.Landscape,
                PaperSize = WkHtmlToPdfDotNet.PaperKind.A4Plus,
                Margins = new MarginSettings() { Top = 10 },
                Out = $@"{documentsPath}\reports\{this.buyerDetails[0] + "_" + currentDate}.pdf",
            },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                    }
                }
            };

            converter.Convert(doc);
            Console.WriteLine(documentsPath);
            //System.Diagnostics.Process.Start($"{documentsPath}\test.pdf");

            //PdfDocument document = new PdfDocument($"{documentsPath}/test.pdf");

            //this.pdfViewer.Open(document);

            BuyersWindow newWindow = new BuyersWindow(this.buyerDetails[0] + "_" + currentDate);
newWindow.Show();


        }

        private void ChangedSelection(object sender, SelectionChangedEventArgs e)
        {
            //txtSearchPlaceholder2_Copy.Visibility = Visibility.Hidden;
            this.currBuyer = ddList.SelectedItem as string;

            for (int i = 0; i < this.buyersList.Count; i++)
            {
                if (this.currBuyer == this.buyersList[i][1])
                {
                    this.currIndex = int.Parse(string.IsNullOrEmpty(this.buyersList[i][0].Trim()) ? "" : this.buyersList[i][0].Trim());
                }
            }
        }

        private void dbButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            MyDatabasePath.Text = dbPathText;
            dbPasswordText = dbPassword.Text;
            connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPathText}/hundidb2k.mdb;Jet OLEDB:Database Password={dbPasswordText};";
            this.buyersList = Database_Connection.getBuyersList(connectionString);
            this.divData = Database_Connection.getDivData(connectionString);
            for (int i = 0; i < this.buyersList.Count; i++)
            {
                ddList.Items.Add(string.IsNullOrEmpty(this.buyersList[i][1]) ? "" : this.buyersList[i][1]);
                Console.WriteLine(this.buyersList[i][1]);
            }
            
        }

        private void dbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if(dbPassword.Text == "Enter Password")
            {
                dbPassword.Text = "";
                dbPassword.Foreground = Brushes.Black;
            }

        }

        private void dbPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dbPassword.Text == "")
            {
                dbPassword.Text = "Enter Password";
                dbPassword.Foreground = Brushes.DimGray;
            }
        }

        private void MyDatabasePath_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MyDatabasePath.Text == "Enter Database Path")
            {
                MyDatabasePath.Text = "";
                MyDatabasePath.Foreground = Brushes.Black;
            }
        }

        private void MyDatabasePath_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MyDatabasePath.Text == "")
            {
                MyDatabasePath.Text = "Enter Database Path";
                MyDatabasePath.Foreground = Brushes.DimGray;
            }
        }
    }
}
