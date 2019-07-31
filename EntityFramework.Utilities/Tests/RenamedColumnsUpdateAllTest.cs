﻿using System;
using System.Data.Entity;
using System.Linq;
using EntityFramework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.FakeDomain;
using Tests.Models;

namespace Tests
{
	[TestClass]
	public class RenamedColumnsUpdateAllTest
	{
		[TestMethod]
		public void UpdateAll_SetDateTimeValueFromVariable_RenamedColumn()
		{
			RenamedAndReorderedContext.SetupTestDb();
			using (var db = new RenamedAndReorderedContext())
			{
				db.BlogPosts.Add(new RenamedAndReorderedBlogPost { Title = "T1", Created = new DateTime(2013, 01, 01) });
				db.BlogPosts.Add(new RenamedAndReorderedBlogPost { Title = "T2", Created = new DateTime(2013, 02, 01) });
				db.BlogPosts.Add(new RenamedAndReorderedBlogPost { Title = "T3", Created = new DateTime(2013, 03, 01) });

				db.SaveChanges();
			}

			using (var db = new RenamedAndReorderedContext())
			{
				var count = EFBatchOperation.For(db, db.BlogPosts).Where(b => b.Title == "T2").Update(b => b.Created, b => DateTime.Today);
				Assert.AreEqual(1, count);
			}

			using (var db = new RenamedAndReorderedContext())
			{
				var post = db.BlogPosts.First(p => p.Title == "T2");
				Assert.AreEqual(DateTime.Today, post.Created);
			}
		}

		[TestMethod]
		public void UpdateAll_IncrementDateTime_RenamedColumn()
		{
			RenamedAndReorderedContext.SetupTestDb();
			using (var db = new RenamedAndReorderedContext())
			{
				db.BlogPosts.Add(new RenamedAndReorderedBlogPost { Title = "T1", Created = new DateTime(2013, 01, 01) });
				db.BlogPosts.Add(new RenamedAndReorderedBlogPost { Title = "T2", Created = new DateTime(2013, 02, 01) });
				db.BlogPosts.Add(new RenamedAndReorderedBlogPost { Title = "T3", Created = new DateTime(2013, 03, 01) });

				db.SaveChanges();
			}

			using (var db = new RenamedAndReorderedContext())
			{
				var count = EFBatchOperation.For(db, db.BlogPosts).Where(b => b.Title == "T2").Update(b => b.Created, b => DbFunctions.AddDays(b.Created, 1));
				Assert.AreEqual(1, count);
			}

			using (var db = new RenamedAndReorderedContext())
			{
				var post = db.BlogPosts.First(p => p.Title == "T2");
				Assert.AreEqual(new DateTime(2013, 02, 02), post.Created);
			}
		}

		[TestMethod]
		public void UpdateAll_IncrementIntValue_RenamedColumn()
		{
			RenamedAndReorderedContext.SetupTestDb();
			using (var db = new RenamedAndReorderedContext())
			{
				db.BlogPosts.Add(new RenamedAndReorderedBlogPost { Title = "T1", Created = new DateTime(2013, 01, 01) });
				db.BlogPosts.Add(new RenamedAndReorderedBlogPost { Title = "T2", Created = new DateTime(2013, 02, 01), Reads = 10 });
				db.BlogPosts.Add(new RenamedAndReorderedBlogPost { Title = "T3", Created = new DateTime(2013, 03, 01) });

				db.SaveChanges();
			}

			using (var db = new RenamedAndReorderedContext())
			{
				var count = EFBatchOperation.For(db, db.BlogPosts).Where(b => b.Title == "T2").Update(b => b.Reads, b => b.Reads + 100);
				Assert.AreEqual(1, count);
			}

			using (var db = new RenamedAndReorderedContext())
			{
				var post = db.BlogPosts.First(p => p.Title == "T2");
				Assert.AreEqual(110, post.Reads);
			}
		}
	}
}
