using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgWebScraper
{
    public class User
    {
        public string NameAndSurname { get; set; }

        public float Score { get; set; }

        public int HIndex { get; set; }

        public int HIndexExcluding { get; set; }

        public User()
        {

        }
    }
}
