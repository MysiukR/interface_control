using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Control
{
    class Register
    {
        public DateTime date;
        public double Device_number;
        public double Register_number;
        public double Value_register;

        public Register(DateTime date, double Device_number, double Register_number, double Value_register)
        {
            this.date = date;
            this.Device_number = Device_number;
            this.Register_number = Register_number;
            this.Value_register = Value_register;
        }
    }
}
