using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Logger_Key
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Int32 vKey);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern UInt32 GetWindowThreadProcessId(IntPtr hwnd, ref Int32 pid);
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);


        public string buf = "";
        public string path1 = "./test_file.txt";
        public string path2 = "./test_file2.txt";
        public string[] mas = null;

        private const int MOUSE_LEFTDOWN = 0x02;
        private const int MOUSE_LEFTUP = 0x04;

        private bool[] keyStates = new bool[256];
        private const int VK_LSHIFT = 0xA0;
        private const int VK_RSHIFT = 0xA1;
        private const int VK_LCONTROL = 0xA2;
        private const int VK_RCONTROL = 0xA3;
        private const int VK_LMENU = 0xA4;
        private const int VK_RMENU = 0xA5;


        public Form1()
        {

            InitializeComponent();

            //Если вдруг нужно посмотреть какие кнопки под каким номером то раскоментировать код ниже
            //for (int key = 7; key < 256; key++)
            //{
            //    //richTextBox1.Text += key + "-" + ((KeysRu)key).ToString()+"\n";
            //}

            //Получаем объект CultureInfo для текущего потока (обычно соответствует языку операционной системы)
            CultureInfo culture = CultureInfo.CurrentCulture;
            //получаем код языка (например, «en» для английского, «fr» для французского и т. д.)
            string languageCode = culture.TwoLetterISOLanguageName;

            //получаем полное название языка (английский, французский)
            string languageName = culture.DisplayName;

            richTextBox2.Text += "код языка: " + languageCode + "\n";
            richTextBox2.Text += "название кода: " + languageName + "\n";

            //Залипание клавиш
            this.KeyDown += FormKeyDown;
            this.KeyUp += FormKeyUp;
            timer3.Start();
        }



        // Таймер срабатывает с частотой 0.1 секунды (выставлено в настройках формы)
        private void timer1_Tick(object sender, EventArgs e)
        {
            int key113 = 0;
            int key70 = 0;
            int key87 = 0;
            int key82 = 0;
            int key79 = 0;



            for (int key = 7; key < 256; key++)
            {
                int state = GetAsyncKeyState(key);

                if (state != 0)
                {
                    richTextBox1.Text += "{" + key + "-" + ((KeysRu)key).ToString() + "}";
                    buf += ((KeysRu)key).ToString();

                    if (key == 113)
                        key113 = 1;
                    if (key == 70)
                        key70 = 1;
                    if (key == 87)
                        key87 = 1;
                    if (key == 82)
                        key82 = 1;
                    if (key == 79)
                        key79 = 1;

                }

            }

            if (key113 != 0 && key70 != 0)
            {
                Process.Start(@"C:\Users\dex\AppData\Local\Yandex\YandexBrowser\Application\browser.exe", "https://visualstudio.microsoft.com/ru/");
            }
            if (key113 != 0 && key87 != 0)
            {
                Process.Start(@"C:\Users\dex\AppData\Local\Yandex\YandexBrowser\Application\browser.exe", "https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.keys?view=windowsdesktop-8.0");
            }
            if (key113 != 0 && key82 != 0)
            {
                Process.Start(@"C:\Users\dex\AppData\Local\Yandex\YandexBrowser\Application\browser.exe", "https://yandex.ru/images/");
            }
            if (key113 != 0 && key79 != 0)
            {
                Process.Start("C:\\Windows\\System32\\Notepad.exe");
            }

            if (buf.Length > 20)
            {
                WriteToTxt(buf);
                buf = "";
            }
        }

        // Таймер срабатывает с частотой 1 секунда (выставлено в настройках формы)
        private void timer2_Tick(object sender, EventArgs e)
        {

            try
            {
                // Получаем активное окно
                IntPtr h = GetForegroundWindow();
                if (h == IntPtr.Zero) return;

                int pid = 0;
                GetWindowThreadProcessId(h, ref pid);

                // Получаем информацию о процессе
                try
                {
                    Process p = Process.GetProcessById(pid);

                    // Формируем строку с информацией
                    string info = $"Id: {p.Id} | ProcessName: {p.ProcessName} | Title: {p.MainWindowTitle}";

                    // Добавляем в richTextBox2 с временной меткой
                    richTextBox2.Text += $"{DateTime.Now:HH:mm:ss} - {info}\n";

                    // Записываем в лог-файл
                    WriteToTxt_2($"{DateTime.Now:HH:mm:ss} - {info}\n");

                    // Проверяем и блокируем нежелательные процессы
                    if (mas.Contains(p.ProcessName))
                    {
                        p.Kill();
                        MessageBox.Show($"Процесс '{p.ProcessName}' был завершен", "Контроль процессов");
                    }
                }
                catch (ArgumentException)
                {
                    // Пропускаем системные процессы
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обработке процесса: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка: {ex.Message}");
            }

        }

        //функция для записи в файл path1 кнопок
        private void WriteToTxt(string value)
        {
            StreamWriter stream = new StreamWriter(path1, true);
            stream.Write(value);
            stream.Close();
        }

        //функция для записи в файл path2 Процессов
        private void WriteToTxt_2(string value)
        {
            StreamWriter stream = new StreamWriter(path2, true);
            stream.Write(value);
            stream.Close();
        }

        //Функция для запуска приложения (скрываем окно и убираем из панели задач)
        private void button1_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Width = 0;
            this.Height = 0;
            this.ShowInTaskbar = false;

            mas = richTextBox3.Text.Split(';');

            if (textBox1.Text != "")
                path1 = textBox1.Text;

            if (textBox2.Text != "")
                path2 = textBox2.Text;

            if (checkBox1.Checked)
                timer1.Start();

            if (checkBox2.Checked)
                timer2.Start();

        }

        //Функция для запуска приложения в тестовом режиме
        private void button2_Click(object sender, EventArgs e)
        {

            mas = richTextBox3.Text.Split(';');


            if (textBox1.Text != "")
                path1 = textBox1.Text;

            if (textBox2.Text != "")
                path2 = textBox2.Text;


            if (checkBox1.Checked)
                timer1.Start();

            if (checkBox2.Checked)
                timer2.Start();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            if (filename.IndexOf("txt") > 0)
            {

                string fileText = System.IO.File.ReadAllText(filename);

                richTextBox1.Text = fileText;
                MessageBox.Show("файл открыт");
            }

        }

        private void timer3_Tick(object sender, EventArgs e) // Автоклик
        {
            // Симулируем нажатие и отпускание левой кнопки мыши
            mouse_event(MOUSE_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSE_LEFTUP, 0, 0, 0, 0);
        }
        //Залипание клавиш
        private void FormKeyDown(object sender, KeyEventArgs e)
        {

            keyStates[(int)e.KeyCode] = true;            // Сохраняем состояние клавиши
        }

        private void FormKeyUp(object sender, KeyEventArgs e)
        {

            keyStates[(int)e.KeyCode] = false;         // Сбрасываем состояние клавиши
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
        }


    }
}
