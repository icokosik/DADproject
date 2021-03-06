﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class Tuple
    {
        public static Tuple EMPTY = new Tuple(new List<string>());

        private List<string> items;

        public Tuple(List<string> items)
        {
            this.items = items;   
        }
    
        public List<string> getItems()
        {
            return items;
        }

        public int getSize()
        {
            return items.Count();
        }

        public string get(int index)
        {
            return items[index];
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach(Object obj in items)
            {
                builder.Append(obj.ToString()).Append(", ");
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            var that = obj as Tuple;
            if (that == null) return false;
            if (this.getSize() != that.getSize()) return false;
            for(int i=0;i<this.getSize();i++)
            {
                if (!this.getItems()[i].Equals(that.getItems()[i])) return false;
            }
            return true;
        }
    }
}
