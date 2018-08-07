using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BJCreation.Helper.Utilies;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.PinYinConverter;

namespace Name.App
{
    public partial class MainForm : Form
    {

        Dictionary<string, int> FirstName = new Dictionary<string, int>();
        Dictionary<string, int> SecondName = new Dictionary<string, int>();
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnNameCount_Click(object sender, EventArgs e)
        {
            var count = 0;
            foreach (var item in this.txtName.Text.ToCharArray())
            {
                ChineseChar chineseChar =
                               new ChineseChar(item);
                count += chineseChar.StrokeNumber;
            }
            MessageBox.Show(count.ToString());

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            FirstName = new Dictionary<string, int>();
            SecondName = new Dictionary<string, int>();
            StringBuilder result = new StringBuilder();
            var file = ConfigHelper.Path + "name.txt";

            var xCount = ChineseHelper.GetStrokeCount(this.txtFirstName.Text);
            if (xCount == 0)
            {
                ChineseChar chineseChar =
                    new ChineseChar(this.txtFirstName.Text.ToCharArray()[0]);
                xCount = chineseChar.StrokeNumber;
            }
            if (!FileHelper.IsExistFile(file))
                MessageBox.Show("对不起，名字字典不存在，请确保安装目录下存在name.txt文件");
            else
            {
                var names = File.ReadAllLines(file, Encoding.UTF8);
                foreach (var item in names)
                {
                    for (int i = 0; i < item.Length; i++)
                    {
                        var c = item.ToCharArray();
                        if (!string.IsNullOrEmpty(c[i].ToString()) && !string.IsNullOrEmpty(c[i].ToString().Trim()))
                        {
                            var cc = c[i].ToString();
                            ChineseChar chineseChar =
                                new ChineseChar(c[i]);
                            var count = chineseChar.StrokeNumber;
                            if (!FirstName.ContainsKey(c[i].ToString()))
                            {
                                FirstName.Add(c[i].ToString(), count);
                                SecondName.Add(c[i].ToString(), count);
                            }
                        }
                    }
                }
                foreach (var fir in FirstName)
                {
                    foreach (var sec in SecondName.Where(x => x.Key != fir.Key).ToList())
                    {
                        var textCount = fir.Value + sec.Value + xCount;
                        var sourceCounts = txtNameCount.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var sourCount in sourceCounts)
                        {
                            if (object.Equals(textCount, int.Parse(sourCount)))
                            {
                                result.Append(this.txtFirstName.Text + fir.Key + sec.Key + ",");
                            }
                        }
                    }
                }
                this.rtbResult.Text = StringHelper.DelLastComma(result.ToString());
            }
        }
    }

}
