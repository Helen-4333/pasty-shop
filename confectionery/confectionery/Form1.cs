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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            add form = new add();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            del form = new del();
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            edit form = new edit();
            form.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            buy form = new buy();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            view form = new view();
            form.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int i = 0;
            string str;

            str = Exchange("127.0.0.1", 8888, $"view_user@");
            string[] mes = str.Split(new char[] { '#' });
            str = "пользователи\n";
            while (i < mes.Length)
            {
                str += $"{mes[i]}\n";
                i++;
            }
            MessageBox.Show(str);
        }
    }
}
