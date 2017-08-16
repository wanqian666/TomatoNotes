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
        public int intNum, intAlarm, intEndTime, intDetail, intFinish;
        public int intAddAlarm, intAddDetail, intAddTime;
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Position[] _position = doItem.SearchItem();
            
            Item[] _items = doItem.SearchItem();
            _atCount = _items.Length;
            //intCount = doItem.intCounts;
            for (int index = 0; index < _atCount; index++)
            {
                dataGridView1.Rows.Add();
                if (_items[index]._isAlarm == true)
                {
                    dataGridView1.Rows[index].Cells[intAlarm].Value = imageList1.Images[0];
                }
                else
                {
                    dataGridView1.Rows[index].Cells[intAlarm].Value = null;
                }
                for (int i = 0; i < _atCount; i++)
                {
                    if (!_items[i]._isFinish)
                    {
                        dataGridView1.Rows[index].Cells[intDetail].Value = _items[index]._detail;
                        dataGridView1.Rows[index].Cells[intEndTime].Value = _items[index]._endTime;
                        dataGridView1.Rows[index].Cells[intFinish].Value = _items[index]._isFinish;
                    }
                }
            }
            this.dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker1.ShowUpDown = true;
            dataGridView1.Columns[intAlarm].DefaultCellStyle.NullValue = null;
            dataGridView2.Columns[intNum].DefaultCellStyle.NullValue = null;
            dataGridView2.Rows[0].Cells[0].Value = null;
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
                    item._detail = dataGridView2.Rows[0].Cells[intAddDetail].Value.ToString();
                    item._endTime = dateTimePicker1.Value;
                    item._isFinish = false;
                    item._id = _atCount;
                    doItem.AddItem(item);
                    dataGridView1.Rows.Clear();
                    this.Form1_Load(sender, e);             
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
            for (int i = 0; i < dataGridView1.RowCount; i++)
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
                    //string time = DateTime.Parse(ts.ToString()).ToString("HH:mm:ss");
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

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //intCount = new int[_atCount];
            Item valueChange = new Item();
            //Item[] _items = doItem.SearchItem();
            //_atCount = _items.Length;
            //intCount = doItem.intCounts;
            if (e.ColumnIndex == 4 && e.RowIndex != -1)
            {
                valueChange = doItem.items[e.RowIndex];
                if (dataGridView1.Rows[e.RowIndex].Cells[intAlarm].Value != null)
                {
                    valueChange._isAlarm = true;
                }
                else
                {
                    valueChange._isAlarm = false;
                }
                
                //MessageBox.Show(e.RowIndex + "+事件" + dataGridView1.Rows[e.RowIndex].Cells[2].Value + "+时间" + dataGridView1.Rows[e.RowIndex].Cells[3].Value+"+image");
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
            Boolean flag = false;
            if (e.RowIndex != -1)
            {
                //获取控件的值
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[e.RowIndex].Cells[intFinish];
                flag = Convert.ToBoolean(checkCell.Value);
                if (e.RowIndex >= 0 && flag != true && dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.White)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 193);
                }
            }
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            Boolean flag = false;
            if (e.RowIndex != -1)
            {
                //获取控件的值
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[e.RowIndex].Cells[intFinish];
                flag = Convert.ToBoolean(checkCell.Value);

                if (e.RowIndex >= 0 && flag != true && dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.FromArgb(255, 255, 193))
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[intAlarm].Value == null)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[intAlarm].Value = imageList1.Images[0];
                    //Position[] _position = doItem.SearchItem();
                    Item[] _items = doItem.SearchItem();
                    _items[e.RowIndex]._isAlarm = true;
                    doItem.ChangeItem(_items[e.RowIndex], e.RowIndex);
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value = null;
                    //Position[] _position = doItem.SearchItem();
                    Item[] _items = doItem.SearchItem();
                    _items[e.RowIndex]._isAlarm = false;
                    doItem.ChangeItem(_items[e.RowIndex], e.RowIndex);
                }
            }

            
        }

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
            if (this.Visible == false)
            {
                this.WindowState = FormWindowState.Normal;
                this.Visible = true;
                this.Activate();
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
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
            Item valueChange = new Item();
            //intCount = doItem.intCounts;
            valueChange._detail = dataGridView1.Rows[e.RowIndex].Cells[intDetail].Value.ToString();
            valueChange._endTime = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[intEndTime].Value);
            doItem.ChangeItem(valueChange, valueChange._id);
        }
    }
}
