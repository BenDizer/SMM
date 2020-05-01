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
using System.Linq;


namespace stellaris_mod_manager
{
    public partial class Form1 : Form
    {
        public static string name_file, name_fileM, url1, name1, path1, lang;
        public static bool wait, wait1 = false;
        public static string[] filer, Papka, Papfl = new string[0];
        public static int raz;
        YandexTranslator yt;
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://steamworkshop.download");
            pause1();
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
            button4.Enabled = false;
            pictureBox1.Enabled = false;
            progressBar1.Maximum = 100;
            name_file = null;
            textBox1.AppendText("Запущен" + Environment.NewLine);
            backgroundWorker1.RunWorkerAsync();

        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
                if (backgroundWorker1.CancellationPending == true) { return; }


            backgroundWorker1.ReportProgress(10, "Start");
            Thread.Sleep(2000);
            if (backgroundWorker1.CancellationPending == true) { return; }
            backgroundWorker1.ReportProgress(30);
            Thread.Sleep(4000);
            if (backgroundWorker1.CancellationPending == true) { return; }
            backgroundWorker1.ReportProgress(50);

            while (wait != true)
            {
                Thread.Sleep(200);
                if (backgroundWorker1.CancellationPending == true) { return; }
            }
            wait = false;
            Thread.Sleep(2000);
            if (backgroundWorker1.CancellationPending == true) { return; }
            backgroundWorker1.ReportProgress(80);
            while (wait1 != true)
            {
                Thread.Sleep(200);
                if (backgroundWorker1.CancellationPending == true) { return; }
            }
            wait = false;
            Thread.Sleep(4000);
            backgroundWorker1.ReportProgress(90);
            Thread.Sleep(2000);
            if (backgroundWorker1.CancellationPending == true) { return; }
            backgroundWorker1.ReportProgress(100);
        }
        public void Start_Search()
        {
            if (Properties.Settings.Default.Folder != null && textBox2.Text != null)
            {
                textBox1.AppendText("Веду поиск" + Environment.NewLine);
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
                textBox1.AppendText("Ошибка! Заполните поля!" + Environment.NewLine);
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
                        textBox1.AppendText("Скачиваю" + Environment.NewLine);
                    }
                    else
                    {
                        textBox1.AppendText("Ошибка! Не могу получить ссылку! :(" + Environment.NewLine);
                        backgroundWorker1.CancelAsync();
                    }
                
            }
            catch
            {
                textBox1.AppendText("Ошибка! Этот файл нельзя скачать :(" + Environment.NewLine);
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
                textBox1.AppendText("Получаю ссылку" + Environment.NewLine);
            }
            catch
            {
                textBox1.AppendText("Ошибка! Такого файла нет :(" + Environment.NewLine);
                backgroundWorker1.CancelAsync();
            }
        }
        public void CF(string path)
        {
            try
            {
                textBox1.AppendText("Создаю .mod" + Environment.NewLine);
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
                    sw.Close();
                }
                textBox1.AppendText("Успешно!" + Environment.NewLine);
            }
            catch
            {
                File.Delete(path);
                textBox1.AppendText("Создаю .mod" + Environment.NewLine);
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
                    sw.Close();
                }
                textBox1.AppendText("Успешно!" + Environment.NewLine);
            }
        }
        public void DLT()
        {
            textBox1.AppendText("Удаляю временные файлы" + Environment.NewLine);
            try { Directory.Delete(Properties.Settings.Default.Folder + @"\Temp", true); } catch { }
            textBox1.AppendText("Файл " + name_fileM + " успешно скачан!" + Environment.NewLine);
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
            textBox1.AppendText("Файл успешно скачан! Перехожу к распаковке" + Environment.NewLine);
            ZipS(path1 + @"\Temp\" + name1, path1, name_file);
        }

        private void WebClient1_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double mbt = Convert.ToInt32(e.TotalBytesToReceive);
            string b = " бит";
            int del = 1;
            if (mbt / 8 > 1)
            {
                mbt = mbt / 8;
                b = " байт";
                del = 8;
                if (mbt / 1024 > 1)
                {
                    mbt = mbt / 1024;
                    b = " Кбайт";
                    del = 8192;
                    if (mbt / 1024 > 1)
                    {
                        mbt = mbt / 1024;
                        b = " Мбайт";
                        del = 8388608;
                    }
                }
            }
            mbt = Math.Round(mbt, 2);
            double mb = Convert.ToInt32(e.BytesReceived);
            mb = Math.Round(mb / del, 2);
            var lines = textBox1.Lines.ToList();
            lines.RemoveAt(lines.Count - 2);
            textBox1.Lines = lines.ToArray();
            textBox1.AppendText((string)e.UserState + " Скачано " + mb + " из " + mbt + b + e.ProgressPercentage + " % ..." + Environment.NewLine);
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
                button4.Enabled = false;
                pictureBox1.Enabled = false;
                folderBrowserDialog1.ShowDialog();
                if (folderBrowserDialog1.SelectedPath != "")
                {
                    Thread MyThread1 = new Thread(delegate () { poisk_papka(folderBrowserDialog1.SelectedPath); });
                    MyThread1.Start();
                }
                else { textBox1.AppendText("Папка с модом не выбрана!" + Environment.NewLine);
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    pictureBox1.Enabled = true;
                }

            }
        }

        public async void ReaD(string path2, string name_file1)
        {
            int i = 0;
            using (StreamReader sr = new StreamReader(path2 + @"\" + name_file1, System.Text.Encoding.UTF8))
            {
                label3.Invoke(new Action(() => { textBox1.AppendText("Перевожу " + name_file1 + Environment.NewLine); }));
                string line;
                Array.Resize(ref filer, 0);
                while ((line = sr.ReadLine()) != null)
                {
                    Array.Resize(ref filer, filer.Length + 1);
                    filer[i] = line;
                    i++;
                    textBox1.Invoke(new Action(() => { textBox1.AppendText("Читаю строчку №" + i + Environment.NewLine); }));
                    //label4.Invoke(new Action(() => { label4.Text = "Читаю строчку №" + i; }));
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
                    label3.Invoke(new Action(() => { textBox1.AppendText("Все переведено!" + Environment.NewLine); }));
                    progressBar1.Invoke(new Action(() => { progressBar1.Value = 0;
                        button1.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = true;
                        button4.Enabled = true;
                        pictureBox1.Enabled = true;
                    }));
                    
                }
                else
                {

                }
            }
            else
            {
                Form1 form1 = new Form1();
                form1.textBox1.AppendText("Нечего переводить!" + Environment.NewLine);
            }
        }
        public delegate void InvokeDelegate();
        public void Prog(int r, int pr)
        {
            progressBar1.Maximum = r;

        }

        public void Tran(string path2, string name_file1)
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
                textBox1.Invoke(new Action(() => { textBox1.AppendText("Перевожу строку № " + i + Environment.NewLine); }));
                //label4.Invoke(new Action(() => { label4.Text = ; }));
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
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            pictureBox1.Enabled = false;
            backgroundWorker3.RunWorkerAsync();

        }

        private void BackgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 10)
            {
                progressBar1.Value = 10;
                name_file = System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                textBox1.AppendText("Открываю" + Environment.NewLine);
                ZipS1(openFileDialog1.FileName, Properties.Settings.Default.Folder, name_file);
                progressBar1.Value = 10;
            }
            if (e.ProgressPercentage == 50)
            {
                ReadM(Properties.Settings.Default.Folder + @"\" + name_file);
                progressBar1.Value = 50;
            }
            if (e.ProgressPercentage == 90)
            {
                CF(Properties.Settings.Default.Folder + @"\" + name_fileM + ".mod");
                progressBar1.Value = 100;
            }
            if (e.ProgressPercentage == 100)
            {
                textBox1.AppendText("Успешно" + Environment.NewLine);
                progressBar1.Value = 0;
            }
        }

        private void BackgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker2.ReportProgress(10);

            while (wait != true)
            {
                Thread.Sleep(100);
            }
            wait = false;
            Thread.Sleep(2000);
            backgroundWorker2.ReportProgress(50);
            while (wait1 != true)
            {
                Thread.Sleep(100);
            }
            wait = false;
            Thread.Sleep(2000);
            backgroundWorker2.ReportProgress(90);
            Thread.Sleep(2000);
            backgroundWorker2.ReportProgress(100);

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
                    textBox1.Invoke(new Action(() => { textBox1.AppendText("Записываю строку № " + i + Environment.NewLine); }));
                    //textBox1.Invoke(new Action(() => { label4.Text =  + i; }));
                }
                sw.Close();
            }

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void BackgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] nf = new string[0];
            int i = 0;
            System.IO.DirectoryInfo info_file = new System.IO.DirectoryInfo(Properties.Settings.Default.Folder);
            System.IO.FileInfo[] file1 = info_file.GetFiles();
            foreach (var dir_file in file1)
            {
                Array.Resize(ref nf, nf.Length + 1);
                nf[i] = Path.GetFileNameWithoutExtension(dir_file.FullName);
                i++;
            }

            try
            {
                File.Delete(Properties.Settings.Default.Folder_Doc + @"\dlc_load.json");
            }
            catch
            { }
            using (FileStream fs = File.Create(Properties.Settings.Default.Folder_Doc + @"\dlc_load.json"))
            {
                //byte[] info = new UTF8Encoding(true).GetBytes("{");
                //fs.Write(info, 0, info.Length);
            }
            string text = "";
            string ver = "2.6.*";
            using (StreamWriter sw = File.AppendText(Properties.Settings.Default.Folder_Doc + @"\dlc_load.json"))
            {
                sw.WriteLine("{\"enabled_mods\":[");
                for (i = 0; i < nf.Length; i++)
                {

                    text = "\"mod/" + nf[i] + "\"";
                    if (i != nf.Length - 1)
                    {
                        text = text + ",";
                    }
                    sw.WriteLine(text);
                    textBox1.Invoke(new Action(() => { textBox1.AppendText(nf[i] + " включен" + Environment.NewLine); }));
                   
                }
                sw.WriteLine("],\"disabled_dlcs\":[]}");
                sw.Close();
            }
        }
        string GenRandomString(string Alphabet, int Length)
        {
            
            StringBuilder sb = new StringBuilder(Length - 1);
            int Position = 0;


            for (int i = 0; i < Length; i++)
            {
                Position = rnd.Next(0, Alphabet.Length - 1);
                sb.Append(Alphabet[Position]);
            }
            string f = sb.ToString();
            sb.Clear();
            return f;
            
        }
        private void BackgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void BackgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            pictureBox1.Enabled = true;
            progressBar1.Value = 0;
        }

        private void BackgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            pictureBox1.Enabled = true;
            progressBar1.Value = 0;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        public void StopW(string ms)
        {
            textBox1.AppendText(ms + Environment.NewLine);
            backgroundWorker1.CancelAsync();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            pictureBox1.Enabled = false;
            openFileDialog1.ShowDialog();
            textBox1.AppendText("Файл " + openFileDialog1.FileName + " выбран" + Environment.NewLine);
            backgroundWorker2.RunWorkerAsync();
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            pictureBox1.Enabled = true;
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
                    textBox1.AppendText("Узнаю название" + Environment.NewLine);
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
                        textBox1.Invoke(new Action(() => { textBox1.AppendText("Записываю строку № " + i + Environment.NewLine); }));
                        //label4.Text = "Читаю строчку №" + i;
                    }
                    sr.Close();
                }
            }
            catch
            {
                try
                {
                    System.IO.DirectoryInfo info_file = new System.IO.DirectoryInfo(path2);
                    System.IO.FileInfo[] file1 = info_file.GetFiles();
                    foreach (var dir_file in file1)
                    {
                       
                        try
                        {
                            ZipFile.ExtractToDirectory(dir_file.FullName, path2);
                        }
                        catch
                        {}
                        File.Delete(dir_file.FullName);
                    }
                    try
                    {
                        using (StreamReader sr = new StreamReader(path2 + @"\descriptor.mod", System.Text.Encoding.UTF8))
                        {
                            textBox1.AppendText("Узнаю название" + Environment.NewLine);
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
                                textBox1.AppendText("Читаю строчку №" + i + Environment.NewLine);
                                //label4.Text = "Читаю строчку №" + i;
                            }
                            sr.Close();
                            if (name_fileM == null)
                            {
                                File.Delete(path2 + @"\descriptor.mod");
                                try
                                {
                                    Directory.Delete(Properties.Settings.Default.Folder + @"\Temp", true);
                                }
                                catch
                                { }
                                try
                                {
                                    Directory.CreateDirectory(Properties.Settings.Default.Folder + @"\Temp");
                                    ZipFile.ExtractToDirectory(openFileDialog1.FileName, Properties.Settings.Default.Folder + @"\Temp");

                                }
                                catch
                                {}
                                try
                                {
                                    System.IO.DirectoryInfo info_file1 = new System.IO.DirectoryInfo(Properties.Settings.Default.Folder + @"\Temp");
                                    System.IO.FileInfo[] file2 = info_file1.GetFiles();
                                    foreach (var dir_file1 in file2)
                                    {

                                        try
                                        {
                                            if (Path.GetExtension(dir_file1.Name) == ".mod")
                                            {
                                                File.Move(dir_file1.FullName, path2 + @"\descriptor.mod");
                                            }
                                        }
                                        catch
                                        { }
                                    }
                                }
                                catch
                                { }
                                try
                                {
                                    Directory.Delete(Properties.Settings.Default.Folder + @"\Temp", true);
                                }
                                catch
                                { }
                                
                                try
                                {
                                    using (StreamReader sr1 = new StreamReader(path2 + @"\descriptor.mod", System.Text.Encoding.UTF8))
                                    {
                                        textBox1.AppendText("Узнаю название" + Environment.NewLine);
                                        
                                        while ((line = sr1.ReadLine()) != null)
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
                                            textBox1.AppendText("Читаю строчку №" + i + Environment.NewLine);
                                            //label4.Text = "Читаю строчку №" + i;
                                        }
                                        sr1.Close();
                                    }
                                }
                                catch
                                {}

                                if (name_fileM == null)
                                {
                                    name_fileM = name_file;
                                    CF(path2 + @"\descriptor.mod");
                                }
                            }
                        }
                    }
                    catch
                    {
                        
                    }
                }
                catch { }
                
                
            }
            try { Directory.Delete(Properties.Settings.Default.Folder + @"\" + name_fileM); }catch { }
            try {
            string patha = Properties.Settings.Default.Folder + @"\" + name_fileM;
                Directory.Move(path2, patha);}catch{ name_fileM = name_file;
                string patha = Properties.Settings.Default.Folder + @"\" + name_fileM;
                try { Directory.Move(path2, patha); } catch { }
            }
            wait1 = true;

        }
        public static async void ZipS1(string path_name, string path, string name_file)
        {
            await Task.Run(() => Zip1(path_name, path, name_file));
        }
       
        public static void Zip1(string zipPath, string extractPath, string name_file)
        {
            try
            {
                Directory.Delete(extractPath + @"\Temp", true);
            }
            catch
            {}
            try
            {
                
                Directory.CreateDirectory(extractPath + @"\Temp");
                ZipFile.ExtractToDirectory(zipPath, extractPath + @"\Temp");
                
            }
            catch
            {
                Form1 form1 = new Form1();
                form1.StopW("Не удалось открыть zip");
            }
            try
            {
                    System.IO.DirectoryInfo info_papka = new System.IO.DirectoryInfo(extractPath + @"\Temp");
                    System.IO.DirectoryInfo[] papka1 = info_papka.GetDirectories();

                    foreach (var dir_papka in papka1)
                    {
                        try
                        {
                            Directory.Move(dir_papka.FullName, extractPath + @"/" + dir_papka.Name);
                        }
                        catch
                        {
                            Directory.Delete(extractPath + @"/" + dir_papka.Name);
                            Directory.Move(dir_papka.FullName, extractPath + @"/" + dir_papka.Name);
                        }
                    Form1.name_file = dir_papka.Name;
                    }
                    
                     wait = true;
            }
            catch
            {}
            try
            {
                Directory.Delete(extractPath + @"\Temp", true);
            }
            catch
            {}
        }

        public static void Zip2(string zipPath, string extractPath, string name_file, string path2)
        {
            
        }

        public void pause1()
        {
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();

            }
        }
    }
}
