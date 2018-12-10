using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab08Lib;
using System.IO;

namespace Lab10
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            gvSubs.AutoGenerateColumns = false;
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Name";
            column.Name = "Имя";
            gvSubs.Columns.Add(column);
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "SecondName";
            column.Name = "Фамилия";
            gvSubs.Columns.Add(column);
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "PhoneNum";
            column.Name = "Номер телефона";
            gvSubs.Columns.Add(column);
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Country";
            column.Name = "Страна";
            gvSubs.Columns.Add(column);
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Cost";
            column.Name = "Стоимость пакета услуг";
            gvSubs.Columns.Add(column);
            column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "HasCost";
            column.Name = "Оплата";
            column.Width = 60;

            gvSubs.Columns.Add(column);
            bindSrcSubs.Add(new Sub("Никита", "Ковальчук", "(095)092-3292", "Украина", 175, true));
            EventArgs args = new EventArgs();
            OnResize(args);

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Sub sub = new Sub();
            SubForm ft = new SubForm(sub);
            if (ft.ShowDialog() == DialogResult.OK)
            {
                bindSrcSubs.Add(sub);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Sub sub = (Sub)bindSrcSubs.List[bindSrcSubs.Position];
            SubForm ft = new SubForm(sub);
            if (ft.ShowDialog() == DialogResult.OK)
            {
                bindSrcSubs.List[bindSrcSubs.Position] = sub;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Видалити поточний запис?", "Видалення запису", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                bindSrcSubs.RemoveCurrent();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Очистити таблицю?\n\nВсі дані будуть втрачені", "Очищення даних", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                bindSrcSubs.Clear();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Закрити застосунок?", "Вихід з програми", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void btnSaveAsText_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Текстові файли (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Зберегти дані у текстовому форматі";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            StreamWriter sw;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8);
                try
                {
                    foreach (Sub sub in bindSrcSubs.List)
                    {
                        sw.Write(sub.Name + "\t" 
                            + sub.SecondName + "\t"
                            + sub.PhoneNum + "\t"
                            + sub.Country + "\t"
                            + sub.Cost + "\t"
                            + sub.HasCost + "\t\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    sw.Close();
                }
            }
        }

        private void btnSaveAsBinary_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Файли даних (*.subs)|*.subs|All files (*.*)|*.*";
            saveFileDialog.Title = "Зберегти дані у бінарному форматі";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            BinaryWriter bw;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                bw = new BinaryWriter(saveFileDialog.OpenFile());
                try
                {
                    foreach (Sub sub in bindSrcSubs.List)
                    {
                        bw.Write(sub.Name);
                        bw.Write(sub.SecondName);
                        bw.Write(sub.PhoneNum);
                        bw.Write(sub.Country);
                        bw.Write(sub.Cost);
                        bw.Write(sub.HasCost);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    bw.Close();
                }
            }
        }

        private void btnOpenFromText_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Текстові файли (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Прочитати дані у текстовому форматі";
            openFileDialog.InitialDirectory = Application.StartupPath;
            StreamReader sr;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bindSrcSubs.Clear();
                sr = new StreamReader(openFileDialog.FileName, Encoding.UTF8);
                string s;
                try
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] split = s.Split('\t');
                        Sub sub = new Sub(split[0],
                            split[1], 
                            split[2],
                            split[3],
                            int.Parse(split[4]),
                            bool.Parse(split[5]));

                        bindSrcSubs.Add(sub);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    sr.Close();
                }
            }

        }

        private void btnOpenFromBinary_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Файли даних (*.subs)|*.subs|All files (*.*)|*.*";
            openFileDialog.Title = "Прочитати дані у бінарному форматі";
            openFileDialog.InitialDirectory = Application.StartupPath;
            BinaryReader br;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bindSrcSubs.Clear();
                br = new BinaryReader(openFileDialog.OpenFile());
                try
                {
                    Sub sub;
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        sub = new Sub();
                        for (int i = 1; i <= 8; i++)
                        {
                            switch (i)
                            {
                                case 1:
                                    sub.Name = br.ReadString();
                                    break;

                                case 2:
                                    sub.SecondName = br.ReadString();
                                    break;

                                case 3:
                                    sub.PhoneNum = br.ReadString();
                                    break;

                                case 4:
                                    sub.Country = br.ReadString();
                                    break;

                                case 5:
                                    sub.Cost = br.ReadInt32();
                                    break;

                                case 6:
                                    sub.HasCost = br.ReadBoolean();
                                    break;
                            }
                        }
                        bindSrcSubs.Add(sub);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    br.Close();
                }
            }
        }
    }
}
