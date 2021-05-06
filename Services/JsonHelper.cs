using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System;

namespace LibraryManagementSystem
{
    //  json парсер для загрузки тестовых данных
    public static class JsonHelper
    {
        //public void JsonSerialize(object data, string filePath)
        //{
        //    JsonSerializer jsonSerializer = new JsonSerializer();
        //    if (File.Exists(filePath)) File.Delete(filePath);
        //    StreamWriter sw = new StreamWriter(filePath);
        //    JsonWriter jsonWriter = new JsonTextWriter(sw);
        //    jsonSerializer.Serialize(jsonWriter, data);
        //    jsonWriter.Close();
        //    sw.Close();
        //}
        //public object JsonDeserialize(Type dataType,string filePath)
        //{
        //    JObject obj = null;
        //    JsonSerializer jsonSerializer = new JsonSerializer();
        //    if (File.Exists(filePath))
        //    {
        //        StreamReader sr = new StreamReader(filePath);
        //        JsonReader jsonReader = new JsonTextReader(sr);
        //        obj = jsonSerializer.Deserialize(jsonReader) as JObject;
        //        jsonReader.Close();
        //        sr.Close();              
        //    }
        //    return obj.ToObject(dataType);
        //}
        //  Возвращает список типа "Person" людей в системе колледжей/библиотек ( из json )
        public static List<Person> GetPeople()
        {
            //  Парсинг файлов "lecturers.json" и "students.json".
            List<LecturerJsonObject> lecturerJsonObjects = JsonParser<LecturerJsonObject>.JsonToList(Properties.Resources.Lecturers);
            List<StudentJsonObject> studentJsonObjects = JsonParser<StudentJsonObject>.JsonToList(Properties.Resources.Students);

            //  Преобразование проанализированных данных в объекты Лектора и студента
            List<Person> people = new List<Person>();
            foreach(LecturerJsonObject person in lecturerJsonObjects)
                people.Add(new Lecturer(person.id,person.salary,person.ppsn,person.name,person.address,person.phone,person.email));
            foreach (StudentJsonObject student in studentJsonObjects)
                people.Add(new Student(student.id, student.status, student.ppsn, student.name, student.address, student.phone, student.email));


            return people;
        }

        //  Возвращает отсортированный список книг и их количество в инвентаре библиотеки
        public static List<Book> GetInventory()
        {
            //удаление дубликатов
            List<Book> inventory = JsonParser<Book>.JsonToList(Properties.Resources.books);
            HashSet<string> titles = new HashSet<string>();
            List<Book> tmp = new List<Book>();
            foreach (Book b in inventory)
                if (!titles.Add(b.Title))
                    tmp.Add(b);
            foreach (Book b in tmp)
                inventory.Remove(b);

            return inventory;
        }

        //  Book doesn't inherit from anything, so serialization is simple
        //private struct BookJsonObject
        //{
        //    public string ISBN { get; set; }
        //    public string Title { get; set; }
        //    public string Author { get; set; }
        //    public string Publisher { get; set; }
        //    public string Category { get; set; }
        //    public string About { get; set; }
        //    public int Quantity { get; set; }
        //}

        //  Because Lecturer and Student inherit from Person, serialization is a struggle
        //  simplest solution I figured was to make tmp data holders, which I then transfer 
        //  to my classes.
        private struct LecturerJsonObject
        {
            public string id { get; set; }
            public decimal salary { get; set; }
            public string ppsn { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
        }

        private struct StudentJsonObject
        {
            public string id { get; set; }
            public GradStatus status { get; set; }
            public string ppsn { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
        }

        //  Generic парсер, без создания большого числа перегруженных методов
        private static class JsonParser<T>
        {
            public static List<T> JsonToList(byte[] jsonBytes)
            {
                string json = Encoding.ASCII.GetString(jsonBytes);
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
        }
    }
}
