using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace Notes
{
    public partial  class FormMain : Form
    {
        ToDoItemFile itemFile = new ToDoItemFile();
        ToDoItemOperation doItem = new ToDoItemOperation();
        Item item = new Item();
        public int _atCount;
        public int intNum, intAlarm, intEndTime, intDetail, intFinish, intid;
        public int intAddAlarm, intAddDetail, intAddTime;
        public int index;
        public FormMain()
        {
            InitializeComponent();
            dataGridView2.Rows[0].Cells[3].Value = "新建";
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                switch (dataGridView1.Columns[i].Name)
                { 
                    case "Num":
                        intNum = i;
                        break;
                    case "Alarm":
                        intAlarm = i;
                        break;
                    case "Detail":
                        intDetail = i;
                        break;
                    case "EndTime":
                        intEndTime = i;
                        break;
                    case "Finish":
                        intFinish = i;
                        break;
                    case "id":
                        intid = i;
                        break;
                }
            }

            for (int i = 0; i < dataGridView2.ColumnCount; i++)
            {
                switch (dataGridView2.Columns[i].Name)
                {
                    case "AddAlarm":
                        intAddAlarm = i;
                        break;
                    case "AddDetail":
                        intAddDetail = i;
                        break;
                    case "AddTime":
                        intAddTime = i;
                        break;
                }
            }
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            //获取当前工作区宽度和高度（工作区不包含状态栏）
            int ScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = 410;
            this.Height = 249;
            //计算窗体显示的坐标值，可以根据需要微调几个像素
            int x = ScreenWidth - this.Width - 5;
            int y = ScreenHeight - this.Height - 5;

            this.Location = (Point)new Size(x, y); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Item[] _items = doItem.SearchItem();
            _atCount = _items.Length;
            int j = 0;
            
            for (index = 0; index < _atCount; index++)
            {
                dataGridView1.Rows.Add();
               
                for (int i = j; i < _atCount; i++)
                {
                    
                    if (!_items[i]._isFinish)
                    {
                        dataGridView1.Rows[index].Cells[intDetail].Value = _items[i]._detail;
                        dataGridView1.Rows[index].Cells[intEndTime].Value = _items[i]._endTime;
                        dataGridView1.Rows[index].Cells[intFinish].Value = _items[i]._isFinish;
                        dataGridView1.Rows[index].Cells[intid].Value = _items[i]._id;
                        if (_items[i]._isAlarm == true)
                        {
                            dataGridView1.Rows[index].Cells[intAlarm].Value = imageList1.Images[0];
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[intAlarm].Value = null;
                        }
                        j++;
                        break;
                    }
                    j++;
                }
                if(j == _atCount)
                {
                    break;
                }
            }
            this.dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker1.ShowUpDown = true;
            dataGridView1.Columns[intAlarm].DefaultCellStyle.NullValue = null;
            dataGridView2.Columns[intNum].DefaultCellStyle.NullValue = null;
            dataGridView2.Rows[0].Cells[0].Value = null;
            if (_atCount < 9)
            {
                this.dataGridView1.Rows.Add(9 - _atCount);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             if (dataGridView2.Columns[e.ColumnIndex].Name == "xinjian")
             {
                 if (dataGridView2.Rows[0].Cells[intAddAlarm].Value == null)
                 {
                     item._isAlarm = false;
                 }
                 else
                 {
                     item._isAlarm = true;
                 }
                 try
                 {
                     item._detail = dataGridView2.Rows[0].Cells[intAddDetail].Value.ToString();
                     item._endTime = dateTimePicker1.Value;
                     item._isFinish = false;
                     item._id = _atCount;
                     doItem.AddItem(item);
                     dataGridView1.Rows.Clear();
                     this.Form1_Load(sender, e);
                 }
                 catch
                 {
                     MessageBox.Show("事项栏不能为空！！！");
                 }
              }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                dr.Cells[intNum].Value = dr.Index + 1;
                
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[intEndTime].Value != null)
                    {
                        DateTime time1 = Convert.ToDateTime(dataGridView1.Rows[i].Cells[intEndTime].Value.ToString());
                        DateTime time2 = Convert.ToDateTime(DateTime.Now.ToString());
                        TimeSpan ts = new TimeSpan();
                        TimeSpan tp = new TimeSpan();
                        ts = time1 - time2;
                        tp = time2 - time1;
                        if (!doItem.items[i]._isFinish)
                        {
                            if (tp.TotalMinutes > 0)
                            {
                                this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 153, 255);
                            }
                        }
                        else
                        {
                            if (tp.TotalMinutes > 0)
                            {
                                this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(146, 208, 80);
                            }

                        }
                        if (dataGridView1.Rows[i].Cells[intAlarm].Value != null)
                        {
                            if (ts.TotalSeconds == 900)
                            {
                                this.notifyIcon1.ShowBalloonTip(1000, "时间提醒", "事件：" + dataGridView1.Rows[i].Cells[intDetail].Value + ",距离开始还剩30分钟。", ToolTipIcon.Info);
                            }
                            if (ts.TotalSeconds == 0)
                            {
                                this.notifyIcon1.ShowBalloonTip(1000, "时间提醒", "事件：" + dataGridView1.Rows[i].Cells[intDetail].Value + ",时间到！！！。", ToolTipIcon.Info);
                            }
                        }
                    }
                }
            }
            catch
            { 
                
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Item valueChange = new Item();
            if (e.ColumnIndex == intFinish && e.RowIndex != -1)
            {
                valueChange = doItem.items[Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[intid].Value)];
                if (dataGridView1.Rows[e.RowIndex].Cells[intAlarm].Value != null)
                {
                    valueChange._isAlarm = true;
                }
                else
                {
                    valueChange._isAlarm = false;
                }
                
                //获取控件的值
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[e.RowIndex].Cells[intFinish];
                Boolean flag = Convert.ToBoolean(checkCell.Value);
                if (flag == true)
                {
                    this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(146, 208, 80);
                    dataGridView1.Rows[e.RowIndex].Cells[intDetail].Style.Font = new Font(dataGridView1.Font, FontStyle.Strikeout);
                    valueChange._isFinish = true;
                }
                else
                {
                    this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[intDetail].Style.Font = new Font(dataGridView1.Font, FontStyle.Regular);
                    valueChange._isFinish = false;  
                }
                doItem.ChangeItem(valueChange, valueChange._id);
            }
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            Boolean flag;
            if (e.RowIndex != -1)
            {
                //获取控件的值
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[e.RowIndex].Cells[intFinish];
                flag = Convert.ToBoolean(checkCell.Value);
                if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.White)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 193);
                }
                if (e.RowIndex > index)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 193);
                }
            }
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e) 
        {
            Boolean flag;
            if (e.RowIndex != -1)
            {
                //获取控件的值
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[e.RowIndex].Cells[intFinish];
                flag = Convert.ToBoolean(checkCell.Value);

                if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.FromArgb(255, 255, 193))
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex < _atCount)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[intAlarm].Value == null)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[intAlarm].Value = imageList1.Images[0];
                    Item[] _items = doItem.SearchItem();
                    _items[Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[intid].Value)]._isAlarm = true;
                    doItem.ChangeItem(_items[Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[intid].Value)], Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[intid].Value));
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value = null;
                    Item[] _items = doItem.SearchItem();
                    _items[Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[intid].Value)]._isAlarm = false;
                    doItem.ChangeItem(_items[Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[intid].Value)], Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[intid].Value));
                }
            }

            
        }
        //全选
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[intFinish].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[intFinish].Value = false;
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(this.Visible)
            {
                this.Hide();
            }
            else
            {
                this.Show();
            }
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataTable dt = this.dataGridView1.DataSource as DataTable;

                if (dataGridView1.Rows[e.RowIndex].Cells[intDetail].Value != null)
                {
                    string value = dataGridView1.Rows[e.RowIndex].Cells[intDetail].Value.ToString();

                    this.toolTip1.Show(value, this);
                }
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.ColumnIndex == 0)
            {
                if (dataGridView2.Rows[0].Cells[intAddAlarm].Value == null)
                {
                    dataGridView2.Rows[0].Cells[intAddAlarm].Value = imageList1.Images[0];      
                }
                else
                {
                    dataGridView2.Rows[0].Cells[intAddAlarm].Value = null;
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Item[] _items = doItem.SearchItem();
            Item valueChange = _items[Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[intid].Value)];
            try
            {
                valueChange._detail = dataGridView1.Rows[e.RowIndex].Cells[intDetail].Value.ToString();
            }
            catch
            {
                MessageBox.Show("事项栏不能为空！！！");
            }
            try
            {
                valueChange._endTime = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[intEndTime].Value);
            }
            catch
            {
                MessageBox.Show("请按xxxx/xx/xx x:xx:xx的格式正确输入时间！！！");
            }
            doItem.ChangeItem(valueChange, valueChange._id);
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
