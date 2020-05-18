using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class Note
    {
        public int Id { get; set; }
        
        public string text { get; set; }

        #region constructors
        public Note()
        {
        }

        public Note(string text)
        {
            this.text = text;
        }
        #endregion constructors
    }
}
