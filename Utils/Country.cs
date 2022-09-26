using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MUNManager.Utils {
	public class Country {
		//public Country(string name, string code, Image flag, bool member = true, bool veto = false)
		public Country(string name, string code, bool veto = false, bool member = true)
		{
			Name = name;
			Code = code;
			// Flag = flag;

			IsMember = member;
			IsVeto = veto;
		}

		/*
		 * Database: https://gist.githubusercontent.com/keeguon/2310008/raw/bdc2ce1c1e3f28f9cab5b4393c7549f38361be4e/countries.json
		 * I couldn't find a database of UN Member states. If you know of one or would like to create one, please let me know.
		 */
		/// <summary>
		/// Returns an array of all countries
		/// </summary>
		/// <param name="membersOnly"></param>
		/// <exception cref="InvalidOperationException">Hard fail if database file is not present.</exception>
		public static List<Country?> All(bool membersOnly = true)
		{
			var countries = new List<Country?>();
			
			dynamic countryList = JsonConvert.DeserializeObject(File.ReadAllText("./Resources/countries.json")) ?? throw new InvalidOperationException();
			foreach (var country in countryList)
			{
				bool isVeto = country.veto != null && bool.Parse((string)country.veto);
				bool isMember = country.member == null || bool.Parse((string)country.member);
				if (membersOnly && !isMember) continue;

				var c = new Country((string)country.name, (string)country.code, isVeto, isMember);
				countries.Add(c);
			}
			return countries;
		}
		public static List<string> AllString(bool membersOnly = true)
		{
			var countries = new List<string>();
			
			dynamic countryList = JsonConvert.DeserializeObject(File.ReadAllText("./Resources/countries.json")) ?? throw new InvalidOperationException();
			foreach (var country in countryList)
			{
				bool isVeto = country.veto != null && bool.Parse((string)country.veto);
				bool isMember = country.member == null || bool.Parse((string)country.member);
				if (membersOnly && !isMember) continue;

				var c = new Country((string)country.name, (string)country.code, isVeto, isMember);
				countries.Add(c.Name);
			}
			return countries;
		}

		public static Country? FindByName(string name)
		{
			return All().Find(c => c.Name == name);
		}

		public static string FindByNameStr(string name)
		{
			return All().Find(c => c.Name == name).Name;
		}

		public static IEnumerable<Country?> AllArray(bool memberOnly = true) { return All(memberOnly).ToArray(); }
		public string Name { get; }
		public string Code { get; }
		//public Image Flag { get; }
		
		public override string ToString()
		{
			return Name;
		}
		
		public bool IsVeto { get; }
		
		public bool IsMember { get; }
	}
}