using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Translator;


namespace stellaris_mod_manager
{
    public partial class Form1 : Form
    {
        public static string name_file, name_fileM, url1, name1, path1, lang;
        public static bool wait = false;
        public static string[] filer, Papka, Papfl = new string[0];
        public static int raz;
        YandexTranslator yt;
        public Form1()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://steamworkshop.download");
            label3.Text = "Версия программы: " + Properties.Settings.Default.Version;
            if (Properties.Settings.Default.Folder == "")
            {
                Form2 form2 = new Form2();
                form2.ShowDialog();
            }
            yt = new YandexTranslator();
            lang = "en-ru";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            pictureBox1.Enabled = false;
            progressBar1.Maximum = 100;
            name_file = null;
            label3.Text = "Запущен";
            backgroundWorker1.RunWorkerAsync();

        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker1.ReportProgress(10, "Start");
            Thread.Sleep(2000);
            backgroundWorker1.ReportProgress(30);
            Thread.Sleep(4000);
            backgroundWorker1.ReportProgress(50);

            while (wait != true)
            {
                Thread.Sleep(100);
            }
            wait = false;
            Thread.Sleep(2000);
            backgroundWorker1.ReportProgress(80);
            while (wait != true)
            {
                Thread.Sleep(100);
            }
            wait = false;
            backgroundWorker1.ReportProgress(90);
            Thread.Sleep(2000);
            backgroundWorker1.ReportProgress(100);
        }
        public void Start_Search()
        {
            if (Properties.Settings.Default.Folder != null && textBox2.Text != null)
            {
                label3.Text = "Веду поиск";
                HtmlDocument doc = webBrowser1.Document;
                HtmlElement workshop = doc.GetElementById("workshop");
                HtmlElementCollection cjlection = workshop.GetElementsByTagName("input");
                foreach (HtmlElement f in cjlection)
                    if (f.Name == "url")
                        f.InnerText = textBox2.Text;
                foreach (HtmlElement f in cjlection)
                    if (f.GetAttribute("type") == "submit")
                        f.InvokeMember("click");

            }
            else
            {
                backgroundWorker1.CancelAsync();
                label3.Text = "Ошибка! Заполните поля!";
            }
        }

