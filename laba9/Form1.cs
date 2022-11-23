using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace laba9
{
    public partial class Form1 : Form
    {
        enum WriteMode { Append, Rewrite}
        public Form1()
        {
            InitializeComponent();

            //Функционал для того чтобы можно было в винде использовать "Открыть с помощью..."
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                var nums = ReadNumbersBinary(args[1]);
                listBox1.Items.Clear();
                for (int i = 0; i < nums.Count; i++)
                {
                    listBox1.Items.Add((i + 1).ToString() + ".\t" + nums[i].ToString());
                }
            }
            //
        }
        void WriteNumbersBinary(List<int> numbers, string @path, WriteMode mode)
        //Функция для записи чисел numbers в файл с адресом path
        //WriteMode - способ записи, есть 2 варианта:
        //WriteMode.Append - дозаписать в файл, сохранив предыдущие значения
        //WriteMode.Rewrite - удаляет предыдущий файл по заданному адресу, и создает на его месте новый
        {
            switch (mode)
            {
                case WriteMode.Append:
                {
                        if (File.Exists(path) == false)
                            throw new Exception("Файл не существует!");
                        else
                        {
                            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                            {
                                foreach (int number in numbers)
                                {
                                    writer.Write(number);
                                }
                            }
                        }
                    break;
                }
                case WriteMode.Rewrite:
                {
                    if (File.Exists(path))
                        File.Delete(path);

                    using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                    {
                        foreach (int number in numbers)
                        {
                            writer.Write(number);
                        }
                    }
                    break;
                }
                default:
                    throw new Exception("Не задан режим записи файла!");
            }
        }
        List<int> ReadNumbersBinary(string @path) //Функция для чтения чисел из файла с адресом path
        {
            List<int> result = new List<int>();

            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                while (reader.PeekChar() > -1)
                {
                    result.Add(reader.ReadInt32());
                }
                reader.Close();
            }

            return result;
        }

        private void button1_Click(object sender, EventArgs e) //Задание 1: Запись чисел в файл
        {
            //Окно выбора адреса для файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Бинарный файл с числами (*.binn)|*.binn";
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel) return;
            string path = saveFileDialog.FileName;

            //Генерация чисел
            List<int> nums = new List<int>();
            Random random = new Random();
            for(int i = 0; i < 100; i++)
            {
                nums.Add(random.Next(100));
            }
            try //Запись чисел в файл
            {
                WriteNumbersBinary(nums,path,WriteMode.Rewrite);
            }
            //Информация по итогу операции: ошибка или сообщение об успешном завершении
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            MessageBox.Show("Файл сохранён.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e) //Задание 1: Чтение чисел из файла
        {
            //Окно выбора файла для открытия
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Бинарный файл с числами (*.binn)|*.binn";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
            string path = openFileDialog.FileName;

            //Чтение чисел из файла в переменную nums
            List<int> nums = new List<int>();
            nums = ReadNumbersBinary(path);

            //Выводим полученные числа в listbox
            listBox1.Items.Clear();
            for(int i=0; i < nums.Count;i++)
            {
                listBox1.Items.Add((i+1).ToString() + ".\t" + nums[i].ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e) //Задание 2: Вставка числа на случайную позицию
        {
            //Окно выбора файла для открытия
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Бинарный файл с числами (*.binn)|*.binn";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
            string path = openFileDialog.FileName;

            //Чтение чисел из файла в переменную nums
            List<int> nums = new List<int>();
            nums = ReadNumbersBinary(path);

            Random rnd = new Random(); //Объект рандома
            var rn = rnd.Next(nums.Count); //Случайный номер позиции для вставки числа
            nums.Insert(rn, (int)numericUpDown1.Value); //Вставляем число из numericUpDown в полученную на пред. строчке позицию
            WriteNumbersBinary(nums, path, WriteMode.Rewrite); //Перезаписываем файл с учётом вставки

            //Выводим информацию о вставке
            MessageBox.Show("Число " + numericUpDown1.Value.ToString() + " вставлено на позицию " +
                (rn+1).ToString(), "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Бинарный файл с числами (*.binn)|*.binn";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
            string path = openFileDialog.FileName;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Бинарный файл с числами (*.binn)|*.binn";
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel) return;
            string path2 = saveFileDialog.FileName;

            File.Copy(path, path2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Бинарный файл с числами (*.binn)|*.binn";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
            string path = openFileDialog.FileName;

            FileInfo fileInfo = new FileInfo(path);

            MessageBox.Show("Дата и время создания файла: " + fileInfo.CreationTime.ToString() + Environment.NewLine
                + "Дата и время последнего доступа: " + fileInfo.LastAccessTime.ToString() + Environment.NewLine
                + "Дата и время последней записи: " + fileInfo.LastWriteTime.ToString(), "Информация о файле");
        }
    }
}
