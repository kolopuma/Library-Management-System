using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    //  Основная точка входа в программу
    //  Паттерн синглтона, так как должен быть создан только 1 экземпляр колледжа
    public class College: ICollection<Person>
    {
        //  Переменные тестирования; установите значение false и перестройте, чтобы не загружать тестовые данные и использовать период кредитования, поддерживаемый библиотекой
        public const bool DebugMode = true;
        public enum DebugTimeBetweenMillis { Min=10000, Max=25000 }

        //  Запускает "Форму ввода", отправную точку графического интерфейса/программы
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
        
        //  Приватные поля
        private static College instance;
        private List<Person> people;
        private Library library;
        
        //internal List<Person> People { get => people; }
        public Library Library { get => library; }

        private College()
        {
            if (DebugMode)
            {
                people = JsonHelper.GetPeople();
                library = new Library(JsonHelper.GetInventory());
            }
            else
            {
                people = new List<Person>();
                library = new Library();
            }
        }

        //  public singelton instance accessor
        public static College Instance
        {
            get
            {
                if (instance == null)
                    instance = new College();
                return instance;
            }
        }

        public int Count => ((ICollection<Person>)people).Count;

        public bool IsReadOnly => ((ICollection<Person>)people).IsReadOnly;

        //  добавление нового человека в список людей в системе
        public bool AddPerson(Person p)
        {
            if (!Contains(p))
            {
                Add(p);
                return true;
            }
            return false;
        }

        //  попробуйте удалить человека из системы
        public bool RemovePerson(Person p)
        {
            return people.Remove(p);
        }

        //  проверьте, находится ли человек в системе по ppsn
        public bool PersonInSystem(string ppsn)
        {
            return people.Exists(x => x.PPSN == ppsn);
        }

        //  проверьте, есть ли человек в системе
        public bool PersonInSystem(Person p)
        {
            if(p != null)
                return people.Contains(p);
            return false;
        }

        public List<Person> GetPeople(string search)
        {
            return people.FindAll(x => x.Name.ToLower().Contains(search.ToLower()));
        }

        public List<Person> GetStudents()
        {
            return people.FindAll(x => x is Student);
        }

        public List<Person> GetStudents(string search)
        {
            return people.FindAll(x => x is Student && x.Name.ToLower().Contains(search.ToLower()));
        }

        public List<Person> GetLecturers()
        {
            return people.FindAll(x => x is Lecturer);
        }

        public List<Person> GetLecturers(string search)
        {
            return people.FindAll(x => x is Lecturer && x.Name.ToLower().Contains(search.ToLower()));
        }

        //  вернуть человека через ppsn
        public Person GetPerson(string ppsn)
        {
            return people.Find(x => x.PPSN == ppsn);
        }

        public void Add(Person item)
        {
            if (!people.Contains(item))
                people.Add(item);
        }

        public void Clear()
        {
            ((ICollection<Person>)people).Clear();
        }

        public bool Contains(Person item)
        {
            return ((ICollection<Person>)people).Contains(item);
        }

        public void CopyTo(Person[] array, int arrayIndex)
        {
            ((ICollection<Person>)people).CopyTo(array, arrayIndex);
        }

        public bool Remove(Person item)
        {
            return ((ICollection<Person>)people).Remove(item);
        }

        public IEnumerator<Person> GetEnumerator()
        {
            return ((ICollection<Person>)people).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<Person>)people).GetEnumerator();
        }
    }
}
