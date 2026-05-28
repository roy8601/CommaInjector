using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommaInjector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IComponentConnector
    {
        private string Result;
        private LstRows lstTable = new LstRows();
        //internal ComboBox pkInOrInsert;
        //internal Button btnBreak;
        //internal TextBox txtBroken;
        //private bool _contentLoaded;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void CreateStatement(string mode, string RawText)
        {
            this.Reset();
            this.BreakText(RawText);
            this.CaseMode(mode);
            this.DisplayResult();
        }

        private void Reset()
        {
            this.Result = "";
            this.lstTable = new LstRows();
        }

        private void BreakText(string RawText)
        {
            string[] strArray = RawText.Replace("\r", "").Split('\n');
            LstFields lstFields1 = new LstFields();
            foreach (string str1 in strArray)
            {
                if (!string.IsNullOrEmpty(str1))
                {
                    LstFields lstFields2 = new LstFields();
                    string str2 = str1;
                    char[] chArray = new char[1] { '\t' };
                    foreach (string CellVal in str2.Split(chArray))
                        lstFields2.Add(this.FormatCell(CellVal));
                    this.lstTable.Add(lstFields2);
                }
            }
        }

        private string FormatCell(string CellVal) => $"'{CellVal.Replace("'", "''")}'";

        private void CaseMode(string mode)
        {
            switch (mode)
            {
                case "In":
                    this.In();
                    break;
                case "Insert":
                    this.Insert();
                    break;
                default:
                    this.Result = "Error: Selected mode not supported";
                    break;
            }
        }

        private string InStmt(LstFields lstCells)
        {
            int num1 = 0;
            int num2 = lstCells.Count<string>() - 1;
            string str = "(";
            for (int index = 0; index < lstCells.Count<string>(); ++index)
            {
                ++num1;
                str += lstCells[index];
                if (index < num2)
                    str += ", ";
                if (num1 == 10 & index != num2)
                {
                    num1 = 0;
                    str += "\n";
                }
            }
            return str + ")";
        }

        private void Insert()
        {
            int num1 = 1000;
            string str = "INSERT INTO $table VALUES\n";
            int num2 = 0;
            int num3 = 0;
            foreach (LstFields lstCells in (List<LstFields>)this.lstTable)
            {
                if (num2 == 0)
                    this.Result += str;
                ++num3;
                ++num2;
                this.Result += this.In(lstCells);
                if (num3 != this.lstTable.Count<LstFields>())
                {
                    if (num2 == num1)
                    {
                        this.Result += "\n\n";
                        num2 = 0;
                    }
                    else
                        this.Result += ",\n";
                }
            }
            this.Print(this.Result);
        }

        private void SwitchMode(object sender, RoutedEventArgs e)
        {
            int selectedIndex = this.pkInOrInsert.SelectedIndex;
            ComboBox pkInOrInsert = this.pkInOrInsert;
            int num1;
            int num2;
            if ((num1 = selectedIndex + 1) != this.pkInOrInsert.Items.Count)
            {
                int num3 = num1;
                int num4 = num3 + 1;
                num2 = num3;
            }
            else
                num2 = 0;
            pkInOrInsert.SelectedIndex = num2;
        }

        private void btnBreak_Click(object sender, RoutedEventArgs e)
        {
            this.CreateStatement(this.pkInOrInsert.Text, this.txtBroken.Text);
        }

        private void DisplayResult()
        {
            this.txtBroken.Text = this.Result;
            this.txtBroken.Focus();
            this.txtBroken.SelectAll();
        }

        private void In()
        {
            LstFields lstCells = new LstFields();
            foreach (LstFields collection in (List<LstFields>)this.lstTable)
                lstCells.AddRange((IEnumerable<string>)collection);
            this.Result = this.InStmt(lstCells);
        }

        private string In(LstFields lstCells) => this.InStmt(lstCells);

        private void Print(string text)
        {
        }
    }
}

//testtesttest

public class LstFields : List<string>
{
}

public class LstRows : List<LstFields>
{
}