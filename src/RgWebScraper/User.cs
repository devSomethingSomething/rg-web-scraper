using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgWebScraper
{
    public class User
    {
        public string NameAndSurname { get; set; } = "John Doe";

        public float Score { get; set; } = 0.0f;

        public int HIndex { get; set; } = 0;

        public int HIndexExcluding { get; set; } = 0;

        public User()
        {

        }
    }
}
