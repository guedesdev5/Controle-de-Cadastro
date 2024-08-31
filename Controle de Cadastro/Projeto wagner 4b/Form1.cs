using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Projeto_wagner_4b
{
    public partial class Form1 : Form
    {
       
        string caminhoarquivo;
        int ponteiro = 0;
        int qntd = 0;
        string database = "SERVER=localhost;DATABASE=bancocti;UID=root;PASSWORD=pietro29012007;";

        void create()
        {
            MySqlConnection connection = new MySqlConnection(database);
            MySqlCommand command = connection.CreateCommand();
            connection.Open();
            string nome = primeira_letra(textBox1.Text);
            string cidade = primeira_letra(textBox4.Text);
            string bairro = primeira_letra(textBox3.Text);
            command.CommandText = "insert into usuarios (codigo, nome, cidade, bairro, telefone, diretorio) values (" + comboBox1.Text + ", '" + nome + "', '" + cidade + "', '" + bairro + "', '" + textBox2.Text + "', '"+caminhoarquivo+"') ";
            MySqlDataReader Query = command.ExecuteReader();
            comboBox1.Items.Add(comboBox1.Text);
            registros();
            connection.Close();

        }

        string primeira_letra(string frase)
        {
            string resposta = " ";

            var nn = new string[] { "dos", "das", "de", "da", "do" };

            var palavras = frase.Split(' ');
            string primeira = " ";
            int h = 0;

            foreach(string x in palavras)
            {
                if (nn.Contains(x))
                {
                    resposta += " " +x ;
                }
                else
                {
                    primeira = x.Substring(0, 1);
                    primeira = primeira.ToUpper();
                    h = x.Length;
                    resposta += " "+ primeira + x.Substring(1, h - 1);
                }
                    
            
                    
            }
            resposta = resposta.Trim();
            return resposta;

        }

        void registros()
        {
            MySqlConnection connection = new MySqlConnection(database);
            MySqlCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = "select COUNT(*) from usuarios ";
            qntd = 0;
            MySqlDataReader Query = command.ExecuteReader();
            if (Query.Read())
            {
                qntd = Query.GetInt32(0);
            }
            connection.Close();
        }


        MySqlDataReader read_inicializa()
        {
            MySqlConnection connection = new MySqlConnection(database);
            MySqlCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = "select codigo from usuarios ";
            MySqlDataReader Query = command.ExecuteReader();

            return Query;
        }

        MySqlDataReader read_textsboxes()
        {
            MySqlConnection connection = new MySqlConnection(database);
            MySqlCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = "select * from usuarios where codigo = " + comboBox1.Text + " ";
            MySqlDataReader Query = command.ExecuteReader();

            return Query;
        }

        void carrega_dados(int indice)
        {
            MySqlConnection connection = new MySqlConnection(database);
            MySqlCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = "select * from usuarios ";
            MySqlDataReader Query = command.ExecuteReader();

            string codigo = " ";
            string nome = "";
            string cidade = "";
            string bairro = "";
            string telefone = "";
            string diretorio = " ";

            for (int y = 0; y <= indice; y++)
            {
                if (Query.Read())
                {
                    codigo = Query.GetString("codigo");
                    nome = Query.GetString("nome");
                    cidade = Query.GetString("cidade");
                    bairro = Query.GetString("bairro");
                    telefone = Query.GetString("telefone");
                    diretorio = Query.GetString("diretorio");
                }

            }
            comboBox1.Text = codigo;
            textBox1.Text = nome;
            textBox4.Text = cidade;
            textBox3.Text = bairro;
            textBox2.Text = telefone;
            pictureBox1.Load("C:\\imagem\\" + diretorio);
            connection.Close();          
        }

        MySqlDataReader delete()
        {
            MySqlConnection connection = new MySqlConnection(database);
            MySqlCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = "delete from usuarios where codigo = "+comboBox1.Text;
            MySqlDataReader Query = command.ExecuteReader();
            registros();
            pictureBox1.Load("C:\\imagem\\deletado.png" );

            return Query;
        }

        MySqlDataReader update()
        {
            MySqlConnection connection = new MySqlConnection(database);
            MySqlCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = "update (nome, cidade, bairro, telefone) from usuarios where codigo = " + comboBox1.Text;
            MySqlDataReader Query = command.ExecuteReader();
            registros();

            return Query;
        }

        public Form1()
        {
            InitializeComponent();
        }   
        private void label9_Click(object sender, EventArgs e)
        {}

        private void label6_Click(object sender, EventArgs e)
        {}

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length != 8)
                MessageBox.Show("Campo 'codigo' deve ter 8 digitos!");
            else
                create();          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Load("C:\\imagem\\deletado.png");
            registros();
            label2.Text = " ";
            label3.Text = " ";
            label4.Text = " ";
            MySqlDataReader query = read_inicializa();
            string nome = "";
            int i = 0;
            while (query.Read())
            {
                nome = query.GetString("codigo");
                comboBox1.Items.Insert(i++, nome);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MySqlDataReader query = read_textsboxes();
            string nome = "";
            string cidade = "";
            string bairro = "";
            string telefone = "";
            string diretorio = " ";
            while (query.Read())
            {
                nome = query.GetString("nome");
                cidade = query.GetString("cidade");
                bairro = query.GetString("bairro");
                telefone = query.GetString("telefone");
                diretorio = query.GetString("diretorio");
                pictureBox1.Load("C:\\imagem\\" + diretorio);
                textBox1.Text = nome;
                textBox4.Text = cidade;
                textBox3.Text = bairro;
                textBox2.Text = telefone;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            label2.Text = "Deseja excluir o registro?";
            label3.Text = "S";
            label4.Text = "N";
            
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog1.FileName;
                pictureBox1.Load();

                caminhoarquivo = openFileDialog1.SafeFileName;           
           }       
        }
        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {}
        private void button5_Click(object sender, EventArgs e)
        {}

        private void button7_Click(object sender, EventArgs e)
        {}
        private void label4_MouseClick(object sender, MouseEventArgs e)
        {
            label2.Text = " ";
            label3.Text = " ";
            label4.Text = " ";
        }

        private void label3_MouseClick(object sender, MouseEventArgs e)
        {
            delete();
            comboBox1.Items.Remove(comboBox1.Text);
            textBox1.Text = " ";
            textBox2.Text = " ";
            textBox3.Text = " ";
            textBox4.Text = " ";
            comboBox1.Text = " ";
            label2.Text = " ";
            label3.Text = " ";
            label4.Text = " ";
        }

        private void button6_Click(object sender, EventArgs e)
        {

            if (ponteiro < qntd-1)
            {
                ponteiro++;
                carrega_dados(ponteiro);
            }          
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (ponteiro > 0)
            {
                ponteiro--;
                carrega_dados(ponteiro);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ponteiro = 0;
            carrega_dados(ponteiro);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ponteiro = qntd-1;
            carrega_dados(ponteiro);
        }
        
    }
}
