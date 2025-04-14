using System.Net.Sockets;
using System.Text.Json;
using System.Text;

namespace Chat_Client
{
    public partial class Form1 : Form
    {
        public Socket socket;
        private bool file_v = false;
        private int fileSize;

        public Form1()
        {
            InitializeComponent();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }
        
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string msg = richTextBox2.Text;

                //Создаем объект класса Msg_t_c 
                Msg_t_c Msg_t_c = new Msg_t_c("msg", msg, null);

                //Сериализуем объект Msg_t_c в json строку   ->    {"Command":"name","Text":"$textBox1.Text$"}
                string json_msg = JsonSerializer.Serialize(Msg_t_c);

                //формируем байтовый массив из строки json_msg
                byte[] requestData = Encoding.UTF8.GetBytes(json_msg + '\n');

                // отправляем данные
                await socket.SendAsync(requestData, SocketFlags.None);

                richTextBox2.Text = "";

            }
            catch (SocketException)
            {
                Console.WriteLine($"Не удалось установить подключение с {socket.RemoteEndPoint}");
            }
        }
        

        private async void button3_Click(object sender, EventArgs e)
        {

            //Создаем объект класса Msg_t_c 
            byte[] requestData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new Msg_t_c("get_client", "", null)) + '\n');
            // отправляем данные
            await socket.SendAsync(requestData, SocketFlags.None);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            //byte[] requestData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new Msg_t_c("file", "", null)) + '\n');
            //await socket.SendAsync(requestData, SocketFlags.None);

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Выберите изображение";
                dialog.Filter = "Изображения (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream stream = new FileStream(dialog.FileName, FileMode.Open))
                        {
                            PictureBox pictureB = new PictureBox();
                            pictureB.Image = Image.FromStream(stream);

                            // Отправляем уведомление о файле
                            byte[] requestData = Encoding.UTF8.GetBytes(
                                JsonSerializer.Serialize(new Msg_t_c("file", dialog.FileName, null)) + '\n'
                            );
                            await socket.SendAsync(requestData, SocketFlags.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}");
                    }
                }
            }

        }
            

        public async void connect(string name)
        {
            //Подключение к серверу
            socket.ConnectAsync("127.0.0.1", 8888);

            string msg = name;

            //Создаем объект класса Msg_t_c 
            Msg_t_c Msg_t_c = new Msg_t_c("name", msg, people: null);

            //Сериализуем объект Msg_t_c в json строку   ->    {"Command":"name","Text":"$textBox1.Text$"}
            string json_msg = JsonSerializer.Serialize(Msg_t_c);

            //формируем байтовый массив из строки json_msg
            byte[] requestData = Encoding.UTF8.GetBytes(json_msg + '\n');
            //отправляем сообщение серверу
            await socket.SendAsync(requestData, SocketFlags.None);

            // запуск задачи на прослушивание сервера
            receive_(socket);
        }

        async Task receive_(Socket socket)
        {
            try
            {
                while (true)
                {
                    // буфер для накопления входящих данных
                    var buffer = new List<byte>();

                    // буфер для считывания одного байта
                    var bytesRead = new byte[1];

                    // считываем данные до конечного символа
                    while (true)
                    {
                        var count = await socket.ReceiveAsync(bytesRead, SocketFlags.None);
                        //смотрим, если считанный байт представляет конечный символ, выходим
                        if (!file_v)
                            if (count == 0 || bytesRead[0] == '\n') break;
                        //иначе добавляем в буфер
                        buffer.Add(bytesRead[0]);
                        if (file_v)
                        {
                            //richTextBox1.Text += fileSize + "-" + Encoding.UTF8.GetString(bytesRead);
                            fileSize--;
                            if (fileSize == 0) break;
                        }
                    }

                    if (file_v)
                    {
                        Add_file(buffer.ToArray());
                        file_v = false;
                    }
                    else
                    {
                        string responseText = Encoding.UTF8.GetString([.. buffer]);
                        Msg_t? r_m_t = JsonSerializer.Deserialize<Msg_t>(responseText);
                        Add_msg(r_m_t);
                    }


                }
            }
            catch (SocketException)
            {
                Console.WriteLine($"Не удалось установить подключение с {socket.RemoteEndPoint}");
            }
        }

        public void Add_msg(Msg_t r_m_t)
        {
            TextBox textBox1 = new TextBox();
            textBox1.Text = r_m_t.Text;

            textBox1.BackColor = System.Drawing.SystemColors.Control;
            textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox1.Margin = new System.Windows.Forms.Padding(3);
            textBox1.Padding = new System.Windows.Forms.Padding(3);
            textBox1.ForeColor = System.Drawing.Color.Green;
            textBox1.Size = new System.Drawing.Size(flowLayoutPanel1.Size.Width - 10, 23);

            textBox1.Text = r_m_t.Sender + " : " + r_m_t.Text;

            if (r_m_t.Command.IndexOf("my_msg") == 0)
            {
                
                textBox1.ForeColor = System.Drawing.Color.Blue;
                textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            }

            if (r_m_t.Command.IndexOf("list_client") == 0)
            {
                textBox1.Text = JsonSerializer.Serialize(r_m_t.People);
            }

            if (r_m_t.Command.IndexOf("file") == 0)
            {

                fileSize = Convert.ToInt32(r_m_t.Text);
                richTextBox1.Text += fileSize;
                file_v = true;
            }
            
            flowLayoutPanel1.Controls.Add(textBox1);

        }

        public void Add_file(byte[] buffer)
        {
            PictureBox pictureB = new PictureBox();
            pictureB.Size = new Size(200, 150);
            pictureB.SizeMode = PictureBoxSizeMode.Zoom;
            using (MemoryStream ms = new MemoryStream(buffer))
                pictureB.Image = Image.FromStream(ms);
            flowLayoutPanel1.Controls.Add(pictureB);
        }
    }
}
