using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem
{
    //  enum используется для представления статуса студента, и у двойников есть лимит кредита на книгу
    public enum GradStatus { Undergrad = 5, Postgrad = 10 }

    //  Студент, который наследует LibraryPatron, и, в свою очередь, Person
    public class Student : LibraryPatron
    {
        //  Приватные поля
        private string id;
        private GradStatus status;

        //  Аксессоры и мутаторы
        public string ID { get => id; private set => id = value; }
        public GradStatus Status { get => status; set => status = value; }

        //  конструктор(ы)
        public Student(string id, GradStatus status, string ppsn, string name, string address, string phone, string email) : base(ppsn, name, address, phone, email)
        {
            ID = id;
            Status = status;
        }

        //  абстрактные реализации

        //  Студент может одолжить книгу в пределах своей ограниченной суммы, если у него нет просроченной задолженности
        public override bool ReachedBookLimit()
        {
            return (Count >= (int)status);
        }

        //  переопределенные методы объектов
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() + "[" + id + "]";
        }
    }
}
