using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Core
{
	public class Waiter
	{
		private static Waiter _instance;
		private Queue<Order> _ordersOrdered = new Queue<Order>();
		private Queue<Order> _ordersServed = new Queue<Order>();
		private Queue<Order> _toPay = new Queue<Order>();
		public List<Article> _articles = new List<Article>();
		public static Waiter Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new Waiter();
				}
				return _instance;
			}
		}

		public event EventHandler<Order> NewOrder;

		private Waiter()
		{  //Hier meldet sich der Kelner als Beobachter der auf den Schnellen Uhr schaut
			FastClock.Instance.IsRunning = true;
			FastClock.Instance.OneMinuteIsOver += OnOneMinuteIsOverFromFastClock;
		}

		private void OnOneMinuteIsOverFromFastClock(object sender, DateTime fastClockTime)
		{
			for (int i = 0; i < _ordersOrdered.Count; i++)
			{
				if (fastClockTime.ToShortTimeString().Equals(_ordersOrdered.ElementAt(i).Delay))
				{
					if(_ordersOrdered.Count > 0)
					{
						Waiter_NewOrder(_ordersOrdered.ElementAt(i));
					}
				}
			}
			for (int i = 0; i < _ordersServed.Count; i++)
			{
				if (fastClockTime.ToShortTimeString().Equals(_ordersServed.ElementAt(i).Delay))
				{
					if(_ordersOrdered.Count > 0)
					{
						_ordersOrdered.Dequeue();
					}
					if(_ordersServed.Count > 0)
					{
						Waiter_NewOrder(_ordersServed.ElementAt(i));
					}
				}
			}
			for (int i = 0; i < _toPay.Count; i++)
			{
				if (fastClockTime.ToShortTimeString().Equals(_toPay.ElementAt(i).Delay))
				{
					if(_ordersServed.Count > 0)
					{
						_ordersServed.Dequeue();
					}
					if (_toPay.Count >= 0)
					{
						Waiter_NewOrder(_toPay.ElementAt(i));
					}
					if(_toPay.Count > 0)
					{
						_toPay.Dequeue();
					}
				}
			}
		}

		public void GetOrder(string pfad)
		{
			if(_ordersOrdered.Count == 0)
			{
				string[] lines = File.ReadAllLines(pfad);
				for (int i = 1; i < lines.Length; i++)
				{
					string[] line = lines[i].Split(';');
					double delayMinutes = double.Parse(line[0]);
					string displayDelayedMinutes = FastClock.Instance.Time.AddMinutes(delayMinutes).ToShortTimeString();
					Order order = new Order(displayDelayedMinutes, line[1], line[2], line[3]);
					_ordersOrdered.Enqueue(order);
				}
			}
			if (_articles.Count == 0)
			{
				string[] lines1 = File.ReadAllLines("Articles.csv");
				for (int i = 1; i < lines1.Length; i++)
				{
					string[] part = lines1[i].Split(';');
					Article article = new Article(part[0], double.Parse(part[1]), int.Parse(part[2]));
					_articles.Add(article);
				}
			}
			ManageOrder();
		}

		protected virtual void Waiter_NewOrder(Order order)
		{
			NewOrder?.Invoke(this, order);
		}

		public void ManageOrder()
		{
			foreach (Order item in _ordersOrdered)
			{
				double delayMinutes = double.Parse(item.Delay.Remove(0, 3)) + (double)item._article.TimeToBuild;
				string displayDelayedMinutes = FastClock.Instance.Time.AddMinutes(delayMinutes).ToShortTimeString();
				Order order = new Order(displayDelayedMinutes, item.Name, "Ready", item.ArticleParameter);
				_ordersServed.Enqueue(order);
			}
			ServeOrder();
		}

		private void ServeOrder()
		{
			foreach (Order item in _ordersOrdered)
			{
				if (item.OrderTypeParameter.Equals("ToPay"))
				{
					_toPay.Enqueue(item);
				}
			}
		}
	}
}
