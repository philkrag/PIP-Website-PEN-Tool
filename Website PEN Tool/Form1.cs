using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Website_PEN_Tool
{
    public partial class Form1 : Form
    {

        public string data = "";

        public Form1()
        {
            InitializeComponent();
        }

        public void button3_Click(object sender, EventArgs e)
        {

            bool Data_Received = false;
            int Form_Counter = 0;
            int Item_Counter = 0;

            string[] Form_Action = new string[1000];

            string[,] Input = new string[100, 100];
            //string[,] Input = new string[100, 100];
            //string 

                string[] Default_Input = new string[1000];

            string[] result = new string[1000];

            


            try
            {
                string urlAddress = textBox1.Text;



                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Data_Received = true;

                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    data = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                }

                textBox13.Text = data;
            }
            catch { }

            if (Data_Received)
            {




                result = data.Split(new string[] { "\n" }, StringSplitOptions.None);

                Form_Counter = 0;

                //textBox3.Text = data;

                for (int i = 0; i < result.Count(); i++)
                {
                    if (result[i].Contains("<form"))
                    {
                        try
                        {
                            //int pFrom = result[i].IndexOf("action=\"") + "action=\"".Length;
                            //int pTo = result[i].LastIndexOf("\" method");
                            //String test = result[i].Substring(pFrom, pTo - pFrom);

                            string item = Get_Item_Content(result[i], "action=\"", "\"");

                            Form_Action[Form_Counter] = item;
                            Form_Counter = Form_Counter + 1;
                            Item_Counter = 0;
                        }
                        catch { }
                    }

                    if (result[i].Contains("<input"))
                    {
                        Input[Form_Counter, Item_Counter] = Get_Item_Content(result[i], "name=\"", "\"");
                        Default_Input[Item_Counter] = Get_Item_Content(result[i], "value=\"", "\"");


                        if (Input[Form_Counter, Item_Counter] != "Error")
                        {
                            textBox3.Text = Input[Form_Counter, Item_Counter];

                            //listBox1.Items.AddRange(new object[]{ Form_Counter, Item_Counter });
                            //listBox1.Items.Add(item);
                            listBox1.Items.Add(Form_Action[2] + " || "+ Input[Form_Counter, Item_Counter]+" || "+ Default_Input[Item_Counter]);
                            

                            Item_Counter++;
                        }
                    }
                }
            }
        }







        string Get_Item_Content(string Main_Content, string Start_String, string End_String)
        {
            try
            {
                string Return_String = "";

                int Start_String_Internal = Main_Content.IndexOf(Start_String) + Start_String.Length;
                int End_String_Internal = Main_Content.IndexOf(End_String, Start_String_Internal);
                Return_String = Main_Content.Substring(Start_String_Internal, End_String_Internal - Start_String_Internal);




                return Return_String;
            }
            catch {

                return "Error";
            }
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                
            



        }

        private void button1_Click(object sender, EventArgs e)
        {
            Send_Post_Data();



        }



        private void Send_Post_Data()
        {

            //setup some variables

            //String input_name = ;
            //String input_value = ;


            //setup some variables end

            String result = "";
            String strPost = "";
            strPost = strPost + textBox2.Text + "=" + textBox3.Text; //+ "&password=" + password + "&firstname=" + firstname + "&lastname=" + lastname;
            strPost = strPost + "&" + textBox5.Text + "=" + textBox4.Text;
            strPost = strPost + "&" + textBox7.Text + "=" + textBox6.Text;
            strPost = strPost + "&" + textBox9.Text + "=" + textBox8.Text;




            StreamWriter myWriter = null;

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(textBox12.Text);
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch
            {
                //return e.Message;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr =
               new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();

                // Close and clean up the StreamReader
                sr.Close();
            }
            //return result;

            //webBrowser1.DocumentText = result;

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt16(numericUpDown1.Value);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Send_Post_Data();
            }
            catch { }

            if (pictureBox1.BackColor != Color.Yellow)
            {
                pictureBox1.BackColor = Color.Yellow;
            }
            else
            {
                pictureBox1.BackColor = Color.White;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
                button4.BackColor = Color.Green;
            }
            else
            {
                timer1.Stop();
                button4.BackColor = Color.Red;
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

    }
}

    

