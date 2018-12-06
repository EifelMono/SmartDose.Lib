using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rowa.Lib.Log;
using Rowa.Lib.Log.Extensions;

namespace RowaLogFileBehaviorTester
{
    public partial class Form1 : Form
    {
        List<FileStream> _files = new List<FileStream>();
        CancellationTokenSource _logToken = null;
        BindingList<LogErrorInfo> _errors = new BindingList<LogErrorInfo>();
        List<LogWorker> _workers = new List<LogWorker>();
        DateTime _lastUIUpdate = DateTime.MinValue;
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1);

        public Form1()
        {
            InitializeComponent();
        }

        private List<string> GetLogFiles()
        {
            var logPath = @"C:\ProgramData\Rowa\Protocol";

            return new List<string>(Directory.GetFiles(logPath, "*.log", SearchOption.AllDirectories));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var logFiles = GetLogFiles();

            foreach (var logFile in logFiles)
            {
                _files.Add(new FileStream(logFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None));
            }

            setupTestButtons();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var fileStream in _files)
            {
                fileStream.Close();
            }
            
            _files.Clear();

            setupTestButtons();
        }

        public void OnNewError(LogErrorEventArgs args)
        {
            _mutex.Wait();
            var errorType = args.Error.ToString();

            var currentError = _errors.FirstOrDefault(x => x.ErrorType.ToString() == errorType);

            bool isAdd = currentError == null;


            var addUpdate = new Action(() =>
            {
                if (currentError == null)
                {
                    currentError = new LogErrorInfo
                    {
                        ErrorType = errorType
                    };

                    _errors.Add(currentError);
                }

                currentError.Count++;
                _errors.ResetItem(_errors.IndexOf(currentError));

                _mutex.Release();
            });



            if (InvokeRequired && ((DateTime.Now - _lastUIUpdate).TotalMilliseconds > 1000 || isAdd))
            {
                this.BeginInvoke(new Action(() =>
                {
                    addUpdate();
                }));

                _lastUIUpdate = DateTime.Now;
            }
            else
            {
                addUpdate();
            }
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LogManager.Initialize("RowaLog", "RowaLogBehaviorTester");
            LogManager.OnError += OnNewError;
            _errors.RaiseListChangedEvents = true;
            listBox1.DataSource = _errors;
            setupTestButtons();
        }


        private void setupTestButtons()
        {
            btStartLogLoop.Enabled = _logToken == null;
            btStopLogLoop.Enabled = _logToken != null;

            tbLogDelay.Enabled = btStartLogLoop.Enabled;
            tbWorkerCount.Enabled = btStartLogLoop.Enabled;


            btLockFiles.Enabled = btStartLogLoop.Enabled && _files.Count == 0;
            btFreeFiles.Enabled = _files.Count > 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogManager.Cleanup();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _logToken = new CancellationTokenSource();
            setupTestButtons();

            var workerCount = 1;
            var logDelay = 0;

            int.TryParse(tbWorkerCount.Text, out workerCount);
            int.TryParse(tbLogDelay.Text, out logDelay);

            for (int i = 0; i < workerCount; i++)
            {
                var worker = new LogWorker($"Hello, How are You? ({i})", logDelay, LogManager.GetLogger($"LogWorker{i}"));
                worker.Start();

                _workers.Add(worker);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            foreach (var worker in _workers)
            {
                worker.Stop();
            }

            _workers.Clear();

            _logToken = null;

            setupTestButtons();
        }
    }
}