        public void DWLK()
        {
            try
            {

                HtmlDocument doc = webBrowser1.Document;
                HtmlElement res = doc.GetElementById("result");
                HtmlElementCollection cjlection = res.GetElementsByTagName("a");
                foreach (HtmlElement f in cjlection)
                    if (f.GetAttribute("href") != null)
                    {
                        int pos = f.InnerText.LastIndexOf('.');
                        name_file = f.InnerText.Substring(0, pos);
                        DownloadFile(f.GetAttribute("href"), Properties.Settings.Default.Folder, f.InnerText, name_file);
                    }
                label3.Text = "Скачиваю";
            }
            catch
            {
                label3.Text = "Ошибка! Этот файл нельзя скачать :(";
                backgroundWorker1.CancelAsync();
            }
        }
        public void DW()
        {
            try
            {
                HtmlDocument doc = webBrowser1.Document;
                HtmlElement dwlk = doc.GetElementById("steamdownload");
                dwlk.InvokeMember("click");
                label3.Text = "Получаю ссылку";
            }
            catch
            {
                label3.Text = "Ошибка! Такого файла нет :(";
                backgroundWorker1.CancelAsync();
            }
        }
        public void CF(string path)
        {
            try
            {
                label3.Text = "Создаю .mod";
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("name=\"" + name_fileM + "\"");
                    fs.Write(info, 0, info.Length);
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("");
                    sw.WriteLine("tags={");
                    sw.WriteLine("\"Graphics\"");
                    sw.WriteLine("}");
                    sw.WriteLine("supported_version=\"2.6.*\"");
                    sw.WriteLine("path=\"" + Properties.Settings.Default.Folder + @"\" + name_fileM + "\"");
                    sw.WriteLine("remote_file_id=\"" + name_file + "\"");
                }
                label3.Text = "Успешно!";
            }
            catch
            {
                File.Delete(path);
                label3.Text = "Создаю .mod";
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("name=\"" + name_fileM + "\"");
                    fs.Write(info, 0, info.Length);
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("");
                    sw.WriteLine("tags={");
                    sw.WriteLine("\"Graphics\"");
                    sw.WriteLine("}");
                    sw.WriteLine("supported_version=\"2.6.*\"");
                    sw.WriteLine("path=\"" + Properties.Settings.Default.Folder + @"\" + name_fileM + "\"");
                    sw.WriteLine("remote_file_id=\"" + name_file + "\"");
                }
                label3.Text = "Успешно!";
            }

        }
        public void DLT()
        {
            label3.Text = "Удаляю временные файлы";
            Directory.Delete(Properties.Settings.Default.Folder + @"\Temp", true);
            label3.Text = "Файл " + name_fileM + " успешно скачан!";
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 10)
            {
                Start_Search();
                progressBar1.Value = 10;
            }
            if (e.ProgressPercentage == 30)
            {
                DW();
                progressBar1.Value = 30;
            }
            if (e.ProgressPercentage == 50)
            {
                DWLK();
                progressBar1.Value = 50;
            }
            if (e.ProgressPercentage == 80)
            {
                ReadM(Properties.Settings.Default.Folder + @"\" + name_file);
                progressBar1.Value = 80;
            }
            if (e.ProgressPercentage == 90)
            {
                CF(Properties.Settings.Default.Folder + @"\" + name_fileM + ".mod");
                progressBar1.Value = 90;
            }
            if (e.ProgressPercentage == 100)
            {
                DLT();
                progressBar1.Value = 100;
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                pictureBox1.Enabled = true;
            }
        }
        public static void MyMethod()
        {

        }
        public void DownloadFile(string url, string path, string name, string name_file)
        {
            Directory.CreateDirectory(path + @"\Temp");
            url1 = url;
            name1 = name;
            path1 = path;



            Uri uri = new Uri(url);
            webClient1.DownloadFileAsync(uri, path + @"\Temp\" + name);
        }
        public static async void ZipS(string path_name, string path, string name_file)
        {
            await Task.Run(() => Zip(path_name, path, name_file));
        }
        public static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.

        }
        public static void Zip(string zipPath, string extractPath, string name_file)
        {
            try
            {
                Directory.Delete(extractPath + @"\" + name_file, true);
            }
            catch
            {

            }
            try
            {
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                wait = true;
            }
            catch
            {
                Form1 form1 = new Form1();
                form1.StopW("Не удалось открыть zip");
            }
        }

        private void WebClient1_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            label3.Text = "Файл успешно скачан! Перехожу к распаковке";
            ZipS(path1 + @"\Temp\" + name1, path1, name_file);
        }

        private void WebClient1_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            label4.Text = (string)e.UserState + "    downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive + " bytes. " + e.ProgressPercentage + " % complete...";
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Key == "")
            {
                Form2 form2 = new Form2();
                form2.ShowDialog();
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                pictureBox1.Enabled = false;
                Thread MyThread1 = new Thread(delegate () { poisk_papka(Properties.Settings.Default.Folder + @"\" + textBox2.Text); });
                MyThread1.Start();


            }
        }

        public async void ReaD(string path2, string name_file1)
        {
            int i = 0;
            using (StreamReader sr = new StreamReader(path2 + @"\" + name_file1, System.Text.Encoding.UTF8))
            {
                label3.Invoke(new Action(() => { label3.Text = "Перевожу " + name_file1; }));
                string line;
                Array.Resize(ref filer, 0);
                while ((line = sr.ReadLine()) != null)
                {
                    Array.Resize(ref filer, filer.Length + 1);
                    filer[i] = line;
                    i++;
                    label4.Invoke(new Action(() => { label4.Text = "Читаю строчку №" + i; }));
                }
                raz = i;
                sr.Close();
            }
            Thread MyThread1 = new Thread(delegate () { Tran(path2, name_file1); });
            MyThread1.Start();
            MyThread1.Join();
        }
        public void poisk_papka(string link)
        {
            string papka_link = link;
            bool loc = false;
            bool rus = false;

            try
            {
                System.IO.DirectoryInfo info_papka = new System.IO.DirectoryInfo(papka_link);
                System.IO.DirectoryInfo[] papka1 = info_papka.GetDirectories();

                foreach (var dir_papka in papka1)
                {
                    if (dir_papka.Name == "localisation")
                    {
                        loc = true;
                        //poisk_file(dir_papka.FullName);

                        System.IO.DirectoryInfo info_papka1 = new System.IO.DirectoryInfo(dir_papka.FullName);

                        System.IO.DirectoryInfo[] papka2 = info_papka1.GetDirectories();
                        try
                        {

                            foreach (var dir_papka1 in papka2)
                            {
                                if (dir_papka1.Name == "russian")
                                {
                                    rus = true;
                                    path1 = dir_papka1.FullName;
                                }

                            }

                        }

                        catch { }
                    }
                }
            }
            catch { }
            if (loc)
            {
                if (rus)
                {

                    try
                    {
                        System.IO.DirectoryInfo info_file = new System.IO.DirectoryInfo(path1);
                        System.IO.FileInfo[] file1 = info_file.GetFiles();
                        int i = 0;
                        Array.Resize(ref Papfl, 0);
                        foreach (var dir_file in file1)
                        {
                            Array.Resize(ref Papfl, Papfl.Length + 1);
                            Papfl[i] = dir_file.Name; // имя файла
                            i++;

                        }

                    }
                    catch { }
                    progressBar1.Invoke(new Action(() => { progressBar1.Maximum = Papfl.Length - 1; }));
                    for (int i = 0; i < Papfl.Length; i++)
                    {
                        name_file = Papfl[i];

                        Thread MyThread1 = new Thread(delegate () { ReaD(path1, name_file); });
                        MyThread1.Start();
                        MyThread1.Join();

                        progressBar1.Invoke(new Action(() => { progressBar1.Value = i; }));

                    }
                    label3.Invoke(new Action(() => { label3.Text = "Все переведено!"; }));
                    progressBar1.Invoke(new Action(() => { progressBar1.Value = 0; }));
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    pictureBox1.Enabled = true;
                }
                else
                {

                }
            }
            else
            {
                Form1 form1 = new Form1();
                form1.label3.Text = "Нечего переводить!";
            }
        }
        public delegate void InvokeDelegate();
        public void Prog(int r, int pr)
        {
            progressBar1.Maximum = r;

        }

        static void poisk_file(string file)
        {

        }
        public async void Tran(string path2, string name_file1)
        {

            for (int i = 1; i < raz; i++)
            {
                string[] words = filer[i].Split(new char[] { '"' });
                string first = words[0];
                try
                {
                    string second = words[1];
                    filer[i] = words[0] + "\"" + yt.Translate(words[1], lang, Properties.Settings.Default.Key) + "\"";
                }
                catch
                {
                }
                label4.Invoke(new Action(() => { label4.Text = "Перевожу строку №" + i; }));
            }
            Thread MyThread1 = new Thread(delegate () { WriD(path2, name_file1); });
            MyThread1.Start();
            MyThread1.Join();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

        }

        private void BackgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 10)
            {
                //ZipS();
                progressBar1.Value = 10;
            }
        }

        public async void WriD(string path2, string name_file1)
        {
            File.Delete(path2 + @"\" + name_file1);
            using (FileStream fs = File.Create(path2 + @"\" + name_file1))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("l_russian:");
                fs.Write(info, 0, info.Length);
            }
            using (StreamWriter sw = File.AppendText(path2 + @"\" + name_file1))
            {
                for (int i = 1; i < raz; i++)
                {
                    sw.WriteLine(filer[i]);
                    label4.Invoke(new Action(() => { label4.Text = "Записываю строку №" + i; }));
                }
                sw.Close();
            }

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        public void StopW(string ms)
        {
            label3.Text = ms;
            backgroundWorker1.CancelAsync();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            name_file = textBox2.Text;
            name_fileM = name_file;
            CF(Properties.Settings.Default.Folder + @"\" + name_file + ".mod");
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = 0;
        }
        public void ReadM(string path2)
        {
            wait = false;
            int i = 0;
            try
            {
                using (StreamReader sr = new StreamReader(path2 + @"\descriptor.mod", System.Text.Encoding.UTF8))
                {
                    label3.Text = "Узнаю название";
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] words = line.Split(new char[] { '=' });
                        string first = words[0];
                        if (first == "name")
                        {
                            string[] words1 = line.Split(new char[] { '"' });
                            name_fileM = words1[1];
                            break;
                        }
                        i++;
                        label4.Text = "Читаю строчку №" + i;
                    }
                    sr.Close();
                }
            }
            catch
            {
                name_fileM = name_file;
            }
            Directory.Move(path2, Properties.Settings.Default.Folder + @"\" + name_fileM);
            wait = true;

        }
    }
}
