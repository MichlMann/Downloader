using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;


namespace FileDownloader
{
    public partial class Form1 : Form
    {
        //0 = not processed
        //1 = processed and success
        //2 = processed and failure
        private ConcurrentDictionary<Tuple<long, long>, int> _tasks = new ConcurrentDictionary<Tuple<long, long>, int>();

        private ProgressPanel _progress = new ProgressPanel();
        private string _url = string.Empty;
        private string _fileName = string.Empty;
        private string _authorization = string.Empty;
        private CancellationTokenSource _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken(false));

        public Form1()
        {
            InitializeComponent();

            this.lblCPUCount.Text = Environment.ProcessorCount.ToString();
            this.numUpDownThreads.Value = Environment.ProcessorCount;

            _progress.Spring = true;
            _progress.BorderSides = ToolStripStatusLabelBorderSides.All;
            _progress.BorderStyle = Border3DStyle.SunkenOuter;
            _progress.Overflow = ToolStripItemOverflow.Never;
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Height = 30;
            this.statusStrip1.Items.Add((ToolStripItem)_progress);

            ToolStripStatusLabel byteStatus = new ToolStripStatusLabel("0 of 0");
            byteStatus.Name = "byteStatus";
            this.statusStrip1.Items.Add(byteStatus);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = 5000;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
        }

        #region Download

        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                btnRetry.Enabled = false;
                btnDownload.Enabled = false;

