using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Core
{
	public class Order
	{
		private string _delay;
		private string _name;
		private string _orderTypeParameter;
		private string _articleParameter;
		private Guest _guest;
		public Article _article;
		public List<Article> _articles = new List<Article>();
		public Order(string delay, string name, string orderTypeParameter, string articleParameter)
		{
			Delay = delay;
			Name = name;
			_guest = new Guest(Name);
			OrderTypeParameter = orderTypeParameter;
			ArticleParameter = articleParameter;
			if (_articles.Count == 0)
			{
				string[] lines1 = File.ReadAllLines("Articles.csv");
				for (int i = 1; i < lines1.Length; i++)
				{
					string[] part = lines1[i].Split(';');
					Article article = new Article(part[0], double.Parse(part[1]), int.Parse(part[2]));
					_articles.Add(article);
					article = null;
				}
			}
			double mply = 0;
			int time = 0;
			for (int i = 0; i < _articles.Count; i++)
			{
				if (_articles[i].ArticleName.Equals(ArticleParameter))
				{
					mply = _articles[i].ArticlePreis;
					time = _articles[i].TimeToBuild;
					break;
				}
			}
			_article = new Article(ArticleParameter, mply, time);
		}

		public string Delay { get => _delay; set => _delay = value; }
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		public string OrderTypeParameter 
		{
			get => _orderTypeParameter;
			set => _orderTypeParameter = value; 
		}
		public string ArticleParameter { get => _articleParameter; set => _articleParameter = value; }

		public override string ToString()
		{
			if (OrderTypeParameter.Equals(OrderType.Order.ToString()))
			{
				return $"{Delay} {_article.ArticleName} für {_guest.Name} ist bestellt.";
			}
			else if (OrderTypeParameter.Equals(OrderType.Ready.ToString()))
			{
				return $"{Delay} {_article.ArticleName} für {_guest.Name} wird serviert.";
			}
			else if (OrderTypeParameter.Equals(OrderType.ToPay.ToString()))
			{
				return $"{Delay} {_guest.Name} bezahlt {new Article(_article.ArticleName, _article.ArticlePreis, _article.TimeToBuild).ArticlePreis} Euro.";
			}
			else return "Keine Gäste mehr...";
		}
	}
}
