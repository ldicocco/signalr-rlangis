using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using LocalWebAPI.Models;

namespace LocalWebAPI.Controllers
{
    public class PersonsController : ApiController
    {
        private static List<Person> _persons = LoadData();

//        private static string _currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private static List<Person> LoadData()
        {
            string _currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); 
            string path = _currentDir + "/data.txt";
            if (System.IO.File.Exists(path))
            {
                string content = System.IO.File.ReadAllText(path);
                var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(content);
                return items;
            }
            else
            {
                return new List<Person> {
							new Person(1, "Luciano", "Di Cocco", "Strindbergsvej 74A", "2500", "Valby", "Denmark"),
							new Person(2, "Mario", "Rossi", "Via Solferino 16", "57100", "Livorno", "Italy"),
						};
            }
        }

        private static void SaveData()
        {
            string _currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string res = Newtonsoft.Json.JsonConvert.SerializeObject(_persons, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(_currentDir + "/data.txt", res);
        }

        // GET api/values 
        public IEnumerable<object> Get()
        {
            return _persons;
        }

        // GET api/values/5 
        public Person Get(int id)
        {
            return _persons.SingleOrDefault(i => i.Id == id);
        }

        // POST api/values 
        public void Post([FromBody]Person value)
        {
            if (value.Id <= 0)
            {
                int newId = _persons.Max(i => i.Id) + 1;
                var item = new Person(newId, value.FirstName, value.LastName, value.Address,
                    value.ZipCode, value.City, value.Country);
                _persons.Add(item);
            }
            else
            {
                var item = _persons.SingleOrDefault(i => i.Id == value.Id);
                if (item != null)
                {
                    item.FirstName = value.FirstName;
                    item.LastName = value.LastName;
                }
            }

            SaveData();
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]Person value)
        {
            var item = _persons.SingleOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.FirstName = value.FirstName;
                item.LastName = value.LastName;
            }
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
            var item = _persons.SingleOrDefault(i => i.Id == id);
            if (item != null)
            {
                _persons.Remove(item);
            }
        }
    }
}
