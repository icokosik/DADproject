using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADstorm
{
    public class Tuple
    {
        private List<Object> items;

        public Tuple(List<Object> items)
        {
            this.items = items;
        }

        public List<Object> getItems()
        {
            return items;
        }

        public int getSize()
        {
            return items.Count();
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
