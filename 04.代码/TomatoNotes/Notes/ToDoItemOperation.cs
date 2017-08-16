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
            //int num = 0, num2 = 0;
            items = itemFile.GetXml();
            //for (int i = 0; i < items.Length; i++)
            //{
            //    if (!items[i]._isFinish)
            //    {
            //        num++;
            //    }
            //}
            //items_show = new Item[num];
            //intCounts = new int[num];
            //for (int j = 0; j < num; j++)
            //{
            //    for (int i = num2; i < items.Length; i++)
            //    {
            //        if (!items[i]._isFinish)
            //        {
            //            items_show[j] = items[i];

            //            //positions[j]._position = num2;
            //            //positions[j]._item = new1;
            //            intCounts[j] = num2;
            //            num2 = i+1;    
            //            break;
            //        }
            //    }
            //}
            return items;
        }
    }
}
