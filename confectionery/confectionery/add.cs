using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace confectionery
{
    public partial class add : Form
    {
        public add()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str;

            if (Exchange("127.0.0.1", 8888, $"check@{textBox1.Text}") == "no")
            {
                str = Exchange("127.0.0.1", 8888, $"add@{textBox1.Text}" +
                $"#{label2.Text} {numericUpDown1.Value.ToString()}шт:" +
                $"{label3.Text} {numericUpDown2.Value.ToString()}шт:" +
                $"{label4.Text} {numericUpDown3.Value.ToString()}шт:" +
                $"{label5.Text} {numericUpDown4.Value.ToString()}шт:");

                string[] mes = str.Split(new char[] { '#' });
                string[] order = mes[1].Split(new char[] { ':' });

                MessageBox.Show($"{mes[0]} добавил в корзину:\n{order[0]}\n{order[1]}\n{order[2]}\n{order[3]}");
            }
            else
                MessageBox.Show("заказ с таким пользователем уже есть");

        }

        static private string Exchange(string address, int port, string outMessage)
        {
            try
            {
                // Инициализация
                TcpClient client = new TcpClient(address, port);
                Byte[] data = Encoding.UTF8.GetBytes(outMessage);
                NetworkStream stream = client.GetStream();
                try
                {
                    // Отправка сообщения
                    stream.Write(data, 0, data.Length);
                    // Получение ответа
                    Byte[] readingData = new Byte[256];
                    String responseData = String.Empty;
                    StringBuilder completeMessage = new StringBuilder();
                    int numberOfBytesRead = 0;
                    do
                    {
                        numberOfBytesRead = stream.Read(readingData, 0, readingData.Length);
                        completeMessage.AppendFormat("{0}", Encoding.UTF8.GetString(readingData, 0, numberOfBytesRead));
                    }
                    while (stream.DataAvailable);
                    responseData = completeMessage.ToString();
                    return responseData;
                }
                finally
                {
                    stream.Close();
                    client.Close();
                }
            }
            catch (Exception)
            {
                return ("Ожидание сервера...");
            }

        }
    }
}
