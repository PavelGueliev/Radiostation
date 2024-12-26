using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radiostation
{
    public class Presenter
    {
        public int КодВедущего { get; set; }
        public string ФИО { get; set; }
        public string НомерТелефона { get; set; }
        public DateTime ДатаРождения { get; set; }

        // Это поле будет загружаться и сохраняться в Users_radiostation.
        public string Пароль { get; set; }
    }
}
