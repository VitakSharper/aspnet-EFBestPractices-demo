using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace aspnet_EFBestPractices.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly PeopleContext _peopleContext;

        public IndexModel(ILogger<IndexModel> logger, PeopleContext peopleContext)
        {
            _logger = logger;
            _peopleContext = peopleContext;
        }

        public void OnGet()
        {
            LoadSampleData();
            var age = ApprovedAge(35);



            var peopleList2 = _peopleContext.People
                .Include(a => a.Addresses)
                .Include(m => m.Emails)
                .Where(p => p.Age > 18 && p.Age < 65)
                .ToList();

        }


        private static bool ApprovedAge(int age) =>
        age > new Age(18) && age < new Age(65);

        private void LoadSampleData()
        {
            if (!_peopleContext.People.Any())
            {
                var file = System.IO.File.ReadAllText("generated.json");
                var people = JsonSerializer.Deserialize<List<Person>>(file);
                _peopleContext.AddRange(people);
                _peopleContext.SaveChanges();
            }
        }
    }

    public struct Age
    {
        private int PersonAge { get; }
        public static bool operator <(int l, Age r) => new Age(l).PersonAge < r.PersonAge;
        public static bool operator >(int l, Age r) => new Age(l).PersonAge > r.PersonAge;
        public Age(int age)
        {
            if (!IsValid(age))
                throw new ArgumentException($"Value {age} is not a valid age.");
            PersonAge = age;
        }

        private static bool IsValid(in int age) =>
           0 < age && age < 120;
    }
}