                if (this.grpBoxAuthentication.Enabled)
                    if (this.txtUser.Text.Trim().Length > 0 && this.txtPassword.Text.Trim().Length > 0)
                        _authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(txtUser.Text + ":" + txtPassword.Text));

                SetFormState(false);

                lblResult.Visible = false;
                lblResult.ForeColor = Color.Blue;
                lblResult.Text = "";

                _tasks.Clear();

                _progress.ClearSegments();
                this.statusStrip1.Items["byteStatus"].Text = "0 of 0";
                this.statusStrip1.Refresh();

                _url = txtURL.Text;
                _fileName = GetFileNameFromQueryString(new Uri(_url));
                if (string.IsNullOrEmpty(_fileName))
                    throw new Exception("'?file=yourfile.ext' was not specified in request query string.");

                btnCancel.Enabled = chkSplitFileIntoChunks.Checked;

                StartProgressIndicator();

                DownloadFile(txtURL.Text);
            }
            catch (Exception ex)
            {
                StopProgressIndicator();

                MessageBox.Show("Download Error:  " + ex.GetBaseException().Message);
                btnRetry.Enabled = false;
                btnDownload.Enabled = true;
                btnCancel.Enabled = false;

                SetFormState(true);

                _authorization = string.Empty;
            }
        }

        private void DownloadFile(string url)
        {
            //get chunk size
            long chunkSize = 0;
            if (chkSplitFileIntoChunks.Checked)
                chunkSize = (long)numUpDownChunkSize.Value;

            //get file length
            long fileLength = 0;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webRequest.Method = "HEAD";
            if (!string.IsNullOrEmpty(_authorization))
                webRequest.Headers.Add("authorization", "Basic " + _authorization);
            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                fileLength = webResponse.ContentLength;
            }
            _progress.TotalFileLength = fileLength;
            this.statusStrip1.Items["byteStatus"].Text = "0 of " + fileLength.ToString();
            this.statusStrip1.Refresh();

            saveFileDialog1.FileName = _fileName;
            //string fullPath = string.Empty;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _fileName = saveFileDialog1.FileName;
            }
            
            using (FileStream fs = new FileStream(_fileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                fs.SetLength(fileLength);
            }

            //transfer entire file
            if (chunkSize == 0)
            {
                Tuple<long,long> bytesRange = new Tuple<long,long>(0, fileLength);
                if (!_tasks.ContainsKey(bytesRange))
                    _tasks.TryAdd(bytesRange, 0);
                else
                    _tasks[bytesRange] = 0;

                ThreadPool.QueueUserWorkItem(o => StartProcessEntireFile(o), new Tuple<long, long>(0, fileLength));
            }
            else  //transfer file in chunks
                ThreadPool.QueueUserWorkItem(o => StartProcessChunks(o), new object[] { fileLength, chunkSize });
        }
        private void StartProcessChunks(object state)
        {
            try
            {
                object[] args = (object[])state;
                ParallelOptions options = new ParallelOptions();
                options.MaxDegreeOfParallelism = (int)numUpDownThreads.Value;
                options.CancellationToken = _cancellationTokenSource.Token;
                Parallel.ForEach(SplitFileIntoChunks((long)args[0], (long)args[1]), options, new Action<Tuple<long, long>>(ProcessChunks));

                Finished();
            }
            catch (OperationCanceledException)
            {
                Canceled();
            }
        }
        private IEnumerable<Tuple<long, long>> SplitFileIntoChunks(long fileLength, long chunkSize)
        {
            long index = 0;
            while (true)
            {
                if (index >= fileLength)
                    break;
                if (index < fileLength && index + chunkSize > fileLength)
                {
                    Tuple<long, long> bytesRange = new Tuple<long, long>(index, fileLength);
                    if (!_tasks.ContainsKey(bytesRange))
                        _tasks.TryAdd(bytesRange, 0);
                    else
                        _tasks[bytesRange] = 0;
                    yield return new Tuple<long, long>(index, fileLength);
                    break;
                }
                Tuple<long, long> bytesRange2 = new Tuple<long, long>(index, (index + chunkSize));
                if (!_tasks.ContainsKey(bytesRange2))
                    _tasks.TryAdd(bytesRange2, 0);
                else
                    _tasks[bytesRange2] = 0;
                yield return new Tuple<long, long>(index, (index + chunkSize));
                index += chunkSize;
            }
        }
        private void ProcessChunks(Tuple<long, long> bytesRange)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(_url);
                webRequest.AddRange(bytesRange.Item1, bytesRange.Item2);
                if (!string.IsNullOrEmpty(_authorization))
                    webRequest.Headers.Add("authorization", "Basic " + _authorization);

                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    string range = webResponse.GetResponseHeader("content-range");
                    string[] parts = range.Split(new char[] { ' ', '-', '/' });
                    long contentLength = long.Parse(parts[2]) - long.Parse(parts[1]);
                    long totalLength = long.Parse(parts[3]);

                    //const FileOptions FILE_NO_BUFFER = (FileOptions)0x20000000;
                    //using (FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 4096, FileOptions.WriteThrough | FILE_NO_BUFFER | FileOptions.Asynchronous))
                    using (FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 4096, true))
                    {
                        fs.Position = long.Parse(parts[1]);
                        using (Stream s = webResponse.GetResponseStream())
                        {
                            s.CopyTo(fs);
                        }
                    }
                }

                //processing successful
                _tasks[bytesRange] = 1;
                ChunkProcessed(bytesRange);
            }
            catch
            {
                //processing failed
                _tasks[bytesRange] = 2;
                ChunkFailed(bytesRange);
            }
        }

        private void StartProcessEntireFile(object state)
        {
            Tuple<long, long> bytesRange = (Tuple<long, long>)state;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(_url);
                if (!string.IsNullOrEmpty(_authorization))
                    webRequest.Headers.Add("authorization", "Basic " + _authorization);

                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 4096, true))
                    {
                        fs.Position = 0;
                        using (Stream s = webResponse.GetResponseStream())
                        {
                            s.CopyTo(fs);
                        }
                    }
                }

                //processing successful
                _tasks[bytesRange] = 1;
                ChunkProcessed(bytesRange);
            }
            catch
            {
                //processing failed
                _tasks[bytesRange] = 2;
                ChunkFailed(bytesRange);
            }
            finally
            {
                Finished();
            }
        }

        #endregion

        #region Retry

        private void btnRetry_Click(object sender, EventArgs e)
        {
            try
            {
                btnDownload.Enabled = false;
                btnRetry.Enabled = false;
                btnCancel.Enabled = true;

                if (this.grpBoxAuthentication.Enabled)
                    if (this.txtUser.Text.Trim().Length > 0 && this.txtPassword.Text.Trim().Length > 0)
                        _authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(txtUser.Text + ":" + txtPassword.Text));

                SetFormState(false);

                lblResult.Visible = false;
                lblResult.ForeColor = Color.Blue;
                lblResult.Text = "";

                _url = txtURL.Text;
                _fileName = GetFileNameFromQueryString(new Uri(_url));
                if (string.IsNullOrEmpty(_fileName))
                    throw new Exception("'?file=yourfile.ext' was not specified in request query string.");

                btnCancel.Enabled = chkSplitFileIntoChunks.Checked;

                StartProgressIndicator();

                DownloadFileRetry(txtURL.Text);
            }
            catch (Exception ex)
            {
                StopProgressIndicator();

                MessageBox.Show("Retry Error:  " + ex.GetBaseException().Message);
                btnRetry.Enabled = true;
                btnDownload.Enabled = true;
                btnCancel.Enabled = false;

                SetFormState(true);

                _authorization = string.Empty;
            }
        }
        private void DownloadFileRetry(string url)
        {
            long chunkSize = 0;
            if (chkSplitFileIntoChunks.Checked)
                chunkSize = (long)numUpDownChunkSize.Value;

            if (chunkSize == 0)
            {
                long fileLength = (long)_tasks.Keys.Sum<Tuple<long, long>>((e) => { return (e.Item2 - e.Item1); });
                ThreadPool.QueueUserWorkItem(o => StartProcessEntireFile(o), new Tuple<long, long>(0, fileLength));
            }
            else
                ThreadPool.QueueUserWorkItem(o => StartProcessRetry(o));
        }
        private void StartProcessRetry(object state)
        {
            try
            {
                ParallelOptions options = new ParallelOptions();
                options.MaxDegreeOfParallelism = (int)numUpDownThreads.Value;
                Parallel.ForEach(FindRetryChunks(), options, new Action<Tuple<long, long>>(ProcessChunks));

                Finished();
            }
            catch (OperationCanceledException)
            {
                Canceled();
            }
        }
        private IEnumerable<Tuple<long, long>> FindRetryChunks()
        {
            foreach (Tuple<long, long> bytesRange in _tasks.Keys)
            {
                if (_tasks[bytesRange] == 2)  //processed but failed
                {
                    if (!_tasks.ContainsKey(bytesRange))
                        _tasks.TryAdd(bytesRange, 0);
                    else
                        _tasks[bytesRange] = 0;
                    yield return bytesRange;
                }
            }
        }

        #endregion

        #region Cancel

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null && _cancellationTokenSource.Token != null && _cancellationTokenSource.Token.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel();

                StartCancelationIndicator();
            }
        }

        #endregion

        private void ChunkProcessed(Tuple<long, long> bytesRangeOfChunk)
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke((Action)(() => ChunkProcessed(bytesRangeOfChunk)));
                else
                {
                    _progress.AddSegment(bytesRangeOfChunk, true);
                    long segments = _progress.GetSumSegments();
                    long totalFileLength  = _progress.TotalFileLength;
                    int percentage = Convert.ToInt16(Convert.ToDecimal(segments) / Convert.ToDecimal(totalFileLength) * Convert.ToDecimal(100)) ;
                    this.statusStrip1.Items["byteStatus"].Text = segments.ToString() + " of " + totalFileLength.ToString() + " thats " + percentage + "%";
                    this.statusStrip1.Refresh();
                }
            }
        }

        private void ChunkFailed(Tuple<long, long> bytesRangeOfChunk)
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke((Action)(() => ChunkFailed(bytesRangeOfChunk)));
                else
                {
                    _progress.UpdateSegment(bytesRangeOfChunk, false);
                    this.statusStrip1.Items["byteStatus"].Text = _progress.GetSumSegments().ToString() + " of " + _progress.TotalFileLength.ToString();
                    this.statusStrip1.Refresh();
                }
            }
        }

        private void Finished()
        {
            StopProgressIndicator();

            _authorization = string.Empty;

            if (this.IsHandleCreated && !this.IsDisposed)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke((Action)(() => Finished()));
                else
                {
                    if (_progress.GetSumSegments() == _progress.TotalFileLength)
                    {
                        lblResult.Visible = true;
                        lblResult.ForeColor = Color.Blue;
                        lblResult.Text = "Success!";

                        btnRetry.Enabled = false;
                        btnDownload.Enabled = true;
                        btnCancel.Enabled = false;

                        SetFormState(true);
                    }
                    else
                    {
                        lblResult.Visible = true;
                        lblResult.ForeColor = Color.Red;
                        lblResult.Text = "Failed!";

                        btnRetry.Enabled = true;
                        btnDownload.Enabled = true;
                        btnCancel.Enabled = false;

                        SetFormState(true);
                    }
                }
            }
        }

        private void Canceled()
        {
            StopProgressIndicator();

            StopCancelationIndicator();

            _authorization = string.Empty;

            if (this.IsHandleCreated && !this.IsDisposed)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke((Action)(() => Canceled()));
                else
                {
                    btnRetry.Enabled = false;
                    btnDownload.Enabled = true;
                    btnCancel.Enabled = false;

                    SetFormState(true);

                    lblResult.Visible = true;
                    lblResult.ForeColor = Color.Red;
                    lblResult.Text = "Canceled!";

                    _tasks.Clear();

                    _progress.ClearSegments();
                    this.statusStrip1.Items["byteStatus"].Text = "0 of 0";
                    this.statusStrip1.Refresh();

                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                    _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken(false));
                }
            }
        }

        private string GetFileNameFromQueryString(Uri uri)
        {
            string fileName = string.Empty;
            /*string query = uri.Query;
            query = query.Replace("?", "");
            string[] queryParts = query.Split('&');
            foreach (string queryPart in queryParts)
            {
                if (queryPart.StartsWith("file="))
                {
                    string[] querySubParts = queryPart.Split('=', '\\', '/');
                    fileName = querySubParts[querySubParts.GetLength(0) - 1];
                    break;
                }
            }
            fileName = fileName.Replace("%20", "_");
            */
            string path = uri.AbsolutePath;// = "/debian-cd/current/amd64/iso-dvd/debian-8.8.0-amd64-DVD-1.iso"
            int i = path.LastIndexOf("/");

            fileName = path.Substring(i+1, path.Length - i-1);

            return fileName;
        }

        private void SetFormState(bool enabled)
        {
            if (enabled)
            {
                chkSplitFileIntoChunks.Enabled = enabled;
                chkBasicAuthorization.Enabled = enabled;
                grpBoxChunks.Enabled = chkSplitFileIntoChunks.Checked;
                grpBoxAuthentication.Enabled = chkBasicAuthorization.Checked;
            }
            else
            {
                chkSplitFileIntoChunks.Enabled = enabled;
                chkBasicAuthorization.Enabled = enabled;
                grpBoxChunks.Enabled = enabled;
                grpBoxAuthentication.Enabled = enabled;
            }
        }

        private void txtURL_TextChanged(object sender, EventArgs e)
        {
            btnDownload.Enabled = txtURL.Text.Trim().Length > 0;
        }

        private void chkChunkSize_CheckedChanged(object sender, EventArgs e)
        {
            grpBoxChunks.Enabled = chkSplitFileIntoChunks.Checked;
        }

        private void chkBasicAuthorization_CheckedChanged(object sender, EventArgs e)
        {
            grpBoxAuthentication.Enabled = chkBasicAuthorization.Checked;
        }

        
        
        #region ProgressPanel

        private class ProgressPanel : ToolStripStatusLabel
        {
            private Dictionary<Tuple<long, long>, bool> _segments = new Dictionary<Tuple<long, long>, bool>();
            private long _totalLength = 0;
            private string _text = string.Empty;

            
            public void ClearSegments()
            {
                _segments.Clear();
            }

            public void AddSegment(Tuple<long, long> segment, bool success)
            {
                if (!_segments.ContainsKey(segment))
                    _segments.Add(segment, success);
                else
                    _segments[segment] = success;
            }

            public void UpdateSegment(Tuple<long, long> segment, bool success)
            {
                if (!_segments.ContainsKey(segment))
                    _segments.Add(segment, success);
                else
                    _segments[segment] = success;
            }

            public void RemoveSegment(Tuple<long, long> segment)
            {
                _segments.Remove(segment);
            }

            public string Text
            {
                get { return _text; }
                set { _text = value; }
            }

            public long TotalFileLength
            {
                get { return _totalLength; }
                set { _totalLength = value; }
            }

            public long GetSumSegments()
            {
                long sum = 0;
                foreach (Tuple<long, long> key in _segments.Keys)
                {
                    if (_segments[key] == true)  //only sum successful
                        sum += key.Item2 - key.Item1;
                }
                return sum;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.TranslateTransform(0f, 1f);

                base.OnPaint(e);

                foreach (Tuple<long, long> segment in _segments.Keys)
                {
                    long begin = segment.Item1;
                    long end = segment.Item2;

                    double beginPercent = (double)begin / (double)_totalLength;
                    double endPercent = (double)end / (double)_totalLength;

                    double left = ((double)this.Width - 4d) * beginPercent;
                    double right = ((double)this.Width - 4d) * endPercent;

                    RectangleF rect = new RectangleF((float)left + 1f, 1f, (float)right - (float)left, (float)this.Height - 4f);

                    
                    if (_segments[segment] == true)
                    {
                        using (LinearGradientBrush b = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Vertical))
                        {
                            b.SetBlendTriangularShape(0.5f, 0.65f);
                            e.Graphics.FillRectangle(b, rect);
                        }
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.Red, rect);

                        using (Pen pen = new Pen(Color.FromArgb(30, Color.Black), 3f))
                        {
                            SmoothingMode mode = e.Graphics.SmoothingMode;
                            try
                            {
                                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                e.Graphics.DrawLine(pen, new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Bottom));
                                e.Graphics.DrawLine(pen, new PointF(rect.Left, rect.Bottom), new PointF(rect.Right, rect.Top));
                            }
                            finally
                            {
                                e.Graphics.SmoothingMode = mode;
                            }
                        }
                    }

                    using (Pen pen = new Pen(Color.White))
                    {
                        e.Graphics.DrawLine(pen, new PointF(rect.Left, rect.Top), new PointF(rect.Left, rect.Bottom));
                        e.Graphics.DrawLine(pen, new PointF(rect.Right, rect.Top), new PointF(rect.Right, rect.Bottom));
                    }
                }

                if (!string.IsNullOrEmpty(_text))
                {
                    SizeF size = e.Graphics.MeasureString(_text, this.Font);
                    float x = ((float)this.Width / 2.0f) - (size.Width / 2.0f);
                    float y = ((float)this.Height / 2.0f) - (size.Height / 2.0f);
                    PointF pt = new PointF(x, y);
                    using (Brush b = new SolidBrush(Color.Red))
                    {
                        e.Graphics.DrawString(_text, this.Font, b, pt);
                    }
                }
            }
        }

        #endregion

        
        #region ProgressIndicator

        private bool _cancelProgressIndicator = true;
        private void StopProgressIndicator()
        {
            _cancelProgressIndicator = true;
        }
        private void StartProgressIndicator()
        {
            _cancelProgressIndicator = false;
            ThreadPool.QueueUserWorkItem(o => AnimateProgressIndicator(o));
        }
        private void AnimateProgressIndicator(object state)
        {
            bool? toggle = true;
            while (!_cancelProgressIndicator)
            {
                UpdateProgressIndicator(toggle);
                Thread.Sleep(500);
                toggle = !toggle;
            }
            UpdateProgressIndicator(null);
        }
        private void UpdateProgressIndicator(bool? toggle)
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke((Action)(() => UpdateProgressIndicator(toggle)));
                else
                {
                    if (toggle.HasValue)
                    {
                        if (toggle.Value)
                            this.Text = "File Downloader  \\";
                        else
                            this.Text = "File Downloader  /";
                    }
                    else
                        this.Text = "File Downloader";
                }
            }
        }

        #endregion

        
        #region CancelationIndicator

        private bool _cancelCancelationIndicator = true;
        private void StopCancelationIndicator()
        {
            _cancelCancelationIndicator = true;
        }
        private void StartCancelationIndicator()
        {
            _cancelCancelationIndicator = false;
            ThreadPool.QueueUserWorkItem(o => AnimateCancelationIndicator(o));
        }
        private void AnimateCancelationIndicator(object state)
        {
            bool? toggle = true;
            while (!_cancelCancelationIndicator)
            {
                UpdateCancelationIndicator(toggle);
                Thread.Sleep(500);
                toggle = !toggle;
            }
            UpdateCancelationIndicator(null);
        }
        private void UpdateCancelationIndicator(bool? toggle)
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke((Action)(() => UpdateCancelationIndicator(toggle)));
                else
                {
                    if (toggle.HasValue)
                    {
                        if (toggle.Value)
                        {
                            _progress.Text = "Canceling...";
                            this.statusStrip1.Refresh();
                        }
                        else
                        {
                            _progress.Text = "";
                            this.statusStrip1.Refresh();
                        }
                    }
                    else
                    {
                        _progress.Text = "";
                        this.statusStrip1.Refresh();
                    }
                }
            }
        }

        #endregion

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

    
    }
}
