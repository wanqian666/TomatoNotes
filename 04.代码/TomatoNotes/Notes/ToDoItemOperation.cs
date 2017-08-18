using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes
{
    class ToDoItemOperation
    {
        ToDoItemFile itemFile = new ToDoItemFile();
        public Item[] items;
        public Item[] items_self;
        //public Item[] items_show;
        //public int[] intCounts;
        public void AddItem(Item newItem)
        {
            int count = items.Length;
            items_self = new Item[count + 1];
            items.CopyTo(items_self, 0);
            items_self[count] = newItem;
            items = items_self;
            itemFile.WriteXml(items[count]);
        }

        public Item DeleteItem(Item newItem)
        {
            Item item = new Item();
            item._detail = newItem._detail;
            item._endTime = newItem._endTime;
            item._isAlarm = newItem._isAlarm;
            item._isFinish = newItem._isFinish;
            return newItem;
        }

        public void ChangeItem(Item newItem, int num)
        {
            items[num] = newItem;
            itemFile.UpdateXml(items);
        }

        public Item[] SearchItem()
        {
            items = itemFile.GetXml();

            if (items == null)
            {
                items[0]._isAlarm = false;
                items[0]._detail = null;
                items[0]._id = 0;
                items[0]._isFinish = false;
                items[0]._endTime = Convert.ToDateTime(2000/01/01);
            }
            return items;
        }

    }
}
