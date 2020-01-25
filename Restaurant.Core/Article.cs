using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Core
{
	public class Article
	{
		private string _articleName;
		private double _articlePreis;
		private int _timeToBuild;
		public Article(string articleName, double articlePreis, int timeToBuild)
		{
			ArticleName = articleName;
			ArticlePreis = articlePreis;
			TimeToBuild = timeToBuild;
		}
		public string ArticleName { get => _articleName; set => _articleName = value; }
		public double ArticlePreis { get => _articlePreis; set => _articlePreis = value; }
		public int TimeToBuild { get => _timeToBuild; set => _timeToBuild = value; }
	}
}
