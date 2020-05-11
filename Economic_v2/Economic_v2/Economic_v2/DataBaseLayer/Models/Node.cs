using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class Node
    {
        public int Id { get; set; }
        
        public string text { get; set; }

        #region constructors
        public Node()
        {
        }

        public Node(string text)
        {
            this.text = text;
        }
        #endregion constructors
    }
}
