using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HelloXamarin
{
    public partial class MainPage : ContentPage
    {
        static readonly HttpClient client = new HttpClient();
        public MainPage()
        {
            InitializeComponent();

        }
        public async void GetSwapiPeople()
        {
            string text = await client.GetStringAsync("https://swapi.dev/api/people");
            JsonDocument doc = JsonDocument.Parse(text);
            JsonElement root = doc.RootElement;
            JsonElement numberOfPeople = root.GetProperty("count");
            //Console.WriteLine(numberOfPeople);
            //Console.WriteLine(numberOfPeople.ToString());
            int intNumberOfPeople = int.Parse(numberOfPeople.ToString());

            PersonsListView.ItemsSource = persons;
            for (int i = 1; i < intNumberOfPeople; i++)
            {
                try
                {
                    string person = await client.GetStringAsync("https://swapi.dev/api/people/" + i);
                    JsonDocument personDoc = JsonDocument.Parse(person);
                    JsonElement personRoot = personDoc.RootElement;
                    JsonElement personName = personRoot.GetProperty("name");
                    JsonElement personHeight = personRoot.GetProperty("height");
                    JsonElement personMass = personRoot.GetProperty("mass");
                    Person personObj = new Person() { Name = Convert.ToString(personName), Height = Convert.ToString(personHeight), Mass = Convert.ToString(personMass) };

                    persons.Add(personObj);

                }
                catch (Exception)
                {

                    continue;
                }

            }

        }

        ObservableCollection<Person> persons = new ObservableCollection<Person>();
        public ObservableCollection<Person> Persons { get { return persons; } }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Test");
            GetSwapiPeople();
        }

        public class Person
        {
            public string Name { get; set; }
            public string Height { get; set; }
            public string Mass { get; set; }
        }

    }
}
