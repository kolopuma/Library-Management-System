using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    //  Объект лектора, который наследует LibraryPatron и, в свою очередь, Человека
    public class Lecturer:LibraryPatron
    {
        //  Частные переменные
        private string id;
        private decimal salary;

        //  Аксессоры и мутаторы
        public string ID { get => id; private set => id = value; }
        public decimal Salary { get => salary; private set => salary = value; }

        //  Конструктор(ы)
        public Lecturer(string id, decimal salary, string ppsn, string name, string address, string phone, string email) : base(ppsn, name, address, phone, email)
        {
            ID = id;
            Salary = salary;
        }

        public override bool ReachedBookLimit()
        {
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //  переопределение(ы)
        public override string ToString()
        {
            return base.ToString() + "[" + id + "]";
        }
    }
}